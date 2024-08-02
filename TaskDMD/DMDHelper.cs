using ASI.Lib.Process;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DMD.TaskDMD
{

    /// <summary>
    /// 判別傳送過來的JsonName 
    /// </summary>
    public static class Constants
    {
        public const string SendPreRecordMsg                = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage";
        public const string SendInstantMsg                  = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendInstantMessage";
        public const string SendScheduleSetting             = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.ScheduleSetting";
        public const string SendPreRecordMessageSetting     = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PreRecordMessageSetting";
        public const string SendTrainMessageSetting         = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.TrainMessageSetting";
        public const string SendPowerTimeSetting            = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.PowerTimeSetting";
        public const string SendGroupSetting                = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.GroupSetting";
        public const string SendParameterSetting            = "ASI.Wanda.DMD.JsonObject.DCU.FromDMD.ParameterSetting";
    }                                                                    

    public class TaskDMDHelper<T> where T : class
    {
        private Action<T, ASI.Wanda.DMD.Message.Message> sendAction;
        private T API;

        public TaskDMDHelper(T api, Action<T, ASI.Wanda.DMD.Message.Message> sendAction)
        {
            API = api;
            this.sendAction = sendAction ?? throw new ArgumentNullException(nameof(sendAction));
        }
        public void HandleAckMessage(ASI.Wanda.DMD.Message.Message DMDServerMessage)
        {
            var sLog = $"Ack，訊息識別碼:[{DMDServerMessage.MessageID}]";
            var MSG = new ASI.Wanda.DMD.Message.Message(ASI.Wanda.DMD.Message.Message.eMessageType.Ack, DMDServerMessage.MessageID, null);
            ASI.Lib.Log.DebugLog.Log("FromDMDService", sLog);
            //利用委派的方式傳送
            sendAction?.Invoke(API, MSG);
        }

        ///傳送到內部MSG 
        public void SendToTaskUPD(int msgType, int msgID, string jsonData)
        {
            try
            {

                var MSGFromTaskUPD = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD(new MSGFrameBase("TaskDMD", "dcuservertaskupd"));
                //組相對應的封包 
                MSGFromTaskUPD.MessageType = msgType;
                MSGFromTaskUPD.MessageID = msgID;
                MSGFromTaskUPD.JsonData = jsonData;
                ASI.Lib.Process.ProcMsg.SendMessage(MSGFromTaskUPD);
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskDMD", ex);
            }
        }
        public void SendToTaskPDU(int msgType, int msgID, string jsonData)
        {
            try
            {
                var sendPreRecordMessage = new JsonObject.DCU.FromDMD.SendPreRecordMessage(Enum.Station.OCC);
      
                var MSGFromTaskPDU = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD(new MSGFrameBase("TaskDMD", "dcuservertaskpdu"));
                //組相對應的封包 
                MSGFromTaskPDU.MessageType = msgType;
                MSGFromTaskPDU.MessageID = msgID;
                MSGFromTaskPDU.JsonData = jsonData;
                ASI.Lib.Process.ProcMsg.SendMessage(MSGFromTaskPDU);
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskDMD", ex);
            }
        }
        public void SendToTaskSDU(int msgType, int msgID, string jsonData)
        {
            try
            {
                var MSGFromTaskSDU = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD(new MSGFrameBase("TaskDMD", "dcuservertasksdu"));
                //組相對應的封包 
                MSGFromTaskSDU.MessageType = msgType;
                MSGFromTaskSDU.MessageID = msgID;
                MSGFromTaskSDU.JsonData = jsonData;
                ASI.Lib.Process.ProcMsg.SendMessage(MSGFromTaskSDU);
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskDMD", ex);
            }
        }
        public void SendToTaskLPD(int msgType, int msgID, string jsonData)
        {
            try
            {
                var MSGFromTaskLPD = new ASI.Wanda.DCU.ProcMsg.MSGFromTaskDMD(new MSGFrameBase("TaskDMD", "dcuservertasklpd"));
                //組相對應的封包 
                MSGFromTaskLPD.MessageType = msgType;
                MSGFromTaskLPD.MessageID = msgID;
                MSGFromTaskLPD.JsonData = jsonData;
                ASI.Lib.Process.ProcMsg.SendMessage(MSGFromTaskLPD);
            }
            catch (System.Exception ex)
            {
                ASI.Lib.Log.ErrorLog.Log("TaskDMD", ex);
            }
        }


        ///收到DCU回傳的資料後 傳給CMFT
        public void SendResponsePreRecordMSGToCMFT(ASI.Wanda.DMD.Message.Message DMDServerMessage, string stationId, bool isSuccess, List<string> failedTargets)
        {
            var oJsonObject = (ASI.Wanda.DMD.JsonObject.DCU.FromDMD.SendPreRecordMessage)ASI.Wanda.DMD.Message.Helper.GetJsonObject(DMDServerMessage.JsonContent);
        }
        #region 資料庫的操作
        /// <summary>
        /// 更新dmd_playlist的資料庫
        /// </summary>  
        public IEnumerable<ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList> UpdateDCUPlayList()
        {
            try
            {
                ///抓取CMFT的資料表 
                var tempList = DMD.DB.Tables.DMD.dmdPlayList.SelectAll();
                ///轉換過程 
                var convertedList = tempList
                    .Select(item => new DB.Models.dmd_playlist
                    {
                        playlist_id = item.playlist_id,
                        station_id = item.station_id,
                        area_id = item.area_id,
                        device_id = item.device_id,
                        message_id = item.message_id,
                        message_type = item.message_type,
                        ins_time = item.ins_time,
                        ins_user = item.ins_user,
                        send_time = item.send_time,
                        upd_time = item.upd_time,
                        upd_user = item.upd_user,
                    })
                    .ToList();
                ///刪除原本的資料  
                convertedList.ForEach(item =>
                {
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.DeletePlayingItem(
                        item.station_id, item.area_id, item.device_id);
                });

                ///遍歷轉換後的列表，進行更新操作  
                foreach (var item in convertedList)
                {
                    ///MSGtype  0 =預錄  1= 及時  
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList.InsertPlayingItem(
                        item.playlist_id,
                        item.station_id,
                        item.area_id,
                        item.device_id,
                        item.message_id,
                        item.message_type,
                        item.send_time
                    );
                }
                ///選擇並分類同一車站的數據 
                return convertedList.Cast<DCU.DB.Tables.DMD.dmdPlayList>();
            }
            catch (Exception updateException)
            {
                ///記錄例外狀況
                ASI.Lib.Log.ErrorLog.Log("Error updating dmd_playlist", updateException);
                return Enumerable.Empty<ASI.Wanda.DCU.DB.Tables.DMD.dmdPlayList>();
            }

        }

        /// <summary>
        /// 更新DMDPreRecordMessage資料表   
        /// </summary>
        /// <returns></returns>    
        public IEnumerable<ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage> UpdataDCUPreRecordMessage()
        {
            try
            {
                var tempList = ASI.Wanda.DMD.DB.Tables.DMD.dmdPreRecordMessage.SelectAll();
                ///轉換過程  
                var convertedList = tempList
                    .Select(item => new ASI.Wanda.DMD.DB.Models.dmd_pre_record_message
                    {
                        message_id = item.message_id,
                        message_name = item.message_name,
                        message_type = item.message_type,
                        message_priority = item.message_priority,
                        move_mode = item.move_mode,
                        move_speed = item.move_speed,
                        Interval = item.Interval,
                        message_content = item.message_content,
                        font_type = item.font_type,
                        font_size = item.font_size,
                        font_color = item.font_color,
                        message_content_en = item.message_content_en,
                        font_type_en = item.font_type_en,
                        font_size_en = item.font_size_en,
                        font_color_en = item.font_color_en,
                        ins_user = item.ins_user,
                        ins_time = item.ins_time,
                        upd_user = item.upd_user,
                        upd_time = item.upd_time,
                    })
                    .ToList();
                convertedList.ForEach(item =>
                {
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.DeletePreRecordMessage(
                       item.message_id
                    );
                });
                ///遍歷轉換後的列表，進行更新操作 
                foreach (var item in convertedList)
                {
                    ///MSGtype  0 =預錄  1= 及時 
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage.InsertPreRecordMessage(
                        item.message_id,
                        item.message_name,
                        item.message_type,
                        item.message_priority,
                        item.move_mode,
                        item.move_speed,
                        item.Interval,
                        item.message_content,
                        item.font_type,
                        item.font_size,
                        item.font_color,
                        item.message_content_en,
                        item.font_type_en,
                        item.font_size_en,
                        item.font_color_en
                    );
                }

                return convertedList.Cast<DCU.DB.Tables.DMD.dmdPreRecordMessage>();
            }
            catch (Exception updateException)
            {
                ///記錄例外狀況   
                ASI.Lib.Log.ErrorLog.Log("Error updating dmdPreRecordMessage", updateException);
                return Enumerable.Empty<ASI.Wanda.DCU.DB.Tables.DMD.dmdPreRecordMessage>();
            }
        }

        /// <summary>
        /// 更新DMDPreRecordMessage資料表   
        /// </summary>
        /// <returns></returns>    
        public IEnumerable<ASI.Wanda.DCU.DB.Tables.DMD.dmdInstantMessage> UpdataDCUInstantMessage()
        {
            try
            {
                var tempList = ASI.Wanda.DMD.DB.Tables.DMD.dmdInstantMessage.SelectAll();
                ///轉換過程  
                var convertedList = tempList
                    .Select(item => new ASI.Wanda.DMD.DB.Models.dmd_instant_message
                    {
                        message_id = item.message_id,

                        message_type = item.message_type,
                        message_priority = item.message_priority,
                        move_mode = item.move_mode,
                        move_speed = item.move_speed,
                        Interval = item.Interval,
                        message_content = item.message_content,
                        font_type = item.font_type,
                        font_size = item.font_size,
                        font_color = item.font_color,
                        message_content_en = item.message_content_en,
                        font_type_en = item.font_type_en,
                        font_size_en = item.font_size_en,
                        font_color_en = item.font_color_en,
                        ins_user = item.ins_user,
                        ins_time = item.ins_time,
                        upd_user = item.upd_user,
                        upd_time = item.upd_time,
                    })
                    .ToList();
                convertedList.ForEach(item =>
                {
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdInstantMessage.DeleteInstantMessages(
                       item.message_id
                    );
                });
                ///遍歷轉換後的列表，進行更新操作 
                foreach (var item in convertedList)
                {
                    ///MSGtype  0 =預錄  1= 及時 
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdInstantMessage.InsertInstantMessages(
                        item.message_id,

                        item.message_type,
                        item.message_priority,
                        item.move_mode,
                        item.move_speed,
                        item.Interval,
                        item.message_content,
                        item.font_type,
                        item.font_size,
                        item.font_color,
                        item.message_content_en,
                        item.font_type_en,
                        item.font_size_en,
                        item.font_color_en
                    );
                }

                return convertedList.Cast<DCU.DB.Tables.DMD.dmdInstantMessage>();
            }
            catch (Exception updateException)
            {
                ///記錄例外狀況   
                ASI.Lib.Log.ErrorLog.Log("Error updating dmdPreRecordMessage", updateException);
                return Enumerable.Empty<ASI.Wanda.DCU.DB.Tables.DMD.dmdInstantMessage>();
            }
        }


        /// <summary>
        /// 從DMD更新Config的表 拿到相對色碼顏色  
        /// </summary>
        public IEnumerable<ASI.Wanda.DCU.DB.Tables.System.sysConfig> UpdataConfig()
        {
            try
            {
                var tempList = ASI.Wanda.DMD.DB.Tables.System.sysConfig.SelectAll();
                ///轉換過程 
                var convertedList = tempList
                    .Select(item => new ASI.Wanda.DMD.DB.Models.System.sys_config
                    {
                        config_name = item.config_name,
                        config_value = item.config_value,
                        config_description = item.config_description,
                        system_id = item.system_id,
                        remark = item.remark,
                        ins_user = item.ins_user,
                        ins_time = item.ins_time,
                        upd_user = item.upd_user,
                        upd_time = item.upd_time,
                    })
                    .ToList();

                ///遍歷轉換後的列表，進行更新操作 
                foreach (var item in convertedList)
                {
                    ASI.Wanda.DCU.DB.Tables.System.sysConfig.UpdataSystemConfig(
                       item.config_name,
                       item.config_value,
                       item.config_description,
                       item.system_id,
                       item.remark
                    );
                }

                return convertedList.Cast<ASI.Wanda.DCU.DB.Tables.System.sysConfig>();
            }
            catch (Exception updateException)
            {
                ///記錄例外狀況 
                ASI.Lib.Log.ErrorLog.Log("Error updating sysConfig", updateException);
                return Enumerable.Empty<ASI.Wanda.DCU.DB.Tables.System.sysConfig>();
            }
        }

        /// <summary>
        /// 更新dmd_schedule資料表  
        /// </summary>
        /// <returns></returns>    
        public IEnumerable<ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedule> UpSchedule()
        {
            try
            {
                var tempList = ASI.Wanda.DMD.DB.Tables.DMD.dmdSchedule.SelectAll();
                ///轉換過程  
                var convertedList = tempList
                    .Select(item => new ASI.Wanda.DCU.DB.Models.DMD.dmd_schedule
                    {
                        schedule_id     = item.schedule_id,
                        schedule_name   = item.schedule_name,
                        is_enable       = item.is_enable,
                        start_date      = item.start_date,
                        end_date        = item.end_date,
                        ins_user        = item.ins_user,
                        ins_time        = item.ins_time,
                        upd_user        = item.upd_user,
                        upd_time        = item.upd_time,
                    })
                    .ToList();
                convertedList.ForEach(item =>
                {
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedule.DeleteSchedule(
                       item.schedule_id
                    );
                });
                ///遍歷轉換後的列表，進行更新操作
                foreach (var item in convertedList)
                {
                    ///MSGtype  0 =預錄  1= 及時 
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedule.InsertSchedule(
                       item.schedule_id,
                       item.schedule_name,
                       item.is_enable,
                       item.start_date,
                       item.end_date
                    );
                }

                return convertedList.Cast<DCU.DB.Tables.DMD.dmdSchedule>();
            }
            catch (Exception updateException)
            {
                ///記錄例外狀況 
                ASI.Lib.Log.ErrorLog.Log("Error updating dmdSchedule", updateException);
                return Enumerable.Empty<ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedule>();
            }
        }

        public IEnumerable<ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedulePlayList> UpDMDSchedulePlaylist()
        {
            try
            {
                var tempList = ASI.Wanda.DMD.DB.Tables.DMD.dmdSchedulePlayList.SelectAll();
                ///轉換過程  
                var convertedList = tempList
                    .Select(item => new ASI.Wanda.DCU.DB.Models.DMD.dmd_schedule_playlist
                    {
                        schedule_id     = item.schedule_id,
                        message_id      = item.message_id,
                        station_id      = item.station_id,
                        device_id       = item.device_id,
                        sned_time       = item.sned_time,
                        ins_user        = item.ins_user,
                        ins_time        = item.ins_time,
                        upd_user        = item.upd_user,
                        upd_time        = item.upd_time,
                    })
                    .ToList();
                convertedList.ForEach(item =>
                {
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedulePlayList.DeleteSchedulePlayListItems(
                       item.schedule_id
                    );
                });
                ///遍歷轉換後的列表，進行更新操作
                foreach (var item in convertedList)
                {
                    ///MSGtype  0 =預錄  1= 及時 
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedulePlayList.InsertSchedulePlayListItem(
                       item.schedule_id,
                       item.message_id,
                       item.station_id,
                       item.device_id
                    );
                }

                return convertedList.Cast<DCU.DB.Tables.DMD.dmdSchedulePlayList>();
            }
            catch (Exception updateException)
            {
                ///記錄例外狀況 
                ASI.Lib.Log.ErrorLog.Log("Error updating dmdSchedulePlayList", updateException);
                return Enumerable.Empty<ASI.Wanda.DCU.DB.Tables.DMD.dmdSchedulePlayList>();
            }
        }
        public IEnumerable<ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting> UpDateDMDPowerSetting()
        {
            try
            {
                var tempList = ASI.Wanda.DMD.DB.Tables.DMD.dmdPowerSetting.SelectAll();
                ///轉換過程
                var convertedList = tempList
                    .Select(item => new ASI.Wanda.DCU.DB.Models.DMD.dmd_power_setting
                    {
                        station_id      = item.station_id,
                        eco_mode        = item.eco_mode,
                        eco_time        = item.eco_time,
                        not_eco_day     = item.not_eco_day,
                        auto_play_time  = item.auto_play_time,
                        auto_eco_time   = item.auto_eco_time,
                        ins_user        = item.ins_user,
                        ins_time        = item.ins_time,
                        upd_user        = item.upd_user,
                        upd_time        = item.upd_time,
                    })
                    .ToList();

                ///遍歷轉換後的列表，進行更新操作
                foreach (var item in convertedList)
                {
                
                    ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting.UpdatePowerSetting(
                       item.station_id,
                       item.eco_mode,
                       item.eco_time,
                       item.not_eco_day,
                       item.auto_play_time,
                       item.auto_eco_time
                    );
                }

                return convertedList.Cast<DCU.DB.Tables.DMD.dmdPowerSetting>();
            }
            catch (Exception updateException)
            {
                ///記錄例外狀況 
                ASI.Lib.Log.ErrorLog.Log("Error updating dmdPowerSetting", updateException);
                return Enumerable.Empty<ASI.Wanda.DCU.DB.Tables.DMD.dmdPowerSetting>();
            }
        }

        #endregion
    }

}




