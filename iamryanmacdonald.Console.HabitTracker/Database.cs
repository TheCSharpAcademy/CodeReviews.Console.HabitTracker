using System.Globalization;
using iamryanmacdonald.Console.HabitTracker.Models;
using Microsoft.Data.Sqlite;

namespace iamryanmacdonald.Console.HabitTracker;

public class Database
{
    private readonly string _path;

    public Database(string path)
    {
        _path = path;

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        using (var tableCommand = connection.CreateCommand())
        {
            tableCommand.CommandText =
                """
                CREATE TABLE IF NOT EXISTS habits (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    unit TEXT,
                    UNIQUE (name)
                )
                """;
            tableCommand.ExecuteNonQuery();
        }

        using (var tableCommand = connection.CreateCommand())
        {
            tableCommand.CommandText =
                """
                CREATE TABLE IF NOT EXISTS habit_entries (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    date TEXT,
                    habit_id INTEGER,
                    quantity INTEGER,
                    UNIQUE (date, habit_id),
                    CONSTRAINT fk_habit_id
                        FOREIGN KEY (habit_id)
                        REFERENCES habits (id)
                        ON DELETE CASCADE
                )
                """;
            tableCommand.ExecuteNonQuery();
        }

        connection.Close();
    }

    internal void DeleteHabit(int id)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;
        tableCmd.CommandText =
            "DELETE FROM habits WHERE id = @Id";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    internal Habit? GetHabit(string name)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();

        tableCmd.Parameters.Add("@Name", SqliteType.Text).Value = name;
        tableCmd.CommandText =
            "SELECT * FROM habits WHERE name = @Name";

        Habit? habit = null;

        var tableReader = tableCmd.ExecuteReader();

        if (tableReader.HasRows)
        {
            tableReader.Read();

            habit = new Habit
            {
                Id = tableReader.GetInt32(0),
                Name = tableReader.GetString(1),
                Unit = tableReader.GetString(2)
            };
        }

        connection.Close();

        return habit;
    }

    internal List<Habit> GetHabits()
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM habits ORDER BY name ASC";

        List<Habit> tableData = [];

        var tableReader = tableCmd.ExecuteReader();

        if (tableReader.HasRows)
            while (tableReader.Read())
                tableData.Add(new Habit
                {
                    Id = tableReader.GetInt32(0),
                    Name = tableReader.GetString(1),
                    Unit = tableReader.GetString(2)
                });

        connection.Close();

        return tableData;
    }

    internal void InsertHabit(string name, string unit)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.Parameters.Add("@Name", SqliteType.Text).Value = name;
        tableCmd.Parameters.Add("@Unit", SqliteType.Text).Value = unit;
        tableCmd.CommandText =
            "INSERT INTO habits(name, unit) VALUES (@Name, @Unit)";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    internal void UpdateHabit(int id, string name, string unit)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();

        tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;
        tableCmd.Parameters.Add("@Name", SqliteType.Text).Value = name;
        tableCmd.Parameters.Add("@Unit", SqliteType.Text).Value = unit;
        tableCmd.CommandText =
            "UPDATE habits SET name = @Name, unit = @Unit WHERE id = @Id";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    internal void DeleteHabitEntry(int id)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;
        tableCmd.CommandText =
            "DELETE FROM habit_entries WHERE id = @Id";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    internal HabitEntry? GetHabitEntry(DateOnly date, int habitId)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();

        tableCmd.Parameters.Add("@Date", SqliteType.Text).Value = date.ToString("dd/MM/yyyy");
        tableCmd.Parameters.Add("@HabitId", SqliteType.Integer).Value = habitId;
        tableCmd.CommandText =
            "SELECT * FROM habit_entries WHERE date = @Date AND habit_id = @HabitId";

        HabitEntry? habitEntry = null;

        var tableReader = tableCmd.ExecuteReader();

        if (tableReader.HasRows)
        {
            tableReader.Read();

            habitEntry = new HabitEntry
            {
                Date = DateOnly.ParseExact(tableReader.GetString(1), "dd/MM/yyyy", new CultureInfo("en-US")),
                HabitId = tableReader.GetInt32(2),
                Id = tableReader.GetInt32(0),
                Quantity = tableReader.GetInt32(3)
            };
        }

        connection.Close();

        return habitEntry;
    }

    internal List<HabitEntry> GetHabitEntries(int habitId)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.Parameters.Add("@HabitId", SqliteType.Integer).Value = habitId;
        tableCmd.CommandText = "SELECT * FROM habit_entries WHERE habit_id = @HabitId  ORDER BY date DESC";

        List<HabitEntry> tableData = [];

        var tableReader = tableCmd.ExecuteReader();

        if (tableReader.HasRows)
            while (tableReader.Read())
                tableData.Add(new HabitEntry
                {
                    Date = DateOnly.ParseExact(tableReader.GetString(1), "dd/MM/yyyy", new CultureInfo("en-US")),
                    HabitId = tableReader.GetInt32(2),
                    Id = tableReader.GetInt32(0),
                    Quantity = tableReader.GetInt32(3)
                });

        connection.Close();

        return tableData;
    }

    internal void InsertHabitEntry(DateOnly date, int habitId, int quantity)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.Parameters.Add("@Date", SqliteType.Text).Value = date.ToString("dd/MM/yyyy");
        tableCmd.Parameters.Add("@HabitId", SqliteType.Integer).Value = habitId;
        tableCmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = quantity;
        tableCmd.CommandText =
            "INSERT INTO habit_entries(date, habit_id, quantity) VALUES (@Date, @HabitId, @Quantity)";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    internal void UpdateHabitEntry(int id, DateOnly date, int quantity)
    {
        using var connection = new SqliteConnection($"Data Source={_path}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.Parameters.Add("@Date", SqliteType.Text).Value = date.ToString("dd/MM/yyyy");
        tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;
        tableCmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = quantity;
        tableCmd.CommandText =
            "UPDATE habit_entries SET date = @Date, quantity = @Quantity WHERE id = @Id";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
}