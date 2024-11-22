using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DMD.ProcMsg;
using ASI.Wanda.DMD.TaskDMD;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;




namespace ASI.Wanda.DCU.TaskDMD
{
    /// <summary>
    /// 處理DMD模組執行程序
    /// </summary>
    public class ProcTaskDMD : ProcBase
    {
        #region construct
        private ASI.Wanda.DMD.DMD_API mDMD_API = null;
        public string DCUSation;
        /// <summary>
        /// 最後一次收到DMD訊息的時間
        /// </summary>
        private System.DateTime LastHeartbeatTime = System.DateTime.Now;
        /// <summary>
        /// 與DMD Server的連線狀態
        /// </summary>

        private const int ConnectionInterval = 10000; // 每10秒
        public string mDMDServerConnStr = "";
        private int _initialDelay = 2000;    // 重試延遲時間（毫秒）
        private bool _isConnecting = false;  // 標記當前是否在連接過程中
        private bool _isConnected = false;   // 標記是否已連接成功
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

            else if (pLabel == MSGFromTaskDCU.Label)
            {
                return ProMsgFromDCU(pBody);
            }
            return base.ProcEvent(pLabel, pBody);
        }
        /// <summary>
        /// 處理DMD模組執行程序所收到之定時訊息  
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
        /// 啟始處理DMD模組執行程序 
        /// </summary>
        /// <param name="pComputer"></param>
        /// <param name="pProcName"></param>
        /// <returns></returns>
        public override int StartTask(string pComputer, string pProcName)
        {
            mTimerTick = 30;
            _mProcName = "TaskDMD";
            // DCU Database Configuration
            string sDCU_DBIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string sDCU_DBPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string sDCU_DBName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string sDMD_DBIP = ConfigApp.Instance.GetConfigSetting("DMD_DB_IP");
            string sDMD_DBPort = ConfigApp.Instance.GetConfigSetting("DMD_DB_Port");
            string sDMD_DBName = ConfigApp.Instance.GetConfigSetting("DMD_DB_Name");

            string sUserID = "postgres";
            string sPassword = "postgres";
            string sCurrentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");
            // Default to an error state
            try
            {
                // 1. 啟動 ScheduledTask，加入偵錯日誌
                ASI.Lib.Log.DebugLog.Log(_mProcName, "嘗試啟動 ScheduledTask...");
                var task = new ScheduledTask();

                task.StartPowerSettingScheduler("LG01");
                ASI.Lib.Log.DebugLog.Log(_mProcName, "ScheduledTask 已啟動成功.");

                // 2. 初始化資料庫連線
                ASI.Lib.Log.DebugLog.Log(_mProcName, "嘗試初始化資料庫連線...");

                // 嘗試初始化 DMD 資料庫連線
                if (!ASI.Wanda.DMD.DB.Manager.Initializer(sDMD_DBIP, sDMD_DBPort, sDMD_DBName, sUserID, sPassword, sCurrentUserID))
                {
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, $"DMD資料庫連線失敗!{sDMD_DBIP}:{sDMD_DBPort};userid={sUserID}");
                    return -1; // 返回錯誤代碼
                }
                // 嘗試初始化 DCU 資料庫連線  
                if (!ASI.Wanda.DCU.DB.Manager.Initializer(sDCU_DBIP, sDCU_DBPort, sDCU_DBName, sUserID, sPassword, sCurrentUserID))
                {
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, $"DCU資料庫連線失敗!{sDCU_DBIP}:{sDCU_DBPort};userid={sUserID}");
                    return -1; // 返回錯誤代碼
                }

            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, $"資料庫連線失敗! Exception: {ex.Message}");
                return -1; // 返回錯誤代碼 
            }
            ConnToDMDServer();
            ASI.Lib.Log.DebugLog.Log(_mProcName, "StartTask 成功啟動.");
            return base.StartTask(pComputer, pProcName);
        }

        /// <summary>
        /// 從DMDServer接收訊息
        /// </summary>
        /// <param name="DMDServerMessage"></param> 
        private void DMD_API_ReceivedEvent(ASI.Wanda.DMD.Message.Message DMDServerMessage)
        {
            string sLog = "";
            try
            {
                var sRcvTime = System.DateTime.Now.ToString("HH:mm:ss.fff");
                var sByteArray = ASI.Lib.Text.Parsing.String.BytesToHexString(DMDServerMessage.CompleteContent, "");
                var sJsonData = DMDServerMessage.JsonContent;
                var sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "JsonObjectName");

                //建立DMDHelper並將 DMD_API的send 委派 
                var DMDHelper = new TaskDMDHelper<ASI.Wanda.DMD.DMD_API>(mDMD_API, (api, message) => api.Send(message));
                ////判斷車站
                int iMsgID = DMDServerMessage.MessageID;

                if (DMDServerMessage.MessageType == ASI.Wanda.DMD.Message.Message.eMessageType.Ack)
                {
                    HandleAckMessage(DMDServerMessage, DMDHelper);
                }
                else if (DMDServerMessage.MessageType == ASI.Wanda.DMD.Message.Message.eMessageType.Command)
                {
                    HandleCommandMessage(DMDServerMessage, DMDHelper, sByteArray, sJsonData, sJsonObjectName, iMsgID);
                }
                else if (DMDServerMessage.MessageType == DMD.Message.Message.eMessageType.Response)
                {
                    HandleUnexpectedResponse(DMDServerMessage);
                }
                else
                {
                    sLog = string.Format("無此種訊息類別:[{0}]", DMDServerMessage.MessageType);
                }

            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskDMD", ex);
            }
        }
        private void DMD_API_DisconnectedEvent(string source)
        {
            ASI.Lib.Log.DebugLog.Log(_mProcName, "與 DMD Server 的連接已斷開，將嘗試重新連接。");
            _isConnected = false;  // 標記為未連接
            ConnToDMDServer();     // 斷線後立即重試連接
        }
        /// <summary>
        /// 結束處理DMD模組執行程序
        /// </summary>
        public override void StopTask()
        {
            if (mDMD_API != null)
            {
                mDMD_API.Dispose();
                mDMD_API = null;
            }

            base.StopTask();
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
                ASI.Wanda.DMD.ProcMsg.MSGFromTaskDCU mSGFromTaskDCU = new MSGFromTaskDCU(new MSGFrameBase(""));
                if (mSGFromTaskDCU.UnPack(pMessage) > 0)
                {
                    if (mSGFromTaskDCU.MessageType == 1)
                    {
                        //DMD內部通訊定義:Ack  
                        //從TaskDCU過來不應該有Ack
                        ASI.Lib.Log.ErrorLog.Log(_mProcName, $"從TaskDCU來的訊息不應有DMD內部通訊定義:Ack，MessageType:{mSGFromTaskDCU.MessageType}"); ;
                    }
                    else if (mSGFromTaskDCU.MessageType == 2)
                    {
                        //DMD內部通訊定義:Change/Command  
                        string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDCU.JsonData, "JsonObjectName");
                        sLog = $"sJsonObjectName = {sJsonObjectName}";
                        ASI.Lib.Log.DebugLog.Log(_mProcName, sLog);
                    }
                    else if (mSGFromTaskDCU.MessageType == 3)
                    {
                        //DMD內部通訊定義:Response  
                        string sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(mSGFromTaskDCU.JsonData, "JsonObjectName");
                        sLog = $"sJsonObjectName = {sJsonObjectName}";
                        ASI.Lib.Log.DebugLog.Log(_mProcName, sLog);
                        //將訊息傳給DMD
                        var oJsonObject = (ASI.Wanda.DMD.JsonObject.DCU.FromDCU.Res_SendPreRecordMessage)ASI.Wanda.DMD.Message.Helper.GetJsonObject(mSGFromTaskDCU.JsonData);

                        //組封包 
                        var Res_SendPreRecordMessage = new ASI.Wanda.DMD.JsonObject.DCU.FromDCU.Res_SendPreRecordMessage(ASI.Wanda.DMD.Enum.Station.OCC);
                        Res_SendPreRecordMessage.seatID = oJsonObject.seatID;
                        Res_SendPreRecordMessage.msg_id = oJsonObject.msg_id;
                        Res_SendPreRecordMessage.failed_target = oJsonObject.failed_target;

                        //組成給DCU的封包  
                        var MSG = new DMD.Message.Message(ASI.Wanda.DMD.Message.Message.eMessageType.Response, mSGFromTaskDCU.MessageID, ASI.Lib.Text.Parsing.Json.SerializeObject(Res_SendPreRecordMessage));
                        mDMD_API.Send(MSG);
                        ASI.Lib.Log.DebugLog.Log("RES_SendPreRecordMSGToDCU", MSG.JsonContent);
                    }
                    else
                    {
                        //無此種訊息類別 
                        ASI.Lib.Log.ErrorLog.Log(_mProcName, $"無此種訊息類別，MessageType:{mSGFromTaskDCU.MessageType}");
                    }
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, ex);
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
        /// 與DMD Server連線 
        /// </summary>
        public void ConnToDMDServer()
        {
            if (_isConnected)
            {
                ASI.Lib.Log.DebugLog.Log(_mProcName, "已經成功連接 DMD Server，不需要重複連接。");
                return;
            }

            if (_isConnecting)
            {
                ASI.Lib.Log.DebugLog.Log(_mProcName, "目前正在嘗試連接 DMD Server，請稍後再試。");
                return;
            }

            _isConnecting = true;  // 開始連接過程

            while (!_isConnected)   // 當未連接成功時，持續嘗試連接
            {
                try
                {
                    DisconnectExistingDMDAPI();  // 斷開已存在的連接

                    mDMD_API = new ASI.Wanda.DMD.DMD_API();
                    mDMD_API.ReceivedEvent += DMD_API_ReceivedEvent;
                    mDMD_API.DisconnectedEvent += DMD_API_DisconnectedEvent; // 監聽斷線事件

                    mDMDServerConnStr = ConfigApp.Instance.GetConfigSetting("DMD_Server");

                    int iResult = mDMD_API.Initial(mDMDServerConnStr);
                    if (iResult == 0)
                    {
                        // 連接成功
                        _isConnected = true;
                        _isConnecting = false;  // 停止連接狀態標記
                        ASI.Lib.Log.DebugLog.Log(_mProcName, "與 DMD Server 的 socket 開啟成功");
                        LastHeartbeatTime = DateTime.Now;
                    }
                    else
                    {
                        // 連接失敗，記錄日誌，並等待後重試
                        ASI.Lib.Log.DebugLog.Log(_mProcName, $"與 DMD Server 的連接失敗，返回值: {iResult}，將在 {_initialDelay / 1000} 秒後重試。");
                        Thread.Sleep(_initialDelay); // 延遲後重試
                    }
                }
                catch (Exception ex)
                {
                    // 捕捉連接異常並記錄，等待後重試
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, $"連接 DMD Server 發生例外: {ex.Message}，將在 {_initialDelay / 1000} 秒後重試。");
                    Thread.Sleep(_initialDelay); // 延遲後重試
                }
            }
        }

        private void DisconnectExistingDMDAPI()
        {
            if (mDMD_API != null)
            {
                mDMD_API.ReceivedEvent -= DMD_API_ReceivedEvent;
                mDMD_API.DisconnectedEvent -= DMD_API_DisconnectedEvent;
                mDMD_API.Dispose();
                ASI.Lib.Log.DebugLog.Log(_mProcName, "Existing DMD_API disconnected and disposed.");
            }
        }



        #region 訊息處理
        private void HandleAckMessage(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper)
        {
            string sLog = string.Format("Ack，訊息識別碼:[{0}]", DMDServerMessage.MessageID);
            DMDHelper.HandleAckMessage(DMDServerMessage);
        }
        private void HandleCommandMessage(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper, string sByteArray, string sJsonData, string sJsonObjectName, int iMsgID)
        {
            string sLog = $"從DMD Server收到:{sByteArray}；訊息類別碼:{DMDServerMessage.MessageType}；識別碼:{iMsgID}；長度:{DMDServerMessage.MessageLength}；內容:{sJsonData}；JsonObjectName:{sJsonObjectName}";
            ASI.Lib.Log.DebugLog.Log("FromDMD_server", $"{sLog}\r\n");

            // 根據 JsonObjectName 處理不同的訊息類型  
            switch (sJsonObjectName)
            {
                case ASI.Wanda.DMD.TaskDMD.Constants.SendPreRecordMsg:
                    HandleSendPreRecordMessage(DMDServerMessage, DMDHelper);
                    break;
                case ASI.Wanda.DMD.TaskDMD.Constants.SendInstantMsg:
                    HandleSendInstantMessage(DMDServerMessage, DMDHelper);
                    break;
                case ASI.Wanda.DMD.TaskDMD.Constants.ScheduleSetting:
                    HandleScheduleSetting(DMDServerMessage, DMDHelper);
                    break;
                case ASI.Wanda.DMD.TaskDMD.Constants.PreRecordMessageSetting:
                    HandlePreRecordMessageSetting(DMDServerMessage, DMDHelper);
                    break;
                case ASI.Wanda.DMD.TaskDMD.Constants.TrainMessageSetting:
                    HandleTrainMessageSetting(DMDServerMessage, DMDHelper);
                    break;
                case ASI.Wanda.DMD.TaskDMD.Constants.GroupSetting:
                    HandleGroupSetting(DMDServerMessage, DMDHelper);
                    break;
                case ASI.Wanda.DMD.TaskDMD.Constants.ParameterSetting:
                    DMDHelper.HandleAckMessage(DMDServerMessage);
                    break;
                case ASI.Wanda.DMD.TaskDMD.Constants.PowerTimeSetting:
                    HandlePowerTimeSetting(DMDServerMessage, DMDHelper);
                    break;
            }
        }
        #region 各處理的method
        private void HandleSendPreRecordMessage(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper)
        {
            var oJsonObject = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
            var sendPreRecordMessage = new DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                seatID = oJsonObject.seatID,
                msg_id = oJsonObject.msg_id,
                target_du = oJsonObject.target_du
            };

            DMDHelper.UpdataConfig();
            DMDHelper.UpdateDCUPlayList();
            DMDHelper.UpdataDCUPreRecordMessage();
            SendToAllTasks(DMDHelper, sendPreRecordMessage);
        }

        private void HandleSendInstantMessage(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper)
        {
            var oJsonObjectInstantMessage = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendInstantMessage)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
            var sendInstantMessage = new DMD.JsonObject.DCU.FromDMD.SendInstantMessage(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                seatID = oJsonObjectInstantMessage.seatID,
                msg_id = oJsonObjectInstantMessage.msg_id,
                target_du = oJsonObjectInstantMessage.target_du
            };

            DMDHelper.UpdataConfig();
            DMDHelper.UpdateDCUPlayList();
            DMDHelper.UpdataDCUInstantMessage();
            SendToAllTasks(DMDHelper, sendInstantMessage);
        }

        private void HandleScheduleSetting(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper)
        {
            var oJsonObjectSendScheduleSetting = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendScheduleSetting)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
            var sendScheduleSetting = new DMD.JsonObject.DCU.FromDMD.SendScheduleSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                seatID = oJsonObjectSendScheduleSetting.seatID,
                sched_id = oJsonObjectSendScheduleSetting.sched_id,
                SqlCommand = oJsonObjectSendScheduleSetting.SqlCommand
            };

            DMDHelper.UpSchedule();
            DMDHelper.UpDMDSchedulePlaylist();
            DMDHelper.UpdataDCUPreRecordMessage();
            SendToAllTasks(DMDHelper, sendScheduleSetting);
        }

        private void HandlePreRecordMessageSetting(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper)
        {
            var oJsonObjectPreRecordMessageSetting = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PreRecordMessageSetting)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
            var PreRecordMessageSetting = new DMD.JsonObject.DCU.FromDMD.PreRecordMessageSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                seatID = oJsonObjectPreRecordMessageSetting.seatID,
                msg_id = oJsonObjectPreRecordMessageSetting.msg_id,
                SqlCommand = oJsonObjectPreRecordMessageSetting.SqlCommand
            };

            DMDHelper.UpdataConfig();
            DMDHelper.UpdateDCUPlayList();
            DMDHelper.UpdataDCUPreRecordMessage();
            SendToAllTasks(DMDHelper, PreRecordMessageSetting);
        }

        private void HandlePowerTimeSetting(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper)
        {
            DMDHelper.UpDateDMDPowerSetting();
            var oJsonObjectPowerTimeSetting = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PowerTimeSetting)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
            var PowerTimeSetting = new DMD.JsonObject.DCU.FromDMD.PowerTimeSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                seatID = oJsonObjectPowerTimeSetting.seatID,
                SqlCommand = oJsonObjectPowerTimeSetting.SqlCommand
            };
            SendToAllTasks(DMDHelper, PowerTimeSetting);
        }


        private void HandleTrainMessageSetting(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper)
        {
            DMDHelper.UpDateDMDTrainMessage();
            var oJsonObjectTrainMessageSetting = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
            var TrainMessageSetting = new DMD.JsonObject.DCU.FromDMD.TrainMessageSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                msg_id = oJsonObjectTrainMessageSetting.msg_id,
                seatID = oJsonObjectTrainMessageSetting.seatID,
                SqlCommand = oJsonObjectTrainMessageSetting.SqlCommand
            };
            SendToAllTasks(DMDHelper, TrainMessageSetting);
        }

        private void HandleGroupSetting(ASI.Wanda.DMD.Message.Message DMDServerMessage, TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper)
        {
            DMDHelper.UpDateDMDGroup();
            var oJsonObjectGroupSetting = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.GroupSetting)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
            var GroupSetting = new DMD.JsonObject.DCU.FromDMD.GroupSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                group_id = oJsonObjectGroupSetting.group_id,
                seatID = oJsonObjectGroupSetting.seatID,
                SqlCommand = oJsonObjectGroupSetting.SqlCommand
            };
            SendToAllTasks(DMDHelper, GroupSetting);
        }
        #endregion

        private void HandleUnexpectedResponse(ASI.Wanda.DMD.Message.Message DMDServerMessage) 
        {
            ASI.Lib.Log.ErrorLog.Log(_mProcName, $"從CMFT來的訊息不應有Response，MessageType:{DMDServerMessage.MessageType}");
        }

        private void SendToAllTasks<T>(TaskDMDHelper<ASI.Wanda.DMD.DMD_API> DMDHelper, T messageObject)
        {
            try
            {
                var serializedMessage = new ASI.Wanda.DCU.Message.Message(
                    ASI.Wanda.DCU.Message.Message.eMessageType.Command,
                    01,
                    ASI.Lib.Text.Parsing.Json.SerializeObject(messageObject)
                );

                DMDHelper.SendToTaskPUP(2, 1, serializedMessage.JsonContent);
                DMDHelper.SendToTaskCDU(2, 1, serializedMessage.JsonContent);
                DMDHelper.SendToTaskSDU(2, 1, serializedMessage.JsonContent);
                DMDHelper.SendToTaskPDN(2, 1, serializedMessage.JsonContent);
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("SendToAllTasks", $"序列化消息時發生錯誤: {ex.Message}");
            }
        }


        #endregion

    }
}
