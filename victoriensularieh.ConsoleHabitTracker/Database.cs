using Microsoft.Data.Sqlite;

namespace ConsoleHabitTracker;

static class Database
{
    public static string connectionString = @"Data Source=HabitTracker.db";

    public static Boolean DatabaseExists()
    {
        if (File.Exists("HabitTracker.db"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void PrepareDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Habit (
         ID INTEGER PRIMARY KEY AUTOINCREMENT,
         Name TEXT,     
         UnitID INTEGER         
        )";
            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Entry (
         ID INTEGER PRIMARY KEY AUTOINCREMENT,
         HabitID INTEGER,
         Date TEXT,
         Quantity INTEGER
        )";
            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Unit (
         ID INTEGER PRIMARY KEY AUTOINCREMENT,
         Name TEXT,
         Symbol TEXT
        )";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public static void SetDefaults()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"INSERT INTO Unit (Name,Symbol) VALUES ('Glasses','Gl')";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}