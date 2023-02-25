using DataBaseLibrary;
using HabitsLibrary;
using ScreensLibrary;

internal class Program
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    static string mainTableName = "HabitsTable";
    static DataBaseCommands dbCommands = new();
    static MainTable mainTable = new(mainTableName, connectionString);
    static Screen screen = new(mainTable);
    private static void Main(string[] args)
    {

        dbCommands.Initialization(mainTableName);
        MainMenu();
    }

    static void MainMenu()
    {
        Console.Clear();
        bool closeApp = false;
        bool invalidCommand = false;
        while (!closeApp)
        {
            Console.Clear();
            Console.WriteLine("HABIT TRACKER");
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View all Habits and select one.");
            Console.WriteLine("Type 2 to Insert a new Habit.");
            Console.WriteLine("Type 3 do Delete a Habit.");
            Console.WriteLine("Type 4 to Update a Habit.");
            Console.WriteLine("----------------------------------------");
            if (invalidCommand)
            {
                Console.Write("Invalid Command. Please choose one of the commands above");
            }
            Console.Write("\n");
            string? commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0": closeApp = true; break;
                case "1": screen.ViewAll(mainTableName); break;
                case "2": screen.Insert(mainTableName); break;
                case "3": screen.Delete(mainTableName); break;
                case "4": screen.Update(mainTableName); break;
                default:
                    invalidCommand = true;
                    break;
            }
        }
        Console.Clear();
        Console.WriteLine("\nGoodbye!");
        Environment.Exit(0);
    }
}