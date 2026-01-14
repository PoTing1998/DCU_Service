using Display;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace UITest
{
    /// <summary>
    /// 工具頁面相關功能
    /// </summary>
    public partial class Form1
    {
        #region Tools - Format Conversion

        /// <summary>
        /// 格式轉換：將十六進制字串每兩個字元添加空格
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            var s = txtFormatInput.Text;
            StringBuilder formattedString = new StringBuilder();

            for (int i = 0; i < s.Length; i += 2)
            {
                if (i + 2 <= s.Length)
                {
                    formattedString.Append(s.Substring(i, 2));
                }
                else
                {
                    formattedString.Append(s.Substring(i, 1));
                }

                if (i + 2 < s.Length)
                {
                    formattedString.Append(" ");
                }
            }

            txtFormatOutput.Text = formattedString.ToString();
        }

        #endregion

        #region Tools - Device Check

        private const string Pattern = @"UPF_PDU";

        /// <summary>
        /// 檢查設備：篩選符合特定模式的設備
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeviceInput.Text))
            {
                txtDeviceOutput.Text = "錯誤: 請輸入設備清單";
                return;
            }

            string[] deviceStrings = txtDeviceInput.Text.Split(',');
            List<string> matchedDevices = new List<string>();

            foreach (var deviceString in deviceStrings)
            {
                string trimmedDevice = deviceString.Trim();
                if (!string.IsNullOrEmpty(trimmedDevice) && Regex.IsMatch(trimmedDevice, Pattern, RegexOptions.IgnoreCase))
                {
                    matchedDevices.Add(trimmedDevice);
                }
            }

            if (matchedDevices.Count > 0)
            {
                txtDeviceOutput.Text = $"找到 {matchedDevices.Count} 個符合模式 '{Pattern}' 的設備:\r\n\r\n";
                txtDeviceOutput.Text += string.Join("\r\n", matchedDevices);
            }
            else
            {
                txtDeviceOutput.Text = $"未找到符合模式 '{Pattern}' 的設備";
            }
        }

        /// <summary>
        /// 從資料庫載入設備清單
        /// </summary>
        private void btnLoadDevicesFromDB_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ASI.Wanda.DMD.DB.Manager.ConnectionString;

                txtDeviceOutput.Text = $"連線字串: {connectionString}\r\n";
                txtDeviceOutput.Text += "正在連接資料庫...\r\n";
                Application.DoEvents();

                List<string> devices = new List<string>();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    txtDeviceOutput.Text += "資料庫連接成功!\r\n";
                    txtDeviceOutput.Text += "正在查詢設備清單...\r\n";
                    Application.DoEvents();

                    string query = @"
                        SELECT station_id, area_id, device_id, device_name, device_status
                        FROM dbo.dmd_target
                        ORDER BY station_id, area_id, device_id";

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string stationId = reader.GetString(0);
                            string areaId = reader.GetString(1);
                            string deviceId = reader.GetString(2);

                            string fullDeviceId = $"{stationId}_{areaId}_{deviceId}";
                            devices.Add(fullDeviceId);
                        }
                    }
                }

                if (devices.Count > 0)
                {
                    txtDeviceInput.Text = string.Join(", ", devices);
                    txtDeviceOutput.Text += $"\r\n✓ 成功從資料庫載入 {devices.Count} 個設備";
                }
                else
                {
                    txtDeviceOutput.Text += "\r\n⚠ 資料庫中沒有找到設備資料";
                }
            }
            catch (Exception ex)
            {
                txtDeviceOutput.Text += $"\r\n✗ 從資料庫載入設備失敗:\r\n{ex.Message}\r\n\r\n";
                txtDeviceOutput.Text += $"例外類型: {ex.GetType().Name}\r\n";
                if (ex.InnerException != null)
                {
                    txtDeviceOutput.Text += $"內部例外: {ex.InnerException.Message}";
                }
            }
        }

        #endregion

        #region Tools - Color Conversion

        /// <summary>
        /// 色碼轉換
        /// </summary>
        private void button8_Click(object sender, EventArgs e)
        {
            var colorName = txtColorInput.Text;

            var colorBytes = ProcessMessageColor(colorName);

            if (colorBytes == null || colorBytes.Length != 3)
            {
                txtColorOutput.Text = "轉換失敗或顏色不存在";
                return;
            }

            var colorHex = ColorHelper.GetColorHex(colorName);

            txtColorOutput.Text = $"顏色名稱: {colorName}\r\n";
            txtColorOutput.Text += $"十六進制色碼: {colorHex}\r\n";
            txtColorOutput.Text += $"RGB 值: R:{colorBytes[0]}, G:{colorBytes[1]}, B:{colorBytes[2]}\r\n";
        }

        /// <summary>
        /// 處理訊息顏色，轉換為RGB字節陣列
        /// </summary>
        private byte[] ProcessMessageColor(string colorName)
        {
            try
            {
                return ColorHelper.GetColorBytes(colorName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
