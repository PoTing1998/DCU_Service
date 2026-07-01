using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    public partial class CommStatusControl : UserControl
    {
        /// <summary>由 Form1 注入，實際傳送 byte[]</summary>
        public Func<byte[], bool> SendAction { get; set; }

        /// <summary>裝置名稱 → 設備 ID byte</summary>
        private static readonly Dictionary<string, byte> DeviceIdMap = new Dictionary<string, byte>
        {
            { "大廳ID1",      0x01 }, { "大廳ID2",      0x02 },
            { "大廳ID3",      0x03 }, { "大廳ID4",      0x04 },
            { "交會站ID5",    0x05 }, { "交會站ID6",    0x06 }, { "交會站ID7",    0x07 },
            { "上行月台ID11", 0x11 }, { "上行月台ID12", 0x12 },
            { "上行月台ID13", 0x13 }, { "上行月台ID14", 0x14 },
            { "下行月台ID15", 0x15 }, { "下行月台ID16", 0x16 },
            { "下行月台ID17", 0x17 }, { "下行月台ID18", 0x18 },
        };
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
            string selected = cmbDevice.SelectedItem?.ToString() ?? "";
            var mon = ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "CommStatus", "傳送失敗：未設定 SendAction");
                MessageBox.Show("尚未連接串列埠。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DeviceIdMap.TryGetValue(selected, out byte deviceId))
            {
                MessageBox.Show($"無法對應裝置ID：{selected}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] packet = BuildVersionQueryPacket(deviceId);
            mon.Log(ConnectionMonitor.LogLevel.Send, "CommStatus",
                $"版本查詢 → {selected}(0x{deviceId:X2})  HEX: {PacketBuilderService.ToHex(packet)}");

            bool ok = SendAction(packet);
            if (!ok)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "CommStatus", "傳送失敗");
                MessageBox.Show("傳送失敗：串列埠未開啟。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 版本查詢封包：功能碼 0x37（設備通訊更新）
        /// TODO: 依協定規格確認正確功能碼
        /// data = 0x00（讀取/查詢命令）
        /// </summary>
        private static byte[] BuildVersionQueryPacket(byte deviceId)
        {
            const byte funcCode = 0x37; // TODO: 確認版本查詢功能碼
            byte[] data = new byte[] { 0x00 };
            byte[] len  = BitConverter.GetBytes((ushort)data.Length);
            byte   cs   = 0x00; // checksum = sum of data bytes

            var result = new List<byte>();
            result.AddRange(new byte[] { 0x55, 0xAA });
            result.Add(0x01);       // ID_Len = 1（單一裝置）
            result.Add(deviceId);   // 目標裝置 ID
            result.Add(funcCode);
            result.AddRange(len);
            result.AddRange(data);
            result.Add(cs);
            return result.ToArray();
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
