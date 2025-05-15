using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker
{
    internal class DataInserter
    {
        static string connection_string = @"Data source=habit-tracker.db";

        internal static void InsertHabits()
        {
            using (var connection = new SqliteConnection(connection_string))
            {
                connection.Open();
                var water_cmd = connection.CreateCommand();
                water_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Glasses INTEGER
                                        )";
                water_cmd.ExecuteNonQuery();

                var pushups_cmd = connection.CreateCommand();
                pushups_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS doing_pushups (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Pushups INTEGER
                                        )";
                pushups_cmd.ExecuteNonQuery();

                var reading_cmd = connection.CreateCommand();
                reading_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS reading_books (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Pages INTEGER
                                        )";
                reading_cmd.ExecuteNonQuery();

                var meditating_cmd = connection.CreateCommand();
                meditating_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS meditating (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Minutes INTEGER
                                        )";
                meditating_cmd.ExecuteNonQuery();
            }
        }
        internal static void InsertData()
        {
            using (var connection = new SqliteConnection(connection_string))
            {
                connection.Open();

                var water_cmd = connection.CreateCommand();
                water_cmd.CommandText = "INSERT INTO drinking_water (Date, Glasses) VALUES (@date, @glasses)";
                var water_data = new List<(string Date, int Glasses)>
                {
                    ("26-10-2023", 2),
                    ("01-11-2023", 3),
                    ("15-11-2023", 1),
                    ("05-12-2023", 4),
                    ("20-12-2023", 2),
                    ("03-01-2024", 3),
                    ("18-01-2024", 1),
                    ("10-02-2024", 5),
                    ("25-02-2024", 2),
                    ("08-03-2024", 3),
                    ("22-03-2024", 1),
                    ("05-04-2024", 4),
                    ("19-04-2024", 2),
                    ("02-05-2024", 3),
                    ("16-05-2024", 1),
                    ("30-05-2024", 5),
                    ("12-06-2024", 2),
                    ("26-06-2024", 3),
                    ("09-07-2024", 1),
                    ("23-07-2024", 4),
                    ("05-08-2025", 2),
                    ("19-08-2025", 3),
                    ("02-09-2025", 1),
                    ("16-09-2025", 5),
                    ("01-10-2025", 2)
                };

                foreach (var (Date, Glasses) in water_data)
                {
                    water_cmd.Parameters.Clear();
                    water_cmd.Parameters.AddWithValue("@date", Date);
                    water_cmd.Parameters.AddWithValue("@glasses", Glasses);
                    water_cmd.ExecuteNonQuery();
                }

                var pushups_cmd = connection.CreateCommand();
                pushups_cmd.CommandText = "INSERT INTO doing_pushups (Date, Pushups) VALUES (@date, @pushups)";
                var pushups_data = new List<(string Date, int Pushups)>
                {
                    ("26-10-2023", 15),
                    ("01-11-2023", 20),
                    ("15-11-2023", 12),
                    ("05-12-2023", 25),
                    ("20-12-2023", 18),
                    ("03-01-2024", 22),
                    ("18-01-2024", 14),
                    ("10-02-2024", 30),
                    ("25-02-2024", 19),
                    ("08-03-2024", 23),
                    ("22-03-2024", 16),
                    ("05-04-2024", 28),
                    ("19-04-2024", 21),
                    ("02-05-2024", 24),
                    ("16-05-2024", 17),
                    ("30-05-2024", 32),
                    ("12-06-2024", 20),
                    ("26-06-2024", 26),
                    ("09-07-2024", 13),
                    ("23-07-2024", 29),
                    ("05-08-2025", 25),
                    ("19-08-2025", 27),
                    ("02-09-2025", 18),
                    ("16-09-2025", 31),
                    ("01-10-2025", 22),
                    ("10-10-2025", 35),
                    ("20-10-2025", 28),
                    ("30-10-2025", 23),
                    ("08-11-2025", 33),
                    ("18-11-2025", 29)
                };

                foreach (var (Date, Pushups) in pushups_data)
                {
                    pushups_cmd.Parameters.Clear();
                    pushups_cmd.Parameters.AddWithValue("@date", Date);
                    pushups_cmd.Parameters.AddWithValue("@pushups", Pushups);
                    pushups_cmd.ExecuteNonQuery();
                }

                var reading_cmd = connection.CreateCommand();
                reading_cmd.CommandText = "INSERT INTO reading_books (Date, Pages) VALUES (@date, @pages)";
                var reading_data = new List<(string Date, int Pages)>
                {
                    ("26-10-2023", 30),
                    ("01-11-2023", 45),
                    ("15-11-2023", 20),
                    ("05-12-2023", 60),
                    ("20-12-2023", 35),
                    ("03-01-2024", 50),
                    ("18-01-2024", 25),
                    ("10-02-2024", 70),
                    ("25-02-2024", 40),
                    ("08-03-2024", 55),
                    ("22-03-2024", 30),
                    ("05-04-2024", 65),
                    ("19-04-2024", 42),
                    ("02-05-2024", 52),
                    ("16-05-2024", 28),
                    ("30-05-2024", 75),
                    ("12-06-2024", 48),
                    ("26-06-2024", 58),
                    ("09-07-2024", 33),
                    ("23-07-2024", 68),
                    ("05-08-2025", 62),
                    ("19-08-2025", 56),
                    ("02-09-2025", 38),
                    ("16-09-2025", 72),
                    ("01-10-2025", 54)
                };

                foreach (var (Date, Pages) in reading_data)
                {
                    reading_cmd.Parameters.Clear();
                    reading_cmd.Parameters.AddWithValue("@date", Date);
                    reading_cmd.Parameters.AddWithValue("@pages", Pages);
                    reading_cmd.ExecuteNonQuery();
                }

                var meditating_cmd = connection.CreateCommand();
                meditating_cmd.CommandText = "INSERT INTO meditating (Date, Minutes) VALUES (@date, @minutes)";
                var meditating_data = new List<(string Date, int Minutes)>
                {
                    ("26-10-2023", 10),
                    ("01-11-2023", 15),
                    ("15-11-2023", 8),
                    ("05-12-2023", 20),
                    ("20-12-2023", 12),
                    ("03-01-2024", 18),
                    ("18-01-2024", 7),
                    ("10-02-2024", 25),
                    ("25-02-2024", 13),
                    ("08-03-2024", 17),
                    ("22-03-2024", 9),
                    ("05-04-2024", 22),
                    ("19-04-2024", 14),
                    ("02-05-2024", 16),
                    ("16-05-2024", 6),
                    ("30-05-2024", 28),
                    ("12-06-2024", 19),
                    ("26-06-2024", 21),
                    ("09-07-2024", 11),
                    ("23-07-2024", 23),
                    ("05-08-2025", 24),
                    ("19-08-2025", 26),
                    ("02-09-2025", 10),
                    ("16-09-2025", 27),
                    ("01-10-2025", 15)
                };

                foreach (var (Date, Minutes) in meditating_data)
                {
                    meditating_cmd.Parameters.Clear();
                    meditating_cmd.Parameters.AddWithValue("@date", Date);
                    meditating_cmd.Parameters.AddWithValue("@minutes", Minutes);
                    meditating_cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
