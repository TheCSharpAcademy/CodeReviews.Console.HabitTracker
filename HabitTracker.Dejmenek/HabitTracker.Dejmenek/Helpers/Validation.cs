using System.Text.RegularExpressions;
using HabitTracker.Dejmenek.Enums;

namespace HabitTracker.Dejmenek.Helpers
{
    public class Validation
    {
        public static bool IsValidMenuOption(string? userInput) {
            if (Enum.TryParse(userInput, out MenuOptions userOption) && Enum.IsDefined(typeof(MenuOptions), userOption))
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool IsValidDateFormat(string? userDate) {
            Regex rg = new Regex("^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$");
            return rg.IsMatch(userDate ?? "");
        }

        public static bool IsValidNumber(string? userInput) {
            if (int.TryParse(userInput, out int number) && number > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool IsValidString(string? userInput) {
            return string.IsNullOrEmpty(userInput);
        }
    }
}
