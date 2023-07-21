using Microsoft.Data.Sqlite;

namespace Habit_Tracker_library;

internal static class Helpers
{
    
    internal static string GetDateInput(string message = "")
    {
        Console.WriteLine($"Please insert the date in specified format dd-MM-yyyy\n{message}");
        string dateInput = Validations.GetDate(Console.ReadLine());

        if(dateInput == "0") Menu.MainMenu();

        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        int finalInput = Validations.IsValidNumber(Console.ReadLine());
        return finalInput;
    }

    internal static Tuple<string, string> GetHabit()
    {
        Console.WriteLine("\nWhat habit do you want to track?");
        string habit = Console.ReadLine();

        while(string.IsNullOrEmpty(habit)) 
        {
            Console.WriteLine("\nYou have to enter some habit: ");
            habit = Console.ReadLine();
        }

        Console.WriteLine("\nEnter unit of measure for this habit");
        string measure = Console.ReadLine();

        while (string.IsNullOrEmpty(measure))
        {
            Console.WriteLine("\nYou have to enter some measure: ");
            measure = Console.ReadLine();
        }

        return new Tuple<string, string>(habit, measure);
    }

    internal static void GetDetails()
    {
        var (habit, measure) = Helpers.GetHabit();
        
        Crud.Measure = measure;
        Crud.CreateTable();
    }

    internal static string GetReportType(string record)
    {
        while (string.IsNullOrEmpty(record) || (record != "yearly" && record != "monthly" && record != "Yearly" && record != "Monthly"))
        {
            Console.Clear();
            Console.WriteLine("\nInvalid Command. Enter a valid report type\n");
            record = Console.ReadLine();
        }
        return record;
    }

    internal static int PossibleUpdate(int recordId)
    {
        using (var connection = new SqliteConnection(Crud.ConnectionString))
        {
            connection.Open();
            var tableCheck = connection.CreateCommand();

            //proverava da li u tabeli postoji red sa datim id-em ako postoji vraca taj red EXISTS u SQL proverava da li podupit vraca neku vrednost (vraca 1 ako podupit vraca neku vrednost 
            tableCheck.CommandText = $"SELECT EXISTS(SELECT 1 FROM '{Crud.Habit}' WHERE Id = {recordId})";

            // vraca 1. kolonu 1. reda dobijenog iz prethodnog upita, ako takav red ne postoji vraca null
            int checkQuery = Convert.ToInt32(tableCheck.ExecuteScalar());

            return checkQuery;
        }
    }

    internal static string GetMonthName(string month)
    {
        switch (Convert.ToInt32(month))
        {
            case 1:
               return Months.January.ToString();

            case 2:
                return Months.February.ToString();

            case 3:
                return Months.March.ToString();

            case 4:
                return Months.April.ToString();

            case 5:
                return Months.May.ToString();

            case 6:
                return Months.June.ToString();

            case 7:
                return Months.July.ToString();

            case 8:
                return Months.August.ToString();

            case 9:
                return Months.September.ToString();

            case 10:
                return Months.October.ToString();

            case 11:
                return Months.November.ToString();

            case 12:
                return Months.December.ToString();

            default:
                return "";
        }
    }

    
}