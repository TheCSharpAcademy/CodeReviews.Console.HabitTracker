using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Data.SQLite;
using TheCSharpAcademy.HabitTracker.Models;

namespace TheCSharpAcademy.HabitTracker
{
  class DatabaseHandler
  {
    #region properties
    private string _dbFilePath;
    string json;
    Configuration.Config config;
    #endregion
    #region constructors
    public DatabaseHandler()
    {
      json = File.ReadAllText("Configuration/config.json");
      config = JsonSerializer.Deserialize<Configuration.Config>(json);
      config.ConnectionString = config?.ConnectionString ?? throw new Exception("Connection string not found in config.json");

      // Extract DB file path from connection string (for SQLite)
      var builder = new SQLiteConnectionStringBuilder(config.ConnectionString);
      _dbFilePath = builder.DataSource;

      // Ensure the database file exists
      EnsureDatabaseExists();
      // Ensure the habits table exists
      EnsureHabitsTableExists(config.ConnectionString);
    }
    #endregion

    #region methods
    /// <summary>
    /// Connects to the database.
    /// </summary>
    public SQLiteConnection Connect()
    {
      var connection = new SQLiteConnection($"Data Source={_dbFilePath};Version=3;");
      connection.Open();
      return connection;
    }
    /// <summary>
    /// Closes the database connection.
    /// </summary>
    public void Close()
    {
      using var connection = new SQLiteConnection($"Data Source={_dbFilePath};Version=3;");
      connection.Open();
    }

    /// <summary>
    /// Executes a SQL command that writes data to the database.
    /// </summary>
    /// <param name="sqlcommand"></param>
    public bool ExecuteSQLCommandWrite(SQLiteCommand sqlcommand)
    {
      try
      {
        var connection = Connect();

        sqlcommand.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error executing SQL command: {ex.Message}");
        return false;
      }
      finally
      {
        Close();
      }
    }
    /// <summary>
    /// Gets all habits from the database.
    /// </summary>
    /// <param name="sqlcommand"></param>
    /// <returns>List<Habit> All Habits</returns>
    public List<Habit> GetAllHabits()
    {
      List<Habit> habits = new List<Habit>();
      try
      {
        var connection = Connect();
        string sqlcommand = "select * from habits";
        using var command = new SQLiteCommand(sqlcommand, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
          var habit = new Habit(
            reader["habitname"].ToString(),
            reader["measuringUnit"].ToString(),
            Convert.ToDouble(reader["amount"])
          );
          habits.Add(habit);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error executing SQL command: {ex.Message}");
      }
      finally
      {
        Close();
      }
      return habits;
    }
    /// <summary>
    /// Gets a habit by ID from the database.
    /// </summary>
    /// <returns>Habit</returns>
    public Habit GetHabitById(int id)
    {
      try
      {
        var connection = Connect();

        string sql = "SELECT * FROM habits WHERE id = @id";
        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id); // Example ID, replace with actual ID
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
          return new Habit(
            reader["habitname"].ToString(),
            reader["measuringUnit"].ToString(),
            Convert.ToDouble(reader["amount"])
          );
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error executing SQL command: {ex.Message}");
      }
      finally
      {
        Close();
      }
      return new Habit("", "", 0); // Return a default Habit object if not found
    }

    /// <summary>
    /// Updates a habit
    /// </summary>
    /// <param name="oldHabit"></param>
    /// <param name="newHabit"></param>
    /// <returns>True if succesfull, false if not</returns>
    public bool UpdateHabit(Habit oldHabit, Habit newHabit)
    {
      SQLiteConnection connection = Connect();

      try
      {
        string sql = "UPDATE habits SET habitname = @habitname, measuringUnit = @measuringUnit, amount = @amount WHERE id = @id;";
        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", oldHabit.Id);
        command.Parameters.AddWithValue("@habitname", newHabit.Habitname);
        command.Parameters.AddWithValue("@measuringUnit", newHabit.MeasuringUnit);
        command.Parameters.AddWithValue("@amount", newHabit.Amount);

        ExecuteSQLCommandWrite(command);
        Close();
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error executing SQL command: {ex.Message}");
        Close();
        return false;
      }
      finally
      {
        Close();
      }
    }
    /// <summary>
    /// Adds a habit to the database.
    /// </summary>
    /// <param name="habit"></param>
    /// <returns></returns>
    public bool AddHabit(Habit habit)
    {
      try
      {
        var connection = Connect();

        string sql = "INSERT INTO habits (habitname, measuringUnit, amount) VALUES (@habitname, @measuringUnit, @amount);";
        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@habitname", habit.Habitname);
        command.Parameters.AddWithValue("@measuringUnit", habit.MeasuringUnit);
        command.Parameters.AddWithValue("@amount", habit.Amount);

        command.ExecuteNonQuery();

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error executing SQL command: {ex.Message}");
        return false;
      }
      finally
      {
        Close();
      }
    }

    /// <summary>
    /// Removes a habit from the database.
    /// </summary>
    /// <param name="habit"></param>
    /// <returns></returns>
    public bool RemoveHabit(Habit habit)
    {
      try
      {
        var connection = Connect();
        string sql = "DELETE FROM habits WHERE id = @id;";
        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", habit.Id);
        command.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error executing SQL command: {ex.Message}");
        return false;
      }
      finally
      {
        Close();
      }
    }
    /// <summary>
    /// Ensures that the database file exists.
    /// </summary>
    public void EnsureDatabaseExists()
    {
      var directory = Path.GetDirectoryName(_dbFilePath);
      if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }
      if (!File.Exists(_dbFilePath))
      {
        SQLiteConnection.CreateFile(_dbFilePath);
      }
    }
    /// <summary>
    /// Ensures that the habits table exists in the database.
    /// </summary>
    /// <param name="constring"></param>
    public void EnsureHabitsTableExists(string constring)
    {
      using var connection = new SQLiteConnection(constring);
      connection.Open();

      string checkTableSql = "SELECT name FROM sqlite_master WHERE type='table' AND name='habits';";
      using var cmd = new SQLiteCommand(checkTableSql, connection);
      var result = cmd.ExecuteScalar();

      if (result == null)
      {
        string createTableSql = @"
                    CREATE TABLE habits (
                        id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,
                        habitname TEXT,
                        measuringUnit TEXT,
                        amount REAL
                    );";
        using var createCmd = new SQLiteCommand(createTableSql, connection);
        createCmd.ExecuteNonQuery();
      }
    }
    #endregion
  }
}
