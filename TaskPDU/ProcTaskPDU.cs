using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DCU.ProcMsg;
using ASI.Wanda.DMD.ProcMsg;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace ASI.Wanda.DCU.TaskPDU
{
    public class ProcTaskPDU : ProcBase
    {
        #region constructor
        static int mSEQ = 0; // 計算累進發送端的次數  
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial = null;

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

        static string Station_ID = ConfigApp.Instance.GetConfigSetting("Station_ID");
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
            else if (pLabel == MSGFromTaskDMD.Label)
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
            mProcName = "TaskPDU";
            string dbIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string dbPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string dbName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string dbUserID = "postgres";
            string dbPassword = "postgres";
            string currentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");
            //serialPort的開啟 
            var iComPort = ConfigApp.Instance.GetConfigSetting("PDUComPort"); ;
            var iBaudrate = ConfigApp.Instance.GetConfigSetting("PDUBaudrate"); ;
            _mSerial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
            string connectionString = $"PortName=COM{iComPort};BaudRate={iBaudrate};DataBits=8;StopBits=One;Parity=None";
            _mSerial.ConnectionString = connectionString;
            _mSerial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
            _mSerial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);
            int result = -1; // 默認為錯誤狀態
            try
            {
                result = _mSerial.Open();
                if (result != 0)
                {
                    ASI.Lib.Log.ErrorLog.Log(mProcName, "打開串口失敗");
                    return result; // 如果串口打開失敗，立即返回
                }
                // 初始化資料庫連線
                if (!ASI.Wanda.DCU.DB.Manager.Initializer(dbIP, dbPort, dbName, dbUserID, dbPassword, currentUserID))
                {
                    ASI.Lib.Log.ErrorLog.Log(mProcName, $"資料庫連線失敗! {dbIP}:{dbPort};userid={dbUserID}");
                    return -1; // 如果資料庫初始化失敗，立即返回
                }
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, $"發生例外! {ex.Message}");
                return -1; // 如果發生任何例外，立即返回
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
                        var taskPDUHelper = new ASI.Wanda.DCU.TaskPDU.TaskPDUHelper(mProcName, _mSerial);
                        switch (sJsonObjectName)
                        {
                            case ASI.Wanda.DCU.TaskPDU.Constants.SendPreRecordMsg:
                                string sSeatID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "seatID");
                                string msg_id = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "msg_id");
                                string dbName1 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName1");
                                string dbName2 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName2");
                                string target_du = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "target_du");

                                ASI.Lib.Log.DebugLog.Log(mProcName, $"收到來自TaskDMD的訊息，mSGFromTaskDMD:{mSGFromTaskDMD.JsonData};SeatID:{sSeatID}；MsgID:{msg_id}；target_du:{target_du}; dbName1 :{dbName1};dbName2 :{dbName2}");

                                if (dbName1 == "dmd_pre_record_message")
                                {
                                    string result = "";  
                                    ASI.Lib.Log.DebugLog.Log(mProcName, "處理 dmd_pre_record_message");

                                    byte[] SerialiazedData = new byte[] { };
                                    //傳送到面板上 
                                    taskPDUHelper.SendMessageToDisplay(target_du, dbName1, dbName2, out result , out SerialiazedData);

                                    _mSerial.Send(SerialiazedData); 

                                    ASI.Lib.Log.DebugLog.Log(mProcName, "處理 dmd_pre_record_message"+ result);
                                }
                                else
                                {
                                    //判斷收到的訊息ID  
                                    ASI.Lib.Log.DebugLog.Log(mProcName, "處理其他訊息");
                                }
                                break;

                            case ASI.Wanda.DCU.TaskPDU.Constants.SendPowerTimeSetting:
                                taskPDUHelper.PowerSetting(Station_ID);
                                break;
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

        #region 處理訊息
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
                    ASI.Lib.Log.DebugLog.Log($"{mProcName} 收到來自 TaskPA 的消息", sJsonData); // 記錄收到的消息
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
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} dataBytes 長度小於 10", sHexString);
                    }
                    if (dataBytes.Length >= 3)
                    {
                        ProcessByteAtIndex2(dataBytes, sRcvTime, sJsonData);
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} dataBytes 長度小於 3", sJsonData);
                    }
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, ex); // 記錄例外情況 
            }

            return -1;
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

        private async Task ProcessDataBytes(byte[] dataBytes)
        {
            byte dataByteAtIndex8 = dataBytes[8];
            var taskUPDHelper = new ASI.Wanda.DCU.TaskPDU.TaskPDUHelper(mProcName, _mSerial);
            Tuple<byte[], byte[], byte[]> serializedData;

            switch (dataByteAtIndex8)
            {
                case 0x81:
                    serializedData = await taskUPDHelper.SendMessageToUrgnt(sCheckChinese, sCheckEnglish, 81);
                    SendSerializedData(serializedData.Item1, "Serialized display packet (Chinese)"); // 發送中文訊息
                    SendSerializedData(serializedData.Item2, "Serialized display packet (English)"); // 發送英文訊息
                    break;
                case 0x82:
                    serializedData = await taskUPDHelper.SendMessageToUrgnt(sEmergencyChinese, sEmergencyEnglish, 82);
                    SendSerializedData(serializedData.Item1, "Serialized display packet (Chinese)"); // 發送中文訊息
                    SendSerializedData(serializedData.Item2, "Serialized display packet (English)"); // 發送英文訊息
                    break;
                case 0x83:
                    serializedData = await taskUPDHelper.SendMessageToUrgnt(sClearedChinese, sClearedEnglish, 83);
                    SendSerializedData(serializedData.Item1, "Serialized display packet (Chinese)"); // 發送中文訊息
                    SendSerializedData(serializedData.Item2, "Serialized display packet (English)"); // 發送英文訊息
                    break;
                case 0x84:
                    serializedData = await taskUPDHelper.SendMessageToUrgnt(sDetectorChinese, sDetectorEnglish, 84);
                    SendSerializedData(serializedData.Item3, "Serialized display packet (Chinese)"); // 關閉訊息
                    
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log(mProcName + " ", $"{mProcName} unknown byte value at index 8: {dataByteAtIndex8.ToString("X2")}");
                    break;
            }
        }

        private void SendSerializedData(byte[] serializedData, string logMessage)
        {
            // 記錄日誌
            ASI.Lib.Log.DebugLog.Log(mProcName + " SendMessageToUrgnt", logMessage + ": " + BitConverter.ToString(serializedData));

            // 發送數據
            var temp = _mSerial.Send(serializedData);

            // 記錄是否發送成功
            ASI.Lib.Log.DebugLog.Log(" 是否傳送成功 " + mProcName, temp.ToString());
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
                    ASI.Lib.Log.DebugLog.Log($"{mProcName} 收到來自 TaskPA 的正確消息", sJsonData);
                    break;
                case 0x15:
                    HandleCase15(dataBytes, sRcvTime, sJsonData);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log($"{mProcName} 收到來自 PA 的未知錯誤消息", sJsonData);
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
            ASI.Lib.Log.DebugLog.Log($"{mProcName} 處理 0x15 案例", sJsonData);
            string errorLog;
            switch (dataBytes[4])
            {
                case 0x01:
                    errorLog = "表示數據包長度錯誤";
                    break;
                case 0x02:
                    errorLog = "表示 LRC 錯誤";
                    break;
                case 0x03:
                    errorLog = "表示其他錯誤";
                    break;
                default:
                    errorLog = "表示未知錯誤";
                    break;
            }

            ASI.Lib.Log.DebugLog.Log($"{mProcName} 在 {sRcvTime} 收到來自 TaskPA 的錯誤消息：{errorLog}", sJsonData);
        }

        #endregion
        #region serialport
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
                ASI.Lib.Log.ErrorLog.Log(mProcName, "斷線處理錯誤");
            }
        }

        void SerialPort_ReceivedEvent(byte[] dataBytes, string source) //顯示器的狀態顯示 只能限制一個ID
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
                ASI.Lib.Log.DebugLog.Log(mProcName, "顯示器的狀態收到的訊息" + sHexString.ToString());  //處理顯示器回報的狀態
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
    }
}
