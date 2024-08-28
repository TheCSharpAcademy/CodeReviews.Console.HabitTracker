namespace HabitTracker;

public class Menu
{
    public static void DisplayWelcomeMessage()
    {
        Console.WriteLine("\n---------------------------------");
        Console.WriteLine("Welcome to the Habit Tracker App!");
        Console.WriteLine("---------------------------------");
    }

    public static void DisplayMenuOptions()
    {
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\n1 - To view all records, type 1 and press Enter");
        Console.WriteLine("2 - To insert a record, type 2 and press Enter");
        Console.WriteLine("3 - To update a record, type 3 and press Enter");
        Console.WriteLine("4 - To delete a record, type 4 and press Enter");
        Console.WriteLine("5 - To close the application, type 5 and press Enter");
    }
}