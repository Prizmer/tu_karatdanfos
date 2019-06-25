﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

using Drivers.LibMeter;
using PollingLibraries.LibPorts;
using System.Threading;



namespace Drivers.KaratDanfosDriver
{
    public enum SubSystemErrors
    {
        ErrorVmin = 0x00000001, // ОШИБКА — расход ниже минимального порога
        ErrorVmax = 0x00000002, // ОШИБКА — расход выше максимального порога
        ErrorV = 0x00000004, // ОШИБКА — отсутствие напряжения питания у расходомера, получающего питание от сети ~220В
        ErrorGvtp = 0x00000008, // ОШИБКА — по полученным данным невозможно рассчитать массу теплоносителя
        WarningVmin = 0x00000010, // ПРЕДУПРЕЖДЕНИЕ — расход ниже минимального порога
        WarningVmax = 0x00000020, // ПРЕДУПРЕЖДЕНИЕ — расход выше максимального порога
        WarningV = 0x00000040, // ПРЕДУПРЕЖДЕНИЕ — отсутствие напряжения питания у расходомера, получающего питание от сети ~220В
        WarningG = 0x00000080, // ПРЕДУПРЕЖДЕНИЕ — небаланс часовой разности масс
        ErrorTmin = 0x00000100, // ОШИБКА — температура ниже минимального порога
        ErrorTmax = 0x00000200, // ОШИБКА — температура выше максимального порога
        ErrorT = 0x00000400, // ОШИБКА по температуре — обрыв в датчике температуры
        WarningTmin = 0x00001000, // ПРЕДУПРЕЖДЕНИЕ — температура ниже минимального порога
        WarningTmax = 0x00002000, // ПРЕДУПРЕЖДЕНИЕ — температура выше максимального порога
        WarningT = 0x00004000, // ПРЕДУПРЕЖДЕНИЕ по температуре — обрыв в датчике температуры
        ErrorPmin = 0x00010000, // ОШИБКА — давление ниже минимального порога
        ErrorPmax = 0x00020000, // ОШИБКА — давление выше максимального порога
        ErrorP = 0x00040000, // ОШИБКА по давлению — обрыв в датчике давления
        WarningPmin = 0x00100000, // ПРЕДУПРЕЖДЕНИЕ — давление ниже минимального порога
        WarningPmax = 0x00200000, // ПРЕДУПРЕЖДЕНИЕ — давление выше максимального порога
        WarningP = 0x00400000, // ПРЕДУПРЕЖДЕНИЕ по давлению — обрыв в датчике давления
        ErrorQgtp = 0x04000000, // ОШИБКА — по полученным данным невозможно рассчитать энергию теплоносителя
        WarningQ = 0x40000000 // ПРЕДУПРЕЖДЕНИЕ — рассчитанная энергия отрицательна
    }

    public class KaratDanfosDriver : CMeter, IMeter
    {
        enum KaratFunctions
        {
            Read = 0x04,
            Write = 0x06,

            User = 0x42
        }

        enum KaratErrors
        {
            ERR_WRONG_FUNCTION = 0x01,
            ERR_WRONG_INITIAL_REGISTER = 0x02,
            ERR_WRONG_REGISTERS_COUNT_VAL = 0x03,
            ERR_CANT_CONFIGURE_METER_IN_WORKING_MODE = 0x04,
            ERR_CANT_MODIFY_CALIBRATION_CONSTANTS = 0x05,
            ERR_WRITE_FUNCTION_PAYLOAD_LENGTH_IS_LESS_THAN_SHOULD_BE = 0x06,
            ERR_ATTEMPT_TO_WRITE_IN_WORKING_MODE_WITH_WRONG_PASSWORD = 0x07
        }

        enum ArchiveTypeRegister 
        {
            DAY = 0x0010,
            MONTH = 0x0020
        }

        // порядковый номер первого байта полезных данных в ответе прибора
        const byte ANSW_DATA_OFFSET = 3;

