using HabitTrackerLibrary;
internal class HabitTrackerMain
{
    static DatabaseQueries databaseQueries = new DatabaseQueries();
    private static bool exit = false;
    private static void Main(string[] args)
    {
        while (!exit)
        {
            Console.WriteLine("\nWelcome to the HabitTracker app!\n");
            databaseQueries.CreateDatabaseIfNotExists();
            ShowMenu();
            string menuChoice = Validator.ValidateMenuChoice(Validator.mainMenuChoices);
            SwitchMenuChoice(menuChoice);
        }
    }
    private static void ShowMenu()
    {
        Console.WriteLine("Please enter one of the following options:\n" +
            "===============================\n" +
            "\t'c' - Create a habit\n" +
            "\t'v' - View habit\n" +
            "\t'u' - Update habit\n" +
            "\t'i' - Insert into habit\n" +
            "\t'd' - Delete from habit\n" +
            "\t'r' - Create a report\n" +
            "===============================\n" +
            "'exit' - Close the application\n" +
            "===============================\n" +
            "Your choice: \n");
    }

    private static void SwitchMenuChoice(string menuUserChoice)
    {
        switch (menuUserChoice)
        {
            case "c":
                Options.CreateHabit();
                break;
            case "v":
                Options.ViewHabit();
                break;
            case "u":
                Options.UpdateHabit();
                break;
            case "i":
                Options.InsertIntoHabit();
                break;
            case "d":
                Options.DeleteFromHabit();
                break;
            case "r":
                ShowReportsMenu();
                string reportsMenuUserChoice = Validator.ValidateMenuChoice(Validator.reportsMenuChoices);
                SwitchReportsMenuChoice(reportsMenuUserChoice);    
                break;
            case "exit":
                exit = true;
                break;
        }
    }
    private static void ShowReportsMenu()
    {
        Console.WriteLine("Which report would you like to generate?\n" +
            "===============================\n" +
            "\t'1' - Total in a given timespan\n" +
            "\t'2' - Amount of times a habit occurred in a given timespan\n" +
            "\t'3' - Average in a given timespan\n" +
            "\t'4' - Five greatest\n" +
            "===============================\n" +
            "'menu' - Go back to main menu\n" +
            "===============================\n" +
            "Your choice: \n");
    }
    private static void SwitchReportsMenuChoice(string reportsMenuChoice)
    {
        switch (reportsMenuChoice)
        {
            case "1":
                Options.GenerateReportTotal();
                break;
            case "2":
                Options.GenerateReportTotalAmount();
                break;
            case "3":
                Options.GenerateReportAverage();
                break;
            case "4":
                Options.GenerateReportFiveGreatest();
                break;
            case "menu":
                break;
        }
    }
}
