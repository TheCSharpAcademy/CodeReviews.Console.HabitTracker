namespace Nelson.Habit_Tracker.DataAccess
{
    public interface IDatabaseInitializer
    {
        void InitializeDatabase();
        void GetFromDatabase();
        void InsertToDatabase(DateTime date, string name, string measure, int quantity);
        void UpdateToDatabase(DateTime date, string name, string measure, int quantity);
        void DeleteFromDatabase(int ID);
    }
}