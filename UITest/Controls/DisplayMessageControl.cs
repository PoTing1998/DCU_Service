using System;
using System.Drawing;
using System.Windows.Forms;

namespace UITest.Controls
{
    public partial class DisplayMessageControl : UserControl
    {
        // ── 預覽狀態 ─────────────────────────────────────────────
        private string _upText   = "";
        private Color  _upColor  = Color.Yellow;
        private float  _upFontPt = 14f;

        private string _dnText   = "";
        private Color  _dnColor  = Color.Yellow;
        private float  _dnFontPt = 14f;

        public DisplayMessageControl()
        {
            InitializeComponent();
        }

        // ── 上傳 / 存檔 ───────────────────────────────────────────

        private void btnUpUpload_Click(object sender, EventArgs e)
        {
            MessageBox.Show("上行上傳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAllUpload_Click(object sender, EventArgs e)
        {
            MessageBox.Show("全部上傳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDnUpload_Click(object sender, EventArgs e)
        {
            MessageBox.Show("下行上傳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("存檔完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── 上行訊息 ──────────────────────────────────────────────

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

        // ── 下行訊息 ──────────────────────────────────────────────

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

        // ── 板型選項變更 → 動態顯示子設定 ───────────────────────

        private void rdoUpBoard_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = sender as RadioButton;
            if (rdo == null || !rdo.Checked) return;

            int idx = GetBoardIndex(rdo);
            // 時間設定：選項 2, 3, 7
            bool showTime = (idx == 2 || idx == 3 || idx == 7);
            // 月台碼設定：選項 3, 5
            bool showPlat = (idx == 3 || idx == 5);

            SetUpTimeVisible(showTime);
            SetUpPlatVisible(showPlat);
        }

        private void rdoDnBoard_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = sender as RadioButton;
            if (rdo == null || !rdo.Checked) return;

            int idx = GetBoardIndex(rdo);
            bool showTime = (idx == 2 || idx == 3 || idx == 7);
            bool showPlat = (idx == 3 || idx == 5);

            SetDnTimeVisible(showTime);
            SetDnPlatVisible(showPlat);
        }

        // ── Visible 控制 ─────────────────────────────────────────

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

        // ── 預覽 Paint ────────────────────────────────────────────

        private void pnlDisplay_Paint(object sender, PaintEventArgs e)
        {
            Graphics g    = e.Graphics;
            int      half = pnlDisplay.Height / 2;

            g.Clear(Color.Black);

            using (var divPen = new Pen(Color.FromArgb(50, 50, 50)))
                g.DrawLine(divPen, 0, half, pnlDisplay.Width, half);

            DrawRow(g, 0,    half, "上行", _upText, _upColor, _upFontPt);
            DrawRow(g, half, half, "下行", _dnText, _dnColor, _dnFontPt);
        }

        private void DrawRow(Graphics g, int offsetY, int rowH,
                             string dir, string text, Color color, float fontPt)
        {
            using (var smallFont = new Font("微軟正黑體", 7f))
            using (var dimBrush  = new SolidBrush(Color.FromArgb(90, 90, 90)))
                g.DrawString(dir, smallFont, dimBrush, new PointF(3, offsetY + 2));

            if (string.IsNullOrEmpty(text)) return;

            using (var font  = new Font("微軟正黑體", fontPt, System.Drawing.FontStyle.Bold))
            using (var brush = new SolidBrush(color))
            {
                float y = offsetY + (rowH - font.Height) / 2f;
                if (y < offsetY) y = offsetY + 1;
                g.DrawString(text, font, brush, new PointF(22, y));
            }
        }

        // ── 工具 ─────────────────────────────────────────────────

        /// <summary>從 RadioButton.Text 第一個數字取得板型編號（1~8）</summary>
        private static int GetBoardIndex(RadioButton rdo)
        {
            if (rdo?.Text?.Length > 0 && char.IsDigit(rdo.Text[0]))
                return rdo.Text[0] - '0';
            return 0;
        }

        private static Color ParseColor(string name)
        {
            switch (name)
            {
                case "clYellow": return Color.Yellow;
                case "clRed":    return Color.Red;
                case "clGreen":  return Color.Lime;
                case "clWhite":  return Color.White;
                default:         return Color.Yellow;
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
    }
}
