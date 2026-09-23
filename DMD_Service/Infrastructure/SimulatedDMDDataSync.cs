namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// 模擬資料庫同步：不讀寫資料庫，僅由 <see cref="DMDHelper"/> 的 Trace 記錄「會做哪些同步」。
    /// </summary>
    public class SimulatedDMDDataSync : IDMDDataSync
    {
        public void UpdateDCUPlayList() { }
        public void UpdateDCUPreRecordMessage() { }
        public void UpdateDCUInstantMessage() { }
        public void UpdateConfig() { }
        public void UpdateSchedule() { }
        public void UpdateSchedulePlaylist() { }
        public void UpdatePowerSetting() { }
        public void UpdateTrainMessage() { }
        public void UpdateGroup() { }
        public void InsertTrainMessage(ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMSG trainMSG) { }
    }
}
