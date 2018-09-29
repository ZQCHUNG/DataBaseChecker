using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseChecker.Class
{
    public static class SQLManager
    {
        public static class Select
        {
            /// <summary>
            /// 
            /// </summary>
            public static string GetAllTableName()
            {
                return "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="TableName"></param>
            public static string GetTableSchema(string TableName)
            {
                return string.Format("SELECT COLUMN_NAME,ORDINAL_POSITION,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", TableName);
            }

            /// <summary>
            /// 
            /// </summary>
            public static string GetTablePK(string TableName)
            {
                return string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = '{0}'",TableName);
            }
        }
    }
}
