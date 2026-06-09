namespace UITest.Controls
{
    partial class PreRecordControl
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
            this.txtHex      = new System.Windows.Forms.TextBox();
            this.btnSend     = new System.Windows.Forms.Button();
            this.btnAscii    = new System.Windows.Forms.Button();
            this.lblStatus   = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(10, 12);
            this.lblTitle.Text     = "預錄封包直接傳送MCU(採16進位方式編碼)";

            // txtHex
            this.txtHex.Location  = new System.Drawing.Point(10, 32);
            this.txtHex.Size      = new System.Drawing.Size(700, 21);
            this.txtHex.Font      = new System.Drawing.Font("Courier New", 9F);
            this.txtHex.Text      = "";

            // btnSend
            this.btnSend.Location = new System.Drawing.Point(10, 62);
            this.btnSend.Size     = new System.Drawing.Size(60, 26);
            this.btnSend.Text     = "傳送";
            this.btnSend.Click   += new System.EventHandler(this.btnSend_Click);

            // btnAscii
            this.btnAscii.Location = new System.Drawing.Point(78, 62);
            this.btnAscii.Size     = new System.Drawing.Size(80, 26);
            this.btnAscii.Text     = "ASCII Code";
            this.btnAscii.Click   += new System.EventHandler(this.btnAscii_Click);

            // lblStatus
            this.lblStatus.AutoSize  = true;
            this.lblStatus.Location  = new System.Drawing.Point(170, 67);
            this.lblStatus.Text      = "";
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;

            // PreRecordControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(730, 110);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtHex);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnAscii);
            this.Controls.Add(this.lblStatus);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label   lblTitle;
        private System.Windows.Forms.TextBox txtHex;
        private System.Windows.Forms.Button  btnSend;
        private System.Windows.Forms.Button  btnAscii;
        private System.Windows.Forms.Label   lblStatus;
    }
}