        // массивы для кэширования данных
        private Dictionary<short, byte[]> archiveCash = new Dictionary<short, byte[]>();
        private byte[] archiveConfigurationCash = new byte[0];
        private string meterSoftwareCashed = "";

        private bool checkLittleEndianAndConvert(ref byte[] payloadBytes)
        {
            bool IsLittleEndian = BitConverter.IsLittleEndian;
            if (IsLittleEndian)
                Array.Reverse(payloadBytes);
            return IsLittleEndian;
        }
        private byte[] makeCmd(KaratFunctions func, ushort initialRegister, byte[] payload)
        {
            List<byte> resCmd = new List<byte>();

            resCmd.Add((byte)this.m_address);
            resCmd.Add((byte)func);

            byte[] initialRegisterBytes = BitConverter.GetBytes(initialRegister);
            checkLittleEndianAndConvert(ref initialRegisterBytes);
            resCmd.AddRange(initialRegisterBytes);

            if (func == KaratFunctions.Read)
            {
                short registersCnt = 0x0; // изначально - только начальный регистр
                registersCnt += (short)(payload.Length / 2);

                byte[] registersCntBytes = BitConverter.GetBytes(registersCnt);
                checkLittleEndianAndConvert(ref registersCntBytes);
                resCmd.AddRange(registersCntBytes);
            }
            else if (func == KaratFunctions.Write)
            {
                //short registersCnt = 0x0000; // не считаем начальный регистр?!
                //registersCnt += (short)(payload.Length / 2);

                //byte[] registersCntBytes = BitConverter.GetBytes(registersCnt);
                //checkLittleEndianAndConvert(ref registersCntBytes);
                //resCmd.AddRange(registersCntBytes);

                //byte bytesCnt = (byte)(payload.Length);
                //resCmd.Add(bytesCnt);

                resCmd.AddRange(payload);
            }
            else
            {
                // пользовательская функция 42 - заразервировано
            }


   
            // CRC
            byte[] crc16 = this.CRC16(resCmd.ToArray(), resCmd.Count);
            resCmd.AddRange(crc16);

            return resCmd.ToArray();
        }

        private bool isAnswerDataOk(byte[] data, ref string errDescription)
        {
            if (data.Length < 6)
            {
                errDescription = "Длина массива данных меньше 6: " + BitConverter.ToString(data);
                return false;
            }

            if (!this.checkIsCRCCorrect(data))
            {
                errDescription = "Ошибка проверки CRC: " + BitConverter.ToString(data);
                return false;
            }

            if (data[1] == 0x83 || data[1] == 0x90)
            {
                byte errDescriptionByte = data[2];
                KaratErrors kr = (KaratErrors)errDescriptionByte;
                string errDescriptionByteStr = Enum.GetName(typeof(KaratErrors), kr);

                errDescription = String.Format("В ответе ошибка, функция dec {0}, описание: {1}, байты: {2}",
                    data[1], errDescriptionByteStr, BitConverter.ToString(data));

                return false;
            }


            errDescription = "Ошибок нет";
            return true;
        }
        private bool checkIsCRCCorrect(byte[] data)
        {

            if (data.Length > 4)
            {
                byte[] crc16 = this.CRC16(data, data.Length - 2);
                if (crc16.Length == 2 && crc16[0] == data[data.Length - 2] && crc16[1] == data[data.Length - 1])
                    return true;
            }

            return false;
        }
        private byte[] CRC16(byte[] Arr, int length)
        {
            byte[] CRC = new byte[2];
            UInt16 B = 0xFFFF;
            int j = 0;
            int i;
            byte b;
            bool f;

            unchecked
            {
                do
                {
                    i = 0;
                    b = Arr[j];
                    B = (UInt16)(B ^ (UInt16)b);
                    do
                    {
                        f = (((B) & (1)) == 1);
                        B = (UInt16)(B / 2);
                        if (f)
                        {
                            B = (UInt16)(B ^ (0xA001));
                        }
                        i++;
                    } while (i < 8);
                    j++;
                } while (j < length);
                CRC[0] = (byte)(B);
                CRC[1] = (byte)(B >> 8);
            }
            return CRC;
        }

