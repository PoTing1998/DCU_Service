namespace UITest.Controls
{
    partial class CommTimeoutControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle      = new System.Windows.Forms.Label();
            this.btnSend       = new System.Windows.Forms.Button();
            this.rdoContinue   = new System.Windows.Forms.RadioButton();
            this.rdoTimeout    = new System.Windows.Forms.RadioButton();
            this.lblTimeVal    = new System.Windows.Forms.Label();
            this.nudTimeout    = new System.Windows.Forms.NumericUpDown();
            this.lblMinute     = new System.Windows.Forms.Label();
            this.lblHint       = new System.Windows.Forms.Label();
            this.rdoSleep      = new System.Windows.Forms.RadioButton();
            this.rdoPreset     = new System.Windows.Forms.RadioButton();

            ((System.ComponentModel.ISupportInitialize)(this.nudTimeout)).BeginInit();
            this.SuspendLayout();

            // ── 標題 ──────────────────────────────────────────────────────
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 14);
            this.lblTitle.Font     = new System.Drawing.Font("微軟正黑體", 10f, System.Drawing.FontStyle.Bold);
            this.lblTitle.Text     = "通訊逾時時間設定";

            // ── 傳送 ──────────────────────────────────────────────────────
            this.btnSend.Location = new System.Drawing.Point(12, 44);
            this.btnSend.Size     = new System.Drawing.Size(60, 26);
            this.btnSend.Text     = "傳送";
            this.btnSend.Click   += new System.EventHandler(this.btnSend_Click);

            // ── 左欄：模式選擇 (x=12, y=86/112) ─────────────────────────
            this.rdoContinue.AutoSize = true;
            this.rdoContinue.Location = new System.Drawing.Point(12, 86);
            this.rdoContinue.Text     = "持續原訊息顯示（不執行休眠）";
            this.rdoContinue.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);

            this.rdoTimeout.AutoSize = true;
            this.rdoTimeout.Location = new System.Drawing.Point(12, 112);
            this.rdoTimeout.Text     = "逾時時間設定（執行休眠或顯示預設訊息）";
            this.rdoTimeout.Checked  = true;
            this.rdoTimeout.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);

            // ── 中欄：逾時值（x=260, y=86/112）──────────────────────────
            this.lblTimeVal.AutoSize = true;
            this.lblTimeVal.Location = new System.Drawing.Point(265, 89);
            this.lblTimeVal.Text     = "逾時時間值";

            this.nudTimeout.Location = new System.Drawing.Point(347, 86);
            this.nudTimeout.Size     = new System.Drawing.Size(55, 21);
            this.nudTimeout.Minimum  = 1;
            this.nudTimeout.Maximum  = 60;
            this.nudTimeout.Value    = 1;

            this.lblMinute.AutoSize = true;
            this.lblMinute.Location = new System.Drawing.Point(408, 89);
            this.lblMinute.Text     = "（分鐘）";

            this.lblHint.AutoSize  = true;
            this.lblHint.Location  = new System.Drawing.Point(265, 114);
            this.lblHint.ForeColor = System.Drawing.Color.Red;
            this.lblHint.Text      = "（超過逾時時間：休眠或顯示預設訊息）";

            // ── 右欄：動作選擇 (x=490, y=86/112) ────────────────────────
            this.rdoSleep.AutoSize = true;
            this.rdoSleep.Location = new System.Drawing.Point(490, 86);
            this.rdoSleep.Text     = "休眠模式";
            this.rdoSleep.Checked  = true;

            this.rdoPreset.AutoSize = true;
            this.rdoPreset.Location = new System.Drawing.Point(490, 112);
            this.rdoPreset.Text     = "顯示預設訊息";

            // ── UserControl ───────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(660, 145);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblTitle, this.btnSend,
                this.rdoContinue, this.rdoTimeout,
                this.lblTimeVal, this.nudTimeout, this.lblMinute, this.lblHint,
                this.rdoSleep, this.rdoPreset
            });

            ((System.ComponentModel.ISupportInitialize)(this.nudTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label         lblTitle;
        private System.Windows.Forms.Button        btnSend;
        private System.Windows.Forms.RadioButton   rdoContinue;
        private System.Windows.Forms.RadioButton   rdoTimeout;
        private System.Windows.Forms.Label         lblTimeVal;
        private System.Windows.Forms.NumericUpDown nudTimeout;
        private System.Windows.Forms.Label         lblMinute;
        private System.Windows.Forms.Label         lblHint;
        private System.Windows.Forms.RadioButton   rdoSleep;
        private System.Windows.Forms.RadioButton   rdoPreset;
    }
}
