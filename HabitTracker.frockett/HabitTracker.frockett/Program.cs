
namespace HabitTracker.frockett;

internal class Program
{

    static void Main(string[] args)
    {
        dbOperations db = new dbOperations();
        UserInput input = new UserInput();

        db.SeedTestData();
        input.GetUserInput();
    }


}
