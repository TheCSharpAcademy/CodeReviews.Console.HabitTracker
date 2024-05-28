public class App
{
    private DatabaseManager dbManager;
    private HabitRepository habitRepo;
    private ConsoleInteraction userIneraction;

    public App(string databasePath)
    {
        dbManager = new DatabaseManager(databasePath);
    }

    public void Run()
    {
        dbManager.InitializeDatabase();

        habitRepo = new HabitRepository(dbManager);

        userIneraction = new ConsoleInteraction();

        while (true)
        {
            Options option = userIneraction.MainOptions();

            switch (option)
            {
                case Options.Insert:
                    // TODO: Add new Habit
                    Console.WriteLine("Add new Habit");
                    break;
                case Options.Update:
                    // TODO: Update previous Habit
                    Console.WriteLine("Update previous Habit");
                    break;
                case Options.Delete:
                    // TODO: Delete a habit
                    Console.WriteLine("Delete a habit");
                    break;
                case Options.ViewAll:
                    // TODO: View all habits
                    Console.WriteLine("View all habits");
                    break;
                case Options.InsertTestData:
                    // TODO: Insert test habits
                    Console.WriteLine("Insert test habits");
                    break;
                case Options.Exit:
                    Console.WriteLine("Exiting application...");
                    Environment.Exit(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            } 
        }
    }

}