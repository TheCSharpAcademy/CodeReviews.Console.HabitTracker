using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace HabitTracker
{
    public class HabitRepository
    {
        private readonly string connectionString;

        public HabitRepository(string connectionString)
        {
            this.connectionString = connectionString;
            CreateDatabaseAndTable();
        }

        private void CreateDatabaseAndTable()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText =
            @"
                    CREATE TABLE IF NOT EXISTS Habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Date DATE NOT NULL
                    )
                ";
            createTableCommand.ExecuteNonQuery();
        }
    }
}
