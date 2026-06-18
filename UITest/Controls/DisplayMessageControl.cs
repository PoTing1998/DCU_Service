using Display;
using Display.DisplayMode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using UITest.Services;
using static Display.DisplaySettingsEnums;

namespace UITest.Controls
{
    public partial class DisplayMessageControl : UserControl
    {
        // ── 委派（由 Form1 注入）────────────────────────────────────────
        /// <summary>實際傳送 byte[] 給串列埠；回傳 true=成功。</summary>
        public Func<byte[], bool>   SendAction      { get; set; }
        /// <summary>取得目前勾選的上行月台 ID 清單。</summary>
        public Func<List<byte>>     GetUpIDsFunc    { get; set; }
        /// <summary>取得目前勾選的下行月台 ID 清單。</summary>
        public Func<List<byte>>     GetDnIDsFunc    { get; set; }

        // ── 預覽狀態 ─────────────────────────────────────────────────────
        private string _upText   = "";
        private Color  _upColor  = Color.Yellow;
        private float  _upFontPt = 14f;

        private string _dnText   = "";
        private Color  _dnColor  = Color.Yellow;
        private float  _dnFontPt = 14f;

        private readonly DisplayMessageService _service = new DisplayMessageService();

        // 字型名稱常數，確保 Designer / 邏輯端完全一致
        private const string FontName24x24 = "24x24";
        private const string FontName16x16 = "16x16";
        private const string FontName5x7   = "英文 5x7";

        private static readonly string SettingsPath =
            Path.Combine(Application.StartupPath, "display_settings.xml");

        // 板型 RadioButton 陣列快取（初始化後不再變動）
        private RadioButton[] _upBoardRadios;
        private RadioButton[] _dnBoardRadios;

        public DisplayMessageControl()
        {
            InitializeComponent();
            _upBoardRadios = new[] { rdoUpBoard1, rdoUpBoard2, rdoUpBoard3, rdoUpBoard4,
                                     rdoUpBoard5, rdoUpBoard6, rdoUpBoard7, rdoUpBoard8 };
            _dnBoardRadios = new[] { rdoDnBoard1, rdoDnBoard2, rdoDnBoard3, rdoDnBoard4,
                                     rdoDnBoard5, rdoDnBoard6, rdoDnBoard7, rdoDnBoard8 };
            // 顏色 ComboBox 的 Items 無法在 Designer.cs 內引用 ColorComboItems（動態屬性），改在此初始化
            foreach (var cmb in new[] { cmbUpColor, cmbDnColor, cmbUpTimeClr, cmbDnTimeClr, cmbUpPlatClr, cmbDnPlatClr })
                cmb.Items.AddRange(ColorComboItems);
            cmbUpColor.SelectedIndex   = 3; // clYellow
            cmbDnColor.SelectedIndex   = 3; // clYellow
            cmbUpTimeClr.SelectedIndex = 0; // clBlack
            cmbDnTimeClr.SelectedIndex = 0; // clBlack
            cmbUpPlatClr.SelectedIndex = 0; // clBlack
            cmbDnPlatClr.SelectedIndex = 0; // clBlack
            chkUpMulti.CheckedChanged += chkUpMulti_CheckedChanged;
            chkDnMulti.CheckedChanged += chkDnMulti_CheckedChanged;
            LoadSettings();
        }

        // ════════════════════════════════════════════════════════════════
        // 上傳 / 存檔
        // ════════════════════════════════════════════════════════════════

        private void btnUpUpload_Click(object sender, EventArgs e)
            => DoUpload(uploadUp: true, uploadDn: false);

        private void btnDnUpload_Click(object sender, EventArgs e)
            => DoUpload(uploadUp: false, uploadDn: true);

