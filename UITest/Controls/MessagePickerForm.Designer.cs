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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabMessages = new System.Windows.Forms.TabPage();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tblUp = new System.Windows.Forms.TableLayoutPanel();
            this.pnlUpHdr = new System.Windows.Forms.Panel();
            this.lblUpTitle = new System.Windows.Forms.Label();
            this.btnUpSave = new System.Windows.Forms.Button();
            this.btnUpRefresh = new System.Windows.Forms.Button();
            this.dgvUp = new System.Windows.Forms.DataGridView();
            this.colUpIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblDn = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDnHdr = new System.Windows.Forms.Panel();
            this.lblDnTitle = new System.Windows.Forms.Label();
            this.btnDnSave = new System.Windows.Forms.Button();
            this.btnDnRefresh = new System.Windows.Forms.Button();
            this.dgvDn = new System.Windows.Forms.DataGridView();
            this.colDnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDnLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvUp)).BeginInit();
            this.tblDn.SuspendLayout();
            this.pnlDnHdr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDn)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.lblHint);
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 580);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1176, 40);
            this.pnlButtons.TabIndex = 1;
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblHint.Location = new System.Drawing.Point(8, 12);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(113, 12);
            this.lblHint.TabIndex = 0;
            this.lblHint.Text = "雙擊訊息列即可選取";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(1706, 7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 26);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "確定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(1788, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabMessages);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1176, 580);
            this.tabControl.TabIndex = 0;
            // 
            // tabMessages
            // 
            this.tabMessages.Controls.Add(this.splitMain);
            this.tabMessages.Location = new System.Drawing.Point(4, 22);
            this.tabMessages.Name = "tabMessages";
            this.tabMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tabMessages.Size = new System.Drawing.Size(1168, 554);
            this.tabMessages.TabIndex = 0;
            this.tabMessages.Text = "上下行訊息";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(3, 3);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.tblUp);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.tblDn);
            this.splitMain.Size = new System.Drawing.Size(1162, 548);
            this.splitMain.SplitterDistance = 937;
            this.splitMain.TabIndex = 0;
            // 
            // tblUp
            // 
            this.tblUp.ColumnCount = 1;
            this.tblUp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblUp.Controls.Add(this.pnlUpHdr, 0, 0);
            this.tblUp.Controls.Add(this.dgvUp, 0, 1);
            this.tblUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblUp.Location = new System.Drawing.Point(0, 0);
            this.tblUp.Name = "tblUp";
            this.tblUp.RowCount = 2;
            this.tblUp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tblUp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblUp.Size = new System.Drawing.Size(937, 548);
            this.tblUp.TabIndex = 0;
            // 
            // pnlUpHdr
            // 
            this.pnlUpHdr.Controls.Add(this.lblUpTitle);
            this.pnlUpHdr.Controls.Add(this.btnUpSave);
            this.pnlUpHdr.Controls.Add(this.btnUpRefresh);
            this.pnlUpHdr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUpHdr.Location = new System.Drawing.Point(3, 3);
            this.pnlUpHdr.Name = "pnlUpHdr";
            this.pnlUpHdr.Size = new System.Drawing.Size(931, 50);
            this.pnlUpHdr.TabIndex = 0;
            // 
            // lblUpTitle
            // 
            this.lblUpTitle.AutoSize = true;
            this.lblUpTitle.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.lblUpTitle.Location = new System.Drawing.Point(4, 4);
            this.lblUpTitle.Name = "lblUpTitle";
            this.lblUpTitle.Size = new System.Drawing.Size(55, 16);
            this.lblUpTitle.TabIndex = 0;
            this.lblUpTitle.Text = "上行訊息";
            // 
            // btnUpSave
            // 
            this.btnUpSave.Location = new System.Drawing.Point(4, 26);
            this.btnUpSave.Name = "btnUpSave";
            this.btnUpSave.Size = new System.Drawing.Size(80, 24);
            this.btnUpSave.TabIndex = 1;
            this.btnUpSave.Text = "儲存表格";
            this.btnUpSave.Click += new System.EventHandler(this.btnUpSave_Click);
            // 
            // btnUpRefresh
            // 
            this.btnUpRefresh.Location = new System.Drawing.Point(90, 26);
            this.btnUpRefresh.Name = "btnUpRefresh";
            this.btnUpRefresh.Size = new System.Drawing.Size(100, 24);
            this.btnUpRefresh.TabIndex = 2;
            this.btnUpRefresh.Text = "上行訊息更新";
            this.btnUpRefresh.Click += new System.EventHandler(this.btnUpRefresh_Click);
            // 
            // dgvUp
            // 
            this.dgvUp.AllowUserToAddRows = false;
            this.dgvUp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUpIndex,
            this.colUpLength,
            this.colUpContent});
            this.dgvUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUp.Location = new System.Drawing.Point(3, 59);
            this.dgvUp.MultiSelect = false;
            this.dgvUp.Name = "dgvUp";
            this.dgvUp.RowHeadersWidth = 25;
            this.dgvUp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUp.Size = new System.Drawing.Size(931, 486);
            this.dgvUp.TabIndex = 1;
            this.dgvUp.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUp_CellDoubleClick);
            this.dgvUp.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellEndEdit);
            // 
            // colUpIndex
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colUpIndex.DefaultCellStyle = dataGridViewCellStyle1;
            this.colUpIndex.HeaderText = "索引值";
            this.colUpIndex.Name = "colUpIndex";
            this.colUpIndex.ReadOnly = true;
            this.colUpIndex.Width = 55;
            // 
            // colUpLength
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colUpLength.DefaultCellStyle = dataGridViewCellStyle2;
            this.colUpLength.HeaderText = "訊息長度";
            this.colUpLength.Name = "colUpLength";
            this.colUpLength.ReadOnly = true;
            this.colUpLength.Width = 60;
            // 
            // colUpContent
            // 
            this.colUpContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colUpContent.HeaderText = "上行訊息編輯內容（雙擊選取）";
            this.colUpContent.Name = "colUpContent";
            // 
            // tblDn
            // 
            this.tblDn.ColumnCount = 1;
            this.tblDn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblDn.Controls.Add(this.pnlDnHdr, 0, 0);
            this.tblDn.Controls.Add(this.dgvDn, 0, 1);
            this.tblDn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblDn.Location = new System.Drawing.Point(0, 0);
            this.tblDn.Name = "tblDn";
            this.tblDn.RowCount = 2;
            this.tblDn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tblDn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblDn.Size = new System.Drawing.Size(221, 548);
            this.tblDn.TabIndex = 0;
            // 
            // pnlDnHdr
            // 
            this.pnlDnHdr.Controls.Add(this.lblDnTitle);
            this.pnlDnHdr.Controls.Add(this.btnDnSave);
            this.pnlDnHdr.Controls.Add(this.btnDnRefresh);
            this.pnlDnHdr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDnHdr.Location = new System.Drawing.Point(3, 3);
            this.pnlDnHdr.Name = "pnlDnHdr";
            this.pnlDnHdr.Size = new System.Drawing.Size(215, 50);
            this.pnlDnHdr.TabIndex = 0;
            // 
            // lblDnTitle
            // 
            this.lblDnTitle.AutoSize = true;
            this.lblDnTitle.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);
            this.lblDnTitle.Location = new System.Drawing.Point(4, 4);
            this.lblDnTitle.Name = "lblDnTitle";
            this.lblDnTitle.Size = new System.Drawing.Size(55, 16);
            this.lblDnTitle.TabIndex = 0;
            this.lblDnTitle.Text = "下行訊息";
            // 
            // btnDnSave
            // 
            this.btnDnSave.Location = new System.Drawing.Point(4, 26);
            this.btnDnSave.Name = "btnDnSave";
            this.btnDnSave.Size = new System.Drawing.Size(80, 24);
            this.btnDnSave.TabIndex = 1;
            this.btnDnSave.Text = "儲存表格";
            this.btnDnSave.Click += new System.EventHandler(this.btnDnSave_Click);
            // 
            // btnDnRefresh
            // 
            this.btnDnRefresh.Location = new System.Drawing.Point(90, 26);
            this.btnDnRefresh.Name = "btnDnRefresh";
            this.btnDnRefresh.Size = new System.Drawing.Size(100, 24);
            this.btnDnRefresh.TabIndex = 2;
            this.btnDnRefresh.Text = "下行訊息更新";
            this.btnDnRefresh.Click += new System.EventHandler(this.btnDnRefresh_Click);
            // 
            // dgvDn
            // 
            this.dgvDn.AllowUserToAddRows = false;
            this.dgvDn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDnIndex,
            this.colDnLength,
            this.colDnContent});
            this.dgvDn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDn.Location = new System.Drawing.Point(3, 59);
            this.dgvDn.MultiSelect = false;
            this.dgvDn.Name = "dgvDn";
            this.dgvDn.RowHeadersWidth = 25;
            this.dgvDn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDn.Size = new System.Drawing.Size(215, 486);
            this.dgvDn.TabIndex = 1;
            this.dgvDn.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDn_CellDoubleClick);
            this.dgvDn.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellEndEdit);
            // 
            // colDnIndex
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDnIndex.DefaultCellStyle = dataGridViewCellStyle3;
            this.colDnIndex.HeaderText = "索引值";
            this.colDnIndex.Name = "colDnIndex";
            this.colDnIndex.ReadOnly = true;
            this.colDnIndex.Width = 55;
            // 
            // colDnLength
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDnLength.DefaultCellStyle = dataGridViewCellStyle4;
            this.colDnLength.HeaderText = "訊息長度";
            this.colDnLength.Name = "colDnLength";
            this.colDnLength.ReadOnly = true;
            this.colDnLength.Width = 60;
            // 
            // colDnContent
            // 
            this.colDnContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDnContent.HeaderText = "下行訊息編輯內容（雙擊選取）";
            this.colDnContent.Name = "colDnContent";
            // 
            // MessagePickerForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1176, 620);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlButtons);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MessagePickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "板型編輯  上下行訊息";
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabMessages.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.tblUp.ResumeLayout(false);
            this.pnlUpHdr.ResumeLayout(false);
            this.pnlUpHdr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUp)).EndInit();
            this.tblDn.ResumeLayout(false);
            this.pnlDnHdr.ResumeLayout(false);
            this.pnlDnHdr.PerformLayout();
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
