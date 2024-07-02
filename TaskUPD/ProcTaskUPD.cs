using ASI.Lib.Config;
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



namespace ASI.Wanda.DCU.TaskUPD
{


    public class ProcTaskUPD : ProcBase
    {
        #region constructor
        static int mSEQ = 0; // 計算累進發送端的次數  
        ASI.Lib.Comm.SerialPort.SerialPortLib serial = null;

        static string sCheckChinese = ConfigApp.Instance.GetConfigSetting("FireDetectorCheckInProgressChinese");
        static string sCheckEnglish = ConfigApp.Instance.GetConfigSetting("FireDetectorCheckInProgressEnglish");
        static string sEmergencyChinese = ConfigApp.Instance.GetConfigSetting("FireEmergencyEvacuateCalmlyChinese");
        static string sEmergencyEnglish = ConfigApp.Instance.GetConfigSetting("FireEmergencyEvacuateCalmlyEnglish");

        static string sClearedChinese = ConfigApp.Instance.GetConfigSetting("FireAlarmClearedChinese");
        static string sClearedEnglish = ConfigApp.Instance.GetConfigSetting("FireAlarmClearedEnglish");

        static string sDetectorChinese = ConfigApp.Instance.GetConfigSetting("FireDetectorClearConfirmedChinese");
        static string sDetectorEnglish = ConfigApp.Instance.GetConfigSetting("FireDetectorClearConfirmedEnglish");
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
           
