namespace DotNETConsole.HabitTracker.Controllers;

using DataModels;
using DB;
using UI;

public class HabitController
{

    public List<Habit> GetHabits()
    {
        var db = new DbConnection();
        List<Habit> habits = new List<Habit>();

        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Habits ORDER BY ID DESC;";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Habit habit = new Habit();

                    // int eq int32.
                    habit.Id = reader.GetInt32(0);
                    habit.Title = reader.GetString(1);
                    habits.Add(habit);
                }
            }
            db.Connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return habits;
    }

    public List<HabitLog> GetHabitLogs()
    {
        var db = new DbConnection();
        List<HabitLog> habitLogs = new List<HabitLog>();

        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"SELECT * FROM HabitLogs ORDER BY ID DESC;";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    HabitLog habitLog = new HabitLog();

                    // int eq int32.
                    habitLog.Id = reader.GetInt32(0);
                    habitLog.LogDate = reader.GetDateTime(1);
                    habitLog.Quantity = reader.GetInt32(2);
                    habitLog.HabitId = reader.GetInt32(3);
                    habitLogs.Add(habitLog);
                }
            }
            db.Connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return habitLogs;
    }

    public void AddHabit(string habit)
    {
        var db = new DbConnection();
        var input = new UserInput();
        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"INSERT INTO Habits
                                    (Title) VALUES (@title);";
            command.Parameters.AddWithValue("@title", habit);
            command.ExecuteNonQuery();
            db.Connection.Close();
            Console.WriteLine("Habit added successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Faild to add habit.");
        }
    }

    public void LogHabit(HabitLog habitLog)
    {
        var db = new DbConnection();
        var input = new UserInput();
        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"INSERT INTO HabitLogs
                                            (DATE, QUENTITY, HABITID) VALUES (@date, @quentity, @habitid);";
            command.Parameters.AddWithValue("@date", habitLog.LogDate);
            command.Parameters.AddWithValue("@quentity", habitLog.Quantity);
            command.Parameters.AddWithValue("@habitid", habitLog.HabitId);
            command.ExecuteNonQuery();
            db.Connection.Close();
            Console.WriteLine("Log added successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Faild to add habit.");
        }
    }
}
