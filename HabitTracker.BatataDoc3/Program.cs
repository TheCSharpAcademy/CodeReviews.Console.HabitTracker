using HabitTracker.BatataDoc3.db;
using HabitTracker.BatataDoc3.HabitTracker;

namespace HabitTracker.BatataDoc3
{
    internal class Program
    {
        static void Main()
        {

            Crud crud = new Crud();
            bool newDb = crud.StartDatabase();

            Console.WriteLine(@"========================
WELCOME TO HABIT TRACKER
========================");
            HabitTrackerApp ht = new HabitTrackerApp(crud);
            if (newDb)
            {
                ht.PopulateDb();
            }
            ht.MainMenu();


        }
    }
}