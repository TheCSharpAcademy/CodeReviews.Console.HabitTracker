using HabitTracker.joshluca98;

var db = new Database(@"Data Source=HabitTracker.db");

UserMenu();

void UserMenu()
{
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.Clear();
        Console.WriteLine("\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine("------------------------------------------\n");

        string commandInput = Console.ReadLine();

        switch (commandInput)
        {
            case "0":
                Console.Clear();
                Console.WriteLine("Terminating..");
                Environment.Exit(0);
                break;
            case "1":
                db.GetAllRecords();
                Console.WriteLine("Press ENTER to continue..");
                Console.ReadLine();
                break;
            case "2":
                db.Insert();
                break;
            case "3":
                db.Delete();
                break;
            case "4":
                db.Update();
                break;
            default:
                Console.Clear();
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}