            serial = new ASI.Lib.Comm.SerialPort.SerialPortLib ();
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
                    var taskUPDHelper = new ASI.Wanda.DMD.TaskUPD.TaskUPDHelper(mProcName,serial);

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
                    taskUPDHelper.SendMessageToDisplay(target_du,dbName1, dbName2);
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
            switch (dataByteAtIndex8)
            {
                case 0x81:
                    SendMessageToUrgnt(sCheckChinese, sCheckEnglish, 81);
                    break;
                case 0x82:
                    SendMessageToUrgnt(sEmergencyChinese, sEmergencyEnglish, 82);
                    break; 
                case 0x83:
                    SendMessageToUrgnt(sClearedChinese, sClearedEnglish, 83);
                    break;
                case 0x84:
                    SendMessageToUrgnt(sDetectorChinese, sDetectorEnglish, 84);
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
        /// <summary>
        /// 一般訊息
        /// </summary>
        void SendMessageToDisplay()
        {
            //從資料庫撈取相對應的參數
           var number =  Guid.NewGuid();
            //取的 dmd_target 跟 dmd_pre_record_message



            //取得各項參數
            var processor = new PacketProcessor();

            var textStringBody = new TextStringBody
            {
                RedColor = 0xFF,
                GreenColor = 0xFF,
                BlueColor = 0xFF,
                StringText = "萬大線測試訊息"
            };
            var stringMessage = new StringMessage
            {
                StringMode = 0x2A, // TextMode (Static)   
                StringBody = textStringBody
            };
            var fullWindowMessage = new FullWindow //Display version
            {
                MessageType = 0x71, // FullWindow message
                MessageLevel = 0x04, //  level
                MessageScroll = new ScrollInfo { ScrollMode = 0x61, ScrollSpeed = 05, PauseTime = 10 },
                MessageContent = new List<StringMessage> { stringMessage }
            };
            var sequence1 = new Display.Sequence
            {
                SequenceNo = 1, 
                Font = new FontSetting { Size = FontSize.Font24x24, Style = FontStyle.Ming },
                Messages = new List<IMessage> { fullWindowMessage }
            };

            var startCode = new byte[] { 0x55, 0xAA };
            var function = new PassengerInfoHandler(); // Use PassengerInfoHandler 
            var packet = processor.CreatePacket(startCode, new List<byte> { 0x23, 0x24 }, function.FunctionCode, new List<Sequence> { sequence1 });
            var serializedData = processor.SerializePacket(packet);
            ASI.Lib.Log.DebugLog.Log(mProcName + " SendMessageToDisplay", "Serialized display packet: " + BitConverter.ToString(serializedData));

            serial.Send(serializedData);
        }
        /// <summary>
        /// 左測月台碼
        /// </summary>
        void SendMessageToDisplay2()
        {
            var processor = new PacketProcessor();

            var textStringBody = new TextStringBody
            {
                RedColor = 0xFF,  
                GreenColor = 0xFF,
                BlueColor = 0xFF,
                StringText = "萬大線"
            };
            var stringMessage = new StringMessage
            {
                StringMode = 0x2A, // TextMode (Static)
                StringBody = textStringBody
            };
            var leftPlatform = new LeftPlatform //Display version
            {
                MessageType = 0x72, // FullWindow message
                MessageLevel = 0x04, //  level 
                MessageScroll = new ScrollInfo { ScrollMode = 0x61, ScrollSpeed = 07, PauseTime = 10 },
                RedColor = 0xFF,
                GreenColor = 0xFF,
                BlueColor = 0xFF,
                PhotoIndex = 1,

                MessageContent = new List<StringMessage> { stringMessage }
            };  
            var sequence1 = new Display.Sequence
            {
                SequenceNo = 1, 
                Font = new FontSetting { Size = FontSize.Font24x24, Style = FontStyle.Ming },
                Messages = new List<IMessage> { leftPlatform } 
            };

            var startCode = new byte[] { 0x55, 0xAA };
            var function = new PassengerInfoHandler(); // Use PassengerInfoHandler  
            var packet = processor.CreatePacket(startCode, new List<byte> { 0x01 }, function.FunctionCode, new List<Sequence> { sequence1 });
            var serializedData = processor.SerializePacket(packet);
            ASI.Lib.Log.DebugLog.Log(mProcName + "送到看板上", "Serialized display packet: " + BitConverter.ToString(serializedData)); 
            serial.Send(serializedData); 
        }

        /// <summary>
        /// 緊急訊息  
        /// </summary> 
        async void SendMessageToUrgnt(string FireContentChinese, string FireContentEnglish, int situation) 
        {
            try
            {
                // 設定警示的視為固定內容  
                var processor = new PacketProcessor();
                var startCode = new byte[] { 0x55, 0xAA };  
                var function = new EmergencyMessagePlaybackHandler(); 

                // Send Chinese Message   
                var sequence1 = CreateSequence(FireContentChinese, 1);  
                var packet1 = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Display.Sequence> { sequence1 });
                var serializedData1 = processor.SerializePacket(packet1);
                ASI.Lib.Log.DebugLog.Log(mProcName + " SendMessageToUrgnt", "Serialized display packet: " + BitConverter.ToString(serializedData1));
                
                var  temp  =   serial.Send(serializedData1);
                ASI.Lib.Log.DebugLog.Log( " 是否傳送成功 " + mProcName, temp.ToString());
                // Send English Message 
                var sequence2 = CreateSequence(FireContentEnglish, 2);
                var packet2 = processor.CreatePacket(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, new List<Display.Sequence> { sequence2 });
                var serializedData2 = processor.SerializePacket(packet2);
                ASI.Lib.Log.DebugLog.Log(mProcName + " SendMessageToUrgnt", "Serialized display packet: " + BitConverter.ToString(serializedData2));

                serial.Send(serializedData2);
                // Optional delay and turn off if situation is 84  
                if (situation == 84)
                {                          
                    await Task.Delay(10000); // 延遲五秒   
                    var OffMode = new byte[] { 0x02 };
                    var packetOff = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, OffMode);
                    var serializedDataOff = processor.SerializePacket(packetOff);
                    ASI.Lib.Log.DebugLog.Log(mProcName + " 解除緊急訊息", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
                    serial.Send(serializedDataOff);
                }
                
                // Optional delay and turn off if situation is 84 
                if (situation == 84)
                {
                    await Task.Delay(10000); // 延遲十秒 
                    var OffMode = new byte[] { 0x02 };
                    var packetOff2 = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, OffMode);
                    var serializedDataOff2 = processor.SerializePacket(packetOff2);
                    ASI.Lib.Log.DebugLog.Log(mProcName + " 解除緊急訊息", "Serialized display packet: " + BitConverter.ToString(serializedDataOff2));
                    serial.Send(serializedDataOff2);   
                }  
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("SendMessageToUrgnt", ex);
            }
        }

            Display.Sequence CreateSequence(string messageContent, int sequenceNo)
            {
                //緊急訊息為紅色
                var textStringBody = new TextStringBody
                {   
                    RedColor = 0xFF,
                    GreenColor = 0x00,
                    BlueColor = 0x00,
                    StringText = messageContent
                };
                var stringMessage = new StringMessage
                {
                    StringMode = 0x2A, // TextMode (Static)  
                    StringBody = textStringBody
                };
                var urgentMessage = new Urgent // Display version
                {
                    UrgntMessageType = 0x79, // message    
                    MessageType = 0x71, 
                    MessageLevel = 0x01, // level
                    MessageScroll = new ScrollInfo { ScrollMode = 0x64, ScrollSpeed = 07, PauseTime = 10 },
                    Font = new FontSetting { Size = FontSize.Font16x16, Style = FontStyle.Ming }, 
                    MessageContent = new List<StringMessage> { stringMessage }
                };
                return new Display.Sequence  
                {
                    SequenceNo = (byte)sequenceNo, 
                    IsUrgent = true,
                    UrgentCommand = 0x01,
                    Font = new FontSetting { Size = FontSize.Font16x16, Style = FontStyle.Ming },
                    Messages = new List<IMessage> { urgentMessage }
                };
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
        #endregion

    }
}