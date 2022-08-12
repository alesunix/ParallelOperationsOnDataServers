using System.Text;

namespace ParallelOperationsOnDataServers
{
    internal static class Config
    {
        private static string con = $@"Строка подключения";
        public static void CreateConfig()
        {
            if(!File.Exists("Config.ini"))// Если файла нет, то создаем
            {
                using (StreamWriter sw = new StreamWriter("Config.ini", false, Encoding.UTF8))
                {
                    sw.WriteLine(con);
                }
            }
        }
    }
}
