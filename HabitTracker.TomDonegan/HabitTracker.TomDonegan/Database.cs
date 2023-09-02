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
                Console.WriteLine($"The Database could not be created. Error: {e}");
            }
        }

        public static async Task<bool> AsyncDatabaseConnection(string databaseCommand)
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

                        using (var reader = await tableCmd.ExecuteReaderAsync())
                        {
                            bool recordExists = false;                            

                            while (await reader.ReadAsync())
                            {
                                if (!recordExists)
                                {
                                    Console.WriteLine(" Record     Date       Quantity    ");
                                    recordExists = true;
                                }

                                int record = reader.GetInt32(0);
                                string date = reader.GetString(1);
                                double quantity = reader.GetDouble(2);
                                Console.WriteLine(
                                    "{0,4} {1,13} {2,10}",
                                    record,
                                    date,
                                    quantity + "L"
                                );

                                recordExists = true;
                            }
                            return recordExists;
                        };
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
    }
}
