namespace UITest.Controls
{
    partial class PacketBuilderControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── 輸入欄位 ──
            this.lblColor       = new System.Windows.Forms.Label();
            this.txtColor       = new System.Windows.Forms.TextBox();
            this.lblContent     = new System.Windows.Forms.Label();
            this.txtContent     = new System.Windows.Forms.TextBox();
            this.lblScrollMode  = new System.Windows.Forms.Label();
            this.cmbScrollMode  = new System.Windows.Forms.ComboBox();
            this.lblScrollSpeed = new System.Windows.Forms.Label();
            this.nudScrollSpeed = new System.Windows.Forms.NumericUpDown();
            this.lblPauseTime   = new System.Windows.Forms.Label();
            this.nudPauseTime   = new System.Windows.Forms.NumericUpDown();
            this.lblLevel       = new System.Windows.Forms.Label();
            this.nudLevel       = new System.Windows.Forms.NumericUpDown();
            this.lblFontSize    = new System.Windows.Forms.Label();
            this.cmbFontSize    = new System.Windows.Forms.ComboBox();
            this.lblFontStyle   = new System.Windows.Forms.Label();
            this.cmbFontStyle   = new System.Windows.Forms.ComboBox();
            this.lblIDs         = new System.Windows.Forms.Label();
            this.txtIDs         = new System.Windows.Forms.TextBox();
            this.lblFuncCode    = new System.Windows.Forms.Label();
            this.cmbFuncCode    = new System.Windows.Forms.ComboBox();
            // ── 步驟按鈕 ──
            this.btnStep1       = new System.Windows.Forms.Button();
            this.btnStep2       = new System.Windows.Forms.Button();
            this.btnStep3       = new System.Windows.Forms.Button();
            this.btnStep4       = new System.Windows.Forms.Button();
            this.btnStep5       = new System.Windows.Forms.Button();
            this.btnBuildAll    = new System.Windows.Forms.Button();
            this.btnClear       = new System.Windows.Forms.Button();
            this.btnCopy        = new System.Windows.Forms.Button();
            // ── 輸出 ──
            this.lblOutput      = new System.Windows.Forms.Label();
            this.txtOutput      = new System.Windows.Forms.TextBox();

            ((System.ComponentModel.ISupportInitialize)(this.nudScrollSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPauseTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevel)).BeginInit();
            this.SuspendLayout();

            // ════════════════════════════════════════════════
            // 輸入欄位（左欄 x=10, 控制項 x=90）
            // ════════════════════════════════════════════════

            // 顏色 (y=10)
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(10, 14);
            this.lblColor.Text     = "顏色(Hex)";
            this.txtColor.Location = new System.Drawing.Point(90, 10);
            this.txtColor.Size     = new System.Drawing.Size(150, 21);
            this.txtColor.Text     = "FF0000";

            // 訊息內容 (y=38)
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(10, 42);
            this.lblContent.Text     = "訊息內容";
            this.txtContent.Location = new System.Drawing.Point(90, 38);
            this.txtContent.Size     = new System.Drawing.Size(150, 21);

            // ScrollMode (y=66)
            this.lblScrollMode.AutoSize = true;
            this.lblScrollMode.Location = new System.Drawing.Point(10, 70);
            this.lblScrollMode.Text     = "ScrollMode";
            this.cmbScrollMode.Location      = new System.Drawing.Point(90, 66);
            this.cmbScrollMode.Size          = new System.Drawing.Size(150, 21);
            this.cmbScrollMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScrollMode.Items.AddRange(new object[]
            {
                "Jump", "ScrollLeft", "ScrollLeftWithDisappear",
                "ScrollLeftWithFadeOut", "ScrollDown", "ScrollUp", "Flash"
            });
            this.cmbScrollMode.SelectedIndex = 3;

            // ScrollSpeed (y=94)
            this.lblScrollSpeed.AutoSize = true;
            this.lblScrollSpeed.Location = new System.Drawing.Point(10, 98);
            this.lblScrollSpeed.Text     = "ScrollSpeed";
            this.nudScrollSpeed.Location = new System.Drawing.Point(90, 94);
            this.nudScrollSpeed.Size     = new System.Drawing.Size(60, 21);
            this.nudScrollSpeed.Minimum  = 0;
            this.nudScrollSpeed.Maximum  = 9;
            this.nudScrollSpeed.Value    = 7;

