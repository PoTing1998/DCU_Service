namespace UITest.Controls
{
    partial class DMDReceiveControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpConn       = new System.Windows.Forms.GroupBox();
            this.lblIP         = new System.Windows.Forms.Label();
            this.txtIP         = new System.Windows.Forms.TextBox();
            this.lblPort       = new System.Windows.Forms.Label();
            this.txtPort       = new System.Windows.Forms.TextBox();
            this.btnConnect    = new System.Windows.Forms.Button();
            this.lblConnStatus = new System.Windows.Forms.Label();
            this.chkRealMode   = new System.Windows.Forms.CheckBox();
            this.lblMode       = new System.Windows.Forms.Label();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.btnClear      = new System.Windows.Forms.Button();
            this.lblCount      = new System.Windows.Forms.Label();

            this.splitMain     = new System.Windows.Forms.SplitContainer();
            this.lvMessages    = new System.Windows.Forms.ListView();
            this.colTime       = new System.Windows.Forms.ColumnHeader();
            this.colMode       = new System.Windows.Forms.ColumnHeader();
            this.colType       = new System.Windows.Forms.ColumnHeader();
            this.colID         = new System.Windows.Forms.ColumnHeader();
            this.colLen        = new System.Windows.Forms.ColumnHeader();
            this.colObject     = new System.Windows.Forms.ColumnHeader();
            this.colHandler    = new System.Windows.Forms.ColumnHeader();
            this.colResult     = new System.Windows.Forms.ColumnHeader();

            this.splitDetail   = new System.Windows.Forms.SplitContainer();
            this.grpDetail     = new System.Windows.Forms.GroupBox();
            this.txtDetail     = new System.Windows.Forms.TextBox();
            this.grpActions    = new System.Windows.Forms.GroupBox();
            this.txtActions    = new System.Windows.Forms.TextBox();

            this.grpConn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDetail)).BeginInit();
            this.splitDetail.Panel1.SuspendLayout();
            this.splitDetail.Panel2.SuspendLayout();
            this.splitDetail.SuspendLayout();
            this.grpDetail.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.SuspendLayout();

            // ── grpConn：DMD Server 連線 / 處理模式 ────────────────────────
            this.grpConn.Text   = "DMD Server 連線（Socket Client，處理邏輯與 TaskDMD 共用 DMD_Service）";
            this.grpConn.Dock   = System.Windows.Forms.DockStyle.Top;
            this.grpConn.Height = 92;
            this.grpConn.Controls.Add(this.lblIP);
            this.grpConn.Controls.Add(this.txtIP);
            this.grpConn.Controls.Add(this.lblPort);
            this.grpConn.Controls.Add(this.txtPort);
            this.grpConn.Controls.Add(this.btnConnect);
            this.grpConn.Controls.Add(this.lblConnStatus);
            this.grpConn.Controls.Add(this.chkRealMode);
            this.grpConn.Controls.Add(this.lblMode);
            this.grpConn.Controls.Add(this.chkAutoScroll);
            this.grpConn.Controls.Add(this.btnClear);
            this.grpConn.Controls.Add(this.lblCount);

            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(16, 28);
            this.lblIP.Text     = "IP";

            this.txtIP.Location = new System.Drawing.Point(40, 24);
            this.txtIP.Size     = new System.Drawing.Size(130, 21);
            this.txtIP.Name     = "txtIP";

            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(186, 28);
            this.lblPort.Text     = "Port";

            this.txtPort.Location = new System.Drawing.Point(220, 24);
            this.txtPort.Size     = new System.Drawing.Size(70, 21);
            this.txtPort.Name     = "txtPort";

            this.btnConnect.Location = new System.Drawing.Point(306, 22);
            this.btnConnect.Size     = new System.Drawing.Size(70, 26);
            this.btnConnect.Text     = "連線";
            this.btnConnect.Name     = "btnConnect";
            this.btnConnect.Click   += new System.EventHandler(this.btnConnect_Click);

            this.lblConnStatus.AutoSize  = true;
            this.lblConnStatus.Location  = new System.Drawing.Point(392, 28);
            this.lblConnStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblConnStatus.Text      = "未連線";

            this.chkRealMode.AutoSize = true;
            this.chkRealMode.Location = new System.Drawing.Point(18, 60);
            this.chkRealMode.Text     = "真實模式";
            this.chkRealMode.Name     = "chkRealMode";
            this.chkRealMode.CheckedChanged += new System.EventHandler(this.chkRealMode_CheckedChanged);

            this.lblMode.AutoSize  = true;
            this.lblMode.Location  = new System.Drawing.Point(100, 62);
            this.lblMode.ForeColor = System.Drawing.Color.DimGray;
            this.lblMode.Text      = "目前：模擬模式（只顯示，不產生副作用）";

            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Checked  = true;
            this.chkAutoScroll.Location = new System.Drawing.Point(420, 60);
            this.chkAutoScroll.Text     = "自動捲動到最新";
            this.chkAutoScroll.Name     = "chkAutoScroll";

            this.btnClear.Location = new System.Drawing.Point(560, 56);
            this.btnClear.Size     = new System.Drawing.Size(70, 26);
            this.btnClear.Text     = "清除";
            this.btnClear.Name     = "btnClear";
            this.btnClear.Click   += new System.EventHandler(this.btnClear_Click);

            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(646, 62);
            this.lblCount.Text     = "共 0 筆";

            // ── splitMain：上＝訊息清單，下＝明細 ─────────────────────────
            this.splitMain.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitMain.Name        = "splitMain";
            this.splitMain.Size        = new System.Drawing.Size(1180, 570);
            this.splitMain.SplitterDistance = 300;
            this.splitMain.Panel1.Controls.Add(this.lvMessages);
            this.splitMain.Panel2.Controls.Add(this.splitDetail);

            this.lvMessages.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.lvMessages.View          = System.Windows.Forms.View.Details;
            this.lvMessages.FullRowSelect = true;
            this.lvMessages.GridLines     = true;
            this.lvMessages.HideSelection = false;
            this.lvMessages.MultiSelect   = false;
            this.lvMessages.Name          = "lvMessages";
            this.lvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colTime, this.colMode, this.colType, this.colID, this.colLen, this.colObject, this.colHandler, this.colResult });
            this.lvMessages.SelectedIndexChanged += new System.EventHandler(this.lvMessages_SelectedIndexChanged);

            this.colTime.Text    = "時間";      this.colTime.Width    = 90;
            this.colMode.Text    = "模式";      this.colMode.Width    = 45;
            this.colType.Text    = "訊息類別";  this.colType.Width    = 90;
            this.colID.Text      = "識別碼";    this.colID.Width      = 60;
            this.colLen.Text     = "長度";      this.colLen.Width     = 50;
            this.colObject.Text  = "JsonObject"; this.colObject.Width  = 190;
            this.colHandler.Text = "處理器";    this.colHandler.Width = 210;
            this.colResult.Text  = "結果";      this.colResult.Width  = 400;

            // ── splitDetail：左＝原始內容，右＝執行動作 ────────────────────
            this.splitDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDetail.Name = "splitDetail";
            this.splitDetail.Size = new System.Drawing.Size(1180, 266);
            this.splitDetail.SplitterDistance = 560;
            this.splitDetail.Panel1.Controls.Add(this.grpDetail);
            this.splitDetail.Panel2.Controls.Add(this.grpActions);

            this.grpDetail.Text = "訊息內容（HEX / JSON）";
            this.grpDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetail.Controls.Add(this.txtDetail);

            this.txtDetail.Dock       = System.Windows.Forms.DockStyle.Fill;
            this.txtDetail.Multiline  = true;
            this.txtDetail.ReadOnly   = true;
            this.txtDetail.WordWrap   = false;
            this.txtDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDetail.Font       = new System.Drawing.Font("Consolas", 9F);
            this.txtDetail.BackColor  = System.Drawing.Color.White;
            this.txtDetail.Name       = "txtDetail";

            this.grpActions.Text = "Service 邏輯執行的動作（DB / MSMQ / Ack）";
            this.grpActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpActions.Controls.Add(this.txtActions);

            this.txtActions.Dock       = System.Windows.Forms.DockStyle.Fill;
            this.txtActions.Multiline  = true;
            this.txtActions.ReadOnly   = true;
            this.txtActions.WordWrap   = false;
            this.txtActions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtActions.Font       = new System.Drawing.Font("Consolas", 9F);
            this.txtActions.BackColor  = System.Drawing.Color.White;
            this.txtActions.Name       = "txtActions";

            // ── DMDReceiveControl ─────────────────────────────────────────
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.grpConn);
            this.Name = "DMDReceiveControl";
            this.Size = new System.Drawing.Size(1180, 664);

            this.grpConn.ResumeLayout(false);
            this.grpConn.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitDetail.Panel1.ResumeLayout(false);
            this.splitDetail.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDetail)).EndInit();
            this.splitDetail.ResumeLayout(false);
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.grpActions.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox       grpConn;
        private System.Windows.Forms.Label          lblIP;
        private System.Windows.Forms.TextBox        txtIP;
        private System.Windows.Forms.Label          lblPort;
        private System.Windows.Forms.TextBox        txtPort;
        private System.Windows.Forms.Button         btnConnect;
        private System.Windows.Forms.Label          lblConnStatus;
        private System.Windows.Forms.CheckBox       chkRealMode;
        private System.Windows.Forms.Label          lblMode;
        private System.Windows.Forms.CheckBox       chkAutoScroll;
        private System.Windows.Forms.Button         btnClear;
        private System.Windows.Forms.Label          lblCount;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ListView       lvMessages;
        private System.Windows.Forms.ColumnHeader   colTime;
        private System.Windows.Forms.ColumnHeader   colMode;
        private System.Windows.Forms.ColumnHeader   colType;
        private System.Windows.Forms.ColumnHeader   colID;
        private System.Windows.Forms.ColumnHeader   colLen;
        private System.Windows.Forms.ColumnHeader   colObject;
        private System.Windows.Forms.ColumnHeader   colHandler;
        private System.Windows.Forms.ColumnHeader   colResult;
        private System.Windows.Forms.SplitContainer splitDetail;
        private System.Windows.Forms.GroupBox       grpDetail;
        private System.Windows.Forms.TextBox        txtDetail;
        private System.Windows.Forms.GroupBox       grpActions;
        private System.Windows.Forms.TextBox        txtActions;
    }
}
