using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;

class Program
{
    static void Main(string[] args)
    {
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

        string connectionString = @"Data Source=habit-Tracker.db";
        using (var sqlConnection = new SqliteConnection(connectionString))
        {
            sqlConnection.Open();
            var tableCommand = sqlConnection.CreateCommand();
            tableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS coding (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    TimeSpentPracticing INTEGER
                )"; 
            tableCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        DisplayMainMenu();
    }

    static void DisplayMainMenu()
    {
        Console.WriteLine("________________________________");
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("Choose from the options below:");
        Console.WriteLine("Press 1 to View Records");
        Console.WriteLine("Press 2 to Insert Sample Record");
        Console.WriteLine("Press 3 to Delete Record");
        Console.WriteLine("Press 4 to Insert Record");
        Console.WriteLine("Press 0 to Quit the application");
        Console.WriteLine("________________________________");

        string input = Console.ReadLine();
        MenuChoice(input);
    }

    public static void MenuChoice(string message)
    {
        switch (message)
        {
            case "1":
                ViewRecords();
                break;
            case "2":
                InsertSampleRecord();
                break;
            case "3":
                
                break;
            case "4":
                
                break;
            case "0":
                Console.WriteLine("Quit the application...");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input, try again!");
                DisplayMainMenu();
                break;
        }
    }

    public static void ViewRecords()
    {
        string connectionString = @"Data Source=habit-Tracker.db";
        using (var sqlConnection = new SqliteConnection(connectionString))
        {
            sqlConnection.Open();
            var selectCommand = sqlConnection.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM coding";

            using (var reader = selectCommand.ExecuteReader())
            {
                bool hasRows = false;
                while (reader.Read())
                {
                    hasRows = true;
                    var id = reader.GetInt32(0);
                    var date = reader.GetString(1);
                    var timeSpentPracticing = reader.GetInt32(2);
                    Console.WriteLine($"Id: {id}, Date: {date}, Time Spent Practicing: {timeSpentPracticing}");
                }
                if (!hasRows)
                {
                    Console.WriteLine("No records found.");
                }
            }

            sqlConnection.Close();
        }

        DisplayMainMenu(); 
    }

    public static void InsertSampleRecord()
    {
        string connectionString = @"Data Source=habit-Tracker.db";
        using (var sqlConnection = new SqliteConnection(connectionString))
        {
            sqlConnection.Open();
            var insertCommand = sqlConnection.CreateCommand();
            insertCommand.CommandText = @"
                INSERT INTO coding (Date, TimeSpentPracticing)
                VALUES ('2023-06-13', 120)";
            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        Console.WriteLine("Sample record inserted.");
        DisplayMainMenu();  
    }
}
