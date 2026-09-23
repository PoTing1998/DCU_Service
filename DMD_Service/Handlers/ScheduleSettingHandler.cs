namespace ASI.Wanda.DMD.Service.Handlers
{
    public class ScheduleSettingHandler : IDMDMessageHandler
    {
        public void Handle(ASI.Wanda.DMD.Message.Message message, DMDHelper helper)
        {
            var obj = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendScheduleSetting)
                ASI.Wanda.DMD.Message.Helper.GetJsonObject(message.JsonContent);

            var data = new ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendScheduleSetting(ASI.Wanda.DMD.Enum.Station.OCC)
            {
                seatID     = obj.seatID,
                sched_id   = obj.sched_id,
                SqlCommand = obj.SqlCommand
            };

            helper.UpdateSchedule();
            helper.UpdateSchedulePlaylist();
            helper.UpdateDCUPreRecordMessage();
            helper.SendToAllTasks(data);
        }
    }
}
