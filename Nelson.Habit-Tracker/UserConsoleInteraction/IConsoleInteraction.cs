namespace Nelson.Habit_Tracker.UserConsoleInteraction
{
    public interface IConsoleInteraction
    {
        void DisplayMenu();
        void ShowMessage(string message);
        void ShowMessageTime(string message);
        string GetUserInput();
    }
}