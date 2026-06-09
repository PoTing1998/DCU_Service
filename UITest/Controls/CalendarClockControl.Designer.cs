namespace UITest.Controls
{
    partial class CalendarClockControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpSet        = new System.Windows.Forms.GroupBox();
            this.btnSend       = new System.Windows.Forms.Button();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.grpGet        = new System.Windows.Forms.GroupBox();
            this.btnRead       = new System.Windows.Forms.Button();
            this.lblMCUTime    = new System.Windows.Forms.Label();

            this.grpSet.SuspendLayout();
            this.grpGet.SuspendLayout();
            this.SuspendLayout();

            // ── grpSet：萬年曆IC時鐘設定 ─────────────────────────────────
            this.grpSet.Location = new System.Drawing.Point(8, 8);
            this.grpSet.Size     = new System.Drawing.Size(480, 90);
            this.grpSet.Text     = "萬年曆IC時鐘設定";

            this.btnSend.Location = new System.Drawing.Point(8, 24);
            this.btnSend.Size     = new System.Drawing.Size(60, 26);
            this.btnSend.Text     = "傳送";
            this.btnSend.Click   += new System.EventHandler(this.btnSend_Click);

            this.lblCurrentTime.AutoSize  = true;
            this.lblCurrentTime.Location  = new System.Drawing.Point(8, 58);
            this.lblCurrentTime.ForeColor = System.Drawing.Color.Red;
            this.lblCurrentTime.Text      = "現在時間：--";

            this.grpSet.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.btnSend, this.lblCurrentTime
            });

            // ── grpGet：取得MCU萬年曆IC時鐘值 ───────────────────────────
            this.grpGet.Location = new System.Drawing.Point(8, 106);
            this.grpGet.Size     = new System.Drawing.Size(480, 80);
            this.grpGet.Text     = "取得MCU萬年曆IC時鐘值";

            this.btnRead.Location = new System.Drawing.Point(8, 22);
            this.btnRead.Size     = new System.Drawing.Size(60, 26);
            this.btnRead.Text     = "讀取";
            this.btnRead.Click   += new System.EventHandler(this.btnRead_Click);

            this.lblMCUTime.AutoSize  = true;
            this.lblMCUTime.Location  = new System.Drawing.Point(8, 54);
            this.lblMCUTime.ForeColor = System.Drawing.Color.Red;
            this.lblMCUTime.Text      = "MCU System Clock";

            this.grpGet.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.btnRead, this.lblMCUTime
            });

            // ── UserControl ───────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(500, 200);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.grpSet, this.grpGet
            });

            this.grpSet.ResumeLayout(false);
            this.grpSet.PerformLayout();
            this.grpGet.ResumeLayout(false);
            this.grpGet.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox  grpSet;
        private System.Windows.Forms.Button    btnSend;
        private System.Windows.Forms.Label     lblCurrentTime;
        private System.Windows.Forms.GroupBox  grpGet;
        private System.Windows.Forms.Button    btnRead;
        private System.Windows.Forms.Label     lblMCUTime;
    }
}
