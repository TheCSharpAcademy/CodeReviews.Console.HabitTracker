using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;
using System.Data;

string connectionString = @"Data Source=habit-Tracker.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText = 
        @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";

    tableCmd.ExecuteNonQuery();

    connection.Close();
}

static void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (!closeApp)
    {
        Console.WriteLine("\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to close the app");
        Console.WriteLine("Type 1 to View all records");
        Console.WriteLine("Type 2 to Insert record");
        Console.WriteLine("Type 3 to Delete record");
        Console.WriteLine("Type 4 to Update record");
        Console.WriteLine("--------------------------------------------\n");

        string commandInput = Console.ReadLine();

        switch (commandInput)
        {
            case 0:
                Console.WriteLine("\nGoodbye!\n");
                closeApp = true;
                break;
            case 1:
                GetAllRecords();
                break;
            case 2:
                Insert();
                break;
            case 3:
                Delete();
                break;
            case 4:
                Update();
                break;
            default:
                Console.WriteLine("\nInvalid command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}
