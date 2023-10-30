namespace HabitTracker.kalsson;

public class ConsoleMessages
{
    /// <summary>
    /// Displays app information to the user when the app starts.
    /// </summary>
    public static void DisplayAppInformation()
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
    public static void ShowMainMenu()
    {
        Console.WriteLine("Main Menu\n");
        Console.WriteLine("1. Create new habit");
        Console.WriteLine("2. Read habits");
        Console.WriteLine("3. Update habit");
        Console.WriteLine("4. Delete habit");
        Console.WriteLine("5. Close");
        Console.WriteLine();
        Console.Write("Select an option: ");
    }
    
    /// <summary>
    /// Read user input for the CRUD operations.
    /// </summary>
    public static int GetUserInput()
    {
        string? userInput = Console.ReadLine();
        bool isNumericAndInRange = int.TryParse(userInput, out int output);

        while (!isNumericAndInRange || output < 1 || output > 5)
            {
            Console.WriteLine("Invalid input. Please try again.");
            Console.Write("Select an option: ");
            userInput = Console.ReadLine();
            isNumericAndInRange = int.TryParse(userInput, out output);
            }
        
        return output;
    }
}