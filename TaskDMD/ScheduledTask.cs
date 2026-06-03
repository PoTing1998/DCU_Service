using System;
using System.Timers;
using System.Collections.Generic;
using ASI.Wanda.DMD.TaskDMD;
using ASI.Wanda.DMD;
using Display;
using Display.Function;

public class ScheduledTask : IDisposable
{
    private readonly Timer _timer;
    private readonly Action _task;
    public const string _mProcName = "ScheduledTask";
    private readonly ASI.Wanda.DMD.DMD_API mDMD_API;
    private bool isDisplayCurrentlyOn = false;

    /// <summary>
    /// 供排程使用的建構子，需傳入有效的 DMD_API
    /// </summary>
    public ScheduledTask(ASI.Wanda.DMD.DMD_API dmdApi)
    {
        mDMD_API = dmdApi ?? throw new ArgumentNullException(nameof(dmdApi));
    }

    private ScheduledTask(Action task, double intervalInMilliseconds, ASI.Wanda.DMD.DMD_API dmdApi)
    {
        _task    = task ?? throw new ArgumentNullException(nameof(task));
        mDMD_API = dmdApi;
        _timer   = new Timer(intervalInMilliseconds);
        _timer.Elapsed  += OnTimedEvent;
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
            ASI.Lib.Log.ErrorLog.Log(_mProcName, $"Error occurred during scheduled task: {ex.Message}");
        }
    }

    public void Start()  => _timer.Start();
    public void Stop()   => _timer.Stop();

    /// <summary>
    /// 啟動節能排程，每小時讀取 DB 判斷是否要開關顯示器
    /// </summary>
    public ScheduledTask StartPowerSettingScheduler(string stationID)
    {
        Action task = () => PowerSetting(stationID);
        var scheduler = new ScheduledTask(task, 3600000, mDMD_API);
        scheduler.Start();
        return scheduler; // 回傳給呼叫端持有，才能在 StopTask 時停止
    }

    /// <summary>
    /// 讀取 DB 判斷現在時間是否要開啟或關閉顯示器
    /// </summary>
    public void PowerSetting(string stationID)
    {
        var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);
        if (stationData == null)
        {
            ASI.Lib.Log.ErrorLog.Log(_mProcName, "無效的車站ID: " + stationID);
            return;
        }

        string[] notEcoDays = stationData.not_eco_day.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        // 節能模式未開啟，保持顯示
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

        var currentDate  = DateTime.Now;
        TimeSpan currentTime = currentDate.TimeOfDay;
        string todayKey  = currentDate.Month.ToString("D2") + currentDate.Day.ToString("D2");

        // 判斷今天是否為非節能日
        bool isNonEcoDay = false;
        foreach (string day in notEcoDays)
        {
            if (day.Length == 4 &&
                int.TryParse(day.Substring(0, 2), out int month) &&
                int.TryParse(day.Substring(2, 2), out int dayOfMonth))
            {
                if (month.ToString("D2") + dayOfMonth.ToString("D2") == todayKey)
                {
                    isNonEcoDay = true;
                    break;
                }
            }
            else
            {
                ASI.Lib.Log.ErrorLog.Log(_mProcName, "無效的日期格式：" + day);
            }
        }

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

        // 解析開關時間
        string[] autoPlayTimes = stationData.auto_play_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        string[] autoEcoTimes  = stationData.auto_eco_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        if (autoPlayTimes.Length == 2 && autoEcoTimes.Length == 2 &&
            int.TryParse(autoPlayTimes[0], out int playHour) && int.TryParse(autoPlayTimes[1], out int playMin) &&
            int.TryParse(autoEcoTimes[0],  out int ecoHour)  && int.TryParse(autoEcoTimes[1],  out int ecoMin))
        {
            TimeSpan playStart = new TimeSpan(playHour, playMin, 0);
            TimeSpan ecoStart  = new TimeSpan(ecoHour, ecoMin, 0);

            bool isInPlayTime = playStart <= ecoStart
                ? currentTime >= playStart && currentTime < ecoStart
                : currentTime >= playStart || currentTime < ecoStart;

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

    /// <summary>
    /// 傳送關閉顯示器封包
    /// </summary>
    public void CloseDisplay()
    {
        try
        {
            var packet = BuildPowerPacket(isOn: false);
            SendPacket(packet);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面關閉",
                "Serialized packet: " + BitConverter.ToString(packet));
        }
        catch (Exception ex)
        {
            ASI.Lib.Log.ErrorLog.Log(_mProcName, $"CloseDisplay 失敗: {ex.Message}");
        }
    }

    /// <summary>
    /// 傳送開啟顯示器封包
    /// </summary>
    public void OpenDisplay()
    {
        try
        {
            var packet = BuildPowerPacket(isOn: true);
            SendPacket(packet);
            ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面開啟",
                "Serialized packet: " + BitConverter.ToString(packet) + " | " + DateTime.Now);
        }
        catch (Exception ex)
        {
            ASI.Lib.Log.ErrorLog.Log(_mProcName, $"OpenDisplay 失敗: {ex.Message}");
        }
    }

    // ── 私有方法 ──────────────────────────────────────────────────────────

    /// <summary>
    /// 組建電源控制封包
    /// </summary>
    private byte[] BuildPowerPacket(bool isOn)
    {
        var processor  = new PacketProcessor();
        var function   = new PowerControlHandler();
        var startCode  = new byte[] { 0x55, 0xAA };
        var ids        = new List<byte> { 0x11, 0x12 };
        var payload    = new byte[] { 0x3A, isOn ? (byte)0x00 : (byte)0x01 };

        var packet = processor.CreatePacketOff(startCode, ids, function.FunctionCode, payload);
        return processor.SerializePacket(packet);
    }

    /// <summary>
    /// 透過 DMD_API 傳送封包
    /// </summary>
    private void SendPacket(byte[] data)
    {
        var message = new ASI.Wanda.DMD.Message.Message(
            ASI.Wanda.DMD.Message.Message.eMessageType.Command,
            0,
            Convert.ToBase64String(data)
        );
        mDMD_API.Send(message);
    }

    private void SendToTaskCDU(string message)
    {
        var helper = new TaskDMDHelper<ASI.Wanda.DMD.DMD_API>(mDMD_API, (api, msg) => api.Send(msg));
        helper.SendToTaskCDU(2, 1, message);
    }

    public void Dispose()
    {
        _timer?.Stop();
        _timer?.Dispose();
    }
}
