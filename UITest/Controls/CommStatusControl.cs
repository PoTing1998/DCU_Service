using System;
using System.Windows.Forms;

namespace UITest.Controls
{
    public partial class CommStatusControl : UserControl
    {
        // 欄位名稱
        private static readonly string[] ColHeaders = new[]
        {
            "設備ID", "通訊狀態", "MCU版本", "FPGA版本",
            "月台路線16x16版本", "月台路線24x24版本", "預錄訊息版本",
            "靜動態16x16圖片版本", "靜動態24x24圖片版本"
        };
        private static readonly int[] ColWidths = new[]
        {
            100, 75, 75, 75, 120, 120, 90, 120, 120
        };

        // 裝置列表（與截圖相同順序）
        private static readonly string[] DeviceRows = new[]
        {
            "上行月台ID11", "上行月台ID12", "上行月台ID13", "上行月台ID14",
            "下行月台ID15", "下行月台ID16", "下行月台ID17", "下行月台ID18",
            "大廳ID1", "大廳ID2", "大廳ID3", "大廳ID4",
            "交會站ID5", "交會站ID6", "交會站ID7"
        };

        // 供下拉選單的選項（依區段）
        private static readonly string[] DeviceOptions = new[]
        {
            "大廳ID1", "大廳ID2", "大廳ID3", "大廳ID4",
            "交會站ID5", "交會站ID6", "交會站ID7",
            "上行月台ID11", "上行月台ID12", "上行月台ID13", "上行月台ID14",
            "下行月台ID15", "下行月台ID16", "下行月台ID17", "下行月台ID18"
        };

        public CommStatusControl()
        {
            InitializeComponent();
            InitComboBox();
            InitGrid();
        }

        private void InitComboBox()
        {
            foreach (var d in DeviceOptions)
                cmbDevice.Items.Add(d);
            if (cmbDevice.Items.Count > 0)
                cmbDevice.SelectedIndex = 0;
        }

        private void InitGrid()
        {
            // 建立欄位
            for (int i = 0; i < ColHeaders.Length; i++)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    HeaderText = ColHeaders[i],
                    Width      = ColWidths[i],
                    ReadOnly   = true
                };
                dgv.Columns.Add(col);
            }

            // 填入裝置列（設備ID 欄，其餘留空）
            foreach (var device in DeviceRows)
            {
                var row = new object[ColHeaders.Length];
                row[0] = device;  // 設備ID
                dgv.Rows.Add(row);
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            // TODO: 向選定裝置發送查詢指令並填入版本資訊
            string selected = cmbDevice.SelectedItem?.ToString() ?? "";
            MessageBox.Show($"查詢裝置：{selected}\n（功能待接入通訊模組）",
                "取得版本資訊", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>由外部更新特定裝置的欄位資料</summary>
        public void UpdateRow(string deviceId, string commStatus, string mcu, string fpga,
            string route16, string route24, string preRec, string static16, string static24)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[0].Value?.ToString() == deviceId)
                {
                    row.Cells[1].Value = commStatus;
                    row.Cells[2].Value = mcu;
                    row.Cells[3].Value = fpga;
                    row.Cells[4].Value = route16;
                    row.Cells[5].Value = route24;
                    row.Cells[6].Value = preRec;
                    row.Cells[7].Value = static16;
                    row.Cells[8].Value = static24;
                    break;
                }
            }
        }
    }
}
