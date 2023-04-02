﻿using System.Diagnostics.CodeAnalysis;
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
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = @"SELECT 1 FROM Habit_Enroll 
                    INNER JOIN Habit ON Habit_Enroll.Habit_Id = Habit.Habit_Id 
                    WHERE Habit_Enroll.User_Id = @userId AND Habit.Habit_Name = @habitname";
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@habitname", habitName);
                var reader = command.ExecuteReader();
                return reader.Read();
            }
        }
    }  
    public void DisplayUserHabit(string email)
    {
        UserRepo userRepo = new UserRepo();
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
                        var habitName = habitRepo.GetHabitName(habit_ID);
                        Console.WriteLine($"Habit : {habitName} \t Start Date:{startdate} \t");
                    }
                }
            }

        }
    }
    public void UpdateUserHabit(string email, string habitName, string updatedunit, int Habit_Id)
    {
        try
        {
            HabitRepo habitRepo = new HabitRepo();

            using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
            {
                cnn.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = cnn;
                    habitRepo.UpdateHabitTable(habitName, Habit_Id, updatedunit, cnn);
                }
            }
        }
        catch (Exception ex)
        {   
            Console.WriteLine("Error in UpdateUserHabit()");
            Console.WriteLine(ex.Message);
        }
    }
    internal List<string> GetUnit(int user_Id,string habitname)
    {
        List<string> unitList = new List<string>();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = @"SELECT habit.Unit
                    FROM Habit_Enroll habitenroll 
                    JOIN Habit habit ON habitenroll.Habit_Id = habit.Habit_Id 
                    JOIN User user ON habitenroll.User_Id =user.User_Id 
                    WHERE user.User_Id =@userid AND Habit.Habit_Name = @habitname";
                command.Parameters.AddWithValue("@habitname", habitname);
                command.Parameters.AddWithValue("@userid", user_Id);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string unit = reader.GetString(0);
                        unitList.Add(unit);
                    }
                }
            }
        }
        return unitList;
    }

    internal void GenerateReport(int user_ID)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = cnn;
                command.CommandText = @"SELECT user.Name,habit.Habit_Name,habit.Unit,habitenroll.Date 
                    FROM Habit_Enroll habitenroll 
                    JOIN Habit habit ON habitenroll.Habit_Id = habit.Habit_Id 
                    JOIN User user ON habitenroll.User_Id =user.User_Id 
                    WHERE user.User_Id =@userid";
                command.Parameters.AddWithValue("@userid", user_ID);
                command.ExecuteNonQuery();
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
                ConsoleTableBuilder.From(tableData).WithTitle("REPORT ", ConsoleColor.Yellow, ConsoleColor.DarkGray).WithColumn("User_Name", "Habit_Name", "Date", "Unit").ExportAndWriteLine();
            }
        }
    }
    public DateTime GetDate(int HabitId)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand habitCommand = new SqliteCommand())
            {
                habitCommand.Connection = cnn;
                habitCommand.CommandText = "SELECT Date FROM Habit_Enroll WHERE Habit_Id = @HabitId";
                habitCommand.Parameters.AddWithValue("@HabitId", HabitId);
                var res = Convert.ToDateTime(habitCommand.ExecuteScalar());
                if(res == null)
                {
                    Console.WriteLine("No data returned");
                    return DateTime.MinValue;
                }
                return res;
            }

        }
    }
    
    public int GetHabitId(string habitName, int User_Id)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            using (SqliteCommand habitCommand = new SqliteCommand())
            {
                habitCommand.Connection = cnn;
                habitCommand.CommandText = @"SELECT Habit.Habit_Id FROM Habit_Enroll
                    INNER JOIN Habit ON Habit_Enroll.Habit_Id = Habit.Habit_Id 
                    WHERE Habit_Enroll.User_Id = @userId AND Habit.Habit_Name = @habitname";
                habitCommand.Parameters.AddWithValue("@habitname", habitName);
                habitCommand.Parameters.AddWithValue("@userId", User_Id);
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
}
