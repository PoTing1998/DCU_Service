using Display;
using Display.DisplayMode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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

        public DisplayMessageControl()
        {
            InitializeComponent();
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
            // TODO: 儲存至設定檔
            MessageBox.Show("存檔完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            int idx = GetBoardIndex(rdo);
            SetUpTimeVisible(idx == 2 || idx == 3 || idx == 7);
            SetUpPlatVisible(idx == 3 || idx == 5);
        }

        private void rdoDnBoard_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = sender as RadioButton;
            if (rdo == null || !rdo.Checked) return;
            int idx = GetBoardIndex(rdo);
            SetDnTimeVisible(idx == 2 || idx == 3 || idx == 7);
            SetDnPlatVisible(idx == 3 || idx == 5);
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
                else
                {
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
                        {
                            mon.Log(Services.ConnectionMonitor.LogLevel.Recv, "DisplayMsg", "上行傳送成功");
                        }
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
                else
                {
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
                        {
                            mon.Log(Services.ConnectionMonitor.LogLevel.Recv, "DisplayMsg", "下行傳送成功");
                        }
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
        private DisplayMessageParams BuildUpParams(List<byte> ids) => new DisplayMessageParams
        {
            Text         = txtUpMsg.Text,
            HexColor     = NameToHex(cmbUpColor.SelectedItem?.ToString()),
            FontSz       = NameToFontSize(cmbUpFontSize.SelectedItem?.ToString()),
            FontSty      = NameToFontStyle(cmbUpFontStyle.SelectedItem?.ToString()),
            ScrollMode   = GetScrollMode(true),
            ScrollSpeed  = (byte)nudUpSpeed.Value,
            PauseTime    = (byte)nudUpPause.Value,
            MessageLevel = ParseLevel(cmbUpLevel.SelectedItem?.ToString()),
            MessageType  = GetMessageType(true),
            SequenceNo   = 0x01,           // 上行
            TargetIDs    = ids,
            FunctionCode = 0x34
        };

        // ── 組下行參數 ────────────────────────────────────────────────────
        private DisplayMessageParams BuildDnParams(List<byte> ids) => new DisplayMessageParams
        {
            Text         = txtDnMsg.Text,
            HexColor     = NameToHex(cmbDnColor.SelectedItem?.ToString()),
            FontSz       = NameToFontSize(cmbDnFontSize.SelectedItem?.ToString()),
            FontSty      = NameToFontStyle(cmbDnFontStyle.SelectedItem?.ToString()),
            ScrollMode   = GetScrollMode(false),
            ScrollSpeed  = (byte)nudDnSpeed.Value,
            PauseTime    = (byte)nudDnPause.Value,
            MessageLevel = ParseLevel(cmbDnLevel.SelectedItem?.ToString()),
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
        }

        private void SetUpPlatVisible(bool v)
        {
            lblUpPlatHdr.Visible   = v;
            lblUpPlatIdx.Visible   = v;
            nudUpPlatIdx.Visible   = v;
            pnlUpPlatThumb.Visible = v;
            pnlUpPlatClr.Visible   = v;
            cmbUpPlatClr.Visible   = v;
        }

        private void SetDnTimeVisible(bool v)
        {
            lblDnTimeHdr.Visible  = v;
            cmbDnTimeType.Visible = v;
            pnlDnTimeClr.Visible  = v;
            cmbDnTimeClr.Visible  = v;
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

        private static byte ParseLevel(string s)
        {
            if (s == null) return 4;
            if (s.Contains("Level 1")) return 1;
            if (s.Contains("Level 2")) return 2;
            if (s.Contains("Level 3")) return 3;
            return 4;
        }

        private static string NameToHex(string name)
        {
            switch (name)
            {
                case "clYellow": return "FFFF00";
                case "clRed":    return "FF0000";
                case "clGreen":  return "00FF00";
                default:         return "FFFFFF";
            }
        }

        private static FontSize NameToFontSize(string s)
        {
            switch (s)
            {
                case "16x16": return FontSize.Font16x16;
                case "32x32": return FontSize.Font5x7;   // 規格中最大字型
                default:      return FontSize.Font24x24;
            }
        }

        private static Display.FontStyle NameToFontStyle(string s)
            => (s == "黑體") ? Display.FontStyle.Hei : Display.FontStyle.Ming;

        private static Color ParseColor(string name)
        {
            switch (name)
            {
                case "clYellow": return Color.Yellow;
                case "clRed":    return Color.Red;
                case "clGreen":  return Color.Lime;
                default:         return Color.White;
            }
        }

        private static float SizeToPt(string size)
        {
            switch (size)
            {
                case "16x16": return 9f;
                case "32x32": return 18f;
                default:      return 13f;
            }
        }

        private static int GetBoardIndex(RadioButton rdo)
            => (rdo?.Text?.Length > 0 && char.IsDigit(rdo.Text[0])) ? (rdo.Text[0] - '0') : 0;
    }
}
