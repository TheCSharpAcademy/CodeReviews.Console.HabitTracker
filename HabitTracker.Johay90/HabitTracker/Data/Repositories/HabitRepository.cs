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

    // TODO: Implement other repository methods (DeleteHabit, UpdateHabit, GetHabitById, GetAllHabits)

}