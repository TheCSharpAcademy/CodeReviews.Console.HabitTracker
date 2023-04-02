
namespace yashsachdev.HabitTracker;

internal class ReportRepo
{
    public enum HabitUnit
    {
        Hrs,
        Times,
        Days,
        km,
        Ltrs
    }
    public static string GetHabitReport(List<string> units, DateTime habitDateTime)
    {
        if (units == null || units.Count == 0)
        {
            return "No habit exists.";
        }

        int totalUnits = 0;
        HabitUnit measurement = HabitUnit.Hrs;

        foreach (string unit in units)
        {
            if (string.IsNullOrEmpty(unit))
            {
                continue;
            }

            string[] parts = unit.Split(' ');

            if (parts.Length < 2)
            {
                continue;
            }

            int value;

            if (!int.TryParse(parts[1], out value))
            {
                continue;
            }

            HabitUnit unitType;

            if (!Enum.TryParse(parts[0], out unitType))
            {
                continue;
            }

            totalUnits += value;
            measurement = unitType;
        }

        if (totalUnits == 0)
        {
            return "No unit value found for habit.";
        }

        TimeSpan difference = DateTime.Today - habitDateTime;
        string timeSpent = $"{totalUnits} {measurement}";
        string duration = $"{difference.Days} day{(difference.Days == 1 ? "" : "s")}, {difference.Hours} hour{(difference.Hours == 1 ? "" : "s")}, {difference.Minutes} minute{(difference.Minutes == 1 ? "" : "s")}, and {difference.Seconds} second{(difference.Seconds == 1 ? "" : "s")}";

        return $"You have spent {timeSpent} on the habit since {habitDateTime.ToShortDateString()}. Duration: {duration}.";
    }
}