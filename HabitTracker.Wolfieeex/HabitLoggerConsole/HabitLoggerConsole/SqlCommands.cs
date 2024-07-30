using Microsoft.Data.Sqlite;
using HabitLoggerConsole;

namespace HabitLoggerConsole;

internal class SqlCommands
{
    internal static void GetAllRecords()
    {
        throw new NotSupportedException();
    }
    internal static void Insert()
    {
        string date = GetDateInput();

        // Add functionality for user's input for number of sets performed this day with numeric data validation:

        Random random = new Random();
        int numberOfSets = random.Next(31);

        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO going_to_gym(Date, Sets) VALUES('{date}', '{numberOfSets}')";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    internal static void Delete()
    {
        throw new NotSupportedException();
    }
    internal static void Update()
    {
        throw new NotSupportedException();
    }

    private static string GetDateInput()
    {
        Console.WriteLine("Please insert the date of the operation (Format that is accepted: dd-mm-yyyy");
        Console.WriteLine("Or, select 0 and press enter to return to Main Menu.");
        Console.Write("\nYour input: ");

        string? dateInput = Console.ReadLine();

        // Add data validation for date format presented and availability to enter 0 to discontinue this operation.

        return dateInput;
    }
}
