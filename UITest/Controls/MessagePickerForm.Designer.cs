namespace UITest.Controls
{
    partial class MessagePickerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── 底部確定/取消 ──
            this.pnlButtons   = new System.Windows.Forms.Panel();
            this.lblHint      = new System.Windows.Forms.Label();
            this.btnOk        = new System.Windows.Forms.Button();
            this.btnCancel    = new System.Windows.Forms.Button();
            // ── Tab ──
            this.tabControl   = new System.Windows.Forms.TabControl();
            this.tabMessages  = new System.Windows.Forms.TabPage();
            // ── SplitContainer（左右各放一個 TableLayoutPanel）──
            this.splitMain    = new System.Windows.Forms.SplitContainer();
            // ── 上行 TableLayout ──
            this.tblUp        = new System.Windows.Forms.TableLayoutPanel();
            this.pnlUpHdr     = new System.Windows.Forms.Panel();
            this.lblUpTitle   = new System.Windows.Forms.Label();
            this.btnUpSave    = new System.Windows.Forms.Button();
            this.btnUpRefresh = new System.Windows.Forms.Button();
            this.dgvUp        = new System.Windows.Forms.DataGridView();
            this.colUpIndex   = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpLength  = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            // ── 下行 TableLayout ──
            this.tblDn        = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDnHdr     = new System.Windows.Forms.Panel();
            this.lblDnTitle   = new System.Windows.Forms.Label();
            this.btnDnSave    = new System.Windows.Forms.Button();
            this.btnDnRefresh = new System.Windows.Forms.Button();
            this.dgvDn        = new System.Windows.Forms.DataGridView();
            this.colDnIndex   = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDnLength  = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDnContent = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlButtons.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.tblUp.SuspendLayout();
            this.pnlUpHdr.SuspendLayout();
            this.tblDn.SuspendLayout();
            this.pnlDnHdr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDn)).BeginInit();
            this.SuspendLayout();

            // ════════ 底部按鈕 Panel ════════
            this.pnlButtons.Dock   = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 40;

            this.lblHint.AutoSize  = true;
            this.lblHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblHint.Location  = new System.Drawing.Point(8, 12);
            this.lblHint.Text      = "雙擊訊息列即可選取";

            this.btnOk.Anchor      = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnOk.Size        = new System.Drawing.Size(75, 26);
            this.btnOk.Location    = new System.Drawing.Point(730, 7);
            this.btnOk.Text        = "確定";
            this.btnOk.Click      += new System.EventHandler(this.btnOk_Click);

            this.btnCancel.Anchor   = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Size     = new System.Drawing.Size(75, 26);
            this.btnCancel.Location = new System.Drawing.Point(812, 7);
            this.btnCancel.Text     = "取消";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.pnlButtons.Controls.Add(this.lblHint);
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Controls.Add(this.btnCancel);

            // ════════ TabControl ════════
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Controls.Add(this.tabMessages);

            // ════════ TabPage ════════
            this.tabMessages.Text    = "上下行訊息";
            this.tabMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tabMessages.Controls.Add(this.splitMain);

            // ════════ SplitContainer（水平切分，左上行/右下行）════════
            this.splitMain.Dock             = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Orientation      = System.Windows.Forms.Orientation.Vertical;
            this.splitMain.SplitterDistance = 440;
            this.splitMain.Panel1.Controls.Add(this.tblUp);
            this.splitMain.Panel2.Controls.Add(this.tblDn);

            // ════════ 上行 TableLayoutPanel ════════
            // Row 0 (header) = Auto, Row 1 (grid) = 100%
            this.tblUp.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.tblUp.ColumnCount = 1;
            this.tblUp.RowCount    = 2;
            this.tblUp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            this.tblUp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56f));
            this.tblUp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            this.tblUp.Controls.Add(this.pnlUpHdr, 0, 0);
            this.tblUp.Controls.Add(this.dgvUp,    0, 1);

            // 上行 header panel
            this.pnlUpHdr.Dock = System.Windows.Forms.DockStyle.Fill;

            this.lblUpTitle.AutoSize = true;
            this.lblUpTitle.Font     = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold);
            this.lblUpTitle.Location = new System.Drawing.Point(4, 4);
            this.lblUpTitle.Text     = "上行訊息";

            this.btnUpSave.Location = new System.Drawing.Point(4, 26);
            this.btnUpSave.Size     = new System.Drawing.Size(80, 24);
            this.btnUpSave.Text     = "儲存表格";
            this.btnUpSave.Click   += new System.EventHandler(this.btnUpSave_Click);

            this.btnUpRefresh.Location = new System.Drawing.Point(90, 26);
            this.btnUpRefresh.Size     = new System.Drawing.Size(100, 24);
            this.btnUpRefresh.Text     = "上行訊息更新";
            this.btnUpRefresh.Click   += new System.EventHandler(this.btnUpRefresh_Click);

            this.pnlUpHdr.Controls.Add(this.lblUpTitle);
            this.pnlUpHdr.Controls.Add(this.btnUpSave);
            this.pnlUpHdr.Controls.Add(this.btnUpRefresh);

            // 上行 DataGridView
            this.dgvUp.Dock              = System.Windows.Forms.DockStyle.Fill;
            this.dgvUp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
                { this.colUpIndex, this.colUpLength, this.colUpContent });
            this.dgvUp.RowHeadersWidth       = 25;
            this.dgvUp.SelectionMode         = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUp.MultiSelect           = false;
            this.dgvUp.AllowUserToAddRows    = false;
            this.dgvUp.CellDoubleClick      += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUp_CellDoubleClick);
            this.dgvUp.CellEndEdit          += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellEndEdit);

            this.colUpIndex.HeaderText  = "索引值";
            this.colUpIndex.Width       = 55;
            this.colUpIndex.ReadOnly    = true;
            this.colUpIndex.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            this.colUpLength.HeaderText = "訊息長度";
            this.colUpLength.Width      = 60;
            this.colUpLength.ReadOnly   = true;
            this.colUpLength.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            this.colUpContent.HeaderText   = "上行訊息編輯內容（雙擊選取）";
            this.colUpContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            // ════════ 下行 TableLayoutPanel ════════
            this.tblDn.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.tblDn.ColumnCount = 1;
            this.tblDn.RowCount    = 2;
            this.tblDn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            this.tblDn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56f));
            this.tblDn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            this.tblDn.Controls.Add(this.pnlDnHdr, 0, 0);
            this.tblDn.Controls.Add(this.dgvDn,    0, 1);

            // 下行 header panel
            this.pnlDnHdr.Dock = System.Windows.Forms.DockStyle.Fill;

            this.lblDnTitle.AutoSize = true;
            this.lblDnTitle.Font     = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold);
            this.lblDnTitle.Location = new System.Drawing.Point(4, 4);
            this.lblDnTitle.Text     = "下行訊息";

            this.btnDnSave.Location = new System.Drawing.Point(4, 26);
            this.btnDnSave.Size     = new System.Drawing.Size(80, 24);
            this.btnDnSave.Text     = "儲存表格";
            this.btnDnSave.Click   += new System.EventHandler(this.btnDnSave_Click);

            this.btnDnRefresh.Location = new System.Drawing.Point(90, 26);
            this.btnDnRefresh.Size     = new System.Drawing.Size(100, 24);
            this.btnDnRefresh.Text     = "下行訊息更新";
            this.btnDnRefresh.Click   += new System.EventHandler(this.btnDnRefresh_Click);

            this.pnlDnHdr.Controls.Add(this.lblDnTitle);
            this.pnlDnHdr.Controls.Add(this.btnDnSave);
            this.pnlDnHdr.Controls.Add(this.btnDnRefresh);

            // 下行 DataGridView
            this.dgvDn.Dock              = System.Windows.Forms.DockStyle.Fill;
            this.dgvDn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
                { this.colDnIndex, this.colDnLength, this.colDnContent });
            this.dgvDn.RowHeadersWidth       = 25;
            this.dgvDn.SelectionMode         = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDn.MultiSelect           = false;
            this.dgvDn.AllowUserToAddRows    = false;
            this.dgvDn.CellDoubleClick      += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDn_CellDoubleClick);
            this.dgvDn.CellEndEdit          += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellEndEdit);

            this.colDnIndex.HeaderText  = "索引值";
            this.colDnIndex.Width       = 55;
            this.colDnIndex.ReadOnly    = true;
            this.colDnIndex.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            this.colDnLength.HeaderText = "訊息長度";
            this.colDnLength.Width      = 60;
            this.colDnLength.ReadOnly   = true;
            this.colDnLength.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            this.colDnContent.HeaderText   = "下行訊息編輯內容（雙擊選取）";
            this.colDnContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            // ════════ Form ════════
            this.AcceptButton        = this.btnOk;
            this.CancelButton        = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(900, 620);
            this.MinimumSize         = new System.Drawing.Size(700, 500);
            this.StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text                = "板型編輯  上下行訊息";
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlButtons);

            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabMessages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.ResumeLayout(false);
            this.tblUp.ResumeLayout(false);
            this.pnlUpHdr.ResumeLayout(false);
            this.pnlUpHdr.PerformLayout();
            this.tblDn.ResumeLayout(false);
            this.pnlDnHdr.ResumeLayout(false);
            this.pnlDnHdr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDn)).EndInit();
            this.ResumeLayout(false);
        }

        // ── 欄位宣告 ──────────────────────────────────────────────────────
        private System.Windows.Forms.Panel                           pnlButtons;
        private System.Windows.Forms.Label                           lblHint;
        private System.Windows.Forms.Button                          btnOk;
        private System.Windows.Forms.Button                          btnCancel;
        private System.Windows.Forms.TabControl                      tabControl;
        private System.Windows.Forms.TabPage                         tabMessages;
        private System.Windows.Forms.SplitContainer                  splitMain;
        private System.Windows.Forms.TableLayoutPanel                tblUp;
        private System.Windows.Forms.Panel                           pnlUpHdr;
        private System.Windows.Forms.Label                           lblUpTitle;
        private System.Windows.Forms.Button                          btnUpSave;
        private System.Windows.Forms.Button                          btnUpRefresh;
        private System.Windows.Forms.DataGridView                    dgvUp;
        private System.Windows.Forms.DataGridViewTextBoxColumn       colUpIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn       colUpLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn       colUpContent;
        private System.Windows.Forms.TableLayoutPanel                tblDn;
        private System.Windows.Forms.Panel                           pnlDnHdr;
        private System.Windows.Forms.Label                           lblDnTitle;
        private System.Windows.Forms.Button                          btnDnSave;
        private System.Windows.Forms.Button                          btnDnRefresh;
        private System.Windows.Forms.DataGridView                    dgvDn;
        private System.Windows.Forms.DataGridViewTextBoxColumn       colDnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn       colDnLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn       colDnContent;
    }
}
