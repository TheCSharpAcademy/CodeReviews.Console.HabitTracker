using System.Globalization;
using System.Text.RegularExpressions;
using ohshie.HabitTracker;

class Program
{
    static readonly string Dbconnection = @"Data Source=habit_tracker.db";

    public static void Main(string[] args)
    {
        Menus menu = new Menus();
        menu.MainMenu();
    }
    
    public static string DateChooser()
    {
        string date = "";
        bool goBack = false;
        while (goBack == false)
        {
            Console.Clear();
            Console.WriteLine("Choose how to enter date of entry\n" +
                              $"Press 1 to enter date automatically ({DateTime.Today.Date.ToShortDateString()})\n" +
                              $"Press 2 to choose date manually\n" +
                              "Press X to go back");
            ConsoleKey consoleKey = Console.ReadKey(true).Key;
            switch (consoleKey)
            {
                case ConsoleKey.D1: 
                    return GetDateOfEntryAuto();
                case ConsoleKey.D2:
                    return GetDateOfEntryManually();
                case ConsoleKey.X:
                    goBack = true;
                    date = "X";
                    break;
                default:
                {
                    Console.WriteLine("INVALID CHOICE, try again.\n" +
                                      "press enter to try again");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }
            }
        }

        Console.Clear();
        return date;
    }

    private static string GetDateOfEntryAuto()
    {
        DateTime dateTime = DateTime.Today.Date;

        string currentDate = dateTime.ToShortDateString();

        return currentDate;
    }

    private static string GetDateOfEntryManually()
    {
        Console.WriteLine("Input date of entry (format: dd.mm.yyyy) or enter X to go back");

        while (true)
        {
            string date = Console.ReadLine().ToUpperInvariant();
            if (date == "X") return date;

            if (DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime cleanDate) && cleanDate <= DateTime.Today)
            {
                return cleanDate.ToShortDateString();
            }
            Console.WriteLine("Looks like date your entered is wrong or you entered something that is not X. Try again");
        }
    }

    public static int GetNumberFromUser(string message)
    {
        
        int inputCleared;
        Console.WriteLine($"Enter {message}, or X to go back.\n" +
                          "remember, no decimals allowed!");
        string userInput = Console.ReadLine().ToUpperInvariant();
        while (!int.TryParse(userInput, out inputCleared) || Convert.ToInt32(userInput) <= 0)
        {
            if (userInput == "X") return -1;
            
            Console.WriteLine("Looks like you entered something that is not a number or not X. Try again");
            userInput = Console.ReadLine().ToUpperInvariant();
        }

        return inputCleared;
    }
    
    public static string GetStringFromUser(string message)
    {
        string regexSafetyCheck = @"[^a-zA-Z0-9]+";
        
        Console.WriteLine($"Enter {message}, or 0 to go back.\n" +
                          "Name should be more than 1 character in length, without spaces or any symbols like #$%");
        string userInput = Console.ReadLine();
        
        while (userInput == "0" | Regex.IsMatch(userInput,regexSafetyCheck) | userInput.Length < 1)
        {
            if (userInput == "0") return "#";
            
            Console.WriteLine("Looks like you entered something that is not allowed or not X. Try again");
            userInput = Console.ReadLine();
        }

        return userInput;
    }
}