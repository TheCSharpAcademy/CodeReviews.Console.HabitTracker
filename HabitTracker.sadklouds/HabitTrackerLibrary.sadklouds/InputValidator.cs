using System.Globalization;

namespace HabitTrackerLibrary.sadklouds
{
    public static class InputValidator
    {
        public static bool DateValidator(string userDate)
        {
            bool output = false;
            if (DateTime.TryParseExact(userDate, "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime date))
            {
                output = true;
            }
            return output;
        }
    }
}
