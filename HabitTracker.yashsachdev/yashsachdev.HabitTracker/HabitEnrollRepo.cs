using System.Reflection.PortableExecutable;

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
    public void DisplayUserHabit(string name, string email)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT User_Id FROM User WHERE Name = @name AND Email = @email";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                var result = command.ExecuteScalar();
                if (result == null)
                {
                    Console.WriteLine("No Data returned");
                    return;
                }
                var user_ID = (Int64)result;
                command.CommandText = "SELECT * FROM Habit_Enroll WHERE User_Id = @userId";
                command.Parameters.AddWithValue("@userId", user_ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var habit_ID = reader.GetInt32(1);
                        var startdate = reader.GetDateTime(2);
                        using (SqliteCommand habitCommand = new SqliteCommand())
                        {
                            habitCommand.Connection = cnn;
                            habitCommand.CommandText = "SELECT Habit_Name FROM Habit WHERE Habit_Id = @habitId";
                            habitCommand.Parameters.AddWithValue("@habitId", habit_ID);
                            var res = habitCommand.ExecuteScalar();
                            if (res == null)
                            {
                                Console.WriteLine("No data returned");
                                return;
                            }
                            var habitName = (String)res;
                            Console.WriteLine($"Habit : {habitName} \t Start Date:{startdate} \t");
                        }

                    }
                }

            }
        }
    }
    public void UpdateUserHabit(string name, string email,string habitName,string updatedHabitname, string updatedunit)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT User_Id FROM User WHERE Name = @name AND Email = @email";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                var result = command.ExecuteScalar();
                if (result == null)
                {
                    Console.WriteLine("No Data returned");
                    return;
                }
                var user_ID = (Int64)result;
                command.CommandText = "SELECT * FROM Habit_Enroll WHERE User_Id = @userId";
                command.Parameters.AddWithValue("@userId", user_ID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var habit_ID = reader.GetInt32(1);
                        using (SqliteCommand habitCommand = new SqliteCommand())
                        {
                            habitCommand.Connection = cnn;
                            habitCommand.CommandText = "UPDATE Habit SET Habit_Name = @UpdatedName,Unit = @UpdatedUnit WHERE  =@habitId;";
                            habitCommand.Parameters.AddWithValue("@habitId", habit_ID);
                            habitCommand.Parameters.AddWithValue("@UpdatedName", updatedHabitname);
                            habitCommand.Parameters.AddWithValue("@UpdatedUnit", updatedunit);
                            habitCommand.ExecuteNonQuery();
                        }
                    }

                }
            }
        }
    }
}