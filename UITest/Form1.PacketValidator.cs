using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UITest.Verify;

namespace UITest
{
    /// <summary>
    /// 封包驗證相關功能（8種版本）
    /// </summary>
    public partial class Form1
    {
        #region Packet Validator - Button Click Events

        /// <summary>
        /// 版本1驗證：一般訊息封包
        /// </summary>
        private void Version1BT_Click(object sender, EventArgs e)
        {
            var ByteData = txtVersion1Input.Text;
            var receivedData = ConvertHexStringToByteArray(ByteData);
            string errorMessage = "";
            bool result = new FullWindowHandlerVerify().ValidatePacket(receivedData, out errorMessage);
            UpdateValidationResult(result, errorMessage);
        }

        /// <summary>
        /// 版本2驗證：左側月台 右側時間封包
        /// </summary>
        private void Version2BT_Click(object sender, EventArgs e)
        {
            var ByteData = txtVersion2Input.Text;
            var receivedData = ConvertHexStringToByteArray(ByteData);
            string errorMessage = "";
            bool result = new LeftPlatformRightTimeHandlerVerify().ValidatePacket(receivedData, out errorMessage);
            UpdateValidationResult(result, errorMessage);
        }

        /// <summary>
        /// 版本3驗證：左側月台封包
        /// </summary>
        private void Version3BT_Click(object sender, EventArgs e)
        {
            var ByteData = txtVersion3Input.Text;
            var receivedData = ConvertHexStringToByteArray(ByteData);
            string errorMessage = "";
            bool result = new LeftPlatformHandlerVerify().ValidatePacket(receivedData, out errorMessage);
            UpdateValidationResult(result, errorMessage);
        }

        /// <summary>
        /// 版本4驗證：右側時間封包
        /// </summary>
        private void Version4BT_Click(object sender, EventArgs e)
        {
            var ByteData = txtVersion4Input.Text;
            var receivedData = ConvertHexStringToByteArray(ByteData);
            string errorMessage = "";
            bool result = new RightTimeHandlerVerify().ValidatePacket(receivedData, out errorMessage);
            UpdateValidationResult(result, errorMessage);
        }

        /// <summary>
        /// 版本5驗證：列車動態訊息封包
        /// </summary>
        private void Version5BT_Click(object sender, EventArgs e)
        {
            var ByteData = txtVersion5Input.Text;
            var receivedData = ConvertHexStringToByteArray(ByteData);
            string errorMessage = "";
            bool result = new TrainDynamicVerify().ValidatePacket(receivedData, out errorMessage);
            UpdateValidationResult(result, errorMessage);
        }

        /// <summary>
        /// 版本6驗證：緊急訊息封包
        /// </summary>
        private void Version6BT_Click(object sender, EventArgs e)
        {
            var ByteData = txtVersion6Input.Text;
            var receivedData = ConvertHexStringToByteArray(ByteData);
            string errorMessage = "";
            bool result = new UrgentHandlerVerify().ValidatePacket(receivedData, out errorMessage);
            UpdateValidationResult(result, errorMessage);
        }

        /// <summary>
        /// 版本7驗證：左下標準時間封包
        /// </summary>
        private void Version7BT_Click(object sender, EventArgs e)
        {
            var ByteData = txtVersion7Input.Text;
            var receivedData = ConvertHexStringToByteArray(ByteData);
            string errorMessage = "";
            bool result = new StandardTimeBottomLeftHandlerVerify().ValidatePacket(receivedData, out errorMessage);
            UpdateValidationResult(result, errorMessage);
        }

        /// <summary>
        /// 版本8驗證：左上識別狀態圖像封包
        /// </summary>
        private void Version8BT_Click(object sender, EventArgs e)
        {
            var ByteData = txtVersion8Input.Text;
            var receivedData = ConvertHexStringToByteArray(ByteData);
            string errorMessage = "";
            bool result = new IdentifierStatusImageTopLeft24x48HandlerVerify().ValidatePacket(receivedData, out errorMessage);
            UpdateValidationResult(result, errorMessage);
        }

        /// <summary>
        /// 清除驗證結果
        /// </summary>
        private void ClearBT_Click(object sender, EventArgs e)
        {
            txtValidationResult.Text = string.Empty;
        }

        #endregion

        #region Packet Validator - Helper Methods

        /// <summary>
        /// 將十六進制字串轉換為字節陣列
        /// </summary>
        private byte[] ConvertHexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "").Replace("-", "");

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
        /// 更新驗證結果顯示
        /// </summary>
        private void UpdateValidationResult(bool result, string errorMessage)
        {
            if (result == false)
            {
                txtValidationResult.Text = "錯誤封包\n" + errorMessage;
            }
            else
            {
                txtValidationResult.Text = "正確封包";
            }
        }

        #endregion
    }
}
