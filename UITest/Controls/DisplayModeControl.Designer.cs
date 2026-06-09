namespace UITest.Controls
{
    partial class DisplayModeControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle    = new System.Windows.Forms.Label();
            this.lblNote     = new System.Windows.Forms.Label();
            this.btnSend     = new System.Windows.Forms.Button();
            this.rdoNormal   = new System.Windows.Forms.RadioButton();
            this.rdoTest     = new System.Windows.Forms.RadioButton();
            this.rdoIDFW     = new System.Windows.Forms.RadioButton();
            this.rdoTimeout  = new System.Windows.Forms.RadioButton();

            this.SuspendLayout();

            // ── 標題 ─────────────────────────────────────────────────────
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 14);
            this.lblTitle.Font     = new System.Drawing.Font("微軟正黑體", 10f, System.Drawing.FontStyle.Bold);
            this.lblTitle.Text     = "顯示模式設定";

            // ── 備註（紅字）─────────────────────────────────────────────
            this.lblNote.AutoSize  = true;
            this.lblNote.Location  = new System.Drawing.Point(110, 16);
            this.lblNote.ForeColor = System.Drawing.Color.Red;
            this.lblNote.Text      = "（重新開機MCU首先播放畫面設定）";

            // ── 傳送按鈕 ─────────────────────────────────────────────────
            this.btnSend.Location = new System.Drawing.Point(12, 44);
            this.btnSend.Size     = new System.Drawing.Size(60, 26);
            this.btnSend.Text     = "傳送";
            this.btnSend.Click   += new System.EventHandler(this.btnSend_Click);

            // ── 選項（4 個 RadioButton）──────────────────────────────────
            this.rdoNormal.AutoSize = true;
            this.rdoNormal.Location = new System.Drawing.Point(12, 86);
            this.rdoNormal.Text     = "正常畫面";
            this.rdoNormal.Checked  = true;

            this.rdoTest.AutoSize = true;
            this.rdoTest.Location = new System.Drawing.Point(12, 112);
            this.rdoTest.Text     = "測試畫面";

            this.rdoIDFW.AutoSize = true;
            this.rdoIDFW.Location = new System.Drawing.Point(12, 138);
            this.rdoIDFW.Text     = "顯示ID碼與FW版本";

            this.rdoTimeout.AutoSize = true;
            this.rdoTimeout.Location = new System.Drawing.Point(12, 164);
            this.rdoTimeout.Text     = "顯示通訊逾時的畫面";

            // ── UserControl ───────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(400, 220);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblTitle, this.lblNote,
                this.btnSend,
                this.rdoNormal, this.rdoTest, this.rdoIDFW, this.rdoTimeout
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label        lblTitle;
        private System.Windows.Forms.Label        lblNote;
        private System.Windows.Forms.Button       btnSend;
        private System.Windows.Forms.RadioButton  rdoNormal;
        private System.Windows.Forms.RadioButton  rdoTest;
        private System.Windows.Forms.RadioButton  rdoIDFW;
        private System.Windows.Forms.RadioButton  rdoTimeout;
    }
}
