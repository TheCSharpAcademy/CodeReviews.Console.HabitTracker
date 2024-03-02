namespace HabitTracker;

internal class Menu
{
    internal static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nPlease select an option!");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert a Record.");
            Console.WriteLine("Type 3 to Delete a Record.");
            Console.WriteLine("Type 4 to Update a Record.");
            Console.WriteLine("-----------------------------\n");

            string command = Console.ReadLine()!;

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    Helpers.GetAllRecords();
                    break;
                case "2":
                    Helpers.Insert();
                    break;
                case "3":
                    Helpers.Delete();
                    break;
                case "4":
                    Helpers.Update();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }

    }
}
