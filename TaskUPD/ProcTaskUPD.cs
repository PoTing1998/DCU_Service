﻿using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DCU.ProcMsg;
using ASI.Wanda.DMD.ProcMsg;
using ASI.Wanda.PA.ProcMsg;

using Display;
using Display.DisplayMode;
using Display.Function;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using System.Threading.Tasks;

using System.Xml;
using System.Timers;

using System.Xml.Linq;



namespace ASI.Wanda.DCU.TaskUPD
{


    public class ProcTaskUPD : ProcBase
    {
        #region constructor
        static int mSEQ = 0; // 計算累進發送端的次數  
        ASI.Lib.Comm.SerialPort.SerialPortLib serial = null;
        /// <summary>
        /// 讀取火災資料
        /// </summary>
        static string sCheckChinese = ConfigApp.Instance.GetConfigSetting("FireDetectorCheckInProgressChinese");
        static string sCheckEnglish = ConfigApp.Instance.GetConfigSetting("FireDetectorCheckInProgressEnglish");
        static string sEmergencyChinese = ConfigApp.Instance.GetConfigSetting("FireEmergencyEvacuateCalmlyChinese");
        static string sEmergencyEnglish = ConfigApp.Instance.GetConfigSetting("FireEmergencyEvacuateCalmlyEnglish");
        static string sClearedChinese = ConfigApp.Instance.GetConfigSetting("FireAlarmClearedChinese");
        static string sClearedEnglish = ConfigApp.Instance.GetConfigSetting("FireAlarmClearedEnglish");
        static string sDetectorChinese = ConfigApp.Instance.GetConfigSetting("FireDetectorClearConfirmedChinese");
        static string sDetectorEnglish = ConfigApp.Instance.GetConfigSetting("FireDetectorClearConfirmedEnglish");

        private static PowerSettingManager _proScheduler;

        #endregion

        #region MSMQ Method
        /// <summary>
        /// 處理UPD模組執行程序所收到之訊息 
        /// </summary>
        /// <param name="pLabel"></param>
        /// <param name="pBody"></param>
        /// <returns></returns>
        public override int ProcEvent(string pLabel, string pBody)
        {
            LogFile.Display(pBody);

            if (pLabel == MSGFinish.Label)
            {
                return 0;
            }
            else if (pLabel == ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD.Label)
            {
                return ProMsgFromDMD(pBody);
            }
            else if (pLabel == MSGFromTaskDCU.Label)
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
        /// 啟始處理UPD模組執行程序
        /// </summary>
        /// <param name="pComputer"></param>
        /// <param name="pProcName"></param>
        /// <returns></returns>
        public override int StartTask(string pComputer, string pProcName)
        {
            mTimerTick = 30;
            mProcName = "TaskUPD";
            // 讀取配置設置
            string dbIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string dbPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string dbName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string currentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");
            var iComPort = ConfigApp.Instance.GetConfigSetting("UPDComPort");
            var iBaudrate = ConfigApp.Instance.GetConfigSetting("UPDBaudrate");
            ///serialPort的開啟  
            string dbUserID = "postgres";
            string dbPassword = "postgres";
            var connectionString = $"PortName=COM{iComPort};BaudRate={iBaudrate};DataBits=8;StopBits=One;Parity=None";

            serial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
            serial.ConnectionString = connectionString;
            serial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
            serial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);
            int result = -1; // Default to an error state
          
            try
            {
                result = serial.Open();
                if (result != 0)
                {
                    ASI.Lib.Log.ErrorLog.Log(mProcName, "Serial port open failed");
                    return result; // Return immediately if the serial port failed to open
                }
                // 初始化資料庫連線
                if (!ASI.Wanda.DCU.DB.Manager.Initializer(dbIP, dbPort, dbName, dbUserID, dbPassword, currentUserID))
                {
                    ASI.Lib.Log.ErrorLog.Log(mProcName, $"資料庫連線失敗! {dbIP}:{dbPort};userid={dbUserID}");
                    return -1; // Return immediately if the database initialization failed
                }
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, $"例外發生! {ex.Message}");
                return -1; // Return immediately if any exception occurs
            }

