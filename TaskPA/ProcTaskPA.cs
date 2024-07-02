using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;

using ASI.Wanda.PA.ProcMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Wanda.DCU.ProcMsg;

namespace ASI.Wanda.DCU.TaskPA
{
    /// <summary>
    /// 處理PA模組執行程序
    /// </summary>
    public class ProcTaskPA : ProcBase
    {

        static int mSEQ = 0; // 計算累進發送端的次數  

        ASI.Lib.Comm.SerialPort.SerialPortLib serial = null;

        private DateTime LastHeartbeatTime = DateTime.Now; //最後連線時間

        private System.Timers.Timer responseTimer;

        private byte[] arrPacketByte; 

        /// <summary>
        /// 處理PA模組執行程序所收到之訊息
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
            else if (pLabel == ProcMsg.MSGFromTaskPDU.Label)
            {
                ProMsgFromUPD(pBody);
            }
          
            return base.ProcEvent(pLabel, pBody);
        } 
        
        /// <summary>
        /// 處理PA模組執行程序所收到之定時訊息
        /// </summary> 
        /// <param name="pMessage"></param>
        /// <returns></returns> 
        public override int ProcTimerEvent(string pMessage) // handle timer message
        {
            //定時回報TaskMain
            if (base.ProcTimerEvent(pMessage) <= 0)
            {
                return -1;
            }
            return 1;
        }
        /// <summary> 
        /// 啟始處理PA模組執行程序
        /// </summary>
        /// <param name="pComputer"></param>
        /// <param name="pProcName"></param>
        /// <returns></returns> 
        public override int StartTask(string pComputer, string pProcName)
        {
            serial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
            serial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
            serial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);  
            mTimerTick = 30;
            mProcName = "TaskPA";
            ///serialPort的開啟 
            var iComPort = ConfigApp.Instance.GetConfigSetting("PAComPort"); 
            var iBaudrate = ConfigApp.Instance.GetConfigSetting("PABaudrate"); 
            string connectionString = $"PortName=COM{iComPort};BaudRate={iBaudrate};DataBits=8;StopBits=One;Parity=None";
            serial.ConnectionString = connectionString;
            var result = serial.Open();
      
