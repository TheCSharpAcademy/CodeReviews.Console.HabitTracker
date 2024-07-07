using HabitTracker.kwm0304.Models;
using Microsoft.Data.Sqlite;

namespace HabitTracker.kwm0304.Data;

public class DbActions
{
  private static readonly string dbFileName = "HabitDB.db";
  private static readonly string connectionString = $"Data Source={dbFileName}";
  //CREATE TABLE
  public void CreateOnStart()
  {
    if (!File.Exists(dbFileName))
    {
      using (File.Create("HabitDB.db")) { }
    }
    using SqliteConnection connection = new(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    const string queryString = @"CREATE TABLE IF NOT EXISTS Habits(
        HabitId INTEGER PRIMARY KEY,
        HabitName TEXT NOT NULL,
        UnitOfMeasurement TEXT NOT NULL,
        Repetitions INTEGER NOT NULL,
        StartedOn TEXT NOT NULL,
        DaysTracked INTEGER NOT NULL
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
  public void GetHabit()
  {

  }
  //GET ALL
  public void GetHabits()
  {

  }
  //POST TO TABLE
  public void InsertHabit(Habit habit)
  {
    using SqliteConnection connection = new(connectionString);
    connection.Open();
    using var command = connection.CreateCommand();
    string queryString = @$"INSERT INTO Habits 
    (HabitName, UnitOfMeasurement, Repetitions, StartedOn, DaysTracked)
    VALUES 
    ({habit.HabitName}, {habit.UnitOfMeasurement}, {habit.Repetitions}, {habit.StartedOn:yyyy-MM-dd)}, {habit.DaysTracked})";
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

  public void UpdateHabit(int id)
  {

  }
  public void DeleteHabit(int id)
  {

  }
}