namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// 資料庫同步的抽象（DMD DB → DCU DB）。
    /// </summary>
    public interface IDMDDataSync
    {
        void UpdateDCUPlayList();
        void UpdateDCUPreRecordMessage();
        void UpdateDCUInstantMessage();
        void UpdateConfig();
        void UpdateSchedule();
        void UpdateSchedulePlaylist();
        void UpdatePowerSetting();
        void UpdateTrainMessage();
        void UpdateGroup();
        void InsertTrainMessage(ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMSG trainMSG);
    }
}
