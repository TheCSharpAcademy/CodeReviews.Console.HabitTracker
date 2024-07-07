using Microsoft.Data.Sqlite;

namespace HabitTracker.kwm0304.Data;

public class DbActions
{
  private static readonly string dbFileName = "HabitDB.db"; 
    //CREATE TABLE
    public void CreateOnStart()
    {
      if (!File.Exists(dbFileName))
      {
       using (File.Create("HabitDB.db")){}
      }
        using var connection = new SqliteConnection($"Data Source={dbFileName}");
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"CREATE TABLE IF NOT EXISTS Habits(
        HabitId INTEGER PRIMARY KEY,
        HabitName TEXT NOT NULL,
        UnitOfMeasurement TEXT NOT NULL,
        Repetitions INTEGER NOT NULL,
        StartedOn TEXT NOT NULL,
        DaysTracked INTEGER NOT NULL
        )";
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
    public void AddHabit()
    {

    }
    //UPDATE ONE
    public void UpdateHabit()
    {

    }
    //DELETE ONE
    public void DeleteHabit()
    {

    }
    
    

}
