using ASI.Wanda.DCU.DB.Models;
using ASI.Wanda.DCU.DB.Models.PA;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Wanda.DCU.DB.Tables.PA
{
    public class paContent : ASI.Wanda.DCU.DB.Tables.Table<pa_content>
    {
        public static void UpdateContent(string message_id, string content)
        {
            var message =
                 SelectWhere(string.Format("where message_id = '{0}'", message_id))
                .SingleOrDefault();
            Update(
                message,
                content
                ); 
        }
        public static string SelectContent(string message_id)
        {
            var Content = 
                SelectWhere(string.Format("where message_id = '{0}'", message_id))
               .SingleOrDefault();

            return Content.message_content; 
        }

    }
}
