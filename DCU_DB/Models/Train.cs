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



  
}
