using HabitTracker.Business;

string? input;
bool shouldEndProgram = false;
HabitService habitService = new HabitService();

habitService.initializeDatabase();

while (shouldEndProgram != true)
{
    Console.Clear();
    Console.WriteLine("Welcome!");
    Console.WriteLine("Select the option from the menu:\n");
    Console.WriteLine("-------------------------------------");
    Console.WriteLine("MAIN MENU\n");
    Console.WriteLine("S - Show your entries");
    Console.WriteLine("C - Create an entry");
    Console.WriteLine("D - Delete an entry");
    Console.WriteLine("U - Update an entry");
    Console.WriteLine("I - To filter the habits by activity");
    Console.WriteLine("-------------------------------------");
    Console.WriteLine("\nE - Exit the application\n");

    input = Console.ReadLine().ToLower();

    switch (input)
    {

        case "s":
            habitService.ShowDatabase();
            Console.WriteLine("\nPress a button to go back to the main menu");
            Console.ReadLine();
            break;
        case "c":
            habitService.AddEntry(habitService.habitReader.IngressHabit());
            Console.WriteLine("\nPress a button to go back to the main menu");
            Console.ReadLine();
            break;
        case "d":
            habitService.DeleteEntry();
            Console.WriteLine("\nPress a button to go back to the main menu");
            Console.ReadLine();
            break;
        case "u":
            habitService.UpdateEntry();
            Console.WriteLine("\nPress a button to go back to the main menu");
            Console.ReadLine();
            break;
        case "i":
            habitService.ShowReports();
            break;
        case "e":
            shouldEndProgram = true;
            Console.WriteLine("\nByebye!");
            break;
        default:
            break;

    }
}
