namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseHelpers.InitializeDataBase();
            DatabaseHelpers.CreateTableOfHabits();
            DatabaseHelpers.LoadHabits();
            Menu.GetUserInput();
        }
    }
}
