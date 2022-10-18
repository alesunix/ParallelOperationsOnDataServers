using Oracle.ManagedDataAccess.Client;
using ParallelOperationsOnDataServers.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelOperationsOnDataServers.Models
{
    internal class MainModel : BaseModel
    {
        private string Ip { get; set; }
        private string Port { get; set; }
        private string Sid { get; set; }
        private string Dsc { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public string Console { get; set; }
        public bool Work { get; set; }
        object locker = new object();
        public DataTable dtTable = new DataTable();
        public static CancellationTokenSource tokenSource = new CancellationTokenSource();
        public static CancellationToken token = tokenSource.Token;
        public MainModel()
        {
            if (GetServersList().Columns["Error"] != null)
                return;
            /// Создаю поля таблицы
            dtTable.Columns.Add("Ip", typeof(string));
            dtTable.Columns.Add("Port", typeof(string));
            dtTable.Columns.Add("Sid", typeof(string));
            dtTable.Columns.Add("Dsc", typeof(string));
            dtTable.Columns.Add("SysDate", typeof(string));
            dtTable.Columns.Add("Difference", typeof(string));
            dtTable.Columns.Add("Id", typeof(string));
            Fill_CollectionServers();/// Заполняю таблицу списком серверов
        }
        public DataTable GetServersList()// Получаю список серверов
        {
            DataTable dt = GetTable("SELECT * FROM tableServers");
            if (dt.Columns["Error"] != null)
            {
                Console = dt.Rows[0]["Error"].ToString();
            }
            return dt;
        }
        private void Fill_CollectionServers()// Заполняю таблицу списком серверов
        {
            dtTable.Clear();
            dtTable.DefaultView.Sort = String.Empty;
            int count = 0;
            foreach (DataRow item in GetServersList().Rows)
            {
                count++;
                Ip = item["IP"].ToString();
                Port = item["PORT"].ToString();
                Sid = item["SID"].ToString();
                Dsc = item["DSC"].ToString();
                dtTable.Rows.Add(Ip, Port, Sid, Dsc, String.Empty, String.Empty, String.Empty);
            }
            Console += $" - Всего серверов: {count}";
        }
        public async void Generate_ConnectionString()
        {
            Work = true;
            Console = "Выполняется!";
            Fill_CollectionServers();/// Заполняю таблицу списком серверов
            Dictionary<int, string> connStringList = new Dictionary<int, string>();
            var connString = new OracleConnectionStringBuilder();
            int count = -1;
            foreach (DataRow item in GetServersList().Rows)
            {
                count++;
                Ip = item["IP"].ToString();
                Port = item["PORT"].ToString();
                Sid = item["SID"].ToString();

                // Создаю строки подключения
                connString.UserID = "login";
                connString.Password = "Password";
                connString.ConnectionTimeout = 60;
                connString.DataSource = $"Строка подключения";
                connStringList.Add(count, connString.ToString());/// Собрать все подключения
            }
            token = tokenSource.Token;
            await Task.Run(() => ParallelStreams(connStringList), token);/// Параллельный запуск подключений к серверам + token отмены
            dtTable.DefaultView.Sort = "[Difference] DESC";/// Отсортировать
            ConsoleLog();
            Work = false;
        }
        private void ParallelStreams(Dictionary<int, string> connStringList)// Выполнение параллельных потоков
        {
            Parallel.ForEach(connStringList, item =>
            {
                if (!token.IsCancellationRequested)
                {
                    ConnectToServer_And_GetData(item.Key, item.Value);
                }
                else// Если поступил запрос на остановку потоков
                {
                    Console = "Принудительная остановка операции";
                    return;
                }
            });
        }
        private void ConnectToServer_And_GetData(int count, string connString)// Выполнение операции с данными
        {
            string sysDate = String.Empty;
            string difference = String.Empty;
            string id = String.Empty;
            WaitAndFillFinalTable(sysDate, difference, id, count);/// Ставлю метку "Waiting" - поток вошел
            DataTable dt = GetTable("SELECT sysdate date, id FROM sys_config WHERE param = 'logsat'");/// Подключение к серверам
            if (dt.Columns["Error"] == null)
            {
                sysDate = dt.Rows[0]["date"].ToString();
                id = dt.Rows[0]["id"].ToString();
                difference = DifferenceDate(sysDate);/// Разница во времени
            }
            else
            {
                sysDate = "Error " + dt.Rows[0]["Error"].ToString();
                difference = "Ошибка!";
                id = "er";
            }
            WaitAndFillFinalTable(sysDate, difference, id, count);/// Заполняю итоговую таблицу
        }
        private void WaitAndFillFinalTable(string sysDate, string difference, string id, int count)
        {
            lock (locker)/// Блокирует эту чать кода для синхронного чтения
            {
                /// Ставлю метку "Waiting" - начато выполнение, а после получения значения заполняю итоговую таблицу
                dtTable.Rows[count]["SysDate"] = sysDate == String.Empty ? "Waiting" : sysDate;
                dtTable.Rows[count]["Difference"] = difference == String.Empty ? "Waiting" : difference;
                dtTable.Rows[count]["Id"] = id == String.Empty ? "Waiting" : id;
            }
        }
        private string DifferenceDate(string sysDate)
        {
            if (Hour == 0 && Minute == 0 && Second == 0) Second = 1;
            TimeSpan timeStart = new TimeSpan(Hour * -1, Minute * -1, Second * -1);
            TimeSpan timeEnd = new TimeSpan(Hour, Minute, Second);
            DateTime startDate = DateTime.Now.Add(timeStart);
            DateTime endDate = DateTime.Now.Add(timeEnd);

            DateTime serverDate = Convert.ToDateTime(sysDate);
            if (serverDate.Between(startDate, endDate) == false)
            {
                DateTime systemDate = DateTime.Now;
                int hour = serverDate.Hour - systemDate.Hour;
                int minute = serverDate.Minute - systemDate.Minute;
                int second = serverDate.Second - systemDate.Second;
                return $"Разница - [{hour} часов. {minute} минут. {second} секунд.]";
            }
            else
                return "Все в порядке!";
        }
        private void ConsoleLog()
        {
            int countDate = 0;
            int errConnect = 0;
            int all = 0;
            int ok = 0;
            foreach (DataRow item in dtTable.Rows)
            {
                if (item.Field<string>("Difference").Contains("Разница") == true)
                    countDate++;
                else if (item.Field<string>("SysDate").Contains("Error") == true)
                    errConnect++;
                else if (item.Field<string>("Difference").Contains("Все в порядке!") == true)
                    ok++;
                all++;
            }
            Console = $"Выполнено! Количество серверов: - [Ok: {ok}] - [с разницей времени: {countDate}] - [с ошибкой подключения: {errConnect}] - Всего: {all}";
        }
        public void CancelTokenSource()
        {
            if(token.IsCancellationRequested == false)// Принудительная остановка потоков
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }
    }
}
