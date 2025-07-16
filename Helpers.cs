using System.Globalization;
using System.Text.RegularExpressions;

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
                    ShowAllRecords(true);
                    break;
                case "2":
                    InsertNewRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "5":
                    AddNewHabit();
                    break;
                case "6":
                    ShowReport();
                    break;
                default:
                    Console.WriteLine("Invalid input, please choose one of the listed options.");
                    break;
            }
        }
    }

    private static void ShowReport()
    {
        Habit habit = PickHabit();
        var report = Database.GenerateReport(habit);

        // no keys in the dictionary meaning no data returned.
        if (report.Count == 0)
        {
            Console.WriteLine("No data for habit could not be found.");
            return;
        }

        Console.WriteLine(@$"
            {habit.Unit} : {report["TotalQuantity"]}
            Total Record: {report["RecordCount"]}
            Average per record: {report["AverageQuantity"]} {habit.Unit}
            Minimum: {report["MinQuantity"]}
            Maximum: {report["MaxQuantity"]}
            First record: {report["FirstRecord"]}
            Last Record: {report["LastRecord"]}
        ");

        Console.ReadKey();
    }

    private static void UpdateRecord()
    {
        Console.Clear();
        if (!Database.HasHabits())
        {
            Console.WriteLine("There are no habits to update results of.");
            return;
        }
        ShowAllRecords();

        var recordId = GetNumberInput("Please type the Id of the record you want to update or type 0 to go back to Main Menu");
        if (recordId == -1) return;

        if (!Database.RecordExists(recordId))
        {
            Console.WriteLine($"Record with ID: {recordId} does not exist.");
            UpdateRecord();
            return;
        }
        string date = GetDateInput();
        if (date == "0") return;
        int quantity = GetNumberInput();

        Database.UpdateRecord(recordId, date, quantity);
    }

    private static void DeleteRecord()
    {
        if (!Database.HasHabits())
        {
            Console.WriteLine("There are no habits to delete records from.");
            return;
        }

        Console.Clear();
        ShowAllRecords();

        var recordId = GetNumberInput("Please type the Id of the record you want to delete or type 0 to go back to Main Menu");

        if (recordId == 0) return;

        if (!Database.RecordExists(recordId))
        {
            Console.WriteLine($"Record with ID: {recordId} does not exist.");
            DeleteRecord();
            return;
        }

        Database.DeleteRecord(recordId);
        Console.WriteLine($"Record with Id {recordId} deleted.");
    }

    private static void InsertNewRecord()
    {
        if (!Database.HasHabits())
        {
            Console.WriteLine("There are no habits to insert into.");
            return;
        }

        Habit habit = PickHabit();

        string date = GetDateInput();
        if (date == "0") return;
        int quantity = GetNumberInput();

        Database.InsertRecord(habit.Id, date, quantity);
    }

    private static void ShowAllRecords(bool stopTerminal = false)
    {
        if (!Database.HasHabits())
        {
            Console.WriteLine("There are no habits to show records of.");
            return;
        }
        List<string> records = Database.GetAllRecords();

        foreach (string record in records)
        {
            Console.WriteLine(record);
        }

        if (stopTerminal) Console.ReadKey();
    }

    private static bool IsValidHabitName(string name)
    {
        // This regex ensures that habit name:
        // > Starts with a letter
        // > Contains only letters
        // > No numbers, special symbols (except _ and -)
        return Regex.IsMatch(name, @"^[a-zA-Z][a-zA-Z _-]*$");
    }

    public static void AddNewHabit()
    {
        Console.Clear();

        Console.WriteLine("Currently logged habits:");
        ListAllHabits();
        var habits = Database.GetAllHabits();

        Console.WriteLine("Provide the table name for the new habit.");
        string newHabit = Console.ReadLine();

        while (!IsValidHabitName(newHabit) || habits.Any(h => h.Name.Contains(newHabit)))
        {
            Console.WriteLine("Habit name invalid or habit already exists.");
            newHabit = Console.ReadLine();
        }

        Console.WriteLine("Please provide the unit name for the new habit");
        string unit = Console.ReadLine();

        while (unit.Trim().Length == 0 || string.IsNullOrEmpty(unit))
        {
            Console.WriteLine("Unit name needs to be at least 1 letter long.");
            unit = Console.ReadLine();
        }

        Database.AddHabit(newHabit, unit);
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
