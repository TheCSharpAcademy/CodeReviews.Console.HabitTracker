using HabitLoggerConsole.Models;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

namespace HabitLoggerConsole;

public class Program
{
    public static string connectionString { private set; get; } = @"Data Source=HabitLoggerConsole.db";

    static char exitChar = 'E';

    static bool runDeveloperSeedTest = true;

    static void Main(string[] args)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                connection.Close();
            }
            try
            {
                if (runDeveloperSeedTest)
                {
                    DataSeed.CreateTestRecord();
                }

                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                Console.SetWindowPosition(0, 0);

                MainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while trying to connect to a database");
                Console.WriteLine(ex.Message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred while running the program");
            Console.WriteLine(ex.Message);
        }
    }

    static void MainMenu()
    {
        bool runApp = true;
        while (runApp)
        {
            Console.WriteLine("MAIN MENU:");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.WriteLine("\n0 - Track a new habit");
            Console.WriteLine("1 - View your existing habits");
            Console.WriteLine("2 - Update settings of a habit");
            Console.WriteLine("3 - Stop tracking a habit");
            Console.WriteLine("4 - Add, update, view, and delete records of the habit\n");

            Console.WriteLine($"{new string('-', Console.BufferWidth)}\n");
            InsertExitPrompt(exitChar, exitApplication: true);

            int userInput = -1;
            bool exitProgram = AssignSelectionInput(ref userInput, 0, 4, skipSelection: exitChar);

            if (exitProgram)
            {
                Console.Clear();
                Console.WriteLine("Thank you for using this application. See you soon!\n");
                break;
            }

            switch (userInput)
            {
                case 0:
                    Console.Clear();
                    HabitCommands.InsertHabit();
                    break;
                case 1:
                    Console.Clear();
                    HabitCommands.ViewAllHabits();
                    break;
                case 2:
                    Console.Clear();
                    HabitCommands.UpdateHabit();
                    break;
                case 3:
                    Console.Clear();
                    HabitCommands.DeleteHabit();
                    break;
                case 4:
                    Console.Clear();
                    HabitRecordMenu();
                    break;
            }
            Console.Clear();
        }
    }
    private static void HabitRecordMenu()
    {
        bool runHabitSelectionPreMenu = true;
        while (runHabitSelectionPreMenu)
        {
            Console.Clear();

            string? habitName = "";

            bool shouldExit = HabitCommands.SelectUpdatingHabit(out habitName, exitChar);
            if (shouldExit)
            {
                break;
            }

            bool runHabitMenu = true;
            while (runHabitMenu)
            {
                Console.Clear();

                Console.Write($"You have opened ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{HabitCommands.TableNameToDisplayableFormat(habitName).ToLower()}");
                Console.ResetColor();
                Console.WriteLine(" table database.");
                Console.WriteLine("From here you can create, read, update and delete its records.");
                Console.WriteLine("\nWhat would you like to do?\n");
                Console.WriteLine($"{new string('-', Console.BufferWidth)}");
                Console.WriteLine("\n0 - Select another habit\n");
                Console.WriteLine("1 - Insert a record");
                Console.WriteLine("2 - View all records");
                Console.WriteLine("3 - Update a record");
                Console.WriteLine("4 - Delete a record");
                Console.WriteLine("5 - Generate a report\n");

                Console.WriteLine($"{new string('-', Console.BufferWidth)}\n");
                InsertExitPrompt(exitChar);

                int userInput = -1;
                shouldExit = AssignSelectionInput(ref userInput, 0, 5, skipSelection: exitChar);
                if (shouldExit)
                {
                    runHabitSelectionPreMenu = false;
                    runHabitMenu = false;
                    break;
                }

                switch (userInput)
                {
                    case 0:
                        runHabitMenu = false;
                        break;
                    case 1:
                        Console.Clear();
                        RecordCommands.Insert(habitName);
                        break;
                    case 2:
                        Console.Clear();
                        RecordCommands.GetAllRecords(true, habitName);
                        break;
                    case 3:
                        Console.Clear();
                        RecordCommands.Update(habitName);
                        break;
                    case 4:
                        Console.Clear();
                        RecordCommands.Delete(habitName);
                        break;
                    case 5:
                        Console.Clear();
                        //RecordCommands.GenerateReport(habitName);
                        ReportClass.GenerateReportV2(habitName);
                        break;
                }
            }
        }
    }
    internal static bool AssingNameInput(ref string input, string failCommand, char? exitChar = null, bool excludeSymbols = false)
    {
        while (true)
        {
            string? userInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(userInput))
            {
                if (excludeSymbols)
                {
                    if (userInput.Any(ch => (!char.IsLetterOrDigit(ch)) && ch != ' '))
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write($"{new string(' ', Console.BufferWidth)}");
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write("No special characters are allowed in your input! Please, try again: ");
                        continue;
                    }
                }
                if (userInput.ToLower() == exitChar.ToString().ToLower())
                {
                    return true;
                }
                userInput = Regex.Replace(userInput, @"^\s+", "");
                userInput = Regex.Replace(userInput, @"\s+$", "");
                userInput = Regex.Replace(userInput, @"\s\s+", " ");
                userInput = Regex.Replace(userInput, @"\s\s+", " ");
                userInput = userInput.ToLower();
                userInput = userInput.Replace(" ", "_");
                input = userInput;
                break;
            }
            else
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write($"{new string(' ', Console.BufferWidth)}");
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(failCommand);
            }
        }
        return false;
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
    internal static bool AssingDoubleInput(ref double input, double rangeMin, double rangeMax, char? skipSelection = null)
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

            double numericOutput = -1;
            if (!double.TryParse(Input, out numericOutput))
            {
                if (Input.ToLower() == skipSelection.ToString().ToLower())
                {
                    return true;
                }
                ReenterLine("Your input must be a valid number.");
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
    internal static void InsertExitPrompt(char exitChar, bool backMenuAlteration = false, bool exitApplication = false)
    {
        string returnToWhere = backMenuAlteration ? "previous menu" : "main menu";
        if (exitApplication)
        {
            Console.WriteLine($"Optionally, insert '{exitChar}' to exit the program.");
        }
        else
        {
            Console.WriteLine($"Optionally, insert '{exitChar}' to return to the {returnToWhere}.");
        }
        Console.Write("\nYour option: ");
    }
}
