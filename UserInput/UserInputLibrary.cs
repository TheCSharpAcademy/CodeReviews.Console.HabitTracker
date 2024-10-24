
namespace UserInput
{

    public class UserInputLibrary
    {
        public static bool IsDateValid(string? date)
        {
            DateTime validDate;

            if (DateTime.TryParse(date, out validDate))
            {
                if (validDate.Year <= DateTime.Now.Year)
                {
                    if (validDate.Month > 1 || validDate.Month < 12)
                    {
                        int daysInMonth = DateTime.DaysInMonth(validDate.Year, validDate.Month);
                        if (validDate.Day <= daysInMonth)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            return false;
        }
    }
}