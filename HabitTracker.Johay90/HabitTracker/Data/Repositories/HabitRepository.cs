using Microsoft.Data.Sqlite;

public class HabitRepository
{
    private readonly DatabaseManager _dbManager;

    public HabitRepository(DatabaseManager dbManager)
    {
        _dbManager = dbManager;
    }

    // TODO placeholder Repository method/pattern. Also think about: DeleteHabit, UpdateHabit, GetHabitById, GetAllHabits
    public void AddHabit(Habit habit)
    {
        string insertQuery = "INSERT INTO habits (name, measurement, quantity, frequency, date_created, notes, status) VALUES (@name, @measurement, @quantity, @frequency, @date_created, @notes, @status)";
        using (var connection = _dbManager.GetConnection())
        {
            SqliteCommand cmd = new SqliteCommand(insertQuery, connection);
            cmd.Parameters.AddWithValue("@name", habit.Name);
            cmd.Parameters.AddWithValue("@measurement", habit.Measurement);
            cmd.Parameters.AddWithValue("@quantity", habit.Quantity);
            cmd.Parameters.AddWithValue("@frequency", habit.Frequency);
            cmd.Parameters.AddWithValue("@date_created", habit.DateCreated);
            cmd.Parameters.AddWithValue("@notes", habit.Notes);
            cmd.Parameters.AddWithValue("@status", habit.Status);

            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}