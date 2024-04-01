using System.Text;

GetMenuOption();

static void DisplayMainMenu()
{
    Console.WriteLine("\nMAIN MENU\n");
    Console.WriteLine("What would you like to do?\n");

    StringBuilder menuBuilder = new StringBuilder();
    menuBuilder.AppendLine("Type 0 to Close the Application.");
    menuBuilder.AppendLine("Type 1 to View All Records.");
    menuBuilder.AppendLine("Type 2 to Insert a Record.");
    menuBuilder.AppendLine("Type 3 to Update a Record.");
    menuBuilder.AppendLine("Type 4 to Delete a Record.");
    menuBuilder.AppendLine("-----------------------------------");

    Console.WriteLine(menuBuilder.ToString());
    Console.Write("Option: ");
}

static void GetMenuOption()
{
    Console.Clear();
    bool closeApp = false;

    while (closeApp == false)
    {
        DisplayMainMenu();
        string? input = Console.ReadLine();
        int option;

        while (!int.TryParse(input, out option) || option < 0 || option > 4)
        {
            Console.WriteLine("Invalid input. Please enter a valid option.");
            Console.Write("Option: ");
            input = Console.ReadLine();
        }

        switch (option)
        {
            case 0:
                Console.WriteLine("This application is now closing. Goodbye!");
                closeApp = true;
                break;
            case 1:
                // ViewAllRecords();
                break;
            case 2:
                // InsertRecord();
                break;
            case 3:
                // UpdateRecord();
                break;
            case 4:
                // DeleteRecord();
                break;
        }
    }
}