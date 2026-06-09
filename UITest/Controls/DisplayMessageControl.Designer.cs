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
            this.lblUpMsgType   = new System.Windows.Forms.Label();
            this.rdoUpGeneral   = new System.Windows.Forms.RadioButton();
            this.rdoUpPreRec    = new System.Windows.Forms.RadioButton();
            this.lblUpMsg       = new System.Windows.Forms.Label();
            this.txtUpMsg       = new System.Windows.Forms.TextBox();
            this.btnUpMsgBrowse = new System.Windows.Forms.Button();
            this.btnUpMsgShow   = new System.Windows.Forms.Button();
            this.btnUpMsgAdd    = new System.Windows.Forms.Button();
            this.lblUpFont      = new System.Windows.Forms.Label();
            this.cmbUpFontSize  = new System.Windows.Forms.ComboBox();
            this.cmbUpFontStyle = new System.Windows.Forms.ComboBox();
            this.pnlUpColor     = new System.Windows.Forms.Panel();
            this.cmbUpColor     = new System.Windows.Forms.ComboBox();
            this.cmbUpLevel     = new System.Windows.Forms.ComboBox();
            this.lblUpAction    = new System.Windows.Forms.Label();
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
            this.lblDnMsgType   = new System.Windows.Forms.Label();
            this.rdoDnGeneral   = new System.Windows.Forms.RadioButton();
            this.rdoDnPreRec    = new System.Windows.Forms.RadioButton();
            this.lblDnMsg       = new System.Windows.Forms.Label();
            this.txtDnMsg       = new System.Windows.Forms.TextBox();
            this.btnDnMsgBrowse = new System.Windows.Forms.Button();
            this.btnDnMsgShow   = new System.Windows.Forms.Button();
            this.btnDnMsgAdd    = new System.Windows.Forms.Button();
            this.lblDnFont      = new System.Windows.Forms.Label();
            this.cmbDnFontSize  = new System.Windows.Forms.ComboBox();
            this.cmbDnFontStyle = new System.Windows.Forms.ComboBox();
            this.pnlDnColor     = new System.Windows.Forms.Panel();
            this.cmbDnColor     = new System.Windows.Forms.ComboBox();
            this.cmbDnLevel     = new System.Windows.Forms.ComboBox();
            this.lblDnAction    = new System.Windows.Forms.Label();
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
            this.lblUpTimeHdr   = new System.Windows.Forms.Label();
            this.cmbUpTimeType  = new System.Windows.Forms.ComboBox();
            this.pnlUpTimeClr   = new System.Windows.Forms.Panel();
            this.cmbUpTimeClr   = new System.Windows.Forms.ComboBox();
            this.lblUpPlatHdr   = new System.Windows.Forms.Label();
            this.lblUpPlatIdx   = new System.Windows.Forms.Label();
            this.nudUpPlatIdx   = new System.Windows.Forms.NumericUpDown();
            this.pnlUpPlatThumb = new System.Windows.Forms.Panel();
            this.pnlUpPlatClr   = new System.Windows.Forms.Panel();
            this.cmbUpPlatClr   = new System.Windows.Forms.ComboBox();
            // ── 下行 Extra 子選項 ──
            this.lblDnTimeHdr   = new System.Windows.Forms.Label();
            this.cmbDnTimeType  = new System.Windows.Forms.ComboBox();
            this.pnlDnTimeClr   = new System.Windows.Forms.Panel();
            this.cmbDnTimeClr   = new System.Windows.Forms.ComboBox();
            this.lblDnPlatHdr   = new System.Windows.Forms.Label();
            this.lblDnPlatIdx   = new System.Windows.Forms.Label();
            this.nudDnPlatIdx   = new System.Windows.Forms.NumericUpDown();
            this.pnlDnPlatThumb = new System.Windows.Forms.Panel();
            this.pnlDnPlatClr   = new System.Windows.Forms.Panel();
            this.cmbDnPlatClr   = new System.Windows.Forms.ComboBox();

            ((System.ComponentModel.ISupportInitialize)(this.nudUpSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpPlatIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnPlatIdx)).BeginInit();
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
            // 上行 (左欄 x=10, y 從 90 開始)
            // ════════════════════════════════════════════════
            this.lblUpMsgType.AutoSize = true;
            this.lblUpMsgType.Location = new System.Drawing.Point(10, 90);
            this.lblUpMsgType.Text     = "上行訊息頻型";

            this.rdoUpGeneral.AutoSize = true;
            this.rdoUpGeneral.Location = new System.Drawing.Point(10, 108);
            this.rdoUpGeneral.Text     = "一般訊息(2AH)";
            this.rdoUpGeneral.Checked  = true;

            this.rdoUpPreRec.AutoSize = true;
            this.rdoUpPreRec.Location = new System.Drawing.Point(135, 108);
            this.rdoUpPreRec.Text     = "預錄訊息(2CH)";

            this.lblUpMsg.AutoSize = true;
            this.lblUpMsg.Location = new System.Drawing.Point(10, 135);
            this.lblUpMsg.Text     = "上行一般訊息";

            this.txtUpMsg.Location = new System.Drawing.Point(10, 153);
            this.txtUpMsg.Size     = new System.Drawing.Size(375, 21);
            this.txtUpMsg.Text     = "萬大線中英文abcdeABCDE123456";

            this.btnUpMsgBrowse.Location = new System.Drawing.Point(389, 152);
            this.btnUpMsgBrowse.Size     = new System.Drawing.Size(28, 23);
            this.btnUpMsgBrowse.Text     = "...";

            this.btnUpMsgShow.Location = new System.Drawing.Point(420, 152);
            this.btnUpMsgShow.Size     = new System.Drawing.Size(50, 23);
            this.btnUpMsgShow.Text     = "顯示";
            this.btnUpMsgShow.Click   += new System.EventHandler(this.btnUpMsgShow_Click);

            this.btnUpMsgAdd.Location = new System.Drawing.Point(473, 152);
            this.btnUpMsgAdd.Size     = new System.Drawing.Size(50, 23);
            this.btnUpMsgAdd.Text     = "新增";
            this.btnUpMsgAdd.Click   += new System.EventHandler(this.btnUpMsgAdd_Click);

            this.lblUpFont.AutoSize = true;
            this.lblUpFont.Location = new System.Drawing.Point(10, 180);
            this.lblUpFont.Text     = "上行訊息(字型參數)";

            this.cmbUpFontSize.Location      = new System.Drawing.Point(10, 198);
            this.cmbUpFontSize.Size          = new System.Drawing.Size(65, 21);
            this.cmbUpFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpFontSize.Items.AddRange(new object[] { "24x24", "16x16", "32x32" });
            this.cmbUpFontSize.SelectedIndex = 0;

            this.cmbUpFontStyle.Location      = new System.Drawing.Point(80, 198);
            this.cmbUpFontStyle.Size          = new System.Drawing.Size(65, 21);
            this.cmbUpFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpFontStyle.Items.AddRange(new object[] { "明體", "黑體" });
            this.cmbUpFontStyle.SelectedIndex = 0;

            this.pnlUpColor.Location    = new System.Drawing.Point(150, 200);
            this.pnlUpColor.Size        = new System.Drawing.Size(18, 18);
            this.pnlUpColor.BackColor   = System.Drawing.Color.Yellow;
            this.pnlUpColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.cmbUpColor.Location      = new System.Drawing.Point(172, 198);
            this.cmbUpColor.Size          = new System.Drawing.Size(100, 21);
            this.cmbUpColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpColor.Items.AddRange(new object[] { "clYellow", "clRed", "clGreen", "clWhite" });
            this.cmbUpColor.SelectedIndex = 0;

            this.cmbUpLevel.Location      = new System.Drawing.Point(276, 198);
            this.cmbUpLevel.Size          = new System.Drawing.Size(105, 21);
            this.cmbUpLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpLevel.Items.AddRange(new object[] { "最低Level 4", "最低Level 3", "最低Level 2", "最低Level 1" });
            this.cmbUpLevel.SelectedIndex = 0;

            this.lblUpAction.AutoSize = true;
            this.lblUpAction.Location = new System.Drawing.Point(10, 225);
            this.lblUpAction.Text     = "上行訊息動作方式";

            this.rdoUpAct61.AutoSize = true;
            this.rdoUpAct61.Location = new System.Drawing.Point(10, 243);
            this.rdoUpAct61.Text     = "立即顯示(61H)";
            this.rdoUpAct61.Checked  = true;

            this.rdoUpAct62.AutoSize = true;
            this.rdoUpAct62.Location = new System.Drawing.Point(10, 265);
            this.rdoUpAct62.Text     = "向左捲動(靠右對齊)(62H)";

            this.rdoUpAct63.AutoSize = true;
            this.rdoUpAct63.Location = new System.Drawing.Point(10, 287);
            this.rdoUpAct63.Text     = "向左捲動(靠左對齊)(63H)";

            this.rdoUpAct64.AutoSize = true;
            this.rdoUpAct64.Location = new System.Drawing.Point(10, 309);
            this.rdoUpAct64.Text     = "向左捲動(左移消失)(64H)";

            this.rdoUpAct65.AutoSize = true;
            this.rdoUpAct65.Location = new System.Drawing.Point(220, 243);
            this.rdoUpAct65.Text     = "向下捲動(65H)";

            this.rdoUpAct66.AutoSize = true;
            this.rdoUpAct66.Location = new System.Drawing.Point(220, 265);
            this.rdoUpAct66.Text     = "向上捲動(66H)";

            this.rdoUpAct67.AutoSize = true;
            this.rdoUpAct67.Location = new System.Drawing.Point(220, 287);
            this.rdoUpAct67.Text     = "閃爍(67H)";

            this.lblUpParam.AutoSize = true;
            this.lblUpParam.Location = new System.Drawing.Point(10, 339);
            this.lblUpParam.Text     = "上行訊息動作參數";

            this.lblUpSpeed.AutoSize = true;
            this.lblUpSpeed.Location = new System.Drawing.Point(10, 359);
            this.lblUpSpeed.Text     = "速度";

            this.nudUpSpeed.Location = new System.Drawing.Point(45, 357);
            this.nudUpSpeed.Size     = new System.Drawing.Size(50, 20);
            this.nudUpSpeed.Minimum  = 1;
            this.nudUpSpeed.Maximum  = 99;
            this.nudUpSpeed.Value    = 5;

            this.lblUpPause.AutoSize = true;
            this.lblUpPause.Location = new System.Drawing.Point(105, 359);
            this.lblUpPause.Text     = "停留時間";

            this.nudUpPause.Location = new System.Drawing.Point(165, 357);
            this.nudUpPause.Size     = new System.Drawing.Size(50, 20);
            this.nudUpPause.Minimum  = 1;
            this.nudUpPause.Maximum  = 99;
            this.nudUpPause.Value    = 10;

            this.lblUpPauseUnit.AutoSize = true;
            this.lblUpPauseUnit.Location = new System.Drawing.Point(220, 359);
            this.lblUpPauseUnit.Text     = "(100ms)";

            // ════════════════════════════════════════════════
            // 下行 (右欄 x=595)
            // ════════════════════════════════════════════════
            this.lblDnMsgType.AutoSize = true;
            this.lblDnMsgType.Location = new System.Drawing.Point(595, 90);
            this.lblDnMsgType.Text     = "下行訊息頻型";

            this.rdoDnGeneral.AutoSize = true;
            this.rdoDnGeneral.Location = new System.Drawing.Point(595, 108);
            this.rdoDnGeneral.Text     = "一般訊息(2AH)";
            this.rdoDnGeneral.Checked  = true;

            this.rdoDnPreRec.AutoSize = true;
            this.rdoDnPreRec.Location = new System.Drawing.Point(720, 108);
            this.rdoDnPreRec.Text     = "預錄訊息(2CH)";

            this.lblDnMsg.AutoSize = true;
            this.lblDnMsg.Location = new System.Drawing.Point(595, 135);
            this.lblDnMsg.Text     = "下行一般訊息";

            this.txtDnMsg.Location = new System.Drawing.Point(595, 153);
            this.txtDnMsg.Size     = new System.Drawing.Size(375, 21);
            this.txtDnMsg.Text     = "萬大線中英文abcdeABCDE123456";

            this.btnDnMsgBrowse.Location = new System.Drawing.Point(974, 152);
            this.btnDnMsgBrowse.Size     = new System.Drawing.Size(28, 23);
            this.btnDnMsgBrowse.Text     = "...";

            this.btnDnMsgShow.Location = new System.Drawing.Point(1005, 152);
            this.btnDnMsgShow.Size     = new System.Drawing.Size(50, 23);
            this.btnDnMsgShow.Text     = "顯示";
            this.btnDnMsgShow.Click   += new System.EventHandler(this.btnDnMsgShow_Click);

            this.btnDnMsgAdd.Location = new System.Drawing.Point(1058, 152);
            this.btnDnMsgAdd.Size     = new System.Drawing.Size(50, 23);
            this.btnDnMsgAdd.Text     = "新增";
            this.btnDnMsgAdd.Click   += new System.EventHandler(this.btnDnMsgAdd_Click);

            this.lblDnFont.AutoSize = true;
            this.lblDnFont.Location = new System.Drawing.Point(595, 180);
            this.lblDnFont.Text     = "下行訊息(字型參數)";

            this.cmbDnFontSize.Location      = new System.Drawing.Point(595, 198);
            this.cmbDnFontSize.Size          = new System.Drawing.Size(65, 21);
            this.cmbDnFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnFontSize.Items.AddRange(new object[] { "24x24", "16x16", "32x32" });
            this.cmbDnFontSize.SelectedIndex = 0;

            this.cmbDnFontStyle.Location      = new System.Drawing.Point(665, 198);
            this.cmbDnFontStyle.Size          = new System.Drawing.Size(65, 21);
            this.cmbDnFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnFontStyle.Items.AddRange(new object[] { "明體", "黑體" });
            this.cmbDnFontStyle.SelectedIndex = 0;

            this.pnlDnColor.Location    = new System.Drawing.Point(735, 200);
            this.pnlDnColor.Size        = new System.Drawing.Size(18, 18);
            this.pnlDnColor.BackColor   = System.Drawing.Color.Yellow;
            this.pnlDnColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.cmbDnColor.Location      = new System.Drawing.Point(757, 198);
            this.cmbDnColor.Size          = new System.Drawing.Size(100, 21);
            this.cmbDnColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnColor.Items.AddRange(new object[] { "clYellow", "clRed", "clGreen", "clWhite" });
            this.cmbDnColor.SelectedIndex = 0;

            this.cmbDnLevel.Location      = new System.Drawing.Point(861, 198);
            this.cmbDnLevel.Size          = new System.Drawing.Size(105, 21);
            this.cmbDnLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnLevel.Items.AddRange(new object[] { "最低Level 4", "最低Level 3", "最低Level 2", "最低Level 1" });
            this.cmbDnLevel.SelectedIndex = 0;

            this.lblDnAction.AutoSize = true;
            this.lblDnAction.Location = new System.Drawing.Point(595, 225);
            this.lblDnAction.Text     = "下行訊息動作方式";

            this.rdoDnAct61.AutoSize = true;
            this.rdoDnAct61.Location = new System.Drawing.Point(595, 243);
            this.rdoDnAct61.Text     = "立即顯示(61H)";
            this.rdoDnAct61.Checked  = true;

            this.rdoDnAct62.AutoSize = true;
            this.rdoDnAct62.Location = new System.Drawing.Point(595, 265);
            this.rdoDnAct62.Text     = "向左捲動(靠右對齊)(62H)";

            this.rdoDnAct63.AutoSize = true;
            this.rdoDnAct63.Location = new System.Drawing.Point(595, 287);
            this.rdoDnAct63.Text     = "向左捲動(靠左對齊)(63H)";

            this.rdoDnAct64.AutoSize = true;
            this.rdoDnAct64.Location = new System.Drawing.Point(595, 309);
            this.rdoDnAct64.Text     = "向左捲動(左移消失)(64H)";

            this.rdoDnAct65.AutoSize = true;
            this.rdoDnAct65.Location = new System.Drawing.Point(805, 243);
            this.rdoDnAct65.Text     = "向下捲動(65H)";

            this.rdoDnAct66.AutoSize = true;
            this.rdoDnAct66.Location = new System.Drawing.Point(805, 265);
            this.rdoDnAct66.Text     = "向上捲動(66H)";

            this.rdoDnAct67.AutoSize = true;
            this.rdoDnAct67.Location = new System.Drawing.Point(805, 287);
            this.rdoDnAct67.Text     = "閃爍(67H)";

            this.lblDnParam.AutoSize = true;
            this.lblDnParam.Location = new System.Drawing.Point(595, 339);
            this.lblDnParam.Text     = "下行訊息動作參數";

            this.lblDnSpeed.AutoSize = true;
            this.lblDnSpeed.Location = new System.Drawing.Point(595, 359);
            this.lblDnSpeed.Text     = "速度";

            this.nudDnSpeed.Location = new System.Drawing.Point(630, 357);
            this.nudDnSpeed.Size     = new System.Drawing.Size(50, 20);
            this.nudDnSpeed.Minimum  = 1;
            this.nudDnSpeed.Maximum  = 99;
            this.nudDnSpeed.Value    = 5;

            this.lblDnPause.AutoSize = true;
            this.lblDnPause.Location = new System.Drawing.Point(690, 359);
            this.lblDnPause.Text     = "停留時間";

            this.nudDnPause.Location = new System.Drawing.Point(750, 357);
            this.nudDnPause.Size     = new System.Drawing.Size(50, 20);
            this.nudDnPause.Minimum  = 1;
            this.nudDnPause.Maximum  = 99;
            this.nudDnPause.Value    = 8;

            this.lblDnPauseUnit.AutoSize = true;
            this.lblDnPauseUnit.Location = new System.Drawing.Point(805, 359);
            this.lblDnPauseUnit.Text     = "(100ms)";

            // ════════════════════════════════════════════════
            // 上行顯示器板型  (x=10, y=394, w=575, h=185)
            // ════════════════════════════════════════════════
            this.grpUpBoard.Location = new System.Drawing.Point(10, 394);
            this.grpUpBoard.Size     = new System.Drawing.Size(575, 185);
            this.grpUpBoard.Text     = "上行顯示器板型";
            this.grpUpBoard.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.rdoUpBoard1, this.rdoUpBoard2, this.rdoUpBoard3, this.rdoUpBoard4,
                this.rdoUpBoard5, this.rdoUpBoard6, this.rdoUpBoard7, this.rdoUpBoard8
            });

            this.rdoUpBoard1.AutoSize = true;
            this.rdoUpBoard1.Location = new System.Drawing.Point(8, 18);
            this.rdoUpBoard1.Text     = "1. 訊息顯示(71H)（全畫面）";
            this.rdoUpBoard1.Checked  = true;
            this.rdoUpBoard1.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard2.AutoSize = true;
            this.rdoUpBoard2.Location = new System.Drawing.Point(8, 38);
            this.rdoUpBoard2.Text     = "2. 訊息顯示(74H) + 右側（標準或列車到站倒數）時間顯示";
            this.rdoUpBoard2.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard3.AutoSize = true;
            this.rdoUpBoard3.Location = new System.Drawing.Point(8, 58);
            this.rdoUpBoard3.Text     = "3. 左側月台碼 + 訊息顯示(73H) + 右側時間顯示";
            this.rdoUpBoard3.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard4.AutoSize = true;
            this.rdoUpBoard4.Location = new System.Drawing.Point(8, 78);
            this.rdoUpBoard4.Text     = "4. 訊息顯示(74H) + 右側48x48頁比時鐘顯示(82H)";
            this.rdoUpBoard4.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard5.AutoSize = true;
            this.rdoUpBoard5.Location = new System.Drawing.Point(8, 98);
            this.rdoUpBoard5.Text     = "5. 左側月台碼 + 訊息顯示(72H)";
            this.rdoUpBoard5.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard6.AutoSize = true;
            this.rdoUpBoard6.Location = new System.Drawing.Point(8, 118);
            this.rdoUpBoard6.Text     = "6. 緊急訊息(71H) + 警示燈  設定";
            this.rdoUpBoard6.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard7.AutoSize = true;
            this.rdoUpBoard7.Location = new System.Drawing.Point(8, 138);
            this.rdoUpBoard7.Text     = "7. 路線代碼(7DH) + 到站訊息(74H) + 右側（時間倒數）";
            this.rdoUpBoard7.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            this.rdoUpBoard8.AutoSize = true;
            this.rdoUpBoard8.Location = new System.Drawing.Point(8, 158);
            this.rdoUpBoard8.Text     = "8. 站與站之間，連續圖片動態顯示模式（83H）";
            this.rdoUpBoard8.CheckedChanged += new System.EventHandler(this.rdoUpBoard_CheckedChanged);

            // ════════════════════════════════════════════════
            // 下行顯示器板型  (x=595, y=394, w=575, h=185)
            // ════════════════════════════════════════════════
            this.grpDnBoard.Location = new System.Drawing.Point(595, 394);
            this.grpDnBoard.Size     = new System.Drawing.Size(575, 185);
            this.grpDnBoard.Text     = "下行顯示器板型";
            this.grpDnBoard.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.rdoDnBoard1, this.rdoDnBoard2, this.rdoDnBoard3, this.rdoDnBoard4,
                this.rdoDnBoard5, this.rdoDnBoard6, this.rdoDnBoard7, this.rdoDnBoard8
            });

            this.rdoDnBoard1.AutoSize = true;
            this.rdoDnBoard1.Location = new System.Drawing.Point(8, 18);
            this.rdoDnBoard1.Text     = "1. 訊息顯示(71H)（全畫面）";
            this.rdoDnBoard1.Checked  = true;
            this.rdoDnBoard1.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard2.AutoSize = true;
            this.rdoDnBoard2.Location = new System.Drawing.Point(8, 38);
            this.rdoDnBoard2.Text     = "2. 訊息顯示(74H) + 右側（標準或列車到站倒數）時間顯示";
            this.rdoDnBoard2.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard3.AutoSize = true;
            this.rdoDnBoard3.Location = new System.Drawing.Point(8, 58);
            this.rdoDnBoard3.Text     = "3. 左側月台碼 + 訊息顯示(73H) + 右側時間顯示";
            this.rdoDnBoard3.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard4.AutoSize = true;
            this.rdoDnBoard4.Location = new System.Drawing.Point(8, 78);
            this.rdoDnBoard4.Text     = "4. 訊息顯示(74H) + 右側48x48頁比時鐘顯示(82H)";
            this.rdoDnBoard4.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard5.AutoSize = true;
            this.rdoDnBoard5.Location = new System.Drawing.Point(8, 98);
            this.rdoDnBoard5.Text     = "5. 左側月台碼 + 訊息顯示(72H)";
            this.rdoDnBoard5.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard6.AutoSize = true;
            this.rdoDnBoard6.Location = new System.Drawing.Point(8, 118);
            this.rdoDnBoard6.Text     = "6. 緊急訊息(71H) + 警示燈  設定";
            this.rdoDnBoard6.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard7.AutoSize = true;
            this.rdoDnBoard7.Location = new System.Drawing.Point(8, 138);
            this.rdoDnBoard7.Text     = "7. 左側標準進時碼(7EH) + 到站訊息(74H) + 右側(時間倒數)";
            this.rdoDnBoard7.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            this.rdoDnBoard8.AutoSize = true;
            this.rdoDnBoard8.Location = new System.Drawing.Point(8, 158);
            this.rdoDnBoard8.Text     = "8. 站與站之間，連續圖片動態顯示模式（83H）";
            this.rdoDnBoard8.CheckedChanged += new System.EventHandler(this.rdoDnBoard_CheckedChanged);

            // ════════════════════════════════════════════════
            // 上行 Extra
            // Board bottom = 394+185 = 579  → Extra 從 y=598 開始
            // 時間列: header=598  controls=616
            // 月台列: header=650  controls=668
            // ════════════════════════════════════════════════
            this.lblUpTimeHdr.AutoSize = true;
            this.lblUpTimeHdr.Location = new System.Drawing.Point(10, 598);
            this.lblUpTimeHdr.Text     = "上行時間(右側顯示)";
            this.lblUpTimeHdr.Visible  = false;

            this.cmbUpTimeType.Location      = new System.Drawing.Point(10, 616);
            this.cmbUpTimeType.Size          = new System.Drawing.Size(130, 21);
            this.cmbUpTimeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpTimeType.Items.AddRange(new object[] { "標準時間", "列車到站倒數" });
            this.cmbUpTimeType.SelectedIndex = 0;
            this.cmbUpTimeType.Visible       = false;

            this.pnlUpTimeClr.Location    = new System.Drawing.Point(148, 618);
            this.pnlUpTimeClr.Size        = new System.Drawing.Size(18, 18);
            this.pnlUpTimeClr.BackColor   = System.Drawing.Color.Yellow;
            this.pnlUpTimeClr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpTimeClr.Visible     = false;

            this.cmbUpTimeClr.Location      = new System.Drawing.Point(170, 616);
            this.cmbUpTimeClr.Size          = new System.Drawing.Size(90, 21);
            this.cmbUpTimeClr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpTimeClr.Items.AddRange(new object[] { "clYellow", "clRed", "clGreen", "clWhite" });
            this.cmbUpTimeClr.SelectedIndex = 0;
            this.cmbUpTimeClr.Visible       = false;

            this.lblUpPlatHdr.AutoSize = true;
            this.lblUpPlatHdr.Location = new System.Drawing.Point(10, 650);
            this.lblUpPlatHdr.Text     = "上行月台碼設定";
            this.lblUpPlatHdr.Visible  = false;

            this.lblUpPlatIdx.AutoSize = true;
            this.lblUpPlatIdx.Location = new System.Drawing.Point(10, 671);
            this.lblUpPlatIdx.Text     = "圖檔索引值";
            this.lblUpPlatIdx.Visible  = false;

            this.nudUpPlatIdx.Location = new System.Drawing.Point(80, 668);
            this.nudUpPlatIdx.Size     = new System.Drawing.Size(50, 20);
            this.nudUpPlatIdx.Minimum  = 0;
            this.nudUpPlatIdx.Maximum  = 99;
            this.nudUpPlatIdx.Value    = 1;
            this.nudUpPlatIdx.Visible  = false;

            this.pnlUpPlatThumb.Location    = new System.Drawing.Point(140, 663);
            this.pnlUpPlatThumb.Size        = new System.Drawing.Size(30, 30);
            this.pnlUpPlatThumb.BackColor   = System.Drawing.Color.DimGray;
            this.pnlUpPlatThumb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpPlatThumb.Visible     = false;

            this.pnlUpPlatClr.Location    = new System.Drawing.Point(180, 668);
            this.pnlUpPlatClr.Size        = new System.Drawing.Size(18, 18);
            this.pnlUpPlatClr.BackColor   = System.Drawing.Color.Yellow;
            this.pnlUpPlatClr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpPlatClr.Visible     = false;

            this.cmbUpPlatClr.Location      = new System.Drawing.Point(202, 668);
            this.cmbUpPlatClr.Size          = new System.Drawing.Size(90, 21);
            this.cmbUpPlatClr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpPlatClr.Items.AddRange(new object[] { "clYellow", "clRed", "clGreen", "clWhite" });
            this.cmbUpPlatClr.SelectedIndex = 0;
            this.cmbUpPlatClr.Visible       = false;

            // ════════════════════════════════════════════════
            // 下行 Extra（與上行同 y，x 偏移 595）
            // ════════════════════════════════════════════════
            this.lblDnTimeHdr.AutoSize = true;
            this.lblDnTimeHdr.Location = new System.Drawing.Point(595, 598);
            this.lblDnTimeHdr.Text     = "下行時間(右側顯示)";
            this.lblDnTimeHdr.Visible  = false;

            this.cmbDnTimeType.Location      = new System.Drawing.Point(595, 616);
            this.cmbDnTimeType.Size          = new System.Drawing.Size(130, 21);
            this.cmbDnTimeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnTimeType.Items.AddRange(new object[] { "標準時間", "列車到站倒數" });
            this.cmbDnTimeType.SelectedIndex = 0;
            this.cmbDnTimeType.Visible       = false;

            this.pnlDnTimeClr.Location    = new System.Drawing.Point(733, 618);
            this.pnlDnTimeClr.Size        = new System.Drawing.Size(18, 18);
            this.pnlDnTimeClr.BackColor   = System.Drawing.Color.Yellow;
            this.pnlDnTimeClr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDnTimeClr.Visible     = false;

            this.cmbDnTimeClr.Location      = new System.Drawing.Point(755, 616);
            this.cmbDnTimeClr.Size          = new System.Drawing.Size(90, 21);
            this.cmbDnTimeClr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnTimeClr.Items.AddRange(new object[] { "clYellow", "clRed", "clGreen", "clWhite" });
            this.cmbDnTimeClr.SelectedIndex = 0;
            this.cmbDnTimeClr.Visible       = false;

            this.lblDnPlatHdr.AutoSize = true;
            this.lblDnPlatHdr.Location = new System.Drawing.Point(595, 650);
            this.lblDnPlatHdr.Text     = "下行月台碼設定";
            this.lblDnPlatHdr.Visible  = false;

            this.lblDnPlatIdx.AutoSize = true;
            this.lblDnPlatIdx.Location = new System.Drawing.Point(595, 671);
            this.lblDnPlatIdx.Text     = "圖檔索引值";
            this.lblDnPlatIdx.Visible  = false;

            this.nudDnPlatIdx.Location = new System.Drawing.Point(665, 668);
            this.nudDnPlatIdx.Size     = new System.Drawing.Size(50, 20);
            this.nudDnPlatIdx.Minimum  = 0;
            this.nudDnPlatIdx.Maximum  = 99;
            this.nudDnPlatIdx.Value    = 1;
            this.nudDnPlatIdx.Visible  = false;

            this.pnlDnPlatThumb.Location    = new System.Drawing.Point(725, 663);
            this.pnlDnPlatThumb.Size        = new System.Drawing.Size(30, 30);
            this.pnlDnPlatThumb.BackColor   = System.Drawing.Color.DimGray;
            this.pnlDnPlatThumb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDnPlatThumb.Visible     = false;

            this.pnlDnPlatClr.Location    = new System.Drawing.Point(765, 668);
            this.pnlDnPlatClr.Size        = new System.Drawing.Size(18, 18);
            this.pnlDnPlatClr.BackColor   = System.Drawing.Color.Yellow;
            this.pnlDnPlatClr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDnPlatClr.Visible     = false;

            this.cmbDnPlatClr.Location      = new System.Drawing.Point(787, 668);
            this.cmbDnPlatClr.Size          = new System.Drawing.Size(90, 21);
            this.cmbDnPlatClr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDnPlatClr.Items.AddRange(new object[] { "clYellow", "clRed", "clGreen", "clWhite" });
            this.cmbDnPlatClr.SelectedIndex = 0;
            this.cmbDnPlatClr.Visible       = false;

            // ════════════════════════════════════════════════
            // UserControl 本身
            // ════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(1185, 720);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.pnlDisplay, this.lblDisplayHint,
                this.btnUpUpload, this.btnAllUpload, this.chkUpMulti,
                this.btnDnUpload, this.btnSave,      this.chkDnMulti,
                this.lblUpMsgType, this.rdoUpGeneral, this.rdoUpPreRec,
                this.lblUpMsg, this.txtUpMsg, this.btnUpMsgBrowse, this.btnUpMsgShow, this.btnUpMsgAdd,
                this.lblUpFont, this.cmbUpFontSize, this.cmbUpFontStyle, this.pnlUpColor, this.cmbUpColor, this.cmbUpLevel,
                this.lblUpAction,
                this.rdoUpAct61, this.rdoUpAct62, this.rdoUpAct63, this.rdoUpAct64,
                this.rdoUpAct65, this.rdoUpAct66, this.rdoUpAct67,
                this.lblUpParam, this.lblUpSpeed, this.nudUpSpeed, this.lblUpPause, this.nudUpPause, this.lblUpPauseUnit,
                this.lblDnMsgType, this.rdoDnGeneral, this.rdoDnPreRec,
                this.lblDnMsg, this.txtDnMsg, this.btnDnMsgBrowse, this.btnDnMsgShow, this.btnDnMsgAdd,
                this.lblDnFont, this.cmbDnFontSize, this.cmbDnFontStyle, this.pnlDnColor, this.cmbDnColor, this.cmbDnLevel,
                this.lblDnAction,
                this.rdoDnAct61, this.rdoDnAct62, this.rdoDnAct63, this.rdoDnAct64,
                this.rdoDnAct65, this.rdoDnAct66, this.rdoDnAct67,
                this.lblDnParam, this.lblDnSpeed, this.nudDnSpeed, this.lblDnPause, this.nudDnPause, this.lblDnPauseUnit,
                this.grpUpBoard, this.grpDnBoard,
                // 上行 Extra
                this.lblUpTimeHdr, this.cmbUpTimeType, this.pnlUpTimeClr, this.cmbUpTimeClr,
                this.lblUpPlatHdr, this.lblUpPlatIdx, this.nudUpPlatIdx, this.pnlUpPlatThumb, this.pnlUpPlatClr, this.cmbUpPlatClr,
                // 下行 Extra
                this.lblDnTimeHdr, this.cmbDnTimeType, this.pnlDnTimeClr, this.cmbDnTimeClr,
                this.lblDnPlatHdr, this.lblDnPlatIdx, this.nudDnPlatIdx, this.pnlDnPlatThumb, this.pnlDnPlatClr, this.cmbDnPlatClr
            });

            ((System.ComponentModel.ISupportInitialize)(this.nudUpSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpPlatIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDnPlatIdx)).EndInit();
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
        private System.Windows.Forms.Label         lblUpMsgType;
        private System.Windows.Forms.RadioButton   rdoUpGeneral;
        private System.Windows.Forms.RadioButton   rdoUpPreRec;
        private System.Windows.Forms.Label         lblUpMsg;
        private System.Windows.Forms.TextBox       txtUpMsg;
        private System.Windows.Forms.Button        btnUpMsgBrowse;
        private System.Windows.Forms.Button        btnUpMsgShow;
        private System.Windows.Forms.Button        btnUpMsgAdd;
        private System.Windows.Forms.Label         lblUpFont;
        private System.Windows.Forms.ComboBox      cmbUpFontSize;
        private System.Windows.Forms.ComboBox      cmbUpFontStyle;
        private System.Windows.Forms.Panel         pnlUpColor;
        private System.Windows.Forms.ComboBox      cmbUpColor;
        private System.Windows.Forms.ComboBox      cmbUpLevel;
        private System.Windows.Forms.Label         lblUpAction;
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
        private System.Windows.Forms.Label         lblDnMsgType;
        private System.Windows.Forms.RadioButton   rdoDnGeneral;
        private System.Windows.Forms.RadioButton   rdoDnPreRec;
        private System.Windows.Forms.Label         lblDnMsg;
        private System.Windows.Forms.TextBox       txtDnMsg;
        private System.Windows.Forms.Button        btnDnMsgBrowse;
        private System.Windows.Forms.Button        btnDnMsgShow;
        private System.Windows.Forms.Button        btnDnMsgAdd;
        private System.Windows.Forms.Label         lblDnFont;
        private System.Windows.Forms.ComboBox      cmbDnFontSize;
        private System.Windows.Forms.ComboBox      cmbDnFontStyle;
        private System.Windows.Forms.Panel         pnlDnColor;
        private System.Windows.Forms.ComboBox      cmbDnColor;
        private System.Windows.Forms.ComboBox      cmbDnLevel;
        private System.Windows.Forms.Label         lblDnAction;
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
        private System.Windows.Forms.Label         lblUpPlatHdr;
        private System.Windows.Forms.Label         lblUpPlatIdx;
        private System.Windows.Forms.NumericUpDown nudUpPlatIdx;
        private System.Windows.Forms.Panel         pnlUpPlatThumb;
        private System.Windows.Forms.Panel         pnlUpPlatClr;
        private System.Windows.Forms.ComboBox      cmbUpPlatClr;
        // ── 下行 Extra ───────────────────────────────────────────
        private System.Windows.Forms.Label         lblDnTimeHdr;
        private System.Windows.Forms.ComboBox      cmbDnTimeType;
        private System.Windows.Forms.Panel         pnlDnTimeClr;
        private System.Windows.Forms.ComboBox      cmbDnTimeClr;
        private System.Windows.Forms.Label         lblDnPlatHdr;
        private System.Windows.Forms.Label         lblDnPlatIdx;
        private System.Windows.Forms.NumericUpDown nudDnPlatIdx;
        private System.Windows.Forms.Panel         pnlDnPlatThumb;
        private System.Windows.Forms.Panel         pnlDnPlatClr;
        private System.Windows.Forms.ComboBox      cmbDnPlatClr;
    }
}
