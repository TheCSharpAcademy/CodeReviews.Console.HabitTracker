using System.Data.SQLite;

namespace HabitTracker.kalsson;

public class DatabaseManager
{
    /// <summary>
    /// Initialize SQLite database if it doesn't exist.
    /// </summary>
    public static void InitializeDatabase() 
    {
        try
            {
            string dbName = "HabitTracker.db";

            if (!File.Exists(dbName))
                {
                // Create a new database if it doesn't exist
                SQLiteConnection.CreateFile(dbName);
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine("An error occurred: " + ex.Message);
            }
    }
    
    /// <summary>
    /// Create table for storing habits.
    /// </summary>
    public static void CreateTable() 
    {
        try
            {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=HabitTracker.db;Version=3;"))
                {
                conn.Open();

                string sql = @"CREATE TABLE IF NOT EXISTS habits (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            quantity INTEGER NOT NULL,
                            unit TEXT NOT NULL
                           );";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                    cmd.ExecuteNonQuery();
                    }

                conn.Close();
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine("An error occurred: " + ex.Message);
            }
    }
}