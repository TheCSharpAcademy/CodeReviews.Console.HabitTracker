public class ReportManager
{
    private readonly DatabaseManager _databaseManager;

    public ReportManager(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void GenerateYearlyReport(int year, bool clearAtStart = true, bool waitForInputAtTheEnd = true)
    {
        if (clearAtStart)
            Console.Clear();
        Console.WriteLine($"Yearly Report for {year}");
        Console.WriteLine("------------------------");

        var habitTypes = _databaseManager.GetHabitTypes();

        foreach (var (id, name, unit) in habitTypes)
        {
            int count = _databaseManager.GetHabitCountForYear(year, id);
            int totalQuantity = _databaseManager.GetTotalQuantityForYear(year, id);

            Console.WriteLine($"{name}:");
            Console.WriteLine($"  Times tracked: {count}");
            Console.WriteLine($"  Total {unit}: {totalQuantity}");
            Console.WriteLine();
        }

        if (waitForInputAtTheEnd)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(); 
        }
    }
}
