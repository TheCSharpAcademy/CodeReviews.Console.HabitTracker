using Microsoft.Data.Sqlite;
using ConsoleHabitTracker;

class Entry
{
    public static void DisplayEntries(int habitId)
    {
        var tableData = new List<List<object>>();
        var headerData = new List<String>();
        headerData.Add("Quantity");
        headerData.Add("Symbol");
        headerData.Add("Date");

        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"select Entry.Quantity,Unit.Symbol,Entry.Date
from entry,Habit h,Unit
WHERE h.ID = entry.habitId
and unit.ID = h.UnitID
AND h.ID = {habitId}";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var obj = new List<String>();
                    tableData.Add(
                        new List<object> { reader.GetInt32(0), reader.GetString(1), reader.GetString(2) }
                    );
                }
                Helpers.PrintTable(tableData, headerData);
            }
            else
            {
                Console.WriteLine("No entry exists.");
            }
            connection.Close();
        }
    }
    
    public static void DisplayReportPerYear()
    {
        var tableData = new List<List<object>>();
        var headerData = new List<String>();
        headerData.Add("Year");
        headerData.Add("Habit");
        headerData.Add("Sum");

        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"select STRFTIME('%Y', date) as year,
Habit.Name,
sum(Quantity) || ' '  || Unit.Name  as Sum
From Entry  ,Habit,Unit
where habit.ID = Entry.HabitID
and Unit.Id = Habit.UnitID
group by year";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var obj = new List<String>();
                    tableData.Add(
                        new List<object> { reader.GetString(0), reader.GetString(1), reader.GetString(2) }
                    );
                }
                Helpers.PrintTable(tableData, headerData);
            }
            else
            {
                Console.WriteLine("No entry exists.");
            }
            connection.Close();
        }
    }
    
    public static void DisplayReportPerMonth()
    {
        var tableData = new List<List<object>>();
        var headerData = new List<String>();
        headerData.Add("Year");
        headerData.Add("Month");
        headerData.Add("Habit");
        headerData.Add("Sum");

        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"select STRFTIME('%Y', date) as year,
STRFTIME('%m', date) as month,
Habit.Name,
sum(Quantity) || ' '  || Unit.Name  as Sum
From Entry  ,Habit,Unit
where habit.ID = Entry.HabitID
and Unit.Id = Habit.UnitID
group by year,month";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var obj = new List<String>();
                    tableData.Add(
                        new List<object> { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3) }
                    );
                }
                Helpers.PrintTable(tableData, headerData);
            }
            else
            {
                Console.WriteLine("No entry exists.");
            }
            connection.Close();
        }
    }
    
    public static void DisplayEntriesDetailed(int habitId)
    {
        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"select Entry.ID,Entry.Quantity,Unit.Symbol,Entry.Date
from entry,Habit h,Unit
WHERE h.ID = entry.habitId
and unit.ID = h.UnitID
AND h.ID = {habitId}";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID = {reader.GetInt32(0)}: {reader.GetInt32(1)}\t{reader.GetString(2)}\t{reader.GetString(3)}");
                }
            }
            else
            {
                Console.WriteLine("No entry exists.");
            }
            connection.Close();
        }
    }
    
    public static void DisplayEntriesSummary()
    {
        var tableData = new List<List<object>>();
        var headerData = new List<String>();
        headerData.Add("Habit");
        headerData.Add("Sum");

        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"select Habit.Name as Habit,
sum(Quantity) || ' '  || Unit.Name  as Sum
From Entry  ,Habit,Unit
where habit.ID = Entry.HabitID
and Unit.Id = Habit.UnitID
group by Habit";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var obj = new List<String>();
                    tableData.Add(
                        new List<object> { reader.GetString(0), reader.GetString(1) }
                    );
                }
                Helpers.PrintTable(tableData, headerData);
            }
            else
            {
                Console.WriteLine("No entry exists.");
            }
            connection.Close();
        }
    }
    
    public static Boolean AddEntry(int habitId, int quantity)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO Entry (HabitID,Date,Quantity) VALUES ('{habitId}',Date(),{quantity})";

                tableCmd.ExecuteNonQuery();
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public static Boolean UpdateEntry(int entryId, int quantity)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE Entry SET Quantity='{quantity}' WHERE ID = {entryId}";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public static Boolean DeleteEntry(int entryId)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM Entry WHERE ID = {entryId}";

                tableCmd.ExecuteNonQuery();

                return true;
            }
        }
        catch
        {
            return true;
        }
    }
    
    public static Boolean EntryExist(int entryId)
    {
        int counter = 0;
        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM Entry where ID = {entryId}";
            
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                counter++;
            }
            connection.Close();
        }
        if (counter > 0)
        {
            return true;
        }
        else { return false; }
    }
}