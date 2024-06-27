using System.Globalization;

namespace HabitLogger;

class User
{
    public void Menu()
    {
        bool endApp = false;
        PrintMainMenu();

        while (!endApp)
        {
            string? userInput = Console.ReadLine();
            
            switch (userInput)
            {
                case "0":
                    Console.WriteLine("Exiting application. Goodbye.");
                    Console.ReadLine();
                    endApp = true;
                    Environment.Exit(0);
                    break;
                 case "1":
                    Database.SearchRecords();
                    break; 
                case "2":
                    Database.ViewRecords();
                    break;
                case "3":
                    Database.Insert();
                    break;
                case "4":
                    Database.Update();
                    break;
                case "5":
                    Database.Delete();
                    break;
                case "6":
                    Database.DeleteDatabase();
                    break;
                default:
                    Console.WriteLine("Invalid input. Please select an option from the menu.");
                    break;
            }
        }
    }

    public static void PrintMainMenu()
    {
        Console.WriteLine("Please select an option below or press 0 to exit:");
        Console.WriteLine("\nMENU\n");
        Console.WriteLine("1. Search Records");
        Console.WriteLine("2. View All Records");
        Console.WriteLine("3. Insert Record");
        Console.WriteLine("4. Update Record");
        Console.WriteLine("5. Delete Record");
        Console.WriteLine("6. Delete Database");
        Console.WriteLine("--------------------------------------------------------\n");
    }

    internal static string GetDate()
    {
        string? userInput = GetString("\nPlease insert a date: (Format: dd-mm-yy).");

        while (!DateTime.TryParseExact(userInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            userInput = GetString("Invalid date. (Format: dd-mm-yy).");
        }
        return userInput;
    }

    internal static string GetString(string message)
    {
        Console.WriteLine(message);
        bool validInput = false;
        string? userInput = Console.ReadLine().ToLower();

        while (!validInput)
        {
            if (userInput == null || int.TryParse(userInput, out _))
            {
                Console.WriteLine("Invalid input. Please input a habit.");
                userInput = Console.ReadLine();
            }
            else
            {
                validInput = true;
            }
        }

        return userInput;
    }

    internal static string GetNumber(string message)
    {
        Console.WriteLine(message);
        string? userInput = Console.ReadLine();
        bool validInput = false;

        while (!validInput)
        {
            if (userInput == null || !int.TryParse(userInput, out _))
            {
                Console.WriteLine("Please input a valid integer.");
                userInput = Console.ReadLine();
            }
            else
            {
                validInput = true;
            }
        }

        return userInput;
    }
}