using Microsoft.Data.Sqlite;
using Models;

namespace DB;

public class DbContext
{
    private readonly string connString;

    public DbContext(string connString)
    {
        this.connString = connString;
    }

    public void SeedDatabase()
    {
        using var conn = new SqliteConnection(connString);

        conn.Open();

        var command = conn.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS habits (
                id INTEGER PRIMARY KEY,
                name TEXT,
                quantity INTEGER,
                date TEXT
            );
        ";

        command.ExecuteNonQuery();

        command.CommandText = @"
            SELECT * FROM habits; 
        ";

        var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            reader.Close();
            command.CommandText = @"
                INSERT INTO habits 
                    (name, quantity, date)
                VALUES
                    ('run', 3, '01-03-24'),
                    ('run', 2, '02-03-24'),
                    ('run', 4, '03-03-24'),
                    ('drink water', 3, '01-03-24'),
                    ('drink water', 5, '02-03-24'),
                    ('drink water', 3, '03-03-24');
            ";

            command.ExecuteNonQuery();
        }
        conn.Close();
    }

    public List<Habit> GetEntriesForHabitByName(string name)
    {
        List<Habit> results = [];

        using var conn = new SqliteConnection(connString);

        var command = conn.CreateCommand();
        command.CommandText = @"
            SELECT * FROM habits WHERE name = @NAME;
        ";
        command.Parameters.Add("@NAME", SqliteType.Text);
        command.Parameters["@NAME"].Value = name;

        try
        {
            conn.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var habitName = reader.GetString(1);
                var quantity = reader.GetInt32(2);
                var date = reader.GetString(3);

                Habit habit = new(id, habitName, quantity, date);

                results.Add(habit);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return results;
    }

    public List<Habit> GetAllHabitEntries()
    {
        List<Habit> results = [];

        using var conn = new SqliteConnection(connString);

        var command = conn.CreateCommand();
        command.CommandText = @"
            SELECT * FROM habits;
        ";

        try
        {
            conn.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var habitName = reader.GetString(1);
                var quantity = reader.GetInt32(2);
                var date = reader.GetString(3);

                Habit habit = new(id, habitName, quantity, date);

                results.Add(habit);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return results;
    }

    public List<Habit> GetHabitsByDate(string date)
    {
        List<Habit> results = [];

        using var conn = new SqliteConnection(connString);

        var command = conn.CreateCommand();
        command.CommandText = @"
            SELECT * FROM habits WHERE date = @DATE;
        ";
        command.Parameters.Add("@DATE", SqliteType.Text);
        command.Parameters["@DATE"].Value = date;

        try
        {
            conn.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var habitName = reader.GetString(1);
                var quantity = reader.GetInt32(2);
                var dateString = reader.GetString(3);

                Habit habit = new(id, habitName, quantity, dateString);

                results.Add(habit);
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return results;
    }

    public void CreateHabit(Habit habit)
    {
        using var conn = new SqliteConnection(connString);

        var command = conn.CreateCommand();
        command.CommandText = @"
            INSERT INTO habits 
                    (name, quantity, date)
                VALUES
                    (@NAME, @QUANTITY, @DATE);
        ";
        command.Parameters.Add("@NAME", SqliteType.Text);
        command.Parameters.Add("@QUANTITY", SqliteType.Integer);
        command.Parameters.Add("@DATE", SqliteType.Text);
        command.Parameters["@NAME"].Value = habit.Name;
        command.Parameters["@QUANTITY"].Value = habit.Quantity;
        command.Parameters["@DATE"].Value = habit.Date;

        try
        {
            conn.Open();
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public Habit? GetHabitEntryById(int id)
    {
        using var conn = new SqliteConnection(connString);

        var command = conn.CreateCommand();
        command.CommandText = @"
            SELECT * FROM habits 
            WHERE
                id = @ID;
        ";

        command.Parameters.Add("@ID", SqliteType.Integer);

        command.Parameters["@ID"].Value = id;

        try
        {
            conn.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var habitId = reader.GetInt32(0);
                var habitName = reader.GetString(1);
                var quantity = reader.GetInt32(2);
                var dateString = reader.GetString(3);

                Habit habit = new(habitId, habitName, quantity, dateString);
                return habit;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return null;
    }

    public void DeleteHabit(int id)
    {
        using var conn = new SqliteConnection(connString);

        var command = conn.CreateCommand();
        command.CommandText = @"
            DELETE FROM habits 
            WHERE
                id = @ID;
        ";

        command.Parameters.Add("@ID", SqliteType.Integer);

        command.Parameters["@ID"].Value = id;

        try
        {
            conn.Open();
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UpdateHabit(Habit updatedHabit)
    {
        using var conn = new SqliteConnection(connString);

        var command = conn.CreateCommand();
        command.CommandText = @"
            UPDATE habits 
            SET name = @NAME,
                quantity = @QUANTITY,
                date = @DATE
            WHERE
                id = @ID;
        ";

        command.Parameters.Add("@NAME", SqliteType.Text);
        command.Parameters.Add("@QUANTITY", SqliteType.Integer);
        command.Parameters.Add("@DATE", SqliteType.Text);
        command.Parameters.Add("@ID", SqliteType.Integer);

        command.Parameters["@NAME"].Value = updatedHabit.Name;
        command.Parameters["@QUANTITY"].Value = updatedHabit.Quantity;
        command.Parameters["@DATE"].Value = updatedHabit.Date;
        command.Parameters["@ID"].Value = updatedHabit.Id;

        try
        {
            conn.Open();
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
