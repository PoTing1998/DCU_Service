using ASI.Wanda.DCU.TaskCDU;
using Display;
using Display.DisplayMode;
using Display.Function;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UITest.Verify;
using static Display.DisplaySettingsEnums;
using Npgsql;

namespace UITest
{
    /// <summary>
    /// DCU 封包測試工具主表單
    /// 功能已拆分為多個 Partial Class：
    /// - Form1.Database.cs: 資料庫設定
    /// - Form1.Tools.cs: 工具功能
    /// - Form1.PacketBuilder.cs: 封包組成測試
    /// - Form1.PacketValidator.cs: 封包驗證
    /// </summary>
    public partial class Form1 : Form
    {
        #region Fields

        private string _mProcName;
        private string StationID;
        private string AreaID;
        private string DeviceID;

        // 封包組成測試所需的中間變數
        private TextStringBody textStringBody;
        private StringMessage stringMessage;
        private FullWindow fullWindow;
        private Sequence sequence;

        #endregion

        #region Constructor

        public Form1()
        {
            InitializeComponent();
            Console.WriteLine();
            LoadDatabaseSettings(); // 定義在 Form1.Database.cs
            InitializePacketValidator(); // 定義在 Form1.PacketValidator.cs
            InitializePacketBuilder(); // 定義在 Form1.PacketBuilder.cs
        }

        #endregion

        // 注意：所有事件處理方法和業務邏輯已移至以下 Partial Class 檔案：
        //
        // Form1.Database.cs:
        //   - LoadDatabaseSettings()
        //   - UpdateConnectionStringDisplay()
        //   - btnUpdateConnectionString_Click()
        //   - btnTestConnection_Click()
        //
        // Form1.Tools.cs:
        //   - button6_Click() - 格式轉換
        //   - button7_Click() - 檢查設備
        //   - button8_Click() - 色碼轉換
        //   - btnLoadDevicesFromDB_Click() - 從資料庫載入設備
        //   - ProcessMessageColor()
        //
        // Form1.PacketBuilder.cs: (優化版)
        //   - InitializePacketBuilder() - 初始化並載入預設範例
        //   - GenerateFullPacketOneClick() - 一鍵生成完整封包
        //   - CreateStringBodyInternal() - 內部建立 StringBody
        //   - CreateStringMessageInternal() - 內部建立 StringMessage
        //   - CreateFullWindowInternal() - 內部建立 FullWindow
        //   - CreateSequenceInternal() - 內部建立 Sequence
        //   - CreatePacketInternal() - 內部建立 Packet
        //   - button1_Click() ~ button5_Click() - 分步建立（優化版）
        //   - btnClearPacketOutput_Click() - 清除並重置
        //   - ExtractValue() - 改進的值提取（支持多種格式）
        //   - FormatHexOutput(), FormatPacketOutput() - 格式化輸出
        //   - Validate*() - 各種驗證方法
        //
        // Form1.PacketValidator.cs:
        //   - InitializePacketValidator() - 初始化封包驗證器
        //   - cmbPacketType_SelectedIndexChanged() - 封包類型選擇變更
        //   - btnLoadSample_Click() - 載入範例封包
        //   - btnValidatePacket_Click() - 驗證封包
        //   - ClearBT_Click() - 清除驗證結果
        //   - ConvertHexStringToByteArray() - 十六進制轉換
        //   - DisplayValidationResult() - 顯示驗證結果
    }
}
