using Microsoft.Data.Sqlite;
using ColumnName;
using DisplayRecords;


namespace ChangeMeasurement;

public class ChangeMeasurementUnit
{
    GetColumnName currentName = new GetColumnName();
    AllRecords printTable = new AllRecords();
    string connectionString = @"Data Source=habit-Tracker2.db";

    public string storeColName;
    public void MeasurementUnit()
    {
        string oldColName = currentName.GetColName();
        Console.WriteLine("What would you like the name column name to be?");
        string newColName = Console.ReadLine();

        bool checkColumn;

        if(checkColumn = newColName.Any(char.IsDigit))
        {
            Console.Clear();
            Console.WriteLine("Please enter a column name without numbers");
            MeasurementUnit();
        }
        else
        {
            // ALTER name
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var insertCommand = connection.CreateCommand();

                insertCommand.CommandText = $"ALTER TABLE drinking_water RENAME COLUMN {oldColName} TO {newColName}";

                // Don't return any values, not querying any values
                insertCommand.ExecuteNonQuery();

                connection.Close();
            }

            System.Console.WriteLine("Column name changed");
            printTable.DisplayRecs();
        }

        
    }
}