namespace Nelson.Habit_Tracker.DataAccess
{
    public interface IDatabaseInitializer
    {
        void InitializeDatabase();
        void InsertToDatabase(string date, string name, int quantity);
    }
}