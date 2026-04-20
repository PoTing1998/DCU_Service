using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UITest.Verify;

namespace UITest
{
    /// <summary>
    /// 封包驗證相關功能 (優化版)
    /// </summary>
    public partial class Form1
    {
        #region Packet Validator - Data Structures

        /// <summary>
        /// 封包類型定義
        /// </summary>
        private class PacketType
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string SamplePacket { get; set; }
            public Func<byte[], (bool result, string message)> Validator { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// 所有封包類型的字典
        /// </summary>
        private Dictionary<string, PacketType> _packetTypes;

        #endregion

        #region Packet Validator - Initialization

        /// <summary>
        /// 初始化封包驗證器
        /// </summary>
        private void InitializePacketValidator()
        {
            _packetTypes = new Dictionary<string, PacketType>
            {
                {
                    "FullWindow",
                    new PacketType
                    {
                        Name = "一般訊息封包 (FullWindow)",
                        Description = "適用於全螢幕文字訊息顯示",
                        SamplePacket = "55 AA 02 11 12 34 19 00 01 15 00 77 7F 22 31 71 0E 00 03 64 07 0A 2A C6 59 11 A6 55 A6 EC 1F 1E 1D 97",
                        Validator = (data) =>
                        {
                            string errorMessage = "";
                            bool result = new FullWindowHandlerVerify().ValidatePacket(data, out errorMessage);
                            return (result, errorMessage);
                        }
                    }
                },
                {
                    "LeftPlatform",
                    new PacketType
                    {
                        Name = "左側月台封包 (LeftPlatform)",
                        Description = "適用於月台左側資訊顯示",
                        SamplePacket = " 55 AA 01 01 34 1F 00 01 1C 00 7F 21 31 7A FF FF 00 01 72 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 30",
                        Validator = (data) =>
                        {
                            string errorMessage = "";
                            bool result = new LeftPlatformHandlerVerify().ValidatePacket(data, out errorMessage);
                            return (result, errorMessage);
                        }
                    }
                },
                {
                    "LeftPlatformRightTime",
                    new PacketType
                    {
                        Name = "左側月台+右側時間封包 (LeftPlatformRightTime)",
                        Description = "適用於月台左側資訊+右側倒數計時顯示",
                        SamplePacket = "55 AA 01 01 34 25 00 01 22 00 7F 21 31 7A 00 00 FF 01 7B FF 00 00 00 00 73 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D B2",
                        Validator = (data) =>
                        {
                            string errorMessage = "";
                            bool result = new LeftPlatformRightTimeHandlerVerify().ValidatePacket(data, out errorMessage);
                            return (result, errorMessage);
                        }
                    }
                },
                {
                    "RightTime",
                    new PacketType
                    {
                        Name = "右側時間封包 (RightTime)",
                        Description = "適用於右側倒數計時顯示",
                        SamplePacket = "55 AA 01 01 34 20 00 01 1D 00 7F 21 31 7B FF FF FF 0C 00 74 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 3E",
                        Validator = (data) =>
                        {
                            string errorMessage = "";
                            bool result = new RightTimeHandlerVerify().ValidatePacket(data, out errorMessage);
                            return (result, errorMessage);
                        }
                    }
                },
                {
                    "TrainDynamic",
                    new PacketType
                    {
                        Name = "列車動態訊息封包 (TrainDynamic)",
                        Description = "適用於列車資訊動態顯示",
                        SamplePacket = "55 AA 01 01 34 3B 00 02 38 00 77 7F 21 31 83 30 00 04 61 07 08 2A FF FF 00 B7 48 A6 77 1F 2D 01 00 01 FF FF 00 1F 2A FF FF 00 A5 5B D3 C2 1F 2D 02 00 01 FF FF 00 1F 2A FF FF 00 A5 BB AF B8 1F 1E 1D CA",
                        Validator = (data) =>
                        {
                            string errorMessage = "";
                            bool result = new TrainDynamicVerify().ValidatePacket(data, out errorMessage);
                            return (result, errorMessage);
                        }
                    }
                },
                {
                    "Urgent",
                    new PacketType
                    {
                        Name = "緊急訊息封包 (Emergency)",
                        Description = "適用於緊急資訊顯示（火災、疏散等）",
                        SamplePacket = "55 AA 01 01 38 20 00 01 01 1C 00 77 79 02 80 FF 7F 21 32 71 10 00 01 64 07 0A 2A FF 00 00 B8 55 A4 6A BD 75 1F 1E 1D 28",
                        Validator = (data) =>
                        {
                            string errorMessage = "";
                            bool result = new UrgentHandlerVerify().ValidatePacket(data, out errorMessage);
                            return (result, errorMessage);
                        }
                    }
                },
                {
                    "StandardTimeBottomLeft",
                    new PacketType
                    {
                        Name = "左上標準時間封包 (StandardTime)",
                        Description = "顯示在上排左側 24(h)x48(w) 時間資訊",
                        SamplePacket = "55 AA 01 01 34 27 00 01 24 00 7F 21 31 7D 31 00 00 FF 01 31 7B FF 00 00 0C 00 74 10 00 04 64 07 0A 2A FF FF FF B8 55 A4 6A BD 75 1F 1E 1D 26",
                        Validator = (data) =>
                        {
                            string errorMessage = "";
                            bool result = new StandardTimeBottomLeftHandlerVerify().ValidatePacket(data, out errorMessage);
                            return (result, errorMessage);
                        }
                    }
                },
                {
                    "IdentifierStatusImage",
                    new PacketType
                    {
                        Name = "左下識別狀態圖像封包 (StatusImage)",
                        Description = "顯示在下排左側 24(h)x48(w) 識別狀態圖",
                        SamplePacket = "55 AA 01 01 34 26 00 02 23 00 7F 21 31 7E 31 00 FF 00 31 7B 00 00 FF 0C 00 74 10 00 04 64 07 0A 2A FF 00 00 B8 55 A4 6A BD 75 1F 1E 1D 28",
                        Validator = (data) =>
                        {
                            string errorMessage = "";
                            bool result = new IdentifierStatusImageTopLeft24x48HandlerVerify().ValidatePacket(data, out errorMessage);
                            return (result, errorMessage);
                        }
                    }
                }
            };

            // 填充下拉選單
            cmbPacketType.Items.Clear();
            foreach (var packetType in _packetTypes.Values)
            {
                cmbPacketType.Items.Add(packetType);
            }

            if (cmbPacketType.Items.Count > 0)
            {
                cmbPacketType.SelectedIndex = 0;
            }
        }

        #endregion

        #region Packet Validator - Event Handlers

        /// <summary>
        /// 封包類型選擇變更事件
        /// </summary>
        private void cmbPacketType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPacketType.SelectedItem is PacketType packetType)
            {
                lblSampleDescription.Text = packetType.Description;
            }
        }

