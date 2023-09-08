using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.TomDonegan
{
    internal static class DatabaseAccess
    {
        internal static ArrayList? GetTableList()
        {
            string connectionString = @"Data Source=HabitTracker.db";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.OpenAsync();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        ArrayList tableList = new ArrayList();

                        Helpers.DisplayHeader("Current Habits");
                        while (reader.Read())
                        {
                            tableList.Add(reader.GetString(0));
                        }
                        return tableList;
                    }
                }
            }
        }


        public static void DatabaseCreation(string habitName, string uom)
        {
            string connectionString = @"Data Source=HabitTracker.db";

            if (string.IsNullOrEmpty(habitName))
            {
                habitName = "drinking_water";
                uom = "L";
            }

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @$"CREATE TABLE IF NOT EXISTS {habitName} (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER,
                                        Unit of Measure TEXT DEFAULT {uom}
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

        public static bool HabitDatabaseConnection(string databaseQuery)
        {
            string connectionString = @"Data Source=HabitTracker.db";

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var tableCmd = connection.CreateCommand())
                    {
                        tableCmd.CommandText = databaseQuery;

                        using (var reader = tableCmd.ExecuteReader())
                        {
                            bool recordExists = false;

                            while (reader.Read())
                            {
                                if (!recordExists)
                                {
                                    Console.WriteLine(" Record     Date       Quantity    Unit");
                                    recordExists = true;
                                }

                                int record = reader.GetInt32(0);
                                string date = reader.GetString(1);
                                double quantity = reader.GetDouble(2);
                                string uom = reader.GetString(3);
                                Console.WriteLine(
                                    "{0,4} {1,13} {2,10} {3,7}",
                                    record,
                                    date,
                                    quantity,
                                    uom
                                );

                                recordExists = true;
                            }
                            return recordExists;
                        }
                        ;
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