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

    public List<HabitLogView> GetHabitLogs()
    {
        var db = new DbConnection();
        List<HabitLogView> habitLogs = new List<HabitLogView>();

        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"SELECT HabitLogs.date, HabitLogs.quentity, Habits.title FROM HabitLogs JOIN Habits ON HabitLogs.habitid = Habits.id ORDER BY HabitLogs.id DESC;";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    HabitLogView habitLog = new HabitLogView();

                    // int eq int32.
                    habitLog.EntryDate = reader.GetDateTime(0);
                    habitLog.Quentity = reader.GetInt32(1);
                    habitLog.HabitTitle = reader.GetString(2);
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

    public long HabitCount()
    {
        try
        {
            var db = new DbConnection();
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM Habits;";
            long rowCount = (long)(command.ExecuteScalar() ?? 0);
            db.Connection.Close();
            return rowCount;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
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
