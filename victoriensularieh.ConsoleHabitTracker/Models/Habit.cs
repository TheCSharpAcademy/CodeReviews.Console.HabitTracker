using Microsoft.Data.Sqlite;
using ConsoleHabitTracker;

class Habit
{
    public static void DisplayHabits()
    {
        var tableData = new List<List<object>>();
        var headerData = new List<String>();
        headerData.Add("ID");
        headerData.Add("Name");
        headerData.Add("Unit");

        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"SELECT Habit.ID, Habit.Name, Habit.UnitID,Unit.Name,Unit.Symbol
            FROM Habit,Unit
            WHERE Habit.UnitID = Unit.ID";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ;
                    tableData.Add(
                        new List<object> { reader.GetInt32(0), reader.GetString(1), reader.GetString(4) }
                    );
                }
                Helpers.PrintTable(tableData, headerData);
            }
            else
            {
                Console.WriteLine("No tracked habits exists.");
            }
            connection.Close();
        }
    }

    public static void DisplaySingleHabit(int habitId)
    {
        var tableData = new List<List<object>>();
        var headerData = new List<String>();
        headerData.Add("ID");
        headerData.Add("Name");
        headerData.Add("Unit");

        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT Habit.ID, Habit.Name, Habit.UnitID,Unit.Name,Unit.Symbol
            FROM Habit,Unit
            WHERE Habit.UnitID = Unit.ID
            AND Habit.ID = {habitId}";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var obj = new List<String>();
                    tableData.Add(
                        new List<object> { reader.GetInt32(0), reader.GetString(1), reader.GetString(4) }
                    );
                }
                Helpers.PrintTable(tableData, headerData);
            }
            else
            {
                Console.WriteLine("No tracked habits exists.");
            }
            connection.Close();
        }
    }

    public static Boolean AddHabit(string newName, int newUnitId)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO Habit (Name, UnitID) VALUES ('{newName}',{newUnitId})";

                tableCmd.ExecuteNonQuery();
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Boolean UpdateHabit(int habitId, string newName, int newUnitId)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE Habit SET Name='{newName}',UnitID='{newUnitId}' WHERE ID = {habitId}";

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

    public static Boolean DeleteHabit(int habitId)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM Habit WHERE ID = {habitId}";

                tableCmd.ExecuteNonQuery();

                return true;
            }
        }
        catch
        {
            return true;
        }
    }

    public static Boolean IsHabitTracked(int habitId)
    {
        int counter = 0;
        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM Entry where HabitID = {habitId}";

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

    public static Boolean HabitExist(int habitId)
    {
        int counter = 0;
        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM Habit where ID = {habitId}";

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