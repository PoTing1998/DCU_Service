namespace UITest.Controls
{
    partial class SystemControlPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabInner          = new System.Windows.Forms.TabControl();
            this.tabConnMonitor    = new System.Windows.Forms.TabPage();
            this.tabCommStatus     = new System.Windows.Forms.TabPage();
            this.tabBrightness     = new System.Windows.Forms.TabPage();
            this.tabDisplayMode    = new System.Windows.Forms.TabPage();
            this.tabTimeout        = new System.Windows.Forms.TabPage();
            this.tabCalendar       = new System.Windows.Forms.TabPage();
            this.tabPowerSwitch    = new System.Windows.Forms.TabPage();
            this.tabAlarmSwitch    = new System.Windows.Forms.TabPage();
            this.tabCountdown      = new System.Windows.Forms.TabPage();
            this.tabPreRecord      = new System.Windows.Forms.TabPage();
            this.tabPktBuilder     = new System.Windows.Forms.TabPage();
            this.tabPktVerify      = new System.Windows.Forms.TabPage();
            this.connMonitorCtrl   = new ConnectionMonitorControl();
            this.brightnessCtrl    = new BrightnessControl();
            this.commTimeoutCtrl   = new CommTimeoutControl();
            this.calendarClockCtrl = new CalendarClockControl();
            this.displayModeCtrl   = new DisplayModeControl();
            this.displayPowerCtrl  = new DisplayPowerControl();
            this.alarmLightCtrl    = new AlarmLightControl();
            this.countdownUnitCtrl = new CountdownUnitControl();
            this.commStatusCtrl    = new CommStatusControl();
            this.preRecordCtrl     = new PreRecordControl();
            this.packetBuilderCtrl = new UITest.Controls.PacketBuilderControl();
            this.packetVerifyCtrl  = new UITest.Controls.PacketVerifyControl();
            this.lblPlaceholder3   = new System.Windows.Forms.Label();
            this.lblPlaceholder6   = new System.Windows.Forms.Label();
            this.lblPlaceholder7   = new System.Windows.Forms.Label();
            this.lblPlaceholder8   = new System.Windows.Forms.Label();

            this.tabInner.SuspendLayout();
            this.tabConnMonitor.SuspendLayout();
            this.tabCommStatus.SuspendLayout();
            this.tabBrightness.SuspendLayout();
            this.tabDisplayMode.SuspendLayout();
            this.tabTimeout.SuspendLayout();
            this.tabCalendar.SuspendLayout();
            this.tabPowerSwitch.SuspendLayout();
            this.tabAlarmSwitch.SuspendLayout();
            this.tabCountdown.SuspendLayout();
            this.tabPreRecord.SuspendLayout();
            this.tabPktBuilder.SuspendLayout();
            this.tabPktVerify.SuspendLayout();
            this.SuspendLayout();

            // ── tabInner ─────────────────────────────────────
            this.tabInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabInner.Controls.AddRange(new System.Windows.Forms.TabPage[]
            {
                this.tabConnMonitor,
                this.tabCommStatus,
                this.tabBrightness,
                this.tabDisplayMode,
                this.tabTimeout,
                this.tabCalendar,
                this.tabPowerSwitch,
                this.tabAlarmSwitch,
                this.tabCountdown,
                this.tabPreRecord,
                this.tabPktBuilder,
                this.tabPktVerify
            });

            // ── 連線監控 ─────────────────────────────────────
            this.tabConnMonitor.Text    = "連線監控";
            this.tabConnMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnMonitor.Controls.Add(this.connMonitorCtrl);
            this.connMonitorCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── 通訊狀態與資源版本 ───────────────────────────
            this.tabCommStatus.Text    = "通訊狀態與資源版本";
            this.tabCommStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommStatus.Controls.Add(this.commStatusCtrl);
            this.commStatusCtrl.Dock = System.Windows.Forms.DockStyle.Fill;


            // ── 亮度設定 ─────────────────────────────────────
            this.tabBrightness.Text    = "亮度設定";
            this.tabBrightness.Padding = new System.Windows.Forms.Padding(3);
            this.tabBrightness.Controls.Add(this.brightnessCtrl);

            // ── 顯示模式 ─────────────────────────────────────
            this.tabDisplayMode.Text    = "顯示模式";
            this.tabDisplayMode.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplayMode.Controls.Add(this.displayModeCtrl);

            // ── 通訊逾時時間 ─────────────────────────────────
            this.tabTimeout.Text    = "通訊逾時時間";
            this.tabTimeout.Padding = new System.Windows.Forms.Padding(3);
            this.tabTimeout.Controls.Add(this.commTimeoutCtrl);

            // ── 萬年曆IC時鐘設定 ─────────────────────────────
            this.tabCalendar.Text    = "萬年曆IC時鐘設定";
            this.tabCalendar.Padding = new System.Windows.Forms.Padding(3);
            this.tabCalendar.Controls.Add(this.calendarClockCtrl);

            // ── 顯示器開關機 ─────────────────────────────────
            this.tabPowerSwitch.Text    = "顯示器開關機";
            this.tabPowerSwitch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPowerSwitch.Controls.Add(this.displayPowerCtrl);

            // ── 警示燈開關設定 ───────────────────────────────
            // ── 警示燈開關設定 ───────────────────────────────
            this.tabAlarmSwitch.Text    = "警示燈開關設定";
            this.tabAlarmSwitch.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlarmSwitch.Controls.Add(this.alarmLightCtrl);

            // ── 倒數時間單位設定 ─────────────────────────────
            this.lblPlaceholder8.AutoSize  = true;
            // ── 倒數時間單位設定 ─────────────────────────────
            this.tabCountdown.Text    = "倒數時間單位設定";
            this.tabCountdown.Padding = new System.Windows.Forms.Padding(3);
            this.tabCountdown.Controls.Add(this.countdownUnitCtrl);

            // ── 預錄封包直接傳送 ─────────────────────────────
            this.tabPreRecord.Text    = "預錄封包直接傳送";
            this.tabPreRecord.Padding = new System.Windows.Forms.Padding(3);
            this.tabPreRecord.Controls.Add(this.preRecordCtrl);
            this.preRecordCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── 封包組成 ─────────────────────────────────────
            this.tabPktBuilder.Text    = "封包組成";
            this.tabPktBuilder.Padding = new System.Windows.Forms.Padding(5);
            this.tabPktBuilder.Controls.Add(this.packetBuilderCtrl);
            this.packetBuilderCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── 封包驗證 ─────────────────────────────────────
            this.tabPktVerify.Text    = "封包驗證";
            this.tabPktVerify.Padding = new System.Windows.Forms.Padding(5);
            this.tabPktVerify.Controls.Add(this.packetVerifyCtrl);
            this.packetVerifyCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── SystemControlPanel ───────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabInner);

            this.tabInner.ResumeLayout(false);
            this.tabConnMonitor.ResumeLayout(false);
            this.tabBrightness.ResumeLayout(false);
            this.tabCommStatus.ResumeLayout(false);
            this.tabBrightness.ResumeLayout(false);
            this.tabDisplayMode.ResumeLayout(false);
            this.tabTimeout.ResumeLayout(false);
            this.tabCalendar.ResumeLayout(false);
            this.tabPowerSwitch.ResumeLayout(false);
            this.tabAlarmSwitch.ResumeLayout(false);
            this.tabCountdown.ResumeLayout(false);
            this.tabPreRecord.ResumeLayout(false);
            this.tabPktBuilder.ResumeLayout(false);
            this.tabPktVerify.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl      tabInner;
        private System.Windows.Forms.TabPage         tabConnMonitor;
        private System.Windows.Forms.TabPage         tabCommStatus;
        private System.Windows.Forms.TabPage         tabBrightness;
        private System.Windows.Forms.TabPage         tabDisplayMode;
        private System.Windows.Forms.TabPage         tabTimeout;
        private System.Windows.Forms.TabPage         tabCalendar;
        private System.Windows.Forms.TabPage         tabPowerSwitch;
        private System.Windows.Forms.TabPage         tabAlarmSwitch;
        private System.Windows.Forms.TabPage         tabCountdown;
        private System.Windows.Forms.TabPage         tabPreRecord;
        private System.Windows.Forms.TabPage         tabPktBuilder;
        private System.Windows.Forms.TabPage         tabPktVerify;
        private CommStatusControl                    commStatusCtrl;
        private PreRecordControl                     preRecordCtrl;
        private UITest.Controls.PacketBuilderControl packetBuilderCtrl;
        private UITest.Controls.PacketVerifyControl  packetVerifyCtrl;
        private ConnectionMonitorControl             connMonitorCtrl;
        private DisplayModeControl                   displayModeCtrl;
        private DisplayPowerControl                  displayPowerCtrl;
        private AlarmLightControl                    alarmLightCtrl;
        private CountdownUnitControl                 countdownUnitCtrl;
        private BrightnessControl                    brightnessCtrl;
        private System.Windows.Forms.Label           lblPlaceholder3;
        private CommTimeoutControl                   commTimeoutCtrl;
        private CalendarClockControl                 calendarClockCtrl;
        private System.Windows.Forms.Label           lblPlaceholder6;
        private System.Windows.Forms.Label           lblPlaceholder7;
        private System.Windows.Forms.Label           lblPlaceholder8;
    }
}
