using Display;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Display.DisplaySettingsEnums;

namespace UITest
{
    public class LeftPlatformRightTimeHandlerVerify
    {
        #region Public Method 判斷方式
        public bool ValidatePacket(byte[] receivedData, out string errorMessage)
        {
            int currentIndex = 0;
            errorMessage = "";
            // , Func<byte[], byte[]> tempFunc

            try
            {
                if (!CheckStartCode(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 1] {errorMessage}";
                    return false;
                }
                if (!CheckIDLength(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 2] {errorMessage}";
                    return false;
                }
                if (!CheckFunctionCode(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 3] {errorMessage}";
                    return false;
                }
                if (!CheckDataLength(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 4] {errorMessage}";
                    return false;
                }
                if (!CheckSequenceLength(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 5] {errorMessage}";
                    return false;
                }
                if (!CheckClearCommand(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 6] {errorMessage}";
                    return false;
                }
                if (!CheckFontSize(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 7] {errorMessage}";
                    return false;
                }
                if (!CheckFontStyle(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 8] {errorMessage}";
                    return false;
                }

                if (!CheckMessageType(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 9] {errorMessage}";
                    return false;
                }
                if (!CheckMessageLength(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 10] {errorMessage}";
                    return false;
                }
                if (!CheckMessageLevel(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 11] {errorMessage}";
                    return false;
                }
                if (!CheckMessageScroll(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 12] {errorMessage}";
                    return false;
                }
                if (!CheckStringMode(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 13] {errorMessage}";
                    return false;
                }
                if (!CheckStringText(receivedData, ref currentIndex, out errorMessage))
                {
                    errorMessage = $"[Step 14] {errorMessage}";
                    return false;
                }
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
        private bool CheckStartCode(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            if (receivedData[currentIndex] != 0x55 || receivedData[currentIndex + 1] != 0xAA)
            {
                errorMessage = $"StartCode mismatch at byte {currentIndex}, expected [0x55, 0xAA]";
                return false;
            }
            currentIndex += 2;
            return true;
        }

        private bool CheckIDLength(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            byte idLength = receivedData[currentIndex];
            if (idLength == 0x00)
            {
                errorMessage = $"Invalid ID_LENGTH at byte {currentIndex}, cannot be 0x00";
                return false;
            }
            currentIndex++;

            if (currentIndex + idLength > receivedData.Length || idLength < 1)
            {
                errorMessage = $"ID length is invalid or exceeds data length at byte {currentIndex}";
                return false;
            }
            currentIndex += idLength;
            return true;
        }

        private bool CheckFunctionCode(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            if (receivedData[currentIndex] != 0x34)
            {
                errorMessage = $"FunctionCode mismatch at byte {currentIndex}, expected 0x34";
                return false;
            }
            currentIndex++;
            return true;
        }

        private bool CheckDataLength(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            if (currentIndex + 2 > receivedData.Length)
            {
                errorMessage = $"Insufficient data for Data_Length at byte {currentIndex}";
                return false;
            }

            int dataLength = receivedData[currentIndex] | (receivedData[currentIndex + 1] << 8);
            currentIndex += 2;

            int remainingLength = receivedData.Length - currentIndex - 1;
            if (dataLength != remainingLength)
            {
                errorMessage = $"Data_Length mismatch at byte {currentIndex}, expected {remainingLength}, got {dataLength}";
                return false;
            }
            return true;
        }

        private bool CheckSequenceLength(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {

            errorMessage = "";
            if (receivedData[currentIndex] != 0x01 && receivedData[currentIndex] != 0x02)
            {
                errorMessage = $"Expected 0x01 or 0x02 at byte {currentIndex}";
                return false;
            }
            currentIndex++;

            if (currentIndex + 2 > receivedData.Length)
            {
                errorMessage = $"Insufficient data for SequenceLength at byte {currentIndex}";
                return false;
            }

            int sequenceLength = receivedData[currentIndex] | (receivedData[currentIndex + 1] << 8);
            currentIndex += 2;

            int sequenceEndIndex = currentIndex + sequenceLength - 2;
            if (sequenceEndIndex + 1 >= receivedData.Length)
            {
                errorMessage = $"Sequence length extends beyond data length at byte {sequenceEndIndex}";
                return false;
            }
            return true;
        }

        private bool CheckClearCommand(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            if (currentIndex + 1 > receivedData.Length)
            {
                errorMessage = $"Insufficient data for Clear Command check at byte {currentIndex}";
                return false;
            }
            if (receivedData[currentIndex] == 0x77)
            {
                currentIndex++;
            }
            if (currentIndex >= receivedData.Length || receivedData[currentIndex] != 0x7F)
            {
                errorMessage = $"Expected Clear Command [optional 0x77, 0x7F] at byte {currentIndex}";
                return false;
            }
            currentIndex++;
            return true;
        }

        private bool CheckFontSize(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            FontSize fontSize = (FontSize)receivedData[currentIndex];
            if (!Enum.IsDefined(typeof(FontSize), fontSize))
            {
                errorMessage = $"Invalid FontSize at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;
            return true;
        }

        private bool CheckFontStyle(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            Display.FontStyle fontStyle = (Display.FontStyle)receivedData[currentIndex];
            if (!Enum.IsDefined(typeof(Display.FontStyle), fontStyle))
            {
                errorMessage = $"Invalid FontStyle at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;
            return true;
        }

        private bool CheckMessageType(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";

            DisplaySettingsEnums.VersionType VersionType = (DisplaySettingsEnums.VersionType)receivedData[currentIndex];

            if (!Enum.IsDefined(typeof(DisplaySettingsEnums.VersionType), VersionType))
            {
                errorMessage = $"Invalid versionType at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex+=5;
            DisplaySettingsEnums.VersionType VersionType2 = (DisplaySettingsEnums.VersionType)receivedData[currentIndex];
            if (!Enum.IsDefined(typeof(DisplaySettingsEnums.VersionType), VersionType2))
            {
                errorMessage = $"Invalid versionType at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex +=6;
            WindowDisplayMode messageType = (WindowDisplayMode)receivedData[currentIndex];
            // 檢查是否為合法的 messageType
            if (!Enum.IsDefined(typeof(WindowDisplayMode), messageType))
            {
                errorMessage = $"Invalid messageType at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            // 使用工廠方法取得對應的處理器
            IMessageTypeHandler handler = MessageTypeHandlerFactory.GetHandler(messageType);
            // 使用處理器進行參數驗證
            return handler.Handle(receivedData, ref currentIndex, out errorMessage);
        }

        private bool CheckMessageLength(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";

            // 檢查當前索引加上2是否超過接收到的資料長度
            // 如果是的話，表示沒有足夠的資料來判斷訊息長度
            if (currentIndex + 2 > receivedData.Length)
            {
                errorMessage = $"在位元 {currentIndex} 沒有足夠的資料來判斷訊息長度";
                return false;
            }
   
            // 計算訊息的長度，從 currentIndex 的兩個 byte 組合出來的長度
            int messageLength = receivedData[currentIndex] | (receivedData[currentIndex + 1] << 8);
            currentIndex += 2;

            // 計算訊息的結尾索引
            int messageEndIndex = currentIndex + messageLength - 1;
            // 檢查訊息的結尾索引是否超過接收到的資料長度

            // 或者訊息的結尾是否不是 0x1E
            if (messageEndIndex >= receivedData.Length || receivedData[messageEndIndex] != 0x1E)
            {
                // 設置錯誤訊息，並返回 false
                errorMessage = $"訊息沒有以 0x1E 結束，或在位元 {messageEndIndex} 長度不正確";
                return false;
            }
            return true;
        }

        private bool CheckMessageLevel(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            byte messageLevel = receivedData[currentIndex];
            if (messageLevel < 0x01 || messageLevel > 0x04)
            {
                errorMessage = $"Invalid MessageLevel at byte {currentIndex}, expected value between 0x01 and 0x04";
                return false;
            }
            currentIndex++;
            return true;
        }

        private bool CheckMessageScroll(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
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
            if (!Enum.IsDefined(typeof(ScrollMode), mode))
            {
                errorMessage = $"Invalid ScrollMode at byte {currentIndex - 3}, received {scrollMode:X2}";
                return false;
            }

            if (scrollSpeed > 0x09)
            {
                errorMessage = $"Invalid ScrollSpeed at byte {currentIndex - 2}, received {scrollSpeed:X2}";
                return false;
            }

            if (pauseTime > 0xFF)
            {
                errorMessage = $"Invalid PauseTime at byte {currentIndex - 1}, received {pauseTime:X2}";
                return false;
            }
            return true;
        }

        private bool CheckStringMode(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            if (currentIndex >= receivedData.Length)
            {
                errorMessage = $"Insufficient data for StringMode at byte {currentIndex}";
                return false;
            }

            StringMode stringMode = (StringMode)receivedData[currentIndex];
            if (!Enum.IsDefined(typeof(StringMode), stringMode))
            {
                errorMessage = $"Invalid StringMode at byte {currentIndex}, received {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;
            return true;
        }

        private bool CheckStringText(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            int endIndex = Array.IndexOf(receivedData, (byte)0x1F, currentIndex);
            if (endIndex == -1)
            {
                errorMessage = $"Message content does not end with 0x1F, starting at byte {currentIndex}";
                return false;
            }

            byte[] textBytes = new byte[endIndex - currentIndex];
            Array.Copy(receivedData, currentIndex, textBytes, 0, textBytes.Length);

            string messageText = Encoding.GetEncoding(950).GetString(textBytes);

            if (receivedData[endIndex] != 0x1F)
            {
                errorMessage = $"Expected 0x1F at byte {endIndex}, but found {receivedData[endIndex]:X2}";
                return false;
            }

            currentIndex = endIndex + 1;
            return true;
        }

        private bool CheckEndBytes(byte[] receivedData, ref int currentIndex, out string errorMessage)
        {
            errorMessage = "";
            if (currentIndex >= receivedData.Length || receivedData[currentIndex] != 0x1E)
            {
                errorMessage = $"Expected 0x1E at byte {currentIndex}, but found {receivedData[currentIndex]:X2}";
                return false;
            }
            currentIndex++;

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
