using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitLogger.TildaDares;

public class HabitLoggerDatabase
{
    private const string FileName = "habitLoggerDb.db";

    public HabitLoggerDatabase()
    {
        CreateHabitLoggerDB();
    }

    public void InsertHabit(DateOnly date, int quantity, string unit, string type)
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO habitLogger(date, quantity, unit, type) VALUES ($date, $quantity, $unit, $type)";
            command.Parameters.AddWithValue("$date", date);
            command.Parameters.AddWithValue("$quantity", quantity);
            command.Parameters.AddWithValue("$unit", unit);
            command.Parameters.AddWithValue("$type", type);
            
            var status = command.ExecuteNonQuery();
            Console.WriteLine(status < 1 ? "Unable to insert habit record!" : "Habit successfully inserted!");
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to insert habit record! {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }

    public Habit GetHabit(int habitId)
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        Habit habit = null;
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT * FROM habitLogger WHERE id = $id LIMIT 1";
            command.Parameters.AddWithValue("$id", habitId);
            
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var date = DateOnly.Parse(reader.GetString(1), new CultureInfo("en-US"), DateTimeStyles.None);
                    var quantity = reader.GetInt32(2);
                    var unit = reader.GetString(3);
                    var type = reader.GetString(4);

                    habit = new Habit(id, date, quantity, unit, type);
                }
                Console.WriteLine("Habit retrieved!");
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to retrieve habit record with {habitId}. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
        
        return habit;
    }
    
    public List<Habit> GetAllHabits()
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        var habits = new List<Habit>();
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM habitLogger";
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var date = DateOnly.Parse(reader.GetString(1), new CultureInfo("en-US"), DateTimeStyles.None);
                    var quantity = reader.GetInt32(2);
                    var unit = reader.GetString(3);
                    var type = reader.GetString(4);
                    
                    habits.Add(new Habit(id, date, quantity, unit, type));
                }
                Console.WriteLine("All habits retrieved!");
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to retrieve all habit records. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
        
        return habits;
    }

    public int GetHabitsByTypeAndDate(string habitType, DateOnly? startDate, DateOnly? endDate)
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        var result = 0;
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT SUM(quantity) FROM habitLogger WHERE type = $type";
            if (startDate.HasValue && endDate.HasValue)
            {
                command.CommandText += @" AND DATE(date) BETWEEN DATE($startDate) AND DATE($endDate)";
                command.Parameters.AddWithValue("$startDate", startDate);
                command.Parameters.AddWithValue("$endDate", endDate);
            }
            
            command.Parameters.AddWithValue("$type", habitType);
            var status = command.ExecuteScalar();

            if (status is not (null or DBNull))
            {
                result = Convert.ToInt32(status);
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to query habits by type {habitType}. {e.Message}");
        }
        finally
        {
            connection.Close();
        }

        return result;
    }
    
    public List<Habit> GetHabitsByDate(DateOnly startDate, DateOnly endDate)
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        var habits = new List<Habit>();
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT * FROM habitLogger WHERE DATE(date) BETWEEN DATE($startDate) AND DATE($endDate) ORDER BY DATE(date) ASC";
            
            command.Parameters.AddWithValue("$startDate", startDate);
            command.Parameters.AddWithValue("$endDate", endDate);
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var date = DateOnly.Parse(reader.GetString(1), new CultureInfo("en-US"), DateTimeStyles.None);
                    var quantity = reader.GetInt32(2);
                    var unit = reader.GetString(3);
                    var type = reader.GetString(4);
                    
                    habits.Add(new Habit(id, date, quantity, unit, type));
                }
                Console.WriteLine($"All habits between {startDate} and {endDate} retrieved!");
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to query between {startDate} and {endDate}. {e.Message}");
        }
        finally
        {
            connection.Close();
        }

        return habits;
    }
    
    public void UpdateHabit(int habitId, DateOnly date, int quantity, string unit, string type)
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"UPDATE habitLogger SET date = $date, quantity = $quantity, unit = $unit, type = $type WHERE id = $id";
            command.Parameters.AddWithValue("$date", date);
            command.Parameters.AddWithValue("$quantity", quantity);
            command.Parameters.AddWithValue("$unit", unit);
            command.Parameters.AddWithValue("$type", type);
            command.Parameters.AddWithValue("$id", habitId);
            
            var status = command.ExecuteNonQuery();
            Console.WriteLine(status < 1 ? $"Unable to update habit record with ID {habitId}!" : "Habit Updated!");
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to update habit record with {habitId}. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
    
    public void DeleteHabit(int habitId)
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"DELETE FROM habitLogger WHERE id = $id";
            command.Parameters.AddWithValue("$id", habitId);
            
            var status = command.ExecuteNonQuery();
            Console.WriteLine(status < 1 ? $"Unable to delete habit record with ID {habitId}!" : "Habit Deleted!");
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to delete habit record with {habitId}. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }

    public long CountHabits()
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        long count = 0;
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM habitLogger";
            count = (long)command.ExecuteScalar();
        } 
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to count habit records. {e.Message}");
        }
        finally
        {
            connection.Close();
        }

        return count;
    }
    
    private void CreateHabitLoggerDB()
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        try
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @" CREATE TABLE IF NOT EXISTS habitLogger (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                date TEXT NOT NULL,
                quantity INTEGER NOT NULL,
                unit TEXT,
                type TEXT NOT NULL )";
            command.ExecuteNonQuery();
            
            // Check if table is already populated
            command.CommandText = "SELECT COUNT(*) FROM habitLogger";
            var count = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            
            if (count == 0)
            {
                SeedHabitLoggerDB();
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to create database. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }

    private void SeedHabitLoggerDB()
    {
        using var connection = new SqliteConnection($"Data Source={FileName}");
        try
        {
            connection.Open();
            var habits = new List<(string, string)>() {("running", "kms"), ("sleeping", "hrs"), ("walking", "steps"), ("knitting", "stitches")};
            var rand = new Random();
            var startDay = new DateOnly(DateTime.UtcNow.Year, 1, 1).DayNumber;
            var endDay = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 28).DayNumber;

            for (var i = 0; i < 100; i++)
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    @"INSERT INTO habitLogger(date, quantity, unit, type) VALUES ($date, $quantity, $unit, $type)";
                
                var randomDayNumber = rand.Next(startDay, endDay + 1);
                var randomDateInTheYear = DateOnly.FromDayNumber(randomDayNumber);
                var randomHabit = rand.Next(0, habits.Count);
                
                command.Parameters.AddWithValue("$date", randomDateInTheYear);
                command.Parameters.AddWithValue("$quantity", rand.Next(1, 10000));
                command.Parameters.AddWithValue("$unit", habits[randomHabit].Item2);
                command.Parameters.AddWithValue("$type", habits[randomHabit].Item1);
                
                command.ExecuteNonQuery();
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to seed database. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
}
