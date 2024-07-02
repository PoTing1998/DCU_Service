using static DuFrame.DisplaySettingsEnums;
using System.Collections.Generic;
using System.Text;
using DuFrame;
using DuFrame.MSG;
using System;
using DU_Frame.Interface;

public static class MessageFormattingHelper 
{
    #region property
    static private byte[] StringEnd = new byte[] { 0x1F };
    static private Dictionary<string, byte> ScrollDic = new Dictionary<string, byte>
            {
                {"立即顯示",(byte)DuFrame.DisplaySettingsEnums.ScrollMode.jump },
                {"向左捲動(靠右對齊)",(byte) DuFrame.DisplaySettingsEnums.ScrollMode.Scroll_right},
                {"向左捲動(靠左對齊)",(byte) DuFrame.DisplaySettingsEnums.ScrollMode.Scroll_left },
                {"向左捲動(左移消失)",(byte) DuFrame.DisplaySettingsEnums.ScrollMode.Scroll_Disappearance },
                {"向下捲動",(byte) DuFrame.DisplaySettingsEnums.ScrollMode.scroll_down },
                {"向上捲動",(byte) DuFrame.DisplaySettingsEnums.ScrollMode.scroll_up},
                {"閃爍",(byte) DuFrame.DisplaySettingsEnums.ScrollMode.flash}
            };
    #endregion
    #region Public Method
    //取得靜態文字的封包
    public static byte[] GetStaticTextMessage(int level, string scroll, byte[] scrollSpeed, byte[] scrollPauseTime, byte[] stringColor, List<string> contentText, int index)
    {
        var byteList = new List<byte>();
        byteList.Add((byte)level);
        var scrollMode = GetStringMode(scroll);
        byteList.Add(scrollMode);
        byteList.AddRange(scrollPauseTime);
        byteList.AddRange(scrollSpeed);

        byteList.Add((byte)DuFrame.DisplaySettingsEnums.StringMode.text);
        byteList.AddRange(stringColor);
        //判斷有幾則訊息
        if (contentText.Count > index && index >= 0)
        {
            byteList.AddRange(Encoding.GetEncoding(950).GetBytes(contentText[index]));
        }
        byteList.AddRange(StringEnd);
        return byteList.ToArray();
    }
    //取得預錄訊息
    public static byte[] GetPreRecordedMessage(int level, string scroll, byte[] scrollSpeed, byte[] scrollPauseTime, byte[] indexNumber, byte[] textColor)
    {
        var byteList = new List<byte>();
        byteList.Add((byte)level); 
        var scrollMode = GetStringMode(scroll);
        byteList.Add((byte)scrollMode);
        byteList.AddRange(scrollSpeed);
        byteList.AddRange(scrollPauseTime); 
        byteList.Add((byte)DuFrame.DisplaySettingsEnums.StringMode.recordedMassage);
        byteList.AddRange(indexNumber);
        byteList.AddRange(textColor);
        byteList.AddRange(StringEnd);

        return byteList.ToArray();
    }
    //列車動態的封包
    public static byte[] GetDynamicMessage(int level, string scroll, byte[] scrollSpeed, byte[] scrollPauseTime, byte[] stringColor
        , int graphicStartIndex, byte[] graphicNumber, int graphicStartIndex2, byte[] graphicNumber2, byte[] graphicColor, string firstStation, string firstStationMode, string secondStation, string secondStationMode, string localStation, string localStationMode)
    {
        var byteList = new List<byte>();
        byteList.Add((byte)level);
        var scrollMode = GetStringMode(scroll);
        byteList.Add(scrollMode);
        byteList.AddRange(scrollPauseTime);
        byteList.AddRange(scrollSpeed);

        byte[] startIndex = GetByteArrayFromInt16(graphicStartIndex);
        byte[] startIndex2 = GetByteArrayFromInt16(graphicStartIndex2);

        AddStringToByteList(byteList, firstStationMode, stringColor, firstStation);

        AddStringWithStartIndexAndNumber(byteList, DisplaySettingsEnums.StringMode.recordedPicture, startIndex, graphicNumber, graphicColor);

        AddStringToByteList(byteList, secondStationMode, stringColor, secondStation);

        AddStringWithStartIndexAndNumber(byteList, DisplaySettingsEnums.StringMode.recordedPicture, startIndex2, graphicNumber2, graphicColor);

        AddStringToByteList(byteList, localStationMode, stringColor, localStation);

        return byteList.ToArray();
    }
    #endregion

    #region Private Method

    private static void AddStringToByteList(List<byte> byteList, string mode, byte[] color, string content)
    {
        DisplaySettingsEnums.StringMode stringMode = mode == "閃爍" ? DisplaySettingsEnums.StringMode.textFlash : DisplaySettingsEnums.StringMode.text;
        byteList.Add((byte)stringMode);
        byteList.AddRange(color);
        byteList.AddRange(Encoding.GetEncoding(950).GetBytes(content));
        byteList.AddRange(StringEnd);
    }

    private static void AddStringWithStartIndexAndNumber(List<byte> byteList, DisplaySettingsEnums.StringMode mode, byte[] startIndex, byte[] number, byte[] color)
    {
        byteList.Add((byte)mode);
        byteList.AddRange(startIndex);
        byteList.AddRange(number);
        byteList.AddRange(color);
        byteList.AddRange(StringEnd);
    }

    private static byte[] GetByteArrayFromInt16(int value)
    {
        byte[] byteArray = new byte[2];
        byteArray[0] = (byte)(value & 0xFF);        // 低位字節
        byteArray[1] = (byte)((value >> 8) & 0xFF); // 高位字節
        return byteArray;
    }
    //判斷移動的模式
    private static byte GetStringMode(string scrollMode)
    {
        if (ScrollDic.TryGetValue(scrollMode, out var mode))
        {
            return mode;
        }
        else
        {
            throw new InvalidOperationException("Invalid scrollMode");
        }
    }

    #endregion

}
