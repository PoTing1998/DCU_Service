using System;
using System.Linq;
using System.ServiceProcess;

namespace UITest.Services
{
    /// <summary>
    /// 連線模式管理 + 集中式通訊記錄器（Singleton）
    /// </summary>
    public class ConnectionMonitor
    {
        // ── Singleton ────────────────────────────────────────────────────
        private static readonly Lazy<ConnectionMonitor> _inst =
            new Lazy<ConnectionMonitor>(() => new ConnectionMonitor());
        public static ConnectionMonitor Instance => _inst.Value;
        private ConnectionMonitor() { }

        // ── 模式 ─────────────────────────────────────────────────────────
        public enum ConnectionMode { Direct, Service }

        private ConnectionMode _mode = ConnectionMode.Direct;

        public ConnectionMode CurrentMode
        {
            get => _mode;
            private set
            {
                if (_mode == value) return;
                _mode = value;
                ModeChanged?.Invoke(value);
            }
        }

        /// <summary>模式切換時觸發（訂閱者更新 UI）</summary>
        public event Action<ConnectionMode> ModeChanged;

        // ── 狀態 ─────────────────────────────────────────────────────────
        public bool IsServiceRunning { get; private set; }
        public bool IsComPortOpen    { get; private set; }

        /// <summary>Service 或 COM 狀態變更時觸發</summary>
        public event Action StatusRefreshed;

        // ── Log ──────────────────────────────────────────────────────────
        public enum LogLevel { Info, Send, Recv, Warn, Error }

        public class LogEntry
        {
            public DateTime  Time    { get; }
            public LogLevel  Level   { get; }
            public string    Source  { get; }
            public string    Message { get; }

            public LogEntry(LogLevel level, string source, string message)
            {
                Time    = DateTime.Now;
                Level   = level;
                Source  = source;
                Message = message;
            }

            public override string ToString()
                => $"[{Time:HH:mm:ss.fff}] [{Level,-5}] [{Source}] {Message}";
        }

        /// <summary>有新 Log 時觸發（UI 訂閱後更新 RichTextBox）</summary>
        public event Action<LogEntry> OnLog;

        public void Log(LogLevel level, string source, string message)
            => OnLog?.Invoke(new LogEntry(level, source, message));

        // ── 偵測 / 切換 ──────────────────────────────────────────────────

        /// <summary>偵測 DCU_Service 是否在執行，並自動切換模式</summary>
        public void AutoDetect(string comPortName = null)
        {
            // 偵測 Windows Service
            try
            {
                var sc = ServiceController.GetServices()
                             .FirstOrDefault(s => s.ServiceName == "DCU_Service");
                IsServiceRunning = sc != null && sc.Status == ServiceControllerStatus.Running;
            }
            catch
            {
                IsServiceRunning = false;
            }

            // COM Port 狀態（由外部注入）
            if (comPortName != null)
                IsComPortOpen = System.IO.Ports.SerialPort.GetPortNames()
                                    .Any(p => p.Equals(comPortName, StringComparison.OrdinalIgnoreCase));

            // 自動決定模式
            var newMode = IsServiceRunning ? ConnectionMode.Service : ConnectionMode.Direct;
            if (CurrentMode != newMode)
            {
                Log(LogLevel.Info, "Monitor",
                    $"偵測到模式變更：{CurrentMode} → {newMode}" +
                    (IsServiceRunning ? "（DCU_Service 執行中）" : "（DCU_Service 未啟動）"));
            }
            CurrentMode = newMode;

            StatusRefreshed?.Invoke();
        }

        /// <summary>手動強制切換模式</summary>
        public void ForceMode(ConnectionMode mode)
        {
            Log(LogLevel.Warn, "Monitor", $"手動覆寫模式：{mode}");
            CurrentMode = mode;
            StatusRefreshed?.Invoke();
        }

        /// <summary>更新 COM Port 開啟狀態（供 SerialSettingControl 呼叫）</summary>
        public void UpdateComPortStatus(bool isOpen, string portName = "")
        {
            IsComPortOpen = isOpen;
            Log(LogLevel.Info, "COM",
                isOpen ? $"{portName} 已開啟" : $"{portName} 已關閉");
            StatusRefreshed?.Invoke();
        }
    }
}
