
namespace STUDY.ConsoleApp.HabitLogger;

internal class Menu
{
    internal static void GetUserInput()
    {
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("------------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    CRUD.GetAllRecords();
                    break;
                case "2":
                    CRUD.Insert();
                    break;
                case "3":
                    CRUD.Delete();
                    break;
                case "4":
                    CRUD.Update();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
}
