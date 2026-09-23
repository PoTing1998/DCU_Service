namespace ASI.Wanda.DMD.Service.Handlers
{
    /// <summary>
    /// 處理號誌訊號（MessageType = trainMessage）：寫入 train_message 並轉送月台 Task（PUP / PDN）。
    /// </summary>
    public class TrainSignalHandler : IDMDMessageHandler
    {
        public void Handle(ASI.Wanda.DMD.Message.Message message, DMDHelper helper)
        {
            var obj = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMSG)
                ASI.Wanda.DMD.Message.Helper.GetJsonObject(message.JsonContent);

            var trainMSG = new ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMSG(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                Type         = obj.Type,
                Command      = obj.Command,
                Platform_id  = obj.Platform_id,
                Arrive_time1 = obj.Arrive_time1,
                Depart_time1 = obj.Depart_time1,
                Destination1 = obj.Destination1,
                Depart_time2 = obj.Depart_time2,
                Arrive_time2 = obj.Arrive_time2,
                Destination2 = obj.Destination2
            };

            // 更新資料庫
            helper.InsertTrainMessage(trainMSG);
            helper.SendToPlatform(trainMSG);
        }
    }
}
