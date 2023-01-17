using System.Text;
using HabitLogger;
using HabitLoggerLibrary;

RunApplication();

static void RunHabitLogger()
{
    using var connection = new HabitLoggerConnection();
    bool isDone = false;
    while (!isDone)
    {
        DisplayMenu();
        int option = ParseOption();
        bool continueApplication = ExecuteOption(option, connection);

        isDone = !continueApplication;
    }
}

static bool ExecuteOption(int option, HabitLoggerConnection connection)
{
    if (connection == null)
    {
        return false;
    }

    switch (option)
    {
        case (int)Options.ExitApplication:
            return false;
        case (int)Options.ReadLogs:
            ReadLogs(connection);
            break;
        case (int)Options.AddNewLog:
            AddNewLog(connection);
            break;
        case (int)Options.DeleteLog:
            DeleteLog(connection);
            break;
        case (int)Options.UpdateLog:
            UpdateLog(connection);
            break;
        default:
            Console.WriteLine("Invalid option.");
            Console.WriteLine("Must choose one of the options given.");
            break;
    }
    return true;
}

static void UpdateLog(HabitLoggerConnection connection)
{
    Console.WriteLine("Updating log...");
    Console.WriteLine();
    int id = GetId();
    int cupsOfWater = GetCupsOfWater();
    bool isSuccess = connection.UpdateLog(id, cupsOfWater);

    if (isSuccess)
    {
        Console.WriteLine($"Updated log of id {id} with {cupsOfWater} cups of water.");
    } else
    {
        Console.WriteLine("Update was not successful.");
    }
    Console.WriteLine();
}

static void DeleteLog(HabitLoggerConnection connection)
{
    Console.WriteLine("Delete log...");
    Console.WriteLine();
    int id = GetId();
    bool isSuccess = connection.DeleteLog(id);
    if (isSuccess)
    {
        Console.WriteLine($"Log of id {id} was successfully deleted.");
    } else
    {
        Console.WriteLine($"Unable to delete a log of id {id}.");
    }

    Console.WriteLine();
}

static int GetId()
{
    Console.Write("Type id of the log: ");
    string? input = Console.ReadLine();
    int id;

    while (string.IsNullOrEmpty(input) || !int.TryParse(input, out id))
    {
        Console.Write("Invalid input. Type a valid id: ");
        input = Console.ReadLine();
    }
    return id;
}

static void AddNewLog(HabitLoggerConnection connection)
{
    Console.WriteLine("Adding new log...");
    Console.WriteLine();
    int cupsOfWater = GetCupsOfWater();
    bool isSuccess = connection.CreateLog(cupsOfWater);
    
    if (isSuccess)
    {
        Console.WriteLine("New log was successfully added.");
    } else
    {
        Console.WriteLine("Unable to create new log.");
    }
    Console.WriteLine();
}

static int GetCupsOfWater()
{
    Console.Write("Type the number of cups: ");

    string? input = Console.ReadLine();
    int cupsOfWater;


    while (string.IsNullOrEmpty(input) || !int.TryParse(input, out cupsOfWater))
    {
        Console.Write("Invalid input. Type a valid number of cups: ");
        input = Console.ReadLine();
    }
    return cupsOfWater;
}

static void ReadLogs(HabitLoggerConnection connection)
{
    Console.WriteLine("Reading all logs...");
    var logs = connection.GetAllLogs();

    if (logs.Count == 0)
    {
        Console.WriteLine("There are no records in the logs.");
        return;
    }

    var sbLogs = new StringBuilder();
    foreach (var log in logs)
    {
        sbLogs.AppendLine($"log id = {log.Item1}, cups of water = {log.Item3}, date added = {log.Item2}");
    }

    Console.WriteLine("The following are the logs: ");
    Console.WriteLine();
    Console.WriteLine(sbLogs.ToString());
}


static int ParseOption()
{
    Console.Write("Type an option: ");

    string? input = Console.ReadLine();
    int option = -1;

    while (!ValidOption(input, ref option))
    {
        Console.WriteLine("Invalid input. Must be one of the options given.");
        Console.WriteLine("Displaying menu again...");
        Console.WriteLine();
        DisplayMenu();
        Console.Write("Type an option: ");
        input = Console.ReadLine();
    }
    return option;

}

static bool ValidOption(string? input, ref int option)
{
    return !string.IsNullOrEmpty(input) && int.TryParse(input, out option)
        && option >= 0 && option <= 4;
}

static void DisplayExit()
{
    Console.WriteLine("Thank you for using Habit Logger.");
    Console.WriteLine("Application is closing. Press any key...");
    Console.ReadKey();
}

static void DisplayMenu()
{
    Console.WriteLine("Choose one of the options below:");
    Console.WriteLine("Type 0: Exit Application");
    Console.WriteLine("Type 1: Show all logs");
    Console.WriteLine("Type 2: Add a new log");
    Console.WriteLine("Type 3: Delete a log");
    Console.WriteLine("Type 4: Update a log");
    Console.WriteLine();

}

static void DisplayIntroduction()
{
    Console.WriteLine("Welcome to Habit Logger.");
    Console.WriteLine("Here you can track how much water you drink.");
    Console.WriteLine("----------------------");
    Console.WriteLine();
}

static void RunApplication()
{
    DisplayIntroduction();
    RunHabitLogger();
    DisplayExit();
}