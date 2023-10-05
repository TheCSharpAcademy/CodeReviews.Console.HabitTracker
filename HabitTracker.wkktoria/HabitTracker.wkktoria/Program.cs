using System.Globalization;

namespace HabitTracker.wkktoria;

internal static class Program
{
    private static void Main(string[] args)
    {
        var path = Path.GetDirectoryName(Directory
            .GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent
            .FullName);

        var dbPath = Path.Combine(path, "data/habit-tracker.db");

        var connectionString = @"Data Source = " + dbPath;

        var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        customCulture.DateTimeFormat.DateSeparator = "-";

        Thread.CurrentThread.CurrentCulture = customCulture;


        var database = new Database(connectionString, customCulture);
        database.Initialize();

        Helpers.ShowMenu();
        Helpers.GetUserInput(database);
    }
}