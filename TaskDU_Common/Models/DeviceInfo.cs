namespace TaskDU_Common.Models
{
    /// <summary>
    /// 從 targetDu 字串（例如 LG01_CCS_CDU-1）解析出的設備資訊。
    /// 取代 TaskCDU / TaskSDU / TaskPDN 各自定義的同名 DeviceInfo 類別。
    /// </summary>
    public class DeviceInfo
    {
        public string Station { get; set; }
        public string Location { get; set; }
        public string DeviceWithNumber { get; set; }
    }
}
