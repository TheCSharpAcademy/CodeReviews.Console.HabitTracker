using System.Collections;
using System.Data.SQLite;

namespace HabitTracker.TomDonegan
{
    internal static class DatabaseAccess
    {
        internal static ArrayList? GetTableList()
        {
            string connectionString = @"Data Source=HabitTracker.db";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.OpenAsync();

                using (var sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        ArrayList tableNames = new ArrayList();

                        Helpers.DisplayHeader("Current Habits");
                        while (reader.Read())
                        {
                            tableNames.Add(reader.GetString(0));
                        }
                        return tableNames;
                    }
                }
            }
        }

        public static void DatabaseCreation(string habitName, string uom)
        {
            string connectionString = @"Data Source=HabitTracker.db";

            if (string.IsNullOrEmpty(habitName))
            {
                habitName = "drinking_water";
                uom = "L";
            }

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var sqlCommand = connection.CreateCommand();

                    sqlCommand.CommandText =
                        @$"CREATE TABLE IF NOT EXISTS {habitName} (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER,
                                        Unit of Measure TEXT DEFAULT {uom}
                                        )";
                    sqlCommand.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The Database could not be created. Error: {e}");
            }
        }

        public static bool QueryAndDisplayResults(string databaseQuery)
        {
            string connectionString = @"Data Source=HabitTracker.db";

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var sqlCommand = connection.CreateCommand())
                    {
                        sqlCommand.CommandText = databaseQuery;

                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            bool hasRecords = false;

                            while (reader.Read())
                            {
                                if (!hasRecords)
                                {
                                    Console.WriteLine(" Record     Date       Quantity    Unit");
                                    hasRecords = true;
                                }

                                int recordId = reader.GetInt32(0);
                                string date = reader.GetString(1);
                                double quantity = reader.GetDouble(2);
                                string uom = reader.GetString(3);
                                Console.WriteLine(
                                    "{0,4} {1,13} {2,10} {3,7}",
                                    recordId,
                                    date,
                                    quantity,
                                    uom
                                );

                                hasRecords = true;
                            }
                            return hasRecords;
                        }
                        ;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"It has not been possible to connect to the Database. Error: {e}"
                );
                return false;
            }
        }

        public static void DeleteHabit(string habitName)
        {
            string connectionString = @"Data Source=HabitTracker.db";

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var sqlCommand = connection.CreateCommand();

                    sqlCommand.CommandText = @$"DROP TABLE {habitName}";
                    sqlCommand.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The Database table could not be deleted. Error: {e}");
            }
        }
    }
}