        public bool getMeterSoftware(ref string softwareString)
        {
            const ushort REG_READ_VERSION = 0x0054;
            byte answerBytesCnt = 16;

            byte[] cmd = this.makeCmd(KaratFunctions.Read, REG_READ_VERSION, new byte[answerBytesCnt]);

            byte[] incommingData = new byte[1];
            int resWriteRead = m_vport.WriteReadData(FindPacketSignature, cmd, ref incommingData, cmd.Length, -1);

            string errDescription = "";
            if (!isAnswerDataOk(incommingData, ref errDescription))
            {
                this.WriteToLog("getMeterModel: " + errDescription);
                return false;
            }

            // приходят младшим байтом вперед (little endian)
            byte[] modelBytes = new byte[answerBytesCnt];
            try
            {
                Array.Copy(incommingData, ANSW_DATA_OFFSET, modelBytes, 0, modelBytes.Length);
            } catch (Exception ex)
            {
                WriteToLog("getMeterSoftware: переполнение массива: " + ex.Message);
                WriteToLog($"getMeterSoftware: доп информация, answerBytesCnt={answerBytesCnt}, пришедшие байты: {BitConverter.ToString(incommingData)}");
                return false;
            }
            

            // если в системе данные хранятся старшим байтом вперед , то сделаем реверс
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(modelBytes);

            // конвертнем в short

            int modelInt = BitConverter.ToInt16(modelBytes, 0);
            softwareString = "" + modelInt;

            softwareString = ASCIIEncoding.ASCII.GetString(modelBytes);
            softwareString = softwareString.TrimEnd('\0');

            this.meterSoftwareCashed = softwareString;

            return true;
        }

        public bool getMeterSerialNumber(ref string serialNumber)
        {
            // чтение заводских констант
            const ushort REG_READ_SN = 0x0044;
            byte[] cmd = this.makeCmd(KaratFunctions.Read, REG_READ_SN, new byte[32]);

            byte[] incommingData = new byte[1];
            int resWriteRead = m_vport.WriteReadData(FindPacketSignature, cmd, ref incommingData, cmd.Length, -1);

            string errDescription = "";
            if (!isAnswerDataOk(incommingData, ref errDescription))
            {
                this.WriteToLog("getMeterSerialNumber: " + errDescription);
                return false;
            }

            // приходят младшим байтом вперед (little endian)
            byte[] serialBytes = new byte[32];
            try
            {
                Array.Copy(incommingData, ANSW_DATA_OFFSET, serialBytes, 0, serialBytes.Length);
            }
            catch (Exception ex)
            {
                return false;
            }
           

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(serialBytes);
            

            serialNumber = ASCIIEncoding.ASCII.GetString(serialBytes);
            serialNumber = serialNumber.TrimEnd('\0');

            if (serialNumber.Length > 0)
                return true;
            else
                return false;
        }


        public bool setMeterAddress(ushort decAddr, bool bClear = false)
        {
            byte[] addrArr = BitConverter.GetBytes(decAddr);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(addrArr);

           // byte[] addrArr = { 0x0, 0x02 };

            ushort REG_READ_ADDR = 0xF001;
            if (bClear)
                REG_READ_ADDR = 0xF000;

            byte[] cmd = this.makeCmd(KaratFunctions.Write, REG_READ_ADDR, addrArr);

            byte[] incommingData = new byte[1];
            int resWriteRead = m_vport.WriteReadData(FindPacketSignature, cmd, ref incommingData, cmd.Length, -1);

    
            string errDescription = "";
            if (!isAnswerDataOk(incommingData, ref errDescription))
            {
                this.WriteToLog("setAddrErr: " + errDescription);
                return false;
            }

            return true;
        }

