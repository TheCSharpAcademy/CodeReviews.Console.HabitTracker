using System.Globalization;
using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.Utils
{
    public class DateValidator : IDateValidator
    {
        readonly IConsoleInteraction _consoleInteraction;

        public DateValidator(IConsoleInteraction consoleInteraction)
        {
            _consoleInteraction = consoleInteraction;
        }
        
        public DateTime GetDateInput()
        {
            _consoleInteraction.ShowMessage("\n\nPlease insert the date: (Format: dd-MM-yyyy). Type 0 to return to main menu.");

            string dateInput = _consoleInteraction.GetUserInput();

            DateTime finalDate = DateTime.ParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            finalDate = finalDate.Date;

            return finalDate;
        }
    }
}