        /// <summary>
        /// 載入範例封包
        /// </summary>
        private void btnLoadSample_Click(object sender, EventArgs e)
        {
            if (cmbPacketType.SelectedItem is PacketType packetType)
            {
                txtPacketInput.Text = packetType.SamplePacket;
                MessageBox.Show("範例封包已載入", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("請先選擇封包類型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 驗證封包
        /// </summary>
        private void btnValidatePacket_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPacketType.SelectedItem is PacketType packetType)
                {
                    string hexString = txtPacketInput.Text;

                    if (string.IsNullOrWhiteSpace(hexString))
                    {
                        MessageBox.Show("請輸入封包內容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 轉換為字節陣列
                    byte[] packetData = ConvertHexStringToByteArray(hexString);

                    // 執行驗證
                    var (result, message) = packetType.Validator(packetData);

                    // 顯示結果
                    DisplayValidationResult(result, message, packetData, packetType.Name);
                }
                else
                {
                    MessageBox.Show("請先選擇封包類型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                txtValidationResult.ForeColor = System.Drawing.Color.Red;
                txtValidationResult.Text = $"驗證過程發生錯誤:\r\n\r\n{ex.Message}\r\n\r\n請檢查封包格式是否正確 (僅支援十六進制字元 0-9, A-F)";
            }
        }

        /// <summary>
        /// 清除驗證結果
        /// </summary>
        private void ClearBT_Click(object sender, EventArgs e)
        {
            txtValidationResult.Text = string.Empty;
            txtValidationResult.ForeColor = System.Drawing.Color.Black;
        }

        #endregion

        #region Packet Validator - Helper Methods

        /// <summary>
        /// 將十六進制字串轉換為字節陣列
        /// </summary>
        private byte[] ConvertHexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "").Replace("-", "").Replace("\r", "").Replace("\n", "");

            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("十六進制字串長度必須是偶數");
            }

            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// 顯示驗證結果 (增強版)
        /// </summary>
        private void DisplayValidationResult(bool result, string errorMessage, byte[] packetData, string packetTypeName)
        {
            StringBuilder sb = new StringBuilder();

            // 標題和驗證結果
            sb.AppendLine("==================== 封包驗證結果 ====================");
            sb.AppendLine();
            sb.AppendLine($"封包類型: {packetTypeName}");
            sb.AppendLine($"驗證時間: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"封包長度: {packetData.Length} bytes");
            sb.AppendLine();

            if (result)
            {
                sb.AppendLine("驗證狀態: ✓ 封包格式正確");
                txtValidationResult.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                sb.AppendLine("驗證狀態: ✗ 封包格式錯誤");
                txtValidationResult.ForeColor = System.Drawing.Color.Red;
                sb.AppendLine();
                sb.AppendLine("錯誤訊息:");
                sb.AppendLine(errorMessage);
            }

            // 顯示封包內容 (十六進制)
            sb.AppendLine();
            sb.AppendLine("==================== 封包內容 (HEX) ====================");
            sb.AppendLine();
            sb.AppendLine(FormatHexDisplay(packetData));

            // 顯示封包內容 (十進制)
            sb.AppendLine();
            sb.AppendLine("==================== 封包內容 (DEC) ====================");
            sb.AppendLine();
            sb.AppendLine(FormatDecDisplay(packetData));

            // 基本結構解析
            sb.AppendLine();
            sb.AppendLine("==================== 基本結構解析 ====================");
            sb.AppendLine();
            if (packetData.Length >= 2)
            {
                sb.AppendLine($"StartCode:    0x{packetData[0]:X2} {packetData[1]:X2}");
            }
            if (packetData.Length >= 3)
            {
                sb.AppendLine($"ID_LENGTH:    {packetData[2]}");
            }
            if (packetData.Length >= 4 && packetData[2] > 0)
            {
                sb.Append("IDs:          ");
                for (int i = 3; i < Math.Min(3 + packetData[2], packetData.Length); i++)
                {
                    sb.Append($"0x{packetData[i]:X2} ");
                }
                sb.AppendLine();
            }
            if (packetData.Length >= 4 + packetData[2])
            {
                sb.AppendLine($"FunctionCode: 0x{packetData[3 + packetData[2]]:X2}");
            }

            txtValidationResult.Text = sb.ToString();
        }

        /// <summary>
        /// 格式化十六進制顯示
        /// </summary>
        private string FormatHexDisplay(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0 && i % 16 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append($"{data[i]:X2} ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化十進制顯示
        /// </summary>
        private string FormatDecDisplay(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0 && i % 16 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append($"{data[i],3} ");
            }
            return sb.ToString();
        }

        #endregion
    }
}
