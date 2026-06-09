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
            this.tabControl       = new System.Windows.Forms.TabControl();
            this.tabSerial        = new System.Windows.Forms.TabPage();
            this.tabDisplayType   = new System.Windows.Forms.TabPage();
            this.tabSysCtrl       = new System.Windows.Forms.TabPage();
            this.serialSettingCtrl   = new SerialSettingControl();
            this.displayMessageCtrl  = new DisplayMessageControl();
            this.sysCtrlPanel        = new SystemControlPanel();

            this.tabControl.SuspendLayout();
            this.tabSerial.SuspendLayout();
            this.tabSysCtrl.SuspendLayout();
            this.SuspendLayout();

            // ── tabControl（外層）────────────────────────────
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Controls.AddRange(new System.Windows.Forms.TabPage[]
            {
                this.tabSerial,
                this.tabDisplayType,
                this.tabSysCtrl
            });

            // ── 串列埠設定 ───────────────────────────────────
            this.tabSerial.Text    = "串列埠設定";
            this.tabSerial.Padding = new System.Windows.Forms.Padding(5);
            this.tabSerial.Controls.Add(this.serialSettingCtrl);
            this.serialSettingCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── 顯示器板型 ───────────────────────────────────
            this.tabDisplayType.Text    = "顯示器板型";
            this.tabDisplayType.Padding = new System.Windows.Forms.Padding(5);
            this.tabDisplayType.AutoScroll = true;
            this.tabDisplayType.Controls.Add(this.displayMessageCtrl);
            this.displayMessageCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── 系統控制 ─────────────────────────────────────
            this.tabSysCtrl.Text    = "系統控制";
            this.tabSysCtrl.Padding = new System.Windows.Forms.Padding(0);
            this.tabSysCtrl.Controls.Add(this.sysCtrlPanel);
            this.sysCtrlPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── Form1 ────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.tabControl);
            this.Name = "Form1";
            this.Text = "DCU UITest";

            this.tabControl.ResumeLayout(false);
            this.tabSerial.ResumeLayout(false);
            this.tabSysCtrl.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label MakePlaceholder()
        {
            return new System.Windows.Forms.Label
            {
                AutoSize  = true,
                Location  = new System.Drawing.Point(16, 16),
                Text      = "（功能開發中）",
                ForeColor = System.Drawing.Color.Gray
            };
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage    tabSerial;
        private System.Windows.Forms.TabPage    tabDisplayType;
        private System.Windows.Forms.TabPage    tabSysCtrl;
        private SerialSettingControl            serialSettingCtrl;
        private DisplayMessageControl           displayMessageCtrl;
        private SystemControlPanel              sysCtrlPanel;
    }
}
