using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DCU.ProcMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.TaskSDU
{
    public class ProcTaskSDU : ProcBase
    {



        #region constructor
        private DateTime LastHeartbeatTime = DateTime.Now; //最後連線時間

        static int mSEQ = 0; // 計算累進發送端的次數  
        public string mDMDServerConnStr = "";
        static private Dictionary<string, int> mServerIP = null;
        ASI.Lib.Comm.SerialPort.SerialPortLib serial = null;
        /// <summary>
        /// 要傳送的port匯集起來
        /// </summary>
        List<ASI.Lib.Comm.SerialPort.SerialPortLib> mySerialPorts = new List<Lib.Comm.SerialPort.SerialPortLib>();
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
            else if (pLabel == MSGFromTaskPA.Label)
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
            mProcName = "TaskSDU";
            mServerIP = new Dictionary<string, int>();
            string sDBIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string sDBPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string sDBName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string sUserID = "postgres";
            string sPassword = "postgres";
            string sCurrentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");

            ///serialPort的開啟 
            serial = new ASI.Lib.Comm.SerialPort.SerialPortLib();
            serial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
            serial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);
            var iComPort = ConfigApp.Instance.GetConfigSetting("SDUComPort"); ;
            var iBaudrate = ConfigApp.Instance.GetConfigSetting("SDUBaudrate"); ;
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
                    int iMsgID = mSGFromTaskDMD.MessageID;
                    ASI.Lib.Log.DebugLog.Log(mProcName, $"收到來自TaskDMD的訊息，SeatID:{sSeatID}；MsgID:{iMsgID}；JsonObjectName:{sJsonObjectName}");
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
                      //  SendMSG(PA.Message.Message.MessageType.msg, dataBytes[7], dataBytes[8], dataBytes[9]);
                    }
                    else if (dataBytes.Length >= 3 && dataBytes[2] == 0x06)
                    {
                       // SendMSG(PA.Message.Message.MessageType.correct, dataBytes[7], dataBytes[8], dataBytes[9]);
                        ASI.Lib.Log.DebugLog.Log(mProcName+"PA收到正確的訊息 " + sRcvTime, sHexString.ToString());//log紀錄 
                    }
                    else if (dataBytes.Length >= 3 && dataBytes[2] == 0x15)
                    {
                        var slog = "";
                        if (dataBytes[4] == 0x01)
                        {
                            slog = "代表封包資料長度錯誤";
                        }
                        else if (dataBytes[4] == 0x02)
                        {
                            slog = "代表LRC錯誤";
                        }
                        else if (dataBytes[4] == 0x03)
                        {
                            slog = "代表其他錯誤";
                        }
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
        #region serialPort 
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
                ASI.Lib.Log.DebugLog.Log(mProcName, "顯示器的狀態收到的訊息" + sHexString.ToString());  //處理顯示器回報的狀態
            }
            else if (dataBytes[4] != 0x00)
            {
                if (dataBytes[4] == 0x01) { ASI.Lib.Log.ErrorLog.Log(mProcName, "曾經有通訊不良"); }
                else if (dataBytes[4] == 0x02) { ASI.Lib.Log.ErrorLog.Log(mProcName, "處於關機狀態 "); }
                else if (dataBytes[4] == 0x04) { ASI.Lib.Log.ErrorLog.Log(mProcName, "通訊逾時"); }
                else if (dataBytes[4] == 0x07) { ASI.Lib.Log.ErrorLog.Log(mProcName, " 1/2/4 多重組合 "); }
            }
            ASI.Lib.Log.DebugLog.Log(mProcName, "從顯示器收到的訊息" + sHexString.ToString());//log紀錄 
        }




        #endregion
    }






}
