namespace HabitTracker.frockett;
internal static class RandomGenerators
{
    static Random gen = new Random();
    static DateTime start = new DateTime(2022, 1, 1);
    static int dateRange = (DateTime.Today - start).Days;

    internal static string GetRandomDate()
    {
        DateTime nextRandomDate = start.AddDays(gen.Next(dateRange));
        return nextRandomDate.ToString("dd-MM-yy");
    }

    internal static int GetRandomQuantity()
    {
        int quantity = gen.Next(0, 20);
        return quantity;
    }
}
