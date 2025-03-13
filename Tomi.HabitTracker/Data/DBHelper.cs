using System.Data.SQLite;

namespace Tomi.HabitTracker.Data;

public class DBHelper
{
    private readonly string _connectionString;

    public DBHelper(string connectionString)
    {
        _connectionString = connectionString;
        InitDB();

    }

    private void InitDB()
    {
        if (File.Exists("../habbit-logger.db")) return;
        try
        {
            CreateTable();
            Console.WriteLine("habit-logger data store initialized successfully");
        }
        catch (Exception err)
        {
            Console.WriteLine($"Can't connect to the data store at the moment: {err}");
            Environment.Exit(0);
        }
    }

    private void CreateTable()
    {
        using (SQLiteConnection connection = new(_connectionString))
        {
            try
            {
                connection.Open();
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS habbit_logger(id INTEGER PRIMARY KEY AUTOINCREMENT, habbit TEXT NOT NULL, quantity REAL NOT NULL, date TEXT NOT NULL)";

                using (SQLiteCommand command = new(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }



}
