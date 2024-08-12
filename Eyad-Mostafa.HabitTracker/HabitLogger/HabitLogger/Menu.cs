namespace HabitLogger;

internal static class Menu
{
    internal static void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Main Menu\n");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Type 0 To Close Application.");
            Console.WriteLine("Type 1 To View All Records.");
            Console.WriteLine("Type 2 To Insert Record.");
            Console.WriteLine("Type 3 To Delete Record");
            Console.WriteLine("Type 4 To Update Record");
            Console.WriteLine("Type 5 To Quantity In Specific Day");
            Console.WriteLine("----------------------------------");
            GetMainMenuOption();
        }
    }

    private static void GetMainMenuOption()
    {
        switch (Console.ReadLine()?.Trim())
        {
            case "0":
                Environment.Exit(0);
                break;
            case "1":
                DataBaseManager.ViewRecords();
                break;
            case "2":
                DataBaseManager.InsertRecord();
                break;
            case "3":
                DataBaseManager.DeleteRecord();
                break;
            case "4":
                DataBaseManager.UpdateRecord();
                break;
            case "5":
                DataBaseManager.QuantityInSpecificDay();
                break;
            default:
                Console.WriteLine("Please Enter a valid numeric value.");
                GetMainMenuOption();
                break;
        }
    }
}
