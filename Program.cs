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

            // Do while not exit
            bool shouldContinue = true;
            do
            {
                // Offer menu to user
                Console.Clear();
                PrintUI();

                // Get user input
                Console.Write("\nInput: ");
                int userInput = GetUserInput("Invalid Input");

                // Handle input
                switch (userInput)
                {
                    case 0:
                        AddData();
                        break;
                    case 1:
                        FindData();
                        break;
                    case 2:
                        UpdateData();
                        break;
                    case 3:
                        DeleteData();
                        break;
                    // Exit
                    case 4:
                        shouldContinue = false;
                        break;
                    default:
                        break;
                }
                
            } while(shouldContinue);
            
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

    static void PrintUI()
    {
        Console.WriteLine("Habit Log Program: ");
        Console.WriteLine("\t[0] - Create log");
        Console.WriteLine("\t[1] - Find log");
        Console.WriteLine("\t[2] - Update log");
        Console.WriteLine("\t[3] - Delete log");
        Console.WriteLine("\t[4] - Exit");
    }

    static int GetUserInput(string errorMessage)
    {
        int parsedInt;
        while (!Int32.TryParse(Console.ReadLine(), out parsedInt))
        {
            Console.Write($"{errorMessage}: ");
        }

        return parsedInt;
    }
    static void AddData(){}

    static void FindData(){}

    static void UpdateData(){}

    static void DeleteData(){}
}