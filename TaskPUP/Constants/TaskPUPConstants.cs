using ASI.Lib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPUP.Constants
{
    /// <summary>
    /// 火災相關訊息
    /// </summary>
    public static class FireAlarmMessages
    {
        public static readonly string CheckChinese = Get("FireDetectorCheckInProgressChinese");
        public static readonly string CheckEnglish = Get("FireDetectorCheckInProgressEnglish");
        public static readonly string EmergencyChinese = Get("FireEmergencyEvacuateCalmlyChinese");
        public static readonly string EmergencyEnglish = Get("FireEmergencyEvacuateCalmlyEnglish");
        public static readonly string ClearedChinese = Get("FireAlarmClearedChinese");
        public static readonly string ClearedEnglish = Get("FireAlarmClearedEnglish");
        public static readonly string DetectorChinese = Get("FireDetectorClearConfirmedChinese");
        public static readonly string DetectorEnglish = Get("FireDetectorClearConfirmedEnglish");

        private static string Get(string key) => ConfigApp.Instance.GetConfigSetting(key);
    }

    /// <summary>
    /// 判斷來自DMD Service  的訊息處理
    /// </summary>
    public static class Constants
    {
        public const string SendPreRecordMsg = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage";
        public const string SendInstantMsg = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendInstantMessage";
        public const string SendScheduleSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.ScheduleSetting";
        public const string SendPreRecordMessageSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PreRecordMessageSetting";
        public const string SendTrainMessageSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting";
        public const string SendPowerTimeSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PowerTimeSetting";
        public const string SendGroupSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.GroupSetting";
        public const string SendParameterSetting = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.ParameterSetting";
        public const string OpenDisplay = "節能模式開啟";
        public const string CloseDisplay= "節能模式關閉";
    }

    /// <summary>
    /// 應用程式設定讀取
    /// </summary>
    public static class AppConfig
    {
        public static readonly string ComPort = Get("PUPComPort");
        public static readonly string BaudRate = Get("PUPBaudrate");

        public static readonly string DbIP = Get("DCU_DB_IP");
        public static readonly string DbPort = Get("DCU_DB_Port");
        public static readonly string DbName = Get("DCU_DB_Name");
        public static readonly string CurrentUserID = Get("Current_User_ID");
        public static readonly string DBUserID = "postgres";
        public static readonly string DBPassword = "postgres";

        private static string Get(string key) => ConfigApp.Instance.GetConfigSetting(key);
    }
}
