using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.Utils
{
    public class InputValidator : IInputValidator
    {
        readonly IConsoleInteraction _consoleInteraction;
        
        public int ConvertToInt(string input)
        {
            int finalInput = Convert.ToInt32(input);
            return finalInput;
        }

        public string GetQualityInput()
        {
            _consoleInteraction.ShowMessage("\n\nPlease insert measure of your choice (no decimal allowed)\n\n");

            return _consoleInteraction.GetUserInput();
        }
    }
}