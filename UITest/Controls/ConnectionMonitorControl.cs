using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UITest.Services;
using static UITest.Services.ConnectionMonitor;

namespace UITest.Controls
{
    public partial class ConnectionMonitorControl : UserControl
    {
        private readonly Timer _pollTimer = new Timer { Interval = 5000 };

        public ConnectionMonitorControl()
        {
            InitializeComponent();
            if (DesignMode) return;

            // 訂閱 Monitor 事件
            ConnectionMonitor.Instance.OnLog          += OnNewLog;
            ConnectionMonitor.Instance.StatusRefreshed += RefreshStatusUI;
            ConnectionMonitor.Instance.ModeChanged    += _ => RefreshStatusUI();

            // 定時自動偵測（每 5 秒）
            _pollTimer.Tick += (s, e) => ConnectionMonitor.Instance.AutoDetect();
            _pollTimer.Start();

            // 初始偵測
            ConnectionMonitor.Instance.AutoDetect();
        }

        // ── 按鈕事件 ──────────────────────────────────────────────────────

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ConnectionMonitor.Instance.AutoDetect();
            ConnectionMonitor.Instance.Log(LogLevel.Info, "UI", "手動觸發重新偵測");
        }

        private void btnToggleMode_Click(object sender, EventArgs e)
        {
            var next = ConnectionMonitor.Instance.CurrentMode == ConnectionMode.Direct
                ? ConnectionMode.Service
                : ConnectionMode.Direct;
            ConnectionMonitor.Instance.ForceMode(next);
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
            ConnectionMonitor.Instance.Log(LogLevel.Info, "UI", "記錄已清除");
        }

        private void btnExportLog_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog
            {
                Filter   = "文字檔|*.txt",
                FileName = $"UITest_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dlg.FileName, rtbLog.Text);
                    ConnectionMonitor.Instance.Log(LogLevel.Info, "UI", $"記錄已匯出：{dlg.FileName}");
                }
            }
        }

        // ── Log 寫入（跨執行緒安全）──────────────────────────────────────

        private void OnNewLog(LogEntry entry)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.BeginInvoke((Action<LogEntry>)OnNewLog, entry);
                return;
            }
            AppendLog(entry);
        }

        private void AppendLog(LogEntry entry)
        {
            // 時間（灰色）
            AppendText($"[{entry.Time:HH:mm:ss.fff}] ", Color.FromArgb(120, 120, 120));

            // Level badge（顏色依等級）
            Color levelColor;
            switch (entry.Level)
            {
                case LogLevel.Send:  levelColor = Color.FromArgb(100, 210, 255); break;
                case LogLevel.Recv:  levelColor = Color.FromArgb(100, 255, 150); break;
                case LogLevel.Warn:  levelColor = Color.FromArgb(255, 210, 80);  break;
                case LogLevel.Error: levelColor = Color.FromArgb(255, 80,  80);  break;
                default:             levelColor = Color.FromArgb(200, 200, 200); break;
            }
            AppendText($"[{entry.Level,-5}] ", levelColor);

            // Source（青色）
            AppendText($"[{entry.Source}] ", Color.FromArgb(140, 220, 220));

            // Message（白色）
            AppendText(entry.Message + "\n", Color.White);

            // 自動捲到底
            rtbLog.SelectionStart  = rtbLog.TextLength;
            rtbLog.ScrollToCaret();
        }

        private void AppendText(string text, Color color)
        {
            rtbLog.SelectionStart  = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor  = color;
            rtbLog.AppendText(text);
        }

        // ── 狀態 UI 更新（跨執行緒安全）────────────────────────────────

        private void RefreshStatusUI()
        {
            if (InvokeRequired) { BeginInvoke((Action)RefreshStatusUI); return; }

            var mon = ConnectionMonitor.Instance;

            // 模式 Badge
            bool isService = mon.CurrentMode == ConnectionMode.Service;
            lblModeBadge.Text      = isService ? "Service 模式" : "直連模式";
            lblModeBadge.BackColor = isService ? Color.FromArgb(40, 167, 69) : Color.Orange;
            btnToggleMode.Text     = isService ? "切換為直連" : "切換為 Service";

            // Service 狀態
            pnlSvcDot.BackColor  = mon.IsServiceRunning ? Color.FromArgb(40, 167, 69) : Color.FromArgb(200, 50, 50);
            lblSvcStatus.Text      = mon.IsServiceRunning ? "執行中" : "未啟動";
            lblSvcStatus.ForeColor = mon.IsServiceRunning ? Color.FromArgb(40, 167, 69) : Color.FromArgb(200, 50, 50);

            // COM 狀態
            pnlComDot.BackColor  = mon.IsComPortOpen ? Color.FromArgb(40, 167, 69) : Color.FromArgb(200, 50, 50);
            lblComStatus.Text      = mon.IsComPortOpen ? "已開啟" : "未開啟";
            lblComStatus.ForeColor = mon.IsComPortOpen ? Color.FromArgb(40, 167, 69) : Color.Gray;
        }
    }
}
