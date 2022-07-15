﻿using Oracle.ManagedDataAccess.Client;
using ParallelOperationsOnDataServers.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelOperationsOnDataServers.Models
{
    internal class MainModel
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
        OracleConnection con = Connect.connect;
        public MainModel()
        {
            if (!ConnTest())
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
        public bool ConnTest()
        {
            try
            {
                con.Open();
                con.Close();
                Console = "Подключение выполнено успешно!";
                return true;
            }
            catch (OracleException ex)
            {
                Console = "Ошибка подключения!" + ex.Message.ToString();
                return false;
            }
        }
        private DataTable GetServersData()// Получаю список серверов
        {
            try
            {
                con.Open();
                var cmd = new OracleCommand("SELECT * FROM tableServers", con);
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                var da = new OracleDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (OracleException ex)
            {
                Console = "Ошибка подключения!" + ex.Message.ToString();
                return null;
            }
        }
        private void Fill_CollectionServers()// Заполняю таблицу списком серверов
        {
            dtTable.Clear();
            dtTable.DefaultView.Sort = String.Empty;
            int count = 0;
            foreach (DataRow item in GetServersData().Rows)
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
            foreach (DataRow item in GetServersData().Rows)
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
            await Task.Run(() => ParallelStreams(connStringList));/// Параллельный запуск подключений к серверам
            dtTable.DefaultView.Sort = "[Difference] DESC";/// Отсортировать
            ConsoleLog();
            Work = false;
        }
        private void ParallelStreams(Dictionary<int, string> connStringList)// Выполнение параллельных потоков
        {
            Parallel.ForEach(connStringList, item =>
            {
                ConnectToServer_And_GetData(item.Key, item.Value);
            });
        }
        private void ConnectToServer_And_GetData(int count, string connString)// Подключение к серверам
        {
            string sysDate = String.Empty;
            string difference = String.Empty;
            string id = String.Empty;
            WaitAndFillFinalTable(sysDate, difference, id, count);/// Ставлю метку "Waiting" - поток вошел
            using (OracleConnection con = new OracleConnection(connString))
            {
                try
                {
                    con.Open();
                    var cmd = new OracleCommand("SELECT sysdate date, id FROM sys_config WHERE param = 'logsat'", con);
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    var da = new OracleDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    sysDate = dt.Rows[0]["date"].ToString();
                    id = dt.Rows[0]["id"].ToString();
                    difference = DifferenceDate(sysDate);/// Разница во времени
                }
                catch (OracleException ex)
                {
                    sysDate = "Error " + ex.Message.ToString();
                    difference = "Ошибка!";
                    id = "er";
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                WaitAndFillFinalTable(sysDate, difference, id, count);/// Заполняю итоговую таблицу
            }
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
            TimeSpan tsStart = new TimeSpan(Hour * -1, Minute * -1, Second * -1);
            TimeSpan tsEnd = new TimeSpan(Hour, Minute, Second);
            DateTime startDate = DateTime.Now.Add(tsStart);
            DateTime endDate = DateTime.Now.Add(tsEnd);

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
                if (item.Field<string>("Difference").Contains("Разница"))
                {
                    countDate++;
                }
                else if (item.Field<string>("SysDate").Contains("Error") == true)
                {
                    errConnect++;
                }
                else if (item.Field<string>("Difference").Contains("Все в порядке!") == true)
                {
                    ok++;
                }
                all++;
            }
            Console = $"Выполнено! Количество серверов: - [Ok: {ok}] - [с разницей времени: {countDate}] - [с ошибкой подключения: {errConnect}] - Всего: {all}";
        }
    }
}
