using System.Data.SqlClient;
using System.Data.SQLite;
using HabitTracker.kwm0304.Models;
using Microsoft.Data.Sqlite;

namespace HabitTracker.kwm0304.Data;

public class DbActions
{
  private static readonly string dbFileName = "HabitDB.db";
  private static readonly string connectionString = $"Data Source={dbFileName}";
  //ON START
  public void CreateDatabaseOnStart()
  {
    if (!File.Exists(dbFileName))
    {
      using (File.Create("HabitDB.db")) { }
    }
  }
  public void CreateTableOnStart()
  {
    using SqliteConnection connection = new(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    const string queryString = @"CREATE TABLE IF NOT EXISTS Habits(
        HabitId INTEGER PRIMARY KEY,
        HabitName TEXT NOT NULL,
        UnitOfMeasurement TEXT NOT NULL,
        Repetitions INTEGER NOT NULL,
        StartedOn TEXT NOT NULL,
        IsMock INTEGER NOT NULL DEFAULT 0
        )";
    command.CommandText = queryString;
    try
    {
      command.ExecuteNonQuery();
    }
    catch (SqliteException e)
    {
      Console.WriteLine($"Error: {e.Message}");
    }
    finally
    {
      command.Dispose();
      connection.Dispose();
    }
  }
  //GET ONE
  public Habit? GetHabitById(int id)
  {
    using SqliteConnection connection = new(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    const string queryString = @"SELECT * FROM Habits WHERE HabitId = @habitId";
    command.Parameters.AddWithValue("@habitId", id);
    command.CommandText = queryString;
    using var reader = command.ExecuteReader();
    if (reader.Read())
    {
      return new Habit
      {
        HabitId = reader.GetInt32(0),
        HabitName = reader.GetString(1),
        UnitOfMeasurement = reader.GetString(2),
        Repetitions = reader.GetInt32(3),
        StartedOn = DateTime.Parse(reader.GetString(4)),
        IsMock = reader.GetInt32(5) == 1
      };
    }
    else
    {
      return null;
    }
  }
  //GET ALL
  public IEnumerable<Habit> GetHabits()
  {
    var habits = new List<Habit>();
    using SQLiteConnection connection = new(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    string queryString = "SELECT * FROM Habits";
    command.CommandText = queryString;
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
      var habit = new Habit
      {
        HabitId = reader.GetInt32(0),
        HabitName = reader.GetString(1),
        UnitOfMeasurement = reader.GetString(2),
        Repetitions = reader.GetInt32(3),
        StartedOn = DateTime.Parse(reader.GetString(4)),
        IsMock = reader.GetInt32(5) == 1
      };
      habits.Add(habit);
    }
    return habits;
  }
  //POST 
  public void InsertHabit(Habit habit)
  {
    using SqliteConnection connection = new(connectionString);
    connection.Open();
    using var command = connection.CreateCommand();
    string queryString = @$"INSERT INTO Habits 
  (HabitName, UnitOfMeasurement, Repetitions, StartedOn, IsMock)
  VALUES 
  (@habitName, @unitOfMeasurement, @repetitions, @startedOn, @isMock);";
    command.Parameters.AddWithValue("@habitName", habit.HabitName);
    command.Parameters.AddWithValue("@unitOfMeasurement", habit.UnitOfMeasurement);
    command.Parameters.AddWithValue("@repetitions", habit.Repetitions);
    command.Parameters.AddWithValue("@startedOn", habit.StartedOn.ToString("yyyy-MM-dd"));
    command.Parameters.AddWithValue("@isMock", habit.IsMock ? 1 : 0);
    command.CommandText = queryString;
    try
    {
      command.ExecuteNonQuery();
      Console.WriteLine($"New habit {habit.HabitName} added successfully");
    }
    catch (SqliteException e)
    {
      Console.WriteLine($"Error adding new habit:\n{e.Message}");
    }
    finally
    {
      command.Dispose();
      connection.Dispose();
    }
  }
//UPDATE
  public void UpdateHabitRepetitions(int addedReps, int id)
  {
    using SqliteConnection connection = new(connectionString);
    connection.Open();
    using var command = connection.CreateCommand();
    string queryString = "UPDATE Habits SET Repetitions = Repetitions + @addedReps WHERE HabitId = @habitId";
    command.Parameters.AddWithValue("@addedReps", addedReps);
    command.Parameters.AddWithValue("@habitId", id);
    command.CommandText = queryString;
    try
    {
      command.ExecuteNonQuery();
      Console.WriteLine($"Habit repetitions successfully updated");
    }
    catch (SQLiteException e)
    {
      Console.WriteLine($"Error updating habit:\n{e.Message}");
    }
    finally
    {
      command.Dispose();
      connection.Dispose();
    }
  }
  public void UpdateHabitFields(string field, object newFieldValue, int id)
  {
    Habit habitToUpdate = GetHabitById(id)!;
    if (habitToUpdate == null)
    {
      Console.WriteLine("No habit found with this id");
      return;
    }
    var validFields = new List<string> { "HabitName", "UnitOfMeasurement", "Repetitions" };
    if (!validFields.Contains(field))
    {
      Console.WriteLine("Invalid field name");
      return;
    }

    using SqliteConnection connection = new(connectionString);
    connection.Open();
    using var command = connection.CreateCommand();

    string queryString = $"UPDATE Habits SET {field} = @newFieldValue WHERE HabitId = @habitId";
    command.Parameters.AddWithValue("@newFieldValue", newFieldValue);
    command.Parameters.AddWithValue("@habitId", id);
    command.CommandText = queryString;
    try
    {
      command.ExecuteNonQuery();
      Console.WriteLine($"Habit {field} successfully updated to {newFieldValue}");
    }
    catch (SQLiteException e)
    {
      Console.WriteLine($"Error updating habit:\n{e.Message}");
    }
    finally
    {
      command.Dispose();
      connection.Dispose();
    }
  }
  //DELETE
  public void DeleteHabit(int id)
  {
    using SqliteConnection connection = new(connectionString);
    connection.Open();
    using var command = connection.CreateCommand();
    string queryString = "DELETE FROM Habits WHERE HabitId = @habitId";
    command.Parameters.AddWithValue("@habitId", id);
    command.CommandText = queryString;
    try
    {
      command.ExecuteNonQuery();
      Console.WriteLine("Habit successfully deleted");
    }
    catch (SqlException e)
    {
      Console.WriteLine($"Error updating habit:\n{e.Message}");
    }
    finally
    {
      command.Dispose();
      connection.Dispose();
    }
  }
  public void DeleteMocks()
  {
    using SqliteConnection connection = new(connectionString);
    connection.Open();
    using var command = connection.CreateCommand();
    const string queryString = "DELETE FROM Habits WHERE IsMock = 1";
    command.CommandText = queryString;
    try
    {
      int affectedRows = command.ExecuteNonQuery();
      Console.WriteLine($"Mock habits successfully deleted, {affectedRows} rows affected.");
    }
    catch (SqliteException e)
    {
      Console.WriteLine($"Error deleting mock habits:\n{e.Message}");
    }
    finally
    {
      command.Dispose();
      connection.Dispose();
    }
  }
}