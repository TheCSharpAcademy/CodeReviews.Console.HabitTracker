using System;
using System.Data.SQLite;

namespace TaskManager
{
    class DatabaseManager
    {
        static string connectionString = "Data Source=taskmanager.sqlite;Version=3;";

        public static void Initialize()
        {
            CreateTable();
        }

        static void CreateTable()
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitName TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Date TEXT NOT NULL
                );
            ";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}