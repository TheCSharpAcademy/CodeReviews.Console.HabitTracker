using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class Repository(SqliteConnection connection)
{
    public HashSet<int> ViewAllRecords(string viewAllRecordsCommand)
    {
        using var command = new SqliteCommand(viewAllRecordsCommand, connection);
        using var reader = command.ExecuteReader();
        HashSet<int> recordIndexes = [];
        
        Console.WriteLine("Displaying all habit records ...");

        if (!reader.HasRows)
        {
            Console.WriteLine("\nNo habit records found!");
            return recordIndexes;
        }

        Console.WriteLine($"\n{"Id", -5}\t{"Date", -12}\t{"Habit", -30}\t{"Unit", -10}\t{"Quantity", 10}");
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            var date = reader.GetDateTime(1);
            string habit = reader.GetString(2);
            string unit = reader.GetString(3);
            int quantity = reader.GetInt32(4);
            recordIndexes.Add(id);
            Console.WriteLine($"{id, -5}\t{date.ToShortDateString(), -12}\t{habit, -30}\t{unit, -10}\t{quantity, 10}");
        }

        return recordIndexes;
    }

    public void InsertRecord(string insertRecordCommand)
    {
        using var command = new SqliteCommand(insertRecordCommand, connection);
        
        var date = Utils.GetDateInput();
        string habit = Utils.GetAlphabeticalInput("habit");
        string unit = Utils.GetAlphabeticalInput("unit");
        int quantity = Utils.GetQuantityInput();
        
        command.Parameters.AddWithValue("@date", date);
        command.Parameters.AddWithValue("@habit", habit);
        command.Parameters.AddWithValue("@unit", unit);
        command.Parameters.AddWithValue("@quantity", quantity);
        
        command.ExecuteNonQuery();
    }

    public void UpdateRecord(string updateRecordCommand, string viewAllRecordsCommand)
    {
        var recordIndexes = ViewAllRecords(viewAllRecordsCommand);

        if (recordIndexes.Count == 0)
        {
            Console.WriteLine("No records to update!");
        }
        else
        {
            using var command = new SqliteCommand(updateRecordCommand, connection);
            
            int idToUpdate = Utils.GetIdOfRecord(recordIndexes, "update");
            
            var updatedHabitDate = Utils.GetDateInput();
            string updatedHabit = Utils.GetAlphabeticalInput("habit");
            string updatedUnit = Utils.GetAlphabeticalInput("unit");
            int updatedHabitQuantity = Utils.GetQuantityInput();

            command.Parameters.AddWithValue("@updatedDate", updatedHabitDate);
            command.Parameters.AddWithValue("@updatedHabit", updatedHabit);
            command.Parameters.AddWithValue("@updatedUnit", updatedUnit);
            command.Parameters.AddWithValue("@updatedQuantity", updatedHabitQuantity);
            command.Parameters.AddWithValue("@id", idToUpdate);

            command.ExecuteNonQuery();
        }
    }

    public void DeleteRecord(string viewAllRecordsCommand, string deleteRecordCommand)
    {
        var recordIndexes = ViewAllRecords(viewAllRecordsCommand);

        if (recordIndexes.Count == 0)
        {
            Console.WriteLine("No records to delete!");
        }
        else
        {
            using var command = new SqliteCommand(deleteRecordCommand, connection);
            
            int idToDelete = Utils.GetIdOfRecord(recordIndexes, "delete");

            command.Parameters.AddWithValue("@id", idToDelete);

            command.ExecuteNonQuery();
        }
    }
}