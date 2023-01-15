namespace HabitTracker.edvaudin;

public class Program
{
    private static bool userFinished = false;
    static void Main(string[] args)
    {
        Viewer.DisplayTitle();
        UserController.InitializeDatabase();

        while (!userFinished)
        {
            Viewer.DisplayOptionsMenu();
            string userInput = UserInput.GetUserOption();
            Viewer.ProcessInput(userInput);
        }

        UserController.ExitApp();
    }

    public static void SetUserFinished() => userFinished = true;
}