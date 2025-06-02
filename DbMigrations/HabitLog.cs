using DotNETConsole.HabitTracker.DB;
using Microsoft.Data.Sqlite;

namespace DotNETConsole.HabitTracker.DbMigrations;
public class HabitLog
{
    public void Up()
    {
        try
        {
            var db = new DbConnection();
            db.Connection.Open();

            var command = db.Connection.CreateCommand();

            command.CommandText = @"CREATE TABLE IF NOT EXISTS HabitLogs (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    DATE DATETIME NOT NULL,
                                    QUANTITY INT NOT NULL,
                                    HABITID INT NOT NULL,
                                    FOREIGN KEY(HABITID) REFERENCES Habits(ID) ON DELETE CASCADE
                                  );";

            command.ExecuteNonQuery();
            db.Connection.Close();
            Console.WriteLine("HabitLogs table Created!");
        }
        catch (SqliteException ex)
        {
            Console.WriteLine("Failed to create HabitLogs table.");
            Console.WriteLine(ex.Message);
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
                DROP TABLE IF EXISTS HabitLogs;";
            command.ExecuteNonQuery();
            Console.WriteLine("HabitLogs Table is dropped");
            db.Connection.Close();
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to drop HabitLogs table");
        }
    }
}
