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
        private DateTime LastHeartbeatTime = DateTime.Now; //最後連線時間

        static int mSEQ = 0; // 計算累進發送端的次數  
        public string mDMDServerConnStr = "";
        static private Dictionary<string, int> mServerIP = null;
        ASI.Lib.Comm.SerialPort.SerialPortLib serial = null;
        /// <summary>
        /// 要傳送的port匯集起來
        /// </summary>
        List<ASI.Lib.Comm.SerialPort.SerialPortLib> mySerialPorts = new List<Lib.Comm.SerialPort.SerialPortLib>();

        public static string Green = "#008000";
        public static string Yellow = "#ffff00";
        public static string Blue = "#0000FF";
        public static string Red = "#FF0000";
        public static string White = "#FFFFFF";
        public static string Black = "#000000";
        public static int PaltFormIndex = 1;
        public static string sCheck = ConfigApp.Instance.GetConfigSetting("FireDetectorCheckInProgress");
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
            mProcName = "TaskLPD";
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
            var iComPort = ConfigApp.Instance.GetConfigSetting("LPDComPort"); ;
            var iBaudrate = ConfigApp.Instance.GetConfigSetting("LPDBaudrate"); ;
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




                    Guid iMsgID = new Guid(mSGFromTaskDMD.MessageID.ToString());
                    ASI.Lib.Log.DebugLog.Log(mProcName, $"收到來自TaskDMD的訊息，SeatID:{sSeatID}；MsgID:{iMsgID}；JsonObjectName:{sJsonObjectName}");
                    ASI.Wanda.DCU.DB.Tables.DCU.userControlPanel.emergency(1, 71);


                    //判斷收到的訊息ID
                    //  SetFullWindowDisplay(ASI.Wanda.DCU.DB.Tables.DMD.dmdTrainMessage.Priority(iMsgID));
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
            //從config當中讀取火災內容 
            var sCheck = ConfigApp.Instance.GetConfigSetting("FireDetectorCheckInProgress");
            var sEmergency = ConfigApp.Instance.GetConfigSetting("FireEmergencyEvacuateCalmly");
            var sCleared = ConfigApp.Instance.GetConfigSetting("FireAlarmCleared");
            var sDetector = ConfigApp.Instance.GetConfigSetting("FireDetectorClearConfirmed");
            try
            {
                ASI.Wanda.DCU.ProcMsg.MSGFromTaskPA MSGFromTaskPA = new ProcMsg.MSGFromTaskPA(new MSGFrameBase(""));
                if (MSGFromTaskPA.UnPack(pMessage) > 0)
                {
                    string sJsonData = MSGFromTaskPA.JsonData;
                    var dataBytes = ASI.Lib.Text.Parsing.String.HexStringToBytes(MSGFromTaskPA.JsonData, 2);
                    var sHexString = ASI.Lib.Text.Parsing.String.BytesToHexString(dataBytes, " ");
                    //拆解封包 依照收到不同的情況的內容投射在顯示器畫面上


                    //組成要送去看板的封包內容 
                    //      SetFullWindowDisplay(1);


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


        #region serialport
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
        public byte calculateLRC(byte[] text)
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
