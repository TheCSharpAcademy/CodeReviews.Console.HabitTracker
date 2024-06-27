namespace HabitLogger;

class Program
{
    static void Main(string[] args)
    {
        Database.CreateDatabase();
        Database.SeedDatabase();

        User user = new User();
        user.Menu();
    }
}
