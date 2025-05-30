using Microsoft.Data.Sqlite;

namespace Habit_Logger.Services
{
    internal class Helpers
    {

        internal static bool IsTableEmpty(string tableName)
        {
            using (var connection = new SqliteConnection(Data.Database.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT COUNT(*) FROM {tableName}";
                    long count = (long)command.ExecuteScalar();

                    return count == 0;
                }
            }
        }

        internal static void SeedData()
        {
            bool recordsTableEmpty = IsTableEmpty("progress");
            bool habitsTableEmpty = IsTableEmpty("habits");

            if (!recordsTableEmpty || !habitsTableEmpty)
                return;

            string[] habitNames = { "Walking", "Biking", "Daily Pushups", "Reading", "Water Consumption" };
            string[] habitUnits = { "Miles", "Miles", "Repetitions", "Pages", "Ounces" };
            string[] dates = GenerateRandomDates(100);
            int[] quantities = GenerateRandomQuantities(100, 0, 200);

            using (var connection = new SqliteConnection(Data.Database.ConnectionString))
            {
                connection.Open();

                for (int i = 0; i < habitNames.Length; i++)
                {
                    var insertSql = "INSERT INTO habits (Name, MeasurementUnit) VALUES (@Name, @MeasurementUnit);";
                    var command = new SqliteCommand(insertSql, connection);
                    command.Parameters.AddWithValue("@Name", habitNames[i]);
                    command.Parameters.AddWithValue("@MeasurementUnit", habitUnits[i]);

                    command.ExecuteNonQuery();
                }

                for (int i = 0; i < 100; i++)
                {
                    var insertSql = "INSERT INTO progress (Date, Quantity, HabitId) VALUES (@Date, @Quantity, @HabitId);";
                    var command = new SqliteCommand(insertSql, connection);
                    command.Parameters.AddWithValue("@Date", dates[i]);
                    command.Parameters.AddWithValue("@Quantity", quantities[i]);
                    command.Parameters.AddWithValue("@HabitId", GetRandomHabitId());

                    command.ExecuteNonQuery();
                }
            }
        }

        internal static int[] GenerateRandomQuantities(int count, int min, int max)
        {
            Random random = new Random();
            int[] quantities = new int[count];

            for (int i = 0; i < count; i++)
            {
                quantities[i] = random.Next(min, max + 1);
            }

            return quantities;
        }

        internal static string[] GenerateRandomDates(int count)
        {
            DateTime startDate = new DateTime(2025, 1, 1);
            TimeSpan range = DateTime.Today - startDate;

            string[] randomDateStrings = new string[count];
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                int daysToAdd = random.Next(0, (int)range.Days);
                DateTime randomDate = startDate.AddDays(daysToAdd);
                randomDateStrings[i] = randomDate.ToString("MM-dd-yyyy");
            }

            return randomDateStrings;
        }

        internal static int GetRandomHabitId()
        {
            Random random = new Random();
            return random.Next(1, 6);
        }
    }
}