        private bool getArchiveRecordConfiguration(out byte[] configurationBytes)
        {
            configurationBytes = new byte[0];

            // чтение конфигурации архива
            const ushort REG_READ_VERSION = 0x0106;
            byte[] cmd = this.makeCmd(KaratFunctions.Read, REG_READ_VERSION, new byte[0]);

            byte[] incommingData = new byte[1];
            int resWriteRead = m_vport.WriteReadData(FindPacketSignature, cmd, ref incommingData, cmd.Length, -1);

            string errDescription = "";
            if (!isAnswerDataOk(incommingData, ref errDescription))
            {
                this.WriteToLog("getArchiveRecordConfiguration ответ некорректен: " + errDescription);
                return false;
            }

            List<byte> configurationBytesList = new List<byte>();
            for (int i = 4; i < incommingData.Length - 2; i++)
            {
                byte tmpVal = incommingData[i];
                // если байт >= F0 - это признак конца массива, дальше можно не смотреть
                if (tmpVal < 0xF0 && tmpVal > 0)
                    configurationBytesList.Add(tmpVal);
            }

            configurationBytes = configurationBytesList.ToArray();
            return true;
        }



        private bool setArchiveRequestDate(DateTime dt)
        {
            // запишем дату в нужный регистр
            const ushort REG_SET_DATE = 0x0060;

            // час, день, месяц, год
            byte[] dateArr = { (byte)dt.Hour, (byte)dt.Day, (byte)dt.Month, byte.Parse(dt.ToString("yy")) };
            byte[] cmdSetDate = this.makeCmd(KaratFunctions.Write, REG_SET_DATE, dateArr);

            byte[] incommingData = new byte[1];
            int resWriteRead = m_vport.WriteReadData(FindPacketSignature, cmdSetDate, ref incommingData, cmdSetDate.Length, -1);

            string errDescription = "";
            if (!isAnswerDataOk(incommingData, ref errDescription))
            {
                this.WriteToLog("getArchiveRecord: " + errDescription);
                return false;
            }

            return true;
        }


        private bool getArchiveRecord(DateTime dt, ArchiveTypeRegister archType, ref byte[] rawRecordBytes)
        {
            // запишем дату в регистр
            if (!this.setArchiveRequestDate(dt))
                return false;

            // запросим нужную запись
            ushort register = (ushort)archType;
            byte[] cmd = this.makeCmd(KaratFunctions.Read, register, new byte[0]);

            byte[] incommingData = new byte[1];
            int resWriteRead = m_vport.WriteReadData(FindPacketSignature, cmd, ref incommingData, cmd.Length, -1);

            string errDescription = "";
            if (!isAnswerDataOk(incommingData, ref errDescription))
            {
                this.WriteToLog("getArchiveRecord: " + Enum.GetName(typeof(ArchiveTypeRegister), archType) + ": " + errDescription);
                return false;
            }

            // проверим существует ли запись вообще перед тем как ее сохранять в кэш
            byte[] headerData = new byte[18];
            byte[] recordDateIdBytes = new byte[5];
            try
            {
                Array.Copy(incommingData, 3, headerData, 0, 18);
                Array.Copy(headerData, 13, recordDateIdBytes, 0, recordDateIdBytes.Length);
            }
            catch (Exception ex)
            {
                WriteToLog("getArchiveRecord, array.copy: " + ex.Message);
                return false;
            }

            if (recordDateIdBytes[0] == 0xFF && recordDateIdBytes[recordDateIdBytes.Length - 1] == 0xFF)
            {
                // запись идентифицируется датой. Если поля даты 0xFF - запись не существует и не удалось 
                // найти хотябы что-то по близости. Мануал стр. 38
                string msg = String.Format("запись идентифицируется датой. Если поля даты 0xFF - запись не существует и не удалось найти хотябы что-то по близости. Байты: {0}", BitConverter.ToString(recordDateIdBytes));
                WriteToLog("getArchiveRecord, array.copy: " + msg);
                return false;
            }


            // кэшируем полностью вместе с доп-инфой
            this.archiveCash[(short)archType] = incommingData;
            rawRecordBytes = incommingData;

            return true;
        }


