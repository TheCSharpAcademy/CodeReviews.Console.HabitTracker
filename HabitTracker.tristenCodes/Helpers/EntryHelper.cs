namespace Helpers;
using DTO;

public static class EntryHelper
{
    public static HabitEntry GenerateHabitEntry(string userEntry)
    {
        HabitEntry entry = new();
        var isValidEntry = false;
        while (!isValidEntry)
        {
            try
            {
                var splitEntry = userEntry.Split(";");
                var habitName = splitEntry[0];
                var date = splitEntry[1];
                entry.Habit = habitName;
                entry.Date = DateHelper.ConvertStringToDateTime(date);
            }
            catch (IndexOutOfRangeException ex)
            {
                isValidEntry = false;
                Console.WriteLine(ex.Message);
                Console.WriteLine("Your entry was not split correctly with the delimiter. Try again.");
            }
            catch (Exception ex)
            {
                isValidEntry = false;
                Console.WriteLine(ex.Message);
                Console.WriteLine("Invalid entry. Try again.");
            }
        }
        return entry;
    }
}
