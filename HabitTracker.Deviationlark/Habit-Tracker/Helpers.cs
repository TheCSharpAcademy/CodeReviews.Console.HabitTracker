using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public static class Helpers
    {
        public static string GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format: dd-mm-yy).");
            Console.WriteLine("Enter 0 to go back to Main Menu.");

            string? dateInput = Console.ReadLine();
            if (dateInput == "0") Menu.GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid Date. (Format: dd-mm-yy).");
                dateInput = Console.ReadLine();
                if (dateInput == "0") Menu.GetUserInput();
            }

            return dateInput;
        }

        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string? numInput = Console.ReadLine();

            if (numInput == "0") Menu.GetUserInput();

            while (!Int32.TryParse(numInput, out _) || Convert.ToInt32(numInput) < 0)
            {
                Console.WriteLine("Invalid Number. Try again.");
                numInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numInput);

            return finalInput;
        }

        public static string GetColumn(string habit)
        {
            string columnName;
            using (var connection = new SqliteConnection(CRUD.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                string sql = $"SELECT * FROM {habit} LIMIT 1;";
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i == 2)
                            {
                                columnName = reader.GetName(i);
                                return columnName;
                            }
                        }
                    }
                }
                connection.Close();
                return "";
            }
        }

    }
    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}