            // PauseTime (y=122)
            this.lblPauseTime.AutoSize = true;
            this.lblPauseTime.Location = new System.Drawing.Point(10, 126);
            this.lblPauseTime.Text     = "PauseTime";
            this.nudPauseTime.Location = new System.Drawing.Point(90, 122);
            this.nudPauseTime.Size     = new System.Drawing.Size(60, 21);
            this.nudPauseTime.Minimum  = 0;
            this.nudPauseTime.Maximum  = 15;
            this.nudPauseTime.Value    = 10;

            // Level (y=150)
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(10, 154);
            this.lblLevel.Text     = "Level";
            this.nudLevel.Location = new System.Drawing.Point(90, 150);
            this.nudLevel.Size     = new System.Drawing.Size(60, 21);
            this.nudLevel.Minimum  = 1;
            this.nudLevel.Maximum  = 4;
            this.nudLevel.Value    = 3;

            // FontSize (y=178)
            this.lblFontSize.AutoSize = true;
            this.lblFontSize.Location = new System.Drawing.Point(10, 182);
            this.lblFontSize.Text     = "FontSize";
            this.cmbFontSize.Location      = new System.Drawing.Point(90, 178);
            this.cmbFontSize.Size          = new System.Drawing.Size(150, 21);
            this.cmbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFontSize.Items.AddRange(new object[] { "Font16x16", "Font24x24", "Font5x7" });
            this.cmbFontSize.SelectedIndex = 1;

            // FontStyle (y=206)
            this.lblFontStyle.AutoSize = true;
            this.lblFontStyle.Location = new System.Drawing.Point(10, 210);
            this.lblFontStyle.Text     = "FontStyle";
            this.cmbFontStyle.Location      = new System.Drawing.Point(90, 206);
            this.cmbFontStyle.Size          = new System.Drawing.Size(150, 21);
            this.cmbFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFontStyle.Items.AddRange(new object[] { "Ming", "Hei", "Kai" });
            this.cmbFontStyle.SelectedIndex = 0;

            // IDs (y=234)
            this.lblIDs.AutoSize = true;
            this.lblIDs.Location = new System.Drawing.Point(10, 238);
            this.lblIDs.Text     = "IDs (Hex)";
            this.txtIDs.Location = new System.Drawing.Point(90, 234);
            this.txtIDs.Size     = new System.Drawing.Size(150, 21);
            this.txtIDs.Text     = "11 12";

            // FuncCode (y=262)
            this.lblFuncCode.AutoSize = true;
            this.lblFuncCode.Location = new System.Drawing.Point(10, 266);
            this.lblFuncCode.Text     = "FuncCode";
            this.cmbFuncCode.Location      = new System.Drawing.Point(90, 262);
            this.cmbFuncCode.Size          = new System.Drawing.Size(150, 21);
            this.cmbFuncCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFuncCode.Items.AddRange(new object[]
            {
                "0x31 通訊測試", "0x32 參數設定", "0x33 開關機",
                "0x34 旅客訊息", "0x35 預錄資料庫", "0x36 字型更新",
                "0x37 設備通訊", "0x38 緊急訊息"
            });
            this.cmbFuncCode.SelectedIndex = 3;

            // ════════════════════════════════════════════════
            // 步驟按鈕（右欄 x=280）
            // ════════════════════════════════════════════════

            this.btnStep1.Location = new System.Drawing.Point(280, 10);
            this.btnStep1.Size     = new System.Drawing.Size(140, 26);
            this.btnStep1.Text     = "1. StringBody";
            this.btnStep1.Click   += new System.EventHandler(this.btnStep1_Click);

            this.btnStep2.Location = new System.Drawing.Point(280, 44);
            this.btnStep2.Size     = new System.Drawing.Size(140, 26);
            this.btnStep2.Text     = "2. StringMessage";
            this.btnStep2.Click   += new System.EventHandler(this.btnStep2_Click);

            this.btnStep3.Location = new System.Drawing.Point(280, 78);
            this.btnStep3.Size     = new System.Drawing.Size(140, 26);
            this.btnStep3.Text     = "3. FullWindow";
            this.btnStep3.Click   += new System.EventHandler(this.btnStep3_Click);

            this.btnStep4.Location = new System.Drawing.Point(280, 112);
            this.btnStep4.Size     = new System.Drawing.Size(140, 26);
            this.btnStep4.Text     = "4. Sequence";
            this.btnStep4.Click   += new System.EventHandler(this.btnStep4_Click);

            this.btnStep5.Location = new System.Drawing.Point(280, 146);
            this.btnStep5.Size     = new System.Drawing.Size(140, 26);
            this.btnStep5.Text     = "5. Packet";
            this.btnStep5.Click   += new System.EventHandler(this.btnStep5_Click);

