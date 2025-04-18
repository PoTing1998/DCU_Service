﻿using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DCU.ProcMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCU_Frame;

namespace ASI.Wanda.DCU.TaskSDU
{
    public class ProcTaskSDU : ProcBase
    {
        #region constructor
        static int mSEQ = 0; // 計算累進發送端的次數  
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial = null;
        static string Station_ID = ConfigApp.Instance.GetConfigSetting("Station_ID");
        static string _mDU_ID = DU_ID.LG01_SDU_05.ToString();
        /// <summary>
        /// 火災相關訊息
        /// </summary>
        private static class FireAlarmMessages
        {
            public static readonly string CheckChinese = Get("FireDetectorCheckInProgressChinese");
            public static readonly string CheckEnglish = Get("FireDetectorCheckInProgressEnglish");
            public static readonly string EmergencyChinese = Get("FireEmergencyEvacuateCalmlyChinese");
            public static readonly string EmergencyEnglish = Get("FireEmergencyEvacuateCalmlyEnglish");
            public static readonly string ClearedChinese = Get("FireAlarmClearedChinese");
            public static readonly string ClearedEnglish = Get("FireAlarmClearedEnglish");
            public static readonly string DetectorChinese = Get("FireDetectorClearConfirmedChinese");
            public static readonly string DetectorEnglish = Get("FireDetectorClearConfirmedEnglish");

            private static string Get(string key) => ConfigApp.Instance.GetConfigSetting(key);
        }

        #endregion
        /// <summary>
        /// 處理DMD模組執行程序所收到之訊息 
        /// </summary>
        /// <param name="pLabel"></param>
        /// <param name="pBody"></param>
        /// <returns></returns>
        public override int ProcEvent(string pLabel, string pBody)
        {

            if (pLabel == MSGFinish.Label)
            {
                return 0;
            }
            else if (pLabel == ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD.Label)
            {
                return ProMsgFromDMD(pBody);
            }
            else if (pLabel == PA.ProcMsg.MSGFromTaskPA.Label)
            {
                return ProMsgFromPA(pBody);
            }
            else if (pLabel == ProcMsg.MSGFromTaskPDU.Label)
            {
                return 0;
            }
            return base.ProcEvent(pLabel, pBody);
        }


        /// <summary>
        /// 啟始處理DCU模組執行程序
        /// </summary>
        /// <param name="pComputer"></param>
        /// <param name="pProcName"></param>
        /// <returns></returns>  
        public override int StartTask(string pComputer, string pProcName)
        {
            mTimerTick = 30;
            _mProcName = "TaskSDU";
          
            string dbIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string dbPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string dbName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string dbUserID = "postgres";
            string dbPassword = "postgres";
            string currentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");

         
            var iComPort = ConfigApp.Instance.GetConfigSetting("SDUComPort"); ;
            var iBaudrate = ConfigApp.Instance.GetConfigSetting("SDUBaudrate"); ;
            _mSerial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
            string connectionString = $"PortName=COM{iComPort};BaudRate={iBaudrate};DataBits=8;StopBits=One;Parity=None";
            _mSerial.ConnectionString = connectionString;
            _mSerial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
            _mSerial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);
            int result = -1; // Default to an error state
            try
            {
                result = _mSerial.Open();
                if (result != 0)
                {
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, " 打開串口失敗  port open failed");
                    return result; // Return immediately if the serial port failed to open
                }

