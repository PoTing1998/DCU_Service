using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DCU.ProcMsg;
using ASI.Wanda.PA.ProcMsg;
using Display;
using Display.DisplayMode;
using Display.Function;
using System;
using System.Collections.Generic;
using System.Text;


namespace ASI.Wanda.DCU.TaskUPDU
{
    public class ProcTaskUPDU : ProcBase
    {
        private DateTime LastHeartbeatTime = DateTime.Now; //最後連線時間

        static int mSEQ = 0; // 計算累進發送端的次數  
        public string mDMDServerConnStr = "";
        static private Dictionary<string, int> mServerIP = null;
        ASI.Lib.Comm.SerialPort.SerialPortLib serial = null;
        /// <summary>
        /// 要傳送的port匯集起來
        /// </summary>
        List<ASI.Lib.Comm.SerialPort.SerialPortLib> mySerialPorts = new List<Lib.Comm.SerialPort.SerialPortLib>();

        /// <summary>
        /// 處理UPDU模組執行程序所收到之訊息 
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
            else if (pLabel == PA.ProcMsg.MSGFromTaskPDU.Label)
            {
                return ProMsgFromPA(pBody);
            }
            else if (pLabel == ProcMsg.MSGFromTaskPDU.Label)
            {
                return 0;
            }
            else if (pLabel == ProcMsg.MSGFromTaskSDU.Label)
            {
                return 0;
            }
            return base.ProcEvent(pLabel, pBody);
        }

        /// <summary> 
        /// 啟始處理UPDU模組執行程序
        /// </summary>
        /// <param name="pComputer"></param>
        /// <param name="pProcName"></param>
        /// <returns></returns>
        public override int StartTask(string pComputer, string pProcName)
        {
            mTimerTick = 30;
            mProcName = "TaskUPDU";
            serial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
            string sDBIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string sDBPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string sDBName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string sUserID = "postgres";
            string sPassword = "postgres";
            string sCurrentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");
            ///serialPort的開啟 
            serial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
            serial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);
            string iComPort = "1";
            int iBaudrate = 9600;
            string connectionString = $"PortName=COM{iComPort};BaudRate={iBaudrate};DataBits=8;StopBits=One;Parity=None";
            serial.ConnectionString = connectionString;
            var result = serial.Open();

