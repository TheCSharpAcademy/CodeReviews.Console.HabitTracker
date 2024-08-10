using HabitLoggerConsole.Models;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

namespace HabitLoggerConsole;

internal class HabitCommands
{
    private static char exitChar = 'E';
    internal static void InsertHabit()
    {
        bool exitFunction = false;
        string name = "";
        string nameOfTheTrackingVariable = "";
        MeasurementType measurementType = 0;

        Console.WriteLine("Choose a name for the habit you want to start tracking.");
        InsertExitPrompt(exitChar);

        while (true)
        {
            exitFunction = Program.AssingNameInput(ref name, "Your name must not be empty. Please, try inserting the habit's name again: ", exitChar: exitChar, excludeSymbols: true);
            if (exitFunction)
            {
                return;
            }

            using (var connection = new SqliteConnection(Program.connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{name}'";

                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write($"{new string(' ', Console.BufferWidth)}");
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write("A habit with this name already exists. Please choose another habit to track: ");
                    continue;
                }

                connection.Close();
            }
            break;
        }
        

        Console.Clear();

        Console.WriteLine("Choose how you would like to measure your habit from the units listed below. Type in an index number.\n");
        Console.WriteLine($"{new string('-', Console.BufferWidth)}");
        Console.WriteLine("\n");

        int listCounter = 0;
        foreach (MeasurementType type in Enum.GetValues(typeof(MeasurementType)))
        {
            listCounter++;
            string measurementName = MeasurementUnits.MeasurementFullName[type];
            measurementName = measurementName[0].ToString().ToUpper() + measurementName.Substring(1);
            Console.WriteLine($"{listCounter} - {measurementName}");
        }

        Console.WriteLine("\n");
        Console.WriteLine($"{new string('-', Console.BufferWidth)}");
        Console.WriteLine("\n");
        InsertExitPrompt(exitChar);

        int measurementTypeLength = Enum.GetNames(typeof(MeasurementType)).Length;
        int userInput = 0;
        exitFunction = Program.AssignSelectionInput(ref userInput, 1, measurementTypeLength, skipSelection: exitChar);
        if (exitFunction) 
        { 
            return; 
        }
        measurementType = (MeasurementType)(userInput - 1);

        Console.Clear();
        Console.WriteLine("Please type in a name for what you are going to be tracking.");
        InsertExitPrompt(exitChar);

        exitFunction = Program.AssingNameInput(ref nameOfTheTrackingVariable, "Your name must not be empty. Please, try inserting variable's name again: ", exitChar: exitChar, excludeSymbols: true);

        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"CREATE TABLE IF NOT EXISTS '{name}' (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, '{nameOfTheTrackingVariable}' TEXT, '{measurementType}' TEXT)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    internal static void ViewAllHabits()
    {
        throw new NotImplementedException();
    }
    internal static void UpdateHabit()
    {
        throw new NotImplementedException();
    }
    internal static void DeleteHabit()
    {
        throw new NotImplementedException();
    }

    private static void InsertExitPrompt(char exitChar)
    {
        Console.WriteLine($"Optionally, insert '{exitChar}' to return to the main menu.");
        Console.Write("\nYour option: ");
    }
}
