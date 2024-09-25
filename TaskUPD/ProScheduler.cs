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

        if (stationData.eco_mode == "ON")
        {
            int currentMonth = DateTime.Now.Month;
            int currentDay = DateTime.Now.Day;
            int currentHour = DateTime.Now.Hour;
            
            var nonEcoDates = new List<(int Month, int Day)>();

            foreach (string day in notEcoDays)
            {
                if (day.Length == 4)
                {
                    int month = int.Parse(day.Substring(0, 2));
                    int dayOfMonth = int.Parse(day.Substring(2, 2));
                    nonEcoDates.Add((month, dayOfMonth));

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

                string[] autoPlayTimes = stationData.auto_play_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string[] autoEcoTimes = stationData.auto_eco_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (autoPlayTimes.Length == 2 && autoEcoTimes.Length == 2)
                {
                    int autoPlayStartHour = int.Parse(autoPlayTimes[0]);
                    int autoPlayEndHour = int.Parse(autoPlayTimes[1]);
                    int autoEcoStartHour = int.Parse(autoEcoTimes[0]);
                    int autoEcoEndHour = int.Parse(autoEcoTimes[1]);
                    
                    if (currentHour >= autoPlayStartHour && currentHour <= autoPlayEndHour)
                    {
                        ASI.Lib.Log.DebugLog.Log(_mProcName, "關閉顯示器");
                        PowerSettingOff();
                    }
                    else if (currentHour >= autoEcoStartHour && currentHour <= autoEcoEndHour)
                    {
                        ASI.Lib.Log.DebugLog.Log(_mProcName, "開啟顯示器");
                        PowerSettingOpen();
                    }
                }
            }
        }
        else
        {
            // 不需要做任何處理
        }

        return null;
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
