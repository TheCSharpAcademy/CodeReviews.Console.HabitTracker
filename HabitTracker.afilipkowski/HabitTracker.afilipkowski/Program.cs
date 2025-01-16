using Database;

namespace HabitTracker;
class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        var db = new DatabaseHandler();
        while (!endApp)
        {
            UserInterface.DisplayMenu();
            endApp = UserInterface.HandleInput(db);
            Console.WriteLine("Press Enter to continue.");
            Console.ReadKey();
        }
        Console.WriteLine("Goodbye!");
    }
}











