using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

namespace Habit_Traker
{
    class Program
    {
        static string connectionString = @"Data Source=Habit-Tracker.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = 
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Menu();
        }

        static void Menu(string previousError = "")
        {
            Console.Clear();
            Console.WriteLine("Please choose an option (1-4, 0 to exit)\n");
            Console.WriteLine("1. View All Records.\n2. Insert Record.\n3. Delete Record.\n4. Update Record.\n\n0. Exit.\n");

            Console.WriteLine(previousError);

            string option = GetUserInput("Please write your option and press Enter: ");

            switch (option)
            {
                case "0":
                    Exit();
                    break;
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    Update();
                    break;
                default:
                    Menu("Wrong input.");
                    break;
            }
        }

        static string GetUserInput(string message)
        {
            string? input = null;
            while (input == null) {
                Console.Write("\n" + message);
                input = Console.ReadLine();
            }
            return input;
        }

        static void Exit()
        {
            Console.Clear();
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }

        private static void Insert()
        {
            Console.Clear();

            string date = DateInput();
            int quantity = GetNumberInput("Amount of times you drank water: ");
            if (quantity == -1)
                Menu();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine("\nHabit logged successfully! Press any key to go to main menu.");
            Console.ReadKey();
            Menu();
        }

        internal static string DateInput(string error = "")
        {
            Console.WriteLine(error + "\n");
            Console.WriteLine("Enter date in this format (enter 0 to return to main menu): dd-MM-yyyy");

            string? input = Console.ReadLine();

            if (input == "0")
                Menu();

            if (DateTime.TryParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return input;
            }
            else
            {
                return DateInput("Date was not correct. Try again.");
            }
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine("\n-1 to go to main menu.\n");
            Console.Write(message);
            string? number = Console.ReadLine();
            if (number == null)
                return GetNumberInput("You did not enter anything.\n" + message);
            if (int.TryParse(number, out int result))
                return result;
            else
                return GetNumberInput("Your input was not a number.\n" + message);
        }

        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM drinking_water";

                List<DrinkingWater> list = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            list.Add(new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                                Quantity = reader.GetInt32(2),
                            });
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing date: {reader.GetString(1)} - {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Database is empty.");
                }

                connection.Close();

                foreach (var data in list)
                {
                    Console.WriteLine($"{data.Id} - {data.Date.ToString("dd-MM-yyyy")} - Quantity: {data.Quantity}");
                }
            }
        }


        public class DrinkingWater
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int Quantity { get; set; }
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var id = GetNumberInput("Enter the id of the record you want to delete (-1 to go to main menu).");

            if (id == -1)
            {
                Menu();
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{id}'";
                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with {id} doesnt exist. Press any key to retry.");
                    Console.ReadKey();
                    Delete();
                }

                connection.Close();

                Console.Clear();
                GetAllRecords();

                Console.WriteLine($"\nRecord with id {id} was deleted. Press any key to continue.");
                Console.ReadKey();
                Menu();
            }
        }

        private static void ViewAllRecords()
        {
            GetAllRecords();
            Console.WriteLine("\nPress any key to return to the menu.");
            Console.ReadKey();
            Menu();
        }

        private static void Update()
        {
            Console.Clear();
            GetAllRecords();

            var id = GetNumberInput("\nEnter the id of the record that you wish to update: ");
            Console.WriteLine();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {id})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (checkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with id {id} was not found. Press any key to try again.");
                    Console.ReadKey();
                    Update();
                }

                

                string date = DateInput();

                int quantity = GetNumberInput("\nEnter the number of water glasses: ");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {id}";

                tableCmd.ExecuteNonQuery();
                connection.Close();

                Console.Clear();
                GetAllRecords();
                Console.WriteLine($"\nRecord with id {id} was updated successfully. Press any key to return to the menu.");
                Console.ReadKey();
                Menu();
            }
        }
    }
}