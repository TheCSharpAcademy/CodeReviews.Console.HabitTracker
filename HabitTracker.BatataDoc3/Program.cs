using HabitTracker.BatataDoc3.db;
using HabitTracker.BatataDoc3.HabitTracker;

namespace HabitTracker.BatataDoc3
{
    internal class Program
    {
        static void Main(string[] args)
        {

            CRUD crud = new CRUD();
            crud.startDatabase();
            Console.WriteLine(@"========================
WELCOME TO HABIT TRACKER
========================");
            HabitTrackerApp ht = new HabitTrackerApp(crud);
            ht.MainMenu();


        }
    }
}