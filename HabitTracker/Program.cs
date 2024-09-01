using System.Text.RegularExpressions;
using HabitTracker;
using Microsoft.Data.Sqlite;

const string dbName = "habit_tracker.db";
const string tableName = "habits";

Repository repository;

try
{
    string dbPath = Utils.GetCorrectPathToStoreDatabase(dbName);
    var connection = Utils.SystemStartUpCheck(dbPath, tableName);
    repository = new Repository(connection);
    bool exitApp = false;
    
    Menu.DisplayWelcomeMessage();
    
    while (!exitApp)
    {
        Menu.DisplayMenuOptions();
    
        string? input = Console.ReadLine();
        
        if (input == null || ! Regex.IsMatch(input, "[1|2|3|4|5|6]"))
        {
            Console.WriteLine("Error: Unrecognized input.");
        }
        else
        {
            int action = int.Parse(input);
            
            switch (action)
            {
                case 1:
                    repository.ViewAllRecords();
                    break;
                case 2:
                    repository.InsertRecord();
                    break;
                case 3:
                    repository.UpdateRecord();
                    break;
                case 4:
                    repository.DeleteRecord();
                    break;
                case 5:
                    exitApp = true;
                    break;
                case 6:
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