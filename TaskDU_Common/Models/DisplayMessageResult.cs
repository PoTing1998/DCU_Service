namespace TaskDU_Common.Models
{
    /// <summary>
    /// 單筆顯示訊息的傳送結果。
    /// 取代 TaskCDU / TaskSDU / TaskPDN / TaskPUP 各自定義的同名 DisplayMessageResult 類別。
    /// </summary>
    public class DisplayMessageResult
    {
        public string Result { get; set; }
        public byte[] DataByte { get; set; }
    }
}
