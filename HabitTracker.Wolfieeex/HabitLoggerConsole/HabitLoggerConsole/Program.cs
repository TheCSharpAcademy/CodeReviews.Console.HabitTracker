using Microsoft.Data.Sqlite;
using HabitLoggerConsole;
using System;

namespace HabitLoggerConsole;

public class Program
{
    public static string connectionString { private set; get; } = @"Data Source=HabitLoggerConsole.db";


    static void Main(string[] args)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS going_to_gym (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Sets TEXT
                                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            MainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    static void MainMenu()
    {
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.Clear();

            Console.WriteLine("MAIN MENU:");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.WriteLine("\n0 - Close application");
            Console.WriteLine("1 - View all records");
            Console.WriteLine("2 - Insert a record");
            Console.WriteLine("3 - Delete a record");
            Console.WriteLine("4 - Update a record\n");
            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.Write("\nYour input: ");

            int userInput = -1;
            AssignSelectionInput(ref userInput, 0, 4);

            switch (userInput)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Thank you for using this application. See you soon!\n");
                    closeApp = true;
                    break;
                case 1:
                    Console.Clear();
                    SqlCommands.GetAllRecords(true);
                    break;
                case 2:
                    Console.Clear();
                    SqlCommands.Insert();
                    break;
                case 3:
                    Console.Clear();
                    SqlCommands.Delete();
                    break;
                case 4:
                    Console.Clear();
                    SqlCommands.Update();
                    break;
            }
        }
    }

    internal static bool AssignSelectionInput(ref int input, int rangeMin, int rangeMax, char? skipSelection = null)
    {
        bool inputChecksNotComplete = true;
        while (inputChecksNotComplete)
        {
            string? Input = Console.ReadLine();
            string reason = "";

            if (string.IsNullOrEmpty(Input))
            {
                ReenterLine("Your input is either invalid or empty.");
                continue;
            }

            int numericOutput = -1;
            if (!int.TryParse(Input, out numericOutput))
            {
                if (Input.ToLower() == skipSelection.ToString().ToLower())
                {
                    return true;
                }
                ReenterLine("Your input must be a valid integer number.");
                continue;
            }

            else
            {
                if (numericOutput < rangeMin || numericOutput > rangeMax)
                {
                    ReenterLine($"Your input must be a number ranging from {rangeMin} to {rangeMax}.");
                    continue;
                }
            }

            input = numericOutput;
            inputChecksNotComplete = false;
        }
        return false;

        void ReenterLine(string reason)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine($"{new string(' ', Console.BufferWidth)}");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(reason + " Please try again to select your option: ");
        }
    }
}