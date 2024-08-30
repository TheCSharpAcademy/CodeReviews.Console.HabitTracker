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

        Console.WriteLine($"\n{"Index", -5}\t{"Date", -12}\t{"Habit", -30}\t{"Quantity", 10}");
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            var date = reader.GetDateTime(1);
            string habit = reader.GetString(2);
            int quantity = reader.GetInt32(3);
            recordIndexes.Add(id);
            Console.WriteLine($"{id, -5}\t{date.ToShortDateString(), -12}\t{habit, -30}\t{quantity, 10}");
        }

        return recordIndexes;
    }

    public void InsertRecord(string insertRecordCommand)
    {
        using var command = new SqliteCommand(insertRecordCommand, connection);
        
        var habitDate = Utils.GetDateInput();
        string habit = Utils.GetHabitInput();
        int habitQuantity = Utils.GetQuantityInput();
        
        command.Parameters.AddWithValue("@date", habitDate);
        command.Parameters.AddWithValue("@habit", habit);
        command.Parameters.AddWithValue("@quantity", habitQuantity);
        
        command.ExecuteNonQuery();
    }

    public void UpdateRecord(string updateRecordCommand, string viewAllRecordsCommand)
    {
        var recordIndexes = ViewAllRecords(viewAllRecordsCommand);

        if (recordIndexes.Count == 0)
        {
            Console.WriteLine("Nothing to update!");
        }
        else
        {
            string? input;
            bool validIndexEntered = false;

            while (!validIndexEntered)
            {
                Console.Write("Enter the index of the record that you want to update: ");
                input = Console.ReadLine();

                if (input != null || int.TryParse(input, out int index) == false)
                {
                    Console.WriteLine("Error: Invalid Input");
                }
                else if (!recordIndexes.Contains(index))
                {
                    Console.WriteLine("Error: Index entered is not in the list shown.");
                }
                else
                {
                    validIndexEntered = true;
                }
            }
            
            var updatedHabitDate = Utils.GetDateInput();
            string updatedHabit = Utils.GetHabitInput();
            int updatedHabitQuantity = Utils.GetQuantityInput();
        }
    }
}