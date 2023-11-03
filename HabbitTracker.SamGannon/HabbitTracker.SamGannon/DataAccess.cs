using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HabitTracker.SamGannon
{
    internal class DataAccess
    {
        private string connectionString;
        private Helpers helper;
        private Menu menu;

        public DataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void GetAllRecords()
        {
            List<DrinkingWater> tableData = new List<DrinkingWater>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM drinking_water";

                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(
                                new DrinkingWater
                                {
                                    Id = reader.GetInt32(0),
                                    Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                    Quantity = reader.GetInt32(2),
                                });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                }
            }

            Console.WriteLine("---------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("---------------------------------------\n");
        }

        public void InsertRecord()
        {
            string date = helper.GetDateInput();

            int quantity = helper.GetNumberInput("\n\nPLease insert number of glasses or other measure of your choice (no decials allowed)\n\n");
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

        }

        public void UpdateRecord()
        {

            GetAllRecords();

            var recordId = helper.GetNumberInput("\n\nPlease type the Id of the record you want to delete or press 1 to go to the Main Menu");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    connection.Close();
                    UpdateRecord();
                }

                string date = helper.GetDateInput();

                int quantity = helper.GetNumberInput("\n\nPlease insert number of glasses or other measureof your choice (no decimals allowed)\n\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();

            }
        }

        public void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = helper.GetNumberInput("\n\nPlease type the Id of the record you want to delete or press 1 to go to the Main Menu");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    DeleteRecord();
                }

                Console.WriteLine($"\n\nRecord with Id {recordId} was deleted \n\n");

                menu.GetUserInput();
            }
        }

        internal void GetRecordsByCups()
        {
            List<DrinkingWater> tableData = new List<DrinkingWater>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM drinking_water ORDER BY quantity DESC";

                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(
                                new DrinkingWater
                                {
                                    Id = reader.GetInt32(0),
                                    Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                    Quantity = reader.GetInt32(2),
                                });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                }
            }

            Console.WriteLine("---------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("---------------------------------------\n");
        }
    }
}
