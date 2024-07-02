using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_Frame.Interface
{
  public  interface IMessageFormatter
    {
        byte[] GetStaticTextMessage(int level, string scroll, byte[] scrollSpeed, byte[] scrollPauseTime, byte[] stringColor, List<string> contentText, int index);
        byte[] GetPreRecordedMessage(int level, string scroll, byte[] scrollSpeed, byte[] scrollPauseTime, byte[] indexNumber, byte[] textColor);
        byte[] GetDynamicMessage(int level, string scroll, byte[] scrollSpeed, byte[] scrollPauseTime, byte[] stringColor, int graphicStartIndex, byte[] graphicNumber, int graphicStartIndex2, byte[] graphicNumber2, byte[] graphicColor, string firstStation, string firstStationMode, string secondStation, string secondStationMode, string localStation, string localStationMode);
    }
}
