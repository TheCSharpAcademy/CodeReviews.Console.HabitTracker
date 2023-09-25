using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.Forser
{
    internal class HabitTrackerLibrary
    {
        public static string ConnectionString { get; internal set; }

        static internal void Update()
        {
            GetAllRecords();

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you want to edit or type 0 to go back to Main Menu\n\n");

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_Water WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    connection.Close();
                    Update();
                }

                string date = Helpers.GetDateInput();

                int quantity = Helpers.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        static internal void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from drinking_water WHERE id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    Delete();
                }

                connection.Close();
            }

            Console.WriteLine($"\n\nRecord with Id{recordId} was deleted. \n\n");

            Helpers.GetUserInput();
        }

        static internal void GetAllRecords()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"SELECT * FROM drinking_water";
                List<DrinkingWater> tableData = new List<DrinkingWater>();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("------------------------------------------------\n");
                foreach (var drinkingWater in tableData)
                {
                    Console.WriteLine($"{drinkingWater.Id} - {drinkingWater.Date.ToString("dd-MMM-yyyy")} - Quantity: {drinkingWater.Quantity}");
                }
                Console.WriteLine("------------------------------------------------\n");
            }
        }

        static internal void Insert()
        {
            string date = Helpers.GetDateInput();

            int quantity = Helpers.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allow)\n\n");

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES ('{date}', {quantity})";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}