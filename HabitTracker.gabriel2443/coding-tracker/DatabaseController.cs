using Dapper;
using Microsoft.Data.Sqlite;

namespace coding_tracker
{
    internal class DatabaseController
    {
        private string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");

        internal void Insert(CodingSession coding)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                var sql = $"INSERT INTO codingSession(Date, StartTime, EndTime, Duration) VALUES('{coding.Date}', '{coding.StartTime}', '{coding.EndTime}', '{coding.Duration}')";

                var session = new { Date = coding.Date, StartTime = coding.StartTime, EndTime = coding.EndTime, Duration = coding.Duration };

                connection.Execute(sql, session);
            }
        }

        internal List<CodingSession> Read()

        {
            using (var connection = new SqliteConnection(connectionString))
            {
                var sql = @"SELECT * FROM codingSession";

                var codingSessions = connection.Query<CodingSession>(sql).ToList();
                if (codingSessions.Any())
                {
                    foreach (var session in codingSessions)
                    {
                        Console.WriteLine($"{session.Id} Date:{session.Date} Start time: {session.StartTime} Endtime: {session.EndTime} Duration: {session.Duration}");
                    }
                }
                else Console.WriteLine("No rows found");

                return codingSessions;
            }
        }

        internal void Delete(CodingSession coding)
        {
            var sql = $"DELETE FROM codingSession WHERE Id = {coding.Id}";

            using (var connection = new SqliteConnection(connectionString))
            {
                var deletedRows = connection.Execute(sql);
                if (deletedRows == 0) Console.WriteLine("Rows can not be deleted or does not exist");
            }
        }

        internal void Update(CodingSession coding)
        {
            var sql = @"UPDATE codingSession SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";

            using (var connection = new SqliteConnection(connectionString))
            {
                var updatedRows = connection.Execute(sql, coding);
                if (updatedRows == 0) Console.Write("no rows available to edit or the id does not exist");
            }
        }
    }
}