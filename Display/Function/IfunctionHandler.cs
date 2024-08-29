using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Function
{
    public interface IFunctionHandler
    {
        byte FunctionCode { get; }
      
    }

    public class CommunicationTestHandler : IFunctionHandler
    {
        public byte FunctionCode => 0x31; // 通訊測試
    }

    public class ParameterSettingHandler : IFunctionHandler
    {
        public byte FunctionCode => 0x32; // 參數設定&顯示模式切換  
    }

    public class PowerControlHandler : IFunctionHandler
    {
        public byte FunctionCode => 0x33; // 開關機
    }

    public class PassengerInfoHandler : IFunctionHandler
    {
        public byte FunctionCode => 0x34; // 旅客訊息的設定
    }

    public class PreRecordedDatabaseHandler : IFunctionHandler
    {
        public byte FunctionCode => 0x35; // 預錄資料庫
    }

    public class FontDatabaseUpdateHandler : IFunctionHandler
    {
        public byte FunctionCode => 0x36; // 字型資料庫更新
    }

    public class DeviceCommunicationUpdateHandler : IFunctionHandler
    {
        public byte FunctionCode => 0x37; // 設備通訊更新
    }

    public class EmergencyMessagePlaybackHandler : IFunctionHandler
    {
        public byte FunctionCode => 0x38; // 緊急訊息播放 
    }


}
