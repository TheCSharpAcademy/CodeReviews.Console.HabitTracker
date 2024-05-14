using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.Utils
{
    public class InputValidator : IInputValidator
    {
        readonly IConsoleInteraction _consoleInteraction;

        public InputValidator(IConsoleInteraction consoleInteraction)
        {
            _consoleInteraction = consoleInteraction;
        }
        
        public int ConvertToInt(string input)
        {
            int finalInput = Convert.ToInt32(input);
            return finalInput;
        }

        public string GetQualityInput()
        {
            _consoleInteraction.ShowMessage("\n\nPlease insert the quantity of measure of your choice (no decimal allowed)");

            return _consoleInteraction.GetUserInput();
        }

        public string GetNameInput()
        {
            _consoleInteraction.ShowMessage("\n\nPlease insert the name of the habit: ");

            return _consoleInteraction.GetUserInput();
        }

        public string? GetMeasurementInput()
        {
            _consoleInteraction.ShowMessage("\n\nPlease insert measure of your choice (litres, cups,...): ");

            return _consoleInteraction.GetUserInput();
        }
    }
}