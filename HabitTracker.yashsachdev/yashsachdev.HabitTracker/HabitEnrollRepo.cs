namespace yashsachdev.HabitTracker;
public class HabitEnrollRepo
{
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
    public bool CheckIfHabitExistsForUser(string habitName, int userId)
    {
        HabitRepo habitRepo = new HabitRepo();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT 1 FROM Habit_Enroll WHERE User_Id = @UserId AND Habit_Id = @HabitId";
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@HabitId", habitRepo.GetHabitId(habitName));
                var reader = command.ExecuteReader();
                return reader.Read();
            }
        }
    }
    public void DisplayUserHabit(string email)
    {
        UserRepo userRepo   = new UserRepo();
        HabitRepo habitRepo = new HabitRepo();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                var user_ID = userRepo.GetIdFromEmail(email);
                command.CommandText = "SELECT * FROM Habit_Enroll WHERE User_Id = @userId";
                command.Parameters.AddWithValue("@userId", user_ID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var habit_ID = reader.GetInt32(1);
                        var startdate = reader.GetDateTime(2);
                        var habitName=habitRepo.GetHabitName(habit_ID);
                        Console.WriteLine($"Habit : {habitName} \t Start Date:{startdate} \t");
                    }
                }
            }

        }
    }
    public void UpdateUserHabit(string email, string habitName, string updatedunit)
    {
        try {

            UserRepo userRepo = new UserRepo();
            HabitRepo habitRepo = new HabitRepo();
            using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
            {
                cnn.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    var user_ID = userRepo.GetIdFromEmail(email,cnn);
                    command.Connection = cnn;
                    command.CommandText = "SELECT * FROM Habit_Enroll WHERE User_Id = @userId";
                    command.Parameters.AddWithValue("@userId", user_ID);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var habit_ID = reader.GetInt32(1);
                            habitRepo.UpdateHabitTable(habitName, habit_ID, updatedunit, cnn);
                        }
                    }
                }
            }
        } 
        catch (Exception ex) 
        {
            Console.WriteLine("Error in UpdateUserHabit()");
            Console.WriteLine(ex.Message);
        }
    }
    internal void GenerateReport(string email)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = "SELECT User_Id FROM User WHERE Email = @email";
                command.Parameters.AddWithValue("@email", email);
                var result = command.ExecuteScalar();
                if (result == null)
                {
                    Console.WriteLine("No Data returned");
                }

                var user_ID = (Int64)result;
                command.CommandText = "SELECT Habit_Id FROM Habit_Enroll WHERE User_Id = @userId";
                command.Parameters.AddWithValue("@userId", user_ID);
                var res = command.ExecuteScalar();
                if (result == null)
                {
                    Console.WriteLine("No Data returned");
                }
                var habit_ID = (Int64)result;
                command.CommandText = "SELECT user.Name,habit.Habit_Name,habit.Unit,habitenroll.Date FROM Habit_Enroll habitenroll JOIN Habit habit ON habitenroll.Habit_Id = habit.Habit_Id JOIN User user ON habitenroll.User_Id =user.User_Id WHERE user.User_Id =@userid";
                command.Parameters.AddWithValue("@userid", user_ID);
                command.ExecuteNonQuery();
                StringBuilder sb = new StringBuilder();
                SqliteDataReader reader = command.ExecuteReader();
                var tableData = new List<Report>();
                while (reader.Read()) 
                {
                    string username = Convert.ToString(reader["Name"]);
                    string habitname = Convert.ToString(reader["Habit_Name"]);
                    DateTime date = Convert.ToDateTime(reader["Date"]);
                    string unit = Convert.ToString(reader["Unit"]);
                    tableData.Add(new Report(username, habitname, date, unit));
                }
                ConsoleTableBuilder.From(tableData).WithTitle("REPORT ", ConsoleColor.Yellow, ConsoleColor.DarkGray).WithColumn("User_Name","Habit_Name","Date","Unit").ExportAndWriteLine();
            }
        }
    }
}