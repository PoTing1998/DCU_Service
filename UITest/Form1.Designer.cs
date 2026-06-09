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
            this.pnlToolbar          = new System.Windows.Forms.Panel();
            this.chkShowLogWindow    = new System.Windows.Forms.CheckBox();
            this.tabControl          = new System.Windows.Forms.TabControl();
            this.tabSerial           = new System.Windows.Forms.TabPage();
            this.tabDisplayType      = new System.Windows.Forms.TabPage();
            this.tabSysCtrl          = new System.Windows.Forms.TabPage();
            this.serialSettingCtrl   = new SerialSettingControl();
            this.displayMessageCtrl  = new DisplayMessageControl();
            this.sysCtrlPanel        = new SystemControlPanel();

            this.pnlToolbar.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabSerial.SuspendLayout();
            this.tabSysCtrl.SuspendLayout();
            this.SuspendLayout();

            // ── 頂部工具列 ───────────────────────────────────────────
            this.pnlToolbar.Dock      = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Height    = 32;
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(235, 235, 240);
            this.pnlToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.chkShowLogWindow.AutoSize = true;
            this.chkShowLogWindow.Location = new System.Drawing.Point(8, 8);
            this.chkShowLogWindow.Text     = "顯示狀態視窗";
            this.chkShowLogWindow.CheckedChanged +=
                new System.EventHandler(this.chkShowLogWindow_CheckedChanged);

            this.pnlToolbar.Controls.Add(this.chkShowLogWindow);

            // ── tabControl（外層）─────────────────────────────────────
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Controls.AddRange(new System.Windows.Forms.TabPage[]
            {
                this.tabSerial,
                this.tabDisplayType,
                this.tabSysCtrl
            });

            // ── 串列埠設定 ────────────────────────────────────────────
            this.tabSerial.Text    = "串列埠設定";
            this.tabSerial.Padding = new System.Windows.Forms.Padding(5);
            this.tabSerial.Controls.Add(this.serialSettingCtrl);
            this.serialSettingCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── 顯示器板型 ────────────────────────────────────────────
            this.tabDisplayType.Text       = "顯示器板型";
            this.tabDisplayType.Padding    = new System.Windows.Forms.Padding(5);
            this.tabDisplayType.AutoScroll = true;
            this.tabDisplayType.Controls.Add(this.displayMessageCtrl);
            this.displayMessageCtrl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── 系統控制 ──────────────────────────────────────────────
            this.tabSysCtrl.Text    = "系統控制";
            this.tabSysCtrl.Padding = new System.Windows.Forms.Padding(0);
            this.tabSysCtrl.Controls.Add(this.sysCtrlPanel);
            this.sysCtrlPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── Form1 ─────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1200, 732);
            this.Controls.Add(this.tabControl);   // Fill 先加
            this.Controls.Add(this.pnlToolbar);   // Top 後加（壓在 Fill 上方）
            this.Name = "Form1";
            this.Text = "DCU UITest";

            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabSerial.ResumeLayout(false);
            this.tabSysCtrl.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel          pnlToolbar;
        private System.Windows.Forms.CheckBox       chkShowLogWindow;
        private System.Windows.Forms.TabControl     tabControl;
        private System.Windows.Forms.TabPage        tabSerial;
        private System.Windows.Forms.TabPage        tabDisplayType;
        private System.Windows.Forms.TabPage        tabSysCtrl;
        private SerialSettingControl                 serialSettingCtrl;
        private DisplayMessageControl                displayMessageCtrl;
        private SystemControlPanel                   sysCtrlPanel;
    }
}
