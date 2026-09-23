using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UITest.Services;

using DmdService = ASI.Wanda.DMD.Service;

namespace UITest.Controls
{
    /// <summary>
    /// DMD 接收分頁：以 Socket Client 連線 DMD Server，接收 DMD 送來的訊號，
    /// 並呼叫 DMD_Service（與 TaskDMD 服務共用）的 <see cref="DmdService.DMDMessageProcessor"/> 處理。
    ///
    /// ・模擬模式（預設）：只解析並列出「會執行的動作」，不寫 DB、不送 MSMQ、不回 Ack。
    /// ・真實模式：與 TaskDMD 服務行為完全相同（寫 DB、送 MSMQ 給 PUP/CDU/SDU/PDN、回 Ack）。
    ///
    /// 畫面只負責顯示（前端），處理邏輯全部在 DMD_Service（後端），兩者不重複實作。
    /// </summary>
    public partial class DMDReceiveControl : UserControl
    {
        private const int MaxRows = 1000;

        private ASI.Wanda.DMD.DMD_API _api;
        private readonly DmdService.DMDMessageProcessor _processor = new DmdService.DMDMessageProcessor();
        private readonly List<DmdService.DMDProcessResult> _results = new List<DmdService.DMDProcessResult>();

        private volatile bool _realMode;
        private bool _dbReady;
        private bool _busy;
        private bool _suppressModeEvent;

        public DMDReceiveControl()
        {
            InitializeComponent();

            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
                LoadDefaults();
        }

        // ── 預設值：沿用 TaskDMD 的 DMD_Server 設定 ─────────────────────────

        private void LoadDefaults()
        {
            txtIP.Text   = "127.0.0.1";
            txtPort.Text = "";
            try
            {
                string conn = ASI.Lib.Config.ConfigApp.Instance.GetConfigSetting("DMD_Server");
                if (!string.IsNullOrEmpty(conn))
                {
                    foreach (string part in conn.Split(';'))
                    {
                        var kv = part.Split(new[] { '=' }, 2);
                        if (kv.Length != 2) continue;
                        string key = kv[0].Trim();
                        if (key.Equals("IP", StringComparison.OrdinalIgnoreCase))   txtIP.Text   = kv[1].Trim();
                        if (key.Equals("Port", StringComparison.OrdinalIgnoreCase)) txtPort.Text = kv[1].Trim();
                    }
                }
            }
            catch
            {
                // 找不到 Config\Config.xml 時使用預設值
            }
        }

        // ── 連線 ─────────────────────────────────────────────────────────

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            if (_api == null) Connect();
            else Disconnect();
        }

        private void Connect()
        {
            string ip = txtIP.Text.Trim();
            if (ip.Length == 0 || !int.TryParse(txtPort.Text.Trim(), out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("請輸入正確的 IP 與 Port", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = $"IP={ip};Port={port};Type=Client";
            SetBusy(true, $"連線中：{ip}:{port} ...");

            Task.Run(() =>
            {
                var api = new ASI.Wanda.DMD.DMD_API();
                api.ReceivedEvent += Api_ReceivedEvent;
                int rc;
                try { rc = api.Initial(connStr); }
                catch (Exception ex)
                {
                    rc = -1;
                    ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Error, "DMD", $"連線例外：{ex.Message}");
                }

                if (rc != 0)
                {
                    api.ReceivedEvent -= Api_ReceivedEvent;
                    api.Dispose();
                    api = null;
                }

                SafeInvoke(() =>
                {
                    _api = api;
                    SetBusy(false, null);
                    if (rc == 0)
                    {
                        SetConnectedUI(true, $"已連線：{ip}:{port}");
                        ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Info, "DMD", $"已連線 DMD Server {connStr}");
                    }
                    else
                    {
                        SetConnectedUI(false, $"連線失敗（代碼 {rc}）");
                        lblConnStatus.ForeColor = Color.Red;
                        ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Error, "DMD", $"連線 DMD Server 失敗，代碼 {rc}，{connStr}");
                    }
                });
            });
        }

