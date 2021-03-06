﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using ExcelLibrary.SpreadSheet;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

using System.Collections.Specialized;
//using System.Configuration.Assemblies;

using PollingLibraries.LibLogger;
using Drivers.LibMeter;
using Drivers.KaratDanfosDriver;
using PollingLibraries.LibPorts;

namespace elfextendedapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = FORM_TEXT_DEFAULT;

            DeveloperMode = true;
            if (DeveloperMode) this.Height -= groupBox1.Height;

            InProgress = false;

            InputDataReady = false;

            //byte[] tmp = { 0xA0, 0x05, 0x00, 0x00 };
            ////Array.Reverse(tmp);
            //float val = BitConverter.ToInt32(tmp, 0);
            //MessageBox.Show(val.ToString());

        }


        private string passwordDefault = "111111";

        //при опросе или тесте связи
        bool bInProcess = false;
        public bool InProgress
        {
            get { return bInProcess; }
            set
            {
                bInProcess = value;

                if (bInProcess)
                {
                    toolStripProgressBar1.Value = 0;

                    comboBoxComPorts.Enabled = false;
                    buttonPoll.Enabled = false;
                    buttonPing.Enabled = false;
                    buttonImport.Enabled = false;
                    label1.Enabled = false;
                    buttonExport.Enabled = false;
                    buttonStop.Enabled = true;
                    numericUpDownComReadTimeout.Enabled = false;
                    checkBoxPollOffline.Enabled = false;

                    this.Text += FORM_TEXT_INPROCESS;
                }
                else
                {
                   // comboBoxComPorts.Enabled = true;
                    buttonPoll.Enabled = true;
                    buttonPing.Enabled = true;
                    buttonImport.Enabled = true;
                    buttonExport.Enabled = true;
                    label1.Enabled = true;
                    buttonStop.Enabled = false;
                  //  numericUpDownComReadTimeout.Enabled = true;
                    checkBoxPollOffline.Enabled = true;
                    dgv1.Enabled = true;

                    this.Text = this.Text.Replace(FORM_TEXT_INPROCESS, String.Empty);
                }
            }
        }

        bool bInputDataReady = false;
        public bool InputDataReady
        {
            get { return bInputDataReady; }
            set
            {
                bInputDataReady = value;

                if (!bInputDataReady)
                {
                    toolStripProgressBar1.Value = 0;

                    comboBoxComPorts.Enabled = false;
                    buttonPoll.Enabled = false;
                    buttonPing.Enabled = false;
                    buttonImport.Enabled = true;
                    buttonExport.Enabled = false;
                    label1.Enabled = false;
                    buttonStop.Enabled = false;
                   // numericUpDownComReadTimeout.Enabled = false;
                    checkBoxPollOffline.Enabled = true;
                }
                else
                {
                    //comboBoxComPorts.Enabled = true;
                    buttonPoll.Enabled = true;
                    //if (!PredefineImpulseInitialsMode) buttonPing.Enabled = true;
                    buttonImport.Enabled = true;
                    buttonExport.Enabled = true;
                    buttonStop.Enabled = false;
                    buttonPing.Enabled = true;
                   // numericUpDownComReadTimeout.Enabled = true;
                   // if (!PredefineImpulseInitialsMode) checkBoxPollOffline.Enabled = true;
                    label1.Enabled = true;
                }
            }
        }

        #region Строковые постоянные 

            const string METER_IS_ONLINE = "ОК";
            const string METER_IS_OFFLINE = "Нет связи";
            const string METER_WAIT = "Ждите";
            const string REPEAT_REQUEST = "Повтор";

            const string FORM_TEXT_DEFAULT = "Карат-Данфос Modbus - группа v.0.1";
            const string FORM_TEXT_DEMO_OFF = " - демо режим ОТКЛЮЧЕН";
            const string FORM_TEXT_DEV_ON = " - режим разработчика";

            const string FORM_TEXT_INPROCESS = " - в процессе";

        #endregion

        KaratDanfosDriver Meter = null;
        VirtualPort Vp = null;

        //изначально ни один процесс не выполняется, все остановлены
        volatile bool doStopProcess = false;
        bool bPollOnlyOffline = false;

        //default settings for input *.xls file
        int flatNumberColumnIndex = 0;
        int factoryNumberColumnIndex = 4;
        int firstRowIndex = 2;
        
        //предустановка значений
        int colIPIndex = 6;
        int colPortIndex = 7;
        int colChannelIndex = 3;
        int colMeterNameIndex = 1;
        int colPredValue = 8;


        private bool initMeterDriver(uint mAddr, string mPass, VirtualPort virtPort)
        {
            if (virtPort == null) return false;

            try
            {
                Meter = new KaratDanfosDriver();
                Meter.Init(mAddr, mPass, virtPort);
                return true;
            }
            catch (Exception ex)
            {
                WriteToStatus("Ошибка инициализации драйвера: " + ex.Message);
                return false;
            }
        }

        private bool refreshSerialPortComboBox()
        {
            try
            {
                string[] portNamesArr = SerialPort.GetPortNames();
                comboBoxComPorts.Items.AddRange(portNamesArr);
                if (comboBoxComPorts.Items.Count > 0)
                {
                    int startIndex = 0;
                    comboBoxComPorts.SelectedIndex = startIndex;
                    return true;
                }
                else
                {
                    // WriteToStatus("В системе не найдены доступные COM порты");
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteToStatus("Ошибка при обновлении списка доступных COM портов: " + ex.Message);
                return false;
            }
        }

        private bool setVirtualSerialPort()
        {
            try
            {
                byte attempts = 1;
                ushort read_timeout = (ushort)numericUpDownComReadTimeout.Value;
                ushort write_timeout = (ushort)numericUpDownComWriteTimeout.Value;

                if (!checkBoxTcp.Checked)
                {
                    SerialPort m_Port = new SerialPort(comboBoxComPorts.Items[comboBoxComPorts.SelectedIndex].ToString());

                    m_Port.BaudRate = int.Parse(ConfigurationSettings.AppSettings["baudrate"]);
                    m_Port.DataBits = int.Parse(ConfigurationSettings.AppSettings["databits"]);
                    m_Port.Parity = (Parity)int.Parse(ConfigurationSettings.AppSettings["parity"]);
                    m_Port.StopBits = (StopBits)int.Parse(ConfigurationSettings.AppSettings["stopbits"]);
                    m_Port.DtrEnable = bool.Parse(ConfigurationSettings.AppSettings["dtr"]);

                    //meters initialized by secondary id (factory n) respond to 0xFD primary addr
                    Vp = new ComPort(m_Port, attempts, read_timeout, write_timeout);
                }
                else
                {
                    //TODO: сделать это подсосом из xml
                    NameValueCollection loadedAppSettings = new NameValueCollection();
                    loadedAppSettings.Add("localEndPointIp", this.listBox1.SelectedItem.ToString());

                    Vp = new TcpipPort(textBoxIp.Text, int.Parse(textBoxPort.Text), write_timeout, read_timeout, 50, loadedAppSettings);
                }



                uint mAddr = 0xFD;
                string mPass = "";

                if (!initMeterDriver(mAddr, mPass, Vp)) return false;

                //check vp settings
                if (!checkBoxTcp.Checked)
                {
                    SerialPort tmpSP = (SerialPort)Vp.GetPortObject();
                        toolStripStatusLabel2.Text = String.Format("{0}-{1}-{2}-DTR({3})-RTimeout: {4}ms", tmpSP.PortName, tmpSP.BaudRate, tmpSP.Parity, tmpSP.DtrEnable, read_timeout);
               
                }
                else
                {
                    toolStripStatusLabel2.Text = "TCP mode";
                }
               

                return true;
            }
            catch (Exception ex)
            {
                WriteToStatus("Ошибка создания виртуального порта: " + ex.Message);
                return false;
            }
        }

        private bool setXlsParser()
        {
            try
            {
                flatNumberColumnIndex = int.Parse(ConfigurationSettings.AppSettings["flatColumn"]) - 1;
                factoryNumberColumnIndex = int.Parse(ConfigurationSettings.AppSettings["factoryColumn"]) - 1;
                firstRowIndex = int.Parse(ConfigurationSettings.AppSettings["firstRow"]) - 1;
                //предустановка значений
                colIPIndex = int.Parse(ConfigurationSettings.AppSettings["colIPIndex"]) - 1;
                colPortIndex = int.Parse(ConfigurationSettings.AppSettings["colPortIndex"]) - 1;
                colChannelIndex = int.Parse(ConfigurationSettings.AppSettings["colChannelIndex"]) - 1;
                colMeterNameIndex = int.Parse(ConfigurationSettings.AppSettings["colMeterNameIndex"]) - 1;
                colPredValue = int.Parse(ConfigurationSettings.AppSettings["colPredValue"]) - 1;

                return true;
            }
            catch (Exception ex)
            {
                WriteToStatus("Ошибка разбора блока \"Настройка парсера\" в файле конфигурации: " + ex.Message);
                return false;
            }

        }

        private void WriteToStatus(string str)
        {
            MessageBox.Show(str, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } 

        private void Form1_Load(object sender, EventArgs e)
        {

            //byte[] arr = { 0x0, 0x00, 0x00, 0x01, 0x81, 0xAA, 0x3F, 0x50 };
            //byte[] arr2 = { 0x0, 0x01, 0x21, 0x99, 0x68, 0xA4, 0x03, 0xBE };

            //Array.Reverse(arr);
            //Array.Reverse(arr2);

            //double b = Math.Round(BitConverter.ToInt64(arr, 0) * Math.Pow(10, -9), 4, MidpointRounding.AwayFromZero);
            //double d = Math.Round(BitConverter.ToInt64(arr2, 0) * Math.Pow(10, -12), 4, MidpointRounding.AwayFromZero); 


            //setting up dialogs
            ofd1.Filter = "Excel files (*.xls) | *.xls";
            sfd1.Filter = ofd1.Filter;
            ofd1.FileName = "FactoryNumbersTable";
            sfd1.FileName = ofd1.FileName;

            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    this.listBox1.Items.Add(ip.ToString());
                }
            }

            this.listBox1.SelectedIndex = 0;

            checkBoxTcp.Checked = true;

            refreshSerialPortComboBox();
            setVirtualSerialPort();
            setXlsParser();

            cbJustRead.Checked = true;


            //привязываются здесь, чтобы можно было выше задать значения без вызова обработчиков
            comboBoxComPorts.SelectedIndexChanged += new EventHandler(comboBoxComPorts_SelectedIndexChanged);
            numericUpDownComReadTimeout.ValueChanged +=new EventHandler(numericUpDownComReadTimeout_ValueChanged);
            numericUpDownComWriteTimeout.ValueChanged += new EventHandler(numericUpDownComWriteTimeout_ValueChanged);
            checkBoxTcp.CheckedChanged += new EventHandler(checkBoxTcp_CheckedChanged);

            meterPinged += new EventHandler(Form1_meterPinged);
            pollingEnd += new EventHandler(Form1_pollingEnd);

            btnAddressRead.Clear();
            /*
            richTextBox1.Text += 
@"Адреса параметров, канал 1, например, температуры = 0x31, 2-й канал 0x32 и т.д.
0x10 - объём
0x20 - масса
0x30 - температура
0x40 - давление
0x50 - тепловая энергия
0x60 - электроэнергия: тариф С1
0x70 - электроэнергия: тариф С2
0x80 - электроэнергия: тариф С3
0x90 - электроэнергия: тариф С4
0xA0 - дата и время архивной записи
0xB0 - наработка подсистемы, в минутах
0xC0 - флаги ошибок
0xD1 – Tmin, интервал времени, в котором расход теплоносителя был меньше минимального значения(Gmin), указанного в паспорте прибора
0xD2 - Tmax, интервал времени, в котором расход теплоносителя был больше максимально допустимого значения(Gmax), указанного в паспорте прибора
0xD3 – Тdt, интервал времени, в котором разность температур была меньше допустимого значения, указанного в паспорте прибора
0xD4 – Тф, время действия нештатных ситуаций, которые привели к аварии
0xD5 - Тэп, интервал времени, в котором питание теплосчетчика или расходомеров было отключено
0xD6 - Тнс, интервал времени, в котором Карат-308 рассчитал, что пар стал насыщенным(только для Карат-308).

";
*/


            btnAddressRead.Text += @"
 Краткая справка тип - кол-во бит:
 sint64, uint64  - 64
 uint32, float - 32
 uint16, uint8 - 16

*uint8 - в документе ошибка. Нельзя запросить меньше одного регистра, а регистр - 16 бит.
";
            dateTimePicker1.Value = DateTime.Now.Date.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now.Date.AddDays(-1); 
        }

        void numericUpDownComWriteTimeout_ValueChanged(object sender, EventArgs e)
        {
            setVirtualSerialPort();
        }

        DataTable dt = new DataTable("meters");
        public string worksheetName = "Лист1";

        //определяет колонки таблицы
        int cfgId = 1;

        //список, хранящий номера параметров в перечислении Params драйвера
        //целесообразно его сделать здесь, так как кол-во считываемых значений зависит от кол-ва колонок
        List<int> paramCodes = null;
        private void createMainTableOld(ref DataTable dt)
        {
            paramCodes = new List<int>();

            //creating columns for internal data table
            DataColumn column = dt.Columns.Add();
            column.DataType = typeof(string);
            column.Caption = "№ кв.";
            column.ColumnName = "colFlat";

            column = dt.Columns.Add();
            column.DataType = typeof(string);
            column.Caption = "S/N";
            column.ColumnName = "colFactory";

            column = dt.Columns.Add();
            column.DataType = typeof(string);
            column.Caption = "Результат";
            column.ColumnName = "colResult";

 




            if (cfgId == 0)
            {
                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Канал";
                column.ColumnName = "colChannel";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Счетчик";
                column.ColumnName = "colMeterNumber";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "IP";
                column.ColumnName = "colIp";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Port";
                column.ColumnName = "colPort";

                if (!cbJustRead.Checked)
                {
                    column = dt.Columns.Add();
                    column.DataType = typeof(string);
                    column.Caption = "Значение";
                    column.ColumnName = "colVal1";
                }
                else
                {
                    column = dt.Columns.Add();
                    column.DataType = typeof(string);
                    column.Caption = "Тариф 1";
                    column.ColumnName = "colVal1";

                    column = dt.Columns.Add();
                    column.DataType = typeof(string);
                    column.Caption = "Тариф 2";
                    column.ColumnName = "colVal2";

                    column = dt.Columns.Add();
                    column.DataType = typeof(string);
                    column.Caption = "Тариф 3";
                    column.ColumnName = "colVal3";

                    column = dt.Columns.Add();
                    column.DataType = typeof(string);
                    column.Caption = "Тариф 4";
                    column.ColumnName = "colVal4";

                }
            } else if (cfgId == 1)
            {
                if (cbFromFileTcp.Checked)
                {
                    column = dt.Columns.Add();
                    column.DataType = typeof(string);
                    column.Caption = "IP";
                    column.ColumnName = "colIp";

                    column = dt.Columns.Add();
                    column.DataType = typeof(string);
                    column.Caption = "Port";
                    column.ColumnName = "colPort";
                }

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Тип прибора";
                column.ColumnName = "colMeterType";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Прошивка";
                column.ColumnName = "colSWVersion";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "S/N прочитаный";
                column.ColumnName = "colReadSN";



                //данные
                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Энергия (ГКал)";
                column.ColumnName = "colEnergy";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Объем (м3)";
                column.ColumnName = "colVolume";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Наработка";
                column.ColumnName = "colTime";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Наработка с о";
                column.ColumnName = "colTimeErr";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "TPod";
                column.ColumnName = "colTempPod";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "TObr";
                column.ColumnName = "colTempObr";
            }



            DataRow captionRow = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
                captionRow[i] = dt.Columns[i].Caption;
            dt.Rows.Add(captionRow);

        }

        private void createMainTable(ref DataTable dt)
        {
            paramCodes = new List<int>();

            //creating columns for internal data table
            DataColumn column = dt.Columns.Add();
            column.DataType = typeof(string);
            // сетевой номер
            column.Caption = "№";
            column.ColumnName = "colFlat";


            column = dt.Columns.Add();
            column.DataType = typeof(string);
            column.Caption = "Результат";
            column.ColumnName = "colResult";

            column = dt.Columns.Add();
            column.DataType = typeof(string);
            column.Caption = "SN";
            column.ColumnName = "colSN";

            DataRow captionRow = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
                captionRow[i] = dt.Columns[i].Caption;
            dt.Rows.Add(captionRow);

        }

        private void loadXlsFile()
        {
            try
            {
                doStopProcess = false;
                buttonStop.Enabled = true;

                string fileName = ofd1.FileName;
                Workbook book = Workbook.Load(fileName);

                //auto detection of working mode
                object typeDirectiveVal = "";



                try
                {
                     Row zeroRow = book.Worksheets[0].Cells.GetRow(0);
                    typeDirectiveVal = zeroRow.GetCell(0).Value;
                }
                catch (Exception ex)
                {
                    return;
                }

                dt = new DataTable();
                createMainTable(ref dt);

                Worksheet workSheet = book.Worksheets[0];

              
                if (book.Worksheets.Count > 1)
                {
                    string wshIndexStr = Prompt.ShowDialog("Укажите номер листа, начиная с 1", "Выберите лист");
                    int wshIndex = 1;
                    if (!int.TryParse(wshIndexStr, out wshIndex) || book.Worksheets.Count < wshIndex)
                        MessageBox.Show("Введен неверный некорректный номер");
          
                    wshIndex--;
                    workSheet = book.Worksheets[wshIndex];
                }


                int rowsInFile = 0;
                rowsInFile = workSheet.Cells.LastRowIndex - firstRowIndex;

                //setting up progress bar
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = rowsInFile;
                toolStripProgressBar1.Step = 1;

                //filling internal data table with *.xls file data according to *.config file
                for (int i = 0; i < 1; i++)
                {
                    Worksheet sheet = workSheet;
                    //если пусто в номере квартиры, берем предыдущий
                    string strFlatPrevNumber = "";

                    for (int rowIndex = firstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
                    {
                        if (doStopProcess)
                        {
                            buttonStop.Enabled = false;
                            return;
                        }

                        Row row_l = sheet.Cells.GetRow(rowIndex);
                        DataRow dataRow = dt.NewRow();




                        object oFlatNumber = row_l.GetCell(flatNumberColumnIndex).Value;
                        int iFlatNumber = -1;
                        if (oFlatNumber != null)
                        {
                            string tmpStrNumb = oFlatNumber.ToString().Replace("Квартира ", "").Replace("Офис ", "");
                            strFlatPrevNumber = tmpStrNumb;
                            if (!int.TryParse(tmpStrNumb, out iFlatNumber))
                            {
                                incrProgressBar();
                                continue;
                            }

                            incrProgressBar();
                            dataRow[0] = iFlatNumber;
                        }
                        else
                        {
                            if (!int.TryParse(strFlatPrevNumber, out iFlatNumber))
                            {
                                incrProgressBar();
                                continue;     
                            }

                            incrProgressBar();
                            dataRow[0] = iFlatNumber;
                 
                        }


                        dataRow[1] = row_l.GetCell(factoryNumberColumnIndex).Value;
                        dataRow[2] = "";

                        if (cfgId == 0)
                        { 
                            //предустановленные
                            dataRow[3] = row_l.GetCell(colChannelIndex).Value;
                            dataRow[4] = row_l.GetCell(colMeterNameIndex).Value;
                            dataRow[5] = row_l.GetCell(colIPIndex).Value;
                            dataRow[6] = row_l.GetCell(colPortIndex).Value;

                            if (!cbJustRead.Checked)
                            {
                                string strTmpPredVal = row_l.GetCell(colPredValue).Value == null ? "" : row_l.GetCell(colPredValue).Value.ToString().Replace(" ", "");
                                dataRow[7] = strTmpPredVal;
                            }
                        } else if (cfgId == 1)
                        {
                            if (cbFromFileTcp.Checked)
                            {
                                dataRow[3] = row_l.GetCell(colIPIndex).Value;
                                dataRow[4] = row_l.GetCell(colPortIndex).Value;
                            }
                        }

                        dt.Rows.Add(dataRow);
                    }
                }


                dgv1.DataSource = dt;

                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Maximum = dt.Rows.Count - 1;
                toolStripStatusLabel1.Text = String.Format("({0}/{1})", toolStripProgressBar1.Value, toolStripProgressBar1.Maximum);

                InputDataReady = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно загрузить таблицу, проверьте что файл не открыт в другой программе", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (ofd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                loadXlsFile();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (sfd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //create new xls file
                string file = sfd1.FileName;
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet(worksheetName);

                //office 2010 will not open file if there is less than 100 cells
                for (int i = 0; i < 100; i++)
                    worksheet.Cells[i, 0] = new Cell("");

                //copying data from data table
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                    {
                        worksheet.Cells[rowIndex, colIndex] = new Cell(dt.Rows[rowIndex][colIndex].ToString());
                    }
                }

                workbook.Worksheets.Add(worksheet);
                workbook.Save(file);
            }
        }

        private void incrProgressBar()
        {
            if (toolStripProgressBar1.Value < toolStripProgressBar1.Maximum)
            {
                toolStripProgressBar1.Value += 1;
                toolStripStatusLabel1.Text = String.Format("({0}/{1})", toolStripProgressBar1.Value, toolStripProgressBar1.Maximum);
            }
        }

        //Возникает по окончании Теста связи или Опроса ОДНОГО счетчика из списка
        public event EventHandler meterPinged;
        void Form1_meterPinged(object sender, EventArgs e)
        {
            incrProgressBar();
        }

        //Возникает по окончании Теста связи или Опроса ВСЕХ счетчиков списка
        public event EventHandler pollingEnd;
        void Form1_pollingEnd(object sender, EventArgs e)
        {
            InProgress = false;
            doStopProcess = false;
        }

        Thread pingThr = null;
        //Обработчик кнопки "Тест связи"
        private void buttonPing_Click(object sender, EventArgs e)
        {
            InProgress = true;
            doStopProcess = false;

            DeleteLogFiles();

            pingThr = new Thread(pingMeters);
            pingThr.Start((object)dt);
        }


        private void pingMeters(Object metersDt)
        {
            DataTable dt = (DataTable)metersDt;


            List<int> rowsWithIncorrectResults = new List<int>();

            //если список строк не определен, источник заполняется номерами строк доступных
            //в таблице, иначе - определенными номерами 
            List<int> rowsList = new List<int>();
            for (int i = 1; i < dt.Rows.Count; i++) rowsList.Add(i);

            List<string> factoryNumbers = new List<string>();

            for (int m = 0; m < rowsList.Count; m++)
            {
                int i = rowsList[m];
                object o = dt.Rows[i]["colFactory"];

                if (o != null)
                {
                    try
                    {
                        Meter = new KaratDanfosDriver();
                        uint address = uint.Parse(o.ToString());

                        if (cbFromFileTcp.Checked)
                        {
                            //TODO: сделать это подсосом из xml
                            NameValueCollection loadedAppSettings = new NameValueCollection();
                            loadedAppSettings.Add("localEndPointIp", this.listBox1.SelectedItem.ToString());

                            Vp = new TcpipPort(dt.Rows[i][3].ToString(), int.Parse(dt.Rows[i][4].ToString()), 600, 1000, 50, loadedAppSettings);
                        }

                        Meter.Init(address, "", Vp);

                        string readSerial = "";
                        if (Meter.ReadSerialNumber(ref readSerial))
                        {
                            if (cfgId == 1) dt.Rows[i]["colReadSN"] = readSerial;
                            dt.Rows[i]["colResult"] = "На связи";
                        }
                        else
                        {
                            dt.Rows[i]["colResult"] = "Ошибка";
                            goto PREEND;
                        }
    
                        /*
                        if (cfgId == 1)
                        {
                            string meterType = "", swVersion = "";
                            if (Meter.ReadMeterType(ref meterType))
                                dt.Rows[i]["colMeterType"] = meterType;

                            if (Meter.ReadSoftwareVersion(ref swVersion))
                                dt.Rows[i]["colSWVersion"] = swVersion;
                        }
                        */

                        if (cfgId == 0)
                        {
                            float readVal = -1;


                            if (Meter.ReadDailyValues(DateTime.Now.Date, 1, 1, ref readVal))
                            {
                                dt.Rows[i]["colResult"] = readVal;
                            }
                            else
                            {
                                dt.Rows[i]["colResult"] = -1;
                                goto PREEND;
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        dt.Rows[i]["colResult"] = "FAIL ex: " + ex.Message;
                        goto PREEND;

                    }

                    Thread.Sleep(50);
                }

                PREEND:

                Invoke(meterPinged);
                //  Meter.UnselectAllMeters();

                if (doStopProcess)
                    break;
            }

            END:
            Invoke(pollingEnd);


        }

        struct PollMetersArguments
        {
            public DataTable dt;
            public List<int> incorrectRows;
        }

        //Обработчик кнопки "Опрос"
        private void buttonPoll_Click(object sender, EventArgs e)
        {
            InProgress = true;
            doStopProcess = false;

            DeleteLogFiles();

            PollMetersArguments pma = new PollMetersArguments();
            pma.dt = dt;
            pma.incorrectRows = null;

            if (cbJustRead.Checked)
                pingThr = new Thread(pollMeters);
            else
                pingThr = new Thread(pollInitMetersWithValues);
            
            pingThr.Start((object)pma);
        }

        private void DeleteLogFiles()
        {
            string curDir = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                FileInfo fi = new FileInfo(curDir + "teplouchetlog.pi");
                if (fi.Exists)
                    fi.Delete();

                fi = new FileInfo(curDir + "metersinfo.pi");
                if (fi.Exists)
                    fi.Delete();

                fi = new FileInfo(curDir + "datainfo.pi");
                if (fi.Exists)
                    fi.Delete();
            }
            catch (Exception ex)
            {
                //
            }
        }


        //если во время опроса были приборы, ответившние некорректно (например с нулевой температурой)
        //строки данных с номерами данных приборов записываются в списоск, и передаются этому методу
        //для повторного опроса по завершении первичного
        private void pollMetersWithIncorrectData(Object metersDt, List<int> rows)
        {
            DataTable dt = (DataTable)metersDt;
            int columnIndexFactory = 1;
            int columnIndexResult = 2;

            for (int i = 0; i < rows.Count; i++)
            {
                int tmpNumb = 0;
                object o = dt.Rows[i][columnIndexFactory];
                object oColResult = dt.Rows[i][columnIndexResult];

                if (o != null)
                {
                    byte addressByte = 0;
                    try
                    {
                        addressByte = Convert.ToByte(o.ToString(), 16);
                        tmpNumb = addressByte;
                        List<float> valList = new List<float>();
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
            }
        }


        private void setPreVals(PollMetersArguments pmaInp)
        {
            DataTable dt = pmaInp.dt;
            List<int> incorrectRows = pmaInp.incorrectRows;

            List<int> rowsWithIncorrectResults = new List<int>();

            //если список строк не определен, источник заполняется номерами строк доступных
            //в таблице, иначе - определенными номерами 
            List<int> rowsList = new List<int>();
            for (int i = 1; i < dt.Rows.Count; i++) rowsList.Add(i);

            List<string> factoryNumbers = new List<string>();

            for (int m = 0; m < rowsList.Count; m++)
            {
                int i = rowsList[m];
                byte tmpNumb = 0x0;
                object o = dt.Rows[i]["colFactory"];

                object oVal = dt.Rows[i]["colVal1"];
                double dVal = 0;

                if (o != null)
                {

                    if (!checkBoxPollOffline.Checked && (oVal == null || oVal.ToString() == "")) goto PREEND;

                    try
                    {
                        string strVal = "";
                        if (oVal != null && oVal.ToString() != "")
                        {
                           strVal =  oVal.ToString().Replace(".", ",");
                            dVal = double.Parse(strVal);
                        }
    

                        Meter = new KaratDanfosDriver();
                        uint address = uint.Parse(o.ToString());
                        string ip = dt.Rows[i]["colIp"].ToString();
                        int port = int.Parse(dt.Rows[i]["colPort"].ToString());
                        Vp = new TcpipPort(ip, port, 400, 800, 10);
                        Meter.Init(address, "", Vp);

                        int chan = int.Parse(dt.Rows[i]["colChannel"].ToString());

                        if (!checkBoxPollOffline.Checked)
                        {
                            /*
                            if (Meter.WriteValue(dVal, chan))
                            {
                                dt.Rows[i]["colResult"] = "OK";

                            }
                            else
                            {*/
                                dt.Rows[i]["colResult"] = "FAIL";
                                goto PREEND;
                            //}
                        }
                        else
                        {
                            float readVal = -1;
                            if (Meter.ReadCurrentValues((ushort)chan, 0, ref readVal))
                            {
                                dt.Rows[i]["colResult"] = readVal;

                            }
                            else
                            {
                                dt.Rows[i]["colResult"] = -1;
                                goto PREEND;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        dt.Rows[i]["colResult"] = "FAIL ex: " + ex.Message ;
                        goto PREEND;

                    }

                    Thread.Sleep(50);
                }

            PREEND:

                Invoke(meterPinged);
              //  Meter.UnselectAllMeters();

                if (doStopProcess)
                    break;
            }

        END:
            Invoke(pollingEnd);

        }

        private void pollInitMetersWithValues(Object pollMetersArgs)
        {

            PollMetersArguments pmaInp = (PollMetersArguments)pollMetersArgs;
            DataTable dt = pmaInp.dt;
            List<int> incorrectRows = pmaInp.incorrectRows;

            if (true)
            {
                setPreVals(pmaInp);
                return;
            }



        }


        private void pollMeters(Object pollMetersArgs)
        {

            PollMetersArguments pmaInp = (PollMetersArguments)pollMetersArgs;
            DataTable dt = pmaInp.dt;
            List<int> incorrectRows = pmaInp.incorrectRows;


            List<int> rowsWithIncorrectResults = new List<int>();

            //если список строк не определен, источник заполняется номерами строк доступных
            //в таблице, иначе - определенными номерами 
            List<int> rowsList = new List<int>();
            for (int i = 1; i < dt.Rows.Count; i++) rowsList.Add(i);

            List<string> factoryNumbers = new List<string>();

            for (int m = 0; m < rowsList.Count; m++)
            {
                int i = rowsList[m];
                object o = dt.Rows[i]["colFlat"];

                if (o != null)
                {
                    try
                    {
                        Meter = new KaratDanfosDriver();
                        uint address = uint.Parse(o.ToString());

   

                        Meter.Init(address, "", Vp);

                        string meterSN = "";
                        if (Meter.ReadSerialNumber(ref meterSN))
                        {
                            dt.Rows[i]["colSN"] = meterSN;
                        }
                        else
                        {
                            dt.Rows[i]["colSN"] = "Err";
                        }


                        // 9	RO	sint64	AccumulatedEnergy	Дж	Накопленная тепловая энергия


                        float resultVal = -1;
                        if (Meter.ReadCurrentValues(9, 64, ref resultVal))
                        {
                            dt.Rows[i]["colResult"] = resultVal;
                        }
                        else
                        {
                            dt.Rows[i]["colResult"] = "Err";
                        }


                    }
                    catch (Exception ex)
                    {
                        dt.Rows[i]["colResult"] = "FAIL ex: " + ex.Message;
                        goto PREEND;

                    }

                    Thread.Sleep(50);
                }

            PREEND:

                Invoke(meterPinged);
                //  Meter.UnselectAllMeters();

                if (doStopProcess)
                    break;
            }

        END:
            Invoke(pollingEnd);


        }


        //Обработчик клавиши "Стоп"
        private void buttonStop_Click(object sender, EventArgs e)
        {
            doStopProcess = true;

            buttonStop.Enabled = false;
            dgv1.Enabled = false;
        }

        private void comboBoxComPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            setVirtualSerialPort();
        }

        private void numericUpDownComReadTimeout_ValueChanged(object sender, EventArgs e)
        {
            setVirtualSerialPort();
        }

        private void checkBoxPollOffline_CheckedChanged(object sender, EventArgs e)
        {
            bPollOnlyOffline = checkBoxPollOffline.Checked;
            if (bPollOnlyOffline)
            {
                buttonPoll.Text = "Читать";
            }
            else
            {
                buttonPoll.Text = "Записать";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (InProgress)
            {
                MessageBox.Show("Остановите опрос перед закрытием программы","Напоминание");
                e.Cancel = true;
                return;
            }

           
            Vp.Close();
        }

        /// <summary>
        /// Запись в ЛОГ-файл
        /// </summary>
        /// <param name="str"></param>
        public void WriteToLog(string str, bool doWrite = true)
        {
            if (doWrite)
            {
                StreamWriter sw = null;
                FileStream fs = null;
                try
                {
                    string curDir = AppDomain.CurrentDomain.BaseDirectory;
                    fs = new FileStream(curDir + "metersinfo.pi", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    sw = new StreamWriter(fs, Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString() + ": " + str);
                    sw.Close();
                    fs.Close();
                }
                catch
                {
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
        }
        public void WriteToSeparateLog(string str, bool doWrite = true)
        {
            if (doWrite)
            {
                StreamWriter sw = null;
                FileStream fs = null;
                try
                {
                    string curDir = AppDomain.CurrentDomain.BaseDirectory;
                    fs = new FileStream(curDir + "datainfo.pi", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    sw = new StreamWriter(fs, Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString() + ": " + str);
                    sw.Close();
                    fs.Close();
                }
                catch
                {
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
        }
        #region Панель разработчика

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.A)
                DeveloperMode = !DeveloperMode;
        }

        bool bDeveloperMode = false;
        public bool DeveloperMode
        {
            get { return bDeveloperMode; }
            set
            {
                bDeveloperMode = value;

                if (bDeveloperMode)
                {
                    this.Text += FORM_TEXT_DEV_ON;
                    this.Height = this.Height + groupBox1.Height;
                    groupBox1.Visible = true;

                }
                else
                {
                    this.Text = this.Text.Replace(FORM_TEXT_DEV_ON, String.Empty);
                    groupBox1.Visible = false;
                    this.Height = this.Height - groupBox1.Height;
                }
            }
        }

        private void checkBoxTcp_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                comboBoxComPorts.Enabled = false;
                //numericUpDownComReadTimeout.Enabled = false;
            }
            else
            {
                comboBoxComPorts.Enabled = true;
               // numericUpDownComReadTimeout.Enabled = true;
            }
            setVirtualSerialPort();
        }



        #endregion

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            Process.Start("http://prizmer.ru/");
        }

        bool _predefineImpulseInitialsMode = false;
        bool PredefineImpulseInitialsMode
        {
            get { return _predefineImpulseInitialsMode; }
            set
            {
                _predefineImpulseInitialsMode = value;
                if (value)
                {
                    buttonPoll.Text = "Старт";
                }
                else
                {
                    buttonPoll.Text = "Опрос";
                }
            }
        }



        private void btnIndPollInfo_Click(object sender, EventArgs e)
        {
            string serial = "", sw = "", mt = "";

            //KaratDanfosDriver pd = new KaratDanfosDriver();
            
            Meter.Init(uint.Parse(textBox1.Text), passwordDefault, Vp);

            string versionSw = "-1";
            if (!Meter.getMeterSoftware(ref versionSw))
            {
                sw = "Can't get meter model...";
            }
            else
            {
                sw = versionSw;
            }

            if (!Meter.ReadSerialNumber(ref serial))
            {
                serial = "No serial...";
            }

            btnAddressRead.Clear();
            btnAddressRead.Text += serial + "\n";
            btnAddressRead.Text += sw + "\n";
        }

        private void btnIndPollDaily_Click(object sender, EventArgs e)
        {
            KaratDanfosDriver pd = new KaratDanfosDriver();
            pd.Init(uint.Parse(textBox1.Text), passwordDefault, Vp);

            if (!pd.OpenLinkCanal())
            {
                btnAddressRead.Clear();
                btnAddressRead.Text += "No chanel opened...";
            }

            btnAddressRead.ScrollToCaret();

            float rVal = 0f;
            if (pd.ReadDailyValues(dateTimePicker1.Value.Date, (ushort)numericUpDown1.Value, (ushort)numericUpDown2.Value, ref rVal))
            {
                btnAddressRead.Text += rVal + ";\n";
            }
            else
            {
                btnAddressRead.Text += "Не удалось " + (ushort)numericUpDown1.Value + ";\n";
            }
        }

        private void btnIndPollMonthly_Click(object sender, EventArgs e)
        {
            KaratDanfosDriver pd = new KaratDanfosDriver();
            pd.Init(uint.Parse(textBox1.Text), passwordDefault, Vp);

            if (!pd.OpenLinkCanal())
            {
                btnAddressRead.Clear();
                btnAddressRead.Text += "No chanel opened...";
            }

            float rVal = 0f;
            if (pd.ReadMonthlyValues(dateTimePicker1.Value.Date, (ushort)numericUpDown1.Value, (ushort)numericUpDown2.Value, ref rVal))
            {
                btnAddressRead.Text += rVal + ";\n";
            }
            else
            {
                btnAddressRead.Text += "Не удалось " + (ushort)numericUpDown1.Value + ";\n";
            }
        }

        private void btnIndPollCurrent_Click(object sender, EventArgs e)
        {
            KaratDanfosDriver pd = new KaratDanfosDriver();

            pd.Init(uint.Parse(textBox1.Text), passwordDefault, Vp);

            if (!pd.OpenLinkCanal())
            {
                btnAddressRead.Clear();
                btnAddressRead.Text += "No chanel opened...";
            }


            float rVal = 0f;
            if (pd.ReadCurrentValues((ushort)numericUpDown1.Value, (ushort)numericUpDown2.Value, ref rVal))
            {
                btnAddressRead.Text += rVal + ";\n";
            }
            else
            {
                btnAddressRead.Text += "Не удалось " + (ushort)numericUpDown1.Value + ";\n";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {
            btnAddressRead.Clear();
        }

        private void btnReadHalfs_MouseCaptureChanged(object sender, EventArgs e)
        {

        }

        private void btnReadHalfs_Click(object sender, EventArgs e)
        {
            //Karat30XDriver pd = new Karat30XDriver();
            //pd.Init(uint.Parse(textBox1.Text), passwordDefault, Vp);

            //richTextBox1.Clear();
            //if (!pd.OpenLinkCanal()) richTextBox1.Text += "Не удалось открыть канал связи...;\n";

            //List<RecordPowerSlice> rpsList = new List<RecordPowerSlice>();
            //DateTime dtFrom = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1
            //    .Value.Month, dateTimePicker1.Value.Day, 0, 0, 0);
            //DateTime dtTo = dateTimePicker2.Value;

            //richTextBox1.Text += "Fr: " + dtFrom + "\n";
            //richTextBox1.Text += "To: " + dtTo + "\n";

            //if (checkBox1.Checked)
            //{
            //    //old routine method
            //    richTextBox1.Text += "Выполняем старый метод;\n\n";
            //    //if (!pd.ReadPowerSliceSlowBeforeSW9(dtFrom, dtTo, ref rpsList, 30))
            //    //{
            //    //    richTextBox1.Text += "Метод ReadPowerSliceSlowAfterSW9 вернул false...;\n";
            //    //    return;
            //    //}
            //} else
            //{
            //    //new fast one
            //    richTextBox1.Text += "Выполняем новый > 9000 метод;\n\n";
            //    if (!pd.ReadPowerSlice(dtFrom, dtTo, ref rpsList, 30))
            //    {
            //        richTextBox1.Text += "Метод ReadPowerSlice вернул false...;\n";
            //        return;
            //    }
            //}

            //for (int i = 0; i < rpsList.Count; i++)
            //{
            //    RecordPowerSlice tmpRps = rpsList[i];
            //    string str = String.Format("Ap{0};Am{1};Rp{2};Rm{3};St:{4};\n", 
            //        tmpRps.APlus, tmpRps.AMinus, tmpRps.RPlus, tmpRps.RMinus, tmpRps.status);
            //    richTextBox1.Text += str;
            //}


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void richTextBox1_DoubleClick_1(object sender, EventArgs e)
        {
            ((RichTextBox)sender).Clear();
        }

    
        private void btnIpPreset_Click(object sender, EventArgs e)
        {
            textBoxIp.Text = "192.168.23.22";
            textBoxPort.Text = "2002";
        }

        private void btnGetErrDescr_Click(object sender, EventArgs e)
        {
            Int32 errCode = -1;
            if (!int.TryParse(tbErrCode.Text, out errCode)) return;

            string resStr = "";

            string[] errNames = Enum.GetNames(typeof(SubSystemErrors));
            int[] errCodes = (Int32[])Enum.GetValues(typeof(SubSystemErrors)).Cast<Int32>();

            for (int i = 0; i < errCodes.Length; i++)
            {
                int iteratee = (int)errCodes[i];
                int res = errCode & iteratee;


                if (res == iteratee)
                {
                    resStr += errNames[i] + " ";
                }
            }

            btnAddressRead.Text = resStr;
        }

        private void btnAddrRead_Click(object sender, EventArgs e)
        {

        }

        private void btnAddressSet_Click(object sender, EventArgs e)
        {
            string serial = "", sw = "", mt = "";

            // обязательно по широковещательному задаем адрес
            Meter.Init(248, passwordDefault, Vp);

            if (!Meter.setMeterAddress(ushort.Parse(textBox1.Text)))
            {
                sw = "Не получилось обновить адрес прибора...";
            } else
            {
                sw = "Готово";
            }

            btnAddressRead.Clear();
            btnAddressRead.Text += sw + "\n";
        }

        private void btnGenerateTable_Click(object sender, EventArgs e)
        {
            dt = new DataTable();
            createMainTable(ref dt);


            for (int rowIndex = (int)numAddrFrom.Value; rowIndex <= (int)numAddrTo.Value; rowIndex++)
            {
                if (doStopProcess)
                {
                    buttonStop.Enabled = false;
                    return;
                }

                DataRow dataRow = dt.NewRow();
                dataRow[0] = rowIndex;
                dataRow[1] = "";

               
                dt.Rows.Add(dataRow);

                incrProgressBar();
            }

            dgv1.DataSource = dt;

            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Maximum = dt.Rows.Count - 1;
            toolStripStatusLabel1.Text = String.Format("({0}/{1})", toolStripProgressBar1.Value, toolStripProgressBar1.Maximum);

            InputDataReady = true;
        }

        private void btnAddrClear_Click(object sender, EventArgs e)
        {
            string serial = "", sw = "", mt = "";


            // обязательно по широковещательному задаем адрес
            Meter.Init(0, passwordDefault, Vp);
            if (!Meter.setMeterAddress(0x00FF, true))
            {
                sw = "Вроде готово...";
            }

            btnAddressRead.Clear();
            btnAddressRead.Text += sw + "\n";
        }

        private void checkBoxTcp_CheckedChanged_1(object sender, EventArgs e)
        {

        }
    }


    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
