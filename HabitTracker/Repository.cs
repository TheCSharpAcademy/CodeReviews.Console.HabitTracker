using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class Repository(SqliteConnection connection)
{
    public HashSet<int> ViewAllRecords()
    {
        using var command = new SqliteCommand(Queries.ViewAllRecordsCommand, connection);
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

    public void InsertRecord()
    {
        using var command = new SqliteCommand(Queries.InsertRecordCommand, connection);
        
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

    public void UpdateRecord()
    {
        var recordIndexes = ViewAllRecords();

        if (recordIndexes.Count == 0)
        {
            Console.WriteLine("No records to update!");
        }
        else
        {
            using var command = new SqliteCommand(Queries.UpdateRecordCommand, connection);
            
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

    public void DeleteRecord()
    {
        var recordIndexes = ViewAllRecords();

        if (recordIndexes.Count == 0)
        {
            Console.WriteLine("No records to delete!");
        }
        else
        {
            using var command = new SqliteCommand(Queries.DeleteRecordCommand, connection);
            
            int idToDelete = Utils.GetIdOfRecord(recordIndexes, "delete");

            command.Parameters.AddWithValue("@id", idToDelete);

            command.ExecuteNonQuery();
        }
    }

    public void ViewReportOfHabits()
    {
        var recordIndexes = ViewAllRecords();

        if (recordIndexes.Count == 0)
        {
            Console.WriteLine("No habits to report on!");
        }
        else
        {
            
        }
    }
}