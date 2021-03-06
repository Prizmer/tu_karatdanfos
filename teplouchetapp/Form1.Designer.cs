﻿namespace elfextendedapp
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonImport = new System.Windows.Forms.Button();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.ofd1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonPing = new System.Windows.Forms.Button();
            this.sfd1 = new System.Windows.Forms.SaveFileDialog();
            this.buttonPoll = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.comboBoxComPorts = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.numericUpDownComReadTimeout = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxPollOffline = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.numericUpDownComWriteTimeout = new System.Windows.Forms.NumericUpDown();
            this.checkBoxTcp = new System.Windows.Forms.CheckBox();
            this.btnIndPollDaily = new System.Windows.Forms.Button();
            this.btnIndPollCurrent = new System.Windows.Forms.Button();
            this.cbFromFileTcp = new System.Windows.Forms.CheckBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnReadHalfs = new System.Windows.Forms.Button();
            this.btnIndPollMonthly = new System.Windows.Forms.Button();
            this.btnGenerateTable = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddrClear = new System.Windows.Forms.Button();
            this.btnAddressSet = new System.Windows.Forms.Button();
            this.btnGetErrDescr = new System.Windows.Forms.Button();
            this.tbErrCode = new System.Windows.Forms.TextBox();
            this.btnIpPreset = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnIndPollInfo = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbJustRead = new System.Windows.Forms.CheckBox();
            this.btnAddressRead = new System.Windows.Forms.RichTextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.numAddrTo = new System.Windows.Forms.NumericUpDown();
            this.numAddrFrom = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownComReadTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownComWriteTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAddrTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAddrFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonImport
            // 
            this.buttonImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImport.Location = new System.Drawing.Point(637, 66);
            this.buttonImport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(129, 31);
            this.buttonImport.TabIndex = 3;
            this.buttonImport.Text = "Импорт (*.xls)";
            this.toolTip1.SetToolTip(this.buttonImport, "Загрузить таблицу содержающую столбец с номерами квартир и столбец с заводскими н" +
        "омерами счетчиков");
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Visible = false;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.AllowUserToDeleteRows = false;
            this.dgv1.AllowUserToResizeRows = false;
            this.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv1.Location = new System.Drawing.Point(17, 100);
            this.dgv1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgv1.Name = "dgv1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv1.RowTemplate.Height = 28;
            this.dgv1.Size = new System.Drawing.Size(792, 303);
            this.dgv1.TabIndex = 4;
            // 
            // ofd1
            // 
            this.ofd1.FileName = "openFileDialog1";
            // 
            // buttonPing
            // 
            this.buttonPing.Enabled = false;
            this.buttonPing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPing.Location = new System.Drawing.Point(675, 84);
            this.buttonPing.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPing.Name = "buttonPing";
            this.buttonPing.Size = new System.Drawing.Size(111, 31);
            this.buttonPing.TabIndex = 5;
            this.buttonPing.Text = "Тест связи";
            this.toolTip1.SetToolTip(this.buttonPing, "Выполняется только проверка связи без получения каких-либо данных со счетчика");
            this.buttonPing.UseVisualStyleBackColor = true;
            this.buttonPing.Visible = false;
            this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
            // 
            // buttonPoll
            // 
            this.buttonPoll.Enabled = false;
            this.buttonPoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPoll.Location = new System.Drawing.Point(370, 57);
            this.buttonPoll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPoll.Name = "buttonPoll";
            this.buttonPoll.Size = new System.Drawing.Size(111, 31);
            this.buttonPoll.TabIndex = 6;
            this.buttonPoll.Text = "Опрос";
            this.toolTip1.SetToolTip(this.buttonPoll, "Выполняются проверка связи и опрос счетчика по текущим значениям");
            this.buttonPoll.UseVisualStyleBackColor = true;
            this.buttonPoll.Click += new System.EventHandler(this.buttonPoll_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Enabled = false;
            this.buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExport.Location = new System.Drawing.Point(383, 5);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(129, 31);
            this.buttonExport.TabIndex = 41;
            this.buttonExport.Text = "Экспорт (*.xls)";
            this.toolTip1.SetToolTip(this.buttonExport, "Сохранить полученные в программе данные");
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // comboBoxComPorts
            // 
            this.comboBoxComPorts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxComPorts.FormattingEnabled = true;
            this.comboBoxComPorts.Location = new System.Drawing.Point(11, 10);
            this.comboBoxComPorts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxComPorts.Name = "comboBoxComPorts";
            this.comboBoxComPorts.Size = new System.Drawing.Size(112, 24);
            this.comboBoxComPorts.TabIndex = 42;
            this.toolTip1.SetToolTip(this.comboBoxComPorts, "Системный последовательный порт");
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 633);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1144, 26);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 43;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(177, 20);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 21);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 21);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.Location = new System.Drawing.Point(487, 57);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(103, 31);
            this.buttonStop.TabIndex = 44;
            this.buttonStop.Text = "Стоп";
            this.toolTip1.SetToolTip(this.buttonStop, "Прекращает длительные процессы в программе и закрывает системный порт");
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // numericUpDownComReadTimeout
            // 
            this.numericUpDownComReadTimeout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownComReadTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownComReadTimeout.Location = new System.Drawing.Point(135, 12);
            this.numericUpDownComReadTimeout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDownComReadTimeout.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownComReadTimeout.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownComReadTimeout.Name = "numericUpDownComReadTimeout";
            this.numericUpDownComReadTimeout.Size = new System.Drawing.Size(61, 18);
            this.numericUpDownComReadTimeout.TabIndex = 45;
            this.toolTip1.SetToolTip(this.numericUpDownComReadTimeout, "Время ожидания ответа одного счетчика");
            this.numericUpDownComReadTimeout.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(201, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 46;
            this.label1.Text = "мс";
            // 
            // checkBoxPollOffline
            // 
            this.checkBoxPollOffline.AutoSize = true;
            this.checkBoxPollOffline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxPollOffline.Location = new System.Drawing.Point(703, 100);
            this.checkBoxPollOffline.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxPollOffline.Name = "checkBoxPollOffline";
            this.checkBoxPollOffline.Size = new System.Drawing.Size(119, 21);
            this.checkBoxPollOffline.TabIndex = 47;
            this.checkBoxPollOffline.Text = "Только не отв";
            this.toolTip1.SetToolTip(this.checkBoxPollOffline, "Если флаг снят, работаем в режиме записи");
            this.checkBoxPollOffline.UseVisualStyleBackColor = true;
            this.checkBoxPollOffline.Visible = false;
            this.checkBoxPollOffline.CheckedChanged += new System.EventHandler(this.checkBoxPollOffline_CheckedChanged);
            // 
            // numericUpDownComWriteTimeout
            // 
            this.numericUpDownComWriteTimeout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownComWriteTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownComWriteTimeout.Location = new System.Drawing.Point(591, 42);
            this.numericUpDownComWriteTimeout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDownComWriteTimeout.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownComWriteTimeout.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownComWriteTimeout.Name = "numericUpDownComWriteTimeout";
            this.numericUpDownComWriteTimeout.Size = new System.Drawing.Size(61, 18);
            this.numericUpDownComWriteTimeout.TabIndex = 56;
            this.toolTip1.SetToolTip(this.numericUpDownComWriteTimeout, "Время по прошествии которого происходит таймаут записи. Не используется в данной " +
        "версии.");
            this.numericUpDownComWriteTimeout.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            // 
            // checkBoxTcp
            // 
            this.checkBoxTcp.AutoSize = true;
            this.checkBoxTcp.Location = new System.Drawing.Point(488, 41);
            this.checkBoxTcp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxTcp.Name = "checkBoxTcp";
            this.checkBoxTcp.Size = new System.Drawing.Size(57, 21);
            this.checkBoxTcp.TabIndex = 55;
            this.checkBoxTcp.Text = "TCP";
            this.toolTip1.SetToolTip(this.checkBoxTcp, "Активировать режим связи по TCP/IP");
            this.checkBoxTcp.UseVisualStyleBackColor = true;
            this.checkBoxTcp.CheckedChanged += new System.EventHandler(this.checkBoxTcp_CheckedChanged_1);
            // 
            // btnIndPollDaily
            // 
            this.btnIndPollDaily.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIndPollDaily.Location = new System.Drawing.Point(66, 124);
            this.btnIndPollDaily.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnIndPollDaily.Name = "btnIndPollDaily";
            this.btnIndPollDaily.Size = new System.Drawing.Size(51, 28);
            this.btnIndPollDaily.TabIndex = 52;
            this.btnIndPollDaily.Text = "С";
            this.toolTip1.SetToolTip(this.btnIndPollDaily, "Суточный параметр");
            this.btnIndPollDaily.UseVisualStyleBackColor = true;
            this.btnIndPollDaily.Click += new System.EventHandler(this.btnIndPollDaily_Click);
            // 
            // btnIndPollCurrent
            // 
            this.btnIndPollCurrent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIndPollCurrent.Location = new System.Drawing.Point(11, 124);
            this.btnIndPollCurrent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnIndPollCurrent.Name = "btnIndPollCurrent";
            this.btnIndPollCurrent.Size = new System.Drawing.Size(51, 28);
            this.btnIndPollCurrent.TabIndex = 59;
            this.btnIndPollCurrent.Text = "Т";
            this.toolTip1.SetToolTip(this.btnIndPollCurrent, "Текущий параетр");
            this.btnIndPollCurrent.UseVisualStyleBackColor = true;
            this.btnIndPollCurrent.Click += new System.EventHandler(this.btnIndPollCurrent_Click);
            // 
            // cbFromFileTcp
            // 
            this.cbFromFileTcp.AutoSize = true;
            this.cbFromFileTcp.Location = new System.Drawing.Point(708, 39);
            this.cbFromFileTcp.Margin = new System.Windows.Forms.Padding(4);
            this.cbFromFileTcp.Name = "cbFromFileTcp";
            this.cbFromFileTcp.Size = new System.Drawing.Size(94, 21);
            this.cbFromFileTcp.TabIndex = 61;
            this.cbFromFileTcp.Text = "Из файла";
            this.toolTip1.SetToolTip(this.cbFromFileTcp, "Брать адрес и порт из загружаемой таблицы");
            this.cbFromFileTcp.UseVisualStyleBackColor = true;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(92, 94);
            this.numericUpDown2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(75, 22);
            this.numericUpDown2.TabIndex = 62;
            this.toolTip1.SetToolTip(this.numericUpDown2, "Тариф");
            this.numericUpDown2.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(11, 94);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(75, 22);
            this.numericUpDown1.TabIndex = 54;
            this.toolTip1.SetToolTip(this.numericUpDown1, "Адрес");
            this.numericUpDown1.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // btnReadHalfs
            // 
            this.btnReadHalfs.Enabled = false;
            this.btnReadHalfs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadHalfs.Location = new System.Drawing.Point(216, 94);
            this.btnReadHalfs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReadHalfs.Name = "btnReadHalfs";
            this.btnReadHalfs.Size = new System.Drawing.Size(51, 28);
            this.btnReadHalfs.TabIndex = 63;
            this.btnReadHalfs.Text = "ПЧ";
            this.toolTip1.SetToolTip(this.btnReadHalfs, "Получасовки за период");
            this.btnReadHalfs.UseVisualStyleBackColor = true;
            this.btnReadHalfs.Click += new System.EventHandler(this.btnReadHalfs_Click);
            this.btnReadHalfs.MouseCaptureChanged += new System.EventHandler(this.btnReadHalfs_MouseCaptureChanged);
            // 
            // btnIndPollMonthly
            // 
            this.btnIndPollMonthly.Enabled = false;
            this.btnIndPollMonthly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIndPollMonthly.Location = new System.Drawing.Point(122, 124);
            this.btnIndPollMonthly.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnIndPollMonthly.Name = "btnIndPollMonthly";
            this.btnIndPollMonthly.Size = new System.Drawing.Size(51, 28);
            this.btnIndPollMonthly.TabIndex = 75;
            this.btnIndPollMonthly.Text = "M";
            this.toolTip1.SetToolTip(this.btnIndPollMonthly, "Месячный параметр");
            this.btnIndPollMonthly.UseVisualStyleBackColor = true;
            this.btnIndPollMonthly.Click += new System.EventHandler(this.btnIndPollMonthly_Click);
            // 
            // btnGenerateTable
            // 
            this.btnGenerateTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateTable.Location = new System.Drawing.Point(216, 57);
            this.btnGenerateTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGenerateTable.Name = "btnGenerateTable";
            this.btnGenerateTable.Size = new System.Drawing.Size(129, 31);
            this.btnGenerateTable.TabIndex = 87;
            this.btnGenerateTable.Text = "Сформировать";
            this.toolTip1.SetToolTip(this.btnGenerateTable, "Сформировать таблицу из сетевых номеров");
            this.btnGenerateTable.UseVisualStyleBackColor = true;
            this.btnGenerateTable.Click += new System.EventHandler(this.btnGenerateTable_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAddrClear);
            this.groupBox1.Controls.Add(this.btnAddressSet);
            this.groupBox1.Controls.Add(this.btnGetErrDescr);
            this.groupBox1.Controls.Add(this.tbErrCode);
            this.groupBox1.Controls.Add(this.btnIpPreset);
            this.groupBox1.Controls.Add(this.btnIndPollMonthly);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnReadHalfs);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.cbFromFileTcp);
            this.groupBox1.Controls.Add(this.btnIndPollInfo);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.btnIndPollCurrent);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDownComWriteTimeout);
            this.groupBox1.Controls.Add(this.checkBoxTcp);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Controls.Add(this.textBoxIp);
            this.groupBox1.Controls.Add(this.btnIndPollDaily);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(7, 448);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(815, 170);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Индивидуальный блок";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnAddrClear
            // 
            this.btnAddrClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddrClear.Location = new System.Drawing.Point(91, 37);
            this.btnAddrClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddrClear.Name = "btnAddrClear";
            this.btnAddrClear.Size = new System.Drawing.Size(65, 28);
            this.btnAddrClear.TabIndex = 81;
            this.btnAddrClear.Text = "Сброс";
            this.btnAddrClear.UseVisualStyleBackColor = true;
            this.btnAddrClear.Click += new System.EventHandler(this.btnAddrClear_Click);
            // 
            // btnAddressSet
            // 
            this.btnAddressSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddressSet.Location = new System.Drawing.Point(162, 37);
            this.btnAddressSet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddressSet.Name = "btnAddressSet";
            this.btnAddressSet.Size = new System.Drawing.Size(111, 28);
            this.btnAddressSet.TabIndex = 80;
            this.btnAddressSet.Text = "Назначить";
            this.btnAddressSet.UseVisualStyleBackColor = true;
            this.btnAddressSet.Click += new System.EventHandler(this.btnAddressSet_Click);
            // 
            // btnGetErrDescr
            // 
            this.btnGetErrDescr.Enabled = false;
            this.btnGetErrDescr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetErrDescr.Location = new System.Drawing.Point(347, 142);
            this.btnGetErrDescr.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGetErrDescr.Name = "btnGetErrDescr";
            this.btnGetErrDescr.Size = new System.Drawing.Size(134, 28);
            this.btnGetErrDescr.TabIndex = 78;
            this.btnGetErrDescr.Text = "Узнать ошибку";
            this.btnGetErrDescr.UseVisualStyleBackColor = true;
            this.btnGetErrDescr.Visible = false;
            this.btnGetErrDescr.Click += new System.EventHandler(this.btnGetErrDescr_Click);
            // 
            // tbErrCode
            // 
            this.tbErrCode.Location = new System.Drawing.Point(400, 143);
            this.tbErrCode.Name = "tbErrCode";
            this.tbErrCode.Size = new System.Drawing.Size(100, 22);
            this.tbErrCode.TabIndex = 77;
            this.tbErrCode.Visible = false;
            // 
            // btnIpPreset
            // 
            this.btnIpPreset.Enabled = false;
            this.btnIpPreset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIpPreset.Location = new System.Drawing.Point(551, 37);
            this.btnIpPreset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnIpPreset.Name = "btnIpPreset";
            this.btnIpPreset.Size = new System.Drawing.Size(28, 28);
            this.btnIpPreset.TabIndex = 76;
            this.btnIpPreset.Text = "2";
            this.btnIpPreset.UseVisualStyleBackColor = true;
            this.btnIpPreset.Click += new System.EventHandler(this.btnIpPreset_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Enabled = false;
            this.dateTimePicker2.Location = new System.Drawing.Point(429, 96);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(144, 22);
            this.dateTimePicker2.TabIndex = 71;
            this.dateTimePicker2.Value = new System.DateTime(2017, 10, 30, 23, 30, 0, 0);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(425, 75);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 17);
            this.label9.TabIndex = 70;
            this.label9.Text = "Читать по";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(272, 74);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 17);
            this.label8.TabIndex = 68;
            this.label8.Text = "Читать с";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Location = new System.Drawing.Point(276, 96);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(144, 22);
            this.dateTimePicker1.TabIndex = 67;
            this.dateTimePicker1.Value = new System.DateTime(2017, 10, 30, 0, 0, 0, 0);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(363, 147);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(124, 21);
            this.checkBox1.TabIndex = 66;
            this.checkBox1.Text = "Старый метод";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(88, 74);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 17);
            this.label7.TabIndex = 65;
            this.label7.Text = "Тариф";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 74);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 17);
            this.label6.TabIndex = 64;
            this.label6.Text = "Адрес";
            // 
            // btnIndPollInfo
            // 
            this.btnIndPollInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIndPollInfo.Location = new System.Drawing.Point(718, 74);
            this.btnIndPollInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnIndPollInfo.Name = "btnIndPollInfo";
            this.btnIndPollInfo.Size = new System.Drawing.Size(72, 28);
            this.btnIndPollInfo.TabIndex = 60;
            this.btnIndPollInfo.Text = "Инфо";
            this.btnIndPollInfo.UseVisualStyleBackColor = true;
            this.btnIndPollInfo.Click += new System.EventHandler(this.btnIndPollInfo_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(587, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 17);
            this.label4.TabIndex = 58;
            this.label4.Text = "Таймаут записи";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(657, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 17);
            this.label3.TabIndex = 57;
            this.label3.Text = "мс";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(426, 43);
            this.textBoxPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(56, 22);
            this.textBoxPort.TabIndex = 54;
            this.textBoxPort.Text = "14004";
            // 
            // textBoxIp
            // 
            this.textBoxIp.Location = new System.Drawing.Point(321, 43);
            this.textBoxIp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxIp.Name = "textBoxIp";
            this.textBoxIp.Size = new System.Drawing.Size(99, 22);
            this.textBoxIp.TabIndex = 53;
            this.textBoxIp.Text = "94.199.105.211";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 17);
            this.label2.TabIndex = 50;
            this.label2.Text = "Сетевой номер (dec):";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(11, 42);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(65, 22);
            this.textBox1.TabIndex = 49;
            this.textBox1.Text = "248";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogo.Image")));
            this.pictureBoxLogo.InitialImage = null;
            this.pictureBoxLogo.Location = new System.Drawing.Point(741, 5);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(80, 72);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 50;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.Click += new System.EventHandler(this.pictureBoxLogo_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(531, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(207, 31);
            this.label5.TabIndex = 51;
            this.label5.Text = "Карат-данфос";
            // 
            // cbJustRead
            // 
            this.cbJustRead.AutoSize = true;
            this.cbJustRead.Checked = true;
            this.cbJustRead.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbJustRead.Enabled = false;
            this.cbJustRead.Location = new System.Drawing.Point(257, 12);
            this.cbJustRead.Margin = new System.Windows.Forms.Padding(4);
            this.cbJustRead.Name = "cbJustRead";
            this.cbJustRead.Size = new System.Drawing.Size(79, 21);
            this.cbJustRead.TabIndex = 53;
            this.cbJustRead.Text = "Чтение";
            this.cbJustRead.UseVisualStyleBackColor = true;
            // 
            // btnAddressRead
            // 
            this.btnAddressRead.Location = new System.Drawing.Point(828, 5);
            this.btnAddressRead.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddressRead.Name = "btnAddressRead";
            this.btnAddressRead.ReadOnly = true;
            this.btnAddressRead.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.btnAddressRead.Size = new System.Drawing.Size(305, 429);
            this.btnAddressRead.TabIndex = 54;
            this.btnAddressRead.Text = "";
            this.btnAddressRead.DockChanged += new System.EventHandler(this.richTextBox1_DoubleClick);
            this.btnAddressRead.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            this.btnAddressRead.DoubleClick += new System.EventHandler(this.richTextBox1_DoubleClick_1);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(828, 545);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(305, 84);
            this.listBox1.TabIndex = 55;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(829, 524);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(169, 17);
            this.label10.TabIndex = 56;
            this.label10.Text = "Выберите локальный ip:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 46);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(153, 17);
            this.label11.TabIndex = 86;
            this.label11.Text = "Сетевые номера с/по:";
            // 
            // numAddrTo
            // 
            this.numAddrTo.Location = new System.Drawing.Point(111, 66);
            this.numAddrTo.Maximum = new decimal(new int[] {
            247,
            0,
            0,
            0});
            this.numAddrTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAddrTo.Name = "numAddrTo";
            this.numAddrTo.Size = new System.Drawing.Size(85, 22);
            this.numAddrTo.TabIndex = 85;
            this.numAddrTo.Value = new decimal(new int[] {
            247,
            0,
            0,
            0});
            // 
            // numAddrFrom
            // 
            this.numAddrFrom.Location = new System.Drawing.Point(18, 66);
            this.numAddrFrom.Maximum = new decimal(new int[] {
            247,
            0,
            0,
            0});
            this.numAddrFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAddrFrom.Name = "numAddrFrom";
            this.numAddrFrom.Size = new System.Drawing.Size(85, 22);
            this.numAddrFrom.TabIndex = 84;
            this.numAddrFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 659);
            this.Controls.Add(this.btnGenerateTable);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numAddrTo);
            this.Controls.Add(this.numAddrFrom);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnAddressRead);
            this.Controls.Add(this.cbJustRead);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxPollOffline);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownComReadTimeout);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.comboBoxComPorts);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonPoll);
            this.Controls.Add(this.buttonPing);
            this.Controls.Add(this.dgv1);
            this.Controls.Add(this.buttonImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Заголовок генерируется автоматически";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownComReadTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownComWriteTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAddrTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAddrFrom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.OpenFileDialog ofd1;
        private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.SaveFileDialog sfd1;
        private System.Windows.Forms.Button buttonPoll;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.ComboBox comboBoxComPorts;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.NumericUpDown numericUpDownComReadTimeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxPollOffline;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnIndPollDaily;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBoxTcp;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownComWriteTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbJustRead;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnIndPollCurrent;
        private System.Windows.Forms.Button btnIndPollInfo;
        private System.Windows.Forms.CheckBox cbFromFileTcp;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnReadHalfs;
        private System.Windows.Forms.RichTextBox btnAddressRead;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnIndPollMonthly;
        private System.Windows.Forms.Button btnIpPreset;
        private System.Windows.Forms.Button btnGetErrDescr;
        private System.Windows.Forms.TextBox tbErrCode;
        private System.Windows.Forms.Button btnAddressSet;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numAddrTo;
        private System.Windows.Forms.NumericUpDown numAddrFrom;
        private System.Windows.Forms.Button btnGenerateTable;
        private System.Windows.Forms.Button btnAddrClear;
    }
}

