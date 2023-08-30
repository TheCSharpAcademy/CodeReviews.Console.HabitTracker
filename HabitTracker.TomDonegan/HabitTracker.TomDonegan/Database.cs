// Required for "SQLiteConnection" - obtain with NuGet
using System.Data.SQLite;

namespace HabitTracker.TomDonegan
{
    internal class Database
    {
        public static void DatabaseCreation()
        {
            string connectionString = @"Data Source=HabitTracker.db";

            // Read up on garbage collector "using"
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                //Create a command to send to the database
                var tableCmd = connection.CreateCommand();

                //tableCmd.CommandText = "";
                // @ allows multi line statements
                // SQLite has not Date type so date is stored as text
                tableCmd.CommandText =
                                @"CREATE TABLE IF NOT EXISTS drinking_water (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER
                                        )";

                // Don't want the database to return any values
                // just creating a table
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static async Task AsyncDatabaseConnection(string queryType, string databaseCommand) 
        {
            string connectionString = @"Data Source=HabitTracker.db";
            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = databaseCommand;
                    // Add CRUD features
                    if (queryType == "read")
                    {
                        using (var reader = await tableCmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Process data from the reader asynchronously
                                // For example: string value = reader.GetString(0);
                                Console.WriteLine(reader.GetString(0));
                            }
                        }
                    } else if (queryType == "write")
                    {
                        await tableCmd.ExecuteNonQueryAsync();
                    }
                }
                connection.Close();
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
                    } else if (queryType == "write")
                    {
                        tableCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void ChecklatestHabitRecords()
        {
            
        }
        
    }
}
