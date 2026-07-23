using ASI.Lib.Config;

namespace TaskDU_Common.Constants
{
    /// <summary>
    /// 火災警報相關訊息文字（中英文），從 Config 讀取。
    /// 統一取代 TaskCDU / TaskSDU / TaskPDN 各自的私有 FireAlarmMessages 巢狀類別，
    /// 以及 TaskPUP.Constants.TaskPUPConstants 裡的同名類別。
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
}
