using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.DB.Models.Train
{
    public  class train_location //列車訊息
    {
        [Key]
        public string train_unit_id // 列車編號
        {
            get;
            set;
        }
        public string train_position //列車在軌道位置
        {
            get;
            set;
        }
        public string train_destination //列車行駛方向
        {
            get;
            set;
        }
        public string train_event //列車事件
        {
            get;
            set;
        }
        public string train_status //列車狀態
        {
            get;
            set;
        }
    }
    public class train_message
    {
        [Key]
        public int start_address { get; set; }
        public string type { get; set; }
        public string command { get; set; }
        public int platform_id { get; set; }

        public int arrive_time1 { get; set; }
        public int depart_time1 { get; set; }
        public int destination1 { get; set; }

        public int arrive_time2 { get; set; }
        public int depart_time2 { get; set; }
        public int destination2 { get; set; }

    }


    public class OCS_Data
    {
        public int number_of_platforms { get; set; }
        public int platform_id { get; set; }
        public int arrival { get; set; }
        public int departure { get; set; }
        public int skip_hold { get; set; }
        public int number_of_journey_data { get; set; }
        public int validity_field { get; set; }
        public int train_unit_id { get; set; }
        public int service_number { get; set; }
        public int trip_number { get; set; }
        public int destination_number { get; set; }
        public int arrivaltime { get; set; }
        public int departuretime { get; set; }
        public int delayatarrival { get; set; }
        public int delayatdeparture { get; set; }
        public int cancelledtrain { get; set; }
        public int trainend_of_service { get; set; }
        public int lasttrainoftheoperatingday { get; set; }
        public int line_operation_mode { get; set; }
        public int train_direction { get; set; }
        public int validity_field2 { get; set; }
        public int train_unit_id2 { get; set; }
        public int service_number2 { get; set; }
        public int trip_number2 { get; set; }
        public int destination_number2 { get; set; }
        public int arrivaltime2 { get; set; }
        public int departuretime2 { get; set; }
        public int delayatarrival2 { get; set; }
        public int delayatdeparture2 { get; set; }
        public int cancelledtrain2 { get; set; }
        public int trainend_of_service2 { get; set; }
        public int lasttrainoftheoperatingday2 { get; set; }
        public int line_operation_mode2 { get; set; }
        public int train_direction2 { get; set; }
    }

}
