using System;
using System.Windows.Forms;

namespace UITest.Controls
{
    public partial class SystemControlPanel : UserControl
    {
        public SystemControlPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 讓 Form1 注入傳送 delegate 給「預錄封包直接傳送」頁面。
        /// </summary>
        public Func<byte[], bool> PreRecordSendAction
        {
            get => preRecordCtrl.SendAction;
            set => preRecordCtrl.SendAction = value;
        }

        public Func<byte[], bool> CommTimeoutSendAction
        {
            get => commTimeoutCtrl.SendAction;
            set => commTimeoutCtrl.SendAction = value;
        }

        public Func<byte[], bool> CalendarSendAction
        {
            get => calendarClockCtrl.SendAction;
            set => calendarClockCtrl.SendAction = value;
        }

        public Func<byte[], byte[]> CalendarReadAction
        {
            get => calendarClockCtrl.ReadAction;
            set => calendarClockCtrl.ReadAction = value;
        }

        public Func<byte[], bool> BrightnessSendAction
        {
            get => brightnessCtrl.SendAction;
            set => brightnessCtrl.SendAction = value;
        }

        public Func<byte[], bool> DisplayModeSendAction
        {
            get => displayModeCtrl.SendAction;
            set => displayModeCtrl.SendAction = value;
        }

        public Func<byte[], bool> DisplayPowerSendAction
        {
            get => displayPowerCtrl.SendAction;
            set => displayPowerCtrl.SendAction = value;
        }

        public Func<byte[], bool> AlarmLightSendAction
        {
            get => alarmLightCtrl.SendAction;
            set => alarmLightCtrl.SendAction = value;
        }

        public Func<byte[], bool> CountdownUnitSendAction
        {
            get => countdownUnitCtrl.SendAction;
            set => countdownUnitCtrl.SendAction = value;
        }

        public Func<byte[], bool> CommStatusSendAction
        {
            get => commStatusCtrl.SendAction;
            set => commStatusCtrl.SendAction = value;
        }
    }
}
