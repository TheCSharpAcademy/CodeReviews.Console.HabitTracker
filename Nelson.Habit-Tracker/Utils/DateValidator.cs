using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.Utils
{
    public class DateValidator : IDateValidator
    {
        readonly IConsoleInteraction _consoleInteraction;
        public string GetDateInput()
        {
            _consoleInteraction.ShowMessage("\n\nPlease insert the date: (Format: dd-MM-yyyy). Type ) to return ro main menu.");

            string dateInput = _consoleInteraction.GetUserInput();

            return dateInput;
        }
    }
}