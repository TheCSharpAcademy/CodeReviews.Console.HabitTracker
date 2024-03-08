using Microsoft.Data.Sqlite;
using Models;
using System.Globalization;

namespace Repository;

public class HabitRepo
{
    public string _connectionString;



    public HabitRepo(string connectionString = @"Data Source=habit-Tracker3.db")
    {
        _connectionString = connectionString;
        InitializeDB();
    }

    private void InitializeDB()
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();
        var sql_command = connection.CreateCommand();
        /*

         */

        sql_command.CommandText =
                            @"CREATE TABLE IF NOT EXISTS Habits (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT,
                                    Type INTEGER,
                                    Quantity INTEGER,
                                    Measure TEXT,    
                                    StartsAt TEXT,
                                    EndsAt TEXT,
                                    CreatedAt TEXT,
                                    ModifiedAt TEXT,
                                    IsDeleted INTEGER,
                                    DeletedAt TEXT,
                                    OwnerId INTEGER
                                )";

        sql_command.ExecuteNonQuery();

        connection.Close();
    }

    public bool AddNewHabit(Habit habit, int userID)
    {
        bool result = false;
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            var sqliteCommand = connection.CreateCommand();
            if (habit is UnitHabit uHabit)
            {
                sqliteCommand.CommandText =
                         $@"INSERT INTO Habits(Name, Type,Quantity,Measure,CreatedAt,IsDeleted,OwnerId)
                                VALUES('{uHabit.Name}', {(int)uHabit.Type}, {uHabit.Quantity},
                                        '{uHabit.Measure}','{DateTime.UtcNow:dd-MM-yy HH:mm}',{0},{userID})";
            }
            else if (habit is DurationHabbit dHabit)
            {
                sqliteCommand.CommandText =
                            $@"INSERT INTO Habits(Name, Type,StartsAt,EndsAt,CreatedAt,IsDeleted,OwnerId)
                                VALUES('{dHabit.Name}', {(int)dHabit.Type}, '{dHabit.StartedAt:dd-MM-yy HH:mm}',
                                        '{dHabit.EndedAt:dd-MM-yy HH:mm}','{DateTime.UtcNow:dd-MM-yy HH:mm}',{0},{userID})";
            }
            sqliteCommand.ExecuteNonQuery();
            result = true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error Failed To Insert Into DB: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
        return result;
    }


    public bool DeleteAHabit(int habitId, int userId)
    {
        bool result = false;
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            var sql_command = connection.CreateCommand();

            //sql_command.CommandText = $"DELETE from Habits WHERE Id = {habitId} AND OwnerId = {userId}";
            //enable soft Delete
            sql_command.CommandText = $"UPDATE Habits SET IsDeleted = {1}, DeletedAt = '{DateTime.UtcNow:dd-MM-yy HH:mm}' WHERE Id = {habitId} AND OwnerId = {userId}";
            sql_command.ExecuteNonQuery();
            result = true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed To Delete A Habit: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
        return result;
    }

    public List<Habit> GetAllRecords(int userId)
    {
        List<Habit> result = new List<Habit>();
        using (var connection = new SqliteConnection(_connectionString))
        {
            try
            {

                connection.Open();
                var sql_command = connection.CreateCommand();
                sql_command.CommandText =
                    $"SELECT * FROM Habits WHERE OwnerId = {userId} AND IsDeleted = {0}";

                SqliteDataReader reader = sql_command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HabitType habitType = (HabitType)reader.GetInt32(2);
                        switch (habitType)
                        {
                            case HabitType.Unit:
                                result.Add(
                                    new UnitHabit
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Type = habitType,
                                        Quantity = reader.GetInt32(3),
                                        Measure = reader.GetString(4),
                                        CreatedAt = DateTime.ParseExact(reader.GetString(7), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                                        ModifiedAt = !reader.IsDBNull(8) ? DateTime.ParseExact(reader.GetString(8), "dd-MM-yy HH:mm", new CultureInfo("en-US")) : new DateTime(),
                                        IsDeleted = reader.GetInt32(9) == 1,
                                        DeletedAt = !reader.IsDBNull(10) ? DateTime.ParseExact(reader.GetString(10), "dd-MM-yy HH:mm", new CultureInfo("en-US")) : new DateTime(),
                                        OwnerId = reader.GetInt32(11)

                                    });
                                break;
                            case HabitType.Duration:
                                result.Add(
                                    new DurationHabbit
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Type = habitType,
                                        StartedAt = DateTime.ParseExact(reader.GetString(5), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                                        EndedAt = DateTime.ParseExact(reader.GetString(6), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                                        Duration = DateTime.ParseExact(reader.GetString(6), "dd-MM-yy HH:mm", new CultureInfo("en-US")) - DateTime.ParseExact(reader.GetString(5), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                                        CreatedAt = DateTime.ParseExact(reader.GetString(7), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                                        ModifiedAt = !reader.IsDBNull(8) ? DateTime.ParseExact(reader.GetString(8), "dd-MM-yy HH:mm", new CultureInfo("en-US")) : new DateTime(),
                                        IsDeleted = reader.GetInt32(9) == 1,
                                        DeletedAt = !reader.IsDBNull(10) ? DateTime.ParseExact(reader.GetString(10), "dd-MM-yy HH:mm", new CultureInfo("en-US")) : new DateTime(),
                                        OwnerId = reader.GetInt32(11),
                                    });
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"DataBase Related Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        return result;
    }

    public bool DeleteAllHabits(int userId)
    {
        bool result = false;
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            var sql_command = connection.CreateCommand();

            //sql_command.CommandText = $"DELETE from Habits WHERE OwnerId = {userId}";
            sql_command.CommandText = $"UPDATE Habits SET IsDeleted = {1}, DeletedAt = '{DateTime.UtcNow:dd-MM-yy HH:mm}' WHERE OwnerId = {userId}";

            sql_command.ExecuteNonQuery();
            result = true;

        }
        catch (Exception ex)
        {
            throw new Exception($"Failed To Delete A Habit: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
        return result;

    }

    public Habit? GetHabit(int habitId, int userId)
    {
        Habit? result = null;
        using var connection = new SqliteConnection(_connectionString);
        try
        {

            connection.Open();
            var sql_command = connection.CreateCommand();
            sql_command.CommandText =
                $"SELECT * FROM Habits WHERE Id = {habitId} AND OwnerId = {userId} AND IsDeleted = {0}";

            SqliteDataReader reader = sql_command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var type = (HabitType)reader.GetInt32(ordinal: 2);
                switch (type)
                {
                    case HabitType.Unit:
                        result = new UnitHabit
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Type = type,
                            Quantity = reader.GetInt32(3),
                            Measure = reader.GetString(4),
                            CreatedAt = DateTime.ParseExact(reader.GetString(7), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                            ModifiedAt = !reader.IsDBNull(8) ? DateTime.ParseExact(reader.GetString(8), "dd-MM-yy HH:mm", new CultureInfo("en-US")) : new DateTime(),
                            IsDeleted = reader.GetInt32(9) == 1,
                            DeletedAt = !reader.IsDBNull(10) ? DateTime.ParseExact(reader.GetString(10), "dd-MM-yy HH:mm", new CultureInfo("en-US")) : new DateTime(),
                            OwnerId = reader.GetInt32(11),
                        };
                        break;
                    case HabitType.Duration:
                        result = new DurationHabbit
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Type = type,
                            StartedAt = DateTime.ParseExact(reader.GetString(5), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                            EndedAt = DateTime.ParseExact(reader.GetString(6), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                            Duration = DateTime.ParseExact(reader.GetString(6), "dd-MM-yy HH:mm", new CultureInfo("en-US")) - DateTime.ParseExact(reader.GetString(5), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                            CreatedAt = DateTime.ParseExact(reader.GetString(7), "dd-MM-yy HH:mm", new CultureInfo("en-US")),
                            ModifiedAt = !reader.IsDBNull(8) ? DateTime.ParseExact(reader.GetString(8), "dd-MM-yy HH:mm", new CultureInfo("en-US")) : new DateTime(),
                            IsDeleted = reader.GetInt32(9) == 1,
                            DeletedAt = !reader.IsDBNull(8) ? DateTime.ParseExact(reader.GetString(8), "dd-MM-yy HH:mm", new CultureInfo("en-US")) : new DateTime(),
                            OwnerId = reader.GetInt32(11),
                        };
                        break;
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception($"DataBase Related Error: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
        return result;
    }


    public bool UpdateHabit(Habit habit, int userId)
    {
        bool result = false;
        using (var connection = new SqliteConnection(_connectionString))
        {
            try
            {
                Habit? existHabit = GetHabit(habit.Id, userId);
                if (existHabit == null) return false;
                connection.Open();
                var sql_command = connection.CreateCommand();
                sql_command.CommandText = $"UPDATE Habits SET ";

                if (habit is UnitHabit uHabit && existHabit is UnitHabit uexHabit)
                {
                    if (uexHabit.Name != uHabit.Name)
                    {
                        sql_command.CommandText += $"Name = '{uHabit.Name}', ";
                    }
                    if (uexHabit.Type != uHabit.Type)
                    {
                        sql_command.CommandText += $"Type = {(int)uHabit.Type}, ";
                    }
                    if (uexHabit.Measure != uHabit.Measure)
                    {
                        sql_command.CommandText += $"Measure = '{uHabit.Measure}', ";
                    }
                    if (uexHabit.Quantity != uHabit.Quantity)
                    {
                        sql_command.CommandText += $"Quantity = {uHabit.Quantity}, ";
                    }

                }
                else if (habit is DurationHabbit dHabit && existHabit is DurationHabbit dexHabit)
                {
                    if (dexHabit.Name != dHabit.Name)
                    {
                        sql_command.CommandText += $"Name = '{dHabit.Name}', ";
                    }
                    if (dexHabit.Type != dHabit.Type)
                    {
                        sql_command.CommandText += $"Type = {(int)dHabit.Type}, ";
                    }
                    if (dexHabit.StartedAt != dHabit.StartedAt)
                    {
                        sql_command.CommandText += $"StartsAt = '{dHabit.StartedAt:dd-MM-yy HH:mm}', ";
                    }
                    if (dexHabit.EndedAt != dHabit.EndedAt)
                    {
                        sql_command.CommandText += $"EndsAt = '{dHabit.EndedAt:dd-MM-yy HH:mm}', ";
                    }
                }
                sql_command.CommandText += $"ModifiedAt = '{DateTime.UtcNow:dd-MM-yy HH:mm}' ";
                sql_command.CommandText += $"WHERE Id = {habit.Id} AND OwnerId = {userId};";

                Console.WriteLine(sql_command.CommandText);
                sql_command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed To Update Habit: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        return result;
    }
}
