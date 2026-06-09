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
            this.tabCommStatus     = new System.Windows.Forms.TabPage();
            this.tabWhiteBalance   = new System.Windows.Forms.TabPage();
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
            this.commStatusCtrl    = new CommStatusControl();
            this.preRecordCtrl     = new PreRecordControl();
            this.packetBuilderCtrl = new UITest.Controls.PacketBuilderControl();
            this.packetVerifyCtrl  = new UITest.Controls.PacketVerifyControl();
            this.lblPlaceholder1   = new System.Windows.Forms.Label();
            this.lblPlaceholder2   = new System.Windows.Forms.Label();
            this.lblPlaceholder3   = new System.Windows.Forms.Label();
            this.lblPlaceholder4   = new System.Windows.Forms.Label();
            this.lblPlaceholder5   = new System.Windows.Forms.Label();
            this.lblPlaceholder6   = new System.Windows.Forms.Label();
            this.lblPlaceholder7   = new System.Windows.Forms.Label();
            this.lblPlaceholder8   = new System.Windows.Forms.Label();

            this.tabInner.SuspendLayout();
            this.tabCommStatus.SuspendLayout();
            this.tabWhiteBalance.SuspendLayout();
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
                this.tabCommStatus,
                this.tabWhiteBalance,
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

            // ── 通訊狀態與資源版本 ───────────────────────────
            this.tabCommStatus.Text    = "通訊狀態與資源版本";
            this.tabCommStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommStatus.Controls.Add(this.commStatusCtrl);
            this.commStatusCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── 顯示器白平衡 ──────────────────────────────────
            this.lblPlaceholder1.AutoSize  = true;
            this.lblPlaceholder1.Location  = new System.Drawing.Point(16, 16);
            this.lblPlaceholder1.Text      = "（功能開發中）";
            this.lblPlaceholder1.ForeColor = System.Drawing.Color.Gray;
            this.tabWhiteBalance.Text    = "顯示器白平衡";
            this.tabWhiteBalance.Padding = new System.Windows.Forms.Padding(3);
            this.tabWhiteBalance.Controls.Add(this.lblPlaceholder1);

            // ── 亮度設定 ─────────────────────────────────────
            this.lblPlaceholder2.AutoSize  = true;
            this.lblPlaceholder2.Location  = new System.Drawing.Point(16, 16);
            this.lblPlaceholder2.Text      = "（功能開發中）";
            this.lblPlaceholder2.ForeColor = System.Drawing.Color.Gray;
            this.tabBrightness.Text    = "亮度設定";
            this.tabBrightness.Padding = new System.Windows.Forms.Padding(3);
            this.tabBrightness.Controls.Add(this.lblPlaceholder2);

            // ── 顯示模式 ─────────────────────────────────────
            this.lblPlaceholder3.AutoSize  = true;
            this.lblPlaceholder3.Location  = new System.Drawing.Point(16, 16);
            this.lblPlaceholder3.Text      = "（功能開發中）";
            this.lblPlaceholder3.ForeColor = System.Drawing.Color.Gray;
            this.tabDisplayMode.Text    = "顯示模式";
            this.tabDisplayMode.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplayMode.Controls.Add(this.lblPlaceholder3);

            // ── 通訊逾時時間 ─────────────────────────────────
            this.lblPlaceholder4.AutoSize  = true;
            this.lblPlaceholder4.Location  = new System.Drawing.Point(16, 16);
            this.lblPlaceholder4.Text      = "（功能開發中）";
            this.lblPlaceholder4.ForeColor = System.Drawing.Color.Gray;
            this.tabTimeout.Text    = "通訊逾時時間";
            this.tabTimeout.Padding = new System.Windows.Forms.Padding(3);
            this.tabTimeout.Controls.Add(this.lblPlaceholder4);

            // ── 萬年曆IC時鐘設定 ─────────────────────────────
            this.lblPlaceholder5.AutoSize  = true;
            this.lblPlaceholder5.Location  = new System.Drawing.Point(16, 16);
            this.lblPlaceholder5.Text      = "（功能開發中）";
            this.lblPlaceholder5.ForeColor = System.Drawing.Color.Gray;
            this.tabCalendar.Text    = "萬年曆IC時鐘設定";
            this.tabCalendar.Padding = new System.Windows.Forms.Padding(3);
            this.tabCalendar.Controls.Add(this.lblPlaceholder5);

            // ── 顯示器開關機 ─────────────────────────────────
            this.lblPlaceholder6.AutoSize  = true;
            this.lblPlaceholder6.Location  = new System.Drawing.Point(16, 16);
            this.lblPlaceholder6.Text      = "（功能開發中）";
            this.lblPlaceholder6.ForeColor = System.Drawing.Color.Gray;
            this.tabPowerSwitch.Text    = "顯示器開關機";
            this.tabPowerSwitch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPowerSwitch.Controls.Add(this.lblPlaceholder6);

            // ── 警示燈開關設定 ───────────────────────────────
            this.lblPlaceholder7.AutoSize  = true;
            this.lblPlaceholder7.Location  = new System.Drawing.Point(16, 16);
            this.lblPlaceholder7.Text      = "（功能開發中）";
            this.lblPlaceholder7.ForeColor = System.Drawing.Color.Gray;
            this.tabAlarmSwitch.Text    = "警示燈開關設定";
            this.tabAlarmSwitch.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlarmSwitch.Controls.Add(this.lblPlaceholder7);

            // ── 倒數時間單位設定 ─────────────────────────────
            this.lblPlaceholder8.AutoSize  = true;
            this.lblPlaceholder8.Location  = new System.Drawing.Point(16, 16);
            this.lblPlaceholder8.Text      = "（功能開發中）";
            this.lblPlaceholder8.ForeColor = System.Drawing.Color.Gray;
            this.tabCountdown.Text    = "倒數時間單位設定";
            this.tabCountdown.Padding = new System.Windows.Forms.Padding(3);
            this.tabCountdown.Controls.Add(this.lblPlaceholder8);

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
            this.tabCommStatus.ResumeLayout(false);
            this.tabWhiteBalance.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage         tabCommStatus;
        private System.Windows.Forms.TabPage         tabWhiteBalance;
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
        private System.Windows.Forms.Label           lblPlaceholder1;
        private System.Windows.Forms.Label           lblPlaceholder2;
        private System.Windows.Forms.Label           lblPlaceholder3;
        private System.Windows.Forms.Label           lblPlaceholder4;
        private System.Windows.Forms.Label           lblPlaceholder5;
        private System.Windows.Forms.Label           lblPlaceholder6;
        private System.Windows.Forms.Label           lblPlaceholder7;
        private System.Windows.Forms.Label           lblPlaceholder8;
    }
}
