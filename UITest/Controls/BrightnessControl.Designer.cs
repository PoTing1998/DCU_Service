namespace UITest.Controls
{
    partial class BrightnessControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle     = new System.Windows.Forms.Label();
            this.btnSend      = new System.Windows.Forms.Button();
            this.rdoSensor    = new System.Windows.Forms.RadioButton();
            this.rdoManual    = new System.Windows.Forms.RadioButton();
            this.lblManual    = new System.Windows.Forms.Label();
            this.nudBrightness = new System.Windows.Forms.NumericUpDown();
            this.lblHint      = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.nudBrightness)).BeginInit();
            this.SuspendLayout();

            // ── 標題 ──────────────────────────────────────────────────────
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 14);
            this.lblTitle.Font     = new System.Drawing.Font("微軟正黑體", 10f, System.Drawing.FontStyle.Bold);
            this.lblTitle.Text     = "顯示器亮度設定";

            // ── 傳送按鈕 ─────────────────────────────────────────────────
            this.btnSend.Location = new System.Drawing.Point(12, 44);
            this.btnSend.Size     = new System.Drawing.Size(60, 26);
            this.btnSend.Text     = "傳送";
            this.btnSend.Click   += new System.EventHandler(this.btnSend_Click);

            // ── RadioButton ───────────────────────────────────────────────
            this.rdoSensor.AutoSize = true;
            this.rdoSensor.Location = new System.Drawing.Point(12, 86);
            this.rdoSensor.Text     = "Sensor自動亮度調整";
            this.rdoSensor.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);

            this.rdoManual.AutoSize = true;
            this.rdoManual.Location = new System.Drawing.Point(12, 112);
            this.rdoManual.Text     = "手動亮度調整";
            this.rdoManual.Checked  = true;
            this.rdoManual.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);

            // ── 手動亮度控制（rdoManual 選中時顯示）────────────────────
            this.lblManual.AutoSize = true;
            this.lblManual.Location = new System.Drawing.Point(170, 89);
            this.lblManual.Text     = "手動亮度值";

            this.nudBrightness.Location = new System.Drawing.Point(248, 86);
            this.nudBrightness.Size     = new System.Drawing.Size(55, 21);
            this.nudBrightness.Minimum  = 1;
            this.nudBrightness.Maximum  = 16;
            this.nudBrightness.Value    = 12;

            this.lblHint.AutoSize  = true;
            this.lblHint.Location  = new System.Drawing.Point(170, 112);
            this.lblHint.ForeColor = System.Drawing.Color.Red;
            this.lblHint.Text      = "較亮（= 16），較暗（= 1）";

            // ── UserControl ───────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(420, 155);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblTitle, this.btnSend,
                this.rdoSensor, this.rdoManual,
                this.lblManual, this.nudBrightness, this.lblHint
            });

            ((System.ComponentModel.ISupportInitialize)(this.nudBrightness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label          lblTitle;
        private System.Windows.Forms.Button         btnSend;
        private System.Windows.Forms.RadioButton    rdoSensor;
        private System.Windows.Forms.RadioButton    rdoManual;
        private System.Windows.Forms.Label          lblManual;
        private System.Windows.Forms.NumericUpDown  nudBrightness;
        private System.Windows.Forms.Label          lblHint;
    }
}
