using ASI.Lib.Config;
using ASI.Lib.DB;
using ASI.Lib.Log;
using ASI.Lib.Process;
using ASI.Wanda.DMD.ProcMsg;
using ASI.Wanda.DMD.TaskDMD;


using System;
using System.Collections.Generic;
using System.Linq;


namespace ASI.Wanda.DCU.TaskDMD
{
    /// <summary>
    /// 處理DMD模組執行程序
    /// </summary>
    public class ProcTaskDMD : ProcBase
    {
        private ASI.Wanda.DMD.DMD_API mDMD_API = null;
        public string DCUSation ;
        /// <summary>
        /// 最後一次收到DMD訊息的時間
        /// </summary>
        private System.DateTime LastHeartbeatTime = System.DateTime.Now;

        /// <summary>
        /// 與DMD Server的連線狀態
        /// </summary>
        private bool mIsConnectedToDMD = false;


        /// <summary>
        /// 設備訊息
        /// </summary>
        public class DeviceInfo
        {                                             
            public string StationID { get; set; } 
            public string AreaID { get; set; }
            public string DeviceID { get; set; }
        }

        public string mDMDServerConnStr = "";
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

            //ping DMD Server
            if (this.mDMDServerConnStr != "")
            {
                try 
                {

                    if (mIsConnectedToDMD)
                    {
                        //若原本為連線，則檢查目前連線狀態 
                        //超過60秒未收到DMD傳送的訊息則判定為離線

                        string sStatusValue = true.ToString();
                        if (System.DateTime.Now.Subtract(LastHeartbeatTime).TotalSeconds > 60)
                        {
                            sStatusValue = false.ToString();
                        }
                    }
                    else
                    {

                    }
                }
                catch (System.Exception ex)
                {
                    ASI.Lib.Log.ErrorLog.Log(mProcName, ex);
                }
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
            mProcName = "TaskDMD";

            string sDBIP = ConfigApp.Instance.GetConfigSetting("DCU_DB_IP");
            string sDBPort = ConfigApp.Instance.GetConfigSetting("DCU_DB_Port");
            string sDBName = ConfigApp.Instance.GetConfigSetting("DCU_DB_Name");
            string sUserID = "postgres";
            string sPassword = "postgres";
            string sCurrentUserID = ConfigApp.Instance.GetConfigSetting("Current_User_ID");

            try
            {
                //"Server='localhost'; Port='5432'; Database='DMDDB'; User Id='postgres'; Password='postgres'"; 
                if (!ASI.Wanda.DMD.DB.Manager.Initializer(sDBIP, sDBPort, sDBName, sUserID, sPassword, sCurrentUserID))
                {
                    ASI.Lib.Log.ErrorLog.Log(mProcName, $"資料庫連線失敗!{sDBIP}:{sDBPort};userid={sUserID}"); 
                }
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, $"資料庫連線失敗!{sDBIP}:{sDBPort};userid={sUserID};ex={ex}");
            }
            ConnToDMDServer();
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
                ////{"station":0,"seatID":"TEST","msg_id":["測試內容"],
                ////"target_du":["LG01_CCS_CDU-1","LG01_CCS_CDU-2","LG01_UPF_PDU-1","LG08A_DPF_PDU-4"],"dbName1":"dmd_pre_record_message","dbName2":"dmd_target","JsonObjectName":"ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage"}
                var sRcvTime = System.DateTime.Now.ToString("HH:mm:ss.fff");
                var sByteArray = ASI.Lib.Text.Parsing.String.BytesToHexString(DMDServerMessage.CompleteContent, "");
                var sJsonData = DMDServerMessage.JsonContent;

                var sJsonObjectName = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "JsonObjectName");
       
                //建立DMDHelper並將 DMD_API的send 委派 
                var DMDHelper = new TaskDMDHelper<ASI.Wanda.DMD.DMD_API>(mDMD_API, (api, message) => api.Send(message));

