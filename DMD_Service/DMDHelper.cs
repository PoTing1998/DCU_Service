using System;

namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// DMD 訊息處理的共用工具（原 TaskDMD 的 TaskDMDHelper）。
    /// 所有「副作用」都透過 <see cref="IDMDOutbound"/> 與 <see cref="IDMDDataSync"/> 執行，
    /// 並以 <see cref="Trace"/> 回報每一個動作，讓畫面可以顯示處理過程。
    /// </summary>
    public class DMDHelper
    {
        private readonly IDMDOutbound _outbound;
        private readonly IDMDDataSync _dataSync;

        /// <summary>是否為模擬模式（不寫 DB、不送 MSMQ、不回 DMD）</summary>
        public bool IsSimulated { get; }

        /// <summary>每執行一個動作時回報文字描述（可為 null）</summary>
        public Action<string> Trace { get; set; }

        public DMDHelper(IDMDOutbound outbound, IDMDDataSync dataSync, bool isSimulated = false)
        {
            _outbound   = outbound ?? throw new ArgumentNullException(nameof(outbound));
            _dataSync   = dataSync ?? throw new ArgumentNullException(nameof(dataSync));
            IsSimulated = isSimulated;
        }

        /// <summary>建立真實模式 Helper（TaskDMD 服務與 UITest 真實模式使用）</summary>
        public static DMDHelper CreateReal(Func<ASI.Wanda.DMD.Message.Message, int> sendToDmd)
            => new DMDHelper(new RealDMDOutbound(sendToDmd), new DbDMDDataSync(), false);

        /// <summary>建立模擬模式 Helper（UITest 模擬模式使用）</summary>
        public static DMDHelper CreateSimulated()
            => new DMDHelper(new SimulatedDMDOutbound(), new SimulatedDMDDataSync(), true);

        private void Log(string text)
        {
            Trace?.Invoke(IsSimulated ? "(模擬) " + text : text);
        }

        #region 回覆 DMD
        public void HandleAckMessage(ASI.Wanda.DMD.Message.Message DMDServerMessage)
        {
            var MSG = new ASI.Wanda.DMD.Message.Message(ASI.Wanda.DMD.Message.Message.eMessageType.Ack, DMDServerMessage.MessageID, null);
            Log($"[DMD] 回覆 Ack，識別碼:{DMDServerMessage.MessageID}");
            _outbound.SendToDMD(MSG);
        }

        public void SendToDMD(ASI.Wanda.DMD.Message.Message message)
        {
            Log($"[DMD] 送出 {message.MessageType}，識別碼:{message.MessageID}，內容:{message.JsonContent}");
            _outbound.SendToDMD(message);
        }
        #endregion

        #region 傳送到內部 MSG
        public void SendToTaskPUP(int msgType, int msgID, string jsonData) => SendToTask(Constants.QueueTaskPUP, msgType, msgID, jsonData);
        public void SendToTaskCDU(int msgType, int msgID, string jsonData) => SendToTask(Constants.QueueTaskCDU, msgType, msgID, jsonData);
        public void SendToTaskSDU(int msgType, int msgID, string jsonData) => SendToTask(Constants.QueueTaskSDU, msgType, msgID, jsonData);
        public void SendToTaskPDN(int msgType, int msgID, string jsonData) => SendToTask(Constants.QueueTaskPDN, msgType, msgID, jsonData);

        private void SendToTask(string queueName, int msgType, int msgID, string jsonData)
        {
            Log($"[MSMQ] → {queueName}，Type:{msgType}，ID:{msgID}，Json:{jsonData}");
            _outbound.SendToTask(queueName, msgType, msgID, jsonData);
        }

        /// <summary>序列化後送給 PUP / CDU / SDU / PDN 四個 Task</summary>
        public void SendToAllTasks<T>(T messageObject)
        {
            try
            {
                var serializedMessage = new ASI.Wanda.DCU.Message.Message(
                    ASI.Wanda.DCU.Message.Message.eMessageType.Command,
                    01,
                    ASI.Lib.Text.Parsing.Json.SerializeObject(messageObject));

                SendToTaskPUP(2, 1, serializedMessage.JsonContent);
                SendToTaskCDU(2, 1, serializedMessage.JsonContent);
                SendToTaskSDU(2, 1, serializedMessage.JsonContent);
                SendToTaskPDN(2, 1, serializedMessage.JsonContent);
            }
            catch (Exception ex)
            {
                Log($"[錯誤] SendToAllTasks 序列化失敗：{ex.Message}");
                ASI.Lib.Log.ErrorLog.Log("SendToAllTasks", $"序列化消息時發生錯誤: {ex.Message}");
            }
        }

        /// <summary>序列化後送給月台 Task（PUP / PDN）</summary>
        public void SendToPlatform<T>(T messageObject)
        {
            try
            {
                var serializedMessage = new ASI.Wanda.DCU.Message.Message(
                    ASI.Wanda.DCU.Message.Message.eMessageType.Command,
                    0,
                    ASI.Lib.Text.Parsing.Json.SerializeObject(messageObject));

                SendToTaskPUP(2, 1, serializedMessage.JsonContent);
                SendToTaskPDN(2, 1, serializedMessage.JsonContent);
            }
            catch (Exception ex)
            {
                Log($"[錯誤] SendToPlatform 序列化失敗：{ex.Message}");
                ASI.Lib.Log.ErrorLog.Log("SendToPlatform", $"序列化消息時發生錯誤: {ex.Message}");
            }
        }
        #endregion

        #region 資料庫同步
        public void UpdateDCUPlayList()          { Log("[DB] 同步 dmd_playlist（每裝置保留 5 則）"); _dataSync.UpdateDCUPlayList(); }
        public void UpdateDCUPreRecordMessage()  { Log("[DB] 同步 dmd_pre_record_message"); _dataSync.UpdateDCUPreRecordMessage(); }
        public void UpdateDCUInstantMessage()    { Log("[DB] 同步 dmd_instant_message"); _dataSync.UpdateDCUInstantMessage(); }
        public void UpdateConfig()               { Log("[DB] 同步 sys_config"); _dataSync.UpdateConfig(); }
        public void UpdateSchedule()             { Log("[DB] 同步 dmd_schedule"); _dataSync.UpdateSchedule(); }
        public void UpdateSchedulePlaylist()     { Log("[DB] 同步 dmd_schedule_playlist"); _dataSync.UpdateSchedulePlaylist(); }
        public void UpdatePowerSetting()         { Log("[DB] 同步 dmd_power_setting"); _dataSync.UpdatePowerSetting(); }
        public void UpdateTrainMessage()         { Log("[DB] 同步 dmd_train_message"); _dataSync.UpdateTrainMessage(); }
        public void UpdateGroup()                { Log("[DB] 同步 dmd_group"); _dataSync.UpdateGroup(); }

        public void InsertTrainMessage(ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMSG trainMSG)
        {
            Log($"[DB] 寫入 train_message，月台:{trainMSG.Platform_id}");
            _dataSync.InsertTrainMessage(trainMSG);
        }
        #endregion
    }
}
