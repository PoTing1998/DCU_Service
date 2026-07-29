using System;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

using UITest.Services;

using ASI.Wanda.PA.Message;
using PAEnum = ASI.Wanda.PA.PA_Enum;

namespace UITest.Controls
{
    /// <summary>
    /// PA（月台廣播系統）測試分頁：測試 DMD → PA 方向的封包（車站/月台/列車狀況）。
    ///
    /// 使用獨立的序列埠連線（對應 TaskPA 實際使用的 PAComPort/PABaudrate 設定），
    /// 不與「串列埠設定」分頁共用連線，可單獨對 PA 設備收送測試。
    ///
    /// 封包內容沿用 PA_Frame.MsgPacket（cmd=0x01, station, platform, situation，
    /// textLength 不含 LRC），是否附加 LRC 校驗位元組可由勾選框控制。
    ///
    /// 回覆判讀（ACK 0x06 / NAK 0x15 及其錯誤碼）比照 TaskPA.ProcTaskPA 既有的慣例
    /// （dataBytes[2] 為回應碼，dataBytes[4] 為 NAK 時的錯誤碼）。
    /// </summary>
    public partial class PATestControl : UserControl
    {
        private SerialPort _port;
        private bool _isOpen;
        private byte[] _lastPacket;

        public PATestControl()
        {
            InitializeComponent();

            if (!DesignMode)
                InitComboBoxes();
        }

        private void InitComboBoxes()
        {
            foreach (string p in SerialPort.GetPortNames())
                cmbCOM.Items.Add(p);
            if (cmbCOM.Items.Count > 0) cmbCOM.SelectedIndex = 0;

            foreach (int b in new[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 })
                cmbBaudRate.Items.Add(b);
            cmbBaudRate.SelectedItem = 9600;

            foreach (string s in Enum.GetNames(typeof(PAEnum.station)))
                cmbStation.Items.Add(s);
            if (cmbStation.Items.Count > 0) cmbStation.SelectedIndex = 0;

            foreach (string s in Enum.GetNames(typeof(PAEnum.platform)))
                cmbPlatform.Items.Add(s);
            if (cmbPlatform.Items.Count > 0) cmbPlatform.SelectedIndex = 0;

            foreach (string s in Enum.GetNames(typeof(PAEnum.situation)))
                cmbSituation.Items.Add(s);
            if (cmbSituation.Items.Count > 0) cmbSituation.SelectedIndex = 0;
        }

        // ── 連線 ─────────────────────────────────────────────────────────

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (!_isOpen) OpenPort();
            else ClosePort();
        }

