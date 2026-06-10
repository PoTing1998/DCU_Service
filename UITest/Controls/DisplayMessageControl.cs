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

        // ════════════════════════════════════════════════════════════════
        // 顏色 ComboBox：資料 + 繪製
        // ════════════════════════════════════════════════════════════════

        /// <summary>顏色名稱 → System.Drawing.Color 對照表（共 30 色）</summary>
        private static readonly (string Name, Color Value)[] ColorEntries =
        {
            ("clBlack",       Color.Black),
            ("clWhite",       Color.White),
            ("clRed",         Color.FromArgb(0xFF, 0x00, 0x00)),
            ("clYellow",      Color.FromArgb(0xFF, 0xFF, 0x00)),
            ("clGreen",       Color.FromArgb(0x00, 0xFF, 0x00)),
            ("clBlue",        Color.FromArgb(0x00, 0x00, 0xFF)),
            ("clCyan",        Color.FromArgb(0x00, 0xFF, 0xFF)),
            ("clMagenta",     Color.FromArgb(0xFF, 0x00, 0xFF)),
            ("clOrange",      Color.FromArgb(0xFF, 0xA5, 0x00)),
            ("clGold",        Color.FromArgb(0xFF, 0xD7, 0x00)),
            ("clPink",        Color.FromArgb(0xFF, 0xC0, 0xCB)),
            ("clDeepPink",    Color.FromArgb(0xFF, 0x14, 0x93)),
            ("clCoral",       Color.FromArgb(0xFF, 0x7F, 0x50)),
            ("clSalmon",      Color.FromArgb(0xFA, 0x80, 0x72)),
            ("clTomato",      Color.FromArgb(0xFF, 0x63, 0x47)),
            ("clOrangeRed",   Color.FromArgb(0xFF, 0x45, 0x00)),
            ("clLime",        Color.FromArgb(0x32, 0xCD, 0x32)),
            ("clSpringGreen", Color.FromArgb(0x00, 0xFF, 0x7F)),
            ("clTurquoise",   Color.FromArgb(0x40, 0xE0, 0xD0)),
            ("clSkyBlue",     Color.FromArgb(0x87, 0xCE, 0xEB)),
            ("clDodgerBlue",  Color.FromArgb(0x1E, 0x90, 0xFF)),
            ("clNavy",        Color.FromArgb(0x00, 0x00, 0x80)),
            ("clTeal",        Color.FromArgb(0x00, 0x80, 0x80)),
            ("clPurple",      Color.FromArgb(0x80, 0x00, 0x80)),
            ("clViolet",      Color.FromArgb(0xEE, 0x82, 0xEE)),
            ("clIndigo",      Color.FromArgb(0x4B, 0x00, 0x82)),
            ("clMaroon",      Color.FromArgb(0x80, 0x00, 0x00)),
            ("clOlive",       Color.FromArgb(0x80, 0x80, 0x00)),
            ("clGray",        Color.FromArgb(0x80, 0x80, 0x80)),
            ("clSilver",      Color.FromArgb(0xC0, 0xC0, 0xC0)),
        };

        /// <summary>Designer 用的 object[] 色名陣列（shared across all color combos）</summary>
        internal static object[] ColorComboItems
            => Array.ConvertAll(ColorEntries, e => (object)e.Name);

        private static Color GetColorByName(string name)
        {
            if (name == null) return Color.White;
            foreach (var e in ColorEntries)
                if (e.Name == name) return e.Value;
            return Color.White;
        }

        private static string GetHexByName(string name)
        {
            Color c = GetColorByName(name);
            return $"{c.R:X2}{c.G:X2}{c.B:X2}";
        }

        /// <summary>Owner-draw：在每個顏色項目左側畫色塊，右側顯示名稱與 hex 碼。</summary>
        private void cmbColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb == null || e.Index < 0) return;

            e.DrawBackground();

            string name  = cmb.Items[e.Index].ToString();
            Color  color = GetColorByName(name);
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
            Color c = GetColorByName(cmb.SelectedItem?.ToString());

            if      (cmb == cmbUpColor)    pnlUpColor.BackColor    = c;
            else if (cmb == cmbDnColor)    pnlDnColor.BackColor    = c;
            else if (cmb == cmbUpTimeClr)  pnlUpTimeClr.BackColor  = c;
            else if (cmb == cmbDnTimeClr)  pnlDnTimeClr.BackColor  = c;
            else if (cmb == cmbUpPlatClr)  pnlUpPlatClr.BackColor  = c;
            else if (cmb == cmbDnPlatClr)  pnlDnPlatClr.BackColor  = c;
        }

        private static string NameToHex(string name) => GetHexByName(name);

        private static FontSize NameToFontSize(string s)
        {
            switch (s)
            {
                case "16x16":   return FontSize.Font16x16;
                case "英文 5x7": return FontSize.Font5x7;
                default:        return FontSize.Font24x24;
            }
        }

        private static Display.FontStyle NameToFontStyle(string s)
        {
            if (s == "黑體") return Display.FontStyle.Hei;
            if (s == "楷體") return Display.FontStyle.Kai;
            return Display.FontStyle.Ming;
        }

        private static Color ParseColor(string name) => GetColorByName(name);

        private static float SizeToPt(string size)
        {
            switch (size)
            {
                case "16x16":   return 9f;
                case "英文 5x7": return 7f;
                default:        return 13f;
            }
        }

        private static int GetBoardIndex(RadioButton rdo)
            => (rdo?.Text?.Length > 0 && char.IsDigit(rdo.Text[0])) ? (rdo.Text[0] - '0') : 0;
    }
}