        private bool getCurrentValue(short regNumber, byte bitCnt, ref float value)
        {
            int byteCnt = bitCnt / 8;

            if (regNumber == 0) return false;
            value = -1;

            // в документации указан номер регистра, адрес регистра = номер регистра - 1
            ushort register = (ushort)(regNumber - 1);
            byte[] cmd = this.makeCmd(KaratFunctions.Read, register, new byte[byteCnt]);

            byte[] incommingData = new byte[1];
            int resWriteRead = m_vport.WriteReadData(FindPacketSignature, cmd, ref incommingData, cmd.Length, -1);

            string errDescription = "";
            if (!isAnswerDataOk(incommingData, ref errDescription))
            {
                this.WriteToLog("getCurrentValue: " + errDescription);
                return false;
            }

            // приходят младшим байтом вперед (little endian)
            byte[] dataBytes = new byte[byteCnt];
            try
            {
                Array.Copy(incommingData, ANSW_DATA_OFFSET, dataBytes, 0, dataBytes.Length);
            }
            catch (Exception ex)
            {
                WriteToLog("getCurrentValue: переполнение массива: " + ex.Message);
                WriteToLog($"getCurrentValue: доп информация, answerBytesCnt={byteCnt}, пришедшие байты: {BitConverter.ToString(incommingData)}");
                return false;
            }

            if (BitConverter.IsLittleEndian)
                Array.Reverse(dataBytes);


            // в зависимости от номера регистра, выполним соответствующее преобразование
            if (regNumber < 17)
            {
                double tmp = BitConverter.ToInt64(dataBytes, 0);
 
                //sint64
                if (regNumber == 1 || regNumber == 5)
                {
                    // объем, приходит в 10^-12 м3               
                    double volInitial = BitConverter.ToInt64(dataBytes, 0);
                    double volInM3 = volInitial * Math.Pow(10, -12);

                    double result = Math.Round(volInM3, 4, MidpointRounding.AwayFromZero);
                    value = (float)result;
                }
                else if (regNumber == 9 || regNumber == 13)
                {
                    // энергия в Дж
                    double energyInJoules = BitConverter.ToInt64(dataBytes, 0);

                    // Джоулей в 1 Ккал (кило, мего, гига)
                    const double JOULE_IN_CCAL = 4184;
                    double energyInGCal = energyInJoules  * Math.Pow(10, -6) / JOULE_IN_CCAL;
 
                    // как лучше хранить в базе? Может лучше хранить ГДж (джоули в 10^-9)? Тогда показания со счетчика
                    // и показания в базе будут сравнимы

                    double result = Math.Round(energyInGCal, 4, MidpointRounding.AwayFromZero);
                    value = (float)result;
                }
                else
                {
                    value = -1;//BitConverter.ToUInt64(dataBytes, 0);
                    this.WriteToLog("getCurrentValue: запрошено значение 64 бит с адресом < 17. СО работает с 32 разрядами, поэтому конвертация может быть ошибочный. Значение не получено.");
                    return false;
                }
            }
            else if (regNumber >= 17 && regNumber < 31)
            {
                //float одинарный
                value = BitConverter.ToSingle(dataBytes, 0);

            }
            else if (regNumber >= 31 && regNumber < 53)
            {
                //uint64 || 32 || 16 
                switch (byteCnt)
                {
                    case 2:
                        {
                            value = BitConverter.ToUInt16(dataBytes, 0);
                            break;
                        }
                    case 4:
                        {
                            value = BitConverter.ToUInt32(dataBytes, 0);
                            break;
                        }
                    case 8:
                        {
                            value = -1;//BitConverter.ToUInt64(dataBytes, 0);
                            this.WriteToLog("getCurrentValue: запрошено значение 64 бит с адресом 31 - 47. СО работает с 32 разрядами, поэтому конвертация может быть ошибочный. Значение не получено.");
                            return false;
                            break;
                        }
                }

            }
            else
            {
                this.WriteToLog("getCurrentValue: запрошен не существующий параметр с начальным регистром/длиной байт: " + regNumber + "/" + byteCnt);
                return false;
            }

            if (value == -1)
            {
                this.WriteToLog("getCurrentValue: возможно произошла ошибка преобразования, параметр value = -1, начальным регистром/длиной байт: " + regNumber + "/" + byteCnt);
                return false;
            }

            return true;
        }

