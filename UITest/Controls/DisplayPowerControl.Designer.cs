namespace UITest.Controls
{
    partial class DisplayPowerControl
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
            this.rdoOff    = new System.Windows.Forms.RadioButton();
            this.rdoOn     = new System.Windows.Forms.RadioButton();

            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 14);
            this.lblTitle.Font     = new System.Drawing.Font("微軟正黑體", 10f, System.Drawing.FontStyle.Bold);
            this.lblTitle.Text     = "顯示器開關機設定";

            this.btnSend.Location = new System.Drawing.Point(12, 44);
            this.btnSend.Size     = new System.Drawing.Size(60, 26);
            this.btnSend.Text     = "傳送";
            this.btnSend.Click   += new System.EventHandler(this.btnSend_Click);

            this.rdoOff.AutoSize = true;
            this.rdoOff.Location = new System.Drawing.Point(12, 86);
            this.rdoOff.Text     = "關閉顯示器";

            this.rdoOn.AutoSize = true;
            this.rdoOn.Location = new System.Drawing.Point(12, 112);
            this.rdoOn.Text     = "打開顯示器";
            this.rdoOn.Checked  = true;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(300, 160);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblTitle, this.btnSend, this.rdoOff, this.rdoOn
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label        lblTitle;
        private System.Windows.Forms.Button       btnSend;
        private System.Windows.Forms.RadioButton  rdoOff;
        private System.Windows.Forms.RadioButton  rdoOn;
    }
}
