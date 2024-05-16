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
            DateTime finalDate = DateTime.MinValue;
            bool isValidDate = false;

            while (!isValidDate)
            {
                _consoleInteraction.ShowMessageTime("\n\nPlease insert the date: (Format: dd-MM-yy).");

                string dateInput = _consoleInteraction.GetUserInput();

                isValidDate = DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out finalDate);
            
                if (!isValidDate)
                {
                    _consoleInteraction.ShowMessage("\n\nPlease insert a valid date.");
                }
            }

            finalDate = finalDate.Date;

            return finalDate;
        }
    }
}