using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Data
{
    internal abstract class Database
    {
        private const string DbFile = "habit-tracker.db";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Data Source={DbFile}");
        }

        public static void Initialize()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                bool tableExists = false;
                using (var checkCmd = connection.CreateCommand())
                {
                    checkCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='habit';";
                    using var reader = checkCmd.ExecuteReader();
                    tableExists = reader.Read();
                }

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"
                             CREATE TABLE IF NOT EXISTS habit_category (
                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                name TEXT,
                                unit TEXT
                             );
                             CREATE TABLE IF NOT EXISTS habit (
                                id INTEGER PRIMARY KEY AUTOINCREMENT,
	                            date TEXT NOT NULL,
	                            quantity INTEGER NOT NULL,
	                            category_id INTEGER NOT NULL,
	                            FOREIGN KEY(category_id) REFERENCES habit_category(id) ON DELETE CASCADE
                             );";
                tableCmd.ExecuteNonQuery();
                if (!tableExists)
                {
                    var seedCmd = connection.CreateCommand();
                    seedCmd.CommandText = $@"
                        INSERT INTO habit_category (name, unit) VALUES ('Water', 'ml');
                        INSERT INTO habit_category (name, unit) VALUES ('Pushups', 'reps');

                        INSERT INTO habit (date, quantity, category_id)
                        VALUES ('{DateTime.Now.ToString("yyyy-MM-dd")}', 2000, 1);
                        INSERT INTO habit (date, quantity, category_id)
                        VALUES ('{DateTime.Now.ToString("yyyy-MM-dd")}', 50, 2);
                    ";
                    seedCmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
