namespace CodeReviews_Console_HabitTracker;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = @"Data Source=habit-Tracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Create table
            CreateTable(connection);

            // Offer menu to user
            
            connection.Close();
        }
    }

    // Creates table if existing table does not exist
    static void CreateTable(SqliteConnection connection)
    {
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = 
        @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";

        tableCmd.ExecuteNonQuery();
    }

    static int GetUserInput()
    {
        int parsedInt;
        string input = Console.ReadLine();
        while (!Int32.TryParse(input, out parsedInt))
        {
            Console.Write("Invalid input: ");
            input = Console.ReadLine();
        }

        return parsedInt;
    }
}