using Microsoft.Data.Sqlite;
namespace HabitTracker;
class Program
{
    // Create a data connectionString
    static void Main(string[] args)
    {
        // Create table
        ShowMenu();
    }
    static void ShowMenu()
    {
        Console.Clear();
        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("\n\nMain Menu");
            Console.WriteLine("\nWhat would you like to do");
            Console.WriteLine("\nType 0 to close application");
            Console.WriteLine("Type 1 to View all Records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete all Record");
            Console.WriteLine("Type 4 to Update all Record");
            Console.WriteLine("--------------------------------");

            string? readResult = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(readResult))
            {
                Console.WriteLine("Please make a selection");
                readResult = Console.ReadLine();
            }

            switch (readResult)
            {
                case "0":
                    Console.WriteLine("\n Goodbye \n");
                    closeApp = true;
                    break;
                case "1":
                    Console.WriteLine("View all records");
                    break;
                case "2":
                    Console.WriteLine("Insert record");
                    break;
                case "3":
                    Console.WriteLine("Delete record");
                    break;
                case "4":
                    Console.WriteLine("Update record");
                    break;
                default:
                    Console.WriteLine("Invalid entry. Please type a number from 0 to 4. \n");
                    break;
            }
        }
    }
}

