using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseChecker.Class
{
    class DataBaseManager
    {
        public DataTable ConnDB(string ConnString, string SqlString)
        {
            ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    SqlCommand cmd = new SqlCommand(SqlString, conn);
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    cmd.ExecuteNonQuery();
                }
                return dt;
            }
            catch (Exception ex)
            {
                Logger.Error("SqlString::" + SqlString);

                throw new Exception(ex.ToString());
            }
        }
    }
}
