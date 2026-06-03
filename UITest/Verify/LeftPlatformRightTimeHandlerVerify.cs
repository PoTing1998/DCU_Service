using Display;

using System;

using static Display.DisplaySettingsEnums;

namespace UITest.Verify
{
    /// <summary>
    /// 左側月台 + 右側時間版型：
    /// CommandType(1) + skip 4 → CommandType2(1) + skip 5 → WindowDisplayMode + Factory
    /// </summary>
    public class LeftPlatformRightTimeHandlerVerify : PacketVerifyBase
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
            i += 5;

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
