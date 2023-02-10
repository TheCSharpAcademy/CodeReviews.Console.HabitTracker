namespace yashsachdev.HabitTracker;
public class HabitRepo
{
    Habit habit = new Habit();
    public void save(Habit habit)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "INSERT INTO Habit(Habit_Name,Unit)VALUES(@Habit_Name,@Unit)";
                command.Parameters.AddWithValue("@Habit_Name", habit.Habit_Name);
                command.Parameters.AddWithValue("@Unit", habit.Unit);
                command.ExecuteNonQuery();

            }
        }
    }
    public int GetLastInsertedId()
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT last_insert_rowid()";
                int lastInsertedId = Convert.ToInt32(command.ExecuteScalar());
                return lastInsertedId;
            }
        }
    }
}
