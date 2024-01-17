
namespace HabitTracker.frockett;

internal class Program
{
    static void Main(string[] args)
    {
        DbOperations db = new DbOperations();
        UserInput input = new UserInput();

        db.SeedTestData();
        input.GetUserInput();
    }


}
