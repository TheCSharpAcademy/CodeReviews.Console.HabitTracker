using HabitLogger;
using Microsoft.Data.Sqlite;


public class Start
{
    public static void Main()
    {
        using (var connection = new SqliteConnection("Data Source=Logger.db"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS Logger (
                ID  INTEGER PRIMARY KEY AUTOINCREMENT,
                Habit TEXT NOT NULL,
                ""Unit of Measurement"" TEXT NOT NULL,
                Units REAL NOT NULL
                )";
            command.ExecuteNonQuery();
            connection.Close();
        }
        UserInterface userInterface = new UserInterface();
        userInterface.Start();   
    }
}
