using System;
using System.Timers;

using Display.Function;
using Display;
using System.Xml.Linq;
using System.Collections.Generic;
using ASI.Wanda.DMD.TaskDMD;
using ASI.Wanda.DMD;

namespace TaskDMD
{
    public class ScheduledTask
    {
        private readonly Timer _timer;
        private readonly Action _task;
        public const string _mName = "ScheduledTask";
        private ASI.Wanda.DMD.DMD_API mDMD_API = null;

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
                ASI.Lib.Log.ErrorLog.Log(_mName, $"Error occurred during scheduled task: {ex.Message}");
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
      
        private void PowerSetting(string stationID)
        {
            var stationData = ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.SelectPowerSetting(stationID);
            string[] notEcoDays = stationData.not_eco_day.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            // 取得當前時間
            int currentMonth = DateTime.Now.Month;
            int currentDay = DateTime.Now.Day;
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            var DMDHelper = new TaskDMDHelper<ASI.Wanda.DMD.DMD_API>(mDMD_API, (api, message) => api.Send(message));

            // 開啟節能模式
            if (stationData.eco_mode == "ON")
            {
                // 檢查是否為非節能日
                bool isNonEcoDay = false;
                foreach (string day in notEcoDays)
                {
                    if (day.Length == 4)
                    {
                        int month = int.Parse(day.Substring(0, 2));
                        int dayOfMonth = int.Parse(day.Substring(2, 2));
                        if (month == currentMonth && dayOfMonth == currentDay)
                        {
                            isNonEcoDay = true;
                            break;
                        }
                    }
                    else
                    {
                        ASI.Lib.Log.ErrorLog.Log(_mName, "無效的日期格式：" + day);
                    }
                }

                // 如果是非節能日，始終保持播放
                if (isNonEcoDay)
                {
                    ASI.Lib.Log.DebugLog.Log(_mName, "今天是非節能日，始終保持播放");
                    OpenDisplay();// 使用 ProcTaskPUP 中的方法開啟顯示器
                    return;
                }

                // 解析自動播放和自動節能時間 
                string[] autoPlayTimes = stationData.auto_play_time.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                        ASI.Lib.Log.DebugLog.Log(_mName, "當前時間符合播放條件，開始顯示");
                        OpenDisplay();// 如果在播放時間內，打開顯示器
                                      //改成送給看板的task  由各看板的task自行關閉  
                        DMDHelper.SendToTaskCDU(2, 1, "節能模式開啟");
                    }
                    else
                    {
                        ASI.Lib.Log.DebugLog.Log(_mName, "當前時間進入節能模式，關閉顯示");
                        CloseDisplay();
                        DMDHelper.SendToTaskCDU(2, 1, "節能模式關閉");
                    }
                }
                else
                {
                    ASI.Lib.Log.ErrorLog.Log(_mName, "自動播放時間或自動節能時間格式錯誤");
                    // 默認不操作顯示器  
                }
            }
            else
            {
                ASI.Lib.Log.DebugLog.Log(_mName, "節能模式關閉，始終保持播放");
                CloseDisplay();
                DMDHelper.SendToTaskCDU(2, 1, "節能模式關閉");
            } 
        }

        public void CloseDisplay()
        {
            // 關閉顯示器的邏輯
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var Off = new byte[] { 0x3A, 0X01 };
            var packetOff = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Off);
            var serializedDataOff = processor.SerializePacket(packetOff);
            // _mSerial.Send(serializedDataOff);
            ASI.Lib.Log.DebugLog.Log(_mName + " 顯示畫面關閉", "Serialized display packet: " + BitConverter.ToString(serializedDataOff));
        }
        public void OpenDisplay()
        {
            // 開啟顯示器的邏輯
            var startCode = new byte[] { 0x55, 0xAA };
            var processor = new PacketProcessor();
            var function = new PowerControlHandler();
            var Open = new byte[] { 0x3A, 0X00 };
            var packetOpen = processor.CreatePacketOff(startCode, new List<byte> { 0x11, 0x12 }, function.FunctionCode, Open);
            var serializedDataOpen = processor.SerializePacket(packetOpen);
            // _mSerial.Send(serializedDataOpen);
            ASI.Lib.Log.DebugLog.Log(_mName + "顯示畫面開啟", "Serialized display packet: " + BitConverter.ToString(serializedDataOpen));
        }


        public void StartPowerSettingScheduler(string stationID)
        {
            // 建立一個 Action，來定義要執行的工作
            Action powerSettingTask = () => PowerSetting(stationID);

            // 初始化每小時調用的計時任務（間隔為 1 小時 = 3600000 毫秒）
            ScheduledTask powerScheduler = new ScheduledTask(powerSettingTask, 3600000);
            powerScheduler.Start();
        }
    }

   
}
