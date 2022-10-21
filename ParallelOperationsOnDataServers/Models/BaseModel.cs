using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ParallelOperationsOnDataServers.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            Config.CreateConfig();
        }
        private static string[] line = File.ReadAllLines("Config.ini");
        public static string connString = line[0].ToString();

        /// <summary>
        /// Выполняет sql-выражение ExecuteReader и формирует OracleDataReader. Подходит для sql-выражения SELECT.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connString"></param>
        /// <returns>Возвращает таблицу DataTable</returns>
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
