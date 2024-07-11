using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DCU.ProcMsg;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;



namespace ASI.Wanda.DCU.TaskLPD
{
    public class ProcTaskLPD : ProcBase
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
        #endregion

        /// <summary>
        /// 處理DMD模組執行程序所收到之訊息 
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
            else if (pLabel == MSGFromTaskDMD.Label)
            {
                return ProMsgFromDMD(pBody);
            }
            else if (pLabel == PA.ProcMsg.MSGFromTaskPA.Label)
            {
                return ProMsgFromPA(pBody);
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
            mProcName = "TaskLPD";
            // 讀取配置設置
            string dbIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string dbPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string dbName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string currentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");
            var iComPort = ConfigApp.Instance.GetConfigSetting("LPDComPort");
            var iBaudrate = ConfigApp.Instance.GetConfigSetting("LPDBaudrate");

            ///serialPort的開啟  
            string dbUserID = "postgres";
            string dbPassword = "postgres";
            var connectionString = $"PortName=COM{iComPort};BaudRate={iBaudrate};DataBits=8;StopBits=One;Parity=None";

            serial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
            serial.ConnectionString = connectionString;
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

            return base.StartTask(pComputer, pProcName);
        }

        /// <summary>
        /// 處理TaskDMD的訊息
        /// </summary> 
        private int ProMsgFromDMD(string pMessage)
        {
            try
            {
                ASI.Wanda.DMD.ProcMsg.MSGFromTaskDMD mSGFromTaskDMD = new ASI.Wanda.DMD.ProcMsg.MSGFromTaskDMD(new MSGFrameBase(""));
                if (mSGFromTaskDMD.UnPack(pMessage) > 0)
                {
                    string sJsonData = mSGFromTaskDMD.JsonData;
                    string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDMD.JsonData, "JsonObjectName");
                    string sStationID = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDMD.JsonData, "StationID");
                    string sSeatID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "SeatID");
                    string dbName1 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName1");
                    string dbName2 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName2");
                    string target_du = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "target_du");

                    Guid iMsgID = new Guid(mSGFromTaskDMD.MessageID.ToString());
                    ASI.Lib.Log.DebugLog.Log(mProcName, $"收到來自TaskDMD的訊息，SeatID:{sSeatID}；MsgID:{iMsgID}；JsonObjectName:{sJsonObjectName}");
                    var taskUPDHelper = new ASI.Wanda.DCU.TaskLPD.TaskLPDHelper(mProcName, serial);

                    if (dbName1 == "dmd_train_message")
                    {
                        ASI.Lib.Log.DebugLog.Log(mProcName, "處理 dmd_train_message");
                    }
                    else
                    {
                        //判斷收到的訊息ID  
                        ASI.Lib.Log.DebugLog.Log(mProcName, "處理其他訊息");
                    }
                    //傳送到面板上
                    taskUPDHelper.SendMessageToDisplay(target_du, dbName1, dbName2);
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
        private void ProcessDataBytes(byte[] dataBytes)
        {
            byte dataByteAtIndex8 = dataBytes[8];
            var taskUPDHelper = new ASI.Wanda.DCU.TaskLPD.TaskLPDHelper(mProcName, serial);
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

        /// <summary>
        /// 計算LRC
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public byte CalculateLRC(byte[] text)
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
        #region DU 


        #endregion
    }
}
