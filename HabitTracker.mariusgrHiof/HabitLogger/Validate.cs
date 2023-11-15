using System.Globalization;

namespace HabitLogger
{
    public static class Validate
    {
        public static bool IsValidateDate(string date)
        {
            return DateTime.TryParseExact(date, "dd/MM/yyyy HH:mm", new CultureInfo("nb-NO"), DateTimeStyles.None, out _);
        }

        public static bool IsValidNumber(string number)
        {
            return int.TryParse(number, out _);
        }

        public static bool IsValidString(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }
    }
}