﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Lib.Text.Parsing;
using ASI.Wanda.DMD.JsonObject;

namespace ASI.Wanda.DMD.JsonObject.DCU.FromDMD
{
    #region  DMD Server to DCU 

    ///4.2.2. DMD Server to DCU 
    ///(1)	列車營運模式(需數張圖作為動態圖) 
    public class OperationMode : ASI.Wanda.DMD.JsonObject.Base
    {
        public OperationMode(ASI.Wanda.DMD.Enum.Station station) : base(station) { }

        public string station_id { get; set; }

        public string mode { get; set; }
    }


    //(4). 預錄訊息命令 
    public class SendPreRecordMessage : ASI.Wanda.DMD.JsonObject.Base
    {
        public SendPreRecordMessage(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        //席位
        public string seatID { get; set; }
        //訊息ID
        public List<string> msg_id { get; set; }
        //目標看板 [stationID]_[areaID]_[deviceID]
        public List<string> target_du { get; set; }
        //相關資料庫
        public string dbName1 { get; set; } = "dmd_pre_record_message";
        public string dbName2 { get; set; } = "dmd_target";
    }

    //(5). 即時訊息命令  
    public class SendInstantMessage : ASI.Wanda.DMD.JsonObject.Base
    {
        public SendInstantMessage(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        //席位
        public string seatID { get; set; }
        // 即時訊息ID
        public string msg_id { get; set; }

        // 目標看板 [stationID]_[areaID]_[deviceID]
        public List<string> target_du { get; set; } = new List<string>();

        public string dbName1 { get; set; } = "dmd_instant_message";
        public string dbName2 { get; set; } = "dmd_target";

    }
    //(6). 預錄訊息排程 
    public class SendScheduleSetting : ASI.Wanda.DMD.JsonObject.Base

    {
        public SendScheduleSetting(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        //席位
        public string seatID { get; set; }
        public string sched_id { get; set; }

        public Enum.SqlCommand SqlCommand { get; set; }

        public string dbName1 { get; set; } = "dmd_schedule";
        public string dbName2 { get; set; } = "dmd_schedule_playlist";

    }

    // (7).	預錄訊息設定
    public class PreRecordMessageSetting : ASI.Wanda.DMD.JsonObject.Base
    {
        public PreRecordMessageSetting(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        //席位
        public string seatID { get; set; }

        public string msg_id { get; set; }

        public Enum.SqlCommand SqlCommand { get; set; }

        public string dbName1 { get; set; } = "dmd_pre_record_message";

    }
    //(8) 列車訊息格式命令設定

    public class TrainMessageSetting : ASI.Wanda.DMD.JsonObject.Base
    {
        public TrainMessageSetting(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        //席位
        public string seatID { get; set; }

        public string msg_id { get; set; }

        public Enum.SqlCommand SqlCommand { get; set; }

        public string dbName1 { get; set; } = "dmd_train_message";
    }
    //  (9) 電源設定
    public class PowerTimeSetting : ASI.Wanda.DMD.JsonObject.Base
    {
        public PowerTimeSetting(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        //席位
        public string seatID { get; set; }
        public Enum.SqlCommand SqlCommand { get; set; }

        public string dbName1 { get; set; } = "dmd_power_setting";
    }
    //(10)	群組設定 
    public class GroupSetting : ASI.Wanda.DMD.JsonObject.Base
    {
        public GroupSetting(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        //席位
        public string seatID { get; set; }
        public string group_id { get; set; }
        public Enum.SqlCommand SqlCommand { get; set; }
        public string dbName1 { get; set; } = "dmd_group";
        public string dbName2 { get; set; } = "dmd_group_target";
    }

    //(11).	參數設定 
    public class ParameterSetting : ASI.Wanda.DMD.JsonObject.Base
    {
        public ParameterSetting(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        //席位
        public string seatID { get; set; }

        public Enum.SqlCommand SqlCommand { get; set; }
        public string dbName1 { get; set; }

    }

    //4.2.3.	DMD Server to DCU from OCS
    //(1).	車站設定(未完全，待討論)
    public class StationSeeting : ASI.Wanda.DMD.JsonObject.Base
    {
        public StationSeeting(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        public string Type;
        public string Command;
        public string Operation_mode;

    }
    //(2).	列車訊息(未完全，待討論)
    public class TrainMSG : ASI.Wanda.DMD.JsonObject.Base
    {
        public TrainMSG(ASI.Wanda.DMD.Enum.Station station) : base(station) { }
        public string Type;
        public string Command { get; set; }
        public int Platform_id; //U：上行(含大廳層、月台層)
                               //N：下行(含大廳層、月台層)
        public int Arrive_time1;
        public int Depart_time1;
        public int Destination1;
        public int Depart_time2;
        public int Arrive_time2;
        public int Destination2;

        public int Special1;  // 1. Cancelled Train = 2      2. NextTrainWillNotStop = 1 3. TrainEndOfService = 1 
                                // 4. TrainWillNotOpenDoor = 1 5. Train Not in service = 1 6. Test Train = 1

        public int Special2; //同上 

    }
}
#endregion

