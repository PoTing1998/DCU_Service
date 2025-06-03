using ASI.Wanda.DCU.DB.Models.Train;
using ASI.Wanda.DMD.JsonObject.DCU.FromDMD;
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
            static public void InsertTrain_MSG(TrainMSG trainMSG)
            {
            
                Insert(
                    trainMSG.Platform_id,
                    trainMSG.Arrive_time1,
                    trainMSG.Depart_time1,
                    trainMSG.Destination1,
                    trainMSG.Arrive_time2,
                    trainMSG.Depart_time2,
                    trainMSG.Destination2
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
