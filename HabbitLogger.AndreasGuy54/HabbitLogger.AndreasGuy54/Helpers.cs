using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitLogger.AndreasGuy54
{
    internal static class Helpers
    {
        static string connectionString = @"Data Source=habit_tracker.db";
        internal static void GetUserInput()
        {
            Console.Clear();

            bool closeApp = false;

            while (!closeApp)
            {
                Console.WriteLine("\n\nMain Menu");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("------------------------------------------\n");

                int userInput;
                bool validInput = int.TryParse(Console.ReadLine().ToLower().Trim(), out userInput);

                while (!validInput || userInput < 0)
                {
                    Console.WriteLine("Enter a valid number");
                    validInput = int.TryParse(Console.ReadLine().ToLower().Trim(), out userInput);
                }

                switch (userInput)
                {
                    case 0:
                        Console.WriteLine("\nGoodbye:\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case 1:
                        ShowAllRecords();
                        break;
                    case 2:
                        InsertRecord();
                        break;
                    default:
                        break;
                }
            }
        }

        internal static void ShowAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM drinking_water";

                List<Water> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (DateTime.TryParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-UK"), DateTimeStyles.None, out DateTime date))
                        {
                            tableData.Add(
                                new Water
                                {
                                    Id = reader.GetInt32(0),
                                    Date = date,
                                    Quantity = reader.GetInt32(2)
                                }
                            );
                        }
                        else
                        {
                            Console.WriteLine($"Invalid date format found: {reader.GetString(1)}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found");
                }

                connection.Close();

                Console.WriteLine("----------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("-----------------------------------\n");
            }
        }

        internal static void InsertRecord()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert the number of glasses or other unit of measure of your choice (no decimals allowed)\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = $@"INSERT INTO drinking_water(date, quantity)
                    VALUES('{date}',{quantity})";

                insertCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to Main Menu.\n\n");
            string dateInput = Console.ReadLine();

            if (dateInput.Length == 0)
                GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-UK"), DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine("\n\nInvalid data. (Format: dd-mm-yy). Type 0 to return to main menu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine().ToLower().Trim();

            while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid Input. Enter a number");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}
