namespace UITest.Controls
{
    partial class CommStatusControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.btnGet    = new System.Windows.Forms.Button();
            this.dgv       = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();

            // cmbDevice
            this.cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevice.Location      = new System.Drawing.Point(8, 8);
            this.cmbDevice.Size          = new System.Drawing.Size(110, 21);

            // btnGet
            this.btnGet.Location = new System.Drawing.Point(124, 6);
            this.btnGet.Size     = new System.Drawing.Size(50, 24);
            this.btnGet.Text     = "取得";
            this.btnGet.Click   += new System.EventHandler(this.btnGet_Click);

            // dgv
            this.dgv.Location             = new System.Drawing.Point(8, 38);
            this.dgv.Anchor               = System.Windows.Forms.AnchorStyles.Top
                                          | System.Windows.Forms.AnchorStyles.Bottom
                                          | System.Windows.Forms.AnchorStyles.Left
                                          | System.Windows.Forms.AnchorStyles.Right;
            this.dgv.Size                 = new System.Drawing.Size(960, 480);
            this.dgv.AllowUserToAddRows   = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ReadOnly             = true;
            this.dgv.RowHeadersVisible    = false;
            this.dgv.AutoSizeColumnsMode  = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgv.BackgroundColor      = System.Drawing.SystemColors.Window;
            this.dgv.BorderStyle          = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgv.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // CommStatusControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.Size                = new System.Drawing.Size(980, 530);
            this.Controls.Add(this.cmbDevice);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.dgv);

            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ComboBox        cmbDevice;
        private System.Windows.Forms.Button          btnGet;
        private System.Windows.Forms.DataGridView    dgv;
    }
}
