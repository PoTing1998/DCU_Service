namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// 對外送出的抽象：回覆 DMD Server、送內部 MSMQ 給其他 Task。
    /// TaskDMD / UITest 真實模式使用 <see cref="RealDMDOutbound"/>；UITest 模擬模式使用 <see cref="SimulatedDMDOutbound"/>。
    /// </summary>
    public interface IDMDOutbound
    {
        /// <summary>送訊息回 DMD Server（Ack / Response）</summary>
        void SendToDMD(ASI.Wanda.DMD.Message.Message message);

        /// <summary>送內部訊息給指定佇列的 Task</summary>
        void SendToTask(string queueName, int msgType, int msgID, string jsonData);
    }
}