        // запись передается полностью (с заголовком и CRC), а данные конфигурации
        // в уже готовом виде
        private bool getArchiveRecordParamValue(byte param, byte[] recordDataRaw, byte[] cfgData, ref float val)
        {
            string errDescr = "";
            if (!isAnswerDataOk(recordDataRaw, ref errDescr))
            {
                WriteToLog("getArchiveRecordParamValue: счетчик сообщает об ошибке: " + errDescr);
                return false;
            }

            byte[] headerData = new byte[18];
            byte[] paramsData = new byte[220];

            try
            {
                Array.Copy(recordDataRaw, 3, headerData, 0, 18);
                Array.Copy(recordDataRaw, 21, paramsData, 0, 220);
            }
            catch (Exception ex)
            {
                WriteToLog("getArchiveRecordParamValue, array.copy1: " + ex.Message);
                return false;
            }
            // индекс параметра в массиве конфигурации архивной записи
            int idxParam = Array.IndexOf(cfgData, param);

            if (idxParam == -1)
            {
                WriteToLog("Запрашиваемый параметр не настроен в приборе и не приходит в массиве параметров dec: " + param);
                return false;
            }


            // float 4 bytes
            const int SIZE_OF_PARAM_VALUE = 4;
            byte[] valBytes = new byte[SIZE_OF_PARAM_VALUE];

            try
            {
                Array.Copy(paramsData, idxParam * SIZE_OF_PARAM_VALUE, valBytes, 0, valBytes.Length);
            }
            catch (Exception ex)
            {
                WriteToLog("getArchiveRecordParamValue, array.copy2: " + ex.Message);
                return false;
            }



            if (param >= 0x10 && param <= 0x9F)
            {

                // common params
                // TODO: little endian?
                val = BitConverter.ToSingle(valBytes, 0);

                if (val == -1)
                {
                    WriteToLog("getArchiveRecordParamValue: из архивной записи взяты некорректные байты: " + BitConverter.ToString(valBytes));
                    return false;
                }

            }
            else if (param >= 0xA0 && param <= 0xAF)
            {
                // date and time 4 archive record
                // 4 bytes час (1 байт), день (1 байт), месяц (1 байт), год — десятки и единицы лет (1 байт).
                DateTime tmpDt = new DateTime(2000 + valBytes[3], valBytes[2], valBytes[1], valBytes[0], 0, 0);
                val = tmpDt.Ticks;
            }
            else if (param >= 0xD1 && param <= 0xD6)
            {
                // наработки
                val = BitConverter.ToInt32(valBytes, 0);

                if (val == -1)
                {
                    WriteToLog("getArchiveRecordParamValue: из архивной записи взяты некорректные байты: " + BitConverter.ToString(valBytes));
                    return false;
                }
                // TODO: little endian?

                if (this.meterSoftwareCashed.Length > 0 && this.meterSoftwareCashed != "308")
                {
                    // Для приборов Карат - 306, Карат - 307 значения параметров 0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 
                    // имеют тип LONG и обозначают время наработки каждого параметра в минутах.При отображении следует 
                    // параметр переводить в часы(делить на 60).
                    val /= 60.0f;
                }
                else
                {
                    // Для приборов Карат - 308 значения параметров 0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, имеют тип LONG и 
                    // обозначают наработку каждого параметра в циклах измерения прибора. При отображении следует параметр 
                    // переводить в часы(делить на 360).
                    val /= 360.0f;
                }
            } else if (param >=  0xC0 && param <= 0xCF)
            {

                // errors
                // мануал страница 18
                val = (float)BitConverter.ToInt32(valBytes, 0);

                if (val == -1)
                {
                    WriteToLog("getArchiveRecordParamValue: из архивной записи взяты некорректные байты: " + BitConverter.ToString(valBytes));
                    return false;
                }

            } else
            {
                val = BitConverter.ToUInt32(valBytes, 0);

                if (param == 0xB0){
                    // если наработки подсистем, то надо разменить на 60
                    val /= 60;
                }
            }

            if (val == -1)
            {
                WriteToLog("getArchiveRecordParamValue: полученное значение = -1 при запросе параметр " + param.ToString("X"));
                return false;
            }

            WriteToLog("getArchiveRecordParamValue: запрос параметра " + param.ToString("X"));
            WriteToLog("getArchiveRecordParamValue: взятые 4 байта из архивной записи: " + BitConverter.ToString(valBytes));
                WriteToLog("getArchiveRecordParamValue: архив: " + BitConverter.ToString(recordDataRaw));
                WriteToLog("getArchiveRecordParamValue: архив, без заголовка: " + BitConverter.ToString(paramsData));
                WriteToLog("getArchiveRecordParamValue: индекс в масс.конфигурации/в полезных данных: " + idxParam + "/" + idxParam * SIZE_OF_PARAM_VALUE);
                WriteToLog("getArchiveRecordParamValue: конфиг: " + BitConverter.ToString(cfgData));

            return true;
        }

