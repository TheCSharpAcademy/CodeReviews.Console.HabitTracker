namespace Habit_Tracker_library;

enum Months
{
    January = 1,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December
}

internal static class RecordReport
{
    
    internal static void YearlyReport()
    {
        Console.Clear();
        Console.WriteLine("Yearly Report\n");
        Console.WriteLine("Enter a year for which you want report\nType 0 to get back to Main Menu");
        string year = Validations.GetYear(Console.ReadLine());

        if (year != "0") Queries.YearlyReportQuery(year);

        else Menu.MainMenu();        
    }

    internal static void MonthlyReport() 
    {
        Console.Clear();
        Console.WriteLine("Monthly Report\n");
        Console.WriteLine("Enter a month for which you want report\nType 0 to get back to Main Menu");
        string month = Validations.GetMonth(Console.ReadLine());

        if (month != "0") Queries.MonthlyReportQuery(month);

        else Menu.MainMenu();
    }
}
