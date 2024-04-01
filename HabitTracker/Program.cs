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

static int GetMenuOption()
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

    return option;
}
