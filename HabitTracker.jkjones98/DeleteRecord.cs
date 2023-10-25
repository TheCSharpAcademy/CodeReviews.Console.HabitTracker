using Microsoft.Data.Sqlite;
using UserInput;
using ViewAllRecords;
using NumberInput;

namespace DeleteRecord;

public class Delete
{
    ClassUserInput rtnToMainMenu = new ClassUserInput();
    ViewAll getRecords = new ViewAll();
    GetNumberInput getInput = new GetNumberInput();
    string connectionString = @"Data Source=habit-Tracker2.db";
    public void DelRecMethod()
    {
        Console.Clear();
        // Show database
        getRecords.ViewAllMethod();
        // Get Id of record to update
        var recordId = getInput.GetNumInput("\n\nPlease enter the ID of the record you would like to remove from the database\n\n");
        // Show record and ask what they would like to update? enter date or quantity/1 or 2
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