using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class CRUD
    {
        public static List<DrinkingWater> tableData = new();
        public static string connectionString = @"Data Source=Habit-Tracker.db";
        public static bool GetAllRecords()
        {
            bool hasRows = false;
            var menu = new Menu();
            string habit = Habits.GetInputHabit(Menu.habits);
            string info = Helpers.GetColumn(habit);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                $"SELECT * FROM {habit}";

                tableData.Clear();

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
                            }
                        );
                    }
                    hasRows = true;
                }
                else
                    Console.WriteLine("No rows found");

                connection.Close();
                if (tableData.Capacity > 0)
                {
                    Console.WriteLine("----------------------");

                    foreach (var dw in tableData)
                    {
                        Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - {info}: {dw.Quantity}");
                    }

                    Console.WriteLine("----------------------");

                }
                Console.ReadLine();
            }
            return hasRows;
        }
        public static void Insert(string habit)
        {
            string? info = Helpers.GetColumn(habit);
            string date = Helpers.GetDateInput();
            int quantity = Helpers.GetNumberInput($"Please insert number of {info} or another measue of your choice (no decimals)");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                $"INSERT INTO {habit}(date, {info}) VALUES('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void Delete(string habit)
        {
            GetAllRecords();
            if (tableData.Capacity > 0)
            {
                var recordID = Helpers.GetNumberInput("Type the ID of the record you want to delete.");

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"DELETE from {habit} WHERE Id = '{recordID}'";
                    int rowCount = tableCmd.ExecuteNonQuery();

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"Record with ID {recordID} doesn't exist.");
                        Delete(habit);
                    }

                    Console.WriteLine($"Record with ID {recordID} was deleted.");

                    Menu.GetUserInput();
                }
            }
            else
                Console.WriteLine("No records found.");

        }

        public static void Update(string habit)
        {
            GetAllRecords();
            string info = Helpers.GetColumn(habit);
            if (tableData.Capacity > 0)
            {
                var recordID = Helpers.GetNumberInput("Type the ID you want to update, Type 0 to go to Main Menu");
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var checkCmd = connection.CreateCommand();
                    checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habit} WHERE Id = {recordID})";
                    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (checkQuery == 0)
                    {
                        Console.WriteLine($"Record with Id {recordID} doesn't exist.");
                        Console.ReadLine();
                        connection.Close();
                        Update(habit);
                    }

                    string date = Helpers.GetDateInput();

                    int quantity = Helpers.GetNumberInput($"Insert the amount of {info} or other measure of your choice");


                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = @$"UPDATE {habit} SET date = '{date}', {info} = {quantity} WHERE Id = 
        {recordID}";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
            else
                Console.WriteLine("No records found");

        }
    }
}