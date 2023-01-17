using MapDataReader;
using Microsoft.Data.Sqlite;

namespace HabitTracker.edvaudin;

internal class DataAccessor
{
    static readonly string connectionString = @"Data Source=habit_tracker.db";

    public static void CreateHabitsTableIfMissing()
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = @"CREATE TABLE IF NOT EXISTS habits (
                       id INTEGER,
                       name TEXT NOT NULL,
                       measurement TEXT NOT NULL,
                       PRIMARY KEY(id AUTOINCREMENT));";
        SqliteCommand cmd = new(sql, conn);
        cmd.ExecuteNonQuery();
    }

    public static void CreateTrackerTableIfMissing()
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = @"CREATE TABLE IF NOT EXISTS tracker (
	                    id INTEGER,
	                    date TEXT NOT NULL,
	                    quantity INTEGER NOT NULL,
	                    habit_id INTEGER NOT NULL,
	                    PRIMARY KEY(id AUTOINCREMENT),
	                    FOREIGN KEY(habit_id) REFERENCES habits);";
        SqliteCommand cmd = new(sql, conn);
        cmd.ExecuteNonQuery();
    }

    public static void CreateHabit(string name, string measurement)
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = "INSERT INTO habits (name, measurement) VALUES (@name, @measurement);";
        SqliteCommand cmd = new(sql, conn);
        AddParameter("@name", name, cmd);
        AddParameter("@measurement", measurement, cmd);
        cmd.ExecuteNonQuery();
    }

    public static List<Habit> GetHabits()
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = "SELECT * FROM habits;";
        SqliteCommand cmd = new(sql, conn);
        return cmd.ExecuteReader().ToHabit();
    }

    public static Habit GetHabit(int id)
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = "SELECT * FROM habits WHERE id = @id;";
        SqliteCommand cmd = new(sql, conn);
        AddParameter("@id", id, cmd);
        return cmd.ExecuteReader().ToHabit().First();
    }

    public static void AddEntry(string date, int quantity, int habitId)
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = "INSERT INTO tracker (date, quantity, habit_id) VALUES (@date, @quantity, @habit_id);";
        SqliteCommand cmd = new(sql, conn);
        AddParameter("@date", date, cmd);
        AddParameter("@quantity", quantity, cmd);
        AddParameter("@habit_id", habitId, cmd);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteEntry(int id)
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = "DELETE FROM tracker WHERE id = @id;";
        SqliteCommand cmd = new(sql, conn);
        AddParameter("@id", id, cmd);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateEntry(int id, int quantity, string date)
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = "UPDATE tracker SET quantity = @quantity, date = @date WHERE id = @id;";
        SqliteCommand cmd = new(sql, conn);
        AddParameter("@id", id, cmd);
        AddParameter("@quantity", quantity, cmd);
        AddParameter("@date", date, cmd);
        cmd.ExecuteNonQuery();
    }

    public static Entry GetHighestEntryForHabit(int habitId)
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = @"SELECT tracker.id, tracker.date, tracker.quantity, habits.measurement FROM tracker
                       JOIN habits ON tracker.habit_id = habits.id
                       WHERE tracker.habit_id = @habitId
                       ORDER BY quantity DESC;";
        SqliteCommand cmd = new(sql, conn);
        AddParameter("@habitId", habitId, cmd);
        return cmd.ExecuteReader().ToEntry().First();
    }

    public static List<Entry> GetEntries()
    {
        using SqliteConnection conn = new(connectionString);
        conn.Open();
        string sql = @"SELECT tracker.id, tracker.date, tracker.quantity, habits.measurement FROM tracker
                       JOIN habits on tracker.habit_id = habits.id;";
        SqliteCommand cmd = new(sql, conn);
        return cmd.ExecuteReader().ToEntry();
    }

    protected static void AddParameter<T>(string name, T value, SqliteCommand cmd)
    {
        SqliteParameter param = new(name, SqlDbTypeConverter.GetDbType(value.GetType()))
        {
            Value = value
        };
        cmd.Parameters.Add(param);
    }
}
