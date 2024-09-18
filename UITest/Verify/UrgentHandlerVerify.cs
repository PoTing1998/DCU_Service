using Display;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Display.DisplaySettingsEnums;

namespace UITest
{
    internal class UrgentHandlerVerify
    {

        #region  Public Method 判斷方式
        public bool ValidatePacket(byte[] receivedData, out string errorMessage)
        {
            int currentIndex = 0;
            errorMessage = "";

            try
            {
                // 檢查封包起始碼是否正確
                if (!CheckStartCode(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 1] {errorMessage}";
                    return false;
                }
                // 檢查 ID 和長度是否正確
                if (!CheckIDLength(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 2] {errorMessage}";
                    return false;
                }
                // 檢查功能碼是否正確
                if (!CheckFunctionCode(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 3] {errorMessage}";
                    return false;
                }
                // 檢查數據長度是否正確
                if (!CheckDataLength(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 4] {errorMessage}";
                    return false;
                }
                // 檢查序列長度是否正確
                if (!CheckSequenceLength(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 5] {errorMessage}";
                    return false;
                }
                // 檢查清除命令是否正確
                if (!CheckClearCommand(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 6] {errorMessage}";
                    return false;
                }
                // 檢查字體大小是否正確
                if (!CheckFontSize(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 7] {errorMessage}";
                    return false;
                }
                // 檢查字體樣式是否正確
                if (!CheckFontStyle(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 8] {errorMessage}";
                    return false;
                }
                // 檢查訊息類型是否正確
                if (!CheckMessageType(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 9] {errorMessage}";
                    return false;
                }
                // 檢查訊息長度是否正確 
                if (!CheckMessageLength(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 10] {errorMessage}";
                    return false;
                }
                // 檢查訊息等級是否正確
                if (!CheckMessageLevel(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 11] {errorMessage}";
                    return false;
                }
                // 檢查訊息是否具有滾動效果
                if (!CheckMessageScroll(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 12] {errorMessage}";
                    return false;
                }
                // 檢查字串模式是否正確
                if (!CheckStringMode(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 13] {errorMessage}";
                    return false;
                }
                // 檢查字串內容是否正確
                if (!CheckStringText(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 14] {errorMessage}";
                    return false;
                }
                // 檢查封包結束字節是否正確
                if (!CheckEndBytes(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 15] {errorMessage}";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Exception occurred: {ex.Message}";
                return false;
            }

            return true;
        }

        #endregion

        #region Private Method 判斷邏輯

        // 檢查開始碼 (StartCode)
        private bool CheckStartCode(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 確認封包的開始碼是否為 [0x55, 0xAA]
            if (receivedData[currentIndex] != 0x55 || receivedData[currentIndex + 1] != 0xAA)
            {
                errorMessage = $"StartCode mismatch at byte {currentIndex}, expected [0x55, 0xAA]";
                return false;
            }
            currentIndex += 2;
            return true;
        }

        // 檢查 ID 長度 (ID Length)
        private bool CheckIDLength(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            byte idLength = receivedData[currentIndex];
            // ID 長度不能為 0
            if (idLength == 0x00)
            {
                errorMessage = $"Invalid ID_LENGTH at byte {currentIndex}, cannot be 0x00";
                return false;
            }
            currentIndex++;

            // 確認 ID 長度是否合理，且不超出數據長度
            if (currentIndex + idLength > receivedData.Length || idLength < 1)
            {
                errorMessage = $"ID length is invalid or exceeds data length at byte {currentIndex}";
                return false;
            }
            currentIndex += idLength;
            return true;
        }

        // 檢查功能碼 (FunctionCode)
        private bool CheckFunctionCode(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 功能碼必須是 0x38
            if (receivedData[currentIndex] != 0x38)
            {
                errorMessage = $"FunctionCode mismatch at byte {currentIndex}, expected 0x38";
                return false;
            }
            currentIndex++;
            return true;
        }

        // 檢查數據長度 (Data Length)
        private bool CheckDataLength(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 確保有足夠的字節來讀取數據長度
            if (currentIndex + 2 > receivedData.Length)
            {
                errorMessage = $"Insufficient data for Data_Length at byte {currentIndex}";
                return false;
            }

            // 計算數據長度
            int dataLength = receivedData[currentIndex] | (receivedData[currentIndex + 1] << 8);
            currentIndex += 2;

            int remainingLength = receivedData.Length - currentIndex - 1;
            // 檢查數據長度是否與剩餘的封包長度一致
            if (dataLength != remainingLength)
            {
                errorMessage = $"Data_Length mismatch at byte {currentIndex}, expected {remainingLength}, got {dataLength}";
                return false;
            }
            return true;
        }

        // 檢查序列長度 (Sequence Length)
        private bool CheckSequenceLength(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 緊急訊息會在上下排顯示器前判斷是否開啟，代碼是 0x01 和 0x02
            if (receivedData[currentIndex] != 0x01 && receivedData[currentIndex] != 0x02)
            {
                errorMessage = $"Expected 0x01 or 0x02 at byte {currentIndex} for signal activation";
                return false;
            }
            currentIndex++;

            // 檢查上下排顯示器的顯示代碼
            if (receivedData[currentIndex] != 0x01 && receivedData[currentIndex] != 0x02)
            {
                errorMessage = $"Expected 0x01 or 0x02 at byte {currentIndex} for display mode";
                return false;
            }
            currentIndex++;

            // 確保有兩個字節來表示序列長度
            if (currentIndex + 2 > receivedData.Length)
            {
                errorMessage = $"Insufficient data for SequenceLength at byte {currentIndex}";
                return false;
            }

            // 計算序列長度
            int sequenceLength = receivedData[currentIndex] | (receivedData[currentIndex + 1] << 8);
            currentIndex += 2;

            // 檢查序列長度是否超出封包範圍
            int sequenceEndIndex = currentIndex + sequenceLength - 2;
            if (sequenceEndIndex + 1 >= receivedData.Length)
            {
                errorMessage = $"Sequence length extends beyond data length at byte {sequenceEndIndex}";
                return false;
            }
            return true;
        }

        // 檢查清除指令和緊急訊息代碼 (Clear Command)
        private bool CheckClearCommand(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 確保至少有一個字節用於清除指令
            if (currentIndex + 1 > receivedData.Length)
            {
                errorMessage = $"Insufficient data for Clear Command check at byte {currentIndex}";
                return false;
            }

            // 檢查是否有 0x77，這是可選的
            if (receivedData[currentIndex] == 0x77)
            {
                currentIndex++;
            }

            // 檢查緊急訊息代碼，必須是 0x79
            if (receivedData[currentIndex] != 0x79)
            {
                errorMessage = $"Expected urgent Command [optional 0x77, 0x79] at byte {currentIndex}";
                return false;
            }
            currentIndex++;

            // 檢查顯示器的燈號模式
            if (receivedData[currentIndex] != 0x01 && receivedData[currentIndex] != 0x02 && receivedData[currentIndex] != 0x03)
            {
                errorMessage = $"Expected urgentLightMode at byte {currentIndex}";
                return false;
            }
            currentIndex++;

            // 檢查顯示器的啟動次數，必須是 0x80
            if (receivedData[currentIndex] != 0x80)
            {
                errorMessage = $"Expected urgentTimeMode at byte {currentIndex}";
                return false;
            }
            currentIndex++;

            // 檢查播放模式，無限播放時應為 0xFF
            if (receivedData[currentIndex] != 0xFF)
            {
                errorMessage = $"Expected urgentTime at byte {currentIndex}";
                return false;
            }
            currentIndex++;

            // 最後應該是 0x7F
            if (currentIndex >= receivedData.Length || receivedData[currentIndex] != 0x7F)
            {
                errorMessage = $"Expected Clear Command [optional 0x77, 0x79, 0x7F] at byte {currentIndex}";
                return false;
            }
            currentIndex++;
            return true;
        }

        // 檢查字體大小 (Font Size)
        private bool CheckFontSize(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            FontSize fontSize = (FontSize)receivedData[currentIndex];
            // 檢查字體大小是否合法
            if (!Enum.IsDefined(typeof(FontSize), fontSize))
            {
                errorMessage = $"Invalid FontSize at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;
            return true;
        }

        // 檢查字體風格 (Font Style)
        private bool CheckFontStyle(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            Display.FontStyle fontStyle = (Display.FontStyle)receivedData[currentIndex];
            // 檢查字體風格是否合法
            if (!Enum.IsDefined(typeof(Display.FontStyle), fontStyle))
            {
                errorMessage = $"Invalid FontStyle at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;
            return true;
        }

        // 檢查訊息類型 (Message Type)
        private bool CheckMessageType(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            WindowDisplayMode messageType = (WindowDisplayMode)receivedData[currentIndex];

            // 檢查是否為合法的訊息類型
            if (!Enum.IsDefined(typeof(WindowDisplayMode), messageType))
            {
                errorMessage = $"Invalid messageType at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }

            // 使用工廠模式取得對應的處理器
            IMessageTypeHandler handler = MessageTypeHandlerFactory.GetHandler(messageType);

            // 使用處理器進行參數驗證
            return handler.Handle(receivedData, ref currentIndex, out errorMessage);
        }

        // 檢查訊息長度 (Message Length)
        private bool CheckMessageLength(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 確保有足夠的字節來讀取訊息長度
            if (currentIndex + 2 > receivedData.Length)
            {
                errorMessage = $"Insufficient data for MessageLength at byte {currentIndex}";
                return false;
            }
            int messageLength = receivedData[currentIndex] | (receivedData[currentIndex + 1] << 8);
            currentIndex += 2;

            // 檢查訊息是否以 0x1E 結束
            int messageEndIndex = currentIndex + messageLength - 1;
            if (messageEndIndex >= receivedData.Length || receivedData[messageEndIndex] != 0x1E)
            {
                errorMessage = $"Message does not end with 0x1E or length is incorrect at byte {messageEndIndex}";
                return false;
            }
            return true;
        }

        // 檢查訊息等級 (Message Level)
        private bool CheckMessageLevel(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            byte messageLevel = receivedData[currentIndex];
            // 訊息等級必須在 0x01 到 0x04 之間
            if (messageLevel < 0x01 || messageLevel > 0x04)
            {
                errorMessage = $"Invalid MessageLevel at byte {currentIndex}, expected value between 0x01 and 0x04";
                return false;
            }
            currentIndex++;
            return true;
        }

        // 檢查滾動訊息 (Message Scroll)
        private bool CheckMessageScroll(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 確保有足夠的字節來讀取滾動訊息參數
            if (currentIndex + 3 > receivedData.Length)
            {
                errorMessage = $"Insufficient data for MessageScroll at byte {currentIndex}";
                return false;
            }

            byte scrollMode = receivedData[currentIndex];
            byte scrollSpeed = receivedData[currentIndex + 1];
            byte pauseTime = receivedData[currentIndex + 2];
            currentIndex += 3;

            ScrollMode mode = (ScrollMode)scrollMode;
            // 檢查滾動模式是否合法
            if (!Enum.IsDefined(typeof(ScrollMode), mode))
            {
                errorMessage = $"Invalid ScrollMode at byte {currentIndex - 3}, received {scrollMode:X2}";
                return false;
            }

            // 檢查滾動速度是否合理
            if (scrollSpeed > 0x09)
            {
                errorMessage = $"Invalid ScrollSpeed at byte {currentIndex - 2}, received {scrollSpeed:X2}";
                return false;
            }

            // 檢查暫停時間是否合理
            if (pauseTime > 0xFF)
            {
                errorMessage = $"Invalid PauseTime at byte {currentIndex - 1}, received {pauseTime:X2}";
                return false;
            }
            return true;
        }

        // 檢查字串模式 (String Mode)
        private bool CheckStringMode(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 確保有足夠的字節來檢查字串模式
            if (currentIndex >= receivedData.Length)
            {
                errorMessage = $"Insufficient data for StringMode at byte {currentIndex}";
                return false;
            }
            StringMode stringMode = (StringMode)receivedData[currentIndex];
            // 檢查字串模式是否合法
            if (!Enum.IsDefined(typeof(StringMode), stringMode))
            {
                errorMessage = $"Invalid StringMode at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;
            return true;
        }

        // 檢查字串內容 (String Text)
        private bool CheckStringText(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 找到字串結束符號 0x1F
            int endIndex = Array.IndexOf(receivedData, (byte)0x1F, currentIndex);
            if (endIndex == -1)
            {
                errorMessage = $"Message content does not end with 0x1F, starting at byte {currentIndex}";
                return false;
            }

            // 提取字串內容
            byte[] textBytes = new byte[endIndex - currentIndex];
            Array.Copy(receivedData, currentIndex, textBytes, 0, textBytes.Length);

            // 將字串轉換為指定編碼
            string messageText = Encoding.GetEncoding(950).GetString(textBytes);

            if (receivedData[endIndex] != 0x1F)
            {
                errorMessage = $"Expected 0x1F at byte {endIndex}, but found {receivedData[endIndex]:X2}";
                return false;
            }
            currentIndex = endIndex + 1;
            return true;
        }

        // 檢查結束字節 (EndBytes)
        private bool CheckEndBytes(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            // 檢查封包是否以 0x1E 結束
            if (currentIndex >= receivedData.Length || receivedData[currentIndex] != 0x1E)
            {
                errorMessage = $"Expected 0x1E at byte {currentIndex}, but found {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;

            // 檢查封包的最後一個字節是否為 0x1D
            if (currentIndex >= receivedData.Length || receivedData[currentIndex] != 0x1D)
            {
                errorMessage = $"Expected 0x1D at byte {currentIndex}, but found {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;
            return true;
        }

        #endregion


    }
}
