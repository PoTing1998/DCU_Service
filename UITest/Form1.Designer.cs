namespace UITest
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPacketBuilder = new System.Windows.Forms.TabPage();
            this.grpPacketOutput = new System.Windows.Forms.GroupBox();
            this.btnClearPacketOutput = new System.Windows.Forms.Button();
            this.txtPacketOutput = new System.Windows.Forms.TextBox();
            this.grpPacketBuilder = new System.Windows.Forms.GroupBox();
            this.btnGeneratePacket = new System.Windows.Forms.Button();
            this.grpMessageInput = new System.Windows.Forms.GroupBox();
            this.txtMessageInput = new System.Windows.Forms.TextBox();
            this.lblMessageInput = new System.Windows.Forms.Label();
            this.tabPacketValidator = new System.Windows.Forms.TabPage();
            this.grpValidationResult = new System.Windows.Forms.GroupBox();
            this.btnClearResult = new System.Windows.Forms.Button();
            this.txtValidationResult = new System.Windows.Forms.TextBox();
            this.grpPacketInput = new System.Windows.Forms.GroupBox();
            this.lblPacketType = new System.Windows.Forms.Label();
            this.cmbPacketType = new System.Windows.Forms.ComboBox();
            this.lblPacketInput = new System.Windows.Forms.Label();
            this.txtPacketInput = new System.Windows.Forms.TextBox();
            this.btnLoadSample = new System.Windows.Forms.Button();
            this.btnValidatePacket = new System.Windows.Forms.Button();
            this.lblSampleDescription = new System.Windows.Forms.Label();
            this.tabTools = new System.Windows.Forms.TabPage();
            this.grpColorConversion = new System.Windows.Forms.GroupBox();
            this.txtColorOutput = new System.Windows.Forms.TextBox();
            this.lblColorOutput = new System.Windows.Forms.Label();
            this.txtColorInput = new System.Windows.Forms.TextBox();
            this.lblColorInput = new System.Windows.Forms.Label();
            this.btnConvertColor = new System.Windows.Forms.Button();
            this.grpDeviceCheck = new System.Windows.Forms.GroupBox();
            this.btnLoadDevicesFromDB = new System.Windows.Forms.Button();
            this.txtDeviceOutput = new System.Windows.Forms.TextBox();
            this.lblDeviceOutput = new System.Windows.Forms.Label();
            this.txtDeviceInput = new System.Windows.Forms.TextBox();
            this.lblDeviceInput = new System.Windows.Forms.Label();
            this.btnCheckDevice = new System.Windows.Forms.Button();
            this.grpFormatConversion = new System.Windows.Forms.GroupBox();
            this.txtFormatOutput = new System.Windows.Forms.TextBox();
            this.lblFormatOutput = new System.Windows.Forms.Label();
            this.txtFormatInput = new System.Windows.Forms.TextBox();
            this.lblFormatInput = new System.Windows.Forms.Label();
            this.btnConvertFormat = new System.Windows.Forms.Button();
            this.tabDatabaseSettings = new System.Windows.Forms.TabPage();
            this.grpDatabaseConnection = new System.Windows.Forms.GroupBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnUpdateConnectionString = new System.Windows.Forms.Button();
            this.txtDbPassword = new System.Windows.Forms.TextBox();
            this.lblDbPassword = new System.Windows.Forms.Label();
            this.txtDbUserId = new System.Windows.Forms.TextBox();
            this.lblDbUserId = new System.Windows.Forms.Label();
            this.txtDbDatabase = new System.Windows.Forms.TextBox();
            this.lblDbDatabase = new System.Windows.Forms.Label();
            this.txtDbPort = new System.Windows.Forms.TextBox();
            this.lblDbPort = new System.Windows.Forms.Label();
            this.txtDbServer = new System.Windows.Forms.TextBox();
            this.lblDbServer = new System.Windows.Forms.Label();
            this.grpCurrentConnectionString = new System.Windows.Forms.GroupBox();
            this.txtDbStatus = new System.Windows.Forms.TextBox();
            this.txtDbConnectionString = new System.Windows.Forms.TextBox();
            this.lblDbConnectionString = new System.Windows.Forms.Label();
            this.grpMessageSender = new System.Windows.Forms.GroupBox();
            this.grpTableBrowser = new System.Windows.Forms.GroupBox();
            this.lblTableName = new System.Windows.Forms.Label();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.btnLoadTables = new System.Windows.Forms.Button();
            this.lblTableFilter = new System.Windows.Forms.Label();
            this.txtTableFilter = new System.Windows.Forms.TextBox();
            this.btnQueryTable = new System.Windows.Forms.Button();
            this.btnClearTableData = new System.Windows.Forms.Button();
            this.lblRowCount = new System.Windows.Forms.Label();
            this.dgvTableData = new System.Windows.Forms.DataGridView();
            this.txtSimpleResult = new System.Windows.Forms.TextBox();
            this.btnSimpleClearResult = new System.Windows.Forms.Button();
            this.btnSimpleUseDefaults = new System.Windows.Forms.Button();
            this.btnSimpleSendMessage = new System.Windows.Forms.Button();
            this.chkSimpleInstantMessage = new System.Windows.Forms.CheckBox();
            this.txtSimpleTargetDevice = new System.Windows.Forms.TextBox();
            this.lblSimpleTargetDevice = new System.Windows.Forms.Label();
            this.txtSimpleMessageText = new System.Windows.Forms.TextBox();
            this.lblSimpleMessageText = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPacketBuilder.SuspendLayout();
            this.grpPacketOutput.SuspendLayout();
            this.grpPacketBuilder.SuspendLayout();
            this.grpMessageInput.SuspendLayout();
            this.tabPacketValidator.SuspendLayout();
            this.grpValidationResult.SuspendLayout();
            this.grpPacketInput.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.grpColorConversion.SuspendLayout();
            this.grpDeviceCheck.SuspendLayout();
            this.grpFormatConversion.SuspendLayout();
            this.tabDatabaseSettings.SuspendLayout();
            this.grpDatabaseConnection.SuspendLayout();
            this.grpCurrentConnectionString.SuspendLayout();
            this.grpMessageSender.SuspendLayout();
            this.grpTableBrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableData)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPacketBuilder);
            this.tabControl1.Controls.Add(this.tabPacketValidator);
            this.tabControl1.Controls.Add(this.tabTools);
            this.tabControl1.Controls.Add(this.tabDatabaseSettings);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1084, 761);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPacketBuilder
            // 
            this.tabPacketBuilder.Controls.Add(this.grpPacketOutput);
            this.tabPacketBuilder.Controls.Add(this.grpPacketBuilder);
            this.tabPacketBuilder.Controls.Add(this.grpMessageInput);
            this.tabPacketBuilder.Location = new System.Drawing.Point(4, 22);
            this.tabPacketBuilder.Name = "tabPacketBuilder";
            this.tabPacketBuilder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPacketBuilder.Size = new System.Drawing.Size(1076, 735);
            this.tabPacketBuilder.TabIndex = 0;
            this.tabPacketBuilder.Text = "封包組成測試";
            this.tabPacketBuilder.UseVisualStyleBackColor = true;
            // 
            // grpPacketOutput
            // 
            this.grpPacketOutput.Controls.Add(this.btnClearPacketOutput);
            this.grpPacketOutput.Controls.Add(this.txtPacketOutput);
            this.grpPacketOutput.Location = new System.Drawing.Point(20, 280);
            this.grpPacketOutput.Name = "grpPacketOutput";
            this.grpPacketOutput.Size = new System.Drawing.Size(1030, 430);
            this.grpPacketOutput.TabIndex = 2;
            this.grpPacketOutput.TabStop = false;
            this.grpPacketOutput.Text = "輸出結果";
            // 
            // btnClearPacketOutput
            // 
            this.btnClearPacketOutput.Location = new System.Drawing.Point(20, 25);
            this.btnClearPacketOutput.Name = "btnClearPacketOutput";
            this.btnClearPacketOutput.Size = new System.Drawing.Size(120, 30);
            this.btnClearPacketOutput.TabIndex = 1;
            this.btnClearPacketOutput.Text = "清除結果";
            this.btnClearPacketOutput.UseVisualStyleBackColor = true;
            this.btnClearPacketOutput.Click += new System.EventHandler(this.btnClearPacketOutput_Click);
            // 
            // txtPacketOutput
            // 
            this.txtPacketOutput.Location = new System.Drawing.Point(20, 65);
            this.txtPacketOutput.Multiline = true;
            this.txtPacketOutput.Name = "txtPacketOutput";
            this.txtPacketOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPacketOutput.Size = new System.Drawing.Size(990, 350);
            this.txtPacketOutput.TabIndex = 0;
            // 
            // grpPacketBuilder
            //
            this.grpPacketBuilder.Controls.Add(this.btnGeneratePacket);
            this.grpPacketBuilder.Location = new System.Drawing.Point(400, 20);
            this.grpPacketBuilder.Name = "grpPacketBuilder";
            this.grpPacketBuilder.Size = new System.Drawing.Size(200, 240);
            this.grpPacketBuilder.TabIndex = 1;
            this.grpPacketBuilder.TabStop = false;
            this.grpPacketBuilder.Text = "封包生成";
            //
            // btnGeneratePacket
            //
            this.btnGeneratePacket.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneratePacket.Location = new System.Drawing.Point(20, 80);
            this.btnGeneratePacket.Name = "btnGeneratePacket";
            this.btnGeneratePacket.Size = new System.Drawing.Size(160, 80);
            this.btnGeneratePacket.TabIndex = 0;
            this.btnGeneratePacket.Text = "一鍵生成\r\n完整封包";
            this.btnGeneratePacket.UseVisualStyleBackColor = true;
            this.btnGeneratePacket.Click += new System.EventHandler(this.btnGeneratePacket_Click);
            // 
            // grpMessageInput
            // 
            this.grpMessageInput.Controls.Add(this.txtMessageInput);
            this.grpMessageInput.Controls.Add(this.lblMessageInput);
            this.grpMessageInput.Location = new System.Drawing.Point(20, 20);
            this.grpMessageInput.Name = "grpMessageInput";
            this.grpMessageInput.Size = new System.Drawing.Size(360, 240);
            this.grpMessageInput.TabIndex = 0;
            this.grpMessageInput.TabStop = false;
            this.grpMessageInput.Text = "輸入訊息";
            // 
            // txtMessageInput
            // 
            this.txtMessageInput.Location = new System.Drawing.Point(20, 50);
            this.txtMessageInput.Multiline = true;
            this.txtMessageInput.Name = "txtMessageInput";
            this.txtMessageInput.Size = new System.Drawing.Size(320, 170);
            this.txtMessageInput.TabIndex = 1;
            // 
            // lblMessageInput
            // 
            this.lblMessageInput.AutoSize = true;
            this.lblMessageInput.Location = new System.Drawing.Point(20, 30);
            this.lblMessageInput.Name = "lblMessageInput";
            this.lblMessageInput.Size = new System.Drawing.Size(272, 12);
            this.lblMessageInput.TabIndex = 0;
            this.lblMessageInput.Text = "請輸入訊息 (格式: 字體顏色 : xxx\\r\\n字體內容 : xxx)";
            // 
            // tabPacketValidator
            //
            this.tabPacketValidator.Controls.Add(this.grpValidationResult);
            this.tabPacketValidator.Controls.Add(this.grpPacketInput);
            this.tabPacketValidator.Location = new System.Drawing.Point(4, 22);
            this.tabPacketValidator.Name = "tabPacketValidator";
            this.tabPacketValidator.Padding = new System.Windows.Forms.Padding(3);
            this.tabPacketValidator.Size = new System.Drawing.Size(1076, 735);
            this.tabPacketValidator.TabIndex = 1;
            this.tabPacketValidator.Text = "封包驗證";
            this.tabPacketValidator.UseVisualStyleBackColor = true;
            // 
            // grpValidationResult
            //
            this.grpValidationResult.Controls.Add(this.btnClearResult);
            this.grpValidationResult.Controls.Add(this.txtValidationResult);
            this.grpValidationResult.Location = new System.Drawing.Point(20, 260);
            this.grpValidationResult.Name = "grpValidationResult";
            this.grpValidationResult.Size = new System.Drawing.Size(1030, 450);
            this.grpValidationResult.TabIndex = 1;
            this.grpValidationResult.TabStop = false;
            this.grpValidationResult.Text = "驗證結果";
            //
            // btnClearResult
            //
            this.btnClearResult.Location = new System.Drawing.Point(20, 25);
            this.btnClearResult.Name = "btnClearResult";
            this.btnClearResult.Size = new System.Drawing.Size(120, 35);
            this.btnClearResult.TabIndex = 1;
            this.btnClearResult.Text = "清除結果";
            this.btnClearResult.UseVisualStyleBackColor = true;
            this.btnClearResult.Click += new System.EventHandler(this.ClearBT_Click);
            //
            // txtValidationResult
            //
            this.txtValidationResult.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValidationResult.Location = new System.Drawing.Point(20, 70);
            this.txtValidationResult.Multiline = true;
            this.txtValidationResult.Name = "txtValidationResult";
            this.txtValidationResult.ReadOnly = true;
            this.txtValidationResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtValidationResult.Size = new System.Drawing.Size(990, 360);
            this.txtValidationResult.TabIndex = 0;
            // 
            //
            // grpPacketInput
            //
            this.grpPacketInput.Controls.Add(this.lblPacketType);
            this.grpPacketInput.Controls.Add(this.cmbPacketType);
            this.grpPacketInput.Controls.Add(this.lblSampleDescription);
            this.grpPacketInput.Controls.Add(this.lblPacketInput);
            this.grpPacketInput.Controls.Add(this.txtPacketInput);
            this.grpPacketInput.Controls.Add(this.btnLoadSample);
            this.grpPacketInput.Controls.Add(this.btnValidatePacket);
            this.grpPacketInput.Location = new System.Drawing.Point(20, 20);
            this.grpPacketInput.Name = "grpPacketInput";
            this.grpPacketInput.Size = new System.Drawing.Size(1030, 220);
            this.grpPacketInput.TabIndex = 0;
            this.grpPacketInput.TabStop = false;
            this.grpPacketInput.Text = "封包輸入";
            //
            // lblPacketType
            //
            this.lblPacketType.AutoSize = true;
            this.lblPacketType.Location = new System.Drawing.Point(20, 30);
            this.lblPacketType.Name = "lblPacketType";
            this.lblPacketType.Size = new System.Drawing.Size(80, 12);
            this.lblPacketType.TabIndex = 0;
            this.lblPacketType.Text = "封包類型:";
            //
            // cmbPacketType
            //
            this.cmbPacketType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPacketType.FormattingEnabled = true;
            this.cmbPacketType.Location = new System.Drawing.Point(110, 27);
            this.cmbPacketType.Name = "cmbPacketType";
            this.cmbPacketType.Size = new System.Drawing.Size(350, 20);
            this.cmbPacketType.TabIndex = 1;
            this.cmbPacketType.SelectedIndexChanged += new System.EventHandler(this.cmbPacketType_SelectedIndexChanged);
            //
            // lblSampleDescription
            //
            this.lblSampleDescription.AutoSize = true;
            this.lblSampleDescription.ForeColor = System.Drawing.Color.Blue;
            this.lblSampleDescription.Location = new System.Drawing.Point(480, 30);
            this.lblSampleDescription.Name = "lblSampleDescription";
            this.lblSampleDescription.Size = new System.Drawing.Size(200, 12);
            this.lblSampleDescription.TabIndex = 2;
            this.lblSampleDescription.Text = "";
            //
            // lblPacketInput
            //
            this.lblPacketInput.AutoSize = true;
            this.lblPacketInput.Location = new System.Drawing.Point(20, 65);
            this.lblPacketInput.Name = "lblPacketInput";
            this.lblPacketInput.Size = new System.Drawing.Size(380, 12);
            this.lblPacketInput.TabIndex = 3;
            this.lblPacketInput.Text = "封包內容 (十六進制，可用空格或無空格分隔，例如: 55 AA 01 或 55AA01):";
            //
            // txtPacketInput
            //
            this.txtPacketInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPacketInput.Location = new System.Drawing.Point(20, 85);
            this.txtPacketInput.Multiline = true;
            this.txtPacketInput.Name = "txtPacketInput";
            this.txtPacketInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPacketInput.Size = new System.Drawing.Size(840, 110);
            this.txtPacketInput.TabIndex = 4;
            //
            // btnLoadSample
            //
            this.btnLoadSample.Location = new System.Drawing.Point(880, 85);
            this.btnLoadSample.Name = "btnLoadSample";
            this.btnLoadSample.Size = new System.Drawing.Size(130, 45);
            this.btnLoadSample.TabIndex = 5;
            this.btnLoadSample.Text = "載入範例封包";
            this.btnLoadSample.UseVisualStyleBackColor = true;
            this.btnLoadSample.Click += new System.EventHandler(this.btnLoadSample_Click);
            //
            // btnValidatePacket
            //
            this.btnValidatePacket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidatePacket.Location = new System.Drawing.Point(880, 145);
            this.btnValidatePacket.Name = "btnValidatePacket";
            this.btnValidatePacket.Size = new System.Drawing.Size(130, 50);
            this.btnValidatePacket.TabIndex = 6;
            this.btnValidatePacket.Text = "驗證封包";
            this.btnValidatePacket.UseVisualStyleBackColor = true;
            this.btnValidatePacket.Click += new System.EventHandler(this.btnValidatePacket_Click);
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.grpColorConversion);
            this.tabTools.Controls.Add(this.grpDeviceCheck);
            this.tabTools.Controls.Add(this.grpFormatConversion);
            this.tabTools.Location = new System.Drawing.Point(4, 22);
            this.tabTools.Name = "tabTools";
            this.tabTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabTools.Size = new System.Drawing.Size(1076, 735);
            this.tabTools.TabIndex = 2;
            this.tabTools.Text = "工具";
            this.tabTools.UseVisualStyleBackColor = true;
            // 
            // grpColorConversion
            // 
            this.grpColorConversion.Controls.Add(this.txtColorOutput);
            this.grpColorConversion.Controls.Add(this.lblColorOutput);
            this.grpColorConversion.Controls.Add(this.txtColorInput);
            this.grpColorConversion.Controls.Add(this.lblColorInput);
            this.grpColorConversion.Controls.Add(this.btnConvertColor);
            this.grpColorConversion.Location = new System.Drawing.Point(559, 20);
            this.grpColorConversion.Name = "grpColorConversion";
            this.grpColorConversion.Size = new System.Drawing.Size(500, 220);
            this.grpColorConversion.TabIndex = 2;
            this.grpColorConversion.TabStop = false;
            this.grpColorConversion.Text = "色碼轉換";
            // 
            // txtColorOutput
            // 
            this.txtColorOutput.Location = new System.Drawing.Point(20, 140);
            this.txtColorOutput.Multiline = true;
            this.txtColorOutput.Name = "txtColorOutput";
            this.txtColorOutput.ReadOnly = true;
            this.txtColorOutput.Size = new System.Drawing.Size(460, 60);
            this.txtColorOutput.TabIndex = 4;
            // 
            // lblColorOutput
            // 
            this.lblColorOutput.AutoSize = true;
            this.lblColorOutput.Location = new System.Drawing.Point(20, 120);
            this.lblColorOutput.Name = "lblColorOutput";
            this.lblColorOutput.Size = new System.Drawing.Size(53, 12);
            this.lblColorOutput.TabIndex = 3;
            this.lblColorOutput.Text = "轉換結果";
            // 
            // txtColorInput
            // 
            this.txtColorInput.Location = new System.Drawing.Point(20, 50);
            this.txtColorInput.Name = "txtColorInput";
            this.txtColorInput.Size = new System.Drawing.Size(330, 22);
            this.txtColorInput.TabIndex = 2;
            // 
            // lblColorInput
            // 
            this.lblColorInput.AutoSize = true;
            this.lblColorInput.Location = new System.Drawing.Point(20, 30);
            this.lblColorInput.Name = "lblColorInput";
            this.lblColorInput.Size = new System.Drawing.Size(215, 12);
            this.lblColorInput.TabIndex = 1;
            this.lblColorInput.Text = "請輸入色碼名稱 (例如: red, green, blue等)";
            // 
            // btnConvertColor
            // 
            this.btnConvertColor.Location = new System.Drawing.Point(370, 45);
            this.btnConvertColor.Name = "btnConvertColor";
            this.btnConvertColor.Size = new System.Drawing.Size(110, 30);
            this.btnConvertColor.TabIndex = 0;
            this.btnConvertColor.Text = "轉換色碼";
            this.btnConvertColor.UseVisualStyleBackColor = true;
            this.btnConvertColor.Click += new System.EventHandler(this.button8_Click);
            // 
            // grpDeviceCheck
            // 
            this.grpDeviceCheck.Controls.Add(this.btnLoadDevicesFromDB);
            this.grpDeviceCheck.Controls.Add(this.txtDeviceOutput);
            this.grpDeviceCheck.Controls.Add(this.lblDeviceOutput);
            this.grpDeviceCheck.Controls.Add(this.txtDeviceInput);
            this.grpDeviceCheck.Controls.Add(this.lblDeviceInput);
            this.grpDeviceCheck.Controls.Add(this.btnCheckDevice);
            this.grpDeviceCheck.Location = new System.Drawing.Point(20, 250);
            this.grpDeviceCheck.Name = "grpDeviceCheck";
            this.grpDeviceCheck.Size = new System.Drawing.Size(643, 393);
            this.grpDeviceCheck.TabIndex = 1;
            this.grpDeviceCheck.TabStop = false;
            this.grpDeviceCheck.Text = "設備檢查";
            // 
            // btnLoadDevicesFromDB
            // 
            this.btnLoadDevicesFromDB.Location = new System.Drawing.Point(370, 30);
            this.btnLoadDevicesFromDB.Name = "btnLoadDevicesFromDB";
            this.btnLoadDevicesFromDB.Size = new System.Drawing.Size(230, 30);
            this.btnLoadDevicesFromDB.TabIndex = 5;
            this.btnLoadDevicesFromDB.Text = "從資料庫載入設備清單";
            this.btnLoadDevicesFromDB.UseVisualStyleBackColor = true;
            this.btnLoadDevicesFromDB.Click += new System.EventHandler(this.btnLoadDevicesFromDB_Click);
            // 
            // txtDeviceOutput
            // 
            this.txtDeviceOutput.Location = new System.Drawing.Point(20, 247);
            this.txtDeviceOutput.Multiline = true;
            this.txtDeviceOutput.Name = "txtDeviceOutput";
            this.txtDeviceOutput.ReadOnly = true;
            this.txtDeviceOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDeviceOutput.Size = new System.Drawing.Size(580, 128);
            this.txtDeviceOutput.TabIndex = 4;
            // 
            // lblDeviceOutput
            // 
            this.lblDeviceOutput.AutoSize = true;
            this.lblDeviceOutput.Location = new System.Drawing.Point(20, 227);
            this.lblDeviceOutput.Name = "lblDeviceOutput";
            this.lblDeviceOutput.Size = new System.Drawing.Size(53, 12);
            this.lblDeviceOutput.TabIndex = 3;
            this.lblDeviceOutput.Text = "檢查結果";
            // 
            // txtDeviceInput
            // 
            this.txtDeviceInput.Location = new System.Drawing.Point(20, 50);
            this.txtDeviceInput.Multiline = true;
            this.txtDeviceInput.Name = "txtDeviceInput";
            this.txtDeviceInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDeviceInput.Size = new System.Drawing.Size(330, 143);
            this.txtDeviceInput.TabIndex = 2;
            // 
            // lblDeviceInput
            // 
            this.lblDeviceInput.AutoSize = true;
            this.lblDeviceInput.Location = new System.Drawing.Point(20, 30);
            this.lblDeviceInput.Name = "lblDeviceInput";
            this.lblDeviceInput.Size = new System.Drawing.Size(281, 12);
            this.lblDeviceInput.TabIndex = 1;
            this.lblDeviceInput.Text = "請輸入設備清單 (逗號分隔，例如: LG01_UPF_PDU-1)";
            // 
            // btnCheckDevice
            // 
            this.btnCheckDevice.Location = new System.Drawing.Point(370, 70);
            this.btnCheckDevice.Name = "btnCheckDevice";
            this.btnCheckDevice.Size = new System.Drawing.Size(230, 30);
            this.btnCheckDevice.TabIndex = 0;
            this.btnCheckDevice.Text = "檢查設備 (篩選符合模式)";
            this.btnCheckDevice.UseVisualStyleBackColor = true;
            this.btnCheckDevice.Click += new System.EventHandler(this.button7_Click);
            // 
            // grpFormatConversion
            // 
            this.grpFormatConversion.Controls.Add(this.txtFormatOutput);
            this.grpFormatConversion.Controls.Add(this.lblFormatOutput);
            this.grpFormatConversion.Controls.Add(this.txtFormatInput);
            this.grpFormatConversion.Controls.Add(this.lblFormatInput);
            this.grpFormatConversion.Controls.Add(this.btnConvertFormat);
            this.grpFormatConversion.Location = new System.Drawing.Point(20, 20);
            this.grpFormatConversion.Name = "grpFormatConversion";
            this.grpFormatConversion.Size = new System.Drawing.Size(500, 220);
            this.grpFormatConversion.TabIndex = 0;
            this.grpFormatConversion.TabStop = false;
            this.grpFormatConversion.Text = "格式轉換 (十六進制字串加空格)";
            // 
            // txtFormatOutput
            // 
            this.txtFormatOutput.Location = new System.Drawing.Point(20, 140);
            this.txtFormatOutput.Multiline = true;
            this.txtFormatOutput.Name = "txtFormatOutput";
            this.txtFormatOutput.ReadOnly = true;
            this.txtFormatOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFormatOutput.Size = new System.Drawing.Size(460, 60);
            this.txtFormatOutput.TabIndex = 4;
            // 
            // lblFormatOutput
            // 
            this.lblFormatOutput.AutoSize = true;
            this.lblFormatOutput.Location = new System.Drawing.Point(20, 120);
            this.lblFormatOutput.Name = "lblFormatOutput";
            this.lblFormatOutput.Size = new System.Drawing.Size(53, 12);
            this.lblFormatOutput.TabIndex = 3;
            this.lblFormatOutput.Text = "轉換結果";
            // 
            // txtFormatInput
            // 
            this.txtFormatInput.Location = new System.Drawing.Point(20, 50);
            this.txtFormatInput.Multiline = true;
            this.txtFormatInput.Name = "txtFormatInput";
            this.txtFormatInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFormatInput.Size = new System.Drawing.Size(330, 50);
            this.txtFormatInput.TabIndex = 2;
            // 
            // lblFormatInput
            // 
            this.lblFormatInput.AutoSize = true;
            this.lblFormatInput.Location = new System.Drawing.Point(20, 30);
            this.lblFormatInput.Name = "lblFormatInput";
            this.lblFormatInput.Size = new System.Drawing.Size(266, 12);
            this.lblFormatInput.TabIndex = 1;
            this.lblFormatInput.Text = "請輸入十六進制字串 (例如: 55AA0111... 不含空格)";
            // 
            // btnConvertFormat
            // 
            this.btnConvertFormat.Location = new System.Drawing.Point(370, 50);
            this.btnConvertFormat.Name = "btnConvertFormat";
            this.btnConvertFormat.Size = new System.Drawing.Size(110, 50);
            this.btnConvertFormat.TabIndex = 0;
            this.btnConvertFormat.Text = "格式轉換";
            this.btnConvertFormat.UseVisualStyleBackColor = true;
            this.btnConvertFormat.Click += new System.EventHandler(this.button6_Click);
            // 
            // tabDatabaseSettings
            //
            this.tabDatabaseSettings.AutoScroll = true;
            this.tabDatabaseSettings.Controls.Add(this.grpTableBrowser);
            this.tabDatabaseSettings.Controls.Add(this.grpMessageSender);
            this.tabDatabaseSettings.Controls.Add(this.grpDatabaseConnection);
            this.tabDatabaseSettings.Controls.Add(this.grpCurrentConnectionString);
            this.tabDatabaseSettings.Location = new System.Drawing.Point(4, 22);
            this.tabDatabaseSettings.Name = "tabDatabaseSettings";
            this.tabDatabaseSettings.Size = new System.Drawing.Size(1076, 735);
            this.tabDatabaseSettings.TabIndex = 3;
            this.tabDatabaseSettings.Text = "資料庫設定";
            this.tabDatabaseSettings.UseVisualStyleBackColor = true;
            // 
            // grpDatabaseConnection
            // 
            this.grpDatabaseConnection.Controls.Add(this.btnTestConnection);
            this.grpDatabaseConnection.Controls.Add(this.btnUpdateConnectionString);
            this.grpDatabaseConnection.Controls.Add(this.txtDbPassword);
            this.grpDatabaseConnection.Controls.Add(this.lblDbPassword);
            this.grpDatabaseConnection.Controls.Add(this.txtDbUserId);
            this.grpDatabaseConnection.Controls.Add(this.lblDbUserId);
            this.grpDatabaseConnection.Controls.Add(this.txtDbDatabase);
            this.grpDatabaseConnection.Controls.Add(this.lblDbDatabase);
            this.grpDatabaseConnection.Controls.Add(this.txtDbPort);
            this.grpDatabaseConnection.Controls.Add(this.lblDbPort);
            this.grpDatabaseConnection.Controls.Add(this.txtDbServer);
            this.grpDatabaseConnection.Controls.Add(this.lblDbServer);
            this.grpDatabaseConnection.Location = new System.Drawing.Point(20, 20);
            this.grpDatabaseConnection.Name = "grpDatabaseConnection";
            this.grpDatabaseConnection.Size = new System.Drawing.Size(500, 300);
            this.grpDatabaseConnection.TabIndex = 0;
            this.grpDatabaseConnection.TabStop = false;
            this.grpDatabaseConnection.Text = "資料庫連線設定";
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(320, 210);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(150, 35);
            this.btnTestConnection.TabIndex = 11;
            this.btnTestConnection.Text = "測試連線";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // btnUpdateConnectionString
            // 
            this.btnUpdateConnectionString.Location = new System.Drawing.Point(150, 210);
            this.btnUpdateConnectionString.Name = "btnUpdateConnectionString";
            this.btnUpdateConnectionString.Size = new System.Drawing.Size(150, 35);
            this.btnUpdateConnectionString.TabIndex = 10;
            this.btnUpdateConnectionString.Text = "更新連線字串";
            this.btnUpdateConnectionString.UseVisualStyleBackColor = true;
            this.btnUpdateConnectionString.Click += new System.EventHandler(this.btnUpdateConnectionString_Click);
            // 
            // txtDbPassword
            // 
            this.txtDbPassword.Location = new System.Drawing.Point(150, 167);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.PasswordChar = '*';
            this.txtDbPassword.Size = new System.Drawing.Size(320, 22);
            this.txtDbPassword.TabIndex = 9;
            // 
            // lblDbPassword
            // 
            this.lblDbPassword.AutoSize = true;
            this.lblDbPassword.Location = new System.Drawing.Point(20, 170);
            this.lblDbPassword.Name = "lblDbPassword";
            this.lblDbPassword.Size = new System.Drawing.Size(86, 12);
            this.lblDbPassword.TabIndex = 8;
            this.lblDbPassword.Text = "密碼 (Password):";
            // 
            // txtDbUserId
            // 
            this.txtDbUserId.Location = new System.Drawing.Point(150, 132);
            this.txtDbUserId.Name = "txtDbUserId";
            this.txtDbUserId.Size = new System.Drawing.Size(320, 22);
            this.txtDbUserId.TabIndex = 7;
            // 
            // lblDbUserId
            // 
            this.lblDbUserId.AutoSize = true;
            this.lblDbUserId.Location = new System.Drawing.Point(20, 135);
            this.lblDbUserId.Name = "lblDbUserId";
            this.lblDbUserId.Size = new System.Drawing.Size(101, 12);
            this.lblDbUserId.TabIndex = 6;
            this.lblDbUserId.Text = "使用者ID (User Id):";
            // 
            // txtDbDatabase
            // 
            this.txtDbDatabase.Location = new System.Drawing.Point(150, 97);
            this.txtDbDatabase.Name = "txtDbDatabase";
            this.txtDbDatabase.Size = new System.Drawing.Size(320, 22);
            this.txtDbDatabase.TabIndex = 5;
            // 
            // lblDbDatabase
            // 
            this.lblDbDatabase.AutoSize = true;
            this.lblDbDatabase.Location = new System.Drawing.Point(20, 100);
            this.lblDbDatabase.Name = "lblDbDatabase";
            this.lblDbDatabase.Size = new System.Drawing.Size(96, 12);
            this.lblDbDatabase.TabIndex = 4;
            this.lblDbDatabase.Text = "資料庫 (Database):";
            // 
            // txtDbPort
            // 
            this.txtDbPort.Location = new System.Drawing.Point(150, 62);
            this.txtDbPort.Name = "txtDbPort";
            this.txtDbPort.Size = new System.Drawing.Size(320, 22);
            this.txtDbPort.TabIndex = 3;
            // 
            // lblDbPort
            // 
            this.lblDbPort.AutoSize = true;
            this.lblDbPort.Location = new System.Drawing.Point(20, 65);
            this.lblDbPort.Name = "lblDbPort";
            this.lblDbPort.Size = new System.Drawing.Size(74, 12);
            this.lblDbPort.TabIndex = 2;
            this.lblDbPort.Text = "連接埠 (Port):";
            // 
            // txtDbServer
            // 
            this.txtDbServer.Location = new System.Drawing.Point(150, 27);
            this.txtDbServer.Name = "txtDbServer";
            this.txtDbServer.Size = new System.Drawing.Size(320, 22);
            this.txtDbServer.TabIndex = 1;
            // 
            // lblDbServer
            // 
            this.lblDbServer.AutoSize = true;
            this.lblDbServer.Location = new System.Drawing.Point(20, 30);
            this.lblDbServer.Name = "lblDbServer";
            this.lblDbServer.Size = new System.Drawing.Size(85, 12);
            this.lblDbServer.TabIndex = 0;
            this.lblDbServer.Text = "伺服器 (Server):";
            // 
            // grpCurrentConnectionString
            // 
            this.grpCurrentConnectionString.Controls.Add(this.txtDbStatus);
            this.grpCurrentConnectionString.Controls.Add(this.txtDbConnectionString);
            this.grpCurrentConnectionString.Controls.Add(this.lblDbConnectionString);
            this.grpCurrentConnectionString.Location = new System.Drawing.Point(20, 340);
            this.grpCurrentConnectionString.Name = "grpCurrentConnectionString";
            this.grpCurrentConnectionString.Size = new System.Drawing.Size(1030, 370);
            this.grpCurrentConnectionString.TabIndex = 1;
            this.grpCurrentConnectionString.TabStop = false;
            this.grpCurrentConnectionString.Text = "當前連線資訊";
            // 
            // txtDbStatus
            // 
            this.txtDbStatus.Location = new System.Drawing.Point(20, 130);
            this.txtDbStatus.Multiline = true;
            this.txtDbStatus.Name = "txtDbStatus";
            this.txtDbStatus.ReadOnly = true;
            this.txtDbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDbStatus.Size = new System.Drawing.Size(990, 220);
            this.txtDbStatus.TabIndex = 2;
            // 
            // txtDbConnectionString
            // 
            this.txtDbConnectionString.Location = new System.Drawing.Point(20, 50);
            this.txtDbConnectionString.Multiline = true;
            this.txtDbConnectionString.Name = "txtDbConnectionString";
            this.txtDbConnectionString.ReadOnly = true;
            this.txtDbConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDbConnectionString.Size = new System.Drawing.Size(990, 60);
            this.txtDbConnectionString.TabIndex = 1;
            // 
            // lblDbConnectionString
            // 
            this.lblDbConnectionString.AutoSize = true;
            this.lblDbConnectionString.Location = new System.Drawing.Point(20, 30);
            this.lblDbConnectionString.Name = "lblDbConnectionString";
            this.lblDbConnectionString.Size = new System.Drawing.Size(56, 12);
            this.lblDbConnectionString.TabIndex = 0;
            this.lblDbConnectionString.Text = "連線字串:";
            //
            // grpMessageSender
            //
            this.grpMessageSender.Controls.Add(this.txtSimpleResult);
            this.grpMessageSender.Controls.Add(this.btnSimpleClearResult);
            this.grpMessageSender.Controls.Add(this.btnSimpleUseDefaults);
            this.grpMessageSender.Controls.Add(this.btnSimpleSendMessage);
            this.grpMessageSender.Controls.Add(this.chkSimpleInstantMessage);
            this.grpMessageSender.Controls.Add(this.txtSimpleTargetDevice);
            this.grpMessageSender.Controls.Add(this.lblSimpleTargetDevice);
            this.grpMessageSender.Controls.Add(this.txtSimpleMessageText);
            this.grpMessageSender.Controls.Add(this.lblSimpleMessageText);
            this.grpMessageSender.Location = new System.Drawing.Point(540, 20);
            this.grpMessageSender.Name = "grpMessageSender";
            this.grpMessageSender.Size = new System.Drawing.Size(510, 690);
            this.grpMessageSender.TabIndex = 2;
            this.grpMessageSender.TabStop = false;
            this.grpMessageSender.Text = "消息發送測試 (數據庫聯動)";
            //
            // lblSimpleMessageText
            //
            this.lblSimpleMessageText.AutoSize = true;
            this.lblSimpleMessageText.Location = new System.Drawing.Point(20, 30);
            this.lblSimpleMessageText.Name = "lblSimpleMessageText";
            this.lblSimpleMessageText.Size = new System.Drawing.Size(56, 12);
            this.lblSimpleMessageText.TabIndex = 0;
            this.lblSimpleMessageText.Text = "消息內容:";
            //
            // txtSimpleMessageText
            //
            this.txtSimpleMessageText.Location = new System.Drawing.Point(20, 50);
            this.txtSimpleMessageText.Multiline = true;
            this.txtSimpleMessageText.Name = "txtSimpleMessageText";
            this.txtSimpleMessageText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSimpleMessageText.Size = new System.Drawing.Size(470, 60);
            this.txtSimpleMessageText.TabIndex = 1;
            this.txtSimpleMessageText.Text = "UITest 測試消息 - Test Message";
            //
            // lblSimpleTargetDevice
            //
            this.lblSimpleTargetDevice.AutoSize = true;
            this.lblSimpleTargetDevice.Location = new System.Drawing.Point(20, 125);
            this.lblSimpleTargetDevice.Name = "lblSimpleTargetDevice";
            this.lblSimpleTargetDevice.Size = new System.Drawing.Size(245, 12);
            this.lblSimpleTargetDevice.TabIndex = 2;
            this.lblSimpleTargetDevice.Text = "目標設備 (格式: StationID_AreaID_DeviceID):";
            //
            // txtSimpleTargetDevice
            //
            this.txtSimpleTargetDevice.Location = new System.Drawing.Point(20, 145);
            this.txtSimpleTargetDevice.Name = "txtSimpleTargetDevice";
            this.txtSimpleTargetDevice.Size = new System.Drawing.Size(300, 22);
            this.txtSimpleTargetDevice.TabIndex = 3;
            this.txtSimpleTargetDevice.Text = "LG01_CCS_CDU-1";
            //
            // chkSimpleInstantMessage
            //
            this.chkSimpleInstantMessage.AutoSize = true;
            this.chkSimpleInstantMessage.Location = new System.Drawing.Point(20, 185);
            this.chkSimpleInstantMessage.Name = "chkSimpleInstantMessage";
            this.chkSimpleInstantMessage.Size = new System.Drawing.Size(234, 16);
            this.chkSimpleInstantMessage.TabIndex = 4;
            this.chkSimpleInstantMessage.Text = "即時消息 (否則為預錄消息)";
            this.chkSimpleInstantMessage.UseVisualStyleBackColor = true;
            //
            // btnSimpleSendMessage
            //
            this.btnSimpleSendMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimpleSendMessage.Location = new System.Drawing.Point(20, 220);
            this.btnSimpleSendMessage.Name = "btnSimpleSendMessage";
            this.btnSimpleSendMessage.Size = new System.Drawing.Size(130, 40);
            this.btnSimpleSendMessage.TabIndex = 5;
            this.btnSimpleSendMessage.Text = "發送測試消息";
            this.btnSimpleSendMessage.UseVisualStyleBackColor = true;
            this.btnSimpleSendMessage.Click += new System.EventHandler(this.btnSimpleSendMessage_Click);
            //
            // btnSimpleUseDefaults
            //
            this.btnSimpleUseDefaults.Location = new System.Drawing.Point(170, 220);
            this.btnSimpleUseDefaults.Name = "btnSimpleUseDefaults";
            this.btnSimpleUseDefaults.Size = new System.Drawing.Size(110, 40);
            this.btnSimpleUseDefaults.TabIndex = 6;
            this.btnSimpleUseDefaults.Text = "填充默認值";
            this.btnSimpleUseDefaults.UseVisualStyleBackColor = true;
            this.btnSimpleUseDefaults.Click += new System.EventHandler(this.btnSimpleUseDefaults_Click);
            //
            // btnSimpleClearResult
            //
            this.btnSimpleClearResult.Location = new System.Drawing.Point(300, 220);
            this.btnSimpleClearResult.Name = "btnSimpleClearResult";
            this.btnSimpleClearResult.Size = new System.Drawing.Size(90, 40);
            this.btnSimpleClearResult.TabIndex = 7;
            this.btnSimpleClearResult.Text = "清除結果";
            this.btnSimpleClearResult.UseVisualStyleBackColor = true;
            this.btnSimpleClearResult.Click += new System.EventHandler(this.btnSimpleClearResult_Click);
            //
            // txtSimpleResult
            //
            this.txtSimpleResult.Location = new System.Drawing.Point(20, 280);
            this.txtSimpleResult.Multiline = true;
            this.txtSimpleResult.Name = "txtSimpleResult";
            this.txtSimpleResult.ReadOnly = true;
            this.txtSimpleResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSimpleResult.Size = new System.Drawing.Size(470, 390);
            this.txtSimpleResult.TabIndex = 8;
            //
            // grpTableBrowser
            //
            this.grpTableBrowser.Controls.Add(this.lblTableName);
            this.grpTableBrowser.Controls.Add(this.cmbTables);
            this.grpTableBrowser.Controls.Add(this.btnLoadTables);
            this.grpTableBrowser.Controls.Add(this.lblTableFilter);
            this.grpTableBrowser.Controls.Add(this.txtTableFilter);
            this.grpTableBrowser.Controls.Add(this.btnQueryTable);
            this.grpTableBrowser.Controls.Add(this.btnClearTableData);
            this.grpTableBrowser.Controls.Add(this.lblRowCount);
            this.grpTableBrowser.Controls.Add(this.dgvTableData);
            this.grpTableBrowser.Location = new System.Drawing.Point(20, 730);
            this.grpTableBrowser.Name = "grpTableBrowser";
            this.grpTableBrowser.Size = new System.Drawing.Size(1030, 550);
            this.grpTableBrowser.TabIndex = 3;
            this.grpTableBrowser.TabStop = false;
            this.grpTableBrowser.Text = "資料庫表查看器";
            //
            // lblTableName
            //
            this.lblTableName.AutoSize = true;
            this.lblTableName.Location = new System.Drawing.Point(20, 30);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(80, 12);
            this.lblTableName.TabIndex = 0;
            this.lblTableName.Text = "選擇資料表:";
            //
            // cmbTables
            //
            this.cmbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTables.FormattingEnabled = true;
            this.cmbTables.Location = new System.Drawing.Point(110, 27);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(300, 20);
            this.cmbTables.TabIndex = 1;
            //
            // btnLoadTables
            //
            this.btnLoadTables.Location = new System.Drawing.Point(430, 22);
            this.btnLoadTables.Name = "btnLoadTables";
            this.btnLoadTables.Size = new System.Drawing.Size(120, 30);
            this.btnLoadTables.TabIndex = 2;
            this.btnLoadTables.Text = "載入表列表";
            this.btnLoadTables.UseVisualStyleBackColor = true;
            this.btnLoadTables.Click += new System.EventHandler(this.btnLoadTables_Click);
            //
            // lblTableFilter
            //
            this.lblTableFilter.AutoSize = true;
            this.lblTableFilter.Location = new System.Drawing.Point(20, 65);
            this.lblTableFilter.Name = "lblTableFilter";
            this.lblTableFilter.Size = new System.Drawing.Size(320, 12);
            this.lblTableFilter.TabIndex = 3;
            this.lblTableFilter.Text = "篩選條件 (可選，WHERE 後的條件，例如: id > 10 AND name LIKE '%test%'):";
            //
            // txtTableFilter
            //
            this.txtTableFilter.Location = new System.Drawing.Point(20, 85);
            this.txtTableFilter.Multiline = true;
            this.txtTableFilter.Name = "txtTableFilter";
            this.txtTableFilter.Size = new System.Drawing.Size(530, 40);
            this.txtTableFilter.TabIndex = 4;
            //
            // btnQueryTable
            //
            this.btnQueryTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQueryTable.Location = new System.Drawing.Point(570, 85);
            this.btnQueryTable.Name = "btnQueryTable";
            this.btnQueryTable.Size = new System.Drawing.Size(120, 40);
            this.btnQueryTable.TabIndex = 5;
            this.btnQueryTable.Text = "查詢表內容";
            this.btnQueryTable.UseVisualStyleBackColor = true;
            this.btnQueryTable.Click += new System.EventHandler(this.btnQueryTable_Click);
            //
            // btnClearTableData
            //
            this.btnClearTableData.Location = new System.Drawing.Point(710, 85);
            this.btnClearTableData.Name = "btnClearTableData";
            this.btnClearTableData.Size = new System.Drawing.Size(100, 40);
            this.btnClearTableData.TabIndex = 6;
            this.btnClearTableData.Text = "清除資料";
            this.btnClearTableData.UseVisualStyleBackColor = true;
            this.btnClearTableData.Click += new System.EventHandler(this.btnClearTableData_Click);
            //
            // lblRowCount
            //
            this.lblRowCount.AutoSize = true;
            this.lblRowCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRowCount.Location = new System.Drawing.Point(20, 138);
            this.lblRowCount.Name = "lblRowCount";
            this.lblRowCount.Size = new System.Drawing.Size(100, 15);
            this.lblRowCount.TabIndex = 7;
            this.lblRowCount.Text = "資料筆數: 0";
            //
            // dgvTableData
            //
            this.dgvTableData.AllowUserToAddRows = false;
            this.dgvTableData.AllowUserToDeleteRows = false;
            this.dgvTableData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvTableData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTableData.Location = new System.Drawing.Point(20, 160);
            this.dgvTableData.Name = "dgvTableData";
            this.dgvTableData.ReadOnly = true;
            this.dgvTableData.RowTemplate.Height = 24;
            this.dgvTableData.Size = new System.Drawing.Size(990, 370);
            this.dgvTableData.TabIndex = 8;
            //
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 761);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "DCU 封包測試工具";
            this.tabControl1.ResumeLayout(false);
            this.tabPacketBuilder.ResumeLayout(false);
            this.grpPacketOutput.ResumeLayout(false);
            this.grpPacketOutput.PerformLayout();
            this.grpPacketBuilder.ResumeLayout(false);
            this.grpMessageInput.ResumeLayout(false);
            this.grpMessageInput.PerformLayout();
            this.tabPacketValidator.ResumeLayout(false);
            this.grpValidationResult.ResumeLayout(false);
            this.grpValidationResult.PerformLayout();
            this.grpPacketInput.ResumeLayout(false);
            this.grpPacketInput.PerformLayout();
            this.tabTools.ResumeLayout(false);
            this.grpColorConversion.ResumeLayout(false);
            this.grpColorConversion.PerformLayout();
            this.grpDeviceCheck.ResumeLayout(false);
            this.grpDeviceCheck.PerformLayout();
            this.grpFormatConversion.ResumeLayout(false);
            this.grpFormatConversion.PerformLayout();
            this.tabDatabaseSettings.ResumeLayout(false);
            this.grpDatabaseConnection.ResumeLayout(false);
            this.grpDatabaseConnection.PerformLayout();
            this.grpCurrentConnectionString.ResumeLayout(false);
            this.grpCurrentConnectionString.PerformLayout();
            this.grpMessageSender.ResumeLayout(false);
            this.grpMessageSender.PerformLayout();
            this.grpTableBrowser.ResumeLayout(false);
            this.grpTableBrowser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPacketBuilder;
        private System.Windows.Forms.TabPage tabPacketValidator;
        private System.Windows.Forms.TabPage tabTools;
        private System.Windows.Forms.GroupBox grpMessageInput;
        private System.Windows.Forms.TextBox txtMessageInput;
        private System.Windows.Forms.Label lblMessageInput;
        private System.Windows.Forms.GroupBox grpPacketBuilder;
        private System.Windows.Forms.Button btnGeneratePacket;
        private System.Windows.Forms.GroupBox grpPacketOutput;
        private System.Windows.Forms.TextBox txtPacketOutput;
        private System.Windows.Forms.Button btnClearPacketOutput;
        private System.Windows.Forms.GroupBox grpPacketInput;
        private System.Windows.Forms.Label lblPacketType;
        private System.Windows.Forms.ComboBox cmbPacketType;
        private System.Windows.Forms.Label lblSampleDescription;
        private System.Windows.Forms.Label lblPacketInput;
        private System.Windows.Forms.TextBox txtPacketInput;
        private System.Windows.Forms.Button btnLoadSample;
        private System.Windows.Forms.Button btnValidatePacket;
        private System.Windows.Forms.GroupBox grpValidationResult;
        private System.Windows.Forms.Button btnClearResult;
        private System.Windows.Forms.TextBox txtValidationResult;
        private System.Windows.Forms.GroupBox grpFormatConversion;
        private System.Windows.Forms.TextBox txtFormatOutput;
        private System.Windows.Forms.Label lblFormatOutput;
        private System.Windows.Forms.TextBox txtFormatInput;
        private System.Windows.Forms.Label lblFormatInput;
        private System.Windows.Forms.Button btnConvertFormat;
        private System.Windows.Forms.GroupBox grpDeviceCheck;
        private System.Windows.Forms.TextBox txtDeviceOutput;
        private System.Windows.Forms.Label lblDeviceOutput;
        private System.Windows.Forms.TextBox txtDeviceInput;
        private System.Windows.Forms.Label lblDeviceInput;
        private System.Windows.Forms.Button btnCheckDevice;
        private System.Windows.Forms.Button btnLoadDevicesFromDB;
        private System.Windows.Forms.GroupBox grpColorConversion;
        private System.Windows.Forms.TextBox txtColorOutput;
        private System.Windows.Forms.Label lblColorOutput;
        private System.Windows.Forms.TextBox txtColorInput;
        private System.Windows.Forms.Label lblColorInput;
        private System.Windows.Forms.Button btnConvertColor;
        private System.Windows.Forms.TabPage tabDatabaseSettings;
        private System.Windows.Forms.GroupBox grpDatabaseConnection;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnUpdateConnectionString;
        private System.Windows.Forms.TextBox txtDbPassword;
        private System.Windows.Forms.Label lblDbPassword;
        private System.Windows.Forms.TextBox txtDbUserId;
        private System.Windows.Forms.Label lblDbUserId;
        private System.Windows.Forms.TextBox txtDbDatabase;
        private System.Windows.Forms.Label lblDbDatabase;
        private System.Windows.Forms.TextBox txtDbPort;
        private System.Windows.Forms.Label lblDbPort;
        private System.Windows.Forms.TextBox txtDbServer;
        private System.Windows.Forms.Label lblDbServer;
        private System.Windows.Forms.GroupBox grpCurrentConnectionString;
        private System.Windows.Forms.TextBox txtDbStatus;
        private System.Windows.Forms.TextBox txtDbConnectionString;
        private System.Windows.Forms.Label lblDbConnectionString;
        private System.Windows.Forms.GroupBox grpMessageSender;
        private System.Windows.Forms.Label lblSimpleMessageText;
        private System.Windows.Forms.TextBox txtSimpleMessageText;
        private System.Windows.Forms.Label lblSimpleTargetDevice;
        private System.Windows.Forms.TextBox txtSimpleTargetDevice;
        private System.Windows.Forms.CheckBox chkSimpleInstantMessage;
        private System.Windows.Forms.Button btnSimpleSendMessage;
        private System.Windows.Forms.Button btnSimpleUseDefaults;
        private System.Windows.Forms.Button btnSimpleClearResult;
        private System.Windows.Forms.TextBox txtSimpleResult;
        private System.Windows.Forms.GroupBox grpTableBrowser;
        private System.Windows.Forms.Label lblTableName;
        private System.Windows.Forms.ComboBox cmbTables;
        private System.Windows.Forms.Button btnLoadTables;
        private System.Windows.Forms.Label lblTableFilter;
        private System.Windows.Forms.TextBox txtTableFilter;
        private System.Windows.Forms.Button btnQueryTable;
        private System.Windows.Forms.Button btnClearTableData;
        private System.Windows.Forms.Label lblRowCount;
        private System.Windows.Forms.DataGridView dgvTableData;
    }
}

