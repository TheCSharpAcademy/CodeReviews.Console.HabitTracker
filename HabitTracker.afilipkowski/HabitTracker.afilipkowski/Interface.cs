using System.Globalization;
using Database;

namespace HabitTracker
{
    static class UserInterface
    {
        static public void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Habit Tracker app!");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1. Display all records");
            Console.WriteLine("2. Add a new record");
            Console.WriteLine("3. Update a record");
            Console.WriteLine("4. Delete a record");
            Console.WriteLine("5. Exit the app");
        }

        static public bool HandleInput(DatabaseHandler db)
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Select an option from the menu.");
            }
            switch (choice)
            {
                case 1:
                    return false;
                case 2:
                    string? date = GetDate();
                    int amount = GetAmount();
                    db.InsertRecord(date, amount);
                    return false;
                case 3:
                    return false;
                case 4:
                    return false;
                case 5:
                    return true;
                default:
                    return false;
            }

        }
        static private string? GetDate()
        {
            string dateInput;
            Console.WriteLine("Enter the date (dd-mm-yyyy):");
            dateInput = Console.ReadLine();
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid input. Enter the date in the correct format.");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
        static private int GetAmount()
        {
            int amount;
            Console.WriteLine("Enter the number of read pages");
            while (!int.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Invalid input. Enter a number.");
            }
            return amount;
        }

    }
}
