using System;
using System.Drawing;
using System.Windows.Forms;

namespace UITest.Controls
{
    /// <summary>
    /// 板型子選項彈出視窗（時間 / 倒數 / 月台碼）。
    /// 完全以程式碼建構控件，避免 VS Designer DPI 縮放問題。
    /// </summary>
    public class BoardOptionsForm : Form
    {
        // ── 回傳值結構 ──────────────────────────────────────────────────
        public sealed class SideOptions
        {
            public int  TimeTypeIdx  { get; set; } = 0;   // 0=標準時間, 1=開始倒數
            public int  TimeClrIdx   { get; set; } = 0;   // ColorPalette index
            public int  CountStart   { get; set; } = 12;
            public int  CountStop    { get; set; } = 0;
            public int  PlatIdx      { get; set; } = 1;
            public int  PlatClrIdx   { get; set; } = 0;
        }

        // 外部讀取用
        public SideOptions UpResult { get; } = new SideOptions();
        public SideOptions DnResult { get; } = new SideOptions();

        // ── 旗標 ───────────────────────────────────────────────────────
        private readonly bool _showTime;
        private readonly bool _showPlat;

        // ── Up 端控件 ──────────────────────────────────────────────────
        private ComboBox _cmbUpTimeType;
        private Panel    _pnlUpTimeClr;
        private ComboBox _cmbUpTimeClr;
        private NumericUpDown _nudUpCountStart;
        private Label    _lblUpCountStart;
        private NumericUpDown _nudUpCountStop;
        private Label    _lblUpCountStop;
        private NumericUpDown _nudUpPlatIdx;
        private Panel    _pnlUpPlatClr;
        private ComboBox _cmbUpPlatClr;

        // ── Dn 端控件 ──────────────────────────────────────────────────
        private ComboBox _cmbDnTimeType;
        private Panel    _pnlDnTimeClr;
        private ComboBox _cmbDnTimeClr;
        private NumericUpDown _nudDnCountStart;
        private Label    _lblDnCountStart;
        private NumericUpDown _nudDnCountStop;
        private Label    _lblDnCountStop;
        private NumericUpDown _nudDnPlatIdx;
        private Panel    _pnlDnPlatClr;
        private ComboBox _cmbDnPlatClr;

        // ── 建構子 ─────────────────────────────────────────────────────
        public BoardOptionsForm(bool showTime, bool showPlat,
                                SideOptions upInit, SideOptions dnInit)
        {
            _showTime = showTime;
            _showPlat = showPlat;

            // ── Form 本身設定 ──────────────────────────────────────────
            this.Text            = "板型子選項設定";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.AutoScaleMode   = AutoScaleMode.None;   // 防止 DPI 縮放

            BuildUI(upInit, dnInit);
        }

