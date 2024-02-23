using Microsoft.Data.Sqlite;

namespace habitTracker.fennikko;
public class RecordManipulation
{
    public static string connectionString = @"Data Source=HabitTracker.db";

    public static void InitialDatabaseCreation()
    {
        using var connection = new SqliteConnection(connectionString);
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER,
                )";
        connection.Close();
    }
    public static void GetUserInput()
    {
        Console.Clear();
        var appRunning = true;
        while (appRunning)
        {
            Console.WriteLine(@"MAIN MENU

What would you like to do?
0 - Close application
1 - View all records
2 - Insert a record
3 - Delete a record
4 - Update a record");
            Console.WriteLine("-----------------------------");

            var command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("Goodbye!");
                    appRunning = false;
                    break;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    Update();
                    break;
                default:
                    Console.WriteLine("Invalid command. Please type a number between 0 to 4");
                    break;
            }
        }
    }

    public static void Delete()
    {

    }

    public static void Update()
    {

    }

    public static void Insert()
    {

    }

    public static string GetDateInput()
    {

    }

    public static int GetNumberInput(string? message)
    {

    }
}