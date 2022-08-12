using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelOperationsOnDataServers
{
    internal static class Config
    {
        public static void CreateConfig()
        {
            if(!File.Exists("Config.ini"))// Если файла нет, то создаем
            {
                using (StreamWriter sw = new StreamWriter("Config.ini", false, Encoding.UTF8))
                {
                    sw.WriteLine($@"Строка подключения");
                }
            }    
        }
    }
}
