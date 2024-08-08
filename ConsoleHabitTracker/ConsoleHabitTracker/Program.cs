namespace ConsoleHabitTracker;

class Program
{
    static void Main(string[] args)
    {
        DisplayMenu();
        var selection = Console.ReadKey().KeyChar;
        var endProgram = true;

        while (endProgram)
        {
            switch (selection)
            {
                case '0':
                    Console.WriteLine("\n\nClosing application");
                    endProgram = false;
                    break;
                case '1':
                    Console.WriteLine("\n\nView All Records");
                    Console.ReadLine();
                    DisplayMenu();
                    selection = Console.ReadKey().KeyChar;
                    break;
                case '2':
                    Console.WriteLine("\n\nAdd a Record");
                    Console.ReadLine();
                    DisplayMenu();
                    selection = Console.ReadKey().KeyChar;
                    break;
                case '3':
                    Console.WriteLine("\n\nDelete a Record");
                    Console.ReadLine();
                    DisplayMenu();
                    selection = Console.ReadKey().KeyChar;
                    break;
                case '4':
                    Console.WriteLine("\n\nEdit a Record");
                    Console.ReadLine();
                    DisplayMenu();
                    selection = Console.ReadKey().KeyChar;
                    break;
                default:
                    Console.WriteLine("\n\nInvalid Selection press enter to try again"); 
                    Console.ReadLine();
                    DisplayMenu();
                    selection = Console.ReadKey().KeyChar;
                    break;

            }
        }
       
    }

    static void DisplayMenu()
    {
        Console.WriteLine("What do you want to do?");
        Console.WriteLine("Type 0 to Close Application");
        Console.WriteLine("Type 1 to View all Records");
        Console.WriteLine("Type 2 to Add a record");
        Console.WriteLine("Type 3 to Delete a record");
        Console.WriteLine("Type 4 to Edit a record");
    }
}