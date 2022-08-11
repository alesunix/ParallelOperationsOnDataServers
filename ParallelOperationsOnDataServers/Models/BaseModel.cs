using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelOperationsOnDataServers.Models
{
    public class BaseModel
    {
        public static string connString = $@"Строка подключения";
        public DataTable GetTable(string query, string connString)
        {
            using (OracleConnection con = new OracleConnection(connString))
            {
                DataTable dt = new DataTable();
                try
                {
                    con.Open();
                    var cmd = new OracleCommand(query, con);
                    OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    dt.Load(dr);
                    return dt;
                }
                catch (OracleException ex)
                {
                    dt.Columns.Add("Error", typeof(string));
                    dt.Rows.Add(ex.Message);
                    return dt;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
}
