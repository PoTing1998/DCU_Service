namespace UITest.Controls
{
    partial class SerialSettingControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── 串列埠設定 ──
            this.grpSerial   = new System.Windows.Forms.GroupBox();
            this.lblCOM      = new System.Windows.Forms.Label();
            this.cmbCOM      = new System.Windows.Forms.ComboBox();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.lblDatabit  = new System.Windows.Forms.Label();
            this.cmbDatabit  = new System.Windows.Forms.ComboBox();
            this.lblParity   = new System.Windows.Forms.Label();
            this.cmbParity   = new System.Windows.Forms.ComboBox();
            this.lblStopbit  = new System.Windows.Forms.Label();
            this.cmbStopbit  = new System.Windows.Forms.ComboBox();
            this.btnOpen     = new System.Windows.Forms.Button();
            // ── 參數設定 ──
            this.grpParam   = new System.Windows.Forms.GroupBox();
            this.lblDelay   = new System.Windows.Forms.Label();
            this.nudDelay   = new System.Windows.Forms.NumericUpDown();
            this.lblMaxRows = new System.Windows.Forms.Label();
            this.nudMaxRows = new System.Windows.Forms.NumericUpDown();
            this.btnSave    = new System.Windows.Forms.Button();
            // ── 大廳層點矩陣顯示器 ──
            this.grpLobby     = new System.Windows.Forms.GroupBox();
            this.chkAllLobby  = new System.Windows.Forms.CheckBox();
            this.lblLobby     = new System.Windows.Forms.Label();
            this.chkLobby_ID1 = new System.Windows.Forms.CheckBox();
            this.chkLobby_ID2 = new System.Windows.Forms.CheckBox();
            this.chkLobby_ID3 = new System.Windows.Forms.CheckBox();
            this.chkLobby_ID4 = new System.Windows.Forms.CheckBox();
            this.lblExchange  = new System.Windows.Forms.Label();
            this.chkLobby_ID5 = new System.Windows.Forms.CheckBox();
            this.chkLobby_ID6 = new System.Windows.Forms.CheckBox();
            this.chkLobby_ID7 = new System.Windows.Forms.CheckBox();
            // ── 月臺層點矩陣顯示器 ──
            this.grpPlatform      = new System.Windows.Forms.GroupBox();
            this.chkAllPlatform   = new System.Windows.Forms.CheckBox();
            this.lblUpPlatform    = new System.Windows.Forms.Label();
            this.chkPlatform_ID11 = new System.Windows.Forms.CheckBox();
            this.chkPlatform_ID12 = new System.Windows.Forms.CheckBox();
            this.chkPlatform_ID13 = new System.Windows.Forms.CheckBox();
            this.chkPlatform_ID14 = new System.Windows.Forms.CheckBox();
            this.lblDownPlatform  = new System.Windows.Forms.Label();
            this.chkPlatform_ID15 = new System.Windows.Forms.CheckBox();
            this.chkPlatform_ID16 = new System.Windows.Forms.CheckBox();
            this.chkPlatform_ID17 = new System.Windows.Forms.CheckBox();
            this.chkPlatform_ID18 = new System.Windows.Forms.CheckBox();

            this.grpSerial.SuspendLayout();
            this.grpParam.SuspendLayout();
            this.grpLobby.SuspendLayout();
            this.grpPlatform.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRows)).BeginInit();
            this.SuspendLayout();

            // ════════════════════════════════════════════════
            // grpSerial  (rowH=28, startY=12, comboX=70, comboW=130)
            // ════════════════════════════════════════════════
            this.grpSerial.Location = new System.Drawing.Point(10, 10);
            this.grpSerial.Size     = new System.Drawing.Size(220, 185);
            this.grpSerial.Text     = "";
            this.grpSerial.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblCOM,      this.cmbCOM,
                this.lblBaudRate, this.cmbBaudRate,
                this.lblDatabit,  this.cmbDatabit,
                this.lblParity,   this.cmbParity,
                this.lblStopbit,  this.cmbStopbit,
                this.btnOpen
            });

            // COM (y=12)
            this.lblCOM.AutoSize = true;
            this.lblCOM.Location = new System.Drawing.Point(8, 17);
            this.lblCOM.Text     = "COM";
            this.cmbCOM.Location      = new System.Drawing.Point(70, 12);
            this.cmbCOM.Size          = new System.Drawing.Size(130, 21);
            this.cmbCOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // BaudRate (y=40)
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(8, 45);
            this.lblBaudRate.Text     = "BaudRate";
            this.cmbBaudRate.Location      = new System.Drawing.Point(70, 40);
            this.cmbBaudRate.Size          = new System.Drawing.Size(130, 21);
            this.cmbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Databit (y=68)
            this.lblDatabit.AutoSize = true;
            this.lblDatabit.Location = new System.Drawing.Point(8, 73);
            this.lblDatabit.Text     = "Databit";
            this.cmbDatabit.Location      = new System.Drawing.Point(70, 68);
            this.cmbDatabit.Size          = new System.Drawing.Size(130, 21);
            this.cmbDatabit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Parity (y=96)
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new System.Drawing.Point(8, 101);
            this.lblParity.Text     = "Parity";
            this.cmbParity.Location      = new System.Drawing.Point(70, 96);
            this.cmbParity.Size          = new System.Drawing.Size(130, 21);
            this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Stopbit (y=124)
            this.lblStopbit.AutoSize = true;
            this.lblStopbit.Location = new System.Drawing.Point(8, 129);
            this.lblStopbit.Text     = "Stopbit";
            this.cmbStopbit.Location      = new System.Drawing.Point(70, 124);
            this.cmbStopbit.Size          = new System.Drawing.Size(130, 21);
            this.cmbStopbit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Open button (y=152)
            this.btnOpen.Location = new System.Drawing.Point(70, 152);
            this.btnOpen.Size     = new System.Drawing.Size(60, 24);
            this.btnOpen.Text     = "開啟";
            this.btnOpen.Click   += new System.EventHandler(this.btnOpen_Click);

            // ════════════════════════════════════════════════
            // grpParam
            // ════════════════════════════════════════════════
            this.grpParam.Location = new System.Drawing.Point(10, 205);
            this.grpParam.Size     = new System.Drawing.Size(220, 130);
            this.grpParam.Text     = "參數設定";
            this.grpParam.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblDelay, this.nudDelay,
                this.lblMaxRows, this.nudMaxRows,
                this.btnSave
            });

            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(8, 20);
            this.lblDelay.Text     = "字型圖框封包之間延遲（ms）";
            this.nudDelay.Location = new System.Drawing.Point(8, 38);
            this.nudDelay.Size     = new System.Drawing.Size(100, 20);
            this.nudDelay.Minimum  = 0;
            this.nudDelay.Maximum  = 999999;
            this.nudDelay.Value    = 10000;

            this.lblMaxRows.AutoSize = true;
            this.lblMaxRows.Location = new System.Drawing.Point(8, 62);
            this.lblMaxRows.Text     = "上下行訊息顯示最多列數";
            this.nudMaxRows.Location = new System.Drawing.Point(8, 80);
            this.nudMaxRows.Size     = new System.Drawing.Size(60, 20);
            this.nudMaxRows.Minimum  = 1;
            this.nudMaxRows.Maximum  = 9999;
            this.nudMaxRows.Value    = 10;

            this.btnSave.Location = new System.Drawing.Point(8, 104);
            this.btnSave.Size     = new System.Drawing.Size(75, 24);
            this.btnSave.Text     = "儲存事件";
            this.btnSave.Click   += new System.EventHandler(this.btnSave_Click);

            // ════════════════════════════════════════════════
            // grpLobby  (x=240, chkColX=145, colStep=83, chkW=80)
            // ════════════════════════════════════════════════
            this.grpLobby.Location = new System.Drawing.Point(240, 10);
            this.grpLobby.Size     = new System.Drawing.Size(480, 90);
            this.grpLobby.Text     = "大廳層點矩陣顯示器";
            this.grpLobby.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.chkAllLobby,
                this.lblLobby,
                this.chkLobby_ID1, this.chkLobby_ID2, this.chkLobby_ID3, this.chkLobby_ID4,
                this.lblExchange,
                this.chkLobby_ID5, this.chkLobby_ID6, this.chkLobby_ID7
            });

            this.chkAllLobby.Location = new System.Drawing.Point(8, 36);
            this.chkAllLobby.Size     = new System.Drawing.Size(80, 17);
            this.chkAllLobby.Text     = "All ID(51H)";
            this.chkAllLobby.CheckedChanged += new System.EventHandler(this.chkAllLobby_CheckedChanged);

            this.lblLobby.AutoSize = true;
            this.lblLobby.Location = new System.Drawing.Point(100, 22);
            this.lblLobby.Text     = "大廳";

            this.chkLobby_ID1.Location = new System.Drawing.Point(145, 20);
            this.chkLobby_ID1.Size     = new System.Drawing.Size(80, 17);
            this.chkLobby_ID1.Text     = "ID1 (01H)";
            this.chkLobby_ID1.CheckedChanged += new System.EventHandler(this.LobbyItem_CheckedChanged);

            this.chkLobby_ID2.Location = new System.Drawing.Point(228, 20);
            this.chkLobby_ID2.Size     = new System.Drawing.Size(80, 17);
            this.chkLobby_ID2.Text     = "ID2 (02H)";
            this.chkLobby_ID2.CheckedChanged += new System.EventHandler(this.LobbyItem_CheckedChanged);

            this.chkLobby_ID3.Location = new System.Drawing.Point(311, 20);
            this.chkLobby_ID3.Size     = new System.Drawing.Size(80, 17);
            this.chkLobby_ID3.Text     = "ID3 (03H)";
            this.chkLobby_ID3.CheckedChanged += new System.EventHandler(this.LobbyItem_CheckedChanged);

            this.chkLobby_ID4.Location = new System.Drawing.Point(394, 20);
            this.chkLobby_ID4.Size     = new System.Drawing.Size(80, 17);
            this.chkLobby_ID4.Text     = "ID4 (04H)";
            this.chkLobby_ID4.CheckedChanged += new System.EventHandler(this.LobbyItem_CheckedChanged);

            this.lblExchange.AutoSize = true;
            this.lblExchange.Location = new System.Drawing.Point(96, 52);
            this.lblExchange.Text     = "交會站";

            this.chkLobby_ID5.Location = new System.Drawing.Point(145, 50);
            this.chkLobby_ID5.Size     = new System.Drawing.Size(80, 17);
            this.chkLobby_ID5.Text     = "ID5 (05H)";
            this.chkLobby_ID5.CheckedChanged += new System.EventHandler(this.LobbyItem_CheckedChanged);

            this.chkLobby_ID6.Location = new System.Drawing.Point(228, 50);
            this.chkLobby_ID6.Size     = new System.Drawing.Size(80, 17);
            this.chkLobby_ID6.Text     = "ID6 (06H)";
            this.chkLobby_ID6.CheckedChanged += new System.EventHandler(this.LobbyItem_CheckedChanged);

            this.chkLobby_ID7.Location = new System.Drawing.Point(311, 50);
            this.chkLobby_ID7.Size     = new System.Drawing.Size(80, 17);
            this.chkLobby_ID7.Text     = "ID7 (07H)";
            this.chkLobby_ID7.CheckedChanged += new System.EventHandler(this.LobbyItem_CheckedChanged);

            // ════════════════════════════════════════════════
            // grpPlatform  (x=240)
            // ════════════════════════════════════════════════
            this.grpPlatform.Location = new System.Drawing.Point(240, 110);
            this.grpPlatform.Size     = new System.Drawing.Size(480, 100);
            this.grpPlatform.Text     = "月臺層點矩陣顯示器";
            this.grpPlatform.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.chkAllPlatform,
                this.lblUpPlatform,
                this.chkPlatform_ID11, this.chkPlatform_ID12, this.chkPlatform_ID13, this.chkPlatform_ID14,
                this.lblDownPlatform,
                this.chkPlatform_ID15, this.chkPlatform_ID16, this.chkPlatform_ID17, this.chkPlatform_ID18
            });

            this.chkAllPlatform.Location = new System.Drawing.Point(8, 42);
            this.chkAllPlatform.Size     = new System.Drawing.Size(80, 17);
            this.chkAllPlatform.Text     = "All ID(52H)";
            this.chkAllPlatform.CheckedChanged += new System.EventHandler(this.chkAllPlatform_CheckedChanged);

            this.lblUpPlatform.AutoSize = true;
            this.lblUpPlatform.Location = new System.Drawing.Point(92, 22);
            this.lblUpPlatform.Text     = "上行月台";

            this.chkPlatform_ID11.Location = new System.Drawing.Point(145, 20);
            this.chkPlatform_ID11.Size     = new System.Drawing.Size(80, 17);
            this.chkPlatform_ID11.Text     = "ID11 (11H)";
            this.chkPlatform_ID11.CheckedChanged += new System.EventHandler(this.PlatformItem_CheckedChanged);

            this.chkPlatform_ID12.Location = new System.Drawing.Point(228, 20);
            this.chkPlatform_ID12.Size     = new System.Drawing.Size(80, 17);
            this.chkPlatform_ID12.Text     = "ID12 (12H)";
            this.chkPlatform_ID12.CheckedChanged += new System.EventHandler(this.PlatformItem_CheckedChanged);

            this.chkPlatform_ID13.Location = new System.Drawing.Point(311, 20);
            this.chkPlatform_ID13.Size     = new System.Drawing.Size(80, 17);
            this.chkPlatform_ID13.Text     = "ID13 (13H)";
            this.chkPlatform_ID13.CheckedChanged += new System.EventHandler(this.PlatformItem_CheckedChanged);

            this.chkPlatform_ID14.Location = new System.Drawing.Point(394, 20);
            this.chkPlatform_ID14.Size     = new System.Drawing.Size(80, 17);
            this.chkPlatform_ID14.Text     = "ID14 (14H)";
            this.chkPlatform_ID14.CheckedChanged += new System.EventHandler(this.PlatformItem_CheckedChanged);

            this.lblDownPlatform.AutoSize = true;
            this.lblDownPlatform.Location = new System.Drawing.Point(92, 55);
            this.lblDownPlatform.Text     = "下行月台";

            this.chkPlatform_ID15.Location = new System.Drawing.Point(145, 53);
            this.chkPlatform_ID15.Size     = new System.Drawing.Size(80, 17);
            this.chkPlatform_ID15.Text     = "ID15 (15H)";
            this.chkPlatform_ID15.CheckedChanged += new System.EventHandler(this.PlatformItem_CheckedChanged);

            this.chkPlatform_ID16.Location = new System.Drawing.Point(228, 53);
            this.chkPlatform_ID16.Size     = new System.Drawing.Size(80, 17);
            this.chkPlatform_ID16.Text     = "ID16 (16H)";
            this.chkPlatform_ID16.CheckedChanged += new System.EventHandler(this.PlatformItem_CheckedChanged);

            this.chkPlatform_ID17.Location = new System.Drawing.Point(311, 53);
            this.chkPlatform_ID17.Size     = new System.Drawing.Size(80, 17);
            this.chkPlatform_ID17.Text     = "ID17 (17H)";
            this.chkPlatform_ID17.CheckedChanged += new System.EventHandler(this.PlatformItem_CheckedChanged);

            this.chkPlatform_ID18.Location = new System.Drawing.Point(394, 53);
            this.chkPlatform_ID18.Size     = new System.Drawing.Size(80, 17);
            this.chkPlatform_ID18.Text     = "ID18 (18H)";
            this.chkPlatform_ID18.CheckedChanged += new System.EventHandler(this.PlatformItem_CheckedChanged);

            // ════════════════════════════════════════════════
            // SerialSettingControl 本身
            // ════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(730, 345);
            this.Controls.Add(this.grpSerial);
            this.Controls.Add(this.grpParam);
            this.Controls.Add(this.grpLobby);
            this.Controls.Add(this.grpPlatform);

            this.grpSerial.ResumeLayout(false);
            this.grpSerial.PerformLayout();
            this.grpParam.ResumeLayout(false);
            this.grpParam.PerformLayout();
            this.grpLobby.ResumeLayout(false);
            this.grpLobby.PerformLayout();
            this.grpPlatform.ResumeLayout(false);
            this.grpPlatform.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxRows)).EndInit();
            this.ResumeLayout(false);
        }

        // ── 串列埠設定 ──────────────────────────────────────
        private System.Windows.Forms.GroupBox      grpSerial;
        private System.Windows.Forms.Label         lblCOM;
        private System.Windows.Forms.ComboBox      cmbCOM;
        private System.Windows.Forms.Label         lblBaudRate;
        private System.Windows.Forms.ComboBox      cmbBaudRate;
        private System.Windows.Forms.Label         lblDatabit;
        private System.Windows.Forms.ComboBox      cmbDatabit;
        private System.Windows.Forms.Label         lblParity;
        private System.Windows.Forms.ComboBox      cmbParity;
        private System.Windows.Forms.Label         lblStopbit;
        private System.Windows.Forms.ComboBox      cmbStopbit;
        private System.Windows.Forms.Button        btnOpen;
        // ── 參數設定 ─────────────────────────────────────────
        private System.Windows.Forms.GroupBox      grpParam;
        private System.Windows.Forms.Label         lblDelay;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.Label         lblMaxRows;
        private System.Windows.Forms.NumericUpDown nudMaxRows;
        private System.Windows.Forms.Button        btnSave;
        // ── 大廳層點矩陣顯示器 ──────────────────────────────
        private System.Windows.Forms.GroupBox      grpLobby;
        private System.Windows.Forms.CheckBox      chkAllLobby;
        private System.Windows.Forms.Label         lblLobby;
        private System.Windows.Forms.CheckBox      chkLobby_ID1;
        private System.Windows.Forms.CheckBox      chkLobby_ID2;
        private System.Windows.Forms.CheckBox      chkLobby_ID3;
        private System.Windows.Forms.CheckBox      chkLobby_ID4;
        private System.Windows.Forms.Label         lblExchange;
        private System.Windows.Forms.CheckBox      chkLobby_ID5;
        private System.Windows.Forms.CheckBox      chkLobby_ID6;
        private System.Windows.Forms.CheckBox      chkLobby_ID7;
        // ── 月臺層點矩陣顯示器 ──────────────────────────────
        private System.Windows.Forms.GroupBox      grpPlatform;
        private System.Windows.Forms.CheckBox      chkAllPlatform;
        private System.Windows.Forms.Label         lblUpPlatform;
        private System.Windows.Forms.CheckBox      chkPlatform_ID11;
        private System.Windows.Forms.CheckBox      chkPlatform_ID12;
        private System.Windows.Forms.CheckBox      chkPlatform_ID13;
        private System.Windows.Forms.CheckBox      chkPlatform_ID14;
        private System.Windows.Forms.Label         lblDownPlatform;
        private System.Windows.Forms.CheckBox      chkPlatform_ID15;
        private System.Windows.Forms.CheckBox      chkPlatform_ID16;
        private System.Windows.Forms.CheckBox      chkPlatform_ID17;
        private System.Windows.Forms.CheckBox      chkPlatform_ID18;
    }
}
