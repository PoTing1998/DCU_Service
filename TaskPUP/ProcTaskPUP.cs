using ASI.Lib.Config;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DCU.ProcMsg;

using Display;
using Display.Function;

using System;
using System.Collections.Generic;
using DCU_Frame;
using TaskPUP.Strategies;
using TaskPUP.PAMessage;
using TaskPUP.Constants;

namespace ASI.Wanda.DCU.TaskPUP
{
    public class ProcTaskPUP : ProcBase
    {
        #region constructor
        static int mSEQ = 0; // 計算累進發送端的次數  
        ASI.Lib.Comm.SerialPort.SerialPortLib _mSerial = null;
        static string Station_ID = ConfigApp.Instance.GetConfigSetting("Station_ID");
        static string _mDU_ID = DU_ID.LG01_PDU_17.ToString();
        static bool _mFront = true;
        static bool _mBack = false;
        #endregion

        #region MSMQ Method
        /// <summary>
        /// 處理PUP模組執行程序所收到之訊息 
        /// </summary>
        /// <param name="pLabel"></param>
        /// <param name="pBody"></param>
        /// <returns></returns>
        public override int ProcEvent(string pLabel, string pBody)
        {
            LogFile.Display(pBody);

            if (pLabel == MSGFinish.Label)
                return 0;
            if(pLabel == MSGFromTaskDMD.Label || pLabel == MSGFromTaskPUP.Label)
                return ProMsgFromDMD(pBody);
            if (pLabel == PA.ProcMsg.MSGFromTaskPA.Label)
                return ProMsgFromPA(pBody);
            
            return base.ProcEvent(pLabel, pBody);
        }

        /// <summary> 
        /// 啟始處理PUP模組執行程序
        /// </summary>
        /// <param name="pComputer"></param>
        /// <param name="pProcName"></param>
        /// <returns></returns>
        public override int StartTask(string pComputer, string pProcName)
        {
            mTimerTick = 30;
            _mProcName = "TaskPUP";

            try
            {
                if (!InitSerial())
                {
                    return -1;
                }

                if (!InitDatabase())
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"啟動任務時發生例外：{ex.Message}");
                return -1;
            }

            return base.StartTask(pComputer, pProcName);
        }

        /// <summary>
        /// 主方法：處理來自 TaskDMD 的消息
        /// </summary>
        /// <param name="pMessage">接收到的消息字符串</param>
        /// <returns>處理結果狀態碼，-1 表示失敗</returns>
        private int ProMsgFromDMD(string pMessage)
        {
            try
            {
                var mSGFromTaskDMD = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD(new MSGFrameBase(""));
                if (mSGFromTaskDMD.UnPack(pMessage) > 0)
                {
                    string jsonData = mSGFromTaskDMD.JsonData;
                    string jsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(jsonData, "JsonObjectName");

                    // 使用工廠建立對應策略物件
                    var strategy = MessageStrategyFactory.GetStrategy(
                        jsonObjectName,
                        jsonData,
                        _mProcName,
                        _mSerial,
                        Station_ID,
                        OpenDisplay,
                        CloseDisplay
                    );

                    if (strategy != null)
                    {
                        strategy.Execute(jsonData);
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log(_mProcName, $"未找到對應策略，JsonObjectName: {jsonObjectName}");
                    }
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, ex.ToString());
            }

            return -1;
        }

        /// <summary>
        /// 處理TaskPA的訊息   
        /// </summary>  
        private int ProMsgFromPA(string pMessage)
        {
            string receiveTime = DateTime.Now.ToString("HH:mm:ss.fff");
            var paMessage = new PAMessage(_mProcName, _mSerial);

            try
            {
                var msg = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskPA(new MSGFrameBase(""));

                if (msg.UnPack(pMessage) <= 0)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"訊息解析失敗，UnPack 回傳值 <= 0：{pMessage}");
                    return -1;
                }

                string hexString = msg.JsonData;
                ASI.Lib.Log.DebugLog.Log(_mProcName, $"接收到來自 TaskPA 的訊息：{hexString}");

