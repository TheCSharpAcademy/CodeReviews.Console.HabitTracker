using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class Repository(SqliteConnection connection)
{
    public void ViewAllRecords(string viewAllRecordsCommand)
    {
        using var command = new SqliteCommand(viewAllRecordsCommand, connection);
        using var reader = command.ExecuteReader();
        
        Console.WriteLine("Displaying all habit records ...");

        if (!reader.HasRows)
        {
            Console.WriteLine("\nNo habit records found!");
            return;
        }

        Console.WriteLine("\nIndex\tDate\t\tHabit\t\tQuantity");
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            var date = reader.GetDateTime(1);
            string habit = reader.GetString(2);
            int quantity = reader.GetInt32(3);
            Console.WriteLine($"{id}\t{date.ToShortDateString()}\t{habit}\t{quantity}");
        }
    }
}