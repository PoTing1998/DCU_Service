namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// 判別 DMD 傳送過來的 JsonObjectName
    /// </summary>
    public static class Constants
    {
        public const string SendPreRecordMsg        = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage";
        public const string SendInstantMsg          = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendInstantMessage";
        public const string ScheduleSetting         = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.ScheduleSetting";
        public const string PreRecordMessageSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PreRecordMessageSetting";
        public const string TrainMessageSetting     = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting";
        public const string PowerTimeSetting        = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PowerTimeSetting";
        public const string GroupSetting            = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.GroupSetting";
        public const string ParameterSetting        = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.ParameterSetting";

        /// <summary>內部 MSMQ 佇列名稱</summary>
        public const string QueueTaskPUP = "dcuservertaskpup";
        public const string QueueTaskCDU = "dcuservertaskcdu";
        public const string QueueTaskSDU = "dcuservertasksdu";
        public const string QueueTaskPDN = "dcuservertaskpdn";
    }
}
