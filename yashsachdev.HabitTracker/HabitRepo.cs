namespace yashsachdev.HabitTracker;
public class HabitRepo
{
    private Habit habit = new Habit();

    public void Save(Habit habit)
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
    public int CountofHabit(string Email, string habitName)
    {
        HabitEnrollRepo habitEnroll = new HabitEnrollRepo();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            {
                cnn.Open();
                SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM Habit WHERE Habit_Name = @habitname", cnn);
                cmd.Parameters.AddWithValue("@habitname", habitName);
                var habitCount = Convert.ToInt32(cmd.ExecuteScalar());
                return habitCount;
            }
        }
        return 0;
    }
    public string GetHabitName(int habit_ID)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand habitCommand = new SqliteCommand())
            {
                habitCommand.Connection = cnn;
                habitCommand.CommandText = "SELECT Habit_Name FROM Habit WHERE Habit_Id = @habitId";
                habitCommand.Parameters.AddWithValue("@habitId", habit_ID);
                var res = habitCommand.ExecuteScalar();
                if (res == null)
                {
                    Console.WriteLine("No data returned");
                    return string.Empty;
                }

                var habitName = Convert.ToString(res);
                return habitName;
            }
        }
    }
    public int GetHabitId(string habitName)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand habitCommand = new SqliteCommand())
            {
                habitCommand.Connection = cnn;
                habitCommand.CommandText = "SELECT Habit_Id FROM Habit WHERE Habit_Name = @habitname";
                habitCommand.Parameters.AddWithValue("@habitname", habitName);
                var res = Convert.ToInt32(habitCommand.ExecuteScalar());
                if (res == null)
                {
                    Console.WriteLine("No data returned");
                    return 0;
                }
                return res;
            }
        }
    }

    public void UpdateHabitTable(string habitName, int habit_ID, string updatedunit, SqliteConnection cnn)
    {
        try
        {
            using (SqliteCommand habitCommand = new SqliteCommand())
            {
                habitCommand.Connection = cnn;
                cnn.Open();
                habitCommand.CommandText = "UPDATE Habit SET Habit_Name = @habitname,Unit = @UpdatedUnit WHERE Habit_Name =@habitname AND Habit_Id = @habitId";
                habitCommand.Parameters.AddWithValue("@habitname", habitName);
                habitCommand.Parameters.AddWithValue("@habitId", habit_ID);
                habitCommand.Parameters.AddWithValue("@UpdatedUnit", updatedunit);
                habitCommand.ExecuteNonQuery();
            }
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
    }
    public void DeleteHabit(int habitid)
    {
        HabitEnrollRepo habitEnroll = new HabitEnrollRepo();
        try
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseClass.connectionString))
            {
                connection.Open();
                {
                    using (SqliteCommand command = new SqliteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "DELETE FROM Habit WHERE Habit_Id =@habitid";
                        command.Parameters.AddWithValue("@habitid", habitid);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
