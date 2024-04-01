using Microsoft.Data.Sqlite;
using System.Globalization;

namespace STUDY.ConsoleApp.HabitLogger;

internal class Helpers
{
    internal static void CreateDB(string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Date TEXT,
                    QUANTITY INTEGER
                    )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.\n\n");
        string dateInput = Console.ReadLine();

        if (dateInput == "0")
            Menu.GetUserInput();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy",new CultureInfo("en-US"),DateTimeStyles.None,out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine($"{message}");
        string numberInput = Console.ReadLine();

        if (numberInput == "0")
            Menu.GetUserInput();

        while(!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }
        
        int correctInput = Convert.ToInt32(numberInput);
        return correctInput;
    }
}
