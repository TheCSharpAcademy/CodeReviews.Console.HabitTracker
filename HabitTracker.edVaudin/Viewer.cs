namespace HabitTracker.edvaudin;

public static class Viewer
{
    public static void DisplayOptionsMenu()
    {
        Console.WriteLine("\nChoose an action from the following list:\n");
        Console.WriteLine("\tv - View your tracker");
        Console.WriteLine("\th - View your biggest entry for a habit");
        Console.WriteLine("\ta - Add a new entry");
        Console.WriteLine("\td - Delete an entry");
        Console.WriteLine("\tu - Update an entry");
        Console.WriteLine("\tc - Create new habit");
        Console.WriteLine("\t0 - Quit this application");
        Console.Write("\nYour option? ");
    }

    public static void DisplayTitle()
    {
        Console.WriteLine("Habit Tracker\r");
        Console.WriteLine("-------------\n");
    }

    public static void DisplayEntryTable()
    {
        List<Entry> entries = DataAccessor.GetEntries();
        string output = "\n";
        foreach (Entry entry in entries)
        {
            output += $"{entry.GetString()}\n";
        }
        Console.WriteLine(output);
    }

    public static void DisplayHighestEntry(Entry entry)
    {
        Console.WriteLine($"\nBelow is the most {entry.Measurement} you have done in one go:\n");
        Console.WriteLine(entry.GetString());
    }

    public static void ProcessInput(string userInput)
    {
        switch (userInput)
        {
            case "v":
                DisplayEntryTable();
                break;
            case "h":
                UserController.ViewHighest();
                break;
            case "a":
                UserController.AddEntry();
                break;
            case "d":
                UserController.DeleteEntry();
                break;
            case "u":
                UserController.UpdateEntry();
                break;
            case "c":
                UserController.CreateHabit();
                break;
            case "0":
                Program.SetUserFinished();
                break;
            default:
                break;
        }
    }
}