            this.btnBuildAll.Location  = new System.Drawing.Point(280, 214);
            this.btnBuildAll.Size      = new System.Drawing.Size(140, 26);
            this.btnBuildAll.Text      = "▶ 一鍵組成";
            this.btnBuildAll.BackColor = System.Drawing.Color.FromArgb(220, 240, 220);
            this.btnBuildAll.Click    += new System.EventHandler(this.btnBuildAll_Click);

            this.btnClear.Location = new System.Drawing.Point(280, 248);
            this.btnClear.Size     = new System.Drawing.Size(140, 26);
            this.btnClear.Text     = "清除";
            this.btnClear.Click   += new System.EventHandler(this.btnClear_Click);

            this.btnCopy.Location = new System.Drawing.Point(280, 282);
            this.btnCopy.Size     = new System.Drawing.Size(140, 26);
            this.btnCopy.Text     = "複製輸出";
            this.btnCopy.Click   += new System.EventHandler(this.btnCopy_Click);

            // ════════════════════════════════════════════════
            // 輸出區
            // ════════════════════════════════════════════════
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(10, 294);
            this.lblOutput.Text     = "輸出結果（Hex）";

            this.txtOutput.Location   = new System.Drawing.Point(10, 310);
            this.txtOutput.Multiline  = true;
            this.txtOutput.Size       = new System.Drawing.Size(600, 180);
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.ReadOnly   = true;
            this.txtOutput.Font       = new System.Drawing.Font("Courier New", 9F);

            // ════════════════════════════════════════════════
            // PacketBuilderControl 本身
            // ════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(640, 510);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblColor,      this.txtColor,
                this.lblContent,    this.txtContent,
                this.lblScrollMode, this.cmbScrollMode,
                this.lblScrollSpeed, this.nudScrollSpeed,
                this.lblPauseTime,  this.nudPauseTime,
                this.lblLevel,      this.nudLevel,
                this.lblFontSize,   this.cmbFontSize,
                this.lblFontStyle,  this.cmbFontStyle,
                this.lblIDs,        this.txtIDs,
                this.lblFuncCode,   this.cmbFuncCode,
                this.btnStep1, this.btnStep2, this.btnStep3, this.btnStep4, this.btnStep5,
                this.btnBuildAll, this.btnClear, this.btnCopy,
                this.lblOutput, this.txtOutput
            });

            ((System.ComponentModel.ISupportInitialize)(this.nudScrollSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPauseTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ── 輸入 ─────────────────────────────────────────────────
        private System.Windows.Forms.Label         lblColor;
        private System.Windows.Forms.TextBox       txtColor;
        private System.Windows.Forms.Label         lblContent;
        private System.Windows.Forms.TextBox       txtContent;
        private System.Windows.Forms.Label         lblScrollMode;
        private System.Windows.Forms.ComboBox      cmbScrollMode;
        private System.Windows.Forms.Label         lblScrollSpeed;
        private System.Windows.Forms.NumericUpDown nudScrollSpeed;
        private System.Windows.Forms.Label         lblPauseTime;
        private System.Windows.Forms.NumericUpDown nudPauseTime;
        private System.Windows.Forms.Label         lblLevel;
        private System.Windows.Forms.NumericUpDown nudLevel;
        private System.Windows.Forms.Label         lblFontSize;
        private System.Windows.Forms.ComboBox      cmbFontSize;
        private System.Windows.Forms.Label         lblFontStyle;
        private System.Windows.Forms.ComboBox      cmbFontStyle;
        private System.Windows.Forms.Label         lblIDs;
        private System.Windows.Forms.TextBox       txtIDs;
        private System.Windows.Forms.Label         lblFuncCode;
        private System.Windows.Forms.ComboBox      cmbFuncCode;
        // ── 按鈕 ─────────────────────────────────────────────────
        private System.Windows.Forms.Button        btnStep1;
        private System.Windows.Forms.Button        btnStep2;
        private System.Windows.Forms.Button        btnStep3;
        private System.Windows.Forms.Button        btnStep4;
        private System.Windows.Forms.Button        btnStep5;
        private System.Windows.Forms.Button        btnBuildAll;
        private System.Windows.Forms.Button        btnClear;
        private System.Windows.Forms.Button        btnCopy;
        // ── 輸出 ─────────────────────────────────────────────────
        private System.Windows.Forms.Label         lblOutput;
        private System.Windows.Forms.TextBox       txtOutput;
    }
}
