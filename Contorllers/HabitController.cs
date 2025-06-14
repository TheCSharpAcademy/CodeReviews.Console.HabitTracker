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
            command.CommandText = @"SELECT HabitLogs.id, HabitLogs.date, HabitLogs.quantity, Habits.title FROM HabitLogs JOIN Habits ON HabitLogs.habitid = Habits.id ORDER BY HabitLogs.id DESC;";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    HabitLogView habitLog = new HabitLogView();

                    // int eq int32.
                    habitLog.LogId = reader.GetInt32(0);
                    habitLog.EntryDate = reader.GetDateTime(1);
                    habitLog.Quantity = reader.GetInt32(2);
                    habitLog.HabitTitle = reader.GetString(3);
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

    public void UpdateHabit(int habitId)
    {
        var db = new DbConnection();
        var input = new UserInput();
        var habitTitle = input.GetNewHabit();
        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"UPDATE Habits SET TITLE = @title WHERE ID = @id;";
            command.Parameters.AddWithValue("@id", habitId);
            command.Parameters.AddWithValue("@title", habitTitle);
            command.ExecuteNonQuery();
            db.Connection.Close();
            Console.WriteLine("Habit updated successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Faild to update habit.");
        }
    }
    public void RemoveHabit(int habitId)
    {
        var db = new DbConnection();

        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"DELETE FROM Habits WHERE id = @id";
            command.Parameters.AddWithValue("@id", habitId);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void LogHabit(HabitLog habitLog)
    {
        var db = new DbConnection();
        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"INSERT INTO HabitLogs
                                            (DATE, QUANTITY, HABITID) VALUES (@date, @quantity, @habitid);";
            command.Parameters.AddWithValue("@date", habitLog.LogDate);
            command.Parameters.AddWithValue("@quantity", habitLog.Quantity);
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

    public void UpdateHabitLog(int logId)
    {
        var db = new DbConnection();
        var input = new UserInput();
        var updatedLog = input.LogHabit();
        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"UPDATE HabitLogs SET DATE = @date, QUANTITY = @quantity, HABITID = @habitid WHERE ID = @id;";
            command.Parameters.AddWithValue("@id", logId);
            command.Parameters.AddWithValue("@date", updatedLog.LogDate);
            command.Parameters.AddWithValue("@quantity", updatedLog.Quantity);
            command.Parameters.AddWithValue("@habitid", updatedLog.HabitId);
            command.ExecuteNonQuery();
            db.Connection.Close();
            Console.WriteLine("Log updated successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Faild to update log.");
        }
    }

    public void RemoveLog(int logId)
    {
        var db = new DbConnection();

        try
        {
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"DELETE FROM HabitLogs WHERE id = @id";
            command.Parameters.AddWithValue("@id", logId);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
