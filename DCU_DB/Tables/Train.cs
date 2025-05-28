using ASI.Wanda.DCU.DB.Models.Train;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.DB.Tables.Train
{
 

        public class trainMessage : Table<train_message>
        {
            public static void DeleteTrainMSG(string type)
            {
                Delete(type);
            }
            #region Methods
            static public void InsertTrain_MSG(int platform_id, int arrive_time1, int depart_time1, int destination1, int arrive_time2, int depart_time2, int destination2)
            {
                Insert(
                    arrive_time1,
                    depart_time1,
                    destination1,
                    arrive_time2,
                    depart_time2,
                    destination2
                    );
            }

            static public void UpdateTrain_MSG(int strat_address)
            {
                string whereString = string.Format("where start_address = '{0}'   ", strat_address);
                Update();
            }

            static public void selectAddressID(int startAddress)
            {
                string whereString = string.Format("where start_address = '{0}'   ", startAddress);
                Select(whereString);
            }

            #endregion
        }
    
}