                byte[] dataBytes = paMessage.HexStringToBytes(hexString);

                if (dataBytes == null || dataBytes.Length < 3)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"無法處理資料，dataBytes 長度 < 3：{hexString}");
                    return -1;
                }

                if (dataBytes.Length >= 10) //正常的處理
                {
                    paMessage.ProcessDataBytes(dataBytes);
                }
                else
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, $"dataBytes 長度 < 10，跳過 ProcessDataBytes：{hexString}");
                }

                paMessage.ProcessByteAtIndex2(dataBytes, receiveTime, hexString);
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"ProMsgFromPA 發生例外：{ex}");
            }

            return -1;
        }
        #endregion

        #region SerialPort 
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
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, "斷線處理錯誤" + ex.ToString());
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
                ASI.Lib.Log.DebugLog.Log(_mProcName, _mProcName + "顯示器的狀態收到的訊息" + sHexString.ToString());  //處理顯示器回報的狀態
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
        #endregion

        #region Prviate Method 
        private bool InitSerial()
        {
            string comPort  = AppConfig.ComPort;
            string baudRate = AppConfig.BaudRate;

            string connectionString = $"PortName=COM{comPort};BaudRate={baudRate};DataBits=8;StopBits=One;Parity=None";

            _mSerial = new ASI.Lib.Comm.SerialPort.SerialPortLib
            {
                ConnectionString = connectionString
            };
            _mSerial.ReceivedEvent += new ASI.Lib.Comm.ReceivedEvents.ReceivedEventHandler(SerialPort_ReceivedEvent);
            _mSerial.DisconnectedEvent += new ASI.Lib.Comm.ReceivedEvents.DisconnectedEventHandler(SerialPort_DisconnectedEvent);

            int result = _mSerial.Open();
            if (result != 0)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, "串列埠開啟失敗");
                return false;
            }

            return true;
        }
        private bool InitDatabase()
        {
            string dbIP          = AppConfig.DbIP;
            string dbPort        = AppConfig.DbPort;
            string dbName        = AppConfig.DbName;
            string currentUserID = AppConfig.CurrentUserID;
            string dbUserID      = AppConfig.DBUserID;
            string dbPassword    = AppConfig.DBPassword;

            ASI.Lib.Log.DebugLog.Log(_mProcName, "初始化資料庫連線中...");

            bool dbInitSuccess = ASI.Wanda.DCU.DB.Manager.Initializer(
                dbIP, dbPort, dbName, dbUserID, dbPassword, currentUserID
            );

            if (!dbInitSuccess)
            {
                string err = $"資料庫連線失敗！({dbIP}:{dbPort}, 使用者: {dbUserID})";
                ASI.Lib.Log.ErrorLog.Log(_mProcName, err);
                return false;
            }

            ASI.Lib.Log.DebugLog.Log(_mProcName, "資料庫連線成功.");
            return true;
        }

        public void CloseDisplay()
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var Off = new byte[] { 0x3A, 0X01 };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, _mBack);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, _mFront);
            var packetOff = processor.CreatePacketOff(startCode, new List<byte> { Convert.ToByte(front), Convert.ToByte(back) }, function.FunctionCode, Off);
            var serializedDataOff = processor.SerializePacket(packetOff);
            _mSerial.Send(serializedDataOff);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面關閉", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
        }
        public void OpenDisplay()
        {
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var Open = new byte[] { 0x3A, 0X00 };
            var front = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, _mBack);
            var back = ASI.Wanda.DCU.DB.Tables.DCU.dulist.GetPanelIDByDuAndOrientation(_mDU_ID, _mFront);
            var packetOpen = processor.CreatePacketOff(startCode, new List<byte> { Convert.ToByte(front), Convert.ToByte(back) }, function.FunctionCode, Open);
            var serializedDataOpen = processor.SerializePacket(packetOpen);
            _mSerial.Send(serializedDataOpen);
            ASI.Lib.Log.DebugLog.Log(_mProcName + "顯示畫面開啟", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen));

        }
        #endregion
    }
}