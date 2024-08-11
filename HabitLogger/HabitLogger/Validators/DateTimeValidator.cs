using System.Globalization;

namespace HabitLogger;

public class DateTimeValidator : IValidator<DateTime>
{
    public (bool, DateTime) Validate(string input)
    {
        return (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result), result);
    }
}
