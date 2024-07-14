namespace IbraheemKarim.HabitLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseOperations.CreateHabitTrackerDatabaseIfItDoesNotExist();
            DatabaseOperations.CreateTableIfItDoesNotExist();
            Menu.StartMenu();
        }
    }
}