        private bool ReadArchiveValuesCommon(ArchiveTypeRegister archTypeReg, DateTime dt, ushort param, ushort tarif, ref float recordValue)
        {
            recordValue = -1f;

            if (this.archiveConfigurationCash.Length == 0)
            {
                bool res = this.getArchiveRecordConfiguration(out this.archiveConfigurationCash);
                if (!res)
                {
                    WriteToLog("ReadArchiveValuesCommon: getArchiveRecordConfiguration вернул false");
                    return res;
                }
            }

            // после получени конфигурации (реального или из кэша), запросим требуемый параметр
            short idxInCashArr = (short)archTypeReg;
            string archTypeRegStr = Enum.GetName(typeof(ArchiveTypeRegister), archTypeReg);


            byte[] cashedArr = this.archiveCash[idxInCashArr];




            byte[] rawRecordBytes = new byte[0];
            if (cashedArr.Length == 0)
            {
                if (!this.getArchiveRecord(dt, archTypeReg, ref rawRecordBytes))
                {
                    string msg = String.Format("ReadArchiveValuesCommon: getArchiveRecord вернул false: dt={0}, тип архива={1} , параметр={2}", dt.ToString(), archTypeRegStr, param);
                    WriteToLog(msg);
                    return false;
                }
            }
            else
            {
                rawRecordBytes = cashedArr;
            }

            // распарсим и выдадим нужное значение

            if (!this.getArchiveRecordParamValue((byte)param, rawRecordBytes, this.archiveConfigurationCash, ref recordValue))
            {
                WriteToLog("ReadArchiveValuesCommon: getArchiveRecordParamValue вернул false");
                return false;
            }

            return true;
        }

        #region Реализация методов интерфейса IMeter

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="address"></param>
        /// <param name="password"></param>
        public void Init(uint address, string pass, VirtualPort data_vport)
        {
            // всегда делай так, нужно для логгирования
            this.m_vport = data_vport;

            //if (address == 0)
            //{
            //    WriteToLog("Init: Не возможно проинициализировать драйвер karat30X с адресом 0");
            //    return;
            //}

            this.WriteToLog("Драйвер карата проинициализирован");

            // проинициализируем массив в котором будет кэш
            int[] archiveTypeRegisterVals = (int[])Enum.GetValues(typeof(ArchiveTypeRegister)); 
            foreach (int typeReg in archiveTypeRegisterVals)
            {
                this.archiveCash[(short)typeReg] = new byte[0];
            }
            
            this.m_address = address;

        }

