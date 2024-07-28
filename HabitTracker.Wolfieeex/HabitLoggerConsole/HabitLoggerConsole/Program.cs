using Microsoft.Data.Sqlite;
using System;

namespace HabitLoggerConsole;

public class Program
{
    static void Main(string[] args)
    {
        string connectionString = @"Data Source=HabitLoggerConsole.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS going_to_gym (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Sets TEXT
                                        )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}