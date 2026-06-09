namespace UITest.Controls
{
    partial class CountdownUnitControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle  = new System.Windows.Forms.Label();
            this.btnSend   = new System.Windows.Forms.Button();
            this.rdo1s     = new System.Windows.Forms.RadioButton();
            this.rdo5s     = new System.Windows.Forms.RadioButton();
            this.rdo10s    = new System.Windows.Forms.RadioButton();
            this.rdo1m     = new System.Windows.Forms.RadioButton();

            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 14);
            this.lblTitle.Font     = new System.Drawing.Font("微軟正黑體", 10f, System.Drawing.FontStyle.Bold);
            this.lblTitle.Text     = "倒數時間單位設定";

            this.btnSend.Location = new System.Drawing.Point(12, 44);
            this.btnSend.Size     = new System.Drawing.Size(60, 26);
            this.btnSend.Text     = "傳送";
            this.btnSend.Click   += new System.EventHandler(this.btnSend_Click);

            // 4 個 RadioButton 橫排（y=86）
            this.rdo1s.AutoSize = true;
            this.rdo1s.Location = new System.Drawing.Point(12, 86);
            this.rdo1s.Text     = "1 秒";

            this.rdo5s.AutoSize = true;
            this.rdo5s.Location = new System.Drawing.Point(80, 86);
            this.rdo5s.Text     = "5 秒";
            this.rdo5s.Checked  = true;

            this.rdo10s.AutoSize = true;
            this.rdo10s.Location = new System.Drawing.Point(152, 86);
            this.rdo10s.Text     = "10 秒";

            this.rdo1m.AutoSize = true;
            this.rdo1m.Location = new System.Drawing.Point(232, 86);
            this.rdo1m.Text     = "1 分鐘";

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(360, 125);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblTitle, this.btnSend,
                this.rdo1s, this.rdo5s, this.rdo10s, this.rdo1m
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label        lblTitle;
        private System.Windows.Forms.Button       btnSend;
        private System.Windows.Forms.RadioButton  rdo1s;
        private System.Windows.Forms.RadioButton  rdo5s;
        private System.Windows.Forms.RadioButton  rdo10s;
        private System.Windows.Forms.RadioButton  rdo1m;
    }
}
