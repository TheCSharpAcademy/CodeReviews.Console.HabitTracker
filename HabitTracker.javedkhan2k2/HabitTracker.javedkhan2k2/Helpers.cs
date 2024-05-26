using System.Globalization;
using System.Text.RegularExpressions;
using HabitTracker.Models;

namespace HabitTracker;

internal class Helpers
{
    internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: yyyy-mm-dd). Try again:\n\n");

        string dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: yyyy-mm-dd). Try again:\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    internal static Habit? GetHabitId(List<Habit>? habits)
    {
        Menu.ShowAllHabits(habits);
        Console.WriteLine("Please Enter Habit Id from the above list.");
        var id = Console.ReadLine();
        while (!Int32.TryParse(id, out _) || Convert.ToInt32(id) <= 0 ||
                (habits.Find(h => h.Id == Convert.ToInt32(id)) == null))
        {
            Console.WriteLine("Invalid Input or Invalid Habit Id!!!\nPlease Enter a Habit Id from the above list.");
            id = Console.ReadLine();
        }
        return habits.FirstOrDefault(h => h.Id == Convert.ToInt32(id));
    }

    internal static int GetHabitLogId(List<HabitLog>? habitLogs)
    {
        Menu.ShowAllHabitLogs(habitLogs);

        Console.WriteLine("Please Enter HabitLog Id from the above list.");
        var id = Console.ReadLine();
        while (!Int32.TryParse(id, out _) || Convert.ToInt32(id) <= 0 ||
                (habitLogs.Find(h => h.Id == Convert.ToInt32(id)) == null))
        {
            Console.WriteLine("Invalid Input or Invalid HabitLog Id!!!\nPlease Enter HabitLog Id from the above list.");
            id = Console.ReadLine();
        }
        return Convert.ToInt32(id);
    }

    internal static int GetIntegerValue(string message)
    {
        Console.WriteLine(message);
        var inputValue = Console.ReadLine();
        while (!Int32.TryParse(inputValue, out _) || Convert.ToInt32(inputValue) <= 0)
        {
            Console.WriteLine("Invalid Input!! " + message);
            inputValue = Console.ReadLine();
        }
        return Convert.ToInt32(inputValue);
    }

    internal static string GetStringInput(string message)
    {
        Console.WriteLine(message);
        string inputValue = Console.ReadLine();
        while(inputValue == null)
        {
            Console.WriteLine($"Input cannot be empty. {message}");
            inputValue = Console.ReadLine();
        }
        return inputValue;
    }

    internal static int GetUserInput()
    {
        string? userInput = Console.ReadLine();
        while (userInput == null || !Regex.IsMatch(userInput, "^[0-7]$"))
        {
            Console.WriteLine("Invalid option selected. Please try again.");
            userInput = Console.ReadLine();
        }
        return Convert.ToInt32(userInput);
    }
}