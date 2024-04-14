using HabitTrackerConsole.Application;
using HabitTrackerConsole.Database;
using HabitTrackerConsole.Services;

namespace HabitTracker;

class Program
{
    static void Main(string[] args)
    {
        DatabaseContext dbContext = InitializeDatabase();
        var habitService = new HabitService(dbContext);
        var logEntryService = new LogEntryService(dbContext);

        ApplicationHandler appHandler = new ApplicationHandler(habitService, logEntryService);
        appHandler.Run();  
    }

    static DatabaseContext InitializeDatabase()
    {
        string dbPath = "HabitTracker.db";
        var dbContext = new DatabaseContext(dbPath);
        var dbInitializer = new DatabaseInitializer(dbContext);
        try
        {
            dbInitializer.Initialize();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to initialize database, the application will now exit.");
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
        return dbContext;
    }
}