            try
            {
                //"Server='localhost'; Port='5432'; Database='DCUDB'; User Id='postgres'; Password='postgres'";
                if (!ASI.Wanda.DCU.DB.Manager.Initializer(sDBIP, sDBPort, sDBName, sUserID, sPassword, sCurrentUserID))
                {
                    ASI.Lib.Log.ErrorLog.Log(mProcName, $"資料庫連線失敗!{sDBIP}:{sDBPort};userid={sUserID}");
                }
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, $"資料庫連線失敗!{sDBIP}:{sDBPort};userid={sUserID};ex={ex}");
            }

            // ConnToDMDServer();

            return base.StartTask(pComputer, pProcName);
        }

        /// <summary>
        /// 處理TaskDMD的訊息
        /// </summary>
        private int ProMsgFromDMD(string pMessage)
        {
            DataBase oDB = null;
            try
            {
                ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD mSGFromTaskDMD = new MSGFromTaskDMD(new MSGFrameBase(""));
                if (mSGFromTaskDMD.UnPack(pMessage) > 0)
                {
                    string sJsonData = mSGFromTaskDMD.JsonData;
                    string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDMD.JsonData, "JsonObjectName");
                    string sStationID = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDMD.JsonData, "StationID");
                    string sSeatID = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "SeatID");
                    string dbName1 = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "dbName1");
                    string target_du = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "target_du");


                    Guid iMsgID = new Guid(mSGFromTaskDMD.MessageID.ToString());
                    ASI.Lib.Log.DebugLog.Log(mProcName, $"收到來自TaskDMD的訊息，SeatID:{sSeatID}；MsgID:{iMsgID}；JsonObjectName:{sJsonObjectName}");
                    if(dbName1 == "dmd_train_message")
                    {
                    }
                    else 
                    {
                        //判斷收到的訊息ID

                    }

                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, ex);
            }
            finally
            {
                if (oDB != null)
                {
                    oDB.Close();
                }
            }
            return -1;
        }


        /// <summary>
        /// 處理TaskPA的訊息
        /// </summary> 
        private int ProMsgFromPA(string pMessage)
        {
            DataBase oDB = null;
            string sRcvTime = System.DateTime.Now.ToString("HH:mm:ss.fff");

            try
            {
                ASI.Wanda.DCU.ProcMsg.MSGFromTaskPA MSGFromTaskPA = new ProcMsg.MSGFromTaskPA(new MSGFrameBase(""));
                if (MSGFromTaskPA.UnPack(pMessage) > 0)
                {
                    string sJsonData = MSGFromTaskPA.JsonData;
                    var dataBytes = ASI.Lib.Text.Parsing.String.HexStringToBytes(MSGFromTaskPA.JsonData, 2);
                    var sHexString = ASI.Lib.Text.Parsing.String.BytesToHexString(dataBytes, " ");
                    if (dataBytes.Length >= 3 && dataBytes[2] == 0x01)
                    {
                        dataBytes[2] = 0x06;
                        // 去掉最後一個byte 
                        Array.Resize(ref dataBytes, dataBytes.Length - 1);
                        var NewLRC = calculateLRC(dataBytes);
                        // 並且重新計算LRC並加回去  
                        Array.Resize(ref dataBytes, dataBytes.Length + 1);
                        dataBytes[dataBytes.Length - 1] = NewLRC;

                        ASI.Lib.Log.DebugLog.Log("回復PA收到的訊息 " + sRcvTime, sHexString.ToString());//log紀錄  
                        SendBroadcastMessage(PA.Message.Message.MessageType.msg, dataBytes[7], dataBytes[8], dataBytes[9]);
                    }
                    else if (dataBytes.Length >= 3 && dataBytes[2] == 0x06)
                    {
                        SendBroadcastMessage(PA.Message.Message.MessageType.correct, dataBytes[7], dataBytes[8], dataBytes[9]);
                        ASI.Lib.Log.DebugLog.Log("PA收到正確的訊息 " + sRcvTime, sHexString.ToString());//log紀錄 
                    }
                    else if (dataBytes.Length >= 3 && dataBytes[2] == 0x15)
                    {
                        var slog = "";
                        if (dataBytes[4] == 0x01)  slog = "代表封包資料長度錯誤"; 
                        else if (dataBytes[4] == 0x02) slog = "代表LRC錯誤";
                        else if (dataBytes[4] == 0x03) slog = "代表其他錯誤";

                        ASI.Lib.Log.DebugLog.Log("PA收到錯誤的訊息 " + slog + sRcvTime, sHexString.ToString());//log紀錄
                    }

                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, ex);
            }
            finally
            {
                if (oDB != null)
                {
                    oDB.Close();
                }
            }

            return -1;
        }
        #region SerialPort 
        public void SendDataToAvailableSerialPorts(ASI.Lib.Comm.SerialPort.SerialPortLib port, byte[] dataToSend)
        {
            // 判斷 SerialPortLib 是否開啟
            if (port.IsOpen)
            {
                // 在開啟的 SerialPortLib 上傳送資料
                port.Send(dataToSend);
            }
            else
            {
                // 在這裡處理 SerialPortLib 未開啟的情況，例如顯示錯誤訊息或進行其他處理
                ASI.Lib.Log.ErrorLog.Log(mProcName, "SerialPort尚未開啟");
            }
        }

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
            catch (Exception)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, "斷線處理錯誤");
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

            //傳送兩次以確保廣播確實收到
            serial.Send(arrPacketByte);
            ASI.Lib.Log.DebugLog.Log("傳送給PA的封包的內容第一次", sHexString.ToString());
            serial.Send(arrPacketByte);
            ASI.Lib.Log.DebugLog.Log("傳送給PA的封包的內容第二次", sHexString.ToString());
        }

        void SendMessageToDisplay()
        {

            var processor = new PacketProcessor();

            var textStringBody = new TextStringBody
            {
                RedColor = 0xFF,
                GreenColor = 0x00,
                BlueColor = 0x00,
                StringText = "火災探測器動作人員確認中請勿驚慌"
            };
            var stringMessage = new StringMessage
            {
                StringMode = 0x2A, // TextMode (Static)   
                StringBody = textStringBody
            };
            var fullWindowMessage = new FullWindow //Display version
            {
                MessageType = 0x71, // FullWindow message
                MessageLevel = 0x01, //  level 
                MessageScroll = new ScrollInfo { ScrollMode = 64, ScrollSpeed = 07, PauseTime = 10 },
                Font = new FontSetting { Size = FontSize.Font16x16, Style = FontStyle.Ming },
                MessageContent = new List<StringMessage> { stringMessage }
            };
            var sequence1 = new Display.Sequence
            {
                SequenceNo = 1,
                Messages = new List<IMessage> { fullWindowMessage }
            };

            var startCode = new byte[] { 0xAA, 0x55 };
            var function = new PassengerInfoHandler(); // Use PassengerInfoHandler  
            var packet = processor.CreatePacket(startCode, new List<byte> { 0x01, 0x02, 0x03 }, function.FunctionCode, new List<Sequence> { sequence1 });
            var serializedData = processor.SerializePacket(packet);
            serial.Send(serializedData);

        }



        #region Prviate Method 
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





        #endregion

    }
}
