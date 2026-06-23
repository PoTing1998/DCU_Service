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
        /// <summary>取得目前勾選的大廳層 ID 清單。</summary>
        public Func<List<byte>>     GetLobbyIDsFunc { get; set; }

        // ── 預覽狀態 ─────────────────────────────────────────────────────
        private string _upText   = "";
        private Color  _upColor  = Color.Yellow;
        private float  _upFontPt = 14f;

        private string _dnText   = "";
        private Color  _dnColor  = Color.Yellow;
        private float  _dnFontPt = 14f;

        private readonly DisplayMessageService  _service = new DisplayMessageService();
        private readonly PacketBuilderService   _builder = new PacketBuilderService();

        // 字型名稱常數，確保 Designer / 邏輯端完全一致
        private const string FontName24x24 = "24x24";
        private const string FontName16x16 = "16x16";
        private const string FontName5x7   = "英文 5x7";

        private static readonly string SettingsPath =
            Path.Combine(Application.StartupPath, "display_settings.xml");

        // 板型 RadioButton 陣列快取（初始化後不再變動）
        private RadioButton[] _upBoardRadios;
        private RadioButton[] _dnBoardRadios;

        // 板型子選項容器 Panel（程式碼建立，避免 VS Designer DPI 縮放）
        private Panel _pnlUpOpts;
        private Panel _pnlDnOpts;

        // ── 時間顯示開關（右側，boards 2/3/7）───────────────────────
        private Label       _lblUpTimeToggle, _lblDnTimeToggle;
        private RadioButton _rdoUpTimeOff, _rdoUpTimeOn;
        private RadioButton _rdoDnTimeOff, _rdoDnTimeOn;
        private bool        _syncingBoards;   // 防止 Up↔Dn radio 互鎖

        // ── 板型7 上行：路線碼設定 ─────────────────────────────────
        private Label         _lblUpRouteHdr, _lblUpRouteIdxLbl, _lblUpRouteToggle;
        private NumericUpDown _nudUpRouteIdx;
        private Panel         _pnlUpRouteThumb, _pnlUpRouteClr;
        private ComboBox      _cmbUpRouteClr;
        private RadioButton   _rdoUpRouteOff, _rdoUpRouteOn;

        // ── 板型7 下行：左側時間 ───────────────────────────────────
        private Label         _lblDnTimeLeftHdr, _lblDnTimeLeftToggle;
        private ComboBox      _cmbDnTimeLeftType, _cmbDnTimeLeftClr;
        private Panel         _pnlDnTimeLeftClr;
        private RadioButton   _rdoDnTimeLeftOff, _rdoDnTimeLeftOn;

        // ── 板型8 下行：站與站之間連續圖片 ─────────────────────────────────
        // 3 個站名列；圖檔子列只在前 2 列（DnTrain83ImgRows = DnTrain83Rows - 1）
        private const int     DnTrain83Rows    = 3;
        private const int     DnTrain83ImgRows = DnTrain83Rows - 1;   // = 2
        private Label         _lblDnTrain83Hdr;
        private ComboBox[]    _cmbDnTrain83Station;
        private TextBox[]     _txtDnTrain83Dest;
        private ComboBox[]    _cmbDnTrain83Scroll;
        private Label[]       _lblDnTrain83Img;
        private NumericUpDown[] _nudDnTrain83ImgStart;
        private NumericUpDown[] _nudDnTrain83ImgEnd;
        private Panel[]       _pnlDnTrain83RowClr;
        private Label         _lblDnTrain83ClrTitle;
        private Panel         _pnlDnTrain83Clr;
        private ComboBox      _cmbDnTrain83Clr;

        public DisplayMessageControl()
        {
            InitializeComponent();
            // 強制關閉 DPI 縮放：Designer.cs 會被 VS 重新產生並可能設回 Font，
            // 在此覆蓋確保 PerformAutoScale 不會縮放子控件座標。
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;

            // ── 建立選項容器 Panel，並將 extra 控件移入 ──────────────────
            // 子控件移入後，Location 自動變為 Panel-relative，與外層縮放無關。
            _pnlUpOpts = new Panel { BorderStyle = BorderStyle.FixedSingle };
            _pnlDnOpts = new Panel { BorderStyle = BorderStyle.FixedSingle };

            _pnlUpOpts.Controls.AddRange(new Control[] {
                lblUpTimeHdr, cmbUpTimeType, pnlUpTimeClr, cmbUpTimeClr,
                lblUpCountStart, nudUpCountStart, lblUpCountStartTime,
                lblUpCountStop,  nudUpCountStop,  lblUpCountStopTime,
                lblUpPlatHdr, lblUpPlatIdx, nudUpPlatIdx,
                pnlUpPlatThumb, pnlUpPlatClr, cmbUpPlatClr,
                pnlUpAlarm
            });
            _pnlDnOpts.Controls.AddRange(new Control[] {
                lblDnTimeHdr, cmbDnTimeType, pnlDnTimeClr, cmbDnTimeClr,
                lblDnCountStart, nudDnCountStart, lblDnCountStartTime,
                lblDnCountStop,  nudDnCountStop,  lblDnCountStopTime,
                lblDnPlatHdr, lblDnPlatIdx, nudDnPlatIdx,
                pnlDnPlatThumb, pnlDnPlatClr, cmbDnPlatClr,
                pnlDnAlarm
            });
            this.Controls.Add(_pnlUpOpts);
            this.Controls.Add(_pnlDnOpts);

            // ── 時間顯示開關（上下行各自獨立）────────────────────────
            _lblUpTimeToggle = new Label { Text = "時間顯示", AutoSize = true, Visible = false };
            _rdoUpTimeOff    = new RadioButton { Text = "關", AutoSize = true, Visible = false };
            _rdoUpTimeOn     = new RadioButton { Text = "開", AutoSize = true, Checked = true, Visible = false };
            _lblDnTimeToggle = new Label { Text = "時間顯示", AutoSize = true, Visible = false };
            _rdoDnTimeOff    = new RadioButton { Text = "關", AutoSize = true, Visible = false };
            _rdoDnTimeOn     = new RadioButton { Text = "開", AutoSize = true, Checked = true, Visible = false };
            _pnlUpOpts.Controls.AddRange(new Control[] { _lblUpTimeToggle, _rdoUpTimeOff, _rdoUpTimeOn });
            _pnlDnOpts.Controls.AddRange(new Control[] { _lblDnTimeToggle, _rdoDnTimeOff, _rdoDnTimeOn });

            // ── 板型7 上行：路線碼設定 ─────────────────────────────
            _lblUpRouteHdr    = new Label { Text = "上行路線碼設定", AutoSize = false,
                Size = new Size(220, 18), TextAlign = ContentAlignment.MiddleLeft,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.FromArgb(220, 235, 255), Visible = false };
            _lblUpRouteIdxLbl = new Label { Text = "圖檔索引值", AutoSize = true, Visible = false };
            _nudUpRouteIdx    = new NumericUpDown { Minimum = 1, Maximum = 9, Value = 1,
                Size = new Size(45, 22), Visible = false };
            _pnlUpRouteThumb  = new Panel { Size = new Size(24, 24),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.Black, Visible = false };
            _pnlUpRouteClr    = new Panel { Size = new Size(18, 18),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.Yellow, Visible = false };
            _cmbUpRouteClr    = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList,
                DrawMode = DrawMode.OwnerDrawFixed, ItemHeight = 18,
                Size = new Size(110, 21), Visible = false };
            _cmbUpRouteClr.Items.AddRange(ColorComboItems);
            _cmbUpRouteClr.SelectedIndex = 3;
            _cmbUpRouteClr.DrawItem += cmbColor_DrawItem;
            _cmbUpRouteClr.SelectedIndexChanged += (s, e) =>
                _pnlUpRouteClr.BackColor = ColorPalette.GetColor(_cmbUpRouteClr.SelectedItem?.ToString());
            _lblUpRouteToggle = new Label { Text = "路線碼", AutoSize = true, Visible = false };
            _rdoUpRouteOff    = new RadioButton { Text = "關", AutoSize = true, Visible = false };
            _rdoUpRouteOn     = new RadioButton { Text = "開", AutoSize = true, Checked = true, Visible = false };
            _pnlUpOpts.Controls.AddRange(new Control[] {
                _lblUpRouteHdr, _lblUpRouteIdxLbl, _nudUpRouteIdx,
                _pnlUpRouteThumb, _pnlUpRouteClr, _cmbUpRouteClr,
                _lblUpRouteToggle, _rdoUpRouteOff, _rdoUpRouteOn });

            // ── 板型7 下行：左側時間 ───────────────────────────────
            _lblDnTimeLeftHdr   = new Label { Text = "下行時間(左側顯示)", AutoSize = false,
                Size = new Size(220, 18), TextAlign = ContentAlignment.MiddleLeft,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.FromArgb(220, 235, 255), Visible = false };
            _cmbDnTimeLeftType  = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(110, 21), Visible = false };
            _cmbDnTimeLeftType.Items.AddRange(new object[] { "標準時間", "開始倒數" });
            _cmbDnTimeLeftType.SelectedIndex = 0;
            _pnlDnTimeLeftClr   = new Panel { Size = new Size(18, 18),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.Yellow, Visible = false };
            _cmbDnTimeLeftClr   = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList,
                DrawMode = DrawMode.OwnerDrawFixed, ItemHeight = 18,
                Size = new Size(110, 21), Visible = false };
            _cmbDnTimeLeftClr.Items.AddRange(ColorComboItems);
            _cmbDnTimeLeftClr.SelectedIndex = 3;
            _cmbDnTimeLeftClr.DrawItem += cmbColor_DrawItem;
            _cmbDnTimeLeftClr.SelectedIndexChanged += (s, e) =>
                _pnlDnTimeLeftClr.BackColor = ColorPalette.GetColor(_cmbDnTimeLeftClr.SelectedItem?.ToString());
            _lblDnTimeLeftToggle = new Label { Text = "時間顯示", AutoSize = true, Visible = false };
            _rdoDnTimeLeftOff   = new RadioButton { Text = "關", AutoSize = true, Visible = false };
            _rdoDnTimeLeftOn    = new RadioButton { Text = "開", AutoSize = true, Checked = true, Visible = false };
            _pnlDnOpts.Controls.AddRange(new Control[] {
                _lblDnTimeLeftHdr, _cmbDnTimeLeftType,
                _pnlDnTimeLeftClr, _cmbDnTimeLeftClr,
                _lblDnTimeLeftToggle, _rdoDnTimeLeftOff, _rdoDnTimeLeftOn });

            // ── 板型8 下行：站與站之間連續圖片 ────────────────────────────
            _lblDnTrain83Hdr = new Label {
                AutoSize = false, Size = new Size(380, 18),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.FromArgb(220, 235, 255),
                Text = "下行站與站之間(連續圖片動態顯示模式)", Visible = false };

            string[] stationItems83 = {
                "LG01", "LG02", "LG03", "LG04", "LG05",
                "LG06", "LG07", "LG08", "LG09", "LG10" };

            _cmbDnTrain83Station  = new ComboBox[DnTrain83Rows];
            _txtDnTrain83Dest     = new TextBox[DnTrain83Rows];
            _cmbDnTrain83Scroll   = new ComboBox[DnTrain83Rows];
            _lblDnTrain83Img      = new Label[DnTrain83ImgRows];
            _nudDnTrain83ImgStart = new NumericUpDown[DnTrain83ImgRows];
            _nudDnTrain83ImgEnd   = new NumericUpDown[DnTrain83ImgRows];
            _pnlDnTrain83RowClr   = new Panel[DnTrain83ImgRows];

            // 站名列（3 列）
            for (int r = 0; r < DnTrain83Rows; r++)
            {
                _cmbDnTrain83Station[r] = new ComboBox {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Size = new System.Drawing.Size(80, 21), Visible = false };
                _cmbDnTrain83Station[r].Items.AddRange(stationItems83);
                _cmbDnTrain83Station[r].SelectedIndex = Math.Min(r, stationItems83.Length - 1);

                _txtDnTrain83Dest[r] = new TextBox {
                    Size = new System.Drawing.Size(60, 21), Visible = false };

                _cmbDnTrain83Scroll[r] = new ComboBox {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Size = new System.Drawing.Size(70, 21), Visible = false };
                _cmbDnTrain83Scroll[r].Items.AddRange(new object[] { "靜止", "閃爍" });
                _cmbDnTrain83Scroll[r].SelectedIndex = 0;
            }

            // 圖檔子列（2 列，最後一站無圖檔）
            for (int r = 0; r < DnTrain83ImgRows; r++)
            {
                _lblDnTrain83Img[r] = new Label { Text = "圖檔", AutoSize = true, Visible = false };

                _nudDnTrain83ImgStart[r] = new NumericUpDown {
                    Minimum = 1, Maximum = 255, Value = 1,
                    Size = new System.Drawing.Size(45, 22), Visible = false };

                _nudDnTrain83ImgEnd[r] = new NumericUpDown {
                    Minimum = 1, Maximum = 255, Value = 1,
                    Size = new System.Drawing.Size(45, 22), Visible = false };

                _pnlDnTrain83RowClr[r] = new Panel {
                    Size = new System.Drawing.Size(18, 18),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = System.Drawing.Color.Yellow, Visible = false };
            }

            // 全域圖檔顏色
            _lblDnTrain83ClrTitle = new Label { Text = "圖檔顏色", AutoSize = true, Visible = false };
            _pnlDnTrain83Clr = new Panel {
                Size = new System.Drawing.Size(18, 18),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.Yellow, Visible = false };
            _cmbDnTrain83Clr = new ComboBox {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DrawMode = DrawMode.OwnerDrawFixed, ItemHeight = 18,
                Size = new System.Drawing.Size(110, 21), Visible = false };
            _cmbDnTrain83Clr.Items.AddRange(ColorComboItems);
            _cmbDnTrain83Clr.SelectedIndex = 3; // clYellow
            _cmbDnTrain83Clr.DrawItem += cmbColor_DrawItem;
            _cmbDnTrain83Clr.SelectedIndexChanged += (s, e) => {
                var c = ColorPalette.GetColor(_cmbDnTrain83Clr.SelectedItem?.ToString());
                _pnlDnTrain83Clr.BackColor = c;
                foreach (var p in _pnlDnTrain83RowClr) p.BackColor = c;
            };

            // 加入 _pnlDnOpts
            var train83Ctrls = new System.Collections.Generic.List<System.Windows.Forms.Control> {
                _lblDnTrain83Hdr, _lblDnTrain83ClrTitle, _pnlDnTrain83Clr, _cmbDnTrain83Clr };
            for (int r = 0; r < DnTrain83Rows; r++)
            {
                train83Ctrls.Add(_cmbDnTrain83Station[r]);
                train83Ctrls.Add(_txtDnTrain83Dest[r]);
                train83Ctrls.Add(_cmbDnTrain83Scroll[r]);
            }
            for (int r = 0; r < DnTrain83ImgRows; r++)
            {
                train83Ctrls.Add(_lblDnTrain83Img[r]);
                train83Ctrls.Add(_nudDnTrain83ImgStart[r]);
                train83Ctrls.Add(_nudDnTrain83ImgEnd[r]);
                train83Ctrls.Add(_pnlDnTrain83RowClr[r]);
            }
            _pnlDnOpts.Controls.AddRange(train83Ctrls.ToArray());

            ApplyLayout();   // 覆蓋 Designer 產生的座標，防止 VS 重新產生時跑版
            _upBoardRadios = new[] { rdoUpBoard1, rdoUpBoard2, rdoUpBoard3, rdoUpBoard4,
                                     rdoUpBoard5, rdoUpBoard6, rdoUpBoard7 };
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
            int idx = Array.IndexOf(_upBoardRadios, rdo);
            SetUpTimeVisible(idx == 1 || idx == 2 || idx == 6);
            SetUpPlatVisible(idx == 2 || idx == 4);
            SetUpAlarmVisible(idx == 5);
            SetUpRouteVisible(idx == 6);
            // 同步 Dn radio
            if (!_syncingBoards && idx >= 0 && idx < _dnBoardRadios.Length)
            {
                _syncingBoards = true;
                _dnBoardRadios[idx].Checked = true;
                _syncingBoards = false;
            }
        }

        private void rdoDnBoard_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = sender as RadioButton;
            if (rdo == null || !rdo.Checked) return;
            int idx = Array.IndexOf(_dnBoardRadios, rdo);
            SetDnTimeVisible(idx == 1 || idx == 2 || idx == 6);
            SetDnPlatVisible(idx == 2 || idx == 4);
            SetDnAlarmVisible(idx == 5);
            SetDnTimeLeftVisible(idx == 6);
            SetDnTrain83Visible(idx == 7);
            // 同步 Up radio
            if (!_syncingBoards && idx >= 0 && idx < _upBoardRadios.Length)
            {
                _syncingBoards = true;
                _upBoardRadios[idx].Checked = true;
                _syncingBoards = false;
            }
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
                var ids = new List<byte>();
                ids.AddRange(GetUpIDsFunc?.Invoke() ?? new List<byte>());
                ids.AddRange(GetLobbyIDsFunc?.Invoke() ?? new List<byte>());
                if (ids.Count == 0)
                {
                    errors.AppendLine("上行：未在串列埠設定中勾選任何 ID。");
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
                var ids = new List<byte>();
                ids.AddRange(GetDnIDsFunc?.Invoke() ?? new List<byte>());
                ids.AddRange(GetLobbyIDsFunc?.Invoke() ?? new List<byte>());
                if (ids.Count == 0)
                {
                    errors.AppendLine("下行：未在串列埠設定中勾選任何 ID。");
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
                    var result = GetMessageType(false) == 0x83
                        ? BuildDnTrain83Packet(ids)
                        : _service.Build(BuildDnParams(ids));
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

        /// <summary>板型8 (0x83) 下行：站與站之間連續圖片封包</summary>
        private BuildResult<byte[]> BuildDnTrain83Packet(List<byte> ids)
        {
            try
            {
                // ── 圖片顏色 RGB ─────────────────────────────────────────────
                Color imgColor = ParseColor(_cmbDnTrain83Clr.SelectedItem?.ToString());

                // ── 文字顏色 RGB（使用下行共用顏色）────────────────────────
                byte[] textRgb = DataConversion.FromHex(NameToHex(cmbDnColor.SelectedItem?.ToString()));
                if (textRgb == null || textRgb.Length != 3)
                    return BuildResult<byte[]>.Fail("文字顏色格式錯誤。");

                // ── 組建 5 個 StringMessage（text[0]→photo[0]→text[1]→photo[1]→text[2]）──
                var content = new List<StringMessage>();
                for (int r = 0; r < DnTrain83Rows; r++)
                {
                    // 文字段：靜止=0x2A，閃爍=0x2B
                    byte strMode = (_cmbDnTrain83Scroll[r].SelectedIndex == 1) ? (byte)0x2B : (byte)0x2A;
                    content.Add(new StringMessage
                    {
                        StringMode = strMode,
                        StringBody = new TextStringBody
                        {
                            RedColor   = textRgb[0],
                            GreenColor = textRgb[1],
                            BlueColor  = textRgb[2],
                            StringText = _txtDnTrain83Dest[r].Text
                        }
                    });

                    // 圖片段（最後一站無圖片）
                    if (r < DnTrain83ImgRows)
                    {
                        ushort startIdx = (ushort)_nudDnTrain83ImgStart[r].Value;
                        byte   num      = (byte)Math.Max(1,
                            (int)_nudDnTrain83ImgEnd[r].Value - startIdx + 1);
                        content.Add(new StringMessage
                        {
                            StringMode = 0x2D,   // Pre_RecordedPicturesStatic_Dynamic
                            StringBody = new PreRecordedGraphicBody
                            {
                                GraphicStartIndex = startIdx,
                                GraphicNumber     = num,
                                RedColor          = imgColor.R,
                                GreenColor        = imgColor.G,
                                BlueColor         = imgColor.B
                            }
                        });
                    }
                }

                // ── trainDynamic ─────────────────────────────────────────────
                var td = new trainDynamic
                {
                    MessageType   = 0x83,
                    MessageLevel  = ParseLevel(cmbDnLevel),
                    MessageScroll = new ScrollInfo
                    {
                        ScrollMode  = 0x61,
                        ScrollSpeed = (byte)nudDnSpeed.Value,
                        PauseTime   = (byte)nudDnPause.Value
                    },
                    MessageContent = content
                };

                // ── Sequence ─────────────────────────────────────────────────
                var seq = new Sequence
                {
                    SequenceNo = 0x02,   // 下行
                    Font       = new FontSetting
                    {
                        Size  = NameToFontSize(cmbDnFontSize.SelectedItem?.ToString()),
                        Style = NameToFontStyle(cmbDnFontStyle.SelectedItem?.ToString())
                    },
                    Messages = new List<IMessage> { td }
                };

                // ── Packet ───────────────────────────────────────────────────
                var r5 = _builder.BuildPacket(seq, ids, 0x34);
                if (!r5.IsValid)
                    return BuildResult<byte[]>.Fail($"[Train83 Packet] {r5.ErrorMessage}");

                byte[] bytes = r5.Value.ToBytes();
                return BuildResult<byte[]>.Success(bytes, PacketBuilderService.ToHex(bytes));
            }
            catch (Exception ex)
            {
                return BuildResult<byte[]>.Fail($"BuildDnTrain83Packet 失敗：{ex.Message}");
            }
        }

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

        /// <summary>
        /// 在 InitializeComponent 後立即套用正確座標。
        /// VS Designer 每次開啟都會重新產生 Designer.cs，此方法確保 Runtime 永遠用正確位置。
        /// </summary>
        // PerformAutoScale 在 constructor 完成後才懶惰執行，OnCreateControl 在其後觸發，
        // 再次呼叫 ApplyLayout 可確保縮放後的座標被我們的版本覆蓋。
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ApplyLayout();
        }

        private void ApplyLayout()
        {
            // ── 選項容器 Panel 位置與大小 ────────────────────────────────
            // 高度 200px：時間(0-76) + 開關行(77-97) + 次區段(98-195)
            if (_pnlUpOpts != null)
            {
                _pnlUpOpts.Location = new Point(0,   100);
                _pnlUpOpts.Size     = new Size(580,  200);
                _pnlDnOpts.Location = new Point(585, 100);
                _pnlDnOpts.Size     = new Size(580,  200);
            }

            // ── 上行 Extra（Panel-relative 座標）────────────────────────
            // Row 0 – 時間區段 header（y=6）
            lblUpTimeHdr.Location        = new Point(10,  6);
            // Row 1 – 時間類型 + 倒數開始（y=32）
            cmbUpTimeType.Location       = new Point(10,  32);
            lblUpCountStart.Location     = new Point(140, 35);
            nudUpCountStart.Location     = new Point(193, 31);
            lblUpCountStartTime.Location = new Point(248, 35);
            // Row 2 – 顏色 + 倒數停止（y=62）
            pnlUpTimeClr.Location        = new Point(10,  64);
            cmbUpTimeClr.Location        = new Point(32,  62);
            lblUpCountStop.Location      = new Point(140, 65);
            nudUpCountStop.Location      = new Point(193, 61);
            lblUpCountStopTime.Location  = new Point(248, 65);
            // Row 3 – 時間顯示開關（y=92，獨立一行）
            if (_lblUpTimeToggle != null)
            {
                _lblUpTimeToggle.Location = new Point(10,  94);
                _rdoUpTimeOff.Location    = new Point(72,  92);
                _rdoUpTimeOn.Location     = new Point(120, 92);
            }

            // Row 4 – 月台碼區段 header（y=122）
            lblUpPlatHdr.Location        = new Point(10,  122);
            // Row 5 – 月台碼控件（y=150）
            lblUpPlatIdx.Location        = new Point(10,  153);
            nudUpPlatIdx.Location        = new Point(78,  149);
            pnlUpPlatThumb.Location      = new Point(128, 148);
            pnlUpPlatClr.Location        = new Point(158, 153);
            cmbUpPlatClr.Location        = new Point(180, 149);

            // Row 4 – 路線碼設定 header（板型7 Up，y=122）
            if (_lblUpRouteHdr != null)
            {
                _lblUpRouteHdr.Location    = new Point(10,  122);
                // Row 5 – 路線碼控件（y=150）
                _lblUpRouteIdxLbl.Location = new Point(10,  153);
                _nudUpRouteIdx.Location    = new Point(78,  149);
                _pnlUpRouteThumb.Location  = new Point(128, 148);
                _pnlUpRouteClr.Location    = new Point(158, 153);
                _cmbUpRouteClr.Location    = new Point(180, 149);
                _lblUpRouteToggle.Location = new Point(290, 153);
                _rdoUpRouteOff.Location    = new Point(340, 151);
                _rdoUpRouteOn.Location     = new Point(392, 151);
            }

            // 緊急訊息區段（板型 6）
            pnlUpAlarm.Location          = new Point(10,  6);

            // ── 下行 Extra（Panel-relative，與上行對稱）─────────────────
            // Row 0 – 時間區段 header（y=6）
            lblDnTimeHdr.Location        = new Point(10,  6);
            // Row 1 – 時間類型 + 倒數開始（y=32）
            cmbDnTimeType.Location       = new Point(10,  32);
            lblDnCountStart.Location     = new Point(140, 35);
            nudDnCountStart.Location     = new Point(193, 31);
            lblDnCountStartTime.Location = new Point(248, 35);
            // Row 2 – 顏色 + 倒數停止（y=62）
            pnlDnTimeClr.Location        = new Point(10,  64);
            cmbDnTimeClr.Location        = new Point(32,  62);
            lblDnCountStop.Location      = new Point(140, 65);
            nudDnCountStop.Location      = new Point(193, 61);
            lblDnCountStopTime.Location  = new Point(248, 65);
            // Row 3 – 時間顯示開關（y=92，獨立一行）
            if (_lblDnTimeToggle != null)
            {
                _lblDnTimeToggle.Location = new Point(10,  94);
                _rdoDnTimeOff.Location    = new Point(72,  92);
                _rdoDnTimeOn.Location     = new Point(120, 92);
            }

            // Row 4 – 月台碼區段 header（y=122）
            lblDnPlatHdr.Location        = new Point(10,  122);
            // Row 5 – 月台碼控件（y=150）
            lblDnPlatIdx.Location        = new Point(10,  153);
            nudDnPlatIdx.Location        = new Point(78,  149);
            pnlDnPlatThumb.Location      = new Point(128, 148);
            pnlDnPlatClr.Location        = new Point(158, 153);
            cmbDnPlatClr.Location        = new Point(180, 149);

            // Row 4 – 左側時間 header（板型7 Dn，y=122）
            if (_lblDnTimeLeftHdr != null)
            {
                _lblDnTimeLeftHdr.Location    = new Point(10,  122);
                // Row 5 – 左側時間控件（y=150）
                _cmbDnTimeLeftType.Location   = new Point(10,  149);
                _pnlDnTimeLeftClr.Location    = new Point(130, 153);
                _cmbDnTimeLeftClr.Location    = new Point(152, 149);
                _lblDnTimeLeftToggle.Location = new Point(270, 153);
                _rdoDnTimeLeftOff.Location    = new Point(330, 151);
                _rdoDnTimeLeftOn.Location     = new Point(382, 151);
            }

            // 緊急訊息區段（板型 6）
            pnlDnAlarm.Location          = new Point(10,  6);

            // ── 板型8 下行：Train 83（交錯排列：站名列→圖檔列→站名列→圖檔列→站名列）
            if (_lblDnTrain83Hdr != null)
            {
                _lblDnTrain83Hdr.Location = new Point(10, 4);
                // 每個站名列 26px 高，圖檔子列 26px 高，共 5 sub-rows
                // 順序：station[0] img[0] station[1] img[1] station[2]
                int[] stationY = { 28, 80, 132 };   // y 座標：station 0,1,2
                int[] imgY     = { 54, 106 };        // y 座標：img 0,1
                for (int r = 0; r < DnTrain83Rows; r++)
                {
                    int sy = stationY[r];
                    _cmbDnTrain83Station[r].Location = new Point(10,  sy);
                    _txtDnTrain83Dest[r].Location    = new Point(94,  sy);
                    _cmbDnTrain83Scroll[r].Location  = new Point(158, sy);
                }
                for (int r = 0; r < DnTrain83ImgRows; r++)
                {
                    int iy = imgY[r];
                    _lblDnTrain83Img[r].Location      = new Point(20,  iy + 4);
                    _nudDnTrain83ImgStart[r].Location = new Point(48,  iy - 1);
                    _nudDnTrain83ImgEnd[r].Location   = new Point(98,  iy - 1);
                    _pnlDnTrain83RowClr[r].Location   = new Point(148, iy + 2);
                }
                _lblDnTrain83ClrTitle.Location = new Point(395, 28);
                _pnlDnTrain83Clr.Location      = new Point(395, 50);
                _cmbDnTrain83Clr.Location      = new Point(417, 48);
            }

            // ── 主要內容 Up（+60 位移，為 Extra 區段讓出空間）────────────
            lblUpMsgType.Location        = new Point(10,  284);
            pnlUpMsgType.Location        = new Point(10,  300);
            lblUpMsg.Location            = new Point(10,  329);
            txtUpMsg.Location            = new Point(10,  347);
            lstUpMultiMsgs.Location      = new Point(10,  347);
            btnUpMsgBrowse.Location      = new Point(389, 346);
            btnUpMsgShow.Location        = new Point(420, 346);
            btnUpMsgAdd.Location         = new Point(473, 346);
            btnUpMultiAdd.Location       = new Point(389, 347);
            btnUpMultiRemove.Location    = new Point(389, 376);
            lblUpFont.Location           = new Point(10,  374);
            cmbUpFontSize.Location       = new Point(10,  392);
            cmbUpFontStyle.Location      = new Point(80,  392);
            pnlUpColor.Location          = new Point(150, 394);
            cmbUpColor.Location          = new Point(172, 392);
            cmbUpLevel.Location          = new Point(310, 392);
            lblUpAction.Location         = new Point(10,  419);
            pnlUpAction.Location         = new Point(10,  435);
            lblUpParam.Location          = new Point(10,  547);
            lblUpSpeed.Location          = new Point(10,  567);
            nudUpSpeed.Location          = new Point(45,  565);
            lblUpPause.Location          = new Point(105, 567);
            nudUpPause.Location          = new Point(165, 565);
            lblUpPauseUnit.Location      = new Point(220, 567);
            grpUpBoard.Location          = new Point(10,  603);

            // ── 主要內容 Dn（+60 位移）───────────────────────────────────
            lblDnMsgType.Location        = new Point(595, 284);
            pnlDnMsgType.Location        = new Point(595, 300);
            lblDnMsg.Location            = new Point(595, 329);
            txtDnMsg.Location            = new Point(595, 347);
            lstDnMultiMsgs.Location      = new Point(595, 347);
            btnDnMsgBrowse.Location      = new Point(974, 346);
            btnDnMsgShow.Location        = new Point(1005,346);
            btnDnMsgAdd.Location         = new Point(1058,346);
            btnDnMultiAdd.Location       = new Point(974, 347);
            btnDnMultiRemove.Location    = new Point(974, 376);
            lblDnFont.Location           = new Point(595, 374);
            cmbDnFontSize.Location       = new Point(595, 392);
            cmbDnFontStyle.Location      = new Point(665, 392);
            pnlDnColor.Location          = new Point(735, 394);
            cmbDnColor.Location          = new Point(757, 392);
            cmbDnLevel.Location          = new Point(895, 392);
            lblDnAction.Location         = new Point(595, 419);
            pnlDnAction.Location         = new Point(595, 435);
            lblDnParam.Location          = new Point(595, 547);
            lblDnSpeed.Location          = new Point(595, 567);
            nudDnSpeed.Location          = new Point(630, 565);
            lblDnPause.Location          = new Point(690, 567);
            nudDnPause.Location          = new Point(750, 565);
            lblDnPauseUnit.Location      = new Point(805, 567);
            grpDnBoard.Location          = new Point(595, 603);
        }

        private void SetUpTimeVisible(bool v)
        {
            lblUpTimeHdr.Visible  = v;
            cmbUpTimeType.Visible = v;
            pnlUpTimeClr.Visible  = v;
            cmbUpTimeClr.Visible  = v;
            _lblUpTimeToggle.Visible = v;
            _rdoUpTimeOff.Visible    = v;
            _rdoUpTimeOn.Visible     = v;
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
            // pnlUpAlarm 已移入 _pnlUpOpts，不再蓋住按鈕，直接控制顯示即可
            pnlUpAlarm.Visible = v;
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
            _lblDnTimeToggle.Visible = v;
            _rdoDnTimeOff.Visible    = v;
            _rdoDnTimeOn.Visible     = v;
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

        private void SetUpRouteVisible(bool v)
        {
            _lblUpRouteHdr.Visible    = v;
            _lblUpRouteIdxLbl.Visible = v;
            _nudUpRouteIdx.Visible    = v;
            _pnlUpRouteThumb.Visible  = v;
            _pnlUpRouteClr.Visible    = v;
            _cmbUpRouteClr.Visible    = v;
            _lblUpRouteToggle.Visible = v;
            _rdoUpRouteOff.Visible    = v;
            _rdoUpRouteOn.Visible     = v;
        }

        private void SetDnTimeLeftVisible(bool v)
        {
            _lblDnTimeLeftHdr.Visible    = v;
            _cmbDnTimeLeftType.Visible   = v;
            _pnlDnTimeLeftClr.Visible    = v;
            _cmbDnTimeLeftClr.Visible    = v;
            _lblDnTimeLeftToggle.Visible = v;
            _rdoDnTimeLeftOff.Visible    = v;
            _rdoDnTimeLeftOn.Visible     = v;
        }

        private void SetDnTrain83Visible(bool v)
        {
            _lblDnTrain83Hdr.Visible      = v;
            _lblDnTrain83ClrTitle.Visible = v;
            _pnlDnTrain83Clr.Visible      = v;
            _cmbDnTrain83Clr.Visible      = v;
            for (int r = 0; r < DnTrain83Rows; r++)
            {
                _cmbDnTrain83Station[r].Visible = v;
                _txtDnTrain83Dest[r].Visible    = v;
                _cmbDnTrain83Scroll[r].Visible  = v;
            }
            for (int r = 0; r < DnTrain83ImgRows; r++)
            {
                _lblDnTrain83Img[r].Visible      = v;
                _nudDnTrain83ImgStart[r].Visible = v;
                _nudDnTrain83ImgEnd[r].Visible   = v;
                _pnlDnTrain83RowClr[r].Visible   = v;
            }
        }

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
                          rdoUpBoard5, rdoUpBoard6, rdoUpBoard7 }
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
                // Extra：時間
                UpTimeTypeIndex = cmbUpTimeType.SelectedIndex,
                UpTimeClrIndex  = cmbUpTimeClr.SelectedIndex,
                UpTimeOn        = _rdoUpTimeOn.Checked,
                DnTimeTypeIndex = cmbDnTimeType.SelectedIndex,
                DnTimeClrIndex  = cmbDnTimeClr.SelectedIndex,
                DnTimeOn        = _rdoDnTimeOn.Checked,
                // Extra：月台碼
                UpPlatIdx       = (int)nudUpPlatIdx.Value,
                UpPlatClrIndex  = cmbUpPlatClr.SelectedIndex,
                DnPlatIdx       = (int)nudDnPlatIdx.Value,
                DnPlatClrIndex  = cmbDnPlatClr.SelectedIndex,
                // Extra：板型7 路線碼（上行）
                UpRouteIdx      = (int)_nudUpRouteIdx.Value,
                UpRouteClrIndex = _cmbUpRouteClr.SelectedIndex,
                UpRouteOn       = _rdoUpRouteOn.Checked,
                // Extra：板型7 左側時間（下行）
                DnTimeLeftTypeIndex = _cmbDnTimeLeftType.SelectedIndex,
                DnTimeLeftClrIndex  = _cmbDnTimeLeftClr.SelectedIndex,
                DnTimeLeftOn        = _rdoDnTimeLeftOn.Checked,
                // Extra：板型8 下行 Train83
                DnTrain83StationIdx = System.Array.ConvertAll(_cmbDnTrain83Station,    c => c.SelectedIndex),
                DnTrain83Dest       = System.Array.ConvertAll(_txtDnTrain83Dest,       t => t.Text),
                DnTrain83ScrollIdx  = System.Array.ConvertAll(_cmbDnTrain83Scroll,     c => c.SelectedIndex),
                DnTrain83ImgStart   = System.Array.ConvertAll(_nudDnTrain83ImgStart,   n => (int)n.Value),
                DnTrain83ImgEnd     = System.Array.ConvertAll(_nudDnTrain83ImgEnd,     n => (int)n.Value),
                DnTrain83ClrIndex   = _cmbDnTrain83Clr.SelectedIndex,
            };

            var serializer = new XmlSerializer(typeof(DisplayMessageSettings));
            using (var writer = new StreamWriter(SettingsPath, append: false, encoding: Encoding.UTF8))
                serializer.Serialize(writer, s);
        }

        private void LoadSettings()
        {
            // 先建立預設值；有設定檔才覆寫
            var s = new DisplayMessageSettings();

            if (File.Exists(SettingsPath))
            try
            {
                var serializer = new XmlSerializer(typeof(DisplayMessageSettings));
                using (var reader = new StreamReader(SettingsPath, Encoding.UTF8))
                    s = (DisplayMessageSettings)serializer.Deserialize(reader);
            }
            catch
            {
                // 設定檔損毀時靜默忽略，改用預設值
            }

            try
            {

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
                // Extra：時間
                SetComboIndex(cmbUpTimeType, s.UpTimeTypeIndex);
                SetComboIndex(cmbUpTimeClr,  s.UpTimeClrIndex);
                _rdoUpTimeOff.Checked = !s.UpTimeOn;
                _rdoUpTimeOn.Checked  =  s.UpTimeOn;
                SetComboIndex(cmbDnTimeType, s.DnTimeTypeIndex);
                SetComboIndex(cmbDnTimeClr,  s.DnTimeClrIndex);
                _rdoDnTimeOff.Checked = !s.DnTimeOn;
                _rdoDnTimeOn.Checked  =  s.DnTimeOn;
                // Extra：月台碼
                nudUpPlatIdx.Value = Clamp(s.UpPlatIdx, nudUpPlatIdx);
                SetComboIndex(cmbUpPlatClr,  s.UpPlatClrIndex);
                nudDnPlatIdx.Value = Clamp(s.DnPlatIdx, nudDnPlatIdx);
                SetComboIndex(cmbDnPlatClr,  s.DnPlatClrIndex);
                // Extra：板型7 路線碼（上行）
                _nudUpRouteIdx.Value = Clamp(s.UpRouteIdx, _nudUpRouteIdx);
                SetComboIndex(_cmbUpRouteClr, s.UpRouteClrIndex);
                _rdoUpRouteOff.Checked = !s.UpRouteOn;
                _rdoUpRouteOn.Checked  =  s.UpRouteOn;
                // Extra：板型7 左側時間（下行）
                SetComboIndex(_cmbDnTimeLeftType, s.DnTimeLeftTypeIndex);
                SetComboIndex(_cmbDnTimeLeftClr,  s.DnTimeLeftClrIndex);
                _rdoDnTimeLeftOff.Checked = !s.DnTimeLeftOn;
                _rdoDnTimeLeftOn.Checked  =  s.DnTimeLeftOn;
                // Extra：板型8 下行 Train83（站名列）
                for (int r = 0; r < DnTrain83Rows; r++)
                {
                    if (s.DnTrain83StationIdx != null && r < s.DnTrain83StationIdx.Length)
                        SetComboIndex(_cmbDnTrain83Station[r], s.DnTrain83StationIdx[r]);
                    if (s.DnTrain83Dest != null && r < s.DnTrain83Dest.Length)
                        _txtDnTrain83Dest[r].Text = s.DnTrain83Dest[r] ?? "";
                    if (s.DnTrain83ScrollIdx != null && r < s.DnTrain83ScrollIdx.Length)
                        SetComboIndex(_cmbDnTrain83Scroll[r], s.DnTrain83ScrollIdx[r]);
                }
                // Extra：板型8 下行 Train83（圖檔子列，只有前 DnTrain83ImgRows 列）
                for (int r = 0; r < DnTrain83ImgRows; r++)
                {
                    if (s.DnTrain83ImgStart != null && r < s.DnTrain83ImgStart.Length)
                        _nudDnTrain83ImgStart[r].Value = Clamp(s.DnTrain83ImgStart[r], _nudDnTrain83ImgStart[r]);
                    if (s.DnTrain83ImgEnd != null && r < s.DnTrain83ImgEnd.Length)
                        _nudDnTrain83ImgEnd[r].Value   = Clamp(s.DnTrain83ImgEnd[r],   _nudDnTrain83ImgEnd[r]);
                }
                SetComboIndex(_cmbDnTrain83Clr, s.DnTrain83ClrIndex);
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
