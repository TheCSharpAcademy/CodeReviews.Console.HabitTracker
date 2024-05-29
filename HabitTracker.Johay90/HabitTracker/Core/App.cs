public class App
{
    private readonly DatabaseManager _dbManager;
    private HabitRepository _habitRepo;
    private ConsoleInteraction _userInteraction;

    public App(string databasePath)
    {
        _dbManager = new DatabaseManager(databasePath);
    }

    public void Run()
    {
        _dbManager.InitializeDatabase();

        _habitRepo = new HabitRepository(_dbManager);

        _userInteraction = new ConsoleInteraction();

        while (true)
        {
            Options option = _userInteraction.MainOptions();

            switch (option)
            {
                case Options.Insert:
                    AddNewHabit();
                    break;
                case Options.Update:
                    UpdateHabit();
                    break;
                case Options.Delete:
                    DeleteHabit();
                    break;
                case Options.ViewAll:
                    ViewAllHabits();
                    break;
                case Options.InsertTestData:
                    InsertTestHabits();
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

    private void AddNewHabit()
    {
        Habit habit = _userInteraction.AddHabit();
        _habitRepo.AddHabit(habit);
        Console.WriteLine("Habit added successfully.");
    }

    private void UpdateHabit()
    {
        Console.WriteLine("Update previous Habit - Not yet implemented.");
    }

    private void DeleteHabit()
    {
        Console.WriteLine("Delete a habit - Not yet implemented.");
    }

    private void ViewAllHabits()
    {
        List<Habit> habits = _habitRepo.GetAllHabits();
        _userInteraction.DisplayAllHabits(habits);
    }

    private void InsertTestHabits()
    {
        int amount = 10;
        _habitRepo.SeedDatabase(amount);
        Console.WriteLine($"Inserted {amount} new random records");
    }

}