                // 初始化資料庫連線
                if (!ASI.Wanda.DCU.DB.Manager.Initializer(dbIP, dbPort, dbName, dbUserID, dbPassword, currentUserID))
                {
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, $"資料庫連線失敗! {dbIP}:{dbPort};userid={dbUserID}");
                    return -1; // Return immediately if the database initialization failed
                }
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"例外發生! {ex.Message}");
                return -1; // Return immediately if any exception occurs
            }

            return base.StartTask(pComputer, pProcName);
        }

        /// <summary>
        /// 處理TaskDMD的訊息
        /// </summary>
        private int ProMsgFromDMD(string pMessage)
        {
            try
            {
                ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD mSGFromTaskDMD = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD(new MSGFrameBase(""));
                if (mSGFromTaskDMD.UnPack(pMessage) > 0)
                {
                    try
                    {
                        string sJsonData = mSGFromTaskDMD.JsonData;
                        string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "JsonObjectName");
                        var taskCDUHelper = new ASI.Wanda.DCU.TaskSDU.TaskSDUHelper(_mProcName, _mSerial);

                        switch (sJsonObjectName)
                        {
                            case ASI.Wanda.DCU.TaskSDU.Constants.SendPreRecordMsg: //預錄訊息
                                string sSeatID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "seatID");
                                string msg_id = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "msg_id");
                                string dbName1 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName1");
                                string dbName2 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName2");
                                string target_du = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "target_du");

                                ASI.Lib.Log.DebugLog.Log(_mProcName, $"收到來自TaskDMD的訊息，mSGFromTaskDMD:{mSGFromTaskDMD.JsonData};SeatID:{sSeatID}；MsgID:{msg_id}；target_du:{target_du}; dbName1 :{dbName1};dbName2 :{dbName2}");
                                string result = "";
                                ASI.Lib.Log.DebugLog.Log(_mProcName, "處理 dmd_pre_record_message");
                                //傳送到面板上 
                                taskCDUHelper.SendMessageToDisplay(target_du, dbName1, dbName2, out result);
                                ASI.Lib.Log.DebugLog.Log(_mProcName, "處理 dmd_pre_record_message" + result);
                                break;

                            case ASI.Wanda.DCU.TaskSDU.Constants.SendInstantMsg: //即時訊息
                                break;
                            case ASI.Wanda.DCU.TaskSDU.Constants.SendPowerTimeSetting:
                                taskCDUHelper.PowerSetting(Station_ID);
                                break;
                            case "節能模式開啟":
                               
                                break;
                            case "節能模式關閉":
                             
                                break;
                        }


                    }
                    catch (Exception ex)
                    {
                        ASI.Lib.Log.ErrorLog.Log(_mProcName, ex.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, ex);
            }

            return -1;
        }

        private int ProMsgFromPA(string pMessage)
        {
            string sRcvTime = System.DateTime.Now.ToString("HH:mm:ss.fff");
            try
            {
                ASI.Wanda.DCU.ProcMsg.MSGFromTaskPA MSGFromTaskPA = new ProcMsg.MSGFromTaskPA(new MSGFrameBase(""));
                if (MSGFromTaskPA.UnPack(pMessage) > 0)
                {
                    var sJsonData = MSGFromTaskPA.JsonData;
                    ASI.Lib.Log.DebugLog.Log(_mProcName + " received a message from TaskPA ", sJsonData); // Log the received message 
                    // 將JSON資料轉換為位元組陣列和再轉回十六進位字串的代碼已移除  
                    // 假設sJsonData已經是十六進位字串格式，直接解析
                    var sHexString = sJsonData;
                    byte[] dataBytes = HexStringToBytes(sJsonData);
                    if (dataBytes.Length >= 10) // 確保有足夠長度的陣列
                    {
                        ProcessDataBytes(dataBytes); 
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log($"{_mProcName} dataBytes length less than 10", sHexString);
                    }
                    if (dataBytes.Length >= 3)
                    {
                        ProcessByteAtIndex2(dataBytes, sRcvTime, sJsonData);
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log($"{_mProcName} dataBytes length less than 3", sJsonData);
                    }
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, ex); // 記錄例外情況 
            }

            return -1;
        }
        #region private Method
        /// <summary>
        /// 計算LRC
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private byte calculateLRC(byte[] text)
        {
            byte xor = 0;
            // if no data then done  
            if (text.Length <= 0)
                return 0;
            // incorporate remaining bytes into the value  
            for (int i = 0; i < text.Length; i++)
                xor ^= text[i];
            return xor;
        }

        public static byte[] HexStringToBytes(string hex)
        {
            var mProcName = "HexStringToBytes";
            try
            {
                // 移除所有空格和無效字符
                hex = hex.Replace(" ", "").Replace("\"", "").ToUpper();

                // 打印原始的十六進位字串
                ASI.Lib.Log.DebugLog.Log(mProcName, "HexStringToBytes 轉換前的字串: " + hex);

                if (hex.Length % 2 != 0)
                {
                    throw new ArgumentException("十六進位字串的長度必須是偶數");
                }
                // 確保所有字符都是有效的十六進位字符
                foreach (char c in hex)
                {
                    if (!Uri.IsHexDigit(c))
                    {
                        throw new ArgumentException($"十六進位字串包含無效字符: {c}");
                    }
                }

                // 創建一個字節陣列，長度為字串長度的一半
                byte[] bytes = new byte[hex.Length / 2];

                // 循環遍歷十六進位字串，將每對十六進位字符轉換為一個字節
                for (int i = 0; i < hex.Length; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                }

                return bytes;
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, "HexStringToBytes 轉換錯誤: " + ex.ToString());
                return null;
            }
        }
        private void ProcessDataBytes(byte[] dataBytes)
        {
            byte dataByteAtIndex8 = dataBytes[8];
            var taskUPDHelper = new ASI.Wanda.DCU.TaskSDU.TaskSDUHelper(_mProcName, _mSerial);
            switch (dataByteAtIndex8)
            {
                case 0x81:
                    taskUPDHelper.SendMessageToUrgnt(FireAlarmMessages.CheckChinese, FireAlarmMessages.CheckEnglish, 81);
                    break;
                case 0x82:
                    taskUPDHelper.SendMessageToUrgnt(FireAlarmMessages.EmergencyChinese, FireAlarmMessages.EmergencyEnglish, 82);
                    break;
                case 0x83:
                    taskUPDHelper.SendMessageToUrgnt(FireAlarmMessages.ClearedChinese, FireAlarmMessages.ClearedEnglish, 83);
                    break;
                case 0x84:
                    taskUPDHelper.SendMessageToUrgnt(FireAlarmMessages.DetectorChinese, FireAlarmMessages.DetectorEnglish, 84);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log(_mProcName + " ", $"{_mProcName} unknown byte value at index 9: {dataByteAtIndex8.ToString("X2")}");
                    break;
            }
        }

        private void ProcessByteAtIndex2(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            byte dataByte2 = dataBytes[2];
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} dataByte2: ", dataByte2.ToString("X2"));

            switch (dataByte2)
            {
                case 0x01:
                    HandleCase01(dataBytes, sRcvTime, sJsonData);
                    break;
                case 0x06:
                    ASI.Lib.Log.DebugLog.Log($"{_mProcName} received a correct message from TaskPA", sJsonData);
                    break;
                case 0x15:
                    HandleCase15(dataBytes, sRcvTime, sJsonData);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log($"{_mProcName} received an unknown error message from PA", sJsonData);
                    break;
            }
        }

        private void HandleCase01(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} processing 0x01 case", sJsonData);
            dataBytes[2] = 0x06;
            Array.Resize(ref dataBytes, dataBytes.Length - 1); // Remove the last byte
            byte newLRC = CalculateLRC(dataBytes);
            Array.Resize(ref dataBytes, dataBytes.Length + 1); // Add a byte back
            dataBytes[dataBytes.Length - 1] = newLRC;
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} replied to TaskPA message at {sRcvTime}", sJsonData);

        }

        private void HandleCase15(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} processing 0x15 case", sJsonData);
            string errorLog;
            switch (dataBytes[4])
            {
                case 0x01:
                    errorLog = "Indicates packet data length error";
                    break;
                case 0x02:
                    errorLog = "Indicates LRC error";
                    break;
                case 0x03:
                    errorLog = "Indicates other errors";
                    break;
                default:
                    errorLog = "Indicates unknown error";
                    break;
            }

            ASI.Lib.Log.DebugLog.Log($"{_mProcName} received an error message from TaskPA: {errorLog} at {sRcvTime}", sJsonData);
        }


        /// <summary>
        /// 計算LRC 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>  
        private byte CalculateLRC(byte[] text)
        {
            byte xor = 0;
            // if no data then done   
            if (text.Length <= 0)
                return 0;
            // incorporate remaining bytes into the value  
            for (int i = 0; i < text.Length; i++)
                xor ^= text[i];
            return xor;
        }
        #endregion
        #region serialPort 
        void SerialPort_DisconnectedEvent(string source) //斷線處理  
        {
            try
            {
                _mSerial.Close();
                _mSerial = null;
                _mSerial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
                _mSerial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
                _mSerial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);
            }
            catch (Exception)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, "斷線處理錯誤");
            }
        }

        void SerialPort_ReceivedEvent(byte[] dataBytes, string source) //顯示器的狀態顯示 
        {
            string sRcvTime = System.DateTime.Now.ToString("HH:mm:ss.fff");
            string str = "";
            foreach (byte b in dataBytes)
            {
                str += Convert.ToString(b, 16).ToUpper().PadLeft(2, '0') + " ";
            }
            var text = string.Format("{0} \r\n收到收包內容 {1} \r\n", sRcvTime, str);
            var sHexString = ASI.Lib.Text.Parsing.String.BytesToHexString(dataBytes, " ");
            if (dataBytes.Length >= 3 && dataBytes[4] == 0x00)
            {
                ASI.Lib.Log.DebugLog.Log(_mProcName, "顯示器的狀態收到的訊息" + sHexString.ToString());  //處理顯示器回報的狀態
            }
            else if (dataBytes[4] != 0x00)
            {
                if (dataBytes[4] == 0x01) { ASI.Lib.Log.ErrorLog.Log(_mProcName, "曾經有通訊不良"); }
                else if (dataBytes[4] == 0x02) { ASI.Lib.Log.ErrorLog.Log(_mProcName, "處於關機狀態 "); }
                else if (dataBytes[4] == 0x04) { ASI.Lib.Log.ErrorLog.Log(_mProcName, "通訊逾時"); }
                else if (dataBytes[4] == 0x07) { ASI.Lib.Log.ErrorLog.Log(_mProcName, " 1/2/4 多重組合 "); }
            }
            ASI.Lib.Log.DebugLog.Log(_mProcName, "從顯示器收到的訊息" + sHexString.ToString());//log紀錄 
        }




        #endregion
    }






}
