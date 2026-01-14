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
        // Form1.PacketBuilder.cs:
        //   - button1_Click() - 建立 StringBody
        //   - button2_Click_1() - 建立 StringMessage
        //   - button3_Click() - 建立 FullWindow
        //   - button4_Click() - 建立 Sequence
        //   - button5_Click() - 建立 Packet
        //   - btnClearPacketOutput_Click() - 清除輸出
        //   - ExtractValue(), ValidateStringText(), ValidateStringMessage()
        //   - ValidateStringBody(), ValidateMessageScroll(), ValidateMessageLevel()
        //   - ValidateMessageFont()
        //
        // Form1.PacketValidator.cs:
        //   - Version1BT_Click() 到 Version8BT_Click() - 8個版本的封包驗證
        //   - ClearBT_Click() - 清除驗證結果
        //   - ConvertHexStringToByteArray()
        //   - UpdateValidationResult()
    }
}
