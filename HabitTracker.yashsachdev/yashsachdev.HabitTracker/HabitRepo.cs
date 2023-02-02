namespace yashsachdev.HabitTracker;
public class HabitRepo
{
    Habit habit = new Habit();
    public Habit Retrieve(int habitId)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT * FROM Habit WHERE Habit_Id = @Id";
                command.Parameters.AddWithValue("@Id", habitId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        habit = new Habit
                        {
                            Habit_Id = reader.GetInt32(0),
                            Habit_Name = reader.GetString(1),
                            Unit = reader.GetString(2)
                        };
                    }
                }
            }
        }
        return habit;

    }
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
    public Habit GetByHabitName(string Habit_Name)
    {
        Habit habit = null;
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT * FROM Habit WHERE Habit_Name = @HabitName";
                command.Parameters.AddWithValue("@HabitName", Habit_Name);
                command.ExecuteNonQuery();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        habit = new Habit
                        {
                            Habit_Id = reader.GetInt32(0),
                            Habit_Name = reader.GetString(1),
                            Unit = reader.GetString(2)
                        };
                        return habit;
                    }
                }
            }

        }
        return null;
    }
}
