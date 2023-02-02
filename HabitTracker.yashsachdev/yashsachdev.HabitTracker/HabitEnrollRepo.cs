namespace yashsachdev.HabitTracker;
public class HabitEnrollRepo
{
    public HabitEnroll Retrieve(int User_Id, int Habit_Id)
    {
        HabitEnroll habitEnroll = null;
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT * FROM Habit_Enroll WHERE User_Id = @User_Id AND Habit_Id = @Habit_Id";
                command.Parameters.AddWithValue("@User_Id", User_Id);
                command.Parameters.AddWithValue("@Habit_Id", Habit_Id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        habitEnroll = new HabitEnroll
                        {
                            User_Id = reader.GetInt32(0),
                            Habit_Id = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                        };
                    }
                }

            }
        }
        return habitEnroll;
    }
    public void Save(HabitEnroll habitEnroll)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "INSERT INTO Habit_Enroll(User_Id,Habit_Id,Date)VALUES(@User_Id,@Habit_Id,@date)";
                command.Parameters.AddWithValue("@User_Id", habitEnroll.User_Id);
                command.Parameters.AddWithValue("@Habit_Id", habitEnroll.Habit_Id);
                command.Parameters.AddWithValue("@date", habitEnroll.Date);
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