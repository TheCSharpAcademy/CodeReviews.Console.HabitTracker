using Microsoft.Data.Sqlite;
using System.Globalization;

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
                    case 3:
                        DeleteRecord();
                        break;
                    case 4:
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid Command. Please type a number from 0 to 4:\n");
                        break;
                }
            }
        }

        internal static void ShowAllRecords()
        {
            Console.Clear();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand showAllCmd = connection.CreateCommand();
                showAllCmd.CommandText = $"SELECT * FROM drinking_water";

                List<Water> waters = new();

                SqliteDataReader reader = showAllCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (DateTime.TryParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-UK"), DateTimeStyles.None, out DateTime date))
                        {
                            waters.Add(
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
                foreach (Water water in waters)
                {
                    Console.WriteLine($"{water.Id} - {water.Date.ToString("dd-MM-yyyy")} - Quantity: {water.Quantity}");
                }
                Console.WriteLine("-----------------------------------\n");
                Console.WriteLine("Hit Enter/Return Key to return to Main Menu");
                Console.ReadLine();
            }
        }

        internal static void InsertRecord()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert the number of glasses or other unit of measure of your choice (no decimals allowed)\n\n");

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand insertCmd = connection.CreateCommand();
                insertCmd.CommandText = $@"INSERT INTO drinking_water(date, quantity)
                    VALUES('{date}',{quantity})";

                insertCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void UpdateRecord()
        {
            Console.Clear();
            ShowAllRecords();

            int recordId = GetNumberInput("\n\nPlease type the Id of the record you want to update");

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand selectCmd = connection.CreateCommand();
                selectCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(selectCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist \n\n");
                    connection.Close();
                    UpdateRecord();
                }

                string date = GetDateInput();
                int quantity = GetNumberInput("\n\nPlease insert the number of glasses or other unit of measure of your choice (no decimals allowed)\n\n");

                SqliteCommand updateCmd = connection.CreateCommand();
                updateCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

                updateCmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was updated\n\n");
            Console.WriteLine("Hit Enter/Return Key to return to Main Menu");
            Console.ReadLine();
            GetUserInput();
        }

        internal static void DeleteRecord()
        {
            Console.Clear();
            ShowAllRecords();

            int recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete");

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = '{recordId}'";

                int rowCount = deleteCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist \n\n");
                    DeleteRecord();
                }

                connection.Close();
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted\n\n");
            Console.WriteLine("Hit Enter/Return Key to return to Main Menu");
            Console.ReadLine();
            GetUserInput();
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
