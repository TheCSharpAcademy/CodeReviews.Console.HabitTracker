using System.Globalization;
using Microsoft.Data.Sqlite;

public class HabitRepository
{
    private readonly DatabaseManager _dbManager;

    public HabitRepository(DatabaseManager dbManager)
    {
        _dbManager = dbManager;
    }

    public void AddHabit(Habit habit)
    {
        string insertQuery = "INSERT INTO habits (name, measurement, quantity, frequency, date_created, notes, status) VALUES (@name, @measurement, @quantity, @frequency, @date_created, @notes, @status)";
        using (var connection = _dbManager.GetConnection())
        {
            try
            {
                connection.Open();
                using (var cmd = new SqliteCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@name", habit.Name);
                    cmd.Parameters.AddWithValue("@measurement", habit.Measurement);
                    cmd.Parameters.AddWithValue("@quantity", habit.Quantity);
                    cmd.Parameters.AddWithValue("@frequency", habit.Frequency);
                    cmd.Parameters.AddWithValue("@date_created", habit.DateCreated);
                    cmd.Parameters.AddWithValue("@notes", habit.Notes);
                    cmd.Parameters.AddWithValue("@status", habit.Status);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
        }
    }

    public void SeedDatabase(int amount)
    {
        string[] names = { "Drink Water", "Exercise", "Read Book", "Meditate", "Write Journal" };
        string[] measurements = { "glasses", "minutes", "pages", "sessions", "entries" };
        string[] frequencies = { "daily", "weekly", "monthly" };
        string[] notes  = { "Stay hydrated", "Keep fit", "Expand knowledge", "Calm mind", "Reflect on the day" };
        string[] statuses = { "Complete", "Ongoing", "Not Started" };

        for (int i = 0; i < amount; i++)
        {
            Habit habit = new Habit
            {
                Name = names[Random.Shared.Next(0, names.Length)],
                Measurement = measurements[Random.Shared.Next(0, measurements.Length)],
                Quantity = Random.Shared.Next(1, 100),
                Frequency = frequencies[Random.Shared.Next(0, frequencies.Length)],
                DateCreated = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Notes = notes[Random.Shared.Next(0, notes.Length)],
                Status = statuses[Random.Shared.Next(0, statuses.Length)]
            };

            AddHabit(habit);
        }
    }

    // TODO: Implement other repository methods (DeleteHabit, UpdateHabit, GetHabitById, GetAllHabits)

}