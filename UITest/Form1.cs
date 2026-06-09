using System.Windows.Forms;

namespace UITest
{
    public partial class Form1 : Form
    {
        private CommLogForm _logForm;

        public Form1()
        {
            InitializeComponent();

            // 啟動時預設顯示「串列埠設定」Tab
            tabControl.SelectedTab = tabSerial;

            // ── 預錄封包直接傳送 ─────────────────────────────────────────
            sysCtrlPanel.PreRecordSendAction    = bytes => serialSettingCtrl.SendData(bytes);

            // ── 顯示器板型訊息上傳 ───────────────────────────────────────
            displayMessageCtrl.SendAction       = bytes => serialSettingCtrl.SendData(bytes);
            displayMessageCtrl.GetUpIDsFunc     = () => serialSettingCtrl.GetSelectedUpPlatformIDs();
            displayMessageCtrl.GetDnIDsFunc     = () => serialSettingCtrl.GetSelectedDnPlatformIDs();

            // ── 顯示模式 / 開關機 ────────────────────────────────────────
            sysCtrlPanel.CommTimeoutSendAction  = bytes => serialSettingCtrl.SendData(bytes);
            sysCtrlPanel.CalendarSendAction     = bytes => serialSettingCtrl.SendData(bytes);
            sysCtrlPanel.CalendarReadAction     = bytes => { serialSettingCtrl.SendData(bytes); return null; };
            sysCtrlPanel.BrightnessSendAction   = bytes => serialSettingCtrl.SendData(bytes);
            sysCtrlPanel.DisplayModeSendAction  = bytes => serialSettingCtrl.SendData(bytes);
            sysCtrlPanel.DisplayPowerSendAction = bytes => serialSettingCtrl.SendData(bytes);
            sysCtrlPanel.AlarmLightSendAction    = bytes => serialSettingCtrl.SendData(bytes);
            sysCtrlPanel.CountdownUnitSendAction = bytes => serialSettingCtrl.SendData(bytes);

            // ── 建立 CommLogForm（懶惰初始化，不立即顯示）──────────────
            _logForm = new CommLogForm();
            _logForm.Owner = this;   // 跟隨主視窗最小化/還原
        }

        // ── 顯示狀態視窗 checkbox ─────────────────────────────────────────

        private void chkShowLogWindow_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkShowLogWindow.Checked)
            {
                _logForm.Show();
                _logForm.BringToFront();
            }
            else
            {
                _logForm.Hide();
            }
        }

        // 主視窗關閉時同步關閉 LOG 視窗
        protected override void OnFormClosed(System.Windows.Forms.FormClosedEventArgs e)
        {
            _logForm?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
