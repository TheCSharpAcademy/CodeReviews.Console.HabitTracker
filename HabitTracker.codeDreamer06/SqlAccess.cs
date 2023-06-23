using System;
using System.Data.SQLite;

namespace DotNet_SQLite
{
    class SqlAccess
    {
        protected static void Execute(string query)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection("Data Source=./codeTime.db;Version=3;"))
                {
                    con.Open();
                    using var cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
            }

            catch
            {
                Console.WriteLine("An unknown error occured.");
            }
        }

        public static void GetTimedLogs(string day)
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(-1);
            string dateToday = $"{today.Year}-{today.Month}-{today.Day}";
            string dateTomorrow = $"{tomorrow.Year}-{tomorrow.Month}-{tomorrow.Day}";
            if (day == "today")
                GetLogs($"select * from logs WHERE created_at = \"{dateToday}\";");
            if (day == "yesterday")
                GetLogs($"select * from logs WHERE created_at = \"{dateTomorrow}\";");
        }

        public static void GetLogs(string query = @"select * from logs")
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=./codeTime.db;Version=3;"))
            {
                con.Open();
                using var cmd = new SQLiteCommand(query, con);
                SQLiteDataReader reader = cmd.ExecuteReader();
                var readCount = 0;
                while (reader.Read())
                {
                    var rawDuration = new DateTime(long.Parse(reader["hours"].ToString()));
                    string hoursSuffix = rawDuration.Hour == 1 ? " hour" : " hours";
                    string minuteSuffix = rawDuration.Minute == 1 ? " minute" : " minutes";
                    var time = Convert.ToDateTime(reader["created_at"]).ToString().Split()[0];
                    var duration = rawDuration.Hour + hoursSuffix;
                    if (rawDuration.Minute != 0) duration = duration + " and " + rawDuration.Minute + minuteSuffix;
                    Console.WriteLine($"Log {reader["id"]}: {duration} on {time}");
                    readCount += 1;
                }
            }
        }

        public static void CreateTable()
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection("Data Source=./codeTime.db;Version=3;"))
                {
                    con.Open();
                    var cmd = new SQLiteCommand(@"SELECT name FROM sqlite_master WHERE type='table' AND name='logs'", con);
                    if (cmd.ExecuteScalar() == null)
                        Execute(@"CREATE TABLE logs(id INTEGER PRIMARY KEY, hours BIGINT NOT NULL, created_at DATETIME DEFAULT CURRENT_DATE)");
                }
            }
            catch
            {
                Console.WriteLine("Unable to check if table exists.");
            }
        }

        public static void AddLog(TimeSpan duration)
        {
            if (duration.Hours == 0 || duration.Hours > DateTime.Now.Hour) return; // Logging 0 hours isn't required, neither you code more than the number of hours already passed in the day
            Execute($"INSERT INTO logs(hours) VALUES({duration.Ticks});");
            Console.WriteLine("Your hours have been logged!");
            GetLogs(@"select * from logs ORDER BY id DESC LIMIT 5");
        }

        public static void RemoveLog(int index)
        {
            Execute($"DELETE FROM logs WHERE id = {index};");
            Console.WriteLine($"Log at {index} has been removed.");
            GetLogs(@"select * from logs ORDER BY id DESC LIMIT 5");
        }

        public static void RemoveLastLog()
        {
            Execute(@"DELETE FROM logs WHERE id = (SELECT MAX(id) FROM logs);");
            Console.WriteLine("The last log has been removed.");
            GetLogs(@"select * from logs ORDER BY id DESC LIMIT 3");
        }

        public static void UpdateLog(int id, TimeSpan duration)
        {
            if (duration.Hours == 0 || duration.Hours > DateTime.Now.Hour) return; // Logging 0 hours isn't required, neither you code more than the number of hours already passed in the day
            Execute($"UPDATE logs SET hours = {duration.Ticks}  WHERE id = {id}");
            Console.WriteLine("Updated successfully.");
            GetLogs($"select * from logs WHERE id BETWEEN {id - 2} and {id + 2}");
        }
    }
}
