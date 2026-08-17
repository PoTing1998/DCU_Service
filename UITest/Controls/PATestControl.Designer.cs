namespace UITest.Controls
{
    partial class PATestControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpConn        = new System.Windows.Forms.GroupBox();
            this.lblCOM         = new System.Windows.Forms.Label();
            this.cmbCOM         = new System.Windows.Forms.ComboBox();
            this.lblBaud        = new System.Windows.Forms.Label();
            this.cmbBaudRate    = new System.Windows.Forms.ComboBox();
            this.btnOpen        = new System.Windows.Forms.Button();
            this.lblConnStatus  = new System.Windows.Forms.Label();

            this.grpPacket      = new System.Windows.Forms.GroupBox();
            this.lblStation     = new System.Windows.Forms.Label();
            this.cmbStation     = new System.Windows.Forms.ComboBox();
            this.lblPlatform    = new System.Windows.Forms.Label();
            this.cmbPlatform    = new System.Windows.Forms.ComboBox();
            this.lblSituation   = new System.Windows.Forms.Label();
            this.cmbSituation   = new System.Windows.Forms.ComboBox();
            this.lblFormatInfo  = new System.Windows.Forms.Label();
            this.btnBuild       = new System.Windows.Forms.Button();
            this.btnSend        = new System.Windows.Forms.Button();
            this.btnClear       = new System.Windows.Forms.Button();

            this.txtOutput      = new System.Windows.Forms.TextBox();

            this.grpConn.SuspendLayout();
            this.grpPacket.SuspendLayout();
            this.SuspendLayout();

            // ── grpConn：序列埠連線設定（PA 專用，獨立於其他分頁）───────────
            this.grpConn.Text     = "序列埠連線設定（PA 專用，獨立連線）";
            this.grpConn.Location = new System.Drawing.Point(12, 12);
            this.grpConn.Size     = new System.Drawing.Size(860, 64);
            this.grpConn.Anchor   = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpConn.Controls.Add(this.lblCOM);
            this.grpConn.Controls.Add(this.cmbCOM);
            this.grpConn.Controls.Add(this.lblBaud);
            this.grpConn.Controls.Add(this.cmbBaudRate);
            this.grpConn.Controls.Add(this.btnOpen);
            this.grpConn.Controls.Add(this.lblConnStatus);

            this.lblCOM.AutoSize = true;
            this.lblCOM.Location = new System.Drawing.Point(16, 28);
            this.lblCOM.Text     = "COM Port";

            this.cmbCOM.Location      = new System.Drawing.Point(84, 24);
            this.cmbCOM.Size          = new System.Drawing.Size(90, 21);
            this.cmbCOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblBaud.AutoSize = true;
            this.lblBaud.Location = new System.Drawing.Point(196, 28);
            this.lblBaud.Text     = "傳輸速率";

            this.cmbBaudRate.Location      = new System.Drawing.Point(260, 24);
            this.cmbBaudRate.Size          = new System.Drawing.Size(90, 21);
            this.cmbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.btnOpen.Location = new System.Drawing.Point(370, 22);
            this.btnOpen.Size     = new System.Drawing.Size(70, 26);
            this.btnOpen.Text     = "開啟";
            this.btnOpen.Click   += new System.EventHandler(this.btnOpen_Click);

            this.lblConnStatus.AutoSize  = true;
            this.lblConnStatus.Location  = new System.Drawing.Point(460, 28);
            this.lblConnStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblConnStatus.Text      = "未連線";

            // ── grpPacket：DMD → PA 封包測試 ─────────────────────────────
            this.grpPacket.Text     = "DMD → PA 封包測試（車站 / 月台 / 列車狀況）";
            this.grpPacket.Location = new System.Drawing.Point(12, 84);
            this.grpPacket.Size     = new System.Drawing.Size(860, 112);
            this.grpPacket.Anchor   = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpPacket.Controls.Add(this.lblStation);
            this.grpPacket.Controls.Add(this.cmbStation);
            this.grpPacket.Controls.Add(this.lblPlatform);
            this.grpPacket.Controls.Add(this.cmbPlatform);
            this.grpPacket.Controls.Add(this.lblSituation);
            this.grpPacket.Controls.Add(this.cmbSituation);
            this.grpPacket.Controls.Add(this.lblFormatInfo);
            this.grpPacket.Controls.Add(this.btnBuild);
            this.grpPacket.Controls.Add(this.btnSend);
            this.grpPacket.Controls.Add(this.btnClear);

            this.lblStation.AutoSize = true;
            this.lblStation.Location = new System.Drawing.Point(16, 30);
            this.lblStation.Text     = "車站";

            this.cmbStation.Location      = new System.Drawing.Point(60, 26);
            this.cmbStation.Size          = new System.Drawing.Size(150, 21);
            this.cmbStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Location = new System.Drawing.Point(226, 30);
            this.lblPlatform.Text     = "月台狀況";

            this.cmbPlatform.Location      = new System.Drawing.Point(296, 26);
            this.cmbPlatform.Size          = new System.Drawing.Size(200, 21);
            this.cmbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblSituation.AutoSize = true;
            this.lblSituation.Location = new System.Drawing.Point(510, 30);
            this.lblSituation.Text     = "列車狀況";

            this.cmbSituation.Location      = new System.Drawing.Point(580, 26);
            this.cmbSituation.Size          = new System.Drawing.Size(200, 21);
            this.cmbSituation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblFormatInfo.AutoSize  = true;
            this.lblFormatInfo.Location  = new System.Drawing.Point(16, 62);
            this.lblFormatInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblFormatInfo.Text      = "封包格式：DLE STX TYP SEQ LEN [cmd 車站 月台 列車狀況] LRC DLE ETX（含完整框架，自動計算 SEQ/LRC）";

            this.btnBuild.Location = new System.Drawing.Point(16, 84);
            this.btnBuild.Size     = new System.Drawing.Size(90, 26);
            this.btnBuild.Text     = "組出封包";
            this.btnBuild.Click   += new System.EventHandler(this.btnBuild_Click);

            this.btnSend.Location = new System.Drawing.Point(116, 84);
            this.btnSend.Size     = new System.Drawing.Size(90, 26);
            this.btnSend.Text     = "傳送";
            this.btnSend.Click   += new System.EventHandler(this.btnSend_Click);

            this.btnClear.Location = new System.Drawing.Point(216, 84);
            this.btnClear.Size     = new System.Drawing.Size(90, 26);
            this.btnClear.Text     = "清除記錄";
            this.btnClear.Click   += new System.EventHandler(this.btnClear_Click);

            // ── txtOutput：組建/傳送/接收記錄 ────────────────────────────
            this.txtOutput.Location      = new System.Drawing.Point(12, 202);
            this.txtOutput.Size          = new System.Drawing.Size(860, 380);
            this.txtOutput.Anchor        = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                                          | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtOutput.Multiline     = true;
            this.txtOutput.ReadOnly      = true;
            this.txtOutput.ScrollBars    = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Font          = new System.Drawing.Font("Consolas", 9F);

            // ── PATestControl ────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(884, 590);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.grpPacket);
            this.Controls.Add(this.grpConn);

            this.grpConn.ResumeLayout(false);
            this.grpConn.PerformLayout();
            this.grpPacket.ResumeLayout(false);
            this.grpPacket.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox grpConn;
        private System.Windows.Forms.Label    lblCOM;
        private System.Windows.Forms.ComboBox cmbCOM;
        private System.Windows.Forms.Label    lblBaud;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Button   btnOpen;
        private System.Windows.Forms.Label    lblConnStatus;

        private System.Windows.Forms.GroupBox grpPacket;
        private System.Windows.Forms.Label    lblStation;
        private System.Windows.Forms.ComboBox cmbStation;
        private System.Windows.Forms.Label    lblPlatform;
        private System.Windows.Forms.ComboBox cmbPlatform;
        private System.Windows.Forms.Label    lblSituation;
        private System.Windows.Forms.ComboBox cmbSituation;
        private System.Windows.Forms.Label    lblFormatInfo;
        private System.Windows.Forms.Button   btnBuild;
        private System.Windows.Forms.Button   btnSend;
        private System.Windows.Forms.Button   btnClear;

        private System.Windows.Forms.TextBox  txtOutput;
    }
}
