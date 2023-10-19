namespace HabitTracker.kalsson;

public class ConsoleMessages
{
    /// <summary>
    /// Displays app information to the user when the app starts.
    /// </summary>
    public static void AppInformation()
    {
        string appName = "Application name: Habit Tracker";
        string appVersion = "Version: 1.0.0";
        string appAuthor = "Created by: Jonas Karlsson";
        
        Console.WriteLine($"{appName}");
        Console.WriteLine($"{appVersion}");
        Console.WriteLine($"{appAuthor}");

        for (int i = 0; i < appName.Length; i++)
            {
            Console.Write("*");
            }
        
        Console.WriteLine();
        Console.WriteLine();
    }

    /// <summary>
    /// Display available CRUD options to the user.
    /// </summary>
    public static void ShowMenu()
    {
        // Display menu
    }
    
    /// <summary>
    /// Read user input for the CRUD operations.
    /// </summary>
    public static void GetUserInput()
    {
        // Get and validate user input
    }
}