namespace CodeReviews_Console_HabitTracker;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;

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
                        LogData data = GetLogData();
                        AddData(data, connection);
                        break;
                    case 1:
                        FindData(connection);
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

    static void AddData(LogData data, SqliteConnection connection)
    {
        SqliteCommand command = connection.CreateCommand();
        
        command.CommandText = 
        $@"INSERT 
            INTO drinking_water (Date, Quantity)
            VALUES ('{data.date.Date:d}', {data.quantity})";
        
        try
        {
            command.ExecuteNonQuery();
        }
        catch (Exception e) { Console.WriteLine(e); Console.Read(); }
    }

    static void FindData(SqliteConnection connection)
    {
        SqliteCommand command = connection.CreateCommand();

        command.CommandText = $@"SELECT * FROM drinking_water";
        
        Console.Clear();
        ReadData(command);

        Console.WriteLine("\nPress Enter to continue");
        Console.Read();
    }

    // Reads data gotten from passed command
    static void ReadData(SqliteCommand command)
    {
        try
        {
            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
                Console.WriteLine("Empty DB");
            
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");
            }
        }
        catch (Exception e) { Console.WriteLine(e); Console.Read(); }
    }

    static void UpdateData(){}

    static void DeleteData(){}

    static void PrintUI()
    {
        Console.WriteLine("Habit Log Program: ");
        Console.WriteLine("\t[0] - Create log");
        Console.WriteLine("\t[1] - Retrieve all logs");
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

    static LogData GetLogData()
    {
        LogData newLog = new();

        Console.Clear();
        Console.WriteLine("Adding log to DB\n");
        Console.WriteLine("Enter Date: ");

        newLog.date = GetDate();
        newLog.quantity = GetQuantity();

        Console.WriteLine($"Log ({newLog.date.Date:d}, {newLog.quantity} glasses) added. Press enter to continue.");
        Console.Read();
        
        return newLog;
    }

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

    static int GetQuantity()
    {
        int quantity = 0;

        do
        {
            Console.Write("Quantity (# of glasses): ");
        }
        while(!int.TryParse(Console.ReadLine(), out quantity));

        return quantity;
    }

}