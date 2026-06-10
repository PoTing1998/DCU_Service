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
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.chkShowLogWindow = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSerial = new System.Windows.Forms.TabPage();
            this.serialSettingCtrl = new UITest.Controls.SerialSettingControl();
            this.tabDisplayType = new System.Windows.Forms.TabPage();
            this.displayMessageCtrl = new UITest.Controls.DisplayMessageControl();
            this.tabSysCtrl = new System.Windows.Forms.TabPage();
            this.sysCtrlPanel = new UITest.Controls.SystemControlPanel();
            this.pnlToolbar.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabSerial.SuspendLayout();
            this.tabDisplayType.SuspendLayout();
            this.tabSysCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(240)))));
            this.pnlToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlToolbar.Controls.Add(this.chkShowLogWindow);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(1200, 32);
            this.pnlToolbar.TabIndex = 1;
            // 
            // chkShowLogWindow
            // 
            this.chkShowLogWindow.AutoSize = true;
            this.chkShowLogWindow.Location = new System.Drawing.Point(8, 8);
            this.chkShowLogWindow.Name = "chkShowLogWindow";
            this.chkShowLogWindow.Size = new System.Drawing.Size(96, 16);
            this.chkShowLogWindow.TabIndex = 0;
            this.chkShowLogWindow.Text = "顯示狀態視窗";
            this.chkShowLogWindow.CheckedChanged += new System.EventHandler(this.chkShowLogWindow_CheckedChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSerial);
            this.tabControl.Controls.Add(this.tabDisplayType);
            this.tabControl.Controls.Add(this.tabSysCtrl);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 32);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1200, 700);
            this.tabControl.TabIndex = 0;
            // 
            // tabSerial
            // 
            this.tabSerial.Controls.Add(this.serialSettingCtrl);
            this.tabSerial.Location = new System.Drawing.Point(4, 22);
            this.tabSerial.Name = "tabSerial";
            this.tabSerial.Padding = new System.Windows.Forms.Padding(5);
            this.tabSerial.Size = new System.Drawing.Size(1192, 674);
            this.tabSerial.TabIndex = 0;
            this.tabSerial.Text = "串列埠設定";
            // 
            // serialSettingCtrl
            // 
            this.serialSettingCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serialSettingCtrl.Location = new System.Drawing.Point(5, 5);
            this.serialSettingCtrl.Name = "serialSettingCtrl";
            this.serialSettingCtrl.Size = new System.Drawing.Size(1182, 664);
            this.serialSettingCtrl.TabIndex = 0;
            // 
            // tabDisplayType
            // 
            this.tabDisplayType.AutoScroll = true;
            this.tabDisplayType.Controls.Add(this.displayMessageCtrl);
            this.tabDisplayType.Location = new System.Drawing.Point(4, 22);
            this.tabDisplayType.Name = "tabDisplayType";
            this.tabDisplayType.Padding = new System.Windows.Forms.Padding(5);
            this.tabDisplayType.Size = new System.Drawing.Size(1192, 674);
            this.tabDisplayType.TabIndex = 1;
            this.tabDisplayType.Text = "顯示器板型";
            // 
            // displayMessageCtrl
            // 
            this.displayMessageCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayMessageCtrl.GetDnIDsFunc = null;
            this.displayMessageCtrl.GetUpIDsFunc = null;
            this.displayMessageCtrl.Location = new System.Drawing.Point(5, 5);
            this.displayMessageCtrl.Name = "displayMessageCtrl";
            this.displayMessageCtrl.SendAction = null;
            this.displayMessageCtrl.Size = new System.Drawing.Size(1182, 664);
            this.displayMessageCtrl.TabIndex = 0;
            // 
            // tabSysCtrl
            // 
            this.tabSysCtrl.Controls.Add(this.sysCtrlPanel);
            this.tabSysCtrl.Location = new System.Drawing.Point(4, 22);
            this.tabSysCtrl.Name = "tabSysCtrl";
            this.tabSysCtrl.Size = new System.Drawing.Size(192, 74);
            this.tabSysCtrl.TabIndex = 2;
            this.tabSysCtrl.Text = "系統控制";
            // 
            // sysCtrlPanel
            // 
            this.sysCtrlPanel.AlarmLightSendAction = null;
            this.sysCtrlPanel.BrightnessSendAction = null;
            this.sysCtrlPanel.CalendarReadAction = null;
            this.sysCtrlPanel.CalendarSendAction = null;
            this.sysCtrlPanel.CommTimeoutSendAction = null;
            this.sysCtrlPanel.CountdownUnitSendAction = null;
            this.sysCtrlPanel.DisplayModeSendAction = null;
            this.sysCtrlPanel.DisplayPowerSendAction = null;
            this.sysCtrlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysCtrlPanel.Location = new System.Drawing.Point(0, 0);
            this.sysCtrlPanel.Name = "sysCtrlPanel";
            this.sysCtrlPanel.PreRecordSendAction = null;
            this.sysCtrlPanel.Size = new System.Drawing.Size(192, 74);
            this.sysCtrlPanel.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 732);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlToolbar);
            this.Name = "Form1";
            this.Text = "DCU UITest";
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabSerial.ResumeLayout(false);
            this.tabDisplayType.ResumeLayout(false);
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