        private void btnAllUpload_Click(object sender, EventArgs e)
            => DoUpload(uploadUp: true, uploadDn: true);

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettings();
                MessageBox.Show($"存檔完成\n路徑：{SettingsPath}", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"存檔失敗：{ex.Message}", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════════
        // 顯示（預覽）
        // ════════════════════════════════════════════════════════════════

        private void btnUpMsgShow_Click(object sender, EventArgs e)
        {
            _upText   = txtUpMsg.Text;
            _upColor  = ParseColor(cmbUpColor.SelectedItem?.ToString());
            _upFontPt = SizeToPt(cmbUpFontSize.SelectedItem?.ToString());
            pnlDisplay.Invalidate();
        }

        // ════════════════════════════════════════════════════════════════
        // 多訊息模式切換
        // ════════════════════════════════════════════════════════════════

        private void chkUpMulti_CheckedChanged(object sender, EventArgs e)
        {
            bool multi = chkUpMulti.Checked;
            txtUpMsg.Visible        = !multi;
            btnUpMsgBrowse.Visible  = !multi;
            btnUpMsgShow.Visible    = !multi;
            btnUpMsgAdd.Visible     = !multi;
            lstUpMultiMsgs.Visible  = multi;
            btnUpMultiAdd.Visible   = multi;
            btnUpMultiRemove.Visible = multi;
        }

        private void chkDnMulti_CheckedChanged(object sender, EventArgs e)
        {
            bool multi = chkDnMulti.Checked;
            txtDnMsg.Visible        = !multi;
            btnDnMsgBrowse.Visible  = !multi;
            btnDnMsgShow.Visible    = !multi;
            btnDnMsgAdd.Visible     = !multi;
            lstDnMultiMsgs.Visible  = multi;
            btnDnMultiAdd.Visible   = multi;
            btnDnMultiRemove.Visible = multi;
        }

        // ── 多訊息清單操作 ────────────────────────────────────────────────

        private void btnUpMultiAdd_Click(object sender, EventArgs e)
        {
            using (var picker = new MessagePickerForm(PickerMode.Up, multiSelect: true))
            {
                if (picker.ShowDialog(this) == DialogResult.OK)
                    foreach (var msg in picker.SelectedMessages)
                        if (!string.IsNullOrEmpty(msg))
                            lstUpMultiMsgs.Items.Add(msg);
            }
        }

        private void btnUpMultiRemove_Click(object sender, EventArgs e)
        {
            while (lstUpMultiMsgs.SelectedIndices.Count > 0)
                lstUpMultiMsgs.Items.RemoveAt(lstUpMultiMsgs.SelectedIndices[0]);
        }

        private void btnDnMultiAdd_Click(object sender, EventArgs e)
        {
            using (var picker = new MessagePickerForm(PickerMode.Down, multiSelect: true))
            {
                if (picker.ShowDialog(this) == DialogResult.OK)
                    foreach (var msg in picker.SelectedMessages)
                        if (!string.IsNullOrEmpty(msg))
                            lstDnMultiMsgs.Items.Add(msg);
            }
        }

        private void btnDnMultiRemove_Click(object sender, EventArgs e)
        {
            while (lstDnMultiMsgs.SelectedIndices.Count > 0)
                lstDnMultiMsgs.Items.RemoveAt(lstDnMultiMsgs.SelectedIndices[0]);
        }

        // ── 單訊息 Browse ─────────────────────────────────────────────────

        private void btnUpMsgBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new MessagePickerForm(PickerMode.Up))
            {
                if (picker.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(picker.SelectedMessage))
                    txtUpMsg.Text = picker.SelectedMessage;
            }
        }

