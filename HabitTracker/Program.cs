using HabitTracker.Data;

namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbConnection.GetConnection(true);
            Menu.ShowMenu();
        }
    }
}
