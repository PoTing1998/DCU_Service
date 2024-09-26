using ASI.Wanda.DCU.DB.Tables.DMD;

using Display.Function;
using Display;

using System;
using System.Collections.Generic;
using System.Timers;

public class PowerSettingManager
{
    private static PowerSettingManager _instance;
    private static Timer _timer;
    private const string _mProcName = "PowerSettingManager"; // for logging

    // 私有建構子，確保只能通過 GetInstance() 獲取實例
    private PowerSettingManager()
    {
        // 初始化計時器，每小時觸發一次
        _timer = new Timer(3600000); // 3600000 milliseconds = 1 hour
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    // 獲取 Singleton 實例
    public static PowerSettingManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PowerSettingManager();
        }
        return _instance;
    }

    // 計時器觸發的事件，其他類別也可以調用
    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        // 可以在這裡定義不同的業務邏輯，或是讓具體類別來調用 PowerSetting
        Console.WriteLine("計時器觸發，每小時執行一次 PowerSetting");
    }

    public dmdPowerSetting PowerSetting(string stationID)
    {
        var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);
        string[] notEcoDays = stationData.not_eco_day.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        // 取得當前時間
        int currentMonth = DateTime.Now.Month;
        int currentDay = DateTime.Now.Day;
        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        //開啟節能模式
        if (stationData.eco_mode == "ON")
        {
            var nonEcoDates = new List<(int Month, int Day)>();

            foreach (string day in notEcoDays)
            {
                if (day.Length == 4)
                {
                    int month = int.Parse(day.Substring(0, 2));
                    int dayOfMonth = int.Parse(day.Substring(2, 2));
                    nonEcoDates.Add((month, dayOfMonth));
                    //特定日期不啟動節能模式
                    if (month == currentMonth && dayOfMonth == currentDay)
                    {
                        continue;
                    }
                }
                else
                {
                    ASI.Lib.Log.ErrorLog.Log(_mProcName, "無效的日期格式：" + day);
                    continue;
                }
                //開啟時間
                string[] autoPlayTimes = stationData.auto_play_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                //關閉時間
                string[] autoEcoTimes = stationData.auto_eco_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (autoPlayTimes.Length == 2 && autoEcoTimes.Length == 2)
                {
                    int autoPlayStartHour = int.Parse(autoPlayTimes[0]);
                    int autoPlayMinute = int.Parse(autoPlayTimes[1]);
                    int autoEcoStartHour = int.Parse(autoEcoTimes[0]);
                    int autoEcoMinute = int.Parse(autoEcoTimes[1]);
                    // 建立 TimeSpan 物件以便比較時間
                    TimeSpan playStartTime = new TimeSpan(autoPlayStartHour, autoPlayMinute, 0);
                    TimeSpan ecoStartTime = new TimeSpan(autoEcoStartHour, autoEcoMinute, 0);

                    // 判斷當前時間是否在播放時間範圍內
                    if (currentTime >= playStartTime && currentTime < ecoStartTime)
                    {
                        ASI.Lib.Log.DebugLog.Log(_mProcName, "當前時間符合播放條件，開始播放");

                        PowerSettingOpen(); // 如果在播放時間內，打開顯示器
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log(_mProcName, "當前時間進入節能模式，停止播放");

                        PowerSettingOff(); // 如果不在播放時間內，關閉顯示器
                    }

                }
            }
        }
        else
        {

            ASI.Lib.Log.DebugLog.Log(_mProcName, "節能模式關閉，始終保持播放");
            // PowerSettingOpen();
        }
        PowerSettingOff(); // 如果不符合任何條件，關閉顯示器
        return null;  // 如果條件不滿足，則不進行播放
    }

    private void PowerSettingOff()
    {
        // 關閉顯示器的邏輯
        var startCode = new byte[] { 0x55, 0xAA };
        var processor = new PacketProcessor();
        var function = new PowerControlHandler();
        var Off = new byte[] { 0x3A, 0X01 };
        var packetOff = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Off);
        var serializedDataOff = processor.SerializePacket(packetOff);
        // _mSerial.Send(serializedDataOff);
        ASI.Lib.Log.DebugLog.Log(_mProcName + " 顯示畫面關閉", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
    }

    private void PowerSettingOpen()
    {
        // 開啟顯示器的邏輯
        var startCode = new byte[] { 0x55, 0xAA };
        var processor = new PacketProcessor();
        var function = new PowerControlHandler();
        var Open = new byte[] { 0x3A, 0X00 };
        var packetOpen = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Open);
        var serializedDataOpen = processor.SerializePacket(packetOpen);
        // _mSerial.Send(serializedDataOpen);
        ASI.Lib.Log.DebugLog.Log(_mProcName + "顯示畫面開啟", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen));
    }
}
