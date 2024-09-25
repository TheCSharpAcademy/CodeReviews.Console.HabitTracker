using Microsoft.Data.Sqlite;

namespace HabitTracker.carlosmorales125;

public class DbContext
{
    // path to habit-tracker.db from the root of my exe: /bin/Debug/net8.0/HabitTracker.carlosmorales125
    // this addresses the Changing Your Working Directory issue found here: https://www.thecsharpacademy.com/project/12/habit-logger
    // I was having a lot of trouble getting my IDE (JetBrains Rider) to let me change my working directory.
    private const string ConnectionString = "Data Source=../../../habit-tracker.db";
    
    public void CreateDatabase()
    {
        var connection = new SqliteConnection(ConnectionString);
        
        try
        {
            connection.Open();
            
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = """
                 CREATE TABLE IF NOT EXISTS Habits (
                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
                     Name TEXT NOT NULL,
                     Quantity INTEGER,
                     Date INTEGER
                 );
                 """;
            createTableCommand.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"SQLite error: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
    
    public List<Habit> GetHabits()
    {
        var connection = new SqliteConnection(ConnectionString);
        var habits = new List<Habit>();
        
        try
        {
            connection.Open();
            
            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM Habits;";
            var reader = selectCommand.ExecuteReader();
            
            while (reader.Read())
            {
                var habit = new Habit
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    Date = new DateTime(reader.GetInt64(3))
                };
                habits.Add(habit);
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"SQLite error: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
        
        return habits;
    }
    
    public void AddHabit(Habit habit)
    {
        var connection = new SqliteConnection(ConnectionString);
        
        try
        {
            connection.Open();
            
            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = """
                INSERT INTO Habits (Name, Quantity, Date)
                  VALUES ($name, $quantity, $date);
                """;
            insertCommand.Parameters.AddWithValue("$name", habit.Name);
            insertCommand.Parameters.AddWithValue("$quantity", habit.Quantity);
            insertCommand.Parameters.AddWithValue("$date", habit.Date.Ticks);
            insertCommand.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"SQLite error: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
    
    public void UpdateHabit(Habit habit)
    {
        var connection = new SqliteConnection(ConnectionString);
        
        try
        {
            connection.Open();
            
            var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = """
                  UPDATE Habits
                  SET Name = $name, Quantity = $quantity, Date = $date
                  WHERE Id = $id;
                """;
            updateCommand.Parameters.AddWithValue("$name", habit.Name);
            updateCommand.Parameters.AddWithValue("$quantity", habit.Quantity);
            updateCommand.Parameters.AddWithValue("$date", habit.Date.Ticks);
            updateCommand.Parameters.AddWithValue("$id", habit.Id);
            updateCommand.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"SQLite error: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
    
    public void DeleteHabit(int id)
    {
        var connection = new SqliteConnection(ConnectionString);
        
        try
        {
            connection.Open();
            
            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = "DELETE FROM Habits WHERE Id = $id;";
            deleteCommand.Parameters.AddWithValue("$id", id);
            deleteCommand.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"SQLite error: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
}