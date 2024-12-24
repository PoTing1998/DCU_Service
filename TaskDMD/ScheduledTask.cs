using System;
using System.Timers;
using System.Collections.Generic;
using ASI.Wanda.DMD.TaskDMD;
using ASI.Wanda.DMD;
using Display;
using Display.Function;

public class ScheduledTask
{
    private readonly Timer _timer;
    private readonly Action _task;
    public const string _mProcName = "ScheduledTask";
    private ASI.Wanda.DMD.DMD_API mDMD_API = null;
    private bool isDisplayCurrentlyOn = false; // 狀態追踪顯示器是否開啟

    // 新增無參數建構函數
    public ScheduledTask() { }

    public ScheduledTask(Action task, double intervalInMilliseconds)
    {
        _task = task ?? throw new ArgumentNullException(nameof(task));
        _timer = new Timer(intervalInMilliseconds);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
    }

    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        try
        {
            _task.Invoke();

        }
        catch (Exception ex)
        {
            // 處理錯誤，例如記錄錯誤日誌
            ASI.Lib.Log.ErrorLog.Log(_mProcName, $"Error occurred during scheduled task: {ex.Message}");
        }
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void StartPowerSettingScheduler(string stationID)
    {
        // 建立一個 Action，來定義要執行的工作
        Action PowerSettingTask = () => PowerSetting(stationID);

        // 初始化每小時調用的計時任務（間隔為 1 小時 = 3600000 毫秒）
        ScheduledTask powerScheduler = new ScheduledTask(PowerSettingTask, 3600000);
        powerScheduler.Start();
    }

    /// <summary>
    /// 找尋車站Id並且判斷是否需要關閉
    /// </summary>
    /// <param name="stationID"></param>
    /// <returns></returns> 
    public void PowerSetting(string stationID)
    {
        var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);
        if (stationData == null)
        {
            ASI.Lib.Log.ErrorLog.Log(_mProcName, "無效的車站ID: " + stationID);
            return;
        }

        string[] notEcoDays = stationData.not_eco_day.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        if (stationData.eco_mode != "ON")
        {
            ASI.Lib.Log.DebugLog.Log(_mProcName, "節能模式關閉，始終保持播放");
            if (!isDisplayCurrentlyOn)
            {
                OpenDisplay();
                isDisplayCurrentlyOn = true;
            }
            return;
        }

        // 正在使用的月日及時分
        var currentDate = DateTime.Now;
        int currentMonth = currentDate.Month;
        int currentDay = currentDate.Day;
        TimeSpan currentTime = currentDate.TimeOfDay;

        // 使用 HashSet 儲存不啟動節能模式的日期，增強查找效能
        var nonEcoDates = new HashSet<string>();
        bool isNonEcoDay = false;

        foreach (string day in notEcoDays)
        {
            if (day.Length == 4 && int.TryParse(day.Substring(0, 2), out int month) && int.TryParse(day.Substring(2, 2), out int dayOfMonth))
            {
                string formattedDate = month.ToString("D2") + dayOfMonth.ToString("D2");
                nonEcoDates.Add(formattedDate);
                if (formattedDate == currentMonth.ToString("D2") + currentDay.ToString("D2"))
                {
                    isNonEcoDay = true;
                }
            }
            else
            {
                // 日期格式錯誤處理
                ASI.Lib.Log.ErrorLog.Log(_mProcName, "無效的日期格式：" + day);
            }
        }

        // 如果是非節能日，始終保持播放
        if (isNonEcoDay)
        {
            ASI.Lib.Log.DebugLog.Log(_mProcName, "今天是非節能日，始終保持播放");
            if (!isDisplayCurrentlyOn)
            {
                OpenDisplay();
                isDisplayCurrentlyOn = true;
            }
            return;
        }

        // 檢查開關顯示器的時間，確保時間段不重疊
        string[] autoPlayTimes = stationData.auto_play_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        string[] autoEcoTimes = stationData.auto_eco_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        if (autoPlayTimes.Length == 2 && autoEcoTimes.Length == 2 &&
            int.TryParse(autoPlayTimes[0], out int autoPlayStartHour) && int.TryParse(autoPlayTimes[1], out int autoPlayMinute) &&
            int.TryParse(autoEcoTimes[0], out int autoEcoStartHour) && int.TryParse(autoEcoTimes[1], out int autoEcoMinute))
        {
            TimeSpan playStartTime = new TimeSpan(autoPlayStartHour, autoPlayMinute, 0);
            TimeSpan ecoStartTime = new TimeSpan(autoEcoStartHour, autoEcoMinute, 0);

            // 檢查時間段是否重疊，並正確處理跨日的情況 
            bool isInPlayTime = playStartTime <= ecoStartTime
                ? currentTime >= playStartTime && currentTime < ecoStartTime
                : currentTime >= playStartTime || currentTime < ecoStartTime;

            if (isInPlayTime)
            {
                if (!isDisplayCurrentlyOn)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, "當前時間符合播放條件，開始顯示");
                    OpenDisplay();
                    isDisplayCurrentlyOn = true; 
                    SendToTaskCDU("節能模式開啟");
                }
            }
            else
            {
                if (isDisplayCurrentlyOn)
                {
                    ASI.Lib.Log.DebugLog.Log(_mProcName, "當前時間進入節能模式，關閉顯示");
                    CloseDisplay();
                    isDisplayCurrentlyOn = false;
                    SendToTaskCDU("節能模式關閉");
                }
            }
        }
        else
        {
            ASI.Lib.Log.ErrorLog.Log(_mProcName, "自動播放時間或自動節能時間格式錯誤");
        }
    }

    public void CloseDisplay()
    {
        // 關閉顯示器的邏輯 
        var startCode = new byte[] { 0x55, 0xAA };
        var processor = new PacketProcessor();
        var function = new PowerControlHandler();
        var Off = new byte[] { 0x3A, 0x01 };
        //設定ID
        var packetOff = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Off);
        var serializedDataOff = processor.SerializePacket(packetOff);
        ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面關閉", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
    }

    public void OpenDisplay()
    {
        // 開啟顯示器的邏輯 
        var startCode = new byte[] { 0x55, 0xAA };
        var processor = new PacketProcessor();
        var function = new PowerControlHandler();
        var Open = new byte[] { 0x3A, 0x00 };
        //設定ID 
        var packetOpen = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Open);
        var serializedDataOpen = processor.SerializePacket(packetOpen);
        ASI.Lib.Log.DebugLog.Log(_mProcName + "顯示畫面開啟", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen) + " | Current Time: " + DateTime.Now);
    }

    private void SendToTaskCDU(string message)
    {
        var DMDHelper = new TaskDMDHelper<ASI.Wanda.DMD.DMD_API>(mDMD_API, (api, msg) => api.Send(msg));
        DMDHelper.SendToTaskCDU(2, 1, message);
    }
}