                var target = ASI.Lib.Text.Parsing.Json.GetValue(sJsonData, "target_du");
                ////判斷車站
               // string DCUSation = target.Split('_')[0];
                int iMsgID = DMDServerMessage.MessageID;
                if (DMDServerMessage.MessageType == ASI.Wanda.DMD.Message.Message.eMessageType.Ack)
                {
                    ///Ack
                    sLog = string.Format("Ack，訊息識別碼:[{0}]", DMDServerMessage.MessageID);
                    DMDHelper.HandleAckMessage(DMDServerMessage);
                }
                else if (DMDServerMessage.MessageType == ASI.Wanda.DMD.Message.Message.eMessageType.Command)
                {
                    sLog = $"從DMD Server收到:{sByteArray}；訊息類別碼:{DMDServerMessage.MessageType}；識別碼:{iMsgID}；長度:{DMDServerMessage.MessageLength}；內容:{sJsonData}；JsonObjectName:{sJsonObjectName}";

                    UpdataConfig();
                    ASI.Lib.Log.DebugLog.Log("FromDMD_server", $"{sLog}\r\n");
                    //收到封包
                    var oJsonObject = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
                    //組封包 
                    var sendPreRecordMessage = new DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage(ASI.Wanda.DMD.Enum.Station.OCC);
                    sendPreRecordMessage.seatID = oJsonObject.seatID;
                    sendPreRecordMessage.msg_id = oJsonObject.msg_id;
                    sendPreRecordMessage.target_du = oJsonObject.target_du;
                 
                    //判斷從過來的ObjactName 
                    switch (sJsonObjectName)
                    {
                        case ASI.Wanda.DMD.TaskDMD.Constants.SendPreRecordMsg: //預錄訊息
                     
                            DMDHelper.UpdateDCUPlayList();
                            DMDHelper.UpdataDCUPreRecordMessage();
                            var RecordMessage = new ASI.Wanda.DCU.Message.Message( ASI.Wanda.DCU.Message.Message.eMessageType.Command, 01, ASI.Lib.Text.Parsing.Json.SerializeObject(sendPreRecordMessage));
                            DMDHelper.SendToTaskUPD(2,1, RecordMessage.JsonContent);
                            DMDHelper.SendToTaskPDU(2, 1, RecordMessage.JsonContent);
                            DMDHelper.SendToTaskSDU(2, 1, RecordMessage.JsonContent);
                            DMDHelper.SendToTaskLPD(2, 1, RecordMessage.JsonContent);
                            break;
                        case ASI.Wanda.DMD.TaskDMD.Constants.SendInstantMsg: //即時訊息 
                           // DMDHelper.UpdataConfig();
                            DMDHelper.UpdateDCUPlayList();
                            DMDHelper.UpdataDCUInstantMessage();
                            var Msg = new ASI.Wanda.DCU.Message.Message(ASI.Wanda.DCU.Message.Message.eMessageType.Command, 01, ASI.Lib.Text.Parsing.Json.SerializeObject(sendPreRecordMessage));
                            DMDHelper.SendToTaskUPD(2, 1, Msg.JsonContent);
                            DMDHelper.SendToTaskPDU(2, 1, Msg.JsonContent);
                            DMDHelper.SendToTaskSDU(2, 1, Msg.JsonContent);
                            DMDHelper.SendToTaskLPD(2, 1, Msg.JsonContent);
                            break;
                        case ASI.Wanda.DMD.TaskDMD.Constants.SendScheduleSetting: //訊息排程
                            DMDHelper.HandleAckMessage(DMDServerMessage);
                            break;
                        case ASI.Wanda.DMD.TaskDMD.Constants.SendPreRecordMessageSetting: //預錄訊息設定 
                            DMDHelper.HandleAckMessage(DMDServerMessage);
                            break;
                        case ASI.Wanda.DMD.TaskDMD.Constants.SendTrainMessageSetting: //列車訊息 
                            DMDHelper.HandleAckMessage(DMDServerMessage);
                            break;
                        case ASI.Wanda.DMD.TaskDMD.Constants.SendPowerTimeSetting: //電池設定
                            DMDHelper.HandleAckMessage(DMDServerMessage);
                            break;
                        case ASI.Wanda.DMD.TaskDMD.Constants.SendGroupSetting: //群組設定
                            DMDHelper.HandleAckMessage(DMDServerMessage);
                            break;    
                        case ASI.Wanda.DMD.TaskDMD.Constants.SendParameterSetting: //參數設定 
                            DMDHelper.HandleAckMessage(DMDServerMessage);
                            break;
                    }
                    var jsonObject = DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
                }
                else if (DMDServerMessage.MessageType == DMD.Message.Message.eMessageType.Response)
                {
                    ///Response
                    ///從CMFT來的訊息不應有Response 
                    ASI.Lib.Log.ErrorLog.Log(mProcName, $"從HMI來的訊息不應有Response，MessageType:{DMDServerMessage.MessageType}");
                }
                else
                {
                    ///無此種訊息類別
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
                        ASI.Lib.Log.ErrorLog.Log(mProcName, $"從TaskDCU來的訊息不應有DMD內部通訊定義:Ack，MessageType:{mSGFromTaskDCU.MessageType}"); ;
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

        /// <summary>
        /// 與DMD Server連線 
        /// </summary>
        private void ConnToDMDServer()
        {
            try
            {
                DisconnectExistingDMDAPI();

                mDMD_API = new ASI.Wanda.DMD.DMD_API();
                mDMD_API.ReceivedEvent += DMD_API_ReceivedEvent;
                mDMD_API.DisconnectedEvent += DMD_API_DisconnectedEvent;
                mDMDServerConnStr = ConfigApp.Instance.GetConfigSetting("DMD_Server");

                int iResult = mDMD_API.Initial(mDMDServerConnStr);
                if (iResult == 0)
                {
                    mIsConnectedToDMD = true;
                    ASI.Lib.Log.DebugLog.Log(mProcName, "與DMD Server連線成功");
                    LastHeartbeatTime = DateTime.Now;
                }
                else
                {
                    mIsConnectedToDMD = false;
                    ASI.Lib.Log.DebugLog.Log(mProcName, $"與DMD Server連線失敗，DCU_Server: {mDMDServerConnStr}");
                }
            }
            catch (Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log(mProcName, $"Exception in ConnToDMDServer: {ex}");
            }
        }

        private void DisconnectExistingDMDAPI()
        {
            if (mDMD_API != null)
            {
                mDMD_API.ReceivedEvent -= DMD_API_ReceivedEvent;
                mDMD_API.DisconnectedEvent -= DMD_API_DisconnectedEvent;
                mDMD_API.Dispose();
                ASI.Lib.Log.DebugLog.Log(mProcName, "Existing DMD_API disconnected and disposed.");
            }
        }
        /// <summary>
        /// 從DMD更新Config的表 拿到相對色碼顏色  
        /// </summary>
        public IEnumerable<ASI.Wanda.DCU.DB.Tables.System.sysConfig> UpdataConfig()
        {
            try
            {
                var tempList = ASI.Wanda.DMD.DB.Tables.System.sysConfig.SelectAll();
                ///轉換過程 
                var convertedList = tempList
                    .Select(item => new ASI.Wanda.DMD.DB.Models.System.sys_config
                    {
                        config_name = item.config_name,
                        config_value = item.config_value,
                        config_description = item.config_description,
                        system_id = item.system_id,
                        remark = item.remark,
                        ins_user = item.ins_user,
                        ins_time = item.ins_time,
                        upd_user = item.upd_user,
                        upd_time = item.upd_time,
                    })
                    .ToList();
               
                ///遍歷轉換後的列表，進行更新操作 
                foreach (var item in convertedList)
                {
                    ASI.Wanda.DCU.DB.Tables.System.sysConfig.UpdataSystemConfig(
                       item.config_name,
                       item.config_value,
                       item.config_description,
                       item.system_id,
                       item.remark
                    );
                }

                return convertedList.Cast<ASI.Wanda.DCU.DB.Tables.System.sysConfig>();
            }
            catch (Exception updateException)
            {
                ///記錄例外狀況 
                ASI.Lib.Log.ErrorLog.Log("Error updating sysConfig", updateException);
                return Enumerable.Empty<ASI.Wanda.DCU.DB.Tables.System.sysConfig>();
            }
        }

    }
}
