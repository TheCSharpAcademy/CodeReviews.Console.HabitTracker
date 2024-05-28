public class App
{
    private DatabaseManager dbManager;
    private HabitRepository habitRepo;

    public App(string databasePath)
    {
        dbManager = new DatabaseManager(databasePath);
    }

    public void Run()
    {
        dbManager.InitializeDatabase();

        habitRepo = new HabitRepository(dbManager);

        Console.ReadKey();
    }

}