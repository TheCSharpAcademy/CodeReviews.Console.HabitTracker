using Microsoft.Data.Sqlite;

namespace HabitTracker.StressedBread
{
    class DatabaseService
    {
        string connectionString = @"Data Source=HabitTracker.db";
        internal void ExecuteCommand(string commandText, List<SqliteParameter>? parameters = null)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(commandText, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }
        internal SqliteDataReader ExecuteRead(string commandText, List<SqliteParameter>? parameters = null)
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            SqliteCommand command = new SqliteCommand(commandText, connection);
            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            return command.ExecuteReader();
        }
    }
}

