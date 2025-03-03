using Microsoft.Data.Sqlite;

namespace HabitTracker.StressedBread
{
    class DatabaseService
    {
        // Connection string to the SQLite database
        string connectionString = @"Data Source=HabitTracker.db";

        // Method to execute a non-query command (e.g., INSERT, UPDATE, DELETE)
        internal void ExecuteCommand(string commandText, List<SqliteParameter>? parameters = null)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    using (SqliteCommand command = new SqliteCommand(commandText, connection))
                    {
                        // Add parameters to the command if provided
                        if (parameters != null)
                            command.Parameters.AddRange(parameters.ToArray());

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                Console.WriteLine("Press any key to close the applicaiton");
                Console.ReadKey();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                Console.WriteLine("Press any key to close the applicaiton");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        // Method to execute a query command and return a data reader (e.g., SELECT)
        internal SqliteDataReader? ExecuteRead(string commandText, List<SqliteParameter>? parameters = null)
        {            
            try
            {
                SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                SqliteCommand command = new SqliteCommand(commandText, connection);
                // Add parameters to the command if provided
                if (parameters != null)
                    command.Parameters.AddRange(parameters.ToArray());

                // Execute the command and return the data reader
                return command.ExecuteReader();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                Console.WriteLine("Press any key to close the applicaiton");
                Console.ReadKey();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                Console.WriteLine("Press any key to close the applicaiton");
                Console.ReadKey();
                Environment.Exit(0);
            }            
            return null;
        }
    }
}

