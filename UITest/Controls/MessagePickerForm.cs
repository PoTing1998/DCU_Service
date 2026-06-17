using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    /// <summary>
    /// 上下行訊息選擇對話框。
    ///
    /// 單選：
    ///   using (var f = new MessagePickerForm(PickerMode.Up))
    ///       if (f.ShowDialog() == OK) txtUpMsg.Text = f.SelectedMessage;
    ///
    /// 多選：
    ///   using (var f = new MessagePickerForm(PickerMode.Up, multiSelect: true))
    ///       if (f.ShowDialog() == OK) /* use f.SelectedMessages */
    /// </summary>
    public enum PickerMode { Up, Down, Both }

    public partial class MessagePickerForm : Form
    {
        // ── 公開屬性 ─────────────────────────────────────────────────────
        /// <summary>單選結果（第一筆選取）</summary>
        public string SelectedMessage { get; private set; } = "";

        /// <summary>多選結果（按索引順序排列）</summary>
        public List<string> SelectedMessages { get; private set; } = new List<string>();

        // ── 私有欄位 ─────────────────────────────────────────────────────
        private readonly PickerMode _mode;
        private readonly bool       _multiSelect;
        private          MessageStore _store;

        // ── 建構子 ──────────────────────────────────────────────────────
        public MessagePickerForm(PickerMode mode = PickerMode.Both, bool multiSelect = false)
        {
            InitializeComponent();
            _mode        = mode;
            _multiSelect = multiSelect;
        }

        // ── Form Load ───────────────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 依模式隱藏不需要的半邊
            switch (_mode)
            {
                case PickerMode.Up:
                    this.Text             = _multiSelect ? "選擇多則上行訊息" : "選擇上行訊息";
                    splitMain.Panel2Collapsed = true;
                    dgvDn.Enabled         = false;
                    break;
                case PickerMode.Down:
                    this.Text             = _multiSelect ? "選擇多則下行訊息" : "選擇下行訊息";
                    splitMain.Panel1Collapsed = true;
                    dgvUp.Enabled         = false;
                    break;
                default:
                    splitMain.SplitterDistance = splitMain.Width / 2;
                    break;
            }

            // 多選模式
            if (_multiSelect)
            {
                dgvUp.MultiSelect  = true;
                dgvDn.MultiSelect  = true;
                lblHint.Text       = "Ctrl/Shift 點擊可多選，再按「確定」";
                // 多選模式下雙擊不直接關閉
                dgvUp.CellDoubleClick -= new DataGridViewCellEventHandler(dgvUp_CellDoubleClick);
                dgvDn.CellDoubleClick -= new DataGridViewCellEventHandler(dgvDn_CellDoubleClick);
            }
            else
            {
                lblHint.Text = _mode == PickerMode.Down
                    ? "雙擊下行訊息列即可選取"
                    : "雙擊上行訊息列即可選取";
            }

            // 底部按鈕靠右
            LayoutBottomButtons();
            pnlButtons.Resize += (s, _) => LayoutBottomButtons();

            // 載入資料
            _store = MessageStoreService.Load();
            MessageStoreService.PadToMaxRows(_store.UpMessages);
            MessageStoreService.PadToMaxRows(_store.DownMessages);
            LoadGrid(dgvUp, _store.UpMessages);
            LoadGrid(dgvDn, _store.DownMessages);
        }

        // ── 資料載入 ────────────────────────────────────────────────────
        private void LoadGrid(DataGridView dgv, List<MessageEntry> list)
        {
            dgv.Rows.Clear();
            foreach (var entry in list)
            {
                int row = dgv.Rows.Add(entry.Index, entry.Length, entry.Content);
                if (string.IsNullOrEmpty(entry.Content))
                    dgv.Rows[row].DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
            }
        }

        // ── 儲存 / 更新 ─────────────────────────────────────────────────
        private void btnUpSave_Click(object sender, EventArgs e)
        {
            SyncGridToStore(dgvUp, _store.UpMessages);
            MessageStoreService.Save(_store);
            MessageBox.Show("上行訊息已儲存。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDnSave_Click(object sender, EventArgs e)
        {
            SyncGridToStore(dgvDn, _store.DownMessages);
            MessageStoreService.Save(_store);
            MessageBox.Show("下行訊息已儲存。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpRefresh_Click(object sender, EventArgs e)
        {
            SyncGridToStore(dgvUp, _store.UpMessages);
            LoadGrid(dgvUp, _store.UpMessages);
        }

        private void btnDnRefresh_Click(object sender, EventArgs e)
        {
            SyncGridToStore(dgvDn, _store.DownMessages);
            LoadGrid(dgvDn, _store.DownMessages);
        }

        // ── 編輯 → 即時更新長度 ─────────────────────────────────────────
        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null || e.ColumnIndex != 2) return;

            string content = dgv.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
            int len = string.IsNullOrEmpty(content) ? 0 : Encoding.Default.GetByteCount(content);
            dgv.Rows[e.RowIndex].Cells[1].Value = len;
            dgv.Rows[e.RowIndex].DefaultCellStyle.ForeColor =
                string.IsNullOrEmpty(content) ? System.Drawing.Color.LightGray : dgv.DefaultCellStyle.ForeColor;
        }

        // ── 雙擊選取（單選模式）────────────────────────────────────────
        private void dgvUp_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string content = dgvUp.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
            if (!string.IsNullOrEmpty(content))
            {
                SelectedMessage  = content;
                SelectedMessages = new List<string> { content };
                DialogResult     = DialogResult.OK;
                Close();
            }
        }

        private void dgvDn_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string content = dgvDn.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
            if (!string.IsNullOrEmpty(content))
            {
                SelectedMessage  = content;
                SelectedMessages = new List<string> { content };
                DialogResult     = DialogResult.OK;
                Close();
            }
        }

        // ── 確定按鈕 ────────────────────────────────────────────────────
        private void btnOk_Click(object sender, EventArgs e)
        {
            SyncGridToStore(dgvUp, _store.UpMessages);
            SyncGridToStore(dgvDn, _store.DownMessages);
            MessageStoreService.Save(_store);

            DataGridView activeGrid = _mode == PickerMode.Down ? dgvDn : dgvUp;

            if (_multiSelect)
            {
                // 多選：依索引欄數值排序後收集
                var rows = new List<DataGridViewRow>();
                foreach (DataGridViewRow r in activeGrid.SelectedRows)
                    rows.Add(r);
                rows.Sort((a, b) =>
                    Comparer<int>.Default.Compare(
                        Convert.ToInt32(a.Cells[0].Value),
                        Convert.ToInt32(b.Cells[0].Value)));

                SelectedMessages.Clear();
                foreach (var r in rows)
                {
                    string c = r.Cells[2].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(c))
                        SelectedMessages.Add(c);
                }
                if (SelectedMessages.Count > 0)
                    SelectedMessage = SelectedMessages[0];
            }
            else
            {
                // 單選
                if (activeGrid.SelectedRows.Count > 0)
                {
                    string content = activeGrid.SelectedRows[0].Cells[2].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(content))
                    {
                        SelectedMessage  = content;
                        SelectedMessages = new List<string> { content };
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }

        // ── 版面 ────────────────────────────────────────────────────────
        private void LayoutBottomButtons()
        {
            const int margin = 8;
            btnCancel.Left = pnlButtons.Width - btnCancel.Width - margin;
            btnOk.Left     = btnCancel.Left - btnOk.Width - 6;
        }

        // ── 工具 ────────────────────────────────────────────────────────
        private void SyncGridToStore(DataGridView dgv, List<MessageEntry> list)
        {
            for (int i = 0; i < dgv.Rows.Count && i < list.Count; i++)
            {
                string content = dgv.Rows[i].Cells[2].Value?.ToString() ?? "";
                list[i].Content = content;
                list[i].Length  = string.IsNullOrEmpty(content) ? 0 : Encoding.Default.GetByteCount(content);
            }
        }
    }
}
