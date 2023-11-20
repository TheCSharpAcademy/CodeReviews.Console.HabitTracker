using Microsoft.Data.Sqlite;
using UserInput;
using DisplayRecords;
using NumberInput;

namespace DeleteRecord;

public class Delete
{
    ClassUserInput rtnToMainMenu = new ClassUserInput();
    AllRecords getRecords = new AllRecords();
    GetNumberInput getInput = new GetNumberInput();
    string connectionString = @"Data Source=habit-Tracker2.db";
    public void DelRecMethod()
    {
        Console.Clear();
        getRecords.DisplayRecs();
        var recordId = getInput.GetNumInput("\n\nPlease enter the ID of the record you would like to remove from the database\n\n");
        
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tblCommand = connection.CreateCommand();
            tblCommand.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

            int rowCount = tblCommand.ExecuteNonQuery();
            if(rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                DelRecMethod();
            }
            connection.Close();
        }
        Console.WriteLine($"\n\nRecord with Id {recordId} has been deleted. \n\n");
        rtnToMainMenu.GetUserInput();
    }
}