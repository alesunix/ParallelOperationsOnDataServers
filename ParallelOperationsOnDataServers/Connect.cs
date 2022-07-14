using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelOperationsOnDataServers
{
    internal class Connect
    {
        public static OracleConnection connect = new OracleConnection($@"Строка подключения"); 
    }
}
