using static Habit_Tracker_library.CRUD;

namespace Habit_Tracker_library;

public static class Menu
{
    public static void Greeting()
    {
        Console.WriteLine("Welcome to Habit Tracker!");
        Helpers.GetDetails();
    }

    public static void MainMenu() 
    {  
        bool closeApp = false;

        do
        {
            ShowMainMenu();
            string commandInput = Console.ReadLine();
            closeApp = MainMenuOption(commandInput);
        }
        while (!closeApp);

    }

    private static void ShowMainMenu() 
    {
        Console.Clear();
        Console.WriteLine($@"MAIN MENU
------------------------------------------------------
Type 'Exit' to Close the Habit Tracker
Type 'View' to View All Records
Type 'Insert' to Insert Record
Type 'Delete' to Delete Record
Type 'Update' to Update Record
Type 'Report' to View Report");

        Console.Write("Your choice: ");
    }

    private static bool MainMenuOption(string commandInput) 
    {
        switch(commandInput.ToLower().Trim())
        {
            case "exit":
                Console.Clear();
                Console.WriteLine("Thank you for using Habit Tracker");
                return true;

            case "view":
                ViewAllRecords();
                Console.WriteLine("\n\nPress any key to get back to Main Menu...");
                Console.ReadKey();
                break;

            case "insert":
                InsertRecord();
                break;

            case "delete":
                DeleteRecord();
                break;

            case "update":
                UpdateRecord();
                break;

            case "report":
                ReportMenuChoice();
                break;

            default:
                Console.WriteLine("\nInvalid Command. Please type an option from the menu above.\n\nPress any key to continue...");
                Console.ReadKey();
                break;
        }   
        
        return false;
    }

    private static void ShowReportMenu()
    {
        Console.Clear();
        Console.WriteLine(@$"REPORT MENU

Type 'Yearly' for the one year report
Type 'Monthly' for the one month report");

        Console.Write("Your choice: ");
    }

    private static void ReportMenuChoice()
    {
        ShowReportMenu();
        string report = Helpers.GetReportType(Console.ReadLine());

        switch (report.ToLower()) 
        {
            case "yearly":
                RecordReport.YearlyReport();
                break;

            case "monthly":
                RecordReport.MonthlyReport();
                break;    
        }
    }
}
