namespace HabitTracker.BrozDa
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dateFormat = "dd-MM-yyyy";
            string databaseName = "habit-tracker.sqlite";

            DatabaseReader reader = new DatabaseReader(databaseName, dateFormat);
            DatabaseWriter writer = new DatabaseWriter(databaseName, dateFormat);
            InputManager inputManager = new InputManager(dateFormat);  
            OutputManager outputManager = new OutputManager(dateFormat);

            HabitTracker tracker = new HabitTracker(reader,writer,inputManager, outputManager);

            tracker.Start();
            Console.ReadLine();

        }
    }
}
