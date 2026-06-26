namespace UITest.Controls
{
    partial class ConnectionMonitorControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlStatus      = new System.Windows.Forms.Panel();
            this.lblModeTitle   = new System.Windows.Forms.Label();
            this.lblModeBadge   = new System.Windows.Forms.Label();
            this.btnToggleMode  = new System.Windows.Forms.Button();
            this.btnRefresh     = new System.Windows.Forms.Button();
            this.lblSvcTitle    = new System.Windows.Forms.Label();
            this.pnlSvcDot      = new System.Windows.Forms.Panel();
            this.lblSvcStatus   = new System.Windows.Forms.Label();
            this.lblComTitle    = new System.Windows.Forms.Label();
            this.pnlComDot      = new System.Windows.Forms.Panel();
            this.lblComStatus   = new System.Windows.Forms.Label();
            this.pnlLogBar      = new System.Windows.Forms.Panel();
            this.lblLogTitle    = new System.Windows.Forms.Label();
            this.btnClearLog    = new System.Windows.Forms.Button();
            this.btnExportLog   = new System.Windows.Forms.Button();
            this.rtbLog         = new System.Windows.Forms.RichTextBox();

            this.pnlStatus.SuspendLayout();
            this.pnlLogBar.SuspendLayout();
            this.SuspendLayout();

            // ════════════════════════════════════════════════
            // pnlStatus  (全寬, h=108, 背景淡灰)
            // ════════════════════════════════════════════════
            this.pnlStatus.Location  = new System.Drawing.Point(0, 0);
            this.pnlStatus.Size      = new System.Drawing.Size(1180, 108);
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(245, 245, 250);
            this.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Row 1 — 連線模式 (y=12)
            this.lblModeTitle.AutoSize = true;
            this.lblModeTitle.Location = new System.Drawing.Point(12, 16);
            this.lblModeTitle.Font     = new System.Drawing.Font("微軟正黑體", 10f, System.Drawing.FontStyle.Bold);
            this.lblModeTitle.Text     = "連線模式";

            this.lblModeBadge.Location  = new System.Drawing.Point(90, 11);
            this.lblModeBadge.Size      = new System.Drawing.Size(120, 26);
            this.lblModeBadge.BackColor = System.Drawing.Color.Orange;
            this.lblModeBadge.ForeColor = System.Drawing.Color.White;
            this.lblModeBadge.Font      = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold);
            this.lblModeBadge.Text      = "直連模式";
            this.lblModeBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Row 2 — DCUService (y=46)
            this.lblSvcTitle.AutoSize = true;
            this.lblSvcTitle.Location = new System.Drawing.Point(12, 48);
            this.lblSvcTitle.Text     = "DCUService";

            this.pnlSvcDot.Location  = new System.Drawing.Point(98, 52);
            this.pnlSvcDot.Size      = new System.Drawing.Size(12, 12);
            this.pnlSvcDot.BackColor = System.Drawing.Color.Gray;

            this.lblSvcStatus.AutoSize = true;
            this.lblSvcStatus.Location = new System.Drawing.Point(116, 48);
            this.lblSvcStatus.Text     = "未偵測";
            this.lblSvcStatus.ForeColor = System.Drawing.Color.Gray;

            // Row 3 — COM Port (y=76)
            this.lblComTitle.AutoSize = true;
            this.lblComTitle.Location = new System.Drawing.Point(12, 80);
            this.lblComTitle.Text     = "COM Port";

            this.pnlComDot.Location  = new System.Drawing.Point(79, 84);
            this.pnlComDot.Size      = new System.Drawing.Size(12, 12);
            this.pnlComDot.BackColor = System.Drawing.Color.Gray;

            this.lblComStatus.AutoSize = true;
            this.lblComStatus.Location = new System.Drawing.Point(97, 80);
            this.lblComStatus.Text     = "未開啟";
            this.lblComStatus.ForeColor = System.Drawing.Color.Gray;

            // Buttons (right side)
            this.btnRefresh.Location = new System.Drawing.Point(970, 12);
            this.btnRefresh.Size     = new System.Drawing.Size(90, 28);
            this.btnRefresh.Text     = "重新偵測";
            this.btnRefresh.Click   += new System.EventHandler(this.btnRefresh_Click);

            this.btnToggleMode.Location = new System.Drawing.Point(1070, 12);
            this.btnToggleMode.Size     = new System.Drawing.Size(100, 28);
            this.btnToggleMode.Text     = "切換為 Service";
            this.btnToggleMode.Click   += new System.EventHandler(this.btnToggleMode_Click);

            // Add to pnlStatus
            this.pnlStatus.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblModeTitle, this.lblModeBadge,
                this.lblSvcTitle,  this.pnlSvcDot, this.lblSvcStatus,
                this.lblComTitle,  this.pnlComDot, this.lblComStatus,
                this.btnRefresh,   this.btnToggleMode
            });

            // ════════════════════════════════════════════════
            // pnlLogBar  (全寬, h=30, 記錄標題列)
            // ════════════════════════════════════════════════
            this.pnlLogBar.Location  = new System.Drawing.Point(0, 108);
            this.pnlLogBar.Size      = new System.Drawing.Size(1180, 30);
            this.pnlLogBar.BackColor = System.Drawing.Color.FromArgb(230, 230, 240);
            this.pnlLogBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Location = new System.Drawing.Point(8, 7);
            this.lblLogTitle.Font     = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.Text     = "通訊記錄";

            this.btnClearLog.Location = new System.Drawing.Point(1090, 4);
            this.btnClearLog.Size     = new System.Drawing.Size(50, 22);
            this.btnClearLog.Text     = "清除";
            this.btnClearLog.Click   += new System.EventHandler(this.btnClearLog_Click);

            this.btnExportLog.Location = new System.Drawing.Point(1030, 4);
            this.btnExportLog.Size     = new System.Drawing.Size(55, 22);
            this.btnExportLog.Text     = "匯出";
            this.btnExportLog.Click   += new System.EventHandler(this.btnExportLog_Click);

            this.pnlLogBar.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblLogTitle, this.btnClearLog, this.btnExportLog
            });

            // ════════════════════════════════════════════════
            // rtbLog  (全寬, 填滿剩餘空間)
            // ════════════════════════════════════════════════
            this.rtbLog.Location   = new System.Drawing.Point(0, 138);
            this.rtbLog.Size       = new System.Drawing.Size(1180, 452);
            this.rtbLog.BackColor  = System.Drawing.Color.FromArgb(18, 18, 18);
            this.rtbLog.ForeColor  = System.Drawing.Color.White;
            this.rtbLog.Font       = new System.Drawing.Font("Consolas", 9.5f);
            this.rtbLog.ReadOnly   = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.WordWrap   = false;

            // ════════════════════════════════════════════════
            // UserControl
            // ════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(1180, 590);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.pnlStatus, this.pnlLogBar, this.rtbLog
            });

            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.pnlLogBar.ResumeLayout(false);
            this.pnlLogBar.PerformLayout();
            this.ResumeLayout(false);
        }

        // ── Fields ───────────────────────────────────────────────────────
        private System.Windows.Forms.Panel       pnlStatus;
        private System.Windows.Forms.Label       lblModeTitle;
        private System.Windows.Forms.Label       lblModeBadge;
        private System.Windows.Forms.Button      btnToggleMode;
        private System.Windows.Forms.Button      btnRefresh;
        private System.Windows.Forms.Label       lblSvcTitle;
        private System.Windows.Forms.Panel       pnlSvcDot;
        private System.Windows.Forms.Label       lblSvcStatus;
        private System.Windows.Forms.Label       lblComTitle;
        private System.Windows.Forms.Panel       pnlComDot;
        private System.Windows.Forms.Label       lblComStatus;
        private System.Windows.Forms.Panel       pnlLogBar;
        private System.Windows.Forms.Label       lblLogTitle;
        private System.Windows.Forms.Button      btnClearLog;
        private System.Windows.Forms.Button      btnExportLog;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}
