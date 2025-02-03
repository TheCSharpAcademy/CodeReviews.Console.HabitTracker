using HabitTracker.KamilKolanowski.Data;

namespace HabitTracker.KamilKolanowski;

class Program
{
    static void Main(string[] args)
    {
        DatabaseInitializer.InitializeDb();
    }
}