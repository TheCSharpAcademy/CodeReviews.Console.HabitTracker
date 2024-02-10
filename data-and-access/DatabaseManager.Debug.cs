// For testing purposes only
#if DEBUG
namespace HabitLogger.data_and_access;

/// Class: DatabaseManager
/// Description: Provides methods to interact with the database.
/// /
public partial class DatabaseManager
{
    /// <summary>
    /// Seeds the database with initial data for testing purposes.
    /// </summary>
    private void SeedData()
    {
        bool recordsTableEmpty = IsTableEmpty("records");
        bool habitsTableEmpty = IsTableEmpty("habits");

        if (!recordsTableEmpty || !habitsTableEmpty)
        {
            return;
        }
        
        string[] habitNames = [
            "Walking", "Running", "Reading", "Meditating", "Coding", "Chocolate", "Drinking Water", "Glasses of Wine"
        ];
        string[] measurementUnits = [
            "Steps", "Meters", "Pages", "Minutes", "Hours", "Grams", "Milliliters", "Milliliters"
        ];
        
        string[] dates = GenerateRandomDates(100);
        int[] quantities = GenerateRandomQuantities(100, 1, 2000);
        
        for (int i = 0; i < habitNames.Length; i++)
        {
            const string habitQuery = "INSERT INTO habits (Name, Unit) VALUES (@name, @unit)";
            var habitParameters = new Dictionary<string, object>
            {
                { "@name", habitNames[i] },
                { "@unit", measurementUnits[i] }
            };
            
            ExecuteNonQuery(habitQuery, habitParameters);
        }
        
        for (int i = 0; i < 100; i++)
        {
            const string recordQuery = "INSERT INTO records (Date, Quantity, HabitId) VALUES (@date, @quantity, @habitId)";
            var recordParameters = new Dictionary<string, object>
            {
                { "@date", dates[i] },
                { "@quantity", quantities[i] },
                { "@habitId", GetRandomHabitId() }
            };
            
            ExecuteNonQuery(recordQuery, recordParameters);
        }
    }

    /// <summary>
    /// Checks if a specified table in the database is empty.
    /// </summary>
    /// <param name="tableName">The name of the table to check.</param>
    /// <returns>True if the table is empty, otherwise false.</returns>
    private bool IsTableEmpty(string tableName)
    {
        var query = $"SELECT COUNT(*) FROM {tableName}";
        var count = ExecuteScalar(query);
        return (long?) count == 0;
    }

    /// <summary>
    /// Generates an array of random quantities.
    /// </summary>
    /// <param name="count">The number of quantities to generate.</param>
    /// <param name="min">The minimum value for each quantity.</param>
    /// <param name="max">The maximum value for each quantity.</param>
    /// <returns>An array of randomly generated quantities.</returns>
    private int[] GenerateRandomQuantities(int count, int min, int max)
    {
        Random random = new();
        int[] quantities = new int[count];

        for (int i = 0; i < count; i++)
        {
            quantities[i] = random.Next(min, max + 1);
        }
        
        return quantities;
    }

    /// <summary>
    /// Generates an array of random dates within a specified range.
    /// </summary>
    /// <param name="count">The number of random dates to generate.</param>
    /// <returns>An array of randomly generated date strings in the format "dd-MM-yyyy".</returns>
    private string[] GenerateRandomDates(int count)
    {
        var startDate = new DateTime(2023, 7, 1);
        var endDate = new DateTime(2024, 2, 1);
        var range = endDate - startDate;
        
        var randomDatesStrings = new string[count];
        Random random = new();

        for (int i = 0; i < count; i++)
        {
            int daysToAdd = random.Next(0, (int)range.TotalDays);
            var randomDate = startDate.AddDays(daysToAdd);
            randomDatesStrings[i] = randomDate.ToString("yyyy-MM-dd");
        }

        return randomDatesStrings;
    }

    /// <summary>
    /// Returns a random habit ID.
    /// </summary>
    /// <returns>A random habit ID.</returns>
    private int GetRandomHabitId()
    {
        Random random = new();
        return random.Next(1, 9);
    }
}
#endif