            return base.StartTask(pComputer, pProcName);

        }

        void SerialPort_ReceivedEvent(byte[] dataBytes, string source)
        {
            try
            {
                string sRcvTime = System.DateTime.Now.ToString("HH:mm:ss.fff");
                var sHexString = ASI.Lib.Text.Parsing.String.BytesToHexString(dataBytes, " ");

                // Log the initial content of dataBytes
                ASI.Lib.Log.DebugLog.Log($"{mProcName} initial dataBytes: ", sHexString);

                // Form the packet for DCU
                if (dataBytes.Length >= 3)
                {
                    byte dataByte2 = dataBytes[2];
                    ASI.Lib.Log.DebugLog.Log($"{mProcName} dataByte2: ", dataByte2.ToString("X2"));

                    if (dataByte2 == 0x01)
                    {
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} processing 0x01 case", sHexString);
                        dataBytes[2] = 0x06;
                        Array.Resize(ref dataBytes, dataBytes.Length - 1); // Remove the last byte
                        byte newLRC = CalculateLRC(dataBytes);
                        Array.Resize(ref dataBytes, dataBytes.Length + 1); // Add a byte back
                        dataBytes[dataBytes.Length - 1] = newLRC;
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} replied to TaskPA message at {sRcvTime}", sHexString);
                    }
                    else if (dataByte2 == 0x06)
                    {
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} received a correct message from TaskPA", sHexString);
                    }
                    else if (dataByte2 == 0x15)
                    {
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} processing 0x15 case", sHexString);
                        string slog = "";
                        if (dataBytes[4] == 0x01) 
                        {
                            slog = "Indicates packet data length error";
                        }
                        else if (dataBytes[4] == 0x02)
                        {
                            slog = "Indicates LRC error";
                        }
                        else if (dataBytes[4] == 0x03)
                        {
                            slog = "Indicates other errors";
                        }
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} received an error message from TaskPA: {slog} at {sRcvTime}", sHexString);
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log($"{mProcName} received an unknown error message from PA", sHexString); // Log other unknown error message
                    }
                }
                else 
                {
                    ASI.Lib.Log.DebugLog.Log($"{mProcName} dataBytes length less than 3", sHexString);
                    ASI.Lib.Log.DebugLog.Log($"{mProcName} replied to TaskPA message at {sRcvTime}", sHexString); // Log the reply message
                }

                var msg = new ASI.Wanda.DMD.Message.Message(ASI.Wanda.DMD.Message.Message.eMessageType.Command, 01, ASI.Lib.Text.Parsing.Json.SerializeObject(sHexString));

                // Send twice to ensure the broadcast is received
                serial.Send(arrPacketByte); 
                ASI.Lib.Log.DebugLog.Log("Content of the packet sent to PA the first time", sHexString.ToString());
                serial.Send(arrPacketByte); 
                ASI.Lib.Log.DebugLog.Log("Content of the packet sent to PA the second time", sHexString.ToString());

                PAHelper.SendToTaskDMD(2, 1, msg.JsonContent);

                serial.Send(dataBytes); // Resend
            }
            catch (Exception e)
            {
                ASI.Lib.Log.ErrorLog.Log("Error reason", e.ToString());
            }
        }

        void SerialPort_DisconnectedEvent(string source) //斷線處理 
        {
            try
            {
                this.serial.Close(); 
                this.serial = null;
                serial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
                serial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent); 
                serial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);
            }
            catch (Exception)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, "斷線處理錯誤"); 
            }
        }

        private void StopResponseTimer()
        {                                               
            if (responseTimer != null)
            {
                responseTimer.Stop();
                responseTimer.Dispose();
            } 
        }

        private void ResponseTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var sHexString = ASI.Lib.Text.Parsing.String.BytesToHexString(arrPacketByte, " ");
            serial.Send(arrPacketByte); // 重新發送 
            ASI.Lib.Log.DebugLog.Log("未收到回覆，重新傳送一次", sHexString.ToString()); // 紀錄log
        }
        private void StartResponseTimer()
        {
            if (responseTimer != null)
            {
                responseTimer.Start();
            }
        }

        static private byte CalculateLRC(byte[] text)
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
        /// <summary>
        /// 處理TaskPDU的訊息  
        /// </summary> 
        private int ProMsgFromUPD(string pMessage)
        {
            string sLog = "";
         
            try
            {
                MSGFromTaskUPD mSGFromTaskDCU = new MSGFromTaskUPD(new MSGFrameBase(""));

                if (mSGFromTaskDCU.UnPack(pMessage) > 0)
                {
                    if (mSGFromTaskDCU.MessageType == 1)
                    {
                        //DMD內部通訊定義:Ack
                        //從TaskDCU過來不應該有Ack
                        ASI.Lib.Log.ErrorLog.Log(mProcName, $"從TaskDCU來的訊息不應有DMD內部通訊定義:Ack，MessageType:{mSGFromTaskDCU.MessageType}");
                    } 
                    else if (mSGFromTaskDCU.MessageType == 2)
                    {
                        //DMD內部通訊定義:Change/Command
                        string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDCU.JsonData, "JsonObjectName");
                        sLog = $"sJsonObjectName = {sJsonObjectName}"; 
                        ASI.Lib.Log.DebugLog.Log(mProcName, sLog);  
                    }
                    else if (mSGFromTaskDCU.MessageType == 3)
                    {
                        //DMD內部通訊定義:Response 
                        string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDCU.JsonData, "JsonObjectName");
                        sLog = $"sJsonObjectName = {sJsonObjectName}";

                        ASI.Lib.Log.DebugLog.Log(mProcName, sLog);

                        //將訊息傳給DCU
                        var oJsonObject = (ASI.Wanda.DMD.JsonObject.DCU.FromDCU.Res_SendPreRecordMessage)ASI.Wanda.DMD.Message.Helper.GetJsonObject(mSGFromTaskDCU.JsonData);

                        //組封包  
                        var Res_SendPreRecordMessage = new ASI.Wanda.DMD.JsonObject.DCU.FromDCU.Res_SendPreRecordMessage(ASI.Wanda.DMD.Enum.Station.OCC);
                        Res_SendPreRecordMessage.seatID = oJsonObject.seatID;
                        Res_SendPreRecordMessage.msg_id = oJsonObject.msg_id;
                        Res_SendPreRecordMessage.failed_target = oJsonObject.failed_target;

                        //組成給DCU的封包  
                        var MSG = new DMD.Message.Message(ASI.Wanda.DMD.Message.Message.eMessageType.Response, mSGFromTaskDCU.MessageID, ASI.Lib.Text.Parsing.Json.SerializeObject(Res_SendPreRecordMessage));

                        ASI.Lib.Log.DebugLog.Log("RES_SendPreRecordMSGToDCU", MSG.JsonContent); 
                    }
                    else
                    {
                        //無此種訊息類別  
                        ASI.Lib.Log.ErrorLog.Log(mProcName, $"無此種訊息類別，MessageType:{mSGFromTaskDCU.MessageType}");
                    }
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, ex); 
            }
            ASI.Lib.Log.DebugLog.Log("收到TaskUPD回傳資料", pMessage.ToString());

            return -1;
        }
        /// <summary>
        /// 處理TaskDCU的訊息  
        /// </summary> 
        private int ProMsgFromDCU(string pMessage)
        {
            DataBase oDB = null;
            string sLog = "";
            try
            {
                ASI.Wanda.DMD.ProcMsg.MSGFromTaskDCU mSGFromTaskDCU = new DMD.ProcMsg.MSGFromTaskDCU(new MSGFrameBase(""));
                if (mSGFromTaskDCU.UnPack(pMessage) > 0)
                {
                    if (mSGFromTaskDCU.MessageType == 1) 
                    {
                        // DMD內部通訊定義:Ack  
                        // 從TaskDCU過來不應該有Ack  
                        ASI.Lib.Log.ErrorLog.Log(mProcName, $"從TaskDCU來的訊息不應有DMD內部通訊定義:Ack，MessageType:{mSGFromTaskDCU.MessageType}"); 
                    }
                    else if (mSGFromTaskDCU.MessageType == 2)
                    {
                        //DMD內部通訊定義:Change/Command 
                        string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDCU.JsonData, "JsonObjectName"); 
                        sLog = $"sJsonObjectName = {sJsonObjectName}";
                        ASI.Lib.Log.DebugLog.Log(mProcName, sLog);
                    }
                    else if (mSGFromTaskDCU.MessageType == 3)  
                    {
                        //DMD內部通訊定義:Response 
                        string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDCU.JsonData, "JsonObjectName"); 
                        sLog = $"sJsonObjectName = {sJsonObjectName}";
                        ASI.Lib.Log.DebugLog.Log(mProcName, sLog);  
                        //將訊息傳給CMFT 
                        var oJsonObject = (ASI.Wanda.DMD.JsonObject.DCU.FromDCU.Res_SendPreRecordMessage)ASI.Wanda.DMD.Message.Helper.GetJsonObject(mSGFromTaskDCU.JsonData);

                        //組封包 
                        var Res_SendPreRecordMessage = new ASI.Wanda.DMD.JsonObject.DCU.FromDCU.Res_SendPreRecordMessage(ASI.Wanda.DMD.Enum.Station.OCC);  
                        Res_SendPreRecordMessage.seatID = oJsonObject.seatID; 
                        Res_SendPreRecordMessage.msg_id = oJsonObject.msg_id; 
                        Res_SendPreRecordMessage.failed_target = oJsonObject.failed_target;   

                        //組成給DCU的封包   
                        ASI.Lib.Log.DebugLog.Log("RES_SendPreRecordMSGToDCU", mSGFromTaskDCU.JsonData); 
                    }
                    else
                    {
                        //無此種訊息類別   
                        ASI.Lib.Log.ErrorLog.Log(mProcName, $"無此種訊息類別，MessageType:{mSGFromTaskDCU.MessageType}"); 
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

        #region Method 

        //private void SerialPort_ReceivedEvent(ASI.Wanda.PA.Message.Message.MessageType messageType, 
        //   string station, string platForm, string situation)
        //{
        //    ASI.Lib.Log.DebugLog.Log("PA接收", messageType.ToString());
        //    //回應PA
        //    responseTimer = new System.Timers.Timer();
        //    responseTimer.Interval = 3000;  
        //    responseTimer.AutoReset = false; 
        //    responseTimer.Elapsed += ResponseTimerElapsed; 
        //    ///組成封包內容
        //    ASI.Wanda.PA.Message.Message oPAMsg = new ASI.Wanda.PA.Message.Message(messageType); 
            
        //    oPAMsg.station = oPAMsg.GetStationValue(station);
        //    oPAMsg.platform = oPAMsg.GetPlatformFromValue(platForm);
        //    oPAMsg.situation = oPAMsg.GetSituationFromValue(situation);
            
        //    oPAMsg.SEQ = (byte)mSEQ++;  
        //    oPAMsg.LRC = oPAMsg.GetMsgLRC();
        //    arrPacketByte = ASI.Wanda.PA.Message.Helper.Pack(oPAMsg);  

        //    string sHexString = ASI.Lib.Text.Parsing.String.BytesToHexString(arrPacketByte, " ");

        //    //傳送兩次以確保廣播確實收到 
        //    serial.Send(arrPacketByte);
        //    ASI.Lib.Log.DebugLog.Log("傳送給PA的封包的內容第一次", sHexString.ToString());
        //    serial.Send(arrPacketByte);
        //    ASI.Lib.Log.DebugLog.Log("傳送給PA的封包的內容第二次", sHexString.ToString());
            

        //}
        #endregion
    }
}