        // ── 建構 UI ────────────────────────────────────────────────────
        private void BuildUI(SideOptions upInit, SideOptions dnInit)
        {
            int colW   = 280;    // 每欄寬度
            int margin = 12;
            int y      = margin;

            // ── 時間區段 ──────────────────────────────────────────────
            if (_showTime)
            {
                AddSectionHeader("時間(右側顯示)", margin, y, colW * 2 + margin);
                y += 24;

                // 上行
                _cmbUpTimeType  = AddCombo(new[] { "標準時間", "開始倒數" }, margin, y, 120);
                _pnlUpTimeClr   = AddColorPanel(margin + 128, y + 2, 18);
                _cmbUpTimeClr   = AddColorCombo(margin + 150, y, 110);
                // 下行
                _cmbDnTimeType  = AddCombo(new[] { "標準時間", "開始倒數" }, margin + colW, y, 120);
                _pnlDnTimeClr   = AddColorPanel(margin + colW + 128, y + 2, 18);
                _cmbDnTimeClr   = AddColorCombo(margin + colW + 150, y, 110);
                y += 28;

                // 倒數行（初始隱藏）
                _lblUpCountStart = AddLabel("開始倒數", margin, y);
                _nudUpCountStart = AddSpinner(margin + 58, y - 2, 0, 255);
                _lblUpCountStop  = AddLabel("停止倒數", margin, y + 26);
                _nudUpCountStop  = AddSpinner(margin + 58, y + 24, 0, 255);

                _lblDnCountStart = AddLabel("開始倒數", margin + colW, y);
                _nudDnCountStart = AddSpinner(margin + colW + 58, y - 2, 0, 255);
                _lblDnCountStop  = AddLabel("停止倒數", margin + colW, y + 26);
                _nudDnCountStop  = AddSpinner(margin + colW + 58, y + 24, 0, 255);
                y += 58;

                // 初始值
                SetComboIdx(_cmbUpTimeType, upInit.TimeTypeIdx);
                SetComboIdx(_cmbUpTimeClr,  upInit.TimeClrIdx);
                _nudUpCountStart.Value = upInit.CountStart;
                _nudUpCountStop.Value  = upInit.CountStop;
                SetComboIdx(_cmbDnTimeType, dnInit.TimeTypeIdx);
                SetComboIdx(_cmbDnTimeClr,  dnInit.TimeClrIdx);
                _nudDnCountStart.Value = dnInit.CountStart;
                _nudDnCountStop.Value  = dnInit.CountStop;

                UpdateTimeClrPanel(_pnlUpTimeClr, _cmbUpTimeClr);
                UpdateTimeClrPanel(_pnlDnTimeClr, _cmbDnTimeClr);
                UpdateCountdownVisible(true);

                // 事件
                _cmbUpTimeType.SelectedIndexChanged += (s, e) => UpdateCountdownVisible(true);
                _cmbDnTimeType.SelectedIndexChanged += (s, e) => UpdateCountdownVisible(false);
                _cmbUpTimeClr.SelectedIndexChanged  += (s, e) => UpdateTimeClrPanel(_pnlUpTimeClr, _cmbUpTimeClr);
                _cmbDnTimeClr.SelectedIndexChanged  += (s, e) => UpdateTimeClrPanel(_pnlDnTimeClr, _cmbDnTimeClr);
            }

            // ── 月台碼區段 ────────────────────────────────────────────
            if (_showPlat)
            {
                AddSectionHeader("月台碼設定", margin, y, colW * 2 + margin);
                y += 24;

                AddLabel("圖檔索引值", margin, y);
                _nudUpPlatIdx = AddSpinner(margin + 65, y - 2, 1, 9);
                _pnlUpPlatClr = AddColorPanel(margin + 130, y, 18);
                _cmbUpPlatClr = AddColorCombo(margin + 152, y - 2, 110);

                AddLabel("圖檔索引值", margin + colW, y);
                _nudDnPlatIdx = AddSpinner(margin + colW + 65, y - 2, 1, 9);
                _pnlDnPlatClr = AddColorPanel(margin + colW + 130, y, 18);
                _cmbDnPlatClr = AddColorCombo(margin + colW + 152, y - 2, 110);
                y += 30;

                _nudUpPlatIdx.Value = upInit.PlatIdx;
                SetComboIdx(_cmbUpPlatClr, upInit.PlatClrIdx);
                _nudDnPlatIdx.Value = dnInit.PlatIdx;
                SetComboIdx(_cmbDnPlatClr, dnInit.PlatClrIdx);

                UpdateTimeClrPanel(_pnlUpPlatClr, _cmbUpPlatClr);
                UpdateTimeClrPanel(_pnlDnPlatClr, _cmbDnPlatClr);

                _cmbUpPlatClr.SelectedIndexChanged += (s, e) => UpdateTimeClrPanel(_pnlUpPlatClr, _cmbUpPlatClr);
                _cmbDnPlatClr.SelectedIndexChanged += (s, e) => UpdateTimeClrPanel(_pnlDnPlatClr, _cmbDnPlatClr);
            }

            y += margin;

            // ── 欄位標題 ──────────────────────────────────────────────
            var lblUp = new Label { Text = "▲ 上行", AutoSize = true,
                Font = new Font(Font, FontStyle.Bold),
                Location = new Point(margin + colW / 2 - 20, 6) };
            var lblDn = new Label { Text = "▼ 下行", AutoSize = true,
                Font = new Font(Font, FontStyle.Bold),
                Location = new Point(margin + colW + colW / 2 - 20, 6) };
            Controls.Add(lblUp);
            Controls.Add(lblDn);

            // ── OK 按鈕 ───────────────────────────────────────────────
            var btnOK = new Button
            {
                Text     = "確定",
                Size     = new Size(80, 28),
                Location = new Point((colW * 2 + margin * 2 - 80) / 2, y),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (s, e) => CommitValues();
            Controls.Add(btnOK);
            AcceptButton = btnOK;
            y += 38;

            // ── Form 尺寸 ─────────────────────────────────────────────
            this.ClientSize = new Size(colW * 2 + margin * 2, y);
        }

        // ── 讀回控件值 ────────────────────────────────────────────────
        private void CommitValues()
        {
            if (_showTime)
            {
                UpResult.TimeTypeIdx  = _cmbUpTimeType.SelectedIndex;
                UpResult.TimeClrIdx   = _cmbUpTimeClr.SelectedIndex;
                UpResult.CountStart   = (int)_nudUpCountStart.Value;
                UpResult.CountStop    = (int)_nudUpCountStop.Value;
                DnResult.TimeTypeIdx  = _cmbDnTimeType.SelectedIndex;
                DnResult.TimeClrIdx   = _cmbDnTimeClr.SelectedIndex;
                DnResult.CountStart   = (int)_nudDnCountStart.Value;
                DnResult.CountStop    = (int)_nudDnCountStop.Value;
            }
            if (_showPlat)
            {
                UpResult.PlatIdx      = (int)_nudUpPlatIdx.Value;
                UpResult.PlatClrIdx   = _cmbUpPlatClr.SelectedIndex;
                DnResult.PlatIdx      = (int)_nudDnPlatIdx.Value;
                DnResult.PlatClrIdx   = _cmbDnPlatClr.SelectedIndex;
            }
        }

        // ── 倒數行顯示切換 ────────────────────────────────────────────
        private void UpdateCountdownVisible(bool isUp)
        {
            if (!_showTime) return;
            if (isUp)
            {
                bool v = _cmbUpTimeType.SelectedIndex == 1;
                _lblUpCountStart.Visible = v;
                _nudUpCountStart.Visible = v;
                _lblUpCountStop.Visible  = v;
                _nudUpCountStop.Visible  = v;
            }
            else
            {
                bool v = _cmbDnTimeType.SelectedIndex == 1;
                _lblDnCountStart.Visible = v;
                _nudDnCountStart.Visible = v;
                _lblDnCountStop.Visible  = v;
                _nudDnCountStop.Visible  = v;
            }
        }

        // ── 顏色方塊更新 ──────────────────────────────────────────────
        private static void UpdateTimeClrPanel(Panel pnl, ComboBox cmb)
        {
            if (cmb.SelectedIndex >= 0)
                pnl.BackColor = ColorPalette.GetColor(cmb.SelectedItem?.ToString());
        }

        // ── 工廠方法 ──────────────────────────────────────────────────
        private Label AddLabel(string text, int x, int y)
        {
            var lbl = new Label { Text = text, AutoSize = true, Location = new Point(x, y) };
            Controls.Add(lbl);
            return lbl;
        }

        private void AddSectionHeader(string text, int x, int y, int width)
        {
            var lbl = new Label
            {
                Text      = text,
                AutoSize  = false,
                Size      = new Size(width, 20),
                Location  = new Point(x, y),
                Font      = new Font(Font, FontStyle.Bold),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(220, 230, 245),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(4, 0, 0, 0)
            };
            Controls.Add(lbl);
        }

        private ComboBox AddCombo(string[] items, int x, int y, int width)
        {
            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(x, y),
                Size     = new Size(width, 21)
            };
            cmb.Items.AddRange(items);
            cmb.SelectedIndex = 0;
            Controls.Add(cmb);
            return cmb;
        }

