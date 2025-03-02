using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.StressedBread
{
    internal class Helpers
    {
        // Instance of DatabaseService to interact with the database
        DatabaseService databaseService = new();

        // Method to validate a non-empty string input from the user
        internal string? ValidateString(string text)
        {
            Console.WriteLine(text);
            string? result = Console.ReadLine();

            // Loop until a valid non-empty string is provided
            while (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Your input is not valid!");
                result = Console.ReadLine();
            }
            return result;
        }

        // Method to validate a positive integer input from the user
        internal int ValidateInt(string text)
        {
            Console.WriteLine(text);
            string? result = Console.ReadLine();

            // Loop until a valid positive integer is provided
            while (string.IsNullOrEmpty(result) || !int.TryParse(result, out int num) || num < 0)
            {
                Console.WriteLine("Your input needs to be a positive whole number!");
                result = Console.ReadLine();
            }
            return int.Parse(result);
        }

        // Method to validate a date input in the format dd/MM/yyyy
        internal string? ValidateDate(string text)
        {
            Console.WriteLine(text);
            string? result = Console.ReadLine();
            string formatedDate = "dd/MM/yyyy";

            // Loop until a valid date in the specified format is provided
            while (string.IsNullOrEmpty(result) || !DateTime.TryParseExact(result, formatedDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Your input is not in a correct format!");
                result = Console.ReadLine();
            }

            // Convert the date to the format yyyy-MM-dd for SQL database
            DateTime date = DateTime.ParseExact(result, formatedDate, CultureInfo.InvariantCulture);
            string formattedDate = date.ToString("yyyy-MM-dd");

            return formattedDate;
        }

        // Method to get date input from the user, with an option to use the current date
        internal string? GetDateInput()
        {
            Console.WriteLine("Do you wish to input current date? Press Y for yes or N for no");
            ConsoleKeyInfo input = Console.ReadKey();
            switch (input.Key)
            {
                case ConsoleKey.Y:
                    Console.WriteLine();
                    return InputCurrentDate();
                case ConsoleKey.N:
                    return ValidateDate("\nInput date in dd/mm/yyyy format.");
                default:
                    return ValidateDate("\nInvalid input! Insert date manually in dd/mm/yyyy format.");
            }
        }

        // Method to get the current date in the format yyyy-MM-dd for SQL database
        internal string? InputCurrentDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        // Method to display data from the database based on a SQL command
        internal void DisplayData(string commandText, List<SqliteParameter>? parameters = null)
        {
            using (SqliteDataReader reader = databaseService.ExecuteRead(commandText, parameters))
            {
                // Read and display each row of the result set
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        var columnValue = reader.GetValue(i);
                        // Format date columns to dd/MM/yyyy
                        if (columnValue is string dateString && DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                        {
                            columnValue = date.ToString("dd/MM/yyyy");
                        }
                        Console.Write($@"| {columnName}: {columnValue} ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
