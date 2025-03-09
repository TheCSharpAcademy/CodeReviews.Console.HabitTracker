namespace HabitTracker.tristenCodes.Helpers;
public static class DateHelper
{
    public static string ConvertDateToString(DateTime date)
    {
        return date.ToString("MM/dd/yyyy");
    }

    public static DateTime ConvertStringToDateTime(string str)
    {
        return DateTime.ParseExact(str, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    }
}
