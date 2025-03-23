namespace CodeReviews_Console_HabitTracker;
using Microsoft.Data.Sqlite;

class Program
{
    class LogData
    {
        public DateTime date;
        public int quantity;
    }
    static void Main(string[] args)
    {
        string connectionString = @"Data Source=habit-Tracker.db";
        int userInput = 0;
        bool shouldContinue = true;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Create table
            CreateTable(connection);

            // Do while not exit
            do
            {
                // Offer menu to user
                PrintUI();

                // Get user input
                Console.Write("\nInput: ");
                userInput = GetUserInput("Invalid Input");

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
                Console.Clear();
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
    static LogData AddData()
    {
        LogData newLog = new();

        Console.Clear();
        Console.WriteLine("Adding log to DB");
        Console.WriteLine("Enter Date: ");
        newLog.date = GetDate();
        
        do
        {
            Console.Write("Quantity (# of glasses): ");
        }
        while(!int.TryParse(Console.ReadLine(), out newLog.quantity));

        Console.WriteLine($"Log ({newLog.date}, {newLog.quantity} glasses) added. Press enter to continue.");
        Console.Read();
        
        return newLog;
    }

    static void FindData(){}

    static void UpdateData(){}

    static void DeleteData(){}

    // Get a valid date from user
    // TODO: fancy error handling
    static DateTime GetDate()
    {
        DateTime returnDate = new();
        string date = "";

        do
        {
            Console.Write("Month: ");
            date += Console.ReadLine().PadLeft(2, '0') + '/';
            Console.Write("Day: ");
            date += Console.ReadLine().PadLeft(2, '0') + '/';
            Console.Write("Year: ");
            date += Console.ReadLine().PadLeft(4, '0');
        }
        while(!DateTime.TryParse(date, out returnDate));

        return returnDate;
    }
}