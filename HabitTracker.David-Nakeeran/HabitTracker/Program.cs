using HabitTracker.Utilities;
using HabitTracker.Database;

namespace HabitTracker;
class Program
{
    static void Main(string[] args)
    {
        HabitTrackerHistory habitTrackerHistory = new HabitTrackerHistory();
        HabitTrackerOperationHandler habitTrackerOperationHandler = new HabitTrackerOperationHandler();
        DatabaseManager databaseManager = new DatabaseManager(habitTrackerHistory);
        Menu menu = new Menu(habitTrackerOperationHandler, databaseManager);

        databaseManager.CreateDatabaseTable();
        menu.ShowMenu();
    }
}