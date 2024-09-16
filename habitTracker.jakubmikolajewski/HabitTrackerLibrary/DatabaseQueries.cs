using System.Data;
using System.Data.SQLite;

namespace HabitTrackerLibrary
{
    public class DatabaseQueries
    {
        private const string connectionString = "Data Source=HabitTrackerDatabase.sqlite;Version=3;New=False";
        public static List<string> currentTables = [];
        public static Dictionary<string, string> currentTableInfo = [];
        private SQLiteConnection? connection;
        private SQLiteCommand? command;
        public void CreateDatabaseIfNotExists()
        {
            if (!File.Exists("HabitTrackerDatabase.sqlite"))
            {
                try
                {
                    SQLiteConnection.CreateFile("HabitTrackerDatabase.sqlite");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during creating database file.\nDetails: {ex.Message}");
                }
                GenerateRandomDatabase();
            }
        }
        private void GenerateRandomDatabase()
        {
            RandomDateTime date = new RandomDateTime();
            List<string> habits = new List<string>() { "Running", "Swimming", "Cycling", "Walking" };

            using (connection = TryOpenConnection())
            {
                try
                {
                    foreach (string habit in habits)
                    {
                        command = connection.CreateCommand();
                        command.CommandText = $"create table {habit} (occurrenceId int, occurrenceDate text, distance int)";
                        command.ExecuteNonQuery();
                        for (int i = 0; i < 100; i++)
                        {
                            string randomDate = date.Next().ToString("yyyy-MM-dd HH:mm");
                            int randomDistance = Random.Shared.Next(1, 20);
                            using (command = connection.CreateCommand())
                            {
                                command.CommandText = $"insert into {habit} (occurrenceId, occurrenceDate, distance) values ({i}, '{randomDate}', {randomDistance})";
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during generating database.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void GetCurrentTables()
        {
            currentTables.Clear();
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = "select name from sqlite_master";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                currentTables.Add(reader.GetString(0));
                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during getting table information.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void CreateHabitQuery(string habitName, string chosenMeasurement)
        {
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = $"create table \"{habitName}\" (occurrenceId int, occurrenceDate text, \"{chosenMeasurement}\" int)";
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine($"Habit \"{habitName}\" has been created! You may insert data into this habit by using the 'i' option in the main menu.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during table creation.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void ViewHabitQuery(string chosenHabit)
        {
            GetTableInfo(chosenHabit);
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = $"select * from \"{chosenHabit}\"";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Occurrence ID: {reader["occurrenceId"]}, Occurrence date: {reader["occurrenceDate"]}, {currentTableInfo.Keys.ElementAt(2)}: {reader[2]}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during viewing records.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void UpdateHabitQuery(string chosenHabit, Dictionary<string, string> entryToUpdate, int occurrenceId)
        {
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        for (int i = 0; i < entryToUpdate.Count; i++)
                        {
                            command.CommandText = $"update \"{chosenHabit}\" set {entryToUpdate.ElementAt(i).Key} = @value where occurrenceId = @occurrenceId";
                            command.Parameters.Add("@value", DbType.String).Value = entryToUpdate.ElementAt(i).Value;
                            command.Parameters.Add("@occurrenceId", DbType.String).Value = occurrenceId;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during updating records.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            Console.WriteLine("Update successful.");
        }
        public void InsertIntoHabitQuery(string chosenHabit, Dictionary<string, string> entryToUpdate, int rowCount)
        {
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        string columns = String.Join(", ", entryToUpdate.Keys);
                        command.CommandText = $"insert into \"{chosenHabit}\" ({columns}) values (@value0, @value1, @value2)";
                        for (int i = 0; i < entryToUpdate.Count; i++)
                        {
                            command.Parameters.Add($"@value{i}", DbType.String).Value = entryToUpdate.ElementAt(i).Value;
                        }
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during inserting records.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            Console.WriteLine("Insert successful.");
        }
        public void DeleteFromQuery(string chosenHabit, List<string> valuesToDelete, int rowCount)
        {
            string column = valuesToDelete[0];
            valuesToDelete.RemoveAt(0);
            string values = String.Join("\", \"", valuesToDelete);

            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        Console.WriteLine(values);
                        command.CommandText = $"delete from \"{chosenHabit}\" where \"{column}\" in (\"{values}\");";
                        for (int j = 0; j < valuesToDelete.Count; j++)
                        {
                            command.Parameters.Add($"@value{j}", DbType.String).Value = valuesToDelete[j];
                        }
                        Console.WriteLine(command.CommandText);
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("Deletion complete.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during record deletion.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            } 
        }
        public void GetTableInfo(string habitChoice)
        {
            currentTableInfo.Clear();
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = $"PRAGMA table_info(\"{habitChoice}\")";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                currentTableInfo.Add($"{reader["name"]}", $"{reader["type"]}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during obtaiting table information.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void GenerateReportTotalQuery(string habitChoice, DateTime startDate, DateTime endDate)
        {
            GetTableInfo(habitChoice);
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = ($"select sum({currentTableInfo.Keys.ElementAt(2)}) from \"{habitChoice}\" where occurrenceDate between @startdate and @enddate");
                        command.Parameters.AddWithValue("@startdate", startDate);
                        command.Parameters.AddWithValue("@enddate", endDate);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Total: {reader[0]} between {startDate} and {endDate}.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during report generation.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }              
            }
        }
        public void GenerateReportTotalAmountQuery(string habitChoice, DateTime startDate, DateTime endDate)
        {
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = ($"select count(occurrenceId) from \"{habitChoice}\" where occurrenceDate between @startdate and @enddate");
                        command.Parameters.AddWithValue("@startdate", startDate);
                        command.Parameters.AddWithValue("@enddate", endDate);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"You have enjoyed this habit {reader["count(occurrenceId)"]} times between {startDate} and {endDate}.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during report generation.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void GenerateReportAverageQuery(string habitChoice, DateTime startDate, DateTime endDate)
        {
            GetTableInfo(habitChoice);
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = ($"select avg({currentTableInfo.Keys.ElementAt(2)}) from \"{habitChoice}\" where occurrenceDate between @startdate and @enddate");
                        command.Parameters.AddWithValue("@startdate", startDate);
                        command.Parameters.AddWithValue("@enddate", endDate);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Average: {reader[0]:N2} between {startDate} and {endDate}.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during report generation.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void GenerateRaportFiveGreatestQuery(string habitChoice)
        {
            GetTableInfo(habitChoice);
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = $"select * from \"{habitChoice}\" order by {currentTableInfo.Keys.ElementAt(2)} desc limit 5";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"OccurrenceId: {reader[0]}, occurrenceDate: {reader[1]}, {currentTableInfo.Keys.ElementAt(2)}: {reader[2]}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during report generation.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public int GetRowCount(string chosenHabit)
        {
            int rowCount = 0;
            using (connection = TryOpenConnection())
            {
                try
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = $"select count(occurrenceId) from \"{chosenHabit}\"";
                        rowCount = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during report generation.\nDetails: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return rowCount;
        }
        private SQLiteConnection TryOpenConnection()
        {
            connection = new SQLiteConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred when trying to open a connection to the database.\nDetails: {ex.Message}");
            }
            return connection;
        }
        class RandomDateTime
        {
            DateTime StartDate { get; set; }
            int Range { get; set; }
            public RandomDateTime()
            {
                StartDate = new DateTime(1995, 1, 1);
                Range = (DateTime.Today - StartDate).Days;
            }
            public DateTime Next()
            {
                return StartDate.AddDays(Random.Shared.Next(Range)).AddHours(Random.Shared.Next(0, 24)).AddMinutes(Random.Shared.Next(0, 60));
            }
        }
    }
}
