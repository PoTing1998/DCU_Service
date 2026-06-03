using UITest.Controls;

namespace UITest
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl         = new System.Windows.Forms.TabControl();
            this.tabBuilder         = new System.Windows.Forms.TabPage();
            this.tabVerify          = new System.Windows.Forms.TabPage();
            this.packetBuilderCtrl  = new PacketBuilderControl();
            this.packetVerifyCtrl   = new PacketVerifyControl();
            this.tabControl.SuspendLayout();
            this.tabBuilder.SuspendLayout();
            this.tabVerify.SuspendLayout();
            this.SuspendLayout();

            // tabControl
            this.tabControl.Controls.Add(this.tabBuilder);
            this.tabControl.Controls.Add(this.tabVerify);
            this.tabControl.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.TabIndex = 0;

            // tabBuilder
            this.tabBuilder.Controls.Add(this.packetBuilderCtrl);
            this.tabBuilder.Text    = "封包組成";
            this.tabBuilder.Padding = new System.Windows.Forms.Padding(5);

            // tabVerify
            this.tabVerify.Controls.Add(this.packetVerifyCtrl);
            this.tabVerify.Text    = "封包驗證";
            this.tabVerify.Padding = new System.Windows.Forms.Padding(5);

            // packetBuilderCtrl
            this.packetBuilderCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // packetVerifyCtrl
            this.packetVerifyCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1080, 680);
            this.Controls.Add(this.tabControl);
            this.Name = "Form1";
            this.Text = "DCU UITest";
            this.tabControl.ResumeLayout(false);
            this.tabBuilder.ResumeLayout(false);
            this.tabVerify.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage    tabBuilder;
        private System.Windows.Forms.TabPage    tabVerify;
        private PacketBuilderControl            packetBuilderCtrl;
        private PacketVerifyControl             packetVerifyCtrl;
    }
}
