using System.Text.RegularExpressions;
using HabitTracker;
using Microsoft.Data.Sqlite;

const string dbName = "habit_tracker.db";
const string tableName = "habits";
const string insertRecordCommand = """
                                   INSERT INTO habits (date, habit, unit, quantity) 
                                   VALUES (@date, @habit, @unit, @quantity)
                                   """;
const string viewAllRecordsCommand = "SELECT id, date, habit, unit, quantity FROM habits";
const string updateRecordCommand = """
                                   UPDATE habits 
                                   SET date = @updatedDate, habit = @updatedHabit, 
                                       unit = @updatedUnit, quantity = @updatedQuantity 
                                   WHERE id = @id
                                   """;
const string deleteRecordCommand = "DELETE FROM habits WHERE id = @id";

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
                    repository.ViewAllRecords(viewAllRecordsCommand);
                    break;
                case 2:
                    repository.InsertRecord(insertRecordCommand);
                    break;
                case 3:
                    repository.UpdateRecord(updateRecordCommand, viewAllRecordsCommand);
                    break;
                case 4:
                    repository.DeleteRecord(viewAllRecordsCommand, deleteRecordCommand);
                    break;
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