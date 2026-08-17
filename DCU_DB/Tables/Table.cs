using Dapper;
using Npgsql;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ASI.Wanda.DCU.DB.Tables
{
    abstract public class Table<T>
    {
        public enum eSortWay
        {
            Asc,
            Desc
        }

        #region 連線

        static protected IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(DB.Manager.ConnectionString);
        }

        #endregion

        #region 基礎存取（對應原本的 Query / NonQuery）

        static protected List<T> Query(string commandString, object sqlParameters = null)
        {
            try
            {
                using (IDbConnection connection = CreateConnection())
                {
                    connection.Open();
                    // Dapper 自動把 IDataReader 的每一列對應到 T 的屬性，
                    // 取代原本手動 while(reader.Read()) + 反射 SetValue 的迴圈。
                    return connection.Query<T>(commandString, sqlParameters).ToList();
                }
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format(
                    "Sql命令:" + Environment.NewLine + "{0}",
                    commandString);
                throw new Exception(errorMsg, ex);
            }
        }

        static protected int NonQuery(string commandString, object sqlParameters = null)
        {
            try
            {
                using (IDbConnection connection = CreateConnection())
                {
                    connection.Open();
                    return connection.Execute(commandString, sqlParameters);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"Sql命令:{Environment.NewLine}{commandString}";
                throw new Exception(errorMsg, ex);
            }
        }

        #endregion

        #region Key 屬性快取（給 Select / Update / Delete 用）

        private static PropertyInfo[] _keyProperties;
        private static PropertyInfo[] KeyProperties
        {
            get
            {
                if (_keyProperties == null)
                {
                    _keyProperties = typeof(T).GetProperties()
                        .Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)))
                        .ToArray();
                }
                return _keyProperties;
            }
        }

        #endregion

        static public List<T> SelectAll(eSortWay inserTimeSortWay = eSortWay.Asc)
        {
            string modelName = typeof(T).Name;
            string order = inserTimeSortWay == eSortWay.Asc ? "asc" : "desc";
            string commandString = $"select * from dbo.{modelName} order by ins_time {order};";
            return Query(commandString);
        }

        static protected T Select(params object[] pks)
        {
            string modelName = typeof(T).Name;
            var keyProps = KeyProperties;
            var dp = new DynamicParameters();
            var whereClauses = new List<string>();

            for (int i = 0; i < keyProps.Length && i < pks.Length; i++)
            {
                if (pks[i] == null) continue;

                string paramName = "pk" + i;
                whereClauses.Add($"{keyProps[i].Name} = @{paramName}");
                dp.Add(paramName, pks[i]);
            }

            string commandString = $"select * from dbo.{modelName} where {string.Join(" and ", whereClauses)};";
            return Query(commandString, dp).FirstOrDefault();
        }

        static protected int Update(params object[] paramObjects)
        {
            string modelName = typeof(T).Name;
            PropertyInfo[] properties = typeof(T).GetProperties();
            var dp = new DynamicParameters();
            var setClauses = new List<string>();

            // 沿用原本邏輯：paramObjects 依「屬性宣告順序」對應每一欄要 SET 的值
            for (int i = 0; i < properties.Length && i < paramObjects.Length; i++)
            {
                string paramName = "set" + i;
                setClauses.Add($"{properties[i].Name} = @{paramName}");
                dp.Add(paramName, paramObjects[i]);
            }

            dp.Add("upd_user", DB.Manager.CurrentUserID);

            // 沿用原本邏輯：where 條件用 Key 屬性，並假設對應的值排在 paramObjects 前面
            // （這是原本程式碼既有的假設，這裡沒有改變行為，只是換成參數化）
            var keyProps = KeyProperties;
            var whereClauses = new List<string>();
            for (int i = 0; i < keyProps.Length && i < paramObjects.Length; i++)
            {
                string paramName = "wk" + i;
                whereClauses.Add($"{keyProps[i].Name} = @{paramName}");
                dp.Add(paramName, paramObjects[i]);
            }

            string commandString = $@"
update dbo.{modelName} set
    {string.Join(", ", setClauses)},
    upd_user = @upd_user,
    upd_time = {DB.Manager.CurrentSqlTime}
where {string.Join(" and ", whereClauses)};";

            return NonQuery(commandString, dp);
        }

        static protected int Insert(params object[] paramObjects)
        {
            string modelName = typeof(T).Name;
            PropertyInfo[] properties = typeof(T).GetProperties();
            var dp = new DynamicParameters();
            var columnNames = new List<string>();
            var valueParams = new List<string>();

            for (int i = 0; i < paramObjects.Length; i++)
            {
                string paramName = "v" + i;
                // 對應原本欄位順序；若之後想更保險，可改成依 properties[i].Name 明確指定欄位
                columnNames.Add(i < properties.Length ? properties[i].Name : paramName);
                valueParams.Add("@" + paramName);
                dp.Add(paramName, paramObjects[i] ?? DBNull.Value);
            }

            columnNames.Add("ins_user");
            valueParams.Add("@ins_user");
            dp.Add("ins_user", DB.Manager.CurrentUserID);

            columnNames.Add("ins_time");
            valueParams.Add(DB.Manager.CurrentSqlTime); // 這是 SQL 函式（clock_timestamp()），不是資料值，不走參數

            columnNames.Add("upd_user");
            valueParams.Add("@upd_user");
            dp.Add("upd_user", DB.Manager.CurrentUserID);

            columnNames.Add("upd_time");
            valueParams.Add(DB.Manager.CurrentSqlTime);

            string commandString = $@"
insert into dbo.{modelName} ({string.Join(", ", columnNames)})
values ({string.Join(", ", valueParams)});";

            return NonQuery(commandString, dp);
        }

        static protected int Delete(params object[] pks)
        {
            string modelName = typeof(T).Name;
            var keyProps = KeyProperties;
            var dp = new DynamicParameters();
            var whereClauses = new List<string>();

            for (int i = 0; i < keyProps.Length && i < pks.Length; i++)
            {
                if (pks[i] == null) continue;

                string paramName = "pk" + i;
                whereClauses.Add($"{keyProps[i].Name} = @{paramName}");
                dp.Add(paramName, pks[i]);
            }

            string commandString = $"delete from dbo.{modelName} where {string.Join(" and ", whereClauses)};";
            return NonQuery(commandString, dp);
        }

        // 注意：eSortWay 保留在第 2 個位置，是為了跟現有呼叫端相容
        // （例如 Tables\DMD.cs 的 SelectWhere(where, eSortWay.Desc) 這種寫法）。
        // 新增的 param 讓你之後可以「漸進式」把呼叫端改成參數化：
        //   舊寫法（相容）：SelectWhere("where equip_id = '" + equip_id + "'")
        //   建議新寫法：    SelectWhere("where equip_id = @equip_id", param: new { equip_id })
        static protected List<T> SelectWhere(string where, eSortWay inserTimeSortWay = eSortWay.Asc, object param = null)
        {
            string modelName = typeof(T).Name;
            string orderByString = inserTimeSortWay == eSortWay.Asc
                ? "order by ins_time asc;"
                : "order by ins_time desc;";

            string commandString = $"select * from dbo.{modelName}\n{where}\n{orderByString}";
            return Query(commandString, param);
        }

        static protected int DeleteWhere(string where, object param = null)
        {
            string modelName = typeof(T).Name;
            string commandString = $"delete from dbo.{modelName}\n{where};";
            return NonQuery(commandString, param);
        }
    }
}
