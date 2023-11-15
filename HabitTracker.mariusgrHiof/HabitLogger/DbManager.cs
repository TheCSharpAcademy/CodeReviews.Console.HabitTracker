using Microsoft.Data.Sqlite;

namespace HabitLogger
{
    public class DbManager
    {
        public void CreateDb(string dbName)
        {
            if (!string.IsNullOrEmpty(dbName))
            {
                using (SqliteConnection connection = new SqliteConnection($"Data Source={dbName}.db"))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS log
                        (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                         Hours INT NOT NULL,
                        DateCreated TEXT NOT NULL,
                        DateUpdated TEXT NOT NULL)";

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Fail to create db.");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invlaid db name");
            }
        }

        public void Add(int hours)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source=Time.db"))
            {
                connection.Open();

                var dateCreated = DateTime.Now.ToString();
                var dateUpdated = DateTime.Now.ToString();

                string queryString = "INSERT INTO log (Hours, DateCreated, DateUpdated) VALUES(@Hours, @DateCreated, @DateUpdated)";

                SqliteCommand command = new SqliteCommand(queryString, connection);
                command.Parameters.Add("Hours", SqliteType.Integer, hours).Value = hours;
                command.Parameters.Add("DateCreated", SqliteType.Text).Value = dateCreated;
                command.Parameters.Add("DateUpdated", SqliteType.Text).Value = dateUpdated;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fail to insert data.");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public CSharpLog Get(int id)
        {
            int logId = 0;
            int hours = 0;
            DateTime dateCreated = new DateTime();
            DateTime dateUpdated = new DateTime();

            using (SqliteConnection connection = new SqliteConnection($"Data Source=Time.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT Id, Hours, DateCreated, DateUpdated " +
                    $"FROM log " +
                    $"WHERE Id = {id}";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to fetch data.");
                    Console.WriteLine(ex.Message);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logId = Convert.ToInt32(reader.GetString(0));
                        hours = Convert.ToInt32(reader.GetString(1));
                        dateCreated = DateTime.Parse(reader.GetString(2));
                        dateUpdated = DateTime.Parse(reader.GetString(3));
                    }
                }
            }

            return new CSharpLog
            {
                Id = logId,
                Hours = hours,
                DateCreated = dateCreated,
                DateUpdated = dateUpdated
            };
        }

        public List<CSharpLog> GetAll()
        {
            List<CSharpLog> logs = new List<CSharpLog>();
            using (SqliteConnection connection = new SqliteConnection($"Data Source=Time.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT Id, Hours, DateCreated, DateUpdated " +
                    $"FROM log";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to fetch data.");
                    Console.WriteLine(ex.Message);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var logId = Convert.ToInt32(reader.GetString(0));
                        var hours = Convert.ToInt32(reader.GetString(1));
                        var dateCreated = DateTime.Parse(reader.GetString(2));
                        var dateUpdated = DateTime.Parse(reader.GetString(3));

                        logs.Add(new CSharpLog
                        {
                            Id = logId,
                            Hours = hours,
                            DateCreated = dateCreated,
                            DateUpdated = dateUpdated
                        });
                    }
                }
            }

            return logs;
        }

        public int Delete(int id)
        {
            int result = 0;
            using (var connection = new SqliteConnection("Data Source = Time.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM log WHERE Id = {id}";

                try
                {
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fail to delete record.");
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }

        public void Update(int id, int hours, DateTime updateDate)
        {
            using (var connection = new SqliteConnection("Data Source = Time.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"UPDATE log " +
                    $"SET Hours = {hours}, DateUpdated = '{updateDate.ToString()}' " +
                    $"WHERE Id = {id}";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to update record.");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
