using Display;

using System;

using static Display.DisplaySettingsEnums;

namespace UITest.Verify
{
    /// <summary>
    /// 左上圖片 + 狀態識別版型：
    /// CommandType(1) → SwitchMode(1) → RGB(3) → PhotoIndex(1) → RightTimeSwitch(1)
    /// → CommandType2(1) + skip 5 → WindowDisplayMode + Factory
    /// </summary>
    public class IdentifierStatusImageTopLeft24x48HandlerVerify : PacketVerifyBase
    {
        protected override bool CheckMessageType(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";

            // 左側版行
            var cmdType = (DisplaySettingsEnums.CommandType)data[i];
            if (!Enum.IsDefined(typeof(DisplaySettingsEnums.CommandType), cmdType))
            {
                errorMessage = $"Invalid CommandType at byte {i}, received {data[i]:X2}";
                return false;
            }
            i++;

            // 左側圖塊開關
            if (data[i] != 0x30 && data[i] != 0x31)
            {
                errorMessage = $"Invalid SwitchMode at byte {i}, received {data[i]:X2}";
                return false;
            }
            i++;

            // RGB 顏色（byte 範圍 0~255 永遠成立，僅做長度安全檢查）
            if (i + 3 > data.Length) { errorMessage = $"Insufficient data for RGB at byte {i}"; return false; }
            i += 3;

            // 圖片代碼 index（0~12）
            byte photoIndex = data[i];
            if (photoIndex > 13)
            {
                errorMessage = $"Invalid PhotoIndex at byte {i}, value {photoIndex} exceeds 13";
                return false;
            }
            i++;

            // 右側時間開關
            if (data[i] != 0x30 && data[i] != 0x31)
            {
                errorMessage = $"Invalid SwitchMode at byte {i}, received {data[i]:X2}";
                return false;
            }
            i++;

            // 右側版行
            var cmdType2 = (DisplaySettingsEnums.CommandType)data[i];
            if (!Enum.IsDefined(typeof(DisplaySettingsEnums.CommandType), cmdType2))
            {
                errorMessage = $"Invalid CommandType at byte {i}, received {data[i]:X2}";
                return false;
            }
            i += 6;

            WindowDisplayMode msgType = (WindowDisplayMode)data[i];
            if (!Enum.IsDefined(typeof(WindowDisplayMode), msgType))
            {
                errorMessage = $"Invalid messageType at byte {i}, received {data[i]:X2}";
                return false;
            }
            return MessageTypeHandlerFactory.GetHandler(msgType).Handle(data, ref i, out errorMessage);
        }
    }
}