        private ComboBox AddColorCombo(int x, int y, int width)
        {
            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DrawMode      = DrawMode.OwnerDrawFixed,
                ItemHeight    = 18,
                Location      = new Point(x, y),
                Size          = new Size(width, 21)
            };
            cmb.Items.AddRange(ColorPalette.ComboItems);
            cmb.SelectedIndex = 0;
            cmb.DrawItem += CmbColor_DrawItem;
            Controls.Add(cmb);
            return cmb;
        }

        private Panel AddColorPanel(int x, int y, int size)
        {
            var pnl = new Panel
            {
                Location    = new Point(x, y),
                Size        = new Size(size, size),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor   = Color.Black
            };
            Controls.Add(pnl);
            return pnl;
        }

        private NumericUpDown AddSpinner(int x, int y, int min, int max)
        {
            var nud = new NumericUpDown
            {
                Location = new Point(x, y),
                Size     = new Size(55, 22),
                Minimum  = min,
                Maximum  = max,
                Value    = min
            };
            Controls.Add(nud);
            return nud;
        }

        private static void SetComboIdx(ComboBox cmb, int idx)
        {
            if (idx >= 0 && idx < cmb.Items.Count)
                cmb.SelectedIndex = idx;
        }

        // ── 顏色 ComboBox 繪製 ────────────────────────────────────────
        private void CmbColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            var entry = ColorPalette.Entries[e.Index];
            using (var brush = new SolidBrush(entry.Value))
                e.Graphics.FillRectangle(brush, e.Bounds.X + 2, e.Bounds.Y + 2, 16, e.Bounds.Height - 4);
            using (var brush = new SolidBrush(e.ForeColor))
                e.Graphics.DrawString(entry.Name, e.Font,
                    brush, e.Bounds.X + 22, e.Bounds.Y + 1);
            e.DrawFocusRectangle();
        }
    }
}