        private void OpenPort()
        {
            try
            {
                if (cmbCOM.SelectedItem == null)
                {
                    MessageBox.Show("請選擇 COM Port", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _port = new SerialPort
                {
                    PortName = cmbCOM.SelectedItem.ToString(),
                    BaudRate = (int)cmbBaudRate.SelectedItem,
                    DataBits = 8,
                    Parity   = Parity.None,
                    StopBits = StopBits.One
                };
                _port.DataReceived += Port_DataReceived;
                _port.Open();
                _isOpen = true;

                btnOpen.Text            = "關閉";
                cmbCOM.Enabled          = false;
                cmbBaudRate.Enabled     = false;
                lblConnStatus.Text      = $"已連線：{_port.PortName} @ {_port.BaudRate}";
                lblConnStatus.ForeColor = System.Drawing.Color.Green;

                ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Info, "PA",
                    $"序列埠已開啟：{_port.PortName} @ {_port.BaudRate}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"開啟失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClosePort()
        {
            try
            {
                if (_port != null)
                {
                    _port.DataReceived -= Port_DataReceived;
                    if (_port.IsOpen) _port.Close();
                    _port.Dispose();
                    _port = null;
                }
                _isOpen = false;

                btnOpen.Text            = "開啟";
                cmbCOM.Enabled          = true;
                cmbBaudRate.Enabled     = true;
                lblConnStatus.Text      = "未連線";
                lblConnStatus.ForeColor = System.Drawing.Color.Gray;

                ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Info, "PA", "序列埠已關閉");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"關閉失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── 封包組建 / 傳送 ──────────────────────────────────────────────

        private byte[] BuildPacket()
        {
            var conv = new MsgPacket();
            var pkt = new MsgPacket
            {
                station   = conv.GetStationValue(cmbStation.SelectedItem.ToString()),
                platform  = conv.GetPlatformFromValue(cmbPlatform.SelectedItem.ToString()),
                situation = conv.GetSituationFromValue(cmbSituation.SelectedItem.ToString())
            };

            byte[] bytes = pkt.textMessage();

            if (chkAppendLRC.Checked)
            {
                byte lrc = ASI.Lib.Msg.Parsing.ByteArray.CalculateLRC(bytes);
                bytes = bytes.Concat(new[] { lrc }).ToArray();
            }

            return bytes;
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                _lastPacket = BuildPacket();
                string hex = ASI.Lib.Text.Parsing.String.BytesToHexString(_lastPacket, " ");
                AppendLine($"✔ 封包組建成功：{hex}");
            }
            catch (Exception ex)
            {
                AppendLine($"✘ 封包組建失敗：{ex.Message}");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (_port == null || !_port.IsOpen)
            {
                MessageBox.Show("請先開啟序列埠", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _lastPacket = BuildPacket();
                _port.Write(_lastPacket, 0, _lastPacket.Length);

                string hex = ASI.Lib.Text.Parsing.String.BytesToHexString(_lastPacket, " ");
                AppendLine($"→ 送出：{hex}");
                ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Send, "PA", $"HEX: {hex}");
            }
            catch (Exception ex)
            {
                AppendLine($"✘ 傳送失敗：{ex.Message}");
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => txtOutput.Clear();

        // ── 接收 ─────────────────────────────────────────────────────────

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var port = _port;
                if (port == null) return;

                int n = port.BytesToRead;
                if (n <= 0) return;
                byte[] buf = new byte[n];
                port.Read(buf, 0, n);

                string hex = ASI.Lib.Text.Parsing.String.BytesToHexString(buf, " ");
                string interpretation = Interpret(buf);

                if (InvokeRequired)
                    BeginInvoke((Action)(() => ShowReceived(hex, interpretation)));
                else
                    ShowReceived(hex, interpretation);
            }
            catch (Exception ex)
            {
                ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Error, "PA", $"接收處理失敗：{ex.Message}");
            }
        }

        private void ShowReceived(string hex, string interpretation)
        {
            AppendLine($"← 收到：{hex}　{interpretation}");
            ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Recv, "PA", $"{interpretation}  HEX: {hex}");
        }

        /// <summary>
        /// 依 TaskPA（ProcTaskPA.SerialPort_ReceivedEvent）既有慣例判讀回應：
        /// index2 = 0x06 表示 ACK；0x15 表示 NAK（index4 為錯誤碼：01=長度錯誤/02=LRC錯誤/03=其他錯誤）；
        /// 其餘視為未知回應或非本協定資料，僅顯示原始 HEX 供人工判讀。
        /// </summary>
        private string Interpret(byte[] data)
        {
            if (data == null || data.Length < 3)
                return "（長度不足，無法判讀）";

            byte b2 = data[2];
            if (b2 == 0x06) return "[ACK 正確回應]";
            if (b2 == 0x15)
            {
                string reason = "未知錯誤";
                if (data.Length >= 5)
                {
                    switch (data[4])
                    {
                        case 0x01: reason = "數據包長度錯誤"; break;
                        case 0x02: reason = "LRC 錯誤"; break;
                        case 0x03: reason = "其他錯誤"; break;
                    }
                }
                return $"[NAK：{reason}]";
            }
            return "[未知回應/資料，請對照裝置文件人工確認]";
        }

        private void AppendLine(string text) => txtOutput.AppendText(text + "\r\n");
    }
}
