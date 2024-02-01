

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Habit_Logger;
class Program
{
    private static bool closeApp = false;
    static void Main(string[] args)
    {
        DataBase.Connect();
        DataBase.Seed();
        Console.WriteLine("++++++++++++Welcome To runnign Habit Tracker++++++++++++");
       
        while (closeApp == false)
        {
           
            StartMenu();
        }
    }

    public static void StartMenu()
    {
        Console.WriteLine("\n\nMAIN MENU");
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
                DataBase.GetAllRecords();
                break;
            case "2":
                DataBase.Insert();
                break;
            case "3":
                DataBase.Delete();
                break;
            case "4":
                DataBase.Update();
                break;
            default:
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}