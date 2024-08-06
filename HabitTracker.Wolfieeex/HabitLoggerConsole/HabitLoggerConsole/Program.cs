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
                //var tableCmd = connection.CreateCommand();

                //tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS going_to_gym (
                //                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                //                        Date TEXT,
                //                        Sets TEXT
                //                        )";

                //tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            try
            {
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
            Console.Clear();

            Console.WriteLine("MAIN MENU:");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.WriteLine("\n0 - Close application");
            Console.WriteLine("1 - Track a new habit");
            Console.WriteLine("2 - View your existing habits");
            Console.WriteLine("3 - Update settings of a habit");
            Console.WriteLine("4 - Stop tracking a habit\n");
            Console.WriteLine("5 - Add, update, view, and delete records of the habit\n");
            
            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.Write("\nYour input: ");

            int userInput = -1;
            AssignSelectionInput(ref userInput, 0, 4);

            switch (userInput)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Thank you for using this application. See you soon!\n");
                    runApp = false;
                    break;
                case 1:
                    Console.Clear();
                    HabitCommands.InsertHabit();
                    break;
                case 2:
                    Console.Clear();
                    HabitCommands.ViewAllHabits();
                    break;
                case 3:
                    Console.Clear();
                    HabitCommands.UpdateHabit();
                    break;
                case 4:
                    Console.Clear();
                    HabitCommands.DeleteHabit();
                    break;
                case 5:
                    Console.Clear();
                    HabitRecordMenu();
                    break;
            }
        }
    }

    private static void HabitRecordMenu()
    {
        Console.Clear();

        // Adding viewing all existing habits table and input selection with appropriate data validation
        // for selecting datatable

        bool runHabitSelectionPreMenu = true;
        while (runHabitSelectionPreMenu)
        {
            bool runHabitMenu = true;
            while (runHabitMenu)
            {
                Console.Clear();

                Console.WriteLine($"YOU ARE CURRENTLY ON ... HABIT. YOU CAN ADD, UPDATE, VIEW, AND DELETE RECORDS:");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine($"{new string('-', Console.BufferWidth)}");
                Console.WriteLine("\n0 - Go back to the Main menu");
                Console.WriteLine("1 - Select another habit\n");
                Console.WriteLine("2 - Insert a record");
                Console.WriteLine("3 - View all records");
                Console.WriteLine("4 - Update a record");
                Console.WriteLine("5 - Delete a record\n");

                Console.WriteLine($"{new string('-', Console.BufferWidth)}");
                Console.Write("\nYour input: ");

                int userInput = -1;
                AssignSelectionInput(ref userInput, 0, 4);

                switch (userInput)
                {
                    case 0:
                        runHabitSelectionPreMenu = false;
                        runHabitMenu = false;
                        break;
                    case 1:
                        runHabitMenu = false;
                        break;
                    case 2:
                        Console.Clear();
                        RecordCommands.Insert();
                        break;
                    case 3:
                        Console.Clear();
                        RecordCommands.GetAllRecords(true);
                        break;
                    case 4:
                        Console.Clear();
                        RecordCommands.Update();
                        break;
                    case 5:
                        Console.Clear();
                        RecordCommands.Delete();
                        break;
                }
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