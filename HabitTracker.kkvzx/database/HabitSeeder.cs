namespace HabitTracker.kkvzx.database;

public static class HabitSeeder
{
    private const int MinDay = 1;
    private const int MaxDay = 28;

    private const int MinMonth = 1;
    private const int MaxMonth = 12;
    
    private const int MinYear = 2000;
    private const int MaxYear = 2200;
    
    public static List<HabitModel> Seed(int elementsToSeed)
    {
        var random = new Random();
        List<HabitModel> records = new();

        for (var i = 0; i < elementsToSeed; i++)
        {
            var date = $"{random.Next(MinDay, MaxDay):00}-{random.Next(MinMonth, MaxMonth):00}-{random.Next(MinYear, MaxYear):0000}";
            var quantity = random.Next(0, 100);

            records.Add(new HabitModel(date, quantity));
        }

        return records;
    }
}