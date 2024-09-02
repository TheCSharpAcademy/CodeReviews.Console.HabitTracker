using System.Globalization;
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

    private bool CheckRecordsExist()
    {
        using var command = new SqliteCommand(Queries.ViewAllRecordsCommand, connection);
        using var reader = command.ExecuteReader();
        
        return reader.HasRows;
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
        if (!CheckRecordsExist())
        {
            Console.WriteLine("No habits to report on!");
        }
        else
        {
            string[] result = GetHabitToGenerateReport();
            string habitToGenerateReport = result[0];
            string unitOfHabitToGenerateReport = result[1];
            
            int totalOccurrencesOfHabit =
                GetTotalOccurrencesOfAHabit(habitToGenerateReport, unitOfHabitToGenerateReport);
            int totalQuantityOfHabit =
                GetTotalQuantityOfAHabit(habitToGenerateReport, unitOfHabitToGenerateReport);
            int totalOccurrencesOfHabitThisYear =
                GetTotalOccurrencesOfAHabitInTheCurrentYear(habitToGenerateReport, unitOfHabitToGenerateReport);
            int totalQuantityOfHabitThisYear =
                GetTotalQuantityOfAHabitInTheCurrentYear(habitToGenerateReport, unitOfHabitToGenerateReport);
            string[] longestDailyStreakInfo =
                GetLongestDailyStreakOfAHabit(habitToGenerateReport, unitOfHabitToGenerateReport);
            string longestStreak = longestDailyStreakInfo[0];
            string longestStreakStartDate = 
                DateTime.ParseExact(
                    longestDailyStreakInfo[1], 
                    "yyyy-MM-dd", 
                    CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            string longestStreakEndDate = 
                DateTime.ParseExact(
                    longestDailyStreakInfo[2],
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            
            Console.WriteLine($"\nReport on {habitToGenerateReport} habit");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Total occurrences: {totalOccurrencesOfHabit}");
            Console.WriteLine($"Total quantity: {totalQuantityOfHabit} {unitOfHabitToGenerateReport}");
            Console.WriteLine($"Total occurrences this year: {totalOccurrencesOfHabitThisYear}");
            Console.WriteLine($"Total quantity this year: {totalQuantityOfHabitThisYear} {unitOfHabitToGenerateReport}");
            Console.WriteLine($"Longest daily streak: {longestStreak} ({longestStreakStartDate} to {longestStreakEndDate})");
        }
    }

    private string[] GetHabitToGenerateReport()
    {
        using var command = new SqliteCommand(Queries.SelectDistinctHabitsCommand, connection);
        using var reader = command.ExecuteReader();
        int id = 1;
        HashSet<int> indexSet = [];
        var habits = new List<string[]>();

        Console.WriteLine($"\n{"Id", -5}\t{"Habit", -30}\t{"Unit", -10}");
        while (reader.Read())
        {
            indexSet.Add(id);
            string habit = reader.GetString(0);
            string unit = reader.GetString(1);
            habits.Add([habit, unit]);

            Console.WriteLine($"{id++, -5}\t{habit, -30}\t{unit, -10}");
        }

        int idOfHabitToGenerateReport = Utils.GetIdOfRecord(indexSet, "get the report on");
        string habitToGenerateReport = habits[idOfHabitToGenerateReport - 1][0];
        string unitOfHabitToGenerateReport = habits[idOfHabitToGenerateReport - 1][1];

        return [habitToGenerateReport, unitOfHabitToGenerateReport];
    }

    private int GetTotalOccurrencesOfAHabit(string habit, string unit)
    {
        using var command = new SqliteCommand(Queries.CountTotalOccurrencesOfTheHabit, connection);
        command.Parameters.AddWithValue("@habit", habit);
        command.Parameters.AddWithValue("@unit", unit);
        using var reader = command.ExecuteReader();

        reader.Read();
        
        return Convert.ToInt32(reader.GetString(0));
    }

    private int GetTotalQuantityOfAHabit(string habit, string unit)
    {
        using var command = new SqliteCommand(Queries.CountTotalQuantityOfTheHabit, connection);
        command.Parameters.AddWithValue("@habit", habit);
        command.Parameters.AddWithValue("@unit", unit);
        using var reader = command.ExecuteReader();

        reader.Read();
        
        return Convert.ToInt32(reader.GetString(0));
    }

    private int GetTotalOccurrencesOfAHabitInTheCurrentYear(string habit, string unit)
    {
        using var command = new SqliteCommand(Queries.CountTotalOccurrencesOfTheHabitThisYear, connection);

        var startDate = new DateTime(DateTime.Now.Year, 1, 1);
        var endDate = new DateTime(DateTime.Now.Year, 12, 31);
        
        command.Parameters.AddWithValue("@habit", habit);
        command.Parameters.AddWithValue("@unit", unit);
        command.Parameters.AddWithValue("@startDate", startDate);
        command.Parameters.AddWithValue("@endDate", endDate);
        
        using var reader = command.ExecuteReader();

        reader.Read();
        
        return Convert.ToInt32(reader.GetString(0));
    }

    private int GetTotalQuantityOfAHabitInTheCurrentYear(string habit, string unit)
    {
        using var command = new SqliteCommand(Queries.CountTotalQuantityOfTheHabitThisYear, connection);

        var startDate = new DateTime(DateTime.Now.Year, 1, 1);
        var endDate = new DateTime(DateTime.Now.Year, 12, 31);
        
        command.Parameters.AddWithValue("@habit", habit);
        command.Parameters.AddWithValue("@unit", unit);
        command.Parameters.AddWithValue("@startDate", startDate);
        command.Parameters.AddWithValue("@endDate", endDate);
        
        using var reader = command.ExecuteReader();

        reader.Read();
        
        return Convert.ToInt32(reader.GetString(0));
    }

    private string[] GetLongestDailyStreakOfAHabit(string habit, string unit)
    {
        using var command = new SqliteCommand(Queries.GetLongestDailyStreakOfHabit, connection);
        command.Parameters.AddWithValue("@habit", habit);
        command.Parameters.AddWithValue("@unit", unit);

        using var reader = command.ExecuteReader();
        reader.Read();
        
        string[] result = ["", "", ""];
        result[0] = reader.GetString(0);
        result[1] = reader.GetString(1);
        result[2] = reader.GetString(2);

        return result;
    }
}