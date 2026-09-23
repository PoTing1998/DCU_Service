namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// 模擬送出：不做任何實際傳送，僅由 <see cref="DMDHelper"/> 的 Trace 記錄「會送出什麼」。
    /// </summary>
    public class SimulatedDMDOutbound : IDMDOutbound
    {
        public void SendToDMD(ASI.Wanda.DMD.Message.Message message) { }
        public void SendToTask(string queueName, int msgType, int msgID, string jsonData) { }
    }
}
