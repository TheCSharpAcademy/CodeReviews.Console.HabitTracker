namespace DotNETConsole.HabitTracker.DbMigrations;

using Microsoft.Data.Sqlite;
using DB;

public class User
{
    public void Up()
    {
        var db = new DbConnection();
        
        try
        {
            db.Connection.Open();
            
            var command = db.Connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL
                );";
            command.ExecuteNonQuery();
            Console.WriteLine("User Table is created");
            db.Connection.Close();
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to create User table");
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
                DROP TABLE IF EXISTS Users;";
            command.ExecuteNonQuery();
            Console.WriteLine("User Table is dropped");
            db.Connection.Close();
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to drop User table");
        }
    }
}