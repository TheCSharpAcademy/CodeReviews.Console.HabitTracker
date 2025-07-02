using DotNETConsole.HabitTracker.DB;
using Microsoft.Data.Sqlite;

namespace DotNETConsole.HabitTracker.DbMigrations;

public class Habit
{
    public void Up()
    {
        try
        {
            var db = new DbConnection();
            db.Connection.Open();
            var command = db.Connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS Habits (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    TITLE STRING NOT NULL,
                                    UNIT STRING NOT NULL
                                  );";
            
            command.ExecuteNonQuery();
            Console.WriteLine("Habits Table is created");
            db.Connection.Close();

        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to create Habits table");
        }
    }
    
    public void Down()
    {
        var db = new DbConnection();
        
        try
        {
            db.Connection.Open();
            
            var command = db.Connection.CreateCommand();
            command.CommandText = @"
                DROP TABLE IF EXISTS Habits;";
            command.ExecuteNonQuery();
            Console.WriteLine("Habits Table is dropped");
            db.Connection.Close();
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to drop Habits table");
        }
    }
}
