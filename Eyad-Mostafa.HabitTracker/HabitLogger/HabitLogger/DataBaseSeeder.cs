using System.Data.SQLite;

public static class DatabaseSeeder
{
    private static Random _random = new();

    public static void SeedData(SQLiteConnection connection)
    {
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM drinking_water";
        long count = (long)checkCmd.ExecuteScalar();

        if (count == 0)
        {
            Console.WriteLine("Seeding data...");

            for (int i = 0; i < 100; i++)
            {
                int habitId = i+1; 
                string date = DateTime.Now.AddDays(-_random.Next(0, 365)).ToString("dd-MM-yyyy");
                int quantity = _random.Next(1, 10);

                var recordCmd = connection.CreateCommand();
                recordCmd.CommandText = "INSERT INTO drinking_water (Id, Date, Quantity) VALUES (@habitId, @date, @quantity)";
                recordCmd.Parameters.AddWithValue("@habitId", habitId);
                recordCmd.Parameters.AddWithValue("@date", date);
                recordCmd.Parameters.AddWithValue("@quantity", quantity);
                recordCmd.ExecuteNonQuery();
            }

            Console.WriteLine("Seeding complete.");
        }
    }
}
