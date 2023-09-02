// Required for "SQLiteConnection" - obtain with NuGet
using System.Data;
using System.Data.SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.TomDonegan
{
    internal class Database
    {
        public static async void DatabaseCheck()
        {
            string connectionString = @"Data Source=HabitTracker.db";

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS drinking_water (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER
                                        )";
                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The Database could not be created. Error: {e.ToString()}");
            }
        }

        public static async Task AsyncDatabaseConnection(string queryType, string databaseCommand)
        {
            string connectionString = @"Data Source=HabitTracker.db";
            string? returnData = null;
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var tableCmd = connection.CreateCommand())
                    {
                        tableCmd.CommandText = databaseCommand;

                        if (queryType == "read")
                        {
                            using (var reader = await tableCmd.ExecuteReaderAsync())
                            {
                                Console.WriteLine(" Record     Date       Quantity    ");
                                while (await reader.ReadAsync())
                                {
                                    int record = reader.GetInt32(0);
                                    string date = reader.GetString(1);
                                    double quantity = reader.GetDouble(2);
                                    Console.WriteLine(
                                        "{0,4} {1,13} {2,10}",
                                        record,
                                        date,
                                        quantity + "L"
                                    );
                                }
                            };
                        }
                        else if (queryType == "delete") 
                        {
                            

                            using (var reader = await tableCmd.ExecuteReaderAsync())
                            {
                                int rowCount = Convert.ToInt32(tableCmd.ExecuteScalar());
                                if (rowCount > 0) {
                                    Console.WriteLine(" Record     Date       Quantity    ");
                                    while (await reader.ReadAsync())
                                    {
                                        int record = reader.GetInt32(0);
                                        string date = reader.GetString(1);
                                        double quantity = reader.GetDouble(2);
                                        Console.WriteLine(
                                            "{0,4} {1,13} {2,10}",
                                            record,
                                            date,
                                            quantity + "L"
                                        );
                                    }
                                } else
                                {
                                    Console.WriteLine("This record does not exist. Please make sure you selection is coreect");
                                }
                            };
                        }
                        else
                        {
                            await tableCmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"It has not been possible to connect to the Database. Error: {e}"
                );
            }
        }

        public static void DatabaseConnection(string queryType, string databaseCommand)
        {
            string connectionString = @"Data Source=HabitTracker.db";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var tableCommand = connection.CreateCommand())
                {
                    tableCommand.CommandText = databaseCommand;

                    if (queryType == "read")
                    {
                        using (SQLiteDataReader reader = tableCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Process data from the reader synchronously
                                Console.WriteLine(reader.GetString(0));
                                //int age = reader.GetInt32(1);
                                //Console.WriteLine($"Name: {name}, Age: {age}");
                            }
                        }
                    }
                    else if (queryType == "write")
                    {
                        tableCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void ChecklatestHabitRecords() { }
    }
}
