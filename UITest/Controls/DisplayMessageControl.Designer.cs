namespace UITest.Controls
{
    partial class DisplayMessageControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── 預覽顯示板 ──
            this.pnlDisplay     = new System.Windows.Forms.Panel();
            this.lblDisplayHint = new System.Windows.Forms.Label();
            // ── Top buttons ──
            this.btnUpUpload    = new System.Windows.Forms.Button();
            this.btnAllUpload   = new System.Windows.Forms.Button();
            this.chkUpMulti     = new System.Windows.Forms.CheckBox();
            this.btnDnUpload    = new System.Windows.Forms.Button();
            this.btnSave        = new System.Windows.Forms.Button();
            this.chkDnMulti     = new System.Windows.Forms.CheckBox();
            // ── 上行 ──
            this.pnlUpMsgType      = new System.Windows.Forms.Panel();
            this.lblUpMsgType      = new System.Windows.Forms.Label();
            this.rdoUpGeneral      = new System.Windows.Forms.RadioButton();
            this.rdoUpPreRec       = new System.Windows.Forms.RadioButton();
            this.lblUpMsg          = new System.Windows.Forms.Label();
            this.txtUpMsg          = new System.Windows.Forms.TextBox();
            this.btnUpMsgBrowse    = new System.Windows.Forms.Button();
            this.btnUpMsgShow      = new System.Windows.Forms.Button();
            this.btnUpMsgAdd       = new System.Windows.Forms.Button();
            this.lstUpMultiMsgs    = new System.Windows.Forms.ListBox();
            this.btnUpMultiAdd     = new System.Windows.Forms.Button();
            this.btnUpMultiRemove  = new System.Windows.Forms.Button();
            this.lblUpFont      = new System.Windows.Forms.Label();
            this.cmbUpFontSize  = new System.Windows.Forms.ComboBox();
            this.cmbUpFontStyle = new System.Windows.Forms.ComboBox();
            this.pnlUpColor     = new System.Windows.Forms.Panel();
            this.cmbUpColor     = new System.Windows.Forms.ComboBox();
            this.cmbUpLevel     = new System.Windows.Forms.ComboBox();
            this.lblUpAction    = new System.Windows.Forms.Label();
            this.pnlUpAction    = new System.Windows.Forms.Panel();
            this.rdoUpAct61     = new System.Windows.Forms.RadioButton();
            this.rdoUpAct62     = new System.Windows.Forms.RadioButton();
            this.rdoUpAct63     = new System.Windows.Forms.RadioButton();
            this.rdoUpAct64     = new System.Windows.Forms.RadioButton();
            this.rdoUpAct65     = new System.Windows.Forms.RadioButton();
            this.rdoUpAct66     = new System.Windows.Forms.RadioButton();
            this.rdoUpAct67     = new System.Windows.Forms.RadioButton();
            this.lblUpParam     = new System.Windows.Forms.Label();
            this.lblUpSpeed     = new System.Windows.Forms.Label();
            this.nudUpSpeed     = new System.Windows.Forms.NumericUpDown();
            this.lblUpPause     = new System.Windows.Forms.Label();
            this.nudUpPause     = new System.Windows.Forms.NumericUpDown();
            this.lblUpPauseUnit = new System.Windows.Forms.Label();
            // ── 下行 ──
            this.pnlDnMsgType      = new System.Windows.Forms.Panel();
            this.lblDnMsgType      = new System.Windows.Forms.Label();
            this.rdoDnGeneral      = new System.Windows.Forms.RadioButton();
            this.rdoDnPreRec       = new System.Windows.Forms.RadioButton();
            this.lblDnMsg          = new System.Windows.Forms.Label();
            this.txtDnMsg          = new System.Windows.Forms.TextBox();
            this.btnDnMsgBrowse    = new System.Windows.Forms.Button();
            this.btnDnMsgShow      = new System.Windows.Forms.Button();
            this.btnDnMsgAdd       = new System.Windows.Forms.Button();
            this.lstDnMultiMsgs    = new System.Windows.Forms.ListBox();
            this.btnDnMultiAdd     = new System.Windows.Forms.Button();
            this.btnDnMultiRemove  = new System.Windows.Forms.Button();
            this.lblDnFont      = new System.Windows.Forms.Label();
            this.cmbDnFontSize  = new System.Windows.Forms.ComboBox();
            this.cmbDnFontStyle = new System.Windows.Forms.ComboBox();
            this.pnlDnColor     = new System.Windows.Forms.Panel();
            this.cmbDnColor     = new System.Windows.Forms.ComboBox();
            this.cmbDnLevel     = new System.Windows.Forms.ComboBox();
            this.lblDnAction    = new System.Windows.Forms.Label();
            this.pnlDnAction    = new System.Windows.Forms.Panel();
            this.rdoDnAct61     = new System.Windows.Forms.RadioButton();
            this.rdoDnAct62     = new System.Windows.Forms.RadioButton();
            this.rdoDnAct63     = new System.Windows.Forms.RadioButton();
            this.rdoDnAct64     = new System.Windows.Forms.RadioButton();
            this.rdoDnAct65     = new System.Windows.Forms.RadioButton();
            this.rdoDnAct66     = new System.Windows.Forms.RadioButton();
            this.rdoDnAct67     = new System.Windows.Forms.RadioButton();
            this.lblDnParam     = new System.Windows.Forms.Label();
            this.lblDnSpeed     = new System.Windows.Forms.Label();
            this.nudDnSpeed     = new System.Windows.Forms.NumericUpDown();
            this.lblDnPause     = new System.Windows.Forms.Label();
            this.nudDnPause     = new System.Windows.Forms.NumericUpDown();
            this.lblDnPauseUnit = new System.Windows.Forms.Label();
            // ── Board type GroupBoxes ──
            this.grpUpBoard  = new System.Windows.Forms.GroupBox();
            this.rdoUpBoard1 = new System.Windows.Forms.RadioButton();
            this.rdoUpBoard2 = new System.Windows.Forms.RadioButton();
            this.rdoUpBoard3 = new System.Windows.Forms.RadioButton();
            this.rdoUpBoard4 = new System.Windows.Forms.RadioButton();
            this.rdoUpBoard5 = new System.Windows.Forms.RadioButton();
            this.rdoUpBoard6 = new System.Windows.Forms.RadioButton();
            this.rdoUpBoard7 = new System.Windows.Forms.RadioButton();
            this.rdoUpBoard8 = new System.Windows.Forms.RadioButton();
            this.grpDnBoard  = new System.Windows.Forms.GroupBox();
            this.rdoDnBoard1 = new System.Windows.Forms.RadioButton();
            this.rdoDnBoard2 = new System.Windows.Forms.RadioButton();
            this.rdoDnBoard3 = new System.Windows.Forms.RadioButton();
            this.rdoDnBoard4 = new System.Windows.Forms.RadioButton();
            this.rdoDnBoard5 = new System.Windows.Forms.RadioButton();
            this.rdoDnBoard6 = new System.Windows.Forms.RadioButton();
            this.rdoDnBoard7 = new System.Windows.Forms.RadioButton();
            this.rdoDnBoard8 = new System.Windows.Forms.RadioButton();
            // ── 上行 Extra 子選項 ──
            this.lblUpTimeHdr        = new System.Windows.Forms.Label();
            this.cmbUpTimeType       = new System.Windows.Forms.ComboBox();
            this.pnlUpTimeClr        = new System.Windows.Forms.Panel();
            this.cmbUpTimeClr        = new System.Windows.Forms.ComboBox();
            this.lblUpCountStart     = new System.Windows.Forms.Label();
            this.nudUpCountStart     = new System.Windows.Forms.NumericUpDown();
            this.lblUpCountStartTime = new System.Windows.Forms.Label();
            this.lblUpCountStop      = new System.Windows.Forms.Label();
            this.nudUpCountStop      = new System.Windows.Forms.NumericUpDown();
            this.lblUpCountStopTime  = new System.Windows.Forms.Label();
            this.lblUpPlatHdr   = new System.Windows.Forms.Label();
            this.lblUpPlatIdx   = new System.Windows.Forms.Label();
            this.nudUpPlatIdx   = new System.Windows.Forms.NumericUpDown();
            this.pnlUpPlatThumb = new System.Windows.Forms.Panel();
            this.pnlUpPlatClr   = new System.Windows.Forms.Panel();
            this.cmbUpPlatClr   = new System.Windows.Forms.ComboBox();
            // ── 下行 Extra 子選項 ──
            this.lblDnTimeHdr        = new System.Windows.Forms.Label();
            this.cmbDnTimeType       = new System.Windows.Forms.ComboBox();
            this.pnlDnTimeClr        = new System.Windows.Forms.Panel();
            this.cmbDnTimeClr        = new System.Windows.Forms.ComboBox();
            this.lblDnCountStart     = new System.Windows.Forms.Label();
            this.nudDnCountStart     = new System.Windows.Forms.NumericUpDown();
            this.lblDnCountStartTime = new System.Windows.Forms.Label();
            this.lblDnCountStop      = new System.Windows.Forms.Label();
            this.nudDnCountStop      = new System.Windows.Forms.NumericUpDown();
            this.lblDnCountStopTime  = new System.Windows.Forms.Label();
            this.lblDnPlatHdr   = new System.Windows.Forms.Label();
            this.lblDnPlatIdx   = new System.Windows.Forms.Label();
            this.nudDnPlatIdx   = new System.Windows.Forms.NumericUpDown();
            this.pnlDnPlatThumb = new System.Windows.Forms.Panel();
            this.pnlDnPlatClr   = new System.Windows.Forms.Panel();
            this.cmbDnPlatClr   = new System.Windows.Forms.ComboBox();
            // ── 上行 警示燈 Extra 子選項（包在 pnlUpAlarm 內）──
            this.pnlUpAlarm       = new System.Windows.Forms.Panel();
            this.lblUpAlarmHdr    = new System.Windows.Forms.Label();
            this.lblUpAlarmMsgLbl = new System.Windows.Forms.Label();
            this.rdoUpAlarmMsgOn  = new System.Windows.Forms.RadioButton();
            this.rdoUpAlarmMsgOff = new System.Windows.Forms.RadioButton();
            this.lblUpAlarmPlay   = new System.Windows.Forms.Label();
            this.nudUpAlarmPlay   = new System.Windows.Forms.NumericUpDown();
            this.lblUpAlarmLight  = new System.Windows.Forms.Label();
            this.rdoUpLightOff    = new System.Windows.Forms.RadioButton();
            this.rdoUpLightOn     = new System.Windows.Forms.RadioButton();
            this.rdoUpLightBlink  = new System.Windows.Forms.RadioButton();
            // ── 下行 警示燈 Extra 子選項（包在 pnlDnAlarm 內）──
            this.pnlDnAlarm       = new System.Windows.Forms.Panel();
            this.lblDnAlarmHdr    = new System.Windows.Forms.Label();
            this.lblDnAlarmMsgLbl = new System.Windows.Forms.Label();
            this.rdoDnAlarmMsgOn  = new System.Windows.Forms.RadioButton();
            this.rdoDnAlarmMsgOff = new System.Windows.Forms.RadioButton();
            this.lblDnAlarmPlay   = new System.Windows.Forms.Label();
            this.nudDnAlarmPlay   = new System.Windows.Forms.NumericUpDown();
            this.lblDnAlarmLight  = new System.Windows.Forms.Label();
            this.rdoDnLightOff    = new System.Windows.Forms.RadioButton();
            this.rdoDnLightOn     = new System.Windows.Forms.RadioButton();
            this.rdoDnLightBlink  = new System.Windows.Forms.RadioButton();

            ((System.ComponentModel.ISupportInitialize)(this.nudUpSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpPlatIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnPlatIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpCountStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpCountStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnCountStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnCountStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpAlarmPlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnAlarmPlay)).BeginInit();
            this.grpUpBoard.SuspendLayout();
            this.grpDnBoard.SuspendLayout();
            this.SuspendLayout();

            // ════════════════════════════════════════════════
            // 預覽顯示板  (x=260, y=3, w=910, h=80)
            // ════════════════════════════════════════════════
            this.pnlDisplay.Location    = new System.Drawing.Point(260, 3);
            this.pnlDisplay.Size        = new System.Drawing.Size(910, 80);
            this.pnlDisplay.BackColor   = System.Drawing.Color.Black;
            this.pnlDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDisplay.Paint      += new System.Windows.Forms.PaintEventHandler(this.pnlDisplay_Paint);

            this.lblDisplayHint.AutoSize  = true;
            this.lblDisplayHint.Location  = new System.Drawing.Point(260, 87);
            this.lblDisplayHint.Text      = "↑ 按「顯示」預覽";
            this.lblDisplayHint.ForeColor = System.Drawing.Color.Gray;

            // ════════════════════════════════════════════════
            // Top buttons
            // ════════════════════════════════════════════════
            this.btnUpUpload.Location = new System.Drawing.Point(10, 5);
            this.btnUpUpload.Size     = new System.Drawing.Size(70, 23);
            this.btnUpUpload.Text     = "上行上傳";
            this.btnUpUpload.Click   += new System.EventHandler(this.btnUpUpload_Click);

            this.btnAllUpload.Location = new System.Drawing.Point(85, 5);
            this.btnAllUpload.Size     = new System.Drawing.Size(70, 23);
            this.btnAllUpload.Text     = "全部上傳";
            this.btnAllUpload.Click   += new System.EventHandler(this.btnAllUpload_Click);

            this.chkUpMulti.Location = new System.Drawing.Point(165, 8);
            this.chkUpMulti.AutoSize = true;
            this.chkUpMulti.Text     = "上行多訊息";

            this.btnDnUpload.Location = new System.Drawing.Point(10, 32);
            this.btnDnUpload.Size     = new System.Drawing.Size(70, 23);
            this.btnDnUpload.Text     = "下行上傳";
            this.btnDnUpload.Click   += new System.EventHandler(this.btnDnUpload_Click);

            this.btnSave.Location = new System.Drawing.Point(85, 32);
            this.btnSave.Size     = new System.Drawing.Size(70, 23);
            this.btnSave.Text     = "存檔";
            this.btnSave.Click   += new System.EventHandler(this.btnSave_Click);

            this.chkDnMulti.Location = new System.Drawing.Point(165, 35);
            this.chkDnMulti.AutoSize = true;
            this.chkDnMulti.Text     = "下行多訊息";

            // ════════════════════════════════════════════════
            // 上行 (左欄 x=10, y 從 224 開始)
            // ════════════════════════════════════════════════
            this.lblUpMsgType.AutoSize = true;
            this.lblUpMsgType.Location = new System.Drawing.Point(10, 224);
            this.lblUpMsgType.Text     = "上行訊息頻型";

            // pnlUpMsgType 包住兩個訊息頻型 RadioButton，避免與動作方式互斥
            this.pnlUpMsgType.Location = new System.Drawing.Point(10, 240);
            this.pnlUpMsgType.Size     = new System.Drawing.Size(290, 24);
            this.pnlUpMsgType.Controls.AddRange(new System.Windows.Forms.Control[]
                { this.rdoUpGeneral, this.rdoUpPreRec });

            this.rdoUpGeneral.AutoSize = true;
            this.rdoUpGeneral.Location = new System.Drawing.Point(0, 2);
            this.rdoUpGeneral.Text     = "一般訊息(2AH)";
            this.rdoUpGeneral.Checked  = true;

            this.rdoUpPreRec.AutoSize = true;
            this.rdoUpPreRec.Location = new System.Drawing.Point(125, 2);
            this.rdoUpPreRec.Text     = "預錄訊息(2CH)";

            this.lblUpMsg.AutoSize = true;
            this.lblUpMsg.Location = new System.Drawing.Point(10, 269);
            this.lblUpMsg.Text     = "上行一般訊息";
            this.lblUpMsg.Visible  = false;

            this.txtUpMsg.Location = new System.Drawing.Point(10, 287);
            this.txtUpMsg.Size     = new System.Drawing.Size(375, 21);
            this.txtUpMsg.Text     = "萬大線中英文abcdeABCDE123456";

            this.btnUpMsgBrowse.Location = new System.Drawing.Point(389, 286);
            this.btnUpMsgBrowse.Size     = new System.Drawing.Size(28, 23);
            this.btnUpMsgBrowse.Text     = "...";
            this.btnUpMsgBrowse.Click   += new System.EventHandler(this.btnUpMsgBrowse_Click);

            this.btnUpMsgShow.Location = new System.Drawing.Point(420, 286);
            this.btnUpMsgShow.Size     = new System.Drawing.Size(50, 23);
            this.btnUpMsgShow.Text     = "顯示";
            this.btnUpMsgShow.Click   += new System.EventHandler(this.btnUpMsgShow_Click);

            this.btnUpMsgAdd.Location = new System.Drawing.Point(473, 286);
            this.btnUpMsgAdd.Size     = new System.Drawing.Size(50, 23);
            this.btnUpMsgAdd.Text     = "新增";
            this.btnUpMsgAdd.Click   += new System.EventHandler(this.btnUpMsgAdd_Click);

            // ── 上行多訊息清單（預設隱藏）──
            this.lstUpMultiMsgs.Location  = new System.Drawing.Point(10, 287);
            this.lstUpMultiMsgs.Size      = new System.Drawing.Size(375, 82);
            this.lstUpMultiMsgs.Visible   = false;

            this.btnUpMultiAdd.Location = new System.Drawing.Point(389, 287);
            this.btnUpMultiAdd.Size     = new System.Drawing.Size(65, 23);
            this.btnUpMultiAdd.Text     = "新增訊息";
            this.btnUpMultiAdd.Visible  = false;
            this.btnUpMultiAdd.Click   += new System.EventHandler(this.btnUpMultiAdd_Click);

            this.btnUpMultiRemove.Location = new System.Drawing.Point(389, 316);
            this.btnUpMultiRemove.Size     = new System.Drawing.Size(65, 23);
            this.btnUpMultiRemove.Text     = "移除";
            this.btnUpMultiRemove.Visible  = false;
            this.btnUpMultiRemove.Click   += new System.EventHandler(this.btnUpMultiRemove_Click);

            this.lblUpFont.AutoSize = true;
            this.lblUpFont.Location = new System.Drawing.Point(10, 314);
            this.lblUpFont.Text     = "上行訊息(字型參數)";

            this.cmbUpFontSize.Location      = new System.Drawing.Point(10, 332);
            this.cmbUpFontSize.Size          = new System.Drawing.Size(65, 21);
            this.cmbUpFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpFontSize.Items.AddRange(new object[] { "24x24", "16x16", "英文 5x7" });
            this.cmbUpFontSize.SelectedIndex = 0;

            this.cmbUpFontStyle.Location      = new System.Drawing.Point(80, 332);
            this.cmbUpFontStyle.Size          = new System.Drawing.Size(65, 21);
            this.cmbUpFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpFontStyle.Items.AddRange(new object[] { "明體", "黑體", "楷體" });
            this.cmbUpFontStyle.SelectedIndex = 0;

            this.pnlUpColor.Location    = new System.Drawing.Point(150, 334);
            this.pnlUpColor.Size        = new System.Drawing.Size(18, 18);
            this.pnlUpColor.BackColor   = System.Drawing.Color.Yellow;
            this.pnlUpColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.cmbUpColor.Location      = new System.Drawing.Point(172, 332);
            this.cmbUpColor.Size          = new System.Drawing.Size(130, 21);
            this.cmbUpColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpColor.DrawMode      = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbUpColor.ItemHeight    = 20;
            // Items 和 SelectedIndex 在建構子裡設定（ColorComboItems 為動態屬性，設計工具不支援）
            this.cmbUpColor.DrawItem     += new System.Windows.Forms.DrawItemEventHandler(this.cmbColor_DrawItem);
            this.cmbUpColor.SelectedIndexChanged += new System.EventHandler(this.cmbColor_SelectedIndexChanged);

            this.cmbUpLevel.Location      = new System.Drawing.Point(310, 332);
            this.cmbUpLevel.Size          = new System.Drawing.Size(105, 21);
            this.cmbUpLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpLevel.Items.AddRange(new object[] { "最高Level 1", "高Level 2", "低Level 3", "最低Level 4" });
            this.cmbUpLevel.SelectedIndex = 3; // 預設最低Level 4

            this.lblUpAction.AutoSize = true;
            this.lblUpAction.Location = new System.Drawing.Point(10, 359);
            this.lblUpAction.Text     = "上行訊息動作方式";

            // pnlUpAction 包住所有動作方式 RadioButton
            this.pnlUpAction.Location = new System.Drawing.Point(10, 375);
            this.pnlUpAction.Size     = new System.Drawing.Size(440, 108);
            this.pnlUpAction.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.rdoUpAct61, this.rdoUpAct62, this.rdoUpAct63, this.rdoUpAct64,
                this.rdoUpAct65, this.rdoUpAct66, this.rdoUpAct67
            });

            this.rdoUpAct61.AutoSize = true;
            this.rdoUpAct61.Location = new System.Drawing.Point(0, 2);
            this.rdoUpAct61.Text     = "立即顯示(61H)";
            this.rdoUpAct61.Checked  = true;

            this.rdoUpAct62.AutoSize = true;
            this.rdoUpAct62.Location = new System.Drawing.Point(0, 28);
            this.rdoUpAct62.Text     = "向左捲動(靠右對齊)(62H)";

            this.rdoUpAct63.AutoSize = true;
            this.rdoUpAct63.Location = new System.Drawing.Point(0, 54);
            this.rdoUpAct63.Text     = "向左捲動(靠左對齊)(63H)";

            this.rdoUpAct64.AutoSize = true;
            this.rdoUpAct64.Location = new System.Drawing.Point(0, 80);
            this.rdoUpAct64.Text     = "向左捲動(左移消失)(64H)";

            this.rdoUpAct65.AutoSize = true;
            this.rdoUpAct65.Location = new System.Drawing.Point(210, 2);
            this.rdoUpAct65.Text     = "向下捲動(65H)";

            this.rdoUpAct66.AutoSize = true;
            this.rdoUpAct66.Location = new System.Drawing.Point(210, 28);
            this.rdoUpAct66.Text     = "向上捲動(66H)";

            this.rdoUpAct67.AutoSize = true;
            this.rdoUpAct67.Location = new System.Drawing.Point(210, 54);
            this.rdoUpAct67.Text     = "閃爍(67H)";

            this.lblUpParam.AutoSize = true;
            this.lblUpParam.Location = new System.Drawing.Point(10, 487);
            this.lblUpParam.Text     = "上行訊息動作參數";

            this.lblUpSpeed.AutoSize = true;
            this.lblUpSpeed.Location = new System.Drawing.Point(10, 507);
            this.lblUpSpeed.Text     = "速度";

            this.nudUpSpeed.Location = new System.Drawing.Point(45, 505);
            this.nudUpSpeed.Size     = new System.Drawing.Size(50, 20);
            this.nudUpSpeed.Minimum  = 0;
            this.nudUpSpeed.Maximum  = 9;
            this.nudUpSpeed.Value    = 5;

            this.lblUpPause.AutoSize = true;
            this.lblUpPause.Location = new System.Drawing.Point(105, 507);
            this.lblUpPause.Text     = "停留時間";

            this.nudUpPause.Location = new System.Drawing.Point(165, 505);
            this.nudUpPause.Size     = new System.Drawing.Size(50, 20);
            this.nudUpPause.Minimum  = 1;
            this.nudUpPause.Maximum  = 99;
            this.nudUpPause.Value    = 10;

            this.lblUpPauseUnit.AutoSize = true;
            this.lblUpPauseUnit.Location = new System.Drawing.Point(220, 507);
            this.lblUpPauseUnit.Text     = "(100ms)";

            // ════════════════════════════════════════════════
            // 下行 (右欄 x=595, y 從 224 開始)
            // ════════════════════════════════════════════════
            this.lblDnMsgType.AutoSize = true;
            this.lblDnMsgType.Location = new System.Drawing.Point(595, 224);
            this.lblDnMsgType.Text     = "下行訊息頻型";

            // pnlDnMsgType 包住兩個訊息頻型 RadioButton
            this.pnlDnMsgType.Location = new System.Drawing.Point(595, 240);
            this.pnlDnMsgType.Size     = new System.Drawing.Size(290, 24);
            this.pnlDnMsgType.Controls.AddRange(new System.Windows.Forms.Control[]
                { this.rdoDnGeneral, this.rdoDnPreRec });

            this.rdoDnGeneral.AutoSize = true;
            this.rdoDnGeneral.Location = new System.Drawing.Point(0, 2);
            this.rdoDnGeneral.Text     = "一般訊息(2AH)";
            this.rdoDnGeneral.Checked  = true;

            this.rdoDnPreRec.AutoSize = true;
            this.rdoDnPreRec.Location = new System.Drawing.Point(125, 2);
            this.rdoDnPreRec.Text     = "預錄訊息(2CH)";

            this.lblDnMsg.AutoSize = true;
            this.lblDnMsg.Location = new System.Drawing.Point(595, 269);
            this.lblDnMsg.Text     = "下行一般訊息";
            this.lblDnMsg.Visible  = false;

            this.txtDnMsg.Location = new System.Drawing.Point(595, 287);
            this.txtDnMsg.Size     = new System.Drawing.Size(375, 21);
            this.txtDnMsg.Text     = "萬大線中英文abcdeABCDE123456";

            this.btnDnMsgBrowse.Location = new System.Drawing.Point(974, 286);
            this.btnDnMsgBrowse.Size     = new System.Drawing.Size(28, 23);
            this.btnDnMsgBrowse.Text     = "...";
            this.btnDnMsgBrowse.Click   += new System.EventHandler(this.btnDnMsgBrowse_Click);

            this.btnDnMsgShow.Location = new System.Drawing.Point(1005, 286);
            this.btnDnMsgShow.Size     = new System.Drawing.Size(50, 23);
            this.btnDnMsgShow.Text     = "顯示";
            this.btnDnMsgShow.Click   += new System.EventHandler(this.btnDnMsgShow_Click);

            this.btnDnMsgAdd.Location = new System.Drawing.Point(1058, 286);
            this.btnDnMsgAdd.Size     = new System.Drawing.Size(50, 23);
            this.btnDnMsgAdd.Text     = "新增";
            this.btnDnMsgAdd.Click   += new System.EventHandler(this.btnDnMsgAdd_Click);

            // ── 下行多訊息清單（預設隱藏）──
            this.lstDnMultiMsgs.Location  = new System.Drawing.Point(595, 287);
            this.lstDnMultiMsgs.Size      = new System.Drawing.Size(375, 82);
            this.lstDnMultiMsgs.Visible   = false;

            this.btnDnMultiAdd.Location = new System.Drawing.Point(974, 287);
            this.btnDnMultiAdd.Size     = new System.Drawing.Size(65, 23);
            this.btnDnMultiAdd.Text     = "新增訊息";
            this.btnDnMultiAdd.Visible  = false;
            this.btnDnMultiAdd.Click   += new System.EventHandler(this.btnDnMultiAdd_Click);

            this.btnDnMultiRemove.Location = new System.Drawing.Point(974, 316);
            this.btnDnMultiRemove.Size     = new System.Drawing.Size(65, 23);
            this.btnDnMultiRemove.Text     = "移除";
            this.btnDnMultiRemove.Visible  = false;
            this.btnDnMultiRemove.Click   += new System.EventHandler(this.btnDnMultiRemove_Click);

            this.lblDnFont.AutoSize = true;
            this.lblDnFont.Location = new System.Drawing.Point(595, 314);
            this.lblDnFont.Text     = "下行訊息(字型參數)";

            this.cmbDnFontSize.Location      = new System.Drawing.Point(595, 332);
            this.cmbDnFontSize.Size          = new System.Drawing.Size(65, 21);
            this.cmbDnFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnFontSize.Items.AddRange(new object[] { "24x24", "16x16", "英文 5x7" });
            this.cmbDnFontSize.SelectedIndex = 0;

            this.cmbDnFontStyle.Location      = new System.Drawing.Point(665, 332);
            this.cmbDnFontStyle.Size          = new System.Drawing.Size(65, 21);
            this.cmbDnFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnFontStyle.Items.AddRange(new object[] { "明體", "黑體", "楷體" });
            this.cmbDnFontStyle.SelectedIndex = 0;

            this.pnlDnColor.Location    = new System.Drawing.Point(735, 334);
            this.pnlDnColor.Size        = new System.Drawing.Size(18, 18);
            this.pnlDnColor.BackColor   = System.Drawing.Color.Yellow;
            this.pnlDnColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.cmbDnColor.Location      = new System.Drawing.Point(757, 332);
            this.cmbDnColor.Size          = new System.Drawing.Size(130, 21);
            this.cmbDnColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnColor.DrawMode      = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDnColor.ItemHeight    = 20;
            // Items 和 SelectedIndex 在建構子裡設定
            this.cmbDnColor.DrawItem     += new System.Windows.Forms.DrawItemEventHandler(this.cmbColor_DrawItem);
            this.cmbDnColor.SelectedIndexChanged += new System.EventHandler(this.cmbColor_SelectedIndexChanged);

            this.cmbDnLevel.Location      = new System.Drawing.Point(895, 332);
            this.cmbDnLevel.Size          = new System.Drawing.Size(105, 21);
            this.cmbDnLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnLevel.Items.AddRange(new object[] { "最高Level 1", "高Level 2", "低Level 3", "最低Level 4" });
            this.cmbDnLevel.SelectedIndex = 3; // 預設最低Level 4

            this.lblDnAction.AutoSize = true;
            this.lblDnAction.Location = new System.Drawing.Point(595, 359);
            this.lblDnAction.Text     = "下行訊息動作方式";

            // pnlDnAction 包住所有動作方式 RadioButton
            this.pnlDnAction.Location = new System.Drawing.Point(595, 375);
            this.pnlDnAction.Size     = new System.Drawing.Size(440, 108);
            this.pnlDnAction.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.rdoDnAct61, this.rdoDnAct62, this.rdoDnAct63, this.rdoDnAct64,
                this.rdoDnAct65, this.rdoDnAct66, this.rdoDnAct67
            });

            this.rdoDnAct61.AutoSize = true;
            this.rdoDnAct61.Location = new System.Drawing.Point(0, 2);
            this.rdoDnAct61.Text     = "立即顯示(61H)";
            this.rdoDnAct61.Checked  = true;

            this.rdoDnAct62.AutoSize = true;
            this.rdoDnAct62.Location = new System.Drawing.Point(0, 28);
            this.rdoDnAct62.Text     = "向左捲動(靠右對齊)(62H)";

            this.rdoDnAct63.AutoSize = true;
            this.rdoDnAct63.Location = new System.Drawing.Point(0, 54);
            this.rdoDnAct63.Text     = "向左捲動(靠左對齊)(63H)";

            this.rdoDnAct64.AutoSize = true;
            this.rdoDnAct64.Location = new System.Drawing.Point(0, 80);
            this.rdoDnAct64.Text     = "向左捲動(左移消失)(64H)";

            this.rdoDnAct65.AutoSize = true;
            this.rdoDnAct65.Location = new System.Drawing.Point(210, 2);
            this.rdoDnAct65.Text     = "向下捲動(65H)";

            this.rdoDnAct66.AutoSize = true;
            this.rdoDnAct66.Location = new System.Drawing.Point(210, 28);
            this.rdoDnAct66.Text     = "向上捲動(66H)";

            this.rdoDnAct67.AutoSize = true;
            this.rdoDnAct67.Location = new System.Drawing.Point(210, 54);
            this.rdoDnAct67.Text     = "閃爍(67H)";

            this.lblDnParam.AutoSize = true;
            this.lblDnParam.Location = new System.Drawing.Point(595, 487);
            this.lblDnParam.Text     = "下行訊息動作參數";

            this.lblDnSpeed.AutoSize = true;
            this.lblDnSpeed.Location = new System.Drawing.Point(595, 507);
            this.lblDnSpeed.Text     = "速度";

            this.nudDnSpeed.Location = new System.Drawing.Point(630, 505);
            this.nudDnSpeed.Size     = new System.Drawing.Size(50, 20);
            this.nudDnSpeed.Minimum  = 0;
            this.nudDnSpeed.Maximum  = 9;
            this.nudDnSpeed.Value    = 5;

            this.lblDnPause.AutoSize = true;
            this.lblDnPause.Location = new System.Drawing.Point(690, 507);
            this.lblDnPause.Text     = "停留時間";

            this.nudDnPause.Location = new System.Drawing.Point(750, 505);
            this.nudDnPause.Size     = new System.Drawing.Size(50, 20);
            this.nudDnPause.Minimum  = 1;
            this.nudDnPause.Maximum  = 99;
            this.nudDnPause.Value    = 8;

            this.lblDnPauseUnit.AutoSize = true;
            this.lblDnPauseUnit.Location = new System.Drawing.Point(805, 507);
            this.lblDnPauseUnit.Text     = "(100ms)";

            // ════════════════════════════════════════════════
            // 上行顯示器板型  (x=10, y=543, w=575, h=228)
            // ════════════════════════════════════════════════
            this.grpUpBoard.Location = new System.Drawing.Point(10, 543);
            this.grpUpBoard.Size     = new System.Drawing.Size(575, 228);
            this.grpUpBoard.Text     = "上行顯示器板型";
            this.grpUpBoard.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.rdoUpBoard1, this.rdoUpBoard2, this.rdoUpBoard3, this.rdoUpBoard4,
                this.rdoUpBoard5, this.rdoUpBoard6, this.rdoUpBoard7, this.rdoUpBoard8
            });

            this.rdoUpBoard1.AutoSize = true;
            this.rdoUpBoard1.Location = new System.Drawing.Point(8, 20);
            this.rdoUpBoard1.Text     = "1. 訊息顯示(71H)（全畫面）";
            this.rdoUpBoard1.Checked  = true;
            this.rdoUpBoard1.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard2.AutoSize = true;
            this.rdoUpBoard2.Location = new System.Drawing.Point(8, 46);
            this.rdoUpBoard2.Text     = "2. 訊息顯示(74H) + 右側（標準或列車到站倒數）時間顯示";
            this.rdoUpBoard2.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard3.AutoSize = true;
            this.rdoUpBoard3.Location = new System.Drawing.Point(8, 72);
            this.rdoUpBoard3.Text     = "3. 左側月台碼 + 訊息顯示(73H) + 右側時間顯示";
            this.rdoUpBoard3.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard4.AutoSize = true;
            this.rdoUpBoard4.Location = new System.Drawing.Point(8, 98);
            this.rdoUpBoard4.Text     = "4. 訊息顯示(74H) + 右側48x48頁比時鐘顯示(82H)";
            this.rdoUpBoard4.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard5.AutoSize = true;
            this.rdoUpBoard5.Location = new System.Drawing.Point(8, 124);
            this.rdoUpBoard5.Text     = "5. 左側月台碼 + 訊息顯示(72H)";
            this.rdoUpBoard5.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard6.AutoSize = true;
            this.rdoUpBoard6.Location = new System.Drawing.Point(8, 150);
            this.rdoUpBoard6.Text     = "6. 緊急訊息(71H) + 警示燈  設定";
            this.rdoUpBoard6.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard7.AutoSize = true;
            this.rdoUpBoard7.Location = new System.Drawing.Point(8, 176);
            this.rdoUpBoard7.Text     = "7. 路線代碼(7DH) + 到站訊息(74H) + 右側（時間倒數）";
            this.rdoUpBoard7.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard8.AutoSize = true;
            this.rdoUpBoard8.Location = new System.Drawing.Point(8, 202);
            this.rdoUpBoard8.Text     = "8. 站與站之間，連續圖片動態顯示模式（83H）";
            this.rdoUpBoard8.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            // ════════════════════════════════════════════════
            // 下行顯示器板型  (x=595, y=543, w=575, h=228)
            // ════════════════════════════════════════════════
            this.grpDnBoard.Location = new System.Drawing.Point(595, 543);
            this.grpDnBoard.Size     = new System.Drawing.Size(575, 228);
            this.grpDnBoard.Text     = "下行顯示器板型";
            this.grpDnBoard.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.rdoDnBoard1, this.rdoDnBoard2, this.rdoDnBoard3, this.rdoDnBoard4,
                this.rdoDnBoard5, this.rdoDnBoard6, this.rdoDnBoard7, this.rdoDnBoard8
            });

            this.rdoDnBoard1.AutoSize = true;
            this.rdoDnBoard1.Location = new System.Drawing.Point(8, 20);
            this.rdoDnBoard1.Text     = "1. 訊息顯示(71H)（全畫面）";
            this.rdoDnBoard1.Checked  = true;
            this.rdoDnBoard1.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard2.AutoSize = true;
            this.rdoDnBoard2.Location = new System.Drawing.Point(8, 46);
            this.rdoDnBoard2.Text     = "2. 訊息顯示(74H) + 右側（標準或列車到站倒數）時間顯示";
            this.rdoDnBoard2.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard3.AutoSize = true;
            this.rdoDnBoard3.Location = new System.Drawing.Point(8, 72);
            this.rdoDnBoard3.Text     = "3. 左側月台碼 + 訊息顯示(73H) + 右側時間顯示";
            this.rdoDnBoard3.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard4.AutoSize = true;
            this.rdoDnBoard4.Location = new System.Drawing.Point(8, 98);
            this.rdoDnBoard4.Text     = "4. 訊息顯示(74H) + 右側48x48頁比時鐘顯示(82H)";
            this.rdoDnBoard4.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard5.AutoSize = true;
            this.rdoDnBoard5.Location = new System.Drawing.Point(8, 124);
            this.rdoDnBoard5.Text     = "5. 左側月台碼 + 訊息顯示(72H)";
            this.rdoDnBoard5.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard6.AutoSize = true;
            this.rdoDnBoard6.Location = new System.Drawing.Point(8, 150);
            this.rdoDnBoard6.Text     = "6. 緊急訊息(71H) + 警示燈  設定";
            this.rdoDnBoard6.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard7.AutoSize = true;
            this.rdoDnBoard7.Location = new System.Drawing.Point(8, 176);
            this.rdoDnBoard7.Text     = "7. 左側標準進時碼(7EH) + 到站訊息(74H) + 右側(時間倒數)";
            this.rdoDnBoard7.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard8.AutoSize = true;
            this.rdoDnBoard8.Location = new System.Drawing.Point(8, 202);
            this.rdoDnBoard8.Text     = "8. 站與站之間，連續圖片動態顯示模式（83H）";
            this.rdoDnBoard8.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            // ════════════════════════════════════════════════
            // 上行 Extra
            // 版型 GroupBox 已移至底部，Extra 從 y=106 開始
            // 時間列: header=106  controls=124
            // 月台列: header=158  controls=174
            // ════════════════════════════════════════════════
            this.lblUpTimeHdr.AutoSize = true;
            this.lblUpTimeHdr.Location = new System.Drawing.Point(10, 106);
            this.lblUpTimeHdr.Text     = "上行時間(右側顯示)";
            this.lblUpTimeHdr.Visible  = false;

            this.cmbUpTimeType.Location      = new System.Drawing.Point(10, 124);
            this.cmbUpTimeType.Size          = new System.Drawing.Size(130, 21);
            this.cmbUpTimeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpTimeType.Items.AddRange(new object[] { "標準時間", "開始倒數" });
            this.cmbUpTimeType.SelectedIndex = 0;
            this.cmbUpTimeType.Visible       = false;
            this.cmbUpTimeType.SelectedIndexChanged += new System.EventHandler(this.cmbUpTimeType_SelectedIndexChanged);

            // 顏色選擇器移至第二列 y=148
            this.pnlUpTimeClr.Location    = new System.Drawing.Point(10, 150);
            this.pnlUpTimeClr.Size        = new System.Drawing.Size(18, 18);
            this.pnlUpTimeClr.BackColor   = System.Drawing.Color.Yellow;
            this.pnlUpTimeClr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpTimeClr.Visible     = false;

            this.cmbUpTimeClr.Location      = new System.Drawing.Point(32, 148);
            this.cmbUpTimeClr.Size          = new System.Drawing.Size(110, 21);
            this.cmbUpTimeClr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpTimeClr.DrawMode      = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbUpTimeClr.ItemHeight    = 20;
            // Items 和 SelectedIndex 在建構子裡設定
            this.cmbUpTimeClr.DrawItem     += new System.Windows.Forms.DrawItemEventHandler(this.cmbColor_DrawItem);
            this.cmbUpTimeClr.SelectedIndexChanged += new System.EventHandler(this.cmbColor_SelectedIndexChanged);
            this.cmbUpTimeClr.Visible       = false;

            // 倒數控件（只在「開始倒數」時顯示）
            this.lblUpCountStart.AutoSize = true;
            this.lblUpCountStart.Location = new System.Drawing.Point(150, 127);
            this.lblUpCountStart.Text     = "開始倒數";
            this.lblUpCountStart.Visible  = false;

            this.nudUpCountStart.Location = new System.Drawing.Point(215, 124);
            this.nudUpCountStart.Size     = new System.Drawing.Size(55, 20);
            this.nudUpCountStart.Minimum  = 0;
            this.nudUpCountStart.Maximum  = 255;
            this.nudUpCountStart.Value    = 12;
            this.nudUpCountStart.Visible  = false;
            this.nudUpCountStart.ValueChanged += new System.EventHandler(this.nudUpCountStart_ValueChanged);

            this.lblUpCountStartTime.AutoSize = true;
            this.lblUpCountStartTime.Location = new System.Drawing.Point(275, 127);
            this.lblUpCountStartTime.Text     = "(01:00)";
            this.lblUpCountStartTime.Visible  = false;

            this.lblUpCountStop.AutoSize = true;
            this.lblUpCountStop.Location = new System.Drawing.Point(170, 151);
            this.lblUpCountStop.Text     = "停止倒數";
            this.lblUpCountStop.Visible  = false;

            this.nudUpCountStop.Location = new System.Drawing.Point(240, 148);
            this.nudUpCountStop.Size     = new System.Drawing.Size(55, 20);
            this.nudUpCountStop.Minimum  = 0;
            this.nudUpCountStop.Maximum  = 255;
            this.nudUpCountStop.Value    = 0;
            this.nudUpCountStop.Visible  = false;
            this.nudUpCountStop.ValueChanged += new System.EventHandler(this.nudUpCountStop_ValueChanged);

            this.lblUpCountStopTime.AutoSize = true;
            this.lblUpCountStopTime.Location = new System.Drawing.Point(305, 151);
            this.lblUpCountStopTime.Text     = "(00:00)";
            this.lblUpCountStopTime.Visible  = false;

            this.lblUpPlatHdr.AutoSize = true;
            this.lblUpPlatHdr.Location = new System.Drawing.Point(10, 180);
            this.lblUpPlatHdr.Text     = "上行月台碼設定";
            this.lblUpPlatHdr.Visible  = false;

            this.lblUpPlatIdx.AutoSize = true;
            this.lblUpPlatIdx.Location = new System.Drawing.Point(10, 201);
            this.lblUpPlatIdx.Text     = "圖檔索引值";
            this.lblUpPlatIdx.Visible  = false;

            this.nudUpPlatIdx.Location = new System.Drawing.Point(80, 198);
            this.nudUpPlatIdx.Size     = new System.Drawing.Size(50, 20);
            this.nudUpPlatIdx.Minimum  = 0;
            this.nudUpPlatIdx.Maximum  = 99;
            this.nudUpPlatIdx.Value    = 1;
            this.nudUpPlatIdx.Visible  = false;

            this.pnlUpPlatThumb.Location    = new System.Drawing.Point(140, 193);
            this.pnlUpPlatThumb.Size        = new System.Drawing.Size(30, 30);
            this.pnlUpPlatThumb.BackColor   = System.Drawing.Color.DimGray;
            this.pnlUpPlatThumb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpPlatThumb.Visible     = false;

            this.pnlUpPlatClr.Location    = new System.Drawing.Point(180, 198);
            this.pnlUpPlatClr.Size        = new System.Drawing.Size(18, 18);
            this.pnlUpPlatClr.BackColor   = System.Drawing.Color.Yellow;
            this.pnlUpPlatClr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpPlatClr.Visible     = false;

            this.cmbUpPlatClr.Location      = new System.Drawing.Point(202, 198);
            this.cmbUpPlatClr.Size          = new System.Drawing.Size(130, 21);
            this.cmbUpPlatClr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpPlatClr.DrawMode      = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbUpPlatClr.ItemHeight    = 20;
            // Items 和 SelectedIndex 在建構子裡設定
            this.cmbUpPlatClr.DrawItem     += new System.Windows.Forms.DrawItemEventHandler(this.cmbColor_DrawItem);
            this.cmbUpPlatClr.SelectedIndexChanged += new System.EventHandler(this.cmbColor_SelectedIndexChanged);
            this.cmbUpPlatClr.Visible       = false;

            // ════════════════════════════════════════════════
            // 下行 Extra（與上行同 y，x 偏移 595）
            // ════════════════════════════════════════════════
            this.lblDnTimeHdr.AutoSize = true;
            this.lblDnTimeHdr.Location = new System.Drawing.Point(595, 106);
            this.lblDnTimeHdr.Text     = "下行時間(右側顯示)";
            this.lblDnTimeHdr.Visible  = false;

            this.cmbDnTimeType.Location      = new System.Drawing.Point(595, 124);
            this.cmbDnTimeType.Size          = new System.Drawing.Size(130, 21);
            this.cmbDnTimeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnTimeType.Items.AddRange(new object[] { "標準時間", "開始倒數" });
            this.cmbDnTimeType.SelectedIndex = 0;
            this.cmbDnTimeType.Visible       = false;
            this.cmbDnTimeType.SelectedIndexChanged += new System.EventHandler(this.cmbDnTimeType_SelectedIndexChanged);

            // 顏色選擇器移至第二列 y=148
            this.pnlDnTimeClr.Location    = new System.Drawing.Point(595, 150);
            this.pnlDnTimeClr.Size        = new System.Drawing.Size(18, 18);
            this.pnlDnTimeClr.BackColor   = System.Drawing.Color.Yellow;
            this.pnlDnTimeClr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDnTimeClr.Visible     = false;

            this.cmbDnTimeClr.Location      = new System.Drawing.Point(617, 148);
            this.cmbDnTimeClr.Size          = new System.Drawing.Size(110, 21);
            this.cmbDnTimeClr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnTimeClr.DrawMode      = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDnTimeClr.ItemHeight    = 20;
            // Items 和 SelectedIndex 在建構子裡設定
            this.cmbDnTimeClr.DrawItem     += new System.Windows.Forms.DrawItemEventHandler(this.cmbColor_DrawItem);
            this.cmbDnTimeClr.SelectedIndexChanged += new System.EventHandler(this.cmbColor_SelectedIndexChanged);
            this.cmbDnTimeClr.Visible       = false;

            // 倒數控件（只在「開始倒數」時顯示）
            this.lblDnCountStart.AutoSize = true;
            this.lblDnCountStart.Location = new System.Drawing.Point(735, 127);
            this.lblDnCountStart.Text     = "開始倒數";
            this.lblDnCountStart.Visible  = false;

            this.nudDnCountStart.Location = new System.Drawing.Point(800, 124);
            this.nudDnCountStart.Size     = new System.Drawing.Size(55, 20);
            this.nudDnCountStart.Minimum  = 0;
            this.nudDnCountStart.Maximum  = 255;
            this.nudDnCountStart.Value    = 12;
            this.nudDnCountStart.Visible  = false;
            this.nudDnCountStart.ValueChanged += new System.EventHandler(this.nudDnCountStart_ValueChanged);

            this.lblDnCountStartTime.AutoSize = true;
            this.lblDnCountStartTime.Location = new System.Drawing.Point(860, 127);
            this.lblDnCountStartTime.Text     = "(01:00)";
            this.lblDnCountStartTime.Visible  = false;

            this.lblDnCountStop.AutoSize = true;
            this.lblDnCountStop.Location = new System.Drawing.Point(755, 151);
            this.lblDnCountStop.Text     = "停止倒數";
            this.lblDnCountStop.Visible  = false;

            this.nudDnCountStop.Location = new System.Drawing.Point(825, 148);
            this.nudDnCountStop.Size     = new System.Drawing.Size(55, 20);
            this.nudDnCountStop.Minimum  = 0;
            this.nudDnCountStop.Maximum  = 255;
            this.nudDnCountStop.Value    = 0;
            this.nudDnCountStop.Visible  = false;
            this.nudDnCountStop.ValueChanged += new System.EventHandler(this.nudDnCountStop_ValueChanged);

            this.lblDnCountStopTime.AutoSize = true;
            this.lblDnCountStopTime.Location = new System.Drawing.Point(890, 151);
            this.lblDnCountStopTime.Text     = "(00:00)";
            this.lblDnCountStopTime.Visible  = false;

            this.lblDnPlatHdr.AutoSize = true;
            this.lblDnPlatHdr.Location = new System.Drawing.Point(595, 180);
            this.lblDnPlatHdr.Text     = "下行月台碼設定";
            this.lblDnPlatHdr.Visible  = false;

            this.lblDnPlatIdx.AutoSize = true;
            this.lblDnPlatIdx.Location = new System.Drawing.Point(595, 201);
            this.lblDnPlatIdx.Text     = "圖檔索引值";
            this.lblDnPlatIdx.Visible  = false;

            this.nudDnPlatIdx.Location = new System.Drawing.Point(665, 198);
            this.nudDnPlatIdx.Size     = new System.Drawing.Size(50, 20);
            this.nudDnPlatIdx.Minimum  = 0;
            this.nudDnPlatIdx.Maximum  = 99;
            this.nudDnPlatIdx.Value    = 1;
            this.nudDnPlatIdx.Visible  = false;

            this.pnlDnPlatThumb.Location    = new System.Drawing.Point(725, 193);
            this.pnlDnPlatThumb.Size        = new System.Drawing.Size(30, 30);
            this.pnlDnPlatThumb.BackColor   = System.Drawing.Color.DimGray;
            this.pnlDnPlatThumb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDnPlatThumb.Visible     = false;

            this.pnlDnPlatClr.Location    = new System.Drawing.Point(765, 198);
            this.pnlDnPlatClr.Size        = new System.Drawing.Size(18, 18);
            this.pnlDnPlatClr.BackColor   = System.Drawing.Color.Yellow;
            this.pnlDnPlatClr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDnPlatClr.Visible     = false;

            this.cmbDnPlatClr.Location      = new System.Drawing.Point(787, 198);
            this.cmbDnPlatClr.Size          = new System.Drawing.Size(130, 21);
            this.cmbDnPlatClr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnPlatClr.DrawMode      = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDnPlatClr.ItemHeight    = 20;
            // Items 和 SelectedIndex 在建構子裡設定
            this.cmbDnPlatClr.DrawItem     += new System.Windows.Forms.DrawItemEventHandler(this.cmbColor_DrawItem);
            this.cmbDnPlatClr.SelectedIndexChanged += new System.EventHandler(this.cmbColor_SelectedIndexChanged);
            this.cmbDnPlatClr.Visible       = false;

            // ════════════════════════════════════════════════
            // 上行 警示燈 Extra ── 全部放在 pnlUpAlarm 內
            // Panel 有實體背景色，不用透明，子控件保證顯示
            // ════════════════════════════════════════════════
            // --- 子控件（相對座標）---
            this.lblUpAlarmHdr.AutoSize = true;
            this.lblUpAlarmHdr.Font     = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold);
            this.lblUpAlarmHdr.Location = new System.Drawing.Point(0, 0);
            this.lblUpAlarmHdr.Text     = "緊急訊息設定";

            this.lblUpAlarmMsgLbl.AutoSize = true;
            this.lblUpAlarmMsgLbl.Location = new System.Drawing.Point(0, 22);
            this.lblUpAlarmMsgLbl.Text     = "緊急訊息：";

            this.rdoUpAlarmMsgOn.AutoSize  = true;
            this.rdoUpAlarmMsgOn.AutoCheck = false;
            this.rdoUpAlarmMsgOn.Location  = new System.Drawing.Point(70, 20);
            this.rdoUpAlarmMsgOn.Text      = "打開";
            this.rdoUpAlarmMsgOn.Checked   = true;
            this.rdoUpAlarmMsgOn.Click    += new System.EventHandler(this.rdoUpAlarmMsg_Click);

            this.rdoUpAlarmMsgOff.AutoSize  = true;
            this.rdoUpAlarmMsgOff.AutoCheck = false;
            this.rdoUpAlarmMsgOff.Location  = new System.Drawing.Point(125, 20);
            this.rdoUpAlarmMsgOff.Text      = "關閉";
            this.rdoUpAlarmMsgOff.Click    += new System.EventHandler(this.rdoUpAlarmMsg_Click);

            this.lblUpAlarmPlay.AutoSize = true;
            this.lblUpAlarmPlay.Location = new System.Drawing.Point(0, 46);
            this.lblUpAlarmPlay.Text     = "播放次數：";

            this.nudUpAlarmPlay.Location = new System.Drawing.Point(70, 43);
            this.nudUpAlarmPlay.Size     = new System.Drawing.Size(55, 20);
            this.nudUpAlarmPlay.Minimum  = 1;
            this.nudUpAlarmPlay.Maximum  = 255;
            this.nudUpAlarmPlay.Value    = 1;

            this.lblUpAlarmLight.AutoSize = true;
            this.lblUpAlarmLight.Font     = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold);
            this.lblUpAlarmLight.Location = new System.Drawing.Point(0, 70);
            this.lblUpAlarmLight.Text     = "警示燈開關設定";

            this.rdoUpLightOff.AutoSize  = true;
            this.rdoUpLightOff.AutoCheck = false;
            this.rdoUpLightOff.Location  = new System.Drawing.Point(0, 90);
            this.rdoUpLightOff.Text      = "關閉";
            this.rdoUpLightOff.Click    += new System.EventHandler(this.rdoUpLight_Click);

            this.rdoUpLightOn.AutoSize  = true;
            this.rdoUpLightOn.AutoCheck = false;
            this.rdoUpLightOn.Location  = new System.Drawing.Point(55, 90);
            this.rdoUpLightOn.Text      = "打開";
            this.rdoUpLightOn.Checked   = true;
            this.rdoUpLightOn.Click    += new System.EventHandler(this.rdoUpLight_Click);

            this.rdoUpLightBlink.AutoSize  = true;
            this.rdoUpLightBlink.AutoCheck = false;
            this.rdoUpLightBlink.Location  = new System.Drawing.Point(110, 90);
            this.rdoUpLightBlink.Text      = "閃爍";
            this.rdoUpLightBlink.Click    += new System.EventHandler(this.rdoUpLight_Click);

            // --- Panel 本身（y=5，與上傳按鈕同高；SetUpAlarmVisible 裡面同步隱藏/顯示被蓋住的按鈕）---
            this.pnlUpAlarm.Location  = new System.Drawing.Point(10, 5);
            this.pnlUpAlarm.Size      = new System.Drawing.Size(245, 118);
            this.pnlUpAlarm.BackColor = System.Drawing.SystemColors.Control;
            this.pnlUpAlarm.Visible  = false;
            this.pnlUpAlarm.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblUpAlarmHdr, this.lblUpAlarmMsgLbl,
                this.rdoUpAlarmMsgOn, this.rdoUpAlarmMsgOff,
                this.lblUpAlarmPlay, this.nudUpAlarmPlay,
                this.lblUpAlarmLight,
                this.rdoUpLightOff, this.rdoUpLightOn, this.rdoUpLightBlink
            });

            // ════════════════════════════════════════════════
            // 下行 警示燈 Extra ── 全部放在 pnlDnAlarm 內（x=595）
            // ════════════════════════════════════════════════
            this.lblDnAlarmHdr.AutoSize = true;
            this.lblDnAlarmHdr.Font     = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold);
            this.lblDnAlarmHdr.Location = new System.Drawing.Point(0, 0);
            this.lblDnAlarmHdr.Text     = "緊急訊息設定";

            this.lblDnAlarmMsgLbl.AutoSize = true;
            this.lblDnAlarmMsgLbl.Location = new System.Drawing.Point(0, 22);
            this.lblDnAlarmMsgLbl.Text     = "緊急訊息：";

            this.rdoDnAlarmMsgOn.AutoSize  = true;
            this.rdoDnAlarmMsgOn.AutoCheck = false;
            this.rdoDnAlarmMsgOn.Location  = new System.Drawing.Point(70, 20);
            this.rdoDnAlarmMsgOn.Text      = "打開";
            this.rdoDnAlarmMsgOn.Checked   = true;
            this.rdoDnAlarmMsgOn.Click    += new System.EventHandler(this.rdoDnAlarmMsg_Click);

            this.rdoDnAlarmMsgOff.AutoSize  = true;
            this.rdoDnAlarmMsgOff.AutoCheck = false;
            this.rdoDnAlarmMsgOff.Location  = new System.Drawing.Point(125, 20);
            this.rdoDnAlarmMsgOff.Text      = "關閉";
            this.rdoDnAlarmMsgOff.Click    += new System.EventHandler(this.rdoDnAlarmMsg_Click);

            this.lblDnAlarmPlay.AutoSize = true;
            this.lblDnAlarmPlay.Location = new System.Drawing.Point(0, 46);
            this.lblDnAlarmPlay.Text     = "播放次數：";

            this.nudDnAlarmPlay.Location = new System.Drawing.Point(70, 43);
            this.nudDnAlarmPlay.Size     = new System.Drawing.Size(55, 20);
            this.nudDnAlarmPlay.Minimum  = 1;
            this.nudDnAlarmPlay.Maximum  = 255;
            this.nudDnAlarmPlay.Value    = 1;

            this.lblDnAlarmLight.AutoSize = true;
            this.lblDnAlarmLight.Font     = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold);
            this.lblDnAlarmLight.Location = new System.Drawing.Point(0, 70);
            this.lblDnAlarmLight.Text     = "警示燈開關設定";

            this.rdoDnLightOff.AutoSize  = true;
            this.rdoDnLightOff.AutoCheck = false;
            this.rdoDnLightOff.Location  = new System.Drawing.Point(0, 90);
            this.rdoDnLightOff.Text      = "關閉";
            this.rdoDnLightOff.Click    += new System.EventHandler(this.rdoDnLight_Click);

            this.rdoDnLightOn.AutoSize  = true;
            this.rdoDnLightOn.AutoCheck = false;
            this.rdoDnLightOn.Location  = new System.Drawing.Point(55, 90);
            this.rdoDnLightOn.Text      = "打開";
            this.rdoDnLightOn.Checked   = true;
            this.rdoDnLightOn.Click    += new System.EventHandler(this.rdoDnLight_Click);

            this.rdoDnLightBlink.AutoSize  = true;
            this.rdoDnLightBlink.AutoCheck = false;
            this.rdoDnLightBlink.Location  = new System.Drawing.Point(110, 90);
            this.rdoDnLightBlink.Text      = "閃爍";
            this.rdoDnLightBlink.Click    += new System.EventHandler(this.rdoDnLight_Click);

            // --- Panel 本身（y=5，與 pnlUpAlarm 對齊，寬度245）---
            this.pnlDnAlarm.Location  = new System.Drawing.Point(595, 5);
            this.pnlDnAlarm.Size      = new System.Drawing.Size(245, 118);
            this.pnlDnAlarm.BackColor = System.Drawing.SystemColors.Control;
            this.pnlDnAlarm.Visible  = false;
            this.pnlDnAlarm.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblDnAlarmHdr, this.lblDnAlarmMsgLbl,
                this.rdoDnAlarmMsgOn, this.rdoDnAlarmMsgOff,
                this.lblDnAlarmPlay, this.nudDnAlarmPlay,
                this.lblDnAlarmLight,
                this.rdoDnLightOff, this.rdoDnLightOn, this.rdoDnLightBlink
            });

            // ════════════════════════════════════════════════
            // UserControl 本身
            // ════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll          = true;
            this.Size                = new System.Drawing.Size(1185, 790);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.pnlDisplay, this.lblDisplayHint,
                this.btnUpUpload, this.btnAllUpload, this.chkUpMulti,
                this.btnDnUpload, this.btnSave,      this.chkDnMulti,
                this.lblUpMsgType, this.pnlUpMsgType,
                this.lblUpMsg, this.txtUpMsg, this.btnUpMsgBrowse, this.btnUpMsgShow, this.btnUpMsgAdd,
                this.lstUpMultiMsgs, this.btnUpMultiAdd, this.btnUpMultiRemove,
                this.lblUpFont, this.cmbUpFontSize, this.cmbUpFontStyle, this.pnlUpColor, this.cmbUpColor, this.cmbUpLevel,
                this.lblUpAction, this.pnlUpAction,
                this.lblUpParam, this.lblUpSpeed, this.nudUpSpeed, this.lblUpPause, this.nudUpPause, this.lblUpPauseUnit,
                this.lblDnMsgType, this.pnlDnMsgType,
                this.lblDnMsg, this.txtDnMsg, this.btnDnMsgBrowse, this.btnDnMsgShow, this.btnDnMsgAdd,
                this.lstDnMultiMsgs, this.btnDnMultiAdd, this.btnDnMultiRemove,
                this.lblDnFont, this.cmbDnFontSize, this.cmbDnFontStyle, this.pnlDnColor, this.cmbDnColor, this.cmbDnLevel,
                this.lblDnAction, this.pnlDnAction,
                this.lblDnParam, this.lblDnSpeed, this.nudDnSpeed, this.lblDnPause, this.nudDnPause, this.lblDnPauseUnit,
                this.grpUpBoard, this.grpDnBoard,
                // 上行 Extra
                this.lblUpTimeHdr, this.cmbUpTimeType, this.pnlUpTimeClr, this.cmbUpTimeClr,
                this.lblUpCountStart, this.nudUpCountStart, this.lblUpCountStartTime,
                this.lblUpCountStop,  this.nudUpCountStop,  this.lblUpCountStopTime,
                this.lblUpPlatHdr, this.lblUpPlatIdx, this.nudUpPlatIdx, this.pnlUpPlatThumb, this.pnlUpPlatClr, this.cmbUpPlatClr,
                // 下行 Extra
                this.lblDnTimeHdr, this.cmbDnTimeType, this.pnlDnTimeClr, this.cmbDnTimeClr,
                this.lblDnCountStart, this.nudDnCountStart, this.lblDnCountStartTime,
                this.lblDnCountStop,  this.nudDnCountStop,  this.lblDnCountStopTime,
                this.lblDnPlatHdr, this.lblDnPlatIdx, this.nudDnPlatIdx, this.pnlDnPlatThumb, this.pnlDnPlatClr, this.cmbDnPlatClr,
                // 警示燈 Panel：最後加入 = z-order 最高，確保蓋過同區其他控件
                this.pnlUpAlarm, this.pnlDnAlarm
            });

            ((System.ComponentModel.ISupportInitialize)(this.nudUpSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpPlatIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnPlatIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpCountStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpCountStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnCountStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnCountStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpAlarmPlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnAlarmPlay)).EndInit();
            this.grpUpBoard.ResumeLayout(false);
            this.grpUpBoard.PerformLayout();
            this.grpDnBoard.ResumeLayout(false);
            this.grpDnBoard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ── 預覽顯示板 ───────────────────────────────────────────
        private System.Windows.Forms.Panel         pnlDisplay;
        private System.Windows.Forms.Label         lblDisplayHint;
        // ── Top buttons ──────────────────────────────────────────
        private System.Windows.Forms.Button        btnUpUpload;
        private System.Windows.Forms.Button        btnAllUpload;
        private System.Windows.Forms.CheckBox      chkUpMulti;
        private System.Windows.Forms.Button        btnDnUpload;
        private System.Windows.Forms.Button        btnSave;
        private System.Windows.Forms.CheckBox      chkDnMulti;
        // ── 上行 ─────────────────────────────────────────────────
        private System.Windows.Forms.Panel         pnlUpMsgType;
        private System.Windows.Forms.Label         lblUpMsgType;
        private System.Windows.Forms.RadioButton   rdoUpGeneral;
        private System.Windows.Forms.RadioButton   rdoUpPreRec;
        private System.Windows.Forms.Label         lblUpMsg;
        private System.Windows.Forms.TextBox       txtUpMsg;
        private System.Windows.Forms.Button        btnUpMsgBrowse;
        private System.Windows.Forms.Button        btnUpMsgShow;
        private System.Windows.Forms.Button        btnUpMsgAdd;
        private System.Windows.Forms.ListBox       lstUpMultiMsgs;
        private System.Windows.Forms.Button        btnUpMultiAdd;
        private System.Windows.Forms.Button        btnUpMultiRemove;
        private System.Windows.Forms.Label         lblUpFont;
        private System.Windows.Forms.ComboBox      cmbUpFontSize;
        private System.Windows.Forms.ComboBox      cmbUpFontStyle;
        private System.Windows.Forms.Panel         pnlUpColor;
        private System.Windows.Forms.ComboBox      cmbUpColor;
        private System.Windows.Forms.ComboBox      cmbUpLevel;
        private System.Windows.Forms.Label         lblUpAction;
        private System.Windows.Forms.Panel         pnlUpAction;
        private System.Windows.Forms.RadioButton   rdoUpAct61;
        private System.Windows.Forms.RadioButton   rdoUpAct62;
        private System.Windows.Forms.RadioButton   rdoUpAct63;
        private System.Windows.Forms.RadioButton   rdoUpAct64;
        private System.Windows.Forms.RadioButton   rdoUpAct65;
        private System.Windows.Forms.RadioButton   rdoUpAct66;
        private System.Windows.Forms.RadioButton   rdoUpAct67;
        private System.Windows.Forms.Label         lblUpParam;
        private System.Windows.Forms.Label         lblUpSpeed;
        private System.Windows.Forms.NumericUpDown nudUpSpeed;
        private System.Windows.Forms.Label         lblUpPause;
        private System.Windows.Forms.NumericUpDown nudUpPause;
        private System.Windows.Forms.Label         lblUpPauseUnit;
        // ── 下行 ─────────────────────────────────────────────────
        private System.Windows.Forms.Panel         pnlDnMsgType;
        private System.Windows.Forms.Label         lblDnMsgType;
        private System.Windows.Forms.RadioButton   rdoDnGeneral;
        private System.Windows.Forms.RadioButton   rdoDnPreRec;
        private System.Windows.Forms.Label         lblDnMsg;
        private System.Windows.Forms.TextBox       txtDnMsg;
        private System.Windows.Forms.Button        btnDnMsgBrowse;
        private System.Windows.Forms.Button        btnDnMsgShow;
        private System.Windows.Forms.Button        btnDnMsgAdd;
        private System.Windows.Forms.ListBox       lstDnMultiMsgs;
        private System.Windows.Forms.Button        btnDnMultiAdd;
        private System.Windows.Forms.Button        btnDnMultiRemove;
        private System.Windows.Forms.Label         lblDnFont;
        private System.Windows.Forms.ComboBox      cmbDnFontSize;
        private System.Windows.Forms.ComboBox      cmbDnFontStyle;
        private System.Windows.Forms.Panel         pnlDnColor;
        private System.Windows.Forms.ComboBox      cmbDnColor;
        private System.Windows.Forms.ComboBox      cmbDnLevel;
        private System.Windows.Forms.Label         lblDnAction;
        private System.Windows.Forms.Panel         pnlDnAction;
        private System.Windows.Forms.RadioButton   rdoDnAct61;
        private System.Windows.Forms.RadioButton   rdoDnAct62;
        private System.Windows.Forms.RadioButton   rdoDnAct63;
        private System.Windows.Forms.RadioButton   rdoDnAct64;
        private System.Windows.Forms.RadioButton   rdoDnAct65;
        private System.Windows.Forms.RadioButton   rdoDnAct66;
        private System.Windows.Forms.RadioButton   rdoDnAct67;
        private System.Windows.Forms.Label         lblDnParam;
        private System.Windows.Forms.Label         lblDnSpeed;
        private System.Windows.Forms.NumericUpDown nudDnSpeed;
        private System.Windows.Forms.Label         lblDnPause;
        private System.Windows.Forms.NumericUpDown nudDnPause;
        private System.Windows.Forms.Label         lblDnPauseUnit;
        // ── Board type ───────────────────────────────────────────
        private System.Windows.Forms.GroupBox      grpUpBoard;
        private System.Windows.Forms.RadioButton   rdoUpBoard1;
        private System.Windows.Forms.RadioButton   rdoUpBoard2;
        private System.Windows.Forms.RadioButton   rdoUpBoard3;
        private System.Windows.Forms.RadioButton   rdoUpBoard4;
        private System.Windows.Forms.RadioButton   rdoUpBoard5;
        private System.Windows.Forms.RadioButton   rdoUpBoard6;
        private System.Windows.Forms.RadioButton   rdoUpBoard7;
        private System.Windows.Forms.RadioButton   rdoUpBoard8;
        private System.Windows.Forms.GroupBox      grpDnBoard;
        private System.Windows.Forms.RadioButton   rdoDnBoard1;
        private System.Windows.Forms.RadioButton   rdoDnBoard2;
        private System.Windows.Forms.RadioButton   rdoDnBoard3;
        private System.Windows.Forms.RadioButton   rdoDnBoard4;
        private System.Windows.Forms.RadioButton   rdoDnBoard5;
        private System.Windows.Forms.RadioButton   rdoDnBoard6;
        private System.Windows.Forms.RadioButton   rdoDnBoard7;
        private System.Windows.Forms.RadioButton   rdoDnBoard8;
        // ── 上行 Extra ───────────────────────────────────────────
        private System.Windows.Forms.Label         lblUpTimeHdr;
        private System.Windows.Forms.ComboBox      cmbUpTimeType;
        private System.Windows.Forms.Panel         pnlUpTimeClr;
        private System.Windows.Forms.ComboBox      cmbUpTimeClr;
        private System.Windows.Forms.Label         lblUpCountStart;
        private System.Windows.Forms.NumericUpDown nudUpCountStart;
        private System.Windows.Forms.Label         lblUpCountStartTime;
        private System.Windows.Forms.Label         lblUpCountStop;
        private System.Windows.Forms.NumericUpDown nudUpCountStop;
        private System.Windows.Forms.Label         lblUpCountStopTime;
        private System.Windows.Forms.Label         lblUpPlatHdr;
        private System.Windows.Forms.Label         lblUpPlatIdx;
        private System.Windows.Forms.NumericUpDown nudUpPlatIdx;
        private System.Windows.Forms.Panel         pnlUpPlatThumb;
        private System.Windows.Forms.Panel         pnlUpPlatClr;
        private System.Windows.Forms.ComboBox      cmbUpPlatClr;
        // ── 上行 警示燈 Extra ────────────────────────────────────
        private System.Windows.Forms.Panel         pnlUpAlarm;
        private System.Windows.Forms.Label         lblUpAlarmHdr;
        private System.Windows.Forms.Label         lblUpAlarmMsgLbl;
        private System.Windows.Forms.RadioButton   rdoUpAlarmMsgOn;
        private System.Windows.Forms.RadioButton   rdoUpAlarmMsgOff;
        private System.Windows.Forms.Label         lblUpAlarmPlay;
        private System.Windows.Forms.NumericUpDown nudUpAlarmPlay;
        private System.Windows.Forms.Label         lblUpAlarmLight;
        private System.Windows.Forms.RadioButton   rdoUpLightOff;
        private System.Windows.Forms.RadioButton   rdoUpLightOn;
        private System.Windows.Forms.RadioButton   rdoUpLightBlink;
        // ── 下行 警示燈 Extra ────────────────────────────────────
        private System.Windows.Forms.Panel         pnlDnAlarm;
        private System.Windows.Forms.Label         lblDnAlarmHdr;
        private System.Windows.Forms.Label         lblDnAlarmMsgLbl;
        private System.Windows.Forms.RadioButton   rdoDnAlarmMsgOn;
        private System.Windows.Forms.RadioButton   rdoDnAlarmMsgOff;
        private System.Windows.Forms.Label         lblDnAlarmPlay;
        private System.Windows.Forms.NumericUpDown nudDnAlarmPlay;
        private System.Windows.Forms.Label         lblDnAlarmLight;
        private System.Windows.Forms.RadioButton   rdoDnLightOff;
        private System.Windows.Forms.RadioButton   rdoDnLightOn;
        private System.Windows.Forms.RadioButton   rdoDnLightBlink;
        // ── 下行 Extra ───────────────────────────────────────────
        private System.Windows.Forms.Label         lblDnTimeHdr;
        private System.Windows.Forms.ComboBox      cmbDnTimeType;
        private System.Windows.Forms.Panel         pnlDnTimeClr;
        private System.Windows.Forms.ComboBox      cmbDnTimeClr;
        private System.Windows.Forms.Label         lblDnCountStart;
        private System.Windows.Forms.NumericUpDown nudDnCountStart;
        private System.Windows.Forms.Label         lblDnCountStartTime;
        private System.Windows.Forms.Label         lblDnCountStop;
        private System.Windows.Forms.NumericUpDown nudDnCountStop;
        private System.Windows.Forms.Label         lblDnCountStopTime;
        private System.Windows.Forms.Label         lblDnPlatHdr;
        private System.Windows.Forms.Label         lblDnPlatIdx;
        private System.Windows.Forms.NumericUpDown nudDnPlatIdx;
        private System.Windows.Forms.Panel         pnlDnPlatThumb;
        private System.Windows.Forms.Panel         pnlDnPlatClr;
        private System.Windows.Forms.ComboBox      cmbDnPlatClr;
    }
}
