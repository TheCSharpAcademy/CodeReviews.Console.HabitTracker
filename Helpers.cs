using System.Globalization;

namespace habit_logger;

class Helpers
{
    public static void GetUserMenuInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\n\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close the Application");
            Console.WriteLine("Type 1 to View All Records");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("Type 5 to Add a new habit.");
            Console.WriteLine("Type 6 to generate a report.");
            Console.WriteLine("---------------------------------\n");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    Console.WriteLine("Goodbye!");
                    closeApp = true;
                    break;
                case "1":
                    Database.ShowAllRecords(true);
                    break;
                case "2":
                    Database.InsertNewRecord();
                    break;
                case "3":
                    Database.DeleteRecord();
                    break;
                case "4":
                    Database.UpdateRecord();
                    break;
                case "5":
                    Database.AddNewHabit();
                    break;
                case "6":
                    Database.GenerateReport();
                    break;
                default:
                    Console.WriteLine("Invalid input, please choose one of the listed options.");
                    break;
            }
        }
    }

    public static Habit PickHabit()
    {
        List<Habit> habits = Database.GetAllHabits();
        Console.WriteLine("Pick a habit from the following list.");
        ListAllHabits();

        string userChoice = Console.ReadLine();
        int result;
        while (!int.TryParse(userChoice, out result) || result < 1 || result > habits.Count || habits[result - 1] == null)
        {
            Console.WriteLine("Invalid index, please try again");
            userChoice = Console.ReadLine();
        }

        return habits[result - 1];
    }

    public static void ListAllHabits()
    {
        List<Habit> habits = Database.GetAllHabits();
        if (habits.Count == 0)
        {
            Console.WriteLine("No habits found.");
            return;
        }

        for (int i = 0; i < habits.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {habits[i].Name}");
        }
    }

    public static string GetDateInput()
    {
        Console.WriteLine("\nPlease insert the date: (Format: dd-mm-yy).\nType 0 to return to main menu\nType now to use todays date.");
        string dateInput = Console.ReadLine();
        if (dateInput == "0") return "0";
        if (dateInput == "now")
        {
            DateTime today = DateTime.Today;
            return today.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
        }

        if (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid date format. Please try again.");
            return GetDateInput(); // ask again
        }

        DateTime cleanDate = DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"));

        return cleanDate.ToString("yyyy-MM-dd");
    }

    public static int GetNumberInput(string message = "Please insert the measurement of your choice (integers only)")
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();
        if (numberInput == "0") return -1;

        while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("Invalid Number, Try Again.");
            numberInput = Console.ReadLine();
            if (numberInput == "0") return -1;
        }

        return Convert.ToInt32(numberInput);
    }
}
