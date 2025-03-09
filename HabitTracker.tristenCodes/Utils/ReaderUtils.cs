using HabitTracker.tristenCodes.Helpers;
using HabitTracker.tristenCodes.Services;

public static class ReaderUtils
{

    public static void DisplayRows(DBService dbService)
    {
        var reader = dbService.GetAllEntries();

        if (!reader.HasRows)
        {
            Console.WriteLine("No rows to display.");
            System.Threading.Thread.Sleep(5000);
            return;
        }

        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var habitName = reader.GetString(1);
            var occurences = reader.GetInt32(2);
            var date = reader.GetDateTime(3);
            Console.WriteLine($"Id: {id}, Habit Name: {habitName}, Date: {DateHelper.ConvertDateToString(date)}, Occurences: {occurences}");
        }
        Console.WriteLine("Press enter key to go back to main menu...");
        Console.ReadLine();

    }
}