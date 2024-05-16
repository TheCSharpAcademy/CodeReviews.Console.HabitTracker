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

        public int GetQualityInput()
        {
            _consoleInteraction.ShowMessageTime("\n\nPlease insert the quantity of measure of your choice (no decimal allowed)");

            string quality = _consoleInteraction.GetUserInput();
            int finalInput = 0;

            while (!int.TryParse(quality, out finalInput) || finalInput < 1)
            {
                _consoleInteraction.ShowMessageTime("\n\nPlease insert the quantity of measure of your choice (no decimal allowed)");
                quality = _consoleInteraction.GetUserInput();
            }

            return finalInput;
        }

        public string GetNameInput()
        {
            _consoleInteraction.ShowMessageTime("\n\nPlease insert the name of the habit: ");

            return _consoleInteraction.GetUserInput();
        }

        public string? GetMeasurementInput()
        {
            _consoleInteraction.ShowMessageTime("\n\nPlease insert measure of your choice (litres, cups,...): ");

            return _consoleInteraction.GetUserInput();
        }
    }
}