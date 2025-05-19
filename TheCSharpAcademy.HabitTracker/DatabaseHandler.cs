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
    private readonly string _dbFilePath;
    readonly string json;
    readonly Configuration.Config config;
    public DatabaseHandler()
    {
      json = File.ReadAllText("Configuration/config.json");
      var deserializedConfig = JsonSerializer.Deserialize<Configuration.Config>(json);

      if (deserializedConfig == null || string.IsNullOrEmpty(deserializedConfig.ConnectionString))
      {
        throw new Exception("Connection string not found in config.json");
      }

      config = deserializedConfig;
      config.ConnectionString = deserializedConfig.ConnectionString;

      // Extract DB file path from connection string (for SQLite)
      var builder = new SQLiteConnectionStringBuilder(config.ConnectionString);
      _dbFilePath = builder.DataSource;

      // Ensure the database file exists
      EnsureDatabaseExists();
      // Ensure the habits table exists
      EnsureHabitsTableExists(config.ConnectionString);
      // Ensure the occurrences table exists
      EnsureOccurrencesTableExists();
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
      List<Habit> habits = [];
      try
      {
        var connection = Connect();
        string sqlcommand = "select * from habits";
        using var command = new SQLiteCommand(sqlcommand, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
          var habitName = reader["habitname"]?.ToString() ?? string.Empty;
          var measuringUnit = reader["measuringUnit"]?.ToString() ?? string.Empty;
          var amount = reader["amount"] != DBNull.Value ? Convert.ToDouble(reader["amount"]) : 0;
          var id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0;

          Habit habit = new(
            habitName,
            measuringUnit,
            amount
          )
          {
            Id = id
          };
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

        string sql = "SELECT id, habitname, measuringUnit, amount FROM habits WHERE id = @id";
        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
          var habitName = reader["habitname"]?.ToString() ?? string.Empty;
          var measuringUnit = reader["measuringUnit"]?.ToString() ?? string.Empty;
          var amount = reader["amount"] != DBNull.Value ? Convert.ToDouble(reader["amount"]) : 0;
          var ID = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0;
          Habit habit = new(
          habitName,
          measuringUnit,
          amount
        )
          {
            Id = id
          };
          return habit;
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
    /// Gets all occurrences from the database from a concrete habit.
    /// </summary>
    /// <param name="habit"></param>
    /// <returns></returns>
    public List<Occurence> GetOccurencesForCurrentMonthByHabit(Habit habit)
    {
      List<Occurence> occurences = [];

      var connection = Connect();

      // Get current month and year as strings
      string currentMonth = DateTime.Now.Month.ToString("D2");
      string currentYear = DateTime.Now.Year.ToString();

      string sql = @"SELECT * FROM occurrences
  WHERE HabitID = @HabitID
  AND substr(Date, 4, 2) = @Month
  AND substr(Date, 7, 4) = @Year";

      using var command = new SQLiteCommand(sql, connection);
      command.Parameters.AddWithValue("@HabitID", habit.Id);
      command.Parameters.AddWithValue("@Month", currentMonth);
      command.Parameters.AddWithValue("@Year", currentYear);

      using var reader = command.ExecuteReader();
      while (reader.Read())
      {
        var date = reader["Date"]?.ToString() ?? string.Empty;
        var occurence = new Occurence(
            date,
            Convert.ToInt32(reader["HabitID"]),
            Convert.ToDouble(reader["value"])
        );
        occurences.Add(occurence);
      }

      return occurences;
    }
    /// <summary>
    /// Logs an occurrence in the database.
    /// </summary>
    /// <param name="occurence"></param>
    public bool LogOccurence(Occurence occurence)
    {
      //Logs an occurence object to the occurence table in database
      try
      {
        var connection = Connect();
        string sql = "INSERT INTO occurrences (HabitID, Date, value) VALUES (@HabitID, @Date, @value);";
        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@HabitID", occurence.HabitID);
        command.Parameters.AddWithValue("@Date", occurence.Date);
        command.Parameters.AddWithValue("@value", occurence.value);
        command.ExecuteNonQuery();

        Habit oldHabit = GetHabitById(occurence.HabitID);
        UpdateHabit(oldHabit,
          new Habit(
            oldHabit.Habitname,
           oldHabit.MeasuringUnit,
            oldHabit.Amount + occurence.value
          ));

        Close();
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error executing SQL command: {ex.Message}");
        Close();
        return false;
      }

    }
    /// <summary>
    /// Ensures that the occurrences table exists in the database.
    /// </summary>
    public void EnsureOccurrencesTableExists()
    {
      var connection = Connect();

      string checkTableSql = "SELECT name FROM sqlite_master WHERE type='table' AND name='occurrences';";
      using var cmd = new SQLiteCommand(checkTableSql, connection);
      var result = cmd.ExecuteScalar();

      if (result == null)
      {
        string createTableSql = @"
            CREATE TABLE occurrences (
                id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,
                HabitID INTEGER,
                Date TEXT,
                value REAL
            );";
        using var createCmd = new SQLiteCommand(createTableSql, connection);
        createCmd.ExecuteNonQuery();
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
    public static void EnsureHabitsTableExists(string constring)
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
