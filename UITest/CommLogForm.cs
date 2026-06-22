using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UITest.Services;
using static UITest.Services.ConnectionMonitor;

namespace UITest
{
    /// <summary>
    /// 浮動通訊 LOG 視窗：顯示所有傳送/接收的 HEX byte 及事件記錄。
    /// 關閉時隱藏（hide）而非銷毀，可重複開啟。
    /// </summary>
    public partial class CommLogForm : Form
    {
        // 儲存目前完整的 log 文字（供 HexOnly 篩選重繪）
        private System.Collections.Generic.List<LogEntry> _entries
            = new System.Collections.Generic.List<LogEntry>();

        private bool _hexOnly = false;

        public CommLogForm()
        {
            InitializeComponent();
            AppendRaw("Start Program\n", Color.FromArgb(120, 120, 120));

            // 訂閱 ConnectionMonitor
            ConnectionMonitor.Instance.OnLog += OnNewLog;
        }

        // ── 接收新 log（跨執行緒安全）────────────────────────────────────

        private void OnNewLog(LogEntry entry)
        {
            if (IsDisposed) return;
            if (InvokeRequired) { BeginInvoke((Action<LogEntry>)OnNewLog, entry); return; }

            _entries.Add(entry);

            // HexOnly 模式：只顯示 Send/Recv
            if (_hexOnly && entry.Level != LogLevel.Send && entry.Level != LogLevel.Recv)
                return;

            WriteEntry(entry);
        }

        private void WriteEntry(LogEntry entry)
        {
            // 時間
            AppendRaw($"[{entry.Time:HH:mm:ss.fff}] ", Color.FromArgb(100, 100, 100));

            // Level badge
            Color lc;
            switch (entry.Level)
            {
                case LogLevel.Send:  lc = Color.FromArgb(80,  200, 255); break;
                case LogLevel.Recv:  lc = Color.FromArgb(80,  255, 140); break;
                case LogLevel.Warn:  lc = Color.FromArgb(255, 200, 60);  break;
                case LogLevel.Error: lc = Color.FromArgb(255, 80,  80);  break;
                default:             lc = Color.FromArgb(180, 180, 180); break;
            }
            AppendRaw($"[{entry.Level,-5}] ", lc);

            // Source
            AppendRaw($"[{entry.Source}] ", Color.FromArgb(130, 210, 210));

            // Message — HEX 部分換行並特別標亮
            string msg = entry.Message;
            int hexIdx = msg.IndexOf("HEX:", StringComparison.OrdinalIgnoreCase);
            if (hexIdx >= 0)
            {
                AppendRaw(msg.Substring(0, hexIdx).TrimEnd(), Color.White);
                AppendRaw("\n    ", Color.White); // 換行 + 縮排對齊
                AppendRaw(msg.Substring(hexIdx), Color.FromArgb(255, 230, 100)); // HEX 內容黃色
            }
            else
            {
                AppendRaw(msg, Color.White);
            }

            AppendRaw("\n", Color.White);

            if (chkAutoScroll.Checked)
            {
                rtbLog.SelectionStart = rtbLog.TextLength;
                rtbLog.ScrollToCaret();
            }
        }

        private void AppendRaw(string text, Color color)
        {
            rtbLog.SelectionStart  = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor  = color;
            rtbLog.AppendText(text);
        }

        // ── 按鈕事件 ──────────────────────────────────────────────────────

        private void btnClear_Click(object sender, EventArgs e)
        {
            _entries.Clear();
            rtbLog.Clear();
            AppendRaw("（記錄已清除）\n", Color.FromArgb(100, 100, 100));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog
            {
                Filter   = "文字檔|*.txt|所有檔案|*.*",
                FileName = $"CommLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dlg.FileName, rtbLog.Text);
                    ConnectionMonitor.Instance.Log(LogLevel.Info, "CommLog",
                        $"LOG 已匯出：{dlg.FileName}");
                }
            }
        }

        private void chkHexOnly_CheckedChanged(object sender, EventArgs e)
        {
            _hexOnly = chkHexOnly.Checked;
            RedrawAll();
        }

        // ── 篩選後重繪 ────────────────────────────────────────────────────

        private void RedrawAll()
        {
            rtbLog.Clear();
            AppendRaw("Start Program\n", Color.FromArgb(100, 100, 100));
            foreach (var entry in _entries)
            {
                if (_hexOnly && entry.Level != LogLevel.Send && entry.Level != LogLevel.Recv)
                    continue;
                WriteEntry(entry);
            }
        }

        // ── 關閉時隱藏（不銷毀）─────────────────────────────────────────

        private void CommLogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
