using Display;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Display.DisplaySettingsEnums;

namespace UITest
{
    public class TestFunction
    {
        public bool ValidatePacket(byte[] receivedData, out string errorMessage)
        {
            int currentIndex = 0;
            errorMessage = "";

            try
            {
                // 檢查起始碼 [0x55, 0xAA]
                if (!CheckStartCode(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 ID_LENGTH 並跳過 ID 區段
                if (!CheckIdLength(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 FunctionCode 是否為 0x34
                if (!CheckFunctionCode(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查並解析 Data_Length (2 bytes, 小端序)
                if (!CheckDataLength(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查下一個 byte 是否為 0x01
                if (!CheckNextByte(receivedData, ref currentIndex, 0x01, out errorMessage)) return false;
                // 檢查並解析 SequenceLength (2 bytes, 小端序)
                if (!CheckSequenceLength(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 SequenceLength 到 0x1D 的範圍
                if (!CheckSequenceEnd(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查是否存在清除指令 0x77 和 0x7F
                if (!CheckClearCommand(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查字體大小 (FontSize)
                if (!CheckFontSize(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查字體樣式 (FontStyle)
                if (!CheckFontStyle(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 messageType (WindowDisplayMode)
                if (!CheckMessageType(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 MessageLength 之後的範圍
                if (!CheckMessageLength(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 MessageLevel
                if (!CheckMessageLevel(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 MessageScroll (ScrollMode, ScrollSpeed, PauseTime) 共 3 bytes
                if (!CheckMessageScroll(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 StringMode
                if (!CheckStringMode(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查 StringText 的內容直到 0x1F 之前
                if (!CheckStringText(receivedData, ref currentIndex, out errorMessage)) return false;
                // 檢查結尾的 0x1F, 0x1E, 0x1D
                if (!CheckEndBytes(receivedData, ref currentIndex, out errorMessage)) return false;
            }
            catch (Exception ex)
            {
                errorMessage = $"發生例外狀況: {ex.Message}";
                return false;
            }

            return true;
        }

        // 檢查起始碼 [0x55, 0xAA]
        private bool CheckStartCode(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            if (data[index] != 0x55 || data[index + 1] != 0xAA)
            {
                errorMessage = $"起始碼錯誤，位於 byte {index}，預期為 [0x55, 0xAA]";
                return false;
            }
            index += 2;
            return true;
        }

        // 檢查 ID_LENGTH 並跳過 ID 區段
        private bool CheckIdLength(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            byte idLength = data[index];
            if (idLength == 0x00)
            {
                errorMessage = $"ID_LENGTH 錯誤，位於 byte {index}，不能為 0x00";
                return false;
            }
            index++;

            if (index + idLength > data.Length || idLength < 1)
            {
                errorMessage = $"ID 長度無效或超出資料長度，位於 byte {index}";
                return false;
            }
            index += idLength;
            return true;
        }

        // 檢查 FunctionCode 是否為 0x34
        private bool CheckFunctionCode(byte[] data, ref int index, out string errorMessage)
        {
            return CheckNextByte(data, ref index, 0x34, out errorMessage, "FunctionCode");
        }

        // 檢查並解析 Data_Length (2 bytes, 小端序)
        private bool CheckDataLength(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            if (index + 2 > data.Length)
            {
                errorMessage = $"Data_Length 資料不足，位於 byte {index}";
                return false;
            }

            int dataLength = data[index] | (data[index + 1] << 8);
            index += 2;

            int remainingLength = data.Length - index - 1; // 扣掉最後一個 byte
            if (dataLength != remainingLength)
            {
                errorMessage = $"Data_Length 不匹配，位於 byte {index}，預期為 {remainingLength}，實際為 {dataLength}";
                return false;
            }
            return true;
        }

        // 檢查指定的 byte 是否符合預期值
        private bool CheckNextByte(byte[] data, ref int index, byte expectedValue, out string errorMessage, string name = "Byte")
        {
            errorMessage = "";
            if (data[index] != expectedValue)
            {
                errorMessage = $"預期 {name} 為 {expectedValue:X2}，位於 byte {index}，但實際為 {data[index]:X2}";
                return false;
            }
            index++;
            return true;
        }

        // 檢查並解析 SequenceLength (2 bytes, 小端序)
        private bool CheckSequenceLength(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            if (index + 2 > data.Length)
            {
                errorMessage = $"SequenceLength 資料不足，位於 byte {index}";
                return false;
            }

            int sequenceLength = data[index] | (data[index + 1] << 8);
            index += 2;

            int sequenceEndIndex = index + sequenceLength - 2; // 2 bytes for 0x1D
            if (sequenceEndIndex + 1 >= data.Length)
            {
                errorMessage = $"Sequence 長度超出資料範圍，位於 byte {sequenceEndIndex}";
                return false;
            }
            return true;
        }

        // 檢查 Sequence 的結束字節 (0x1D)
        private bool CheckSequenceEnd(byte[] data, ref int index, out string errorMessage)
        {
            return CheckNextByte(data, ref index, 0x1D, out errorMessage, "Sequence End");
        }

        // 檢查清除指令 0x77 和 0x7F
        private bool CheckClearCommand(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            if (index + 2 > data.Length)
            {
                errorMessage = $"清除指令資料不足，位於 byte {index}";
                return false;
            }

            if (data[index] != 0x77 || data[index + 1] != 0x7F)
            {
                errorMessage = $"預期清除指令 [0x77, 0x7F]，位於 bytes {index} 和 {index + 1}";
                return false;
            }
            index += 2;
            return true;
        }

        // 檢查字體大小 (FontSize)
        private bool CheckFontSize(byte[] data, ref int index, out string errorMessage)
        {
            return CheckEnumValue<FontSize>(data, ref index, out errorMessage, "FontSize");
        }

        // 檢查字體樣式 (FontStyle)
        private bool CheckFontStyle(byte[] data, ref int index, out string errorMessage)
        {
            return CheckEnumValue<Display.FontStyle>(data, ref index, out errorMessage, "FontStyle");
        }

        // 檢查 messageType (WindowDisplayMode)
        private bool CheckMessageType(byte[] data, ref int index, out string errorMessage)
        {
            return CheckEnumValue<WindowDisplayMode>(data, ref index, out errorMessage, "messageType");
        }
        
        // 檢查並解析 MessageLength (2 bytes, 小端序)
        private bool CheckMessageLength(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            if (index + 2 > data.Length)
            {
                errorMessage = $"MessageLength 資料不足，位於 byte {index}";
                return false;
            }

            int messageLength = data[index] | (data[index + 1] << 8);
            index += 2;

            int messageEndIndex = index + messageLength - 1;
            if (messageEndIndex >= data.Length || data[messageEndIndex] != 0x1E)
            {
                errorMessage = $"訊息未以 0x1E 結束或長度不正確，位於 byte {messageEndIndex}";
                return false;
            }
            return true;
        }

        // 檢查 MessageLevel (1 byte)
        private bool CheckMessageLevel(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            byte messageLevel = data[index];
            if (messageLevel < 0x01 || messageLevel > 0x04)
            {
                errorMessage = $"無效的 MessageLevel，位於 byte {index}，預期值介於 0x01 和 0x04 之間";
                return false;
            }
            index++;
            return true;
        }

        // 檢查 MessageScroll (ScrollMode, ScrollSpeed, PauseTime) 共 3 bytes
        private bool CheckMessageScroll(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            if (index + 3 > data.Length)
            {
                errorMessage = $"MessageScroll 資料不足，位於 byte {index}";
                return false;
            }

            byte scrollMode = data[index];
            byte scrollSpeed = data[index + 1];
            byte pauseTime = data[index + 2];
            index += 3;

            if (!Enum.IsDefined(typeof(ScrollMode), scrollMode)) 
            {
                errorMessage = $"無效的 ScrollMode，位於 byte {index - 3}，收到 {scrollMode:X2}";
                return false;
            }

            if (scrollSpeed > 0x09)
            {
                errorMessage = $"無效的 ScrollSpeed，位於 byte {index - 2}，收到 {scrollSpeed:X2}";
                return false;
            }

            if (pauseTime > 0xFF)
            {
                errorMessage = $"無效的 PauseTime，位於 byte {index - 1}，收到 {pauseTime:X2}";
                return false;
            }

            return true;
        }

        // 檢查 StringMode
        private bool CheckStringMode(byte[] data, ref int index, out string errorMessage)
        {
            return CheckEnumValue<StringMode>(data, ref index, out errorMessage, "StringMode");
        }

        // 檢查 StringText 的內容直到 0x1F 之前
        private bool CheckStringText(byte[] data, ref int index, out string errorMessage)
        {
            errorMessage = "";
            int endIndex = Array.IndexOf(data, (byte)0x1F, index);

            if (endIndex == -1)
            {
                errorMessage = $"訊息內容未以 0x1F 結束，從 byte {index} 開始";
                return false;
            }

            byte[] textBytes = new byte[endIndex - index];
            Array.Copy(data, index, textBytes, 0, textBytes.Length);

            string messageText = Encoding.GetEncoding(950).GetString(textBytes);

            // 在這裡可以進一步檢查 messageText 是否符合特定格式或條件
            // if (!ValidateMessageText(messageText)) { ... }

            index = endIndex + 1;
            return true;
        }

        // 檢查結尾的 0x1F, 0x1E, 0x1D
        private bool CheckEndBytes(byte[] data, ref int index, out string errorMessage)
        {
            if (!CheckNextByte(data, ref index, 0x1F, out errorMessage, "結尾 Byte 0x1F")) return false;
            if (!CheckNextByte(data, ref index, 0x1E, out errorMessage, "結尾 Byte 0x1E")) return false;
            return CheckNextByte(data, ref index, 0x1D, out errorMessage, "結尾 Byte 0x1D");
        }

        // 檢查枚舉值是否有效
        private bool CheckEnumValue<TEnum>(byte[] data, ref int index, out string errorMessage, string name) where TEnum : struct
        {
            errorMessage = "";
            TEnum value = (TEnum)(object)data[index];
            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                errorMessage = $"無效的 {name}，位於 byte {index}，收到 {data[index]:X2}";
                return false;
            }
            index++;
            return true;
        }
        // 定義策略接口，所有的 Handler 都需要實現這個接口
        public interface IMessageTypeHandler
        {
            bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage);
        }

        public class FullWindowHandler : IMessageTypeHandler
        {
            public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
            {
                errorMessage = "";
                // FullWindow 無需處理額外參數，直接返回 true
                return true;
            }
        }

        public class LeftPlatformHandler : IMessageTypeHandler
        {
            public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
            {
                errorMessage = "";
                // 檢查 LeftPlatform 的 4 個參數
                if (currentIndex + 4 > receivedData.Length)
                {
                    errorMessage = $"Insufficient data for LeftPlatform at byte {currentIndex}";
                    return false;
                }
                currentIndex += 4; // 跳過 4 個參數
                return true;
            }
        }

        public class LeftPlatformRightTimeHandler : IMessageTypeHandler
        {
            public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
            {
                errorMessage = "";
                // 檢查 LeftPlatformRightTime 的 9 個參數
                if (currentIndex + 9 > receivedData.Length)
                {
                    errorMessage = $"Insufficient data for LeftPlatformRightTime at byte {currentIndex}";
                    return false;
                }
                currentIndex += 9; // 跳過 9 個參數
                return true;
            }
        }
        public class RightTimeHandler : IMessageTypeHandler
        {
            public bool Handle(byte[] receivedData, ref int currentIndex, out string errorMessage)
            {
                errorMessage = "";
                // 檢查 RightTime 的 9 個參數
                if (currentIndex + 9 > receivedData.Length)
                {
                    errorMessage = $"Insufficient data for RightTime at byte {currentIndex}";
                    return false;
                }
                currentIndex += 9; // 跳過 9 個參數
                return true;
            }
        }


    }
}
