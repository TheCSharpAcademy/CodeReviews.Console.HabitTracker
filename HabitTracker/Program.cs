using System.Text.RegularExpressions;
using HabitTracker;
using Microsoft.Data.Sqlite;

const string dbName = "habit_tracker.db";
const string tableName = "habits";
const string checkTableExistsQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName;";
const string createTableCommand = """
                                  CREATE TABLE habits (
                                      id INTEGER PRIMARY KEY,
                                      date DATE NOT NULL,
                                      habit TEXT NOT NULL,
                                      quantity INT NOT NULL
                                  )
                                  """;
const string insertRecordCommand = """
                                   INSERT INTO habits (date, habit, quantity) 
                                   VALUES (@date, @habit, @quantity)
                                   """;
const string viewAllRecordsCommand = "SELECT id, date, habit, quantity FROM habits";
const string updateRecordCommand = """
                                   UPDATE habits SET date = @updatedDate, habit = @updatedHabit, quantity = @updatedQuantity 
                                   WHERE id = @id
                                   """;

Repository repository;

try
{
    string dbPath = GetCorrectPathToStoreDatabase();
    var connection = SystemStartUpCheck(dbPath, tableName);
    repository = new Repository(connection);
    bool exitApp = false;
    
    Menu.DisplayWelcomeMessage();
    
    while (!exitApp)
    {
        Menu.DisplayMenuOptions();
    
        string? input = Console.ReadLine();
        
        if (input == null || ! Regex.IsMatch(input, "[1|2|3|4|5]"))
        {
            Console.WriteLine("Error: Unrecognized input.");
        }
        else
        {
            int action = int.Parse(input);
            
            switch (action)
            {
                case 1:
                {
                    repository.ViewAllRecords(viewAllRecordsCommand);
                    break;
                }
                case 2:
                {
                    repository.InsertRecord(insertRecordCommand);
                    break;
                }
                case 3:
                {
                    repository.UpdateRecord(updateRecordCommand, viewAllRecordsCommand);
                    break;
                }
                case 5:
                    exitApp = true;
                    break;
            }
        }
    }
}
catch (SqliteException ex)
{
    Console.WriteLine(ex.Message);
}

return;

static SqliteConnection SystemStartUpCheck(string dbPath, string tableName)
{
    Console.WriteLine("Performing application start up checks...");
    var connection = new SqliteConnection($"Data Source={dbPath}");
    connection.Open();

    Console.WriteLine("Successfully connected to SQLite database!");

    if (!CheckTableExists(connection, tableName))
    {
        CreateTable(connection, tableName);
        Console.WriteLine($"{tableName} table created!");
    }
    else
    {
        Console.WriteLine($"{tableName} table found!");
    }

    return connection;
}

static bool CheckTableExists(SqliteConnection connection, string tableName)
{
    
    using var command = new SqliteCommand(checkTableExistsQuery, connection);
    command.Parameters.AddWithValue("@tableName", tableName);
    using var reader = command.ExecuteReader();

    return reader.HasRows;
}

static void CreateTable(SqliteConnection connection, string tableName)
{
    using var command = new SqliteCommand(createTableCommand, connection);
    command.Parameters.AddWithValue("@tableName", tableName);

    command.ExecuteNonQuery();
    Console.WriteLine($"{tableName} table successfully created.");
}

static string GetCorrectPathToStoreDatabase()
{
    string curPath = Directory.GetCurrentDirectory();
    var directoryInfo = Directory.GetParent(curPath);

    for (int i = 0; i < 2; i++)
    {
        if (directoryInfo != null)
        {
            directoryInfo = directoryInfo.Parent;
        }
        else
        {
            break;
        }
    }

    string dbPath = directoryInfo?.FullName + $"/{dbName}";

    return dbPath;
}