        private int FindPacketSignature(Queue<byte> queue)
        {
            byte[] arr = queue.ToArray();
            Array.Reverse(arr);

            if (checkIsCRCCorrect(arr)) return 1;

            return 0;
        }

        public bool ReadSerialNumber(ref string serial_number)
        {
            return this.getMeterSerialNumber(ref serial_number);
        }

        /// <summary>
        /// открытие канала связи
        /// </summary>
        /// <returns></returns>
        public bool OpenLinkCanal()
        {
            string meterModelStr = "";
            if (this.getMeterSoftware(ref meterModelStr) && meterModelStr.Length > 0)
            {
                return true;
            } 

            return false;
        }


        /// <summary>
        /// Чтение текущих показаний
        /// </summary>
        /// <param name="tarif">0 - по сумме тарифов, 1 - по 1му тарифу, и т.д.</param>
        /// <param name="recordValue"></param>
        /// <returns></returns>
        public bool ReadCurrentValues(ushort param, ushort tarif, ref float recordValue)
        {
            return getCurrentValue((short)param, (byte)tarif, ref recordValue);
        }

        /// <summary>
        /// Чтение показаний на начало текущих суток
        /// </summary>
        /// <param name="date_time"></param>
        /// <param name="recordValue"></param>
        /// <returns></returns>
        public bool ReadDailyValues(DateTime dt, ushort param, ushort tarif, ref float recordValue)
        {
            // карат держит данные не на начало суток, а на конец предыдущих суток
            //DateTime newDt = new DateTime(dt.Ticks).AddDays(-1);
            //return this.ReadArchiveValuesCommon(ArchiveTypeRegister.DAY, newDt, param, tarif, ref recordValue);

            return ReadCurrentValues(param, tarif, ref recordValue);
        }

        /// <summary>
        /// Чтение показаний на начало месяца
        /// </summary>
        /// <param name="tarif"></param>
        /// <param name="month"></param>
        /// <param name="recordValue"></param>
        /// <returns></returns>
        public bool ReadMonthlyValues(DateTime dt, ushort param, ushort tarif, ref float recordValue)
        {
            // карат держит данные не на начало месяца, а на конец предыдущего
            DateTime newDt = new DateTime(dt.Ticks).AddDays(-1);
            return this.ReadArchiveValuesCommon(ArchiveTypeRegister.MONTH, dt, param, tarif, ref recordValue);
        }


        #endregion

        #region Вспомогательные функции (со временем нужно вынести в отдельную библиотеку)

        /// <summary>
        /// перевод из DEC в HEX
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private byte dec2hex(byte value)
        {
            return Convert.ToByte((value >> 4) * 10 + (value & 0xF));
        }

        /// <summary>
        /// перевод из HEX в DEC
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private byte hex2dec(byte value)
        {
            return Convert.ToByte(((value / 10) << 4) + (value % 10));
        }


        #endregion

        #region Неиспользуемые методы

        // поиск получасовок, обертка, в которой выбирается метод поиска
        public bool ReadPowerSlice(DateTime dt_begin, DateTime dt_end, ref List<RecordPowerSlice> listRPS, byte period)
        {
            return false;
        }

        public bool SyncTime(DateTime dt)
        {
            return false;
        }

        public bool ReadDailyValues(uint recordId, ushort param, ushort tarif, ref float recordValue)
        {
            return false;
        }

        public bool ReadPowerSlice(ref List<SliceDescriptor> sliceUniversalList, DateTime dt_end, SlicePeriod period)
        {
            return false;
        }

        // возвращает дату последней инициализации массива срезов
        public bool ReadSliceArrInitializationDate(ref DateTime lastInitDt)
        {
            return false;
        }

        public List<byte> GetTypesForCategory(CommonCategory common_category)
        {
            return new List<byte>();
        }

        #endregion

    }
}