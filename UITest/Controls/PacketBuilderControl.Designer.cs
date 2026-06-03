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
            this.txtInput  = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.button1   = new System.Windows.Forms.Button();
            this.button2   = new System.Windows.Forms.Button();
            this.button3   = new System.Windows.Forms.Button();
            this.button4   = new System.Windows.Forms.Button();
            this.button5   = new System.Windows.Forms.Button();
            this.lblInput  = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblInput
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(10, 8);
            this.lblInput.Text = "輸入（字體顏色 / 字體內容）";

            // txtInput
            this.txtInput.Location  = new System.Drawing.Point(10, 25);
            this.txtInput.Multiline = true;
            this.txtInput.Size      = new System.Drawing.Size(320, 120);
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // button1 ~ button5（垂直排列）
            this.button1.Location = new System.Drawing.Point(345, 25);
            this.button1.Size     = new System.Drawing.Size(110, 28);
            this.button1.Text     = "1. StringBody";
            this.button1.Click   += new System.EventHandler(this.button1_Click);

            this.button2.Location = new System.Drawing.Point(345, 60);
            this.button2.Size     = new System.Drawing.Size(110, 28);
            this.button2.Text     = "2. StringMessage";
            this.button2.Click   += new System.EventHandler(this.button2_Click);

            this.button3.Location = new System.Drawing.Point(345, 95);
            this.button3.Size     = new System.Drawing.Size(110, 28);
            this.button3.Text     = "3. FullWindow";
            this.button3.Click   += new System.EventHandler(this.button3_Click);

            this.button4.Location = new System.Drawing.Point(345, 130);
            this.button4.Size     = new System.Drawing.Size(110, 28);
            this.button4.Text     = "4. Sequence";
            this.button4.Click   += new System.EventHandler(this.button4_Click);

            this.button5.Location = new System.Drawing.Point(345, 165);
            this.button5.Size     = new System.Drawing.Size(110, 28);
            this.button5.Text     = "5. Packet";
            this.button5.Click   += new System.EventHandler(this.button5_Click);

            // lblOutput
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(10, 155);
            this.lblOutput.Text     = "輸出結果";

            // txtOutput
            this.txtOutput.Location   = new System.Drawing.Point(10, 172);
            this.txtOutput.Multiline  = true;
            this.txtOutput.Size       = new System.Drawing.Size(445, 200);
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.ReadOnly   = true;

            // PacketBuilderControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(480, 390);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.txtOutput);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button  button1;
        private System.Windows.Forms.Button  button2;
        private System.Windows.Forms.Button  button3;
        private System.Windows.Forms.Button  button4;
        private System.Windows.Forms.Button  button5;
        private System.Windows.Forms.Label   lblInput;
        private System.Windows.Forms.Label   lblOutput;
    }
}
