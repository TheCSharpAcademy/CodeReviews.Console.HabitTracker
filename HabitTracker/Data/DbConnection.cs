using Microsoft.Data.Sqlite;

namespace HabitTracker.Data
{
    public class DbConnection
    {
        private static SqliteConnection _connection;

        private DbConnection() { }
        enum Frequency
        {
            Daily,
            Weekly,
            Monthly
        }

        public static SqliteConnection GetConnection(bool isStart = false)
        {
            if (_connection == null)
            {
                const string dbPath = "testDb.db";
                _connection = new SqliteConnection($"Data Source={dbPath}");
                _connection.Open();
                Console.WriteLine("The database has been connected.");
               
                if (isStart)
                {
                    DbConnection.CreateTable("Habits");
                    DbConnection.CreateTable("Records");
                    DbConnection.SeedData();
                }


                _connection.Close();
            }
            return _connection;
        }

        public static void CloseConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
                Console.WriteLine("The database connection has been closed.");
            }
        }

        public static void CreateTable(string name)
        {
            try
            {
                var command = _connection.CreateCommand();
                if (name == "Habits")
                {
                    command.CommandText =
                    @"
                        CREATE TABLE IF NOT EXISTS Habits (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,                    
                            Frequency TEXT NOT NULL,                    
                            TimesPerPeriod INTEGER NOT NULL,                    
                            StartDate TEXT NOT NULL                    
                        )
                    ";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText =
                    @"
                        CREATE TABLE IF NOT EXISTS Records (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,                    
                            HabitDate TEXT NOT NULL,
                            HabitId INTEGER,
                            FOREIGN KEY (HabitId) REFERENCES Habits(Id) ON DELETE CASCADE
                        )
                    ";
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

        }


        public static void SeedData()
        {
            bool create = false;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Habits";
                long habitCount = (long)command.ExecuteScalar();

                if (habitCount == 0)
                {
                    create = true;
                    Console.WriteLine("Seeding data into the Habits table.");
                    var random = new Random();

                    for (int i = 0; i < 100; i++)
                    {
                        string habitName = $"Habit {i + 1}";
                        Frequency frequency = (Frequency)random.Next(0, 3); 
                        string frequencyString = frequency.ToString(); 
                        int timesPerPeriod = random.Next(1, 5);
                        string startDate = DateTime.Now.AddDays(-random.Next(0, 100)).ToString("yyyy-MM-dd");

                        command.CommandText =
                        @"
                    INSERT INTO Habits (Name, Frequency, TimesPerPeriod, StartDate)
                    VALUES (@habitName, @frequency, @timesPerPeriod, @startDate)
                ";

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@habitName", habitName);
                        command.Parameters.AddWithValue("@frequency", frequencyString);
                        command.Parameters.AddWithValue("@timesPerPeriod", timesPerPeriod);
                        command.Parameters.AddWithValue("@startDate", startDate);

                        command.ExecuteNonQuery();
                    }
                }
            }

            if (create)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Name FROM Habits";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int habitId = reader.GetInt32(0);
                            string habitName = reader.GetString(1);

                            using (var insertCommand = _connection.CreateCommand())
                            {
                                for (int j = 0; j < 10; j++)
                                {
                                    string habitDate = DateTime.Now.AddDays(-new Random().Next(0, 30)).ToString("yyyy-MM-dd");

                                    insertCommand.CommandText =
                                    @"
                                INSERT INTO Records (Name, HabitDate, HabitId)
                                VALUES (@habitName, @habitDate, @habitId)
                            ";

                                    insertCommand.Parameters.Clear();
                                    insertCommand.Parameters.AddWithValue("@habitName", habitName);
                                    insertCommand.Parameters.AddWithValue("@habitDate", habitDate);
                                    insertCommand.Parameters.AddWithValue("@habitId", habitId);

                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }


    }
}
