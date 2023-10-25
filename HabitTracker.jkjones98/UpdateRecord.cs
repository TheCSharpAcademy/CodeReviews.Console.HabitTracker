using Microsoft.Data.Sqlite;
using UserInput;
using NumberInput;
using DisplayRecords;
using DateInput;
using ColumnName;

namespace UpdateRecord;

public class Update
{
    AllRecords getRecords = new AllRecords();
    UserDateInput getDate = new UserDateInput();
    GetNumberInput getNum = new GetNumberInput();
    ClassUserInput mainMenu = new ClassUserInput();
    GetColumnName columnName = new GetColumnName();
    string connectionString = @"Data Source=habit-Tracker2.db";
    public void UpdateRecMethod()
    {
        string currentColumnName = columnName.GetColName();

        // Show database
        getRecords.DisplayRecs();
        // Get Id of record to update
        var recordId = getNum.GetNumInput("\n\nPlease enter the ID of the record you would like to Update in the database\n\n");
        // Show record and ask what they would like to update? enter date or quantity/1 or 2

        using(var connection = new SqliteConnection(connectionString))
        {
            // Open connection
            // ALTER command
            connection.Open();

            var tblCmd = connection.CreateCommand();
            tblCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            // Execute scalar as we want a value returned from the database
            int checkQuery = Convert.ToInt32(tblCmd.ExecuteScalar());

            if(checkQuery == 0)
            {
                Console.WriteLine($"\nRecord with Id {recordId} does not exist");
                connection.Close();
                UpdateRecMethod();
            }

            // Get the new date
            string date = getDate.getDate("\nEnter the updated date with the following format 'DD-MM-YYYY'\n");
            // Get new number of litres drank
            int quantity = getNum.GetNumInput("\nEnter quantity for chosen measurement\n");

            // Update the date and quantity where the Id = selected by user
            tblCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', {currentColumnName} = {quantity} WHERE Id = {recordId}";

            tblCmd.ExecuteNonQuery();
            connection.Close();
        }

        Console.WriteLine("\nDatabase has been updated - see below");
        getRecords.DisplayRecs();
        mainMenu.GetUserInput();
    }
}