        private void Disconnect()
        {
            var api = _api;
            _api = null;
            if (api != null)
            {
                api.ReceivedEvent -= Api_ReceivedEvent;
                try { api.Dispose(); } catch { }
                ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Info, "DMD", "已中斷 DMD Server 連線");
            }
            SetConnectedUI(false, "未連線");
        }

        private void SetBusy(bool busy, string status)
        {
            _busy = busy;
            btnConnect.Enabled = !busy;
            if (status != null)
            {
                lblConnStatus.Text      = status;
                lblConnStatus.ForeColor = Color.DarkOrange;
            }
        }

        private void SetConnectedUI(bool connected, string status)
        {
            btnConnect.Text         = connected ? "中斷" : "連線";
            txtIP.Enabled           = !connected;
            txtPort.Enabled         = !connected;
            lblConnStatus.Text      = status;
            lblConnStatus.ForeColor = connected ? Color.Green : Color.Gray;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            Disconnect();
            base.OnHandleDestroyed(e);
        }

        // ── 模式切換 ─────────────────────────────────────────────────────

        private void chkRealMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressModeEvent) return;

            if (chkRealMode.Checked)
            {
                var answer = MessageBox.Show(
                    "真實模式會與 TaskDMD 服務做完全相同的事：\r\n" +
                    "  ・同步 DMD DB → DCU DB\r\n" +
                    "  ・送 MSMQ 給 TaskPUP / TaskCDU / TaskSDU / TaskPDN\r\n" +
                    "  ・回覆 Ack 給 DMD Server\r\n\r\n" +
                    "若 DCUService 的 TaskDMD 也同時連線，訊息會被處理兩次。\r\n確定要切換？",
                    "切換為真實模式", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (answer != DialogResult.Yes || !EnsureDatabase())
                {
                    _suppressModeEvent = true;
                    chkRealMode.Checked = false;
                    _suppressModeEvent = false;
                    return;
                }
            }

            _realMode = chkRealMode.Checked;
            lblMode.Text      = _realMode ? "目前：真實模式（會寫 DB / 送 MSMQ / 回 Ack）" : "目前：模擬模式（只顯示，不產生副作用）";
            lblMode.ForeColor = _realMode ? Color.Red : Color.DimGray;
            ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Warn, "DMD", lblMode.Text);
        }

        /// <summary>真實模式需要資料庫連線，設定來源與 TaskDMD.StartTask 相同</summary>
        private bool EnsureDatabase()
        {
            if (_dbReady) return true;
            try
            {
                var cfg = ASI.Lib.Config.ConfigApp.Instance;
                string user    = cfg.GetConfigSetting("DCU_DB_User");
                string pwd     = cfg.GetConfigSetting("DCU_DB_Password");
                string curUser = cfg.GetConfigSetting("Current_User_ID");

                string dmdIP = cfg.GetConfigSetting("DMD_DB_IP"), dmdPort = cfg.GetConfigSetting("DMD_DB_Port"), dmdName = cfg.GetConfigSetting("DMD_DB_Name");
                string dcuIP = cfg.GetConfigSetting("DCU_DB_IP"), dcuPort = cfg.GetConfigSetting("DCU_DB_Port"), dcuName = cfg.GetConfigSetting("DCU_DB_Name");

                if (!ASI.Wanda.DMD.DB.Manager.Initializer(dmdIP, dmdPort, dmdName, user, pwd, curUser))
                {
                    MessageBox.Show($"DMD 資料庫連線失敗：{dmdIP}:{dmdPort}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (!ASI.Wanda.DCU.DB.Manager.Initializer(dcuIP, dcuPort, dcuName, user, pwd, curUser))
                {
                    MessageBox.Show($"DCU 資料庫連線失敗：{dcuIP}:{dcuPort}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                _dbReady = true;
                ConnectionMonitor.Instance.Log(ConnectionMonitor.LogLevel.Info, "DMD", "DMD / DCU 資料庫初始化成功");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"資料庫初始化失敗（請確認 Config\\Config.xml）：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ── 接收（DMD_API 解析執行緒）───────────────────────────────────

        private void Api_ReceivedEvent(ASI.Wanda.DMD.Message.Message message)
        {
            DmdService.DMDProcessResult result;
            try
            {
                var helper = _realMode
                    ? DmdService.DMDHelper.CreateReal(msg => { var api = _api; return api == null ? -3 : api.Send(msg); })
                    : DmdService.DMDHelper.CreateSimulated();

                result = _processor.Process(message, helper);
            }
            catch (Exception ex)
            {
                result = new DmdService.DMDProcessResult { Success = false, ErrorMessage = ex.Message };
            }

            ConnectionMonitor.Instance.Log(
                result.Success ? ConnectionMonitor.LogLevel.Recv : ConnectionMonitor.LogLevel.Error, "DMD",
                $"{result.MessageType} ID:{result.MessageID} {result.ShortObjectName} → {result.HandlerName} {(result.Success ? "" : result.ErrorMessage)}");

            SafeInvoke(() => AddResult(result));
        }

        private void SafeInvoke(Action action)
        {
            if (IsDisposed || !IsHandleCreated) return;
            try
            {
                if (InvokeRequired) BeginInvoke(action);
                else action();
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        // ── 顯示 ─────────────────────────────────────────────────────────

        private void AddResult(DmdService.DMDProcessResult r)
        {
            _results.Add(r);

            var item = new ListViewItem(r.ReceivedTime.ToString("HH:mm:ss.fff"));
            item.SubItems.Add(r.IsSimulated ? "模擬" : "真實");
            item.SubItems.Add(r.MessageType.ToString());
            item.SubItems.Add(r.MessageID.ToString());
            item.SubItems.Add(r.MessageLength.ToString());
            item.SubItems.Add(r.ShortObjectName);
            item.SubItems.Add(r.HandlerName);
            item.SubItems.Add(r.Success ? $"成功（{r.Actions.Count} 個動作）" : "失敗：" + r.ErrorMessage);
            item.Tag = r;
            if (!r.Success) item.ForeColor = Color.Red;
            else if (!r.IsSimulated) item.ForeColor = Color.DarkBlue;

            lvMessages.BeginUpdate();
            lvMessages.Items.Add(item);
            while (lvMessages.Items.Count > MaxRows)
            {
                _results.Remove((DmdService.DMDProcessResult)lvMessages.Items[0].Tag);
                lvMessages.Items.RemoveAt(0);
            }
            lvMessages.EndUpdate();

            lblCount.Text = $"共 {lvMessages.Items.Count} 筆";

            if (chkAutoScroll.Checked)
            {
                item.EnsureVisible();
                lvMessages.SelectedItems.Clear();
                item.Selected = true;
            }
        }

        private void lvMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMessages.SelectedItems.Count == 0) return;
            if (!(lvMessages.SelectedItems[0].Tag is DmdService.DMDProcessResult r)) return;

            var sb = new StringBuilder();
            sb.AppendLine($"時間　　：{r.ReceivedTime:yyyy-MM-dd HH:mm:ss.fff}");
            sb.AppendLine($"模式　　：{(r.IsSimulated ? "模擬" : "真實")}");
            sb.AppendLine($"訊息類別：{r.MessageType}　識別碼：{r.MessageID}　長度：{r.MessageLength}");
            sb.AppendLine($"物件名稱：{r.JsonObjectName}");
            sb.AppendLine($"處理器　：{r.HandlerName}");
            sb.AppendLine($"結果　　：{(r.Success ? "成功" : "失敗 - " + r.ErrorMessage)}");
            sb.AppendLine();
            sb.AppendLine("── HEX ─────────────────────────");
            sb.AppendLine(FormatHex(r.HexContent));
            sb.AppendLine();
            sb.AppendLine("── JSON ────────────────────────");
            sb.AppendLine(PrettyJson(r.JsonContent));
            txtDetail.Text = sb.ToString();

            var act = new StringBuilder();
            if (r.Actions.Count == 0) act.AppendLine("（無動作）");
            for (int i = 0; i < r.Actions.Count; i++)
                act.AppendLine($"{i + 1,2}. {r.Actions[i]}");
            txtActions.Text = act.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lvMessages.Items.Clear();
            _results.Clear();
            txtDetail.Clear();
            txtActions.Clear();
            lblCount.Text = "共 0 筆";
        }

        private static string FormatHex(string hex)
        {
            if (string.IsNullOrEmpty(hex)) return "";
            var sb = new StringBuilder();
            for (int i = 0; i + 1 < hex.Length; i += 2)
            {
                sb.Append(hex, i, 2).Append(' ');
                if ((i / 2 + 1) % 16 == 0) sb.AppendLine();
            }
            return sb.ToString().TrimEnd();
        }

        /// <summary>簡易 JSON 縮排（不依賴 Newtonsoft）</summary>
        private static string PrettyJson(string json)
        {
            if (string.IsNullOrEmpty(json)) return "";
            var sb = new StringBuilder();
            int indent = 0;
            bool inString = false;
            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"' && (i == 0 || json[i - 1] != '\\')) inString = !inString;
                if (inString) { sb.Append(c); continue; }
                switch (c)
                {
                    case '{':
                    case '[':
                        sb.Append(c).AppendLine().Append(' ', ++indent * 2);
                        break;
                    case '}':
                    case ']':
                        sb.AppendLine().Append(' ', Math.Max(0, --indent) * 2).Append(c);
                        break;
                    case ',':
                        sb.Append(c).AppendLine().Append(' ', indent * 2);
                        break;
                    case ':':
                        sb.Append(": ");
                        break;
                    default:
                        if (!char.IsWhiteSpace(c)) sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
