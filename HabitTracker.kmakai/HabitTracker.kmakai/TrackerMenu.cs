namespace HabitTracker.kmakai;

public class TrackerMenu
{
    public static string MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Habit Tracker!");
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Add a habit");
        Console.WriteLine("2. Remove a habit");
        Console.WriteLine("3. Manage habits");
        Console.WriteLine("4. View Habits");
        Console.WriteLine("0. Exit");

        return GetOption();
    }

    public static string ManageHabitsMenu(List<Habit> list)
    {
        Console.Clear();
        Console.WriteLine("Please select the habit:");
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {list[i].Name}");
        }
        Console.WriteLine("0. Back");

        return GetOption();

    }

    public static string HabitMenu(Habit habit)
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {habit.Name} habit!");
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Add Entry");
        Console.WriteLine("2. Edit Entry");
        Console.WriteLine("3. Delete Entry");
        Console.WriteLine("4. View all Entries");
        Console.WriteLine("0. Back");

        return GetOption();
    }

    public static string GetOption(string? message = null)
    {
        if (message != null) Console.WriteLine(message);
        Console.Write("\nEnter your input: ");

        string? option = Console.ReadLine();

        while (option == null || option == "")
        {
            Console.WriteLine("Please enter a valid input!");
            Console.Write("\nEnter your input: ");
            option = Console.ReadLine();
        }

        return option;
    }
}