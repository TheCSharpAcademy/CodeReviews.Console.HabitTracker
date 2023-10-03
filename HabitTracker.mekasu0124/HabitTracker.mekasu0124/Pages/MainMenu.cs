using HabitTracker.Services;

namespace HabitTracker;

public class MainMenu
{
    public static void ShowMenu()
    {
        Console.WriteLine("What Would You Like To Do?");
        
        List<string> options = new()
        {
            "1 - Track New Habit",
            "2 - Edit Current Habit",
            "3 - Delete Current Habit",
            "4 - View Currently Tracked Habits",
            "0 - Quit Application"
        };

        foreach (string line in options)
        {
            Console.WriteLine(line);
        }
        
        Console.Write("Your Selection: ");
        string userInput = Console.ReadLine();
        userInput = Helpers.ValidateMainMenu(userInput);

        switch (userInput)
        {
            case "1":
                Habits.NewHabitEntry();
                break;
            
            case "2":
                Habits.SelectEditToHabit();
                break;
            
            case "3":
                Habits.DeleteHabit();
                break;

            case "4":
                Habits.ViewAllHabits();
                break;
            
            case "0":
                Console.WriteLine("Exiting Application...");
                Environment.Exit(0);
                break;
            
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Error Invalidating Menu Option Switch Case.");
                break;
        }
    }
}