        private void btnDnMsgBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new MessagePickerForm(PickerMode.Down))
            {
                if (picker.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(picker.SelectedMessage))
                    txtDnMsg.Text = picker.SelectedMessage;
            }
        }

        private void btnUpMsgAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("新增上行訊息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDnMsgShow_Click(object sender, EventArgs e)
        {
            _dnText   = txtDnMsg.Text;
            _dnColor  = ParseColor(cmbDnColor.SelectedItem?.ToString());
            _dnFontPt = SizeToPt(cmbDnFontSize.SelectedItem?.ToString());
            pnlDisplay.Invalidate();
        }

        private void btnDnMsgAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("新增下行訊息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ════════════════════════════════════════════════════════════════
        // 板型選項變更 → 動態子選項
        // ════════════════════════════════════════════════════════════════

        private void rdoUpBoard_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = sender as RadioButton;
            if (rdo == null || !rdo.Checked) return;
            // 0-based: boards 2,3,7 → index 1,2,6 ; boards 3,5 → index 2,4 ; board 6 → index 5
            int idx = Array.IndexOf(_upBoardRadios, rdo);
            SetUpTimeVisible(idx == 1 || idx == 2 || idx == 6);
            SetUpPlatVisible(idx == 2 || idx == 4);
            SetUpAlarmVisible(idx == 5);
        }

        private void rdoDnBoard_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = sender as RadioButton;
            if (rdo == null || !rdo.Checked) return;
            int idx = Array.IndexOf(_dnBoardRadios, rdo);
            SetDnTimeVisible(idx == 1 || idx == 2 || idx == 6);
            SetDnPlatVisible(idx == 2 || idx == 4);
            SetDnAlarmVisible(idx == 5);
        }

        // ════════════════════════════════════════════════════════════════
        // 核心：組封包並傳送
        // ════════════════════════════════════════════════════════════════

        private void DoUpload(bool uploadUp, bool uploadDn)
        {
            var mon = Services.ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(Services.ConnectionMonitor.LogLevel.Error, "DisplayMsg",
                    "上傳失敗：尚未設定 SendAction（串列埠未開啟或 Service 未連線）");
                MessageBox.Show("尚未連接串列埠，請先在「串列埠設定」頁面開啟 COM Port。",
                    "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var errors = new StringBuilder();

            if (uploadUp)
            {
                var ids = GetUpIDsFunc?.Invoke() ?? new List<byte>();
                if (ids.Count == 0)
                {
                    errors.AppendLine("上行：未在串列埠設定中勾選任何上行月台 ID。");
                }
                else if (chkUpMulti.Checked)
                {
                    // ── 多訊息模式：依序發送 ──
                    if (lstUpMultiMsgs.Items.Count == 0)
                    {
                        errors.AppendLine("上行：多訊息清單為空，請先新增訊息。");
                    }
                    else
                    {
                        int sent = 0;
                        for (int i = 0; i < lstUpMultiMsgs.Items.Count; i++)
                        {
                            string msg    = lstUpMultiMsgs.Items[i].ToString();
                            var    result = _service.Build(BuildUpParams(ids, msg));
                            if (result.IsValid)
                            {
                                mon.Log(Services.ConnectionMonitor.LogLevel.Send, "DisplayMsg",
                                    $"上行[{i + 1}/{lstUpMultiMsgs.Items.Count}] → {msg}  HEX:{result.HexDump}");
                                if (SendAction(result.Value))
                                    sent++;
                                else
                                    errors.AppendLine($"上行訊息 {i + 1}（{msg}）：傳送失敗。");
                            }
                            else
                            {
                                errors.AppendLine($"上行訊息 {i + 1}（{msg}）：封包組建失敗 - {result.ErrorMessage}");
                            }
                        }
                        if (sent > 0)
                            mon.Log(Services.ConnectionMonitor.LogLevel.Recv, "DisplayMsg",
                                $"上行多訊息：共傳送 {sent}/{lstUpMultiMsgs.Items.Count} 則");
                    }
                }
                else
                {
                    // ── 單訊息模式 ──
                    var result = _service.Build(BuildUpParams(ids));
                    if (result.IsValid)
                    {
                        mon.Log(Services.ConnectionMonitor.LogLevel.Send, "DisplayMsg",
                            $"上行 → IDs:{string.Join(",", ids.ConvertAll(b => $"{b:X2}"))}  " +
                            $"模式:{mon.CurrentMode}  HEX:{result.HexDump}");
                        if (!SendAction(result.Value))
                        {
                            mon.Log(Services.ConnectionMonitor.LogLevel.Error, "DisplayMsg", "上行傳送失敗");
                            errors.AppendLine("上行：傳送失敗（串列埠未開啟）。");
                        }
                        else
                            mon.Log(Services.ConnectionMonitor.LogLevel.Recv, "DisplayMsg", "上行傳送成功");
                    }
                    else
                    {
                        mon.Log(Services.ConnectionMonitor.LogLevel.Error, "DisplayMsg",
                            $"上行封包組建失敗：{result.ErrorMessage}");
                        errors.AppendLine($"上行：{result.ErrorMessage}");
                    }
                }
            }

            if (uploadDn)
            {
                var ids = GetDnIDsFunc?.Invoke() ?? new List<byte>();
                if (ids.Count == 0)
                {
                    errors.AppendLine("下行：未在串列埠設定中勾選任何下行月台 ID。");
                }
                else if (chkDnMulti.Checked)
                {
                    // ── 多訊息模式：依序發送 ──
                    if (lstDnMultiMsgs.Items.Count == 0)
                    {
                        errors.AppendLine("下行：多訊息清單為空，請先新增訊息。");
                    }
                    else
                    {
                        int sent = 0;
                        for (int i = 0; i < lstDnMultiMsgs.Items.Count; i++)
                        {
                            string msg    = lstDnMultiMsgs.Items[i].ToString();
                            var    result = _service.Build(BuildDnParams(ids, msg));
                            if (result.IsValid)
                            {
                                mon.Log(Services.ConnectionMonitor.LogLevel.Send, "DisplayMsg",
                                    $"下行[{i + 1}/{lstDnMultiMsgs.Items.Count}] → {msg}  HEX:{result.HexDump}");
                                if (SendAction(result.Value))
                                    sent++;
                                else
                                    errors.AppendLine($"下行訊息 {i + 1}（{msg}）：傳送失敗。");
                            }
                            else
                            {
                                errors.AppendLine($"下行訊息 {i + 1}（{msg}）：封包組建失敗 - {result.ErrorMessage}");
                            }
                        }
                        if (sent > 0)
                            mon.Log(Services.ConnectionMonitor.LogLevel.Recv, "DisplayMsg",
                                $"下行多訊息：共傳送 {sent}/{lstDnMultiMsgs.Items.Count} 則");
                    }
                }
                else
                {
                    // ── 單訊息模式 ──
                    var result = _service.Build(BuildDnParams(ids));
                    if (result.IsValid)
                    {
                        mon.Log(Services.ConnectionMonitor.LogLevel.Send, "DisplayMsg",
                            $"下行 → IDs:{string.Join(",", ids.ConvertAll(b => $"{b:X2}"))}  " +
                            $"模式:{mon.CurrentMode}  HEX:{result.HexDump}");
                        if (!SendAction(result.Value))
                        {
                            mon.Log(Services.ConnectionMonitor.LogLevel.Error, "DisplayMsg", "下行傳送失敗");
                            errors.AppendLine("下行：傳送失敗（串列埠未開啟）。");
                        }
                        else
                            mon.Log(Services.ConnectionMonitor.LogLevel.Recv, "DisplayMsg", "下行傳送成功");
                    }
                    else
                    {
                        mon.Log(Services.ConnectionMonitor.LogLevel.Error, "DisplayMsg",
                            $"下行封包組建失敗：{result.ErrorMessage}");
                        errors.AppendLine($"下行：{result.ErrorMessage}");
                    }
                }
            }

            if (errors.Length > 0)
                MessageBox.Show(errors.ToString().TrimEnd(), "上傳錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("上傳完成", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── 組上行參數 ────────────────────────────────────────────────────
        private DisplayMessageParams BuildUpParams(List<byte> ids, string text = null) => new DisplayMessageParams
        {
            Text         = text ?? txtUpMsg.Text,
            HexColor     = NameToHex(cmbUpColor.SelectedItem?.ToString()),
            FontSz       = NameToFontSize(cmbUpFontSize.SelectedItem?.ToString()),
            FontSty      = NameToFontStyle(cmbUpFontStyle.SelectedItem?.ToString()),
            ScrollMode   = GetScrollMode(true),
            ScrollSpeed  = (byte)nudUpSpeed.Value,
            PauseTime    = (byte)nudUpPause.Value,
            MessageLevel = ParseLevel(cmbUpLevel),
            MessageType  = GetMessageType(true),
            SequenceNo   = 0x01,           // 上行
            TargetIDs    = ids,
            FunctionCode = 0x34
        };

        // ── 組下行參數 ────────────────────────────────────────────────────
        private DisplayMessageParams BuildDnParams(List<byte> ids, string text = null) => new DisplayMessageParams
        {
            Text         = text ?? txtDnMsg.Text,
            HexColor     = NameToHex(cmbDnColor.SelectedItem?.ToString()),
            FontSz       = NameToFontSize(cmbDnFontSize.SelectedItem?.ToString()),
            FontSty      = NameToFontStyle(cmbDnFontStyle.SelectedItem?.ToString()),
            ScrollMode   = GetScrollMode(false),
            ScrollSpeed  = (byte)nudDnSpeed.Value,
            PauseTime    = (byte)nudDnPause.Value,
            MessageLevel = ParseLevel(cmbDnLevel),
            MessageType  = GetMessageType(false),
            SequenceNo   = 0x02,           // 下行
            TargetIDs    = ids,
            FunctionCode = 0x34
        };

        // ════════════════════════════════════════════════════════════════
        // 預覽 Paint
        // ════════════════════════════════════════════════════════════════

        private void pnlDisplay_Paint(object sender, PaintEventArgs e)
        {
            var g    = e.Graphics;
            int half = pnlDisplay.Height / 2;

            g.Clear(Color.Black);

            using (var pen = new Pen(Color.FromArgb(50, 50, 50)))
                g.DrawLine(pen, 0, half, pnlDisplay.Width, half);

            DrawRow(g, 0,    half, "上行", _upText, _upColor, _upFontPt);
            DrawRow(g, half, half, "下行", _dnText, _dnColor, _dnFontPt);
        }

        private static void DrawRow(Graphics g, int offsetY, int rowH,
            string dir, string text, Color color, float fontPt)
        {
            using (var sf = new Font("微軟正黑體", 7f))
            using (var db = new SolidBrush(Color.FromArgb(90, 90, 90)))
                g.DrawString(dir, sf, db, new PointF(3, offsetY + 2));

            if (string.IsNullOrEmpty(text)) return;

            using (var font  = new Font("微軟正黑體", fontPt, System.Drawing.FontStyle.Bold))
            using (var brush = new SolidBrush(color))
            {
                float y = offsetY + (rowH - font.Height) / 2f;
                if (y < offsetY) y = offsetY + 1;
                g.DrawString(text, font, brush, new PointF(22, y));
            }
        }

        // ════════════════════════════════════════════════════════════════
        // Visible 控制（Extra 子選項）
        // ════════════════════════════════════════════════════════════════

        private void SetUpTimeVisible(bool v)
        {
            lblUpTimeHdr.Visible  = v;
            cmbUpTimeType.Visible = v;
            pnlUpTimeClr.Visible  = v;
            cmbUpTimeClr.Visible  = v;
            if (v) UpdateUpCountdownVisible();
            else   SetUpCountdownVisible(false);
        }

        private void cmbUpTimeType_SelectedIndexChanged(object sender, EventArgs e)
            => UpdateUpCountdownVisible();

        private void UpdateUpCountdownVisible()
        {
            bool isCountdown = cmbUpTimeType.SelectedIndex == 1;
            SetUpCountdownVisible(isCountdown);
        }

        private void SetUpCountdownVisible(bool v)
        {
            lblUpCountStart.Visible     = v;
            nudUpCountStart.Visible     = v;
            lblUpCountStartTime.Visible = v;
            lblUpCountStop.Visible      = v;
            nudUpCountStop.Visible      = v;
            lblUpCountStopTime.Visible  = v;
        }

        private void nudUpCountStart_ValueChanged(object sender, EventArgs e)
            => lblUpCountStartTime.Text = CountToTime((int)nudUpCountStart.Value);

        private void nudUpCountStop_ValueChanged(object sender, EventArgs e)
            => lblUpCountStopTime.Text = CountToTime((int)nudUpCountStop.Value);

        private void SetUpPlatVisible(bool v)
        {
            lblUpPlatHdr.Visible   = v;
            lblUpPlatIdx.Visible   = v;
            nudUpPlatIdx.Visible   = v;
            pnlUpPlatThumb.Visible = v;
            pnlUpPlatClr.Visible   = v;
            cmbUpPlatClr.Visible   = v;
        }

        private void SetUpAlarmVisible(bool v)
        {
            pnlUpAlarm.Visible = v;
            // pnlUpAlarm 在 y=5，會蓋住上傳/存檔/多訊息按鈕，同步隱藏/顯示
            btnUpUpload.Visible  = !v;
            btnAllUpload.Visible = !v;
            btnDnUpload.Visible  = !v;
            btnSave.Visible      = !v;
            chkUpMulti.Visible   = !v;
            chkDnMulti.Visible   = !v;
        }

        private void rdoUpAlarmMsg_Click(object sender, EventArgs e)
        {
            rdoUpAlarmMsgOn.Checked  = (sender == rdoUpAlarmMsgOn);
            rdoUpAlarmMsgOff.Checked = (sender == rdoUpAlarmMsgOff);
        }

        private void rdoUpLight_Click(object sender, EventArgs e)
        {
            rdoUpLightOff.Checked   = (sender == rdoUpLightOff);
            rdoUpLightOn.Checked    = (sender == rdoUpLightOn);
            rdoUpLightBlink.Checked = (sender == rdoUpLightBlink);
        }

        private void SetDnTimeVisible(bool v)
        {
            lblDnTimeHdr.Visible  = v;
            cmbDnTimeType.Visible = v;
            pnlDnTimeClr.Visible  = v;
            cmbDnTimeClr.Visible  = v;
            if (v) UpdateDnCountdownVisible();
            else   SetDnCountdownVisible(false);
        }

        private void cmbDnTimeType_SelectedIndexChanged(object sender, EventArgs e)
            => UpdateDnCountdownVisible();

        private void UpdateDnCountdownVisible()
        {
            bool isCountdown = cmbDnTimeType.SelectedIndex == 1;
            SetDnCountdownVisible(isCountdown);
        }

        private void SetDnCountdownVisible(bool v)
        {
            lblDnCountStart.Visible     = v;
            nudDnCountStart.Visible     = v;
            lblDnCountStartTime.Visible = v;
            lblDnCountStop.Visible      = v;
            nudDnCountStop.Visible      = v;
            lblDnCountStopTime.Visible  = v;
        }

        private void nudDnCountStart_ValueChanged(object sender, EventArgs e)
            => lblDnCountStartTime.Text = CountToTime((int)nudDnCountStart.Value);

        private void nudDnCountStop_ValueChanged(object sender, EventArgs e)
            => lblDnCountStopTime.Text = CountToTime((int)nudDnCountStop.Value);

        private static string CountToTime(int value)
        {
            int totalSeconds = value * 5;
            int minutes      = totalSeconds / 60;
            int seconds      = totalSeconds % 60;
            return $"({minutes:D2}:{seconds:D2})";
        }

        private void SetDnPlatVisible(bool v)
        {
            lblDnPlatHdr.Visible   = v;
            lblDnPlatIdx.Visible   = v;
            nudDnPlatIdx.Visible   = v;
            pnlDnPlatThumb.Visible = v;
            pnlDnPlatClr.Visible   = v;
            cmbDnPlatClr.Visible   = v;
        }

        private void SetDnAlarmVisible(bool v)
            => pnlDnAlarm.Visible = v;

        private void rdoDnAlarmMsg_Click(object sender, EventArgs e)
        {
            rdoDnAlarmMsgOn.Checked  = (sender == rdoDnAlarmMsgOn);
            rdoDnAlarmMsgOff.Checked = (sender == rdoDnAlarmMsgOff);
        }

        private void rdoDnLight_Click(object sender, EventArgs e)
        {
            rdoDnLightOff.Checked   = (sender == rdoDnLightOff);
            rdoDnLightOn.Checked    = (sender == rdoDnLightOn);
            rdoDnLightBlink.Checked = (sender == rdoDnLightBlink);
        }

        // ════════════════════════════════════════════════════════════════
        // 工具：讀 UI → 參數
        // ════════════════════════════════════════════════════════════════

        /// <summary>讀取上行或下行目前選中的捲動模式 byte</summary>
        private byte GetScrollMode(bool isUp)
        {
            if (isUp)
            {
                if (rdoUpAct61.Checked) return 0x61;
                if (rdoUpAct62.Checked) return 0x62;
                if (rdoUpAct63.Checked) return 0x63;
                if (rdoUpAct64.Checked) return 0x64;
                if (rdoUpAct65.Checked) return 0x65;
                if (rdoUpAct66.Checked) return 0x66;
                if (rdoUpAct67.Checked) return 0x67;
            }
            else
            {
                if (rdoDnAct61.Checked) return 0x61;
                if (rdoDnAct62.Checked) return 0x62;
                if (rdoDnAct63.Checked) return 0x63;
                if (rdoDnAct64.Checked) return 0x64;
                if (rdoDnAct65.Checked) return 0x65;
                if (rdoDnAct66.Checked) return 0x66;
                if (rdoDnAct67.Checked) return 0x67;
            }
            return 0x61;
        }

        /// <summary>讀取上行或下行選中的板型 → MessageType byte</summary>
        private byte GetMessageType(bool isUp)
        {
            RadioButton[] radios = isUp
                ? new[] { rdoUpBoard1, rdoUpBoard2, rdoUpBoard3, rdoUpBoard4,
                          rdoUpBoard5, rdoUpBoard6, rdoUpBoard7, rdoUpBoard8 }
                : new[] { rdoDnBoard1, rdoDnBoard2, rdoDnBoard3, rdoDnBoard4,
                          rdoDnBoard5, rdoDnBoard6, rdoDnBoard7, rdoDnBoard8 };

            byte[] types = { 0x71, 0x74, 0x73, 0x82, 0x72, 0x71, 0x74, 0x83 };

            for (int i = 0; i < radios.Length; i++)
                if (radios[i].Checked) return types[i];

            return 0x71;
        }

        /// <summary>
        /// Level ComboBox 的 index 直接對應 Level 數值。
        /// 選項順序：index 0 = 最高Level 1, 1 = 高Level 2, 2 = 低Level 3, 3 = 最低Level 4
        /// </summary>
        private static byte ParseLevel(ComboBox cmb)
            => (byte)(cmb.SelectedIndex >= 0 ? cmb.SelectedIndex + 1 : 4);

        // ════════════════════════════════════════════════════════════════
        // 設定存讀
        // ════════════════════════════════════════════════════════════════

        private void SaveSettings()
        {
            var s = new DisplayMessageSettings
            {
                // 板型
                UpBoardIndex   = GetCheckedBoardIndex(true),
                DnBoardIndex   = GetCheckedBoardIndex(false),
                // 訊息頻型
                UpMsgTypeIndex = rdoUpPreRec.Checked ? 1 : 0,
                DnMsgTypeIndex = rdoDnPreRec.Checked ? 1 : 0,
                // 訊息文字
                UpMsg = txtUpMsg.Text,
                DnMsg = txtDnMsg.Text,
                // 字型參數
                UpFontSizeIndex  = cmbUpFontSize.SelectedIndex,
                UpFontStyleIndex = cmbUpFontStyle.SelectedIndex,
                UpColorIndex     = cmbUpColor.SelectedIndex,
                UpLevelIndex     = cmbUpLevel.SelectedIndex,
                DnFontSizeIndex  = cmbDnFontSize.SelectedIndex,
                DnFontStyleIndex = cmbDnFontStyle.SelectedIndex,
                DnColorIndex     = cmbDnColor.SelectedIndex,
                DnLevelIndex     = cmbDnLevel.SelectedIndex,
                // 動作方式
                UpScrollAction = GetScrollMode(true),
                DnScrollAction = GetScrollMode(false),
                // 動作參數
                UpSpeed = (int)nudUpSpeed.Value,
                UpPause = (int)nudUpPause.Value,
                DnSpeed = (int)nudDnSpeed.Value,
                DnPause = (int)nudDnPause.Value,
                // Extra
                UpTimeTypeIndex = cmbUpTimeType.SelectedIndex,
                UpTimeClrIndex  = cmbUpTimeClr.SelectedIndex,
                DnTimeTypeIndex = cmbDnTimeType.SelectedIndex,
                DnTimeClrIndex  = cmbDnTimeClr.SelectedIndex,
                UpPlatIdx       = (int)nudUpPlatIdx.Value,
                UpPlatClrIndex  = cmbUpPlatClr.SelectedIndex,
                DnPlatIdx       = (int)nudDnPlatIdx.Value,
                DnPlatClrIndex  = cmbDnPlatClr.SelectedIndex,
            };

            var serializer = new XmlSerializer(typeof(DisplayMessageSettings));
            using (var writer = new StreamWriter(SettingsPath, append: false, encoding: Encoding.UTF8))
                serializer.Serialize(writer, s);
        }

        private void LoadSettings()
        {
            if (!File.Exists(SettingsPath)) return;

            try
            {
                DisplayMessageSettings s;
                var serializer = new XmlSerializer(typeof(DisplayMessageSettings));
                using (var reader = new StreamReader(SettingsPath, Encoding.UTF8))
                    s = (DisplayMessageSettings)serializer.Deserialize(reader);

                // 板型
                SetCheckedBoard(true,  s.UpBoardIndex);
                SetCheckedBoard(false, s.DnBoardIndex);
                // 訊息頻型
                rdoUpGeneral.Checked = s.UpMsgTypeIndex == 0;
                rdoUpPreRec.Checked  = s.UpMsgTypeIndex == 1;
                rdoDnGeneral.Checked = s.DnMsgTypeIndex == 0;
                rdoDnPreRec.Checked  = s.DnMsgTypeIndex == 1;
                // 訊息文字
                txtUpMsg.Text = s.UpMsg ?? "";
                txtDnMsg.Text = s.DnMsg ?? "";
                // 字型參數
                SetComboIndex(cmbUpFontSize,  s.UpFontSizeIndex);
                SetComboIndex(cmbUpFontStyle, s.UpFontStyleIndex);
                SetComboIndex(cmbUpColor,     s.UpColorIndex);
                SetComboIndex(cmbUpLevel,     s.UpLevelIndex);
                SetComboIndex(cmbDnFontSize,  s.DnFontSizeIndex);
                SetComboIndex(cmbDnFontStyle, s.DnFontStyleIndex);
                SetComboIndex(cmbDnColor,     s.DnColorIndex);
                SetComboIndex(cmbDnLevel,     s.DnLevelIndex);
                // 動作方式
                SetScrollAction(true,  s.UpScrollAction);
                SetScrollAction(false, s.DnScrollAction);
                // 動作參數
                nudUpSpeed.Value = Clamp(s.UpSpeed, nudUpSpeed);
                nudUpPause.Value = Clamp(s.UpPause, nudUpPause);
                nudDnSpeed.Value = Clamp(s.DnSpeed, nudDnSpeed);
                nudDnPause.Value = Clamp(s.DnPause, nudDnPause);
                // Extra
                SetComboIndex(cmbUpTimeType, s.UpTimeTypeIndex);
                SetComboIndex(cmbUpTimeClr,  s.UpTimeClrIndex);
                SetComboIndex(cmbDnTimeType, s.DnTimeTypeIndex);
                SetComboIndex(cmbDnTimeClr,  s.DnTimeClrIndex);
                nudUpPlatIdx.Value = Clamp(s.UpPlatIdx, nudUpPlatIdx);
                SetComboIndex(cmbUpPlatClr,  s.UpPlatClrIndex);
                nudDnPlatIdx.Value = Clamp(s.DnPlatIdx, nudDnPlatIdx);
                SetComboIndex(cmbDnPlatClr,  s.DnPlatClrIndex);
            }
            catch
            {
                // 設定檔損毀時靜默忽略，使用預設值
            }
        }

        // ── LoadSettings 輔助 ─────────────────────────────────────────

        private static void SetComboIndex(ComboBox cmb, int index)
        {
            if (index >= 0 && index < cmb.Items.Count)
                cmb.SelectedIndex = index;
        }

        private static decimal Clamp(int value, NumericUpDown nud)
            => Math.Max(nud.Minimum, Math.Min(nud.Maximum, value));

        private int GetCheckedBoardIndex(bool isUp)
        {
            var radios = isUp ? _upBoardRadios : _dnBoardRadios;
            for (int i = 0; i < radios.Length; i++)
                if (radios[i].Checked) return i;
            return 0;
        }

        private void SetCheckedBoard(bool isUp, int index)
        {
            var radios = isUp ? _upBoardRadios : _dnBoardRadios;
            if (index >= 0 && index < radios.Length)
                radios[index].Checked = true;
        }

        private void SetScrollAction(bool isUp, byte action)
        {
            if (isUp)
            {
                rdoUpAct61.Checked = action == 0x61;
                rdoUpAct62.Checked = action == 0x62;
                rdoUpAct63.Checked = action == 0x63;
                rdoUpAct64.Checked = action == 0x64;
                rdoUpAct65.Checked = action == 0x65;
                rdoUpAct66.Checked = action == 0x66;
                rdoUpAct67.Checked = action == 0x67;
            }
            else
            {
                rdoDnAct61.Checked = action == 0x61;
                rdoDnAct62.Checked = action == 0x62;
                rdoDnAct63.Checked = action == 0x63;
                rdoDnAct64.Checked = action == 0x64;
                rdoDnAct65.Checked = action == 0x65;
                rdoDnAct66.Checked = action == 0x66;
                rdoDnAct67.Checked = action == 0x67;
            }
        }

        // ════════════════════════════════════════════════════════════════
        // 顏色 ComboBox：資料 + 繪製
        // ════════════════════════════════════════════════════════════════

        /// <summary>Designer 用的 object[]，直接取自 ColorPalette（只建立一次）。</summary>
        internal static object[] ColorComboItems => ColorPalette.ComboItems;

        /// <summary>Owner-draw：在每個顏色項目左側畫色塊，右側顯示名稱與 hex 碼。</summary>
        private void cmbColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb == null || e.Index < 0) return;

            e.DrawBackground();

            string name  = cmb.Items[e.Index].ToString();
            Color  color = ColorPalette.GetColor(name);
            string hex   = $"#{color.R:X2}{color.G:X2}{color.B:X2}";

            // 色塊
            var swatchRect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, 18, e.Bounds.Height - 4);
            using (var fill = new SolidBrush(color))
                e.Graphics.FillRectangle(fill, swatchRect);
            e.Graphics.DrawRectangle(Pens.DarkGray, swatchRect);

            // 文字：名稱 + hex
            string label = $"{name}  {hex}";
            using (var brush = new SolidBrush(e.ForeColor))
                e.Graphics.DrawString(label, e.Font, brush,
                    new PointF(e.Bounds.X + 24, e.Bounds.Y + (e.Bounds.Height - e.Font.Height) / 2f));

            e.DrawFocusRectangle();
        }

        /// <summary>顏色選項變更時，同步更新對應色塊 Panel。</summary>
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb == null) return;
            Color c = ColorPalette.GetColor(cmb.SelectedItem?.ToString());

            if      (cmb == cmbUpColor)    pnlUpColor.BackColor    = c;
            else if (cmb == cmbDnColor)    pnlDnColor.BackColor    = c;
            else if (cmb == cmbUpTimeClr)  pnlUpTimeClr.BackColor  = c;
            else if (cmb == cmbDnTimeClr)  pnlDnTimeClr.BackColor  = c;
            else if (cmb == cmbUpPlatClr)  pnlUpPlatClr.BackColor  = c;
            else if (cmb == cmbDnPlatClr)  pnlDnPlatClr.BackColor  = c;
        }

        private static string NameToHex(string name) => ColorPalette.GetHex(name);

        private static FontSize NameToFontSize(string s)
        {
            if (s == FontName16x16) return FontSize.Font16x16;
            if (s == FontName5x7)   return FontSize.Font5x7;
            return FontSize.Font24x24;
        }

        private static Display.FontStyle NameToFontStyle(string s)
        {
            if (s == "黑體") return Display.FontStyle.Hei;
            if (s == "楷體") return Display.FontStyle.Kai;
            return Display.FontStyle.Ming;
        }

        private static Color ParseColor(string name) => ColorPalette.GetColor(name);

        private static float SizeToPt(string size)
        {
            if (size == FontName16x16) return 9f;
            if (size == FontName5x7)   return 7f;
            return 13f;
        }

        // GetBoardIndex(RadioButton) 已廢棄，請改用 Array.IndexOf(_upBoardRadios / _dnBoardRadios, rdo)
    }
}