            // 獲取 PowerSettingManager 的實例
            PowerSettingManager manager = PowerSettingManager.GetInstance();

            // 呼叫 PowerSetting 方法
            manager.PowerSetting("LG01");
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
                        ASI.Wanda.DCU.TaskUPD.TaskUPDHelper _taskUPDHelper = new ASI.Wanda.DCU.TaskUPD.TaskUPDHelper(mProcName, serial);

                        string sJsonData = mSGFromTaskDMD.JsonData;
                        string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "JsonObjectName");
                       
                        if (sJsonObjectName == ASI.Wanda.DCU.TaskUPD.TaskUPDHelper.Constants.SendPreRecordMsg)
                        {
                            string PreRecordMessageSettingSeatID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "seatID");
                            string PreRecordMessageSettingMsg_id = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "msg_id");
                            string PreRecordMessageSettingDbName1 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName1");
                            string PreRecordMessageSettingDbName2 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName2");
                            string PreRecordMessageSettingTarget_du = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "target_du");

                            ASI.Lib.Log.DebugLog.Log(mProcName, $"收到來自TaskDMD的訊息，mSGFromTaskDMD:{mSGFromTaskDMD.JsonData};SeatID:{PreRecordMessageSettingSeatID}；MsgID:{PreRecordMessageSettingMsg_id}；target_du:{PreRecordMessageSettingTarget_du}; dbName1 :{PreRecordMessageSettingDbName1};dbName2 :{PreRecordMessageSettingDbName2}");
                           // _taskUPDHelper.SendMessageToUrgnt("您好!下雨期間車站電聯車受風雨引響，地板較濕請小心行走，謝謝!", "Hello! During the rain, the stations electric vehicle was led by the wind and rain. Please walk carefully, thank you!", 81);
                            //傳送到面板上
                            _taskUPDHelper.judgeDbName(PreRecordMessageSettingDbName1);

                         


                        }
                        else if (sJsonObjectName == ASI.Wanda.DCU.TaskUPD.TaskUPDHelper.Constants.SendInstantMsg)
                        {
                            string PreInstantMessageSettingSeatID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "seatID");
                            string PreInstantMessageSettingMsg_id = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "msg_id");
                            string PreInstantMessageSettingDbName1 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName1");
                            string PreInstantMessageSettingDbName2 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName2");
                            string PreInstantMessageSettingTarget_du = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "target_du");

                            ASI.Lib.Log.DebugLog.Log(mProcName, $"收到來自TaskDMD的訊息，mSGFromTaskDMD:{mSGFromTaskDMD.JsonData};SeatID:{PreInstantMessageSettingSeatID}；MsgID:{PreInstantMessageSettingMsg_id}；target_du:{PreInstantMessageSettingTarget_du}; dbName1 :{PreInstantMessageSettingDbName1};dbName2 :{PreInstantMessageSettingDbName2}");

                            //傳送到面板上
                            _taskUPDHelper.judgeDbName( PreInstantMessageSettingDbName1);

                        }
                        else if (sJsonObjectName == ASI.Wanda.DCU.TaskUPD.TaskUPDHelper.Constants.SendScheduleSetting)
                        {
                            string PreScheduleSettingSeatID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "seatID");
                            string PreSendScheduleSettingMsg_id = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "msg_id");
                            string PreSendScheduleSettingDbName1 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName1");
                            string PreSendScheduleSettingDbName2 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName2");
                            ASI.Lib.Log.DebugLog.Log(mProcName, $"收到來自TaskDMD的訊息，mSGFromTaskDMD:{mSGFromTaskDMD.JsonData};SeatID:{PreScheduleSettingSeatID}；MsgID:{PreSendScheduleSettingMsg_id}；dbName1 :{PreSendScheduleSettingDbName1};dbName2 :{PreSendScheduleSettingDbName2}");
                            //傳送到面板上
                            _taskUPDHelper.judgeDbName(PreSendScheduleSettingDbName1);
                        }
                        else if(sJsonObjectName == ASI.Wanda.DCU.TaskUPD.TaskUPDHelper.Constants.SendPowerTimeSetting)
                        {
                            _taskUPDHelper.PowerSetting("LG01");

                        }
                    }
                    catch (Exception ex)
                    {
                        ASI.Lib.Log.ErrorLog.Log(mProcName, ex.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, ex);
            }
            return -1;
        }
      
        /// <summary>
        /// 處理TaskPA的訊息   
        /// </summary>  
        private int ProMsgFromPA(string pMessage)
        {
            string sRcvTime = System.DateTime.Now.ToString("HH:mm:ss.fff");
            try
            {
                ASI.Wanda.DCU.ProcMsg.MSGFromTaskPA MSGFromTaskPA = new ProcMsg.MSGFromTaskPA(new MSGFrameBase(""));
                if (MSGFromTaskPA.UnPack(pMessage) > 0)
                {
                    var sJsonData = MSGFromTaskPA.JsonData;
                    ASI.Lib.Log.DebugLog.Log(mProcName + " received a message from TaskPA ", sJsonData); // Log the received message 
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
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} dataBytes length less than 10", sHexString);
                    }
                    if (dataBytes.Length >= 3)
                    {
                        ProcessByteAtIndex2(dataBytes, sRcvTime, sJsonData);
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} dataBytes length less than 3", sJsonData);
                    }
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, ex); // 記錄例外情況 
            }

            return -1;
        }

        #endregion
        #region SerialPort 
        void SerialPort_DisconnectedEvent(string source) //斷線處理   
        {
            try
            {
                serial.Close();
                serial = null;
                serial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
                serial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
                serial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, "斷線處理錯誤" + ex.ToString());
            }
        }

        void SerialPort_ReceivedEvent(byte[] dataBytes, string source)
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
                ASI.Lib.Log.DebugLog.Log(mProcName, mProcName + "顯示器的狀態收到的訊息" + sHexString.ToString());  //處理顯示器回報的狀態
            }
            else if (dataBytes[4] != 0x00)
            {
                if (dataBytes[4] == 0x01) { ASI.Lib.Log.ErrorLog.Log(mProcName, "曾經有通訊不良"); }
                else if (dataBytes[4] == 0x02) { ASI.Lib.Log.ErrorLog.Log(mProcName, "處於關機狀態 "); }
                else if (dataBytes[4] == 0x04) { ASI.Lib.Log.ErrorLog.Log(mProcName, "通訊逾時"); }
                else if (dataBytes[4] == 0x07) { ASI.Lib.Log.ErrorLog.Log(mProcName, " 1/2/4 多重組合 "); }
            }
            ASI.Lib.Log.ErrorLog.Log(mProcName, "從顯示器收到的訊息" + sHexString.ToString());//log紀錄 
        }
        #endregion
        /// <summary>
        /// 回傳給廣播
        /// </summary>
        void SendBroadcastMessage(ASI.Wanda.PA.Message.Message.MessageType messageType, byte station, byte platForm, byte situation)
        {
            ///組成封包內容
            ASI.Wanda.PA.Message.Message oPAMsg = new ASI.Wanda.PA.Message.Message(messageType);

            oPAMsg.station = station;
            oPAMsg.platform = platForm;
            oPAMsg.situation = situation;
            oPAMsg.SEQ = (byte)mSEQ++;
            oPAMsg.LRC = oPAMsg.GetMsgLRC();

            var arrPacketByte = ASI.Wanda.PA.Message.Helper.Pack(oPAMsg);
            string sHexString = ASI.Lib.Text.Parsing.String.BytesToHexString(arrPacketByte, " ");
            //MSMQ 回傳給TaskPA
            SendToTaskPA(2, 10, sHexString);
        }
        /// <summary> 
        /// 回傳給TaskPA  
        /// </summary>
        /// <param name="msgType"></param>   
        /// <param name="msgID"></param>    
        /// <param name="jsonData"></param> 
        static public void SendToTaskPA(int msgType, int msgID, string ContentDataBytes)
        {
            try
            {

                ASI.Wanda.DCU.ProcMsg.MSGFromTaskUPD MSGFromTaskUPD = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskUPD(new MSGFrameBase("taskUPD", "dcuservertaskpa"));

                MSGFromTaskUPD.MessageType = msgType;
                MSGFromTaskUPD.MessageID = msgID;
                MSGFromTaskUPD.JsonData = ContentDataBytes;
                ASI.Lib.Process.ProcMsg.SendMessage(MSGFromTaskUPD);
                 
                // 假設 contentDataBytes 需要序列化為十六進制字符串 
                string serializedContent = ASI.Lib.Text.Parsing.Json.SerializeObject(ContentDataBytes);
                var msg = new ASI.Wanda.DCU.Message.Message(ASI.Wanda.DCU.Message.Message.eMessageType.Command, 01, serializedContent);
                msg.JsonContent = serializedContent;
                ASI.Lib.Log.DebugLog.Log("Sent packet to TaskPA  ", msg.JsonContent);

            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskPA", ex);
            }
        }
        #region Prviate Method 
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

        private void ProcessDataBytes(byte[] dataBytes)
        {
            byte dataByteAtIndex8 = dataBytes[8];
            var taskUPDHelper = new ASI.Wanda.DCU.TaskUPD.TaskUPDHelper(mProcName, serial);
            switch (dataByteAtIndex8)
            {
                case 0x81:
                    taskUPDHelper.SendMessageToUrgnt(sCheckChinese, sCheckEnglish, 81);
                    break;
                case 0x82:
                    taskUPDHelper.SendMessageToUrgnt(sEmergencyChinese, sEmergencyEnglish, 82);
                    break;
                case 0x83:
                    taskUPDHelper.SendMessageToUrgnt(sClearedChinese, sClearedEnglish, 83);
                    break;
                case 0x84:
                    taskUPDHelper.SendMessageToUrgnt(sDetectorChinese, sDetectorEnglish, 84);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log(mProcName + " ", $"{mProcName} unknown byte value at index 9: {dataByteAtIndex8.ToString("X2")}");
                    break;
            }
        }
        private void ProcessByteAtIndex2(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            byte dataByte2 = dataBytes[2];
            ASI.Lib.Log.DebugLog.Log($"{mProcName} dataByte2: ", dataByte2.ToString("X2"));
            switch (dataByte2)
            {
                case 0x01:
                    HandleCase01(dataBytes, sRcvTime, sJsonData);
                    break;
                case 0x06:
                    ASI.Lib.Log.DebugLog.Log($"{mProcName} received a correct message from TaskPA", sJsonData);
                    break;
                case 0x15:
                    HandleCase15(dataBytes, sRcvTime, sJsonData);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log($"{mProcName} received an unknown error message from PA", sJsonData);
                    break;
            }
        }
        private void HandleCase01(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            ASI.Lib.Log.DebugLog.Log($"{mProcName} processing 0x01 case", sJsonData);
            dataBytes[2] = 0x06;
            Array.Resize(ref dataBytes, dataBytes.Length - 1); // Remove the last byte
            byte newLRC = CalculateLRC(dataBytes);
            Array.Resize(ref dataBytes, dataBytes.Length + 1); // Add a byte back
            dataBytes[dataBytes.Length - 1] = newLRC;
            ASI.Lib.Log.DebugLog.Log($"{mProcName} replied to TaskPA message at {sRcvTime}", sJsonData);

        }

        private void HandleCase15(byte[] dataBytes, string sRcvTime, string sJsonData)
        {
            ASI.Lib.Log.DebugLog.Log($"{mProcName} processing 0x15 case", sJsonData);
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

            ASI.Lib.Log.DebugLog.Log($"{mProcName} received an error message from TaskPA: {errorLog} at {sRcvTime}", sJsonData);
        }
        /// <summary>
        /// Convert a hexadecimal string to a byte array.
        /// </summary>
        /// <param name="hex">The hexadecimal string to convert.</param>
        /// <returns>A byte array representing the hexadecimal string.</returns>
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
        #endregion
    }
}