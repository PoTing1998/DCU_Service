using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DCU.ProcMsg;
using ASI.Wanda.DMD.ProcMsg;
using Display.Function;
using Display;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;



namespace ASI.Wanda.DCU.TaskCDU
{

    public class ProcTaskCDU : ProcBase
    {
        #region constructor
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
        static string _mDU_ID = "LG01_CDU_09";
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
            _mProcName = "TaskCDU";
       
            string dbIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string dbPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string dbName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string dbUserID = "postgres";
            string dbPassword = "postgres";
            string currentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");
            //serialPort的開啟 
            var iComPort = ConfigApp.Instance.GetConfigSetting("CDUComPort"); 
            var iBaudrate = ConfigApp.Instance.GetConfigSetting("CDUBaudrate"); 
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
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, "打開串口失敗 Serial port open failed ");
                    return result; // 如果串口打開失敗，立即返回
                }
                // 初始化資料庫連線 
                if (!ASI.Wanda.DCU.DB.Manager.Initializer(dbIP, dbPort, dbName, dbUserID, dbPassword, currentUserID))
                {
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, $"資料庫連線失敗! {dbIP}:{dbPort};userid={dbUserID}");
                    return -1; // 如果資料庫初始化失敗，立即返回
                }
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"發生例外! {ex.Message}");
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
                        var taskCDUHelper = new ASI.Wanda.DCU.TaskCDU.TaskCDUHelper(_mProcName, _mSerial);

                        switch (sJsonObjectName)
                        {
                            case ASI.Wanda.DCU.TaskCDU.Constants.SendPreRecordMsg: //預錄訊息 
                            case ASI.Wanda.DCU.TaskCDU.Constants.SendInstantMsg: //即時訊息
                                // 從 JSON 數據中提取相關值
                                var logData = new
                                {
                                    Message = "收到來自TaskDMD的訊息",
                                    JsonData = sJsonData,
                                    SeatID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "seatID"),
                                    MsgID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "msg_id"),
                                    TargetDU = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "target_du"),
                                    DbName1 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName1"), 
                                    DbName2 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName2")
                                };
                                    // 將 logData 物件序列化為 JSON 格式以進行結構化日誌記錄 
                                    string formattedLog = JsonConvert.SerializeObject(logData, Formatting.Indented);
                                    ASI.Lib.Log.DebugLog.Log(_mProcName, formattedLog);
                                    // 處理消息並記錄結果   
                                    string result = "";
                                switch (logData.DbName1)
                                {
                                    case "dmd_pre_record_message": 
                                        ASI.Lib.Log.DebugLog.Log(_mProcName, "處理 dmd_pre_record_message");
                                        // 發送消息到顯示面板（針對預錄訊息的處理）
                                        taskCDUHelper.SendMessageToDisplay(logData.TargetDU, logData.DbName1, logData.DbName2, out result);
                                        ASI.Lib.Log.DebugLog.Log(_mProcName, "處理 dmd_pre_record_message: " + result);
                                        break;
                                    case "dmd_instant_message": 
                                        ASI.Lib.Log.DebugLog.Log(_mProcName, "處理即時消息 dmd_instant_message");
                                        // 發送消息到顯示面板（針對即時訊息的處理） 
                                        taskCDUHelper.SendMessageToDisplay(logData.TargetDU, logData.DbName1, logData.DbName2, out result);
                                        ASI.Lib.Log.DebugLog.Log(_mProcName, "處理即時消息 dmd_instant_message: " + result);
                                        break;
                                    default:
                                        ASI.Lib.Log.DebugLog.Log(_mProcName, $"未知的 DbName1 值：{logData.DbName1}");
                                        break;
                                }
                                break;
                            case ASI.Wanda.DCU.TaskCDU.Constants.SendPowerTimeSetting:
                                taskCDUHelper.PowerSetting(Station_ID);
                                break; 
                            case "節能模式開啟": 
                                OpenDisplay(); 
                                break;
                            case "節能模式關閉":
                                CloseDisplay();
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

        #region 處理廣播的訊息
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
                    ASI.Lib.Log.DebugLog.Log($"{_mProcName} 收到來自 TaskPA 的消息", sJsonData); // 記錄收到的消息
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
                        ASI.Lib.Log.DebugLog.Log($"{_mProcName} dataBytes 長度小於 10", sHexString);
                    }
                    if (dataBytes.Length >= 3)
                    {
                        ProcessByteAtIndex2(dataBytes, sRcvTime, sJsonData);
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log($"{_mProcName} dataBytes 長度小於 3", sJsonData);
                    }
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, ex); // 記錄例外情況  
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
        /// <summary>
        /// 處理緊急訊息
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        private void ProcessDataBytes(byte[] dataBytes)
        {
            byte dataByteAtIndex8 = dataBytes[8];
            var taskUPDHelper = new ASI.Wanda.DCU.TaskCDU.TaskCDUHelper(_mProcName, _mSerial);
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
                    ASI.Lib.Log.DebugLog.Log($"{_mProcName} 收到來自 TaskPA 的正確消息", sJsonData);
                    break;
                case 0x15:
                    HandleCase15(dataBytes, sRcvTime, sJsonData);
                    break;
                default:
                    ASI.Lib.Log.DebugLog.Log($"{_mProcName} 收到來自 PA 的未知錯誤消息", sJsonData);
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
            ASI.Lib.Log.DebugLog.Log($"{_mProcName} 處理 0x15 案例", sJsonData);
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

            ASI.Lib.Log.DebugLog.Log($"{_mProcName} 在 {sRcvTime} 收到來自 TaskPA 的錯誤消息：{errorLog}", sJsonData);
        }

        #endregion
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
                ASI.Lib.Log.DebugLog.Log(_mProcName, "顯示器的狀態收到的訊息" + sHexString.ToString());  //處理顯示器回報的狀態 
            }
            else if (dataBytes[4] != 0x00)  
            {   
                if (dataBytes[4] == 0x01) { ASI.Lib.Log.ErrorLog.Log(_mProcName, "曾經有通訊不良"); }
                else if (dataBytes[4] == 0x02) { ASI.Lib.Log.ErrorLog.Log(_mProcName, "處於關機狀態 "); }
                else if (dataBytes[4] == 0x04) { ASI.Lib.Log.ErrorLog.Log(_mProcName, "通訊逾時"); }
                else if (dataBytes[4] == 0x07) { ASI.Lib.Log.ErrorLog.Log(_mProcName, " 1/2/4 多重組合 "); }
            }
            ASI.Lib.Log.ErrorLog.Log(_mProcName, "從顯示器收到的訊息" + sHexString.ToString());//log紀錄 
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

        public void CloseDisplay()
        {
            // 關閉顯示器的邏輯  
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor(); 
            var function = new PowerControlHandler(); 
            var Off = new byte[] { 0x3A, 0X01 };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, false);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, true);
            var packetOff = processor.CreatePacketOff(startCode, new List<byte> { Convert.ToByte(front), Convert.ToByte(back) }, function.FunctionCode, Off);
            var serializedDataOff = processor.SerializePacket(packetOff);
             _mSerial.Send(serializedDataOff);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面關閉", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
        }
        public void OpenDisplay()
        {
            // 開啟顯示器的邏輯 
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler(); 
            var Open = new byte[] { 0x3A, 0X00 };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, false);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, true);
            var packetOpen = processor.CreatePacketOff(startCode, new List<byte> { Convert.ToByte(front), Convert.ToByte(back) }, function.FunctionCode, Open);
            var serializedDataOpen = processor.SerializePacket(packetOpen);
             _mSerial.Send(serializedDataOpen);
            ASI.Lib.Log.DebugLog.Log(_mProcName + "顯示畫面開啟", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen));
       
        }

      
    }
}
