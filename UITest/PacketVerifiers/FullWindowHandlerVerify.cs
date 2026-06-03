using Display;

using System;

using static Display.DisplaySettingsEnums;

namespace UITest.PacketVerifiers
{
    /// <summary>
    /// 全視窗版型：MessageType 直接為 WindowDisplayMode，交由 Factory 處理
    /// </summary>
    public class FullWindowHandlerVerify : PacketVerifyBase
    {
        protected override bool CheckMessageType(byte[] data, ref int i, out string errorMessage)
        {
            errorMessage = "";
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
