using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Display.DisplaySettingsEnums;

namespace UITest
{
    #region interface
    public interface IMessageTypeHandler
    {
        bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage);
    }
    #endregion
    #region    2. 為每種 messageType 實作不同的處理類別
    public class FullWindowHandler : IMessageTypeHandler //一般訊息
    {
        public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // FullWindow messageType 沒有額外參數，只需增加索引即可
            currentIndex++;
            return true;
        }
    }
    
    public class LeftPlatformHandler : IMessageTypeHandler// 左側月台圖片的 
    {
        public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // LeftPlatform 需要處理四個額外參數
            if (currentIndex + 4 > receivedData.Length)
            {
                errorMessage = "Insufficient data for LeftPlatform parameters.";
                return false;
            }
            // 模擬檢查這四個參數 (這裡可以根據實際需求擴充)
            // 提取並設置 RGB 顏色參數和 PhotoIndex
            currentIndex-=4;
            var RedColor = receivedData[currentIndex];
            var GreenColor = receivedData[currentIndex + 1];
            var BlueColor = receivedData[currentIndex + 2];
            var PhotoIndex = receivedData[currentIndex + 3];

            // 檢查 RGB 範圍，RGB 值應該在 0 到 255 之間
            if (RedColor < 0 || RedColor > 255 || GreenColor < 0 || GreenColor > 255 || BlueColor < 0 || BlueColor > 255)
            {
                errorMessage = $"Invalid RGB values: Red={RedColor}, Green={GreenColor}, Blue={BlueColor}. Each should be between 0 and 255.";
                return false;
            }
            // 檢查 Index的範圍不超過 1-12
            if (PhotoIndex < 0 || PhotoIndex > 13)
            {
                errorMessage = $"Invalid PhotoIndex values: Red={PhotoIndex}. Each should be between 1 and  12.";
                return false;
            }
          
            currentIndex += 5;
            return true;
        }
    }
    public class LeftPlatformRightTimeHandler : IMessageTypeHandler //左側月台碼 加上右側時間
    {
        public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // LeftPlatformRightTime 需要處理九個額外參數
            if (currentIndex + 11 > receivedData.Length)
            {
                errorMessage = "Insufficient data for LeftPlatform parameters.";
                return false;
            }
            currentIndex -= 10;
            var RedColor = receivedData[currentIndex];
            var GreenColor = receivedData[currentIndex + 1];
            var BlueColor = receivedData[currentIndex + 2];
            var PhotoIndex = receivedData[currentIndex + 3];
            var TimeRedColor = receivedData[currentIndex + 4];
            var TimeGreenColor = receivedData[currentIndex + 6];
            var TimeBlueColor = receivedData[currentIndex + 7];
            var StartValue = receivedData[currentIndex + 8];
            var EndValue = receivedData[currentIndex + 9];

            // 檢查 RGB 範圍，RGB 值應該在 0 到 255 之間
            if (RedColor < 0 || RedColor > 255 || GreenColor < 0 || GreenColor > 255 || BlueColor < 0 || BlueColor > 255)
            {
                errorMessage = $"Invalid RGB values: Red={RedColor}, Green={GreenColor}, Blue={BlueColor}. Each should be between 0 and 255.";
                return false;
            }
            // 檢查 Index的範圍不超過 1-12
            if (PhotoIndex < 0 || PhotoIndex > 13)
            {
                errorMessage = $"Invalid PhotoIndex values: Red={PhotoIndex}. Each should be between 1 and  12.";
                return false;
            }
            // 檢查 RGB 範圍，RGB 值應該在 0 到 255 之間
            if (TimeRedColor < 0 || TimeRedColor > 255 || TimeGreenColor < 0 || TimeGreenColor > 255 || TimeBlueColor < 0 || TimeBlueColor > 255)
            {
                errorMessage = $"Invalid RGB values: Red={TimeRedColor}, Green={TimeGreenColor}, Blue={TimeBlueColor}. Each should be between 0 and 255.";
                return false;
            }

            // 檢查 StartValue 和 EndValue 的條件
            if (StartValue == 0x00)
            {
                // StartValue 為 0x00 時，表示顯示標準時間，EndValue 可以忽略
                // 不做進一步檢查
            }
            else if (StartValue >= 0x01 && StartValue <= 0xFF)
            {
                // StartValue 在 0x01 到 0xFF 範圍內，表示開始倒數，檢查 EndValue 的邏輯
                if (EndValue < 0x00 || EndValue > 0xFE)
                {
                    errorMessage = $"Invalid EndValue: {EndValue}. Should be between 0x00 and 0xFE.";
                    return false;
                }

                if (StartValue <= EndValue)
                {
                    errorMessage = $"StartValue ({StartValue:X2}) should be greater than EndValue ({EndValue:X2}).";
                    return false;
                }
            }
            else
            {
                errorMessage = $"Invalid StartValue: {StartValue:X2}.";
                return false;
            }


            currentIndex += 11;
            return true;
        }
    }

    public class rightTimeHandler : IMessageTypeHandler //右側時間
    {
        public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // LeftPlatformRightTime 需要處理九個額外參數 
            if (currentIndex + 6 > receivedData.Length)
            {
                errorMessage = "Insufficient data for LeftPlatform parameters.";
                return false;
            }
            currentIndex -= 5;
            // 模擬檢查這六個參數 (這裡可以根據實際需求擴充)
            var RedColor = receivedData[currentIndex];
            var GreenColor = receivedData[currentIndex + 1];
            var BlueColor = receivedData[currentIndex + 2];
            var StartValue = receivedData[currentIndex + 3];
            var EndValue = receivedData[currentIndex + 4];
            // 檢查 RGB 範圍，RGB 值應該在 0 到 255 之間
            if (RedColor < 0 || RedColor > 255 || GreenColor < 0 || GreenColor > 255 || BlueColor < 0 || BlueColor > 255)
            {
                errorMessage = $"Invalid RGB values: Red={RedColor}, Green={GreenColor}, Blue={BlueColor}. Each should be between 0 and 255.";
                return false;
            }
            // 檢查 StartValue 和 EndValue 的條件
            if (StartValue == 0x00)
            {
                // StartValue 為 0x00 時，表示顯示標準時間，EndValue 可以忽略
                // 不做進一步檢查
            }
            else if (StartValue >= 0x01 && StartValue <= 0xFF)
            {
                // StartValue 在 0x01 到 0xFF 範圍內，表示開始倒數，檢查 EndValue 的邏輯
                if (EndValue < 0x00 || EndValue > 0xFE)
                {
                    errorMessage = $"Invalid EndValue: {EndValue}. Should be between 0x00 and 0xFE.";
                    return false;
                }

                if (StartValue <= EndValue)
                {
                    errorMessage = $"StartValue ({StartValue:X2}) should be greater than EndValue ({EndValue:X2}).";
                    return false;
                }
            }
            else
            {
                errorMessage = $"Invalid StartValue: {StartValue:X2}.";
                return false;
            }
            currentIndex += 6;
            return true;
        }
    }

    public class TrainDynamicHandler : IMessageTypeHandler //列車動態
    {
        public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            //  TrainDynamic messageType 沒有額外參數，只需增加索引即可 
            currentIndex++; 
            return true;
        }
    }
    #endregion
    #region  3. 判斷版型的類型 
    public class MessageTypeHandlerFactory
    {
        public static IMessageTypeHandler GetHandler(WindowDisplayMode messageType)
        {
            switch (messageType)  
            {
                case WindowDisplayMode.FullWindow:
                    return new FullWindowHandler();
                case WindowDisplayMode.LeftPlatform:
                    return new LeftPlatformHandler();
                case WindowDisplayMode.LeftPlatformRightTime:
                    return new LeftPlatformRightTimeHandler();
                case WindowDisplayMode.RightTime:
                    return new rightTimeHandler();
                case WindowDisplayMode.TrainDynamic:
                    return new TrainDynamicHandler(); 
                default:
                    throw new ArgumentException($"No handler found for messageType: {messageType}");
            }
        }
    }
    #endregion
}
