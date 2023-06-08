using Microsoft.Data.Sqlite;
using ConsoleHabitTracker;

class Unit
{
    public static void DisplayUnits()
    {
        var tableData = new List<List<object>>();
        var headerData = new List<String>();
        headerData.Add("ID");
        headerData.Add("Name");
        headerData.Add("Symbol");

        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"SELECT ID,Name,Symbol FROM Unit";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
    new List<object> { reader.GetInt32(0), reader.GetString(1), reader.GetString(2) }
);
                }
                Helpers.PrintTable(tableData, headerData);
            }
            else
            {
                Console.WriteLine("No Units exists.");
            }
            connection.Close();
        }
    }

    public static Boolean AddUnit(string newName, string newUnitSymbol)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO Unit (Name, Symbol) VALUES ('{newName}','{newUnitSymbol}')";

                tableCmd.ExecuteNonQuery();
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Boolean UpdateUnit(int unitId, string newName, string newSymbol)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE Unit SET Name='{newName}',Symbol='{newSymbol}' WHERE ID = {unitId}";

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

    public static Boolean DeleteUnit(int unitId)
    {
        try
        {
            using (var connection = new SqliteConnection(Database.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM Unit WHERE ID = {unitId}";

                tableCmd.ExecuteNonQuery();

                return true;
            }
        }
        catch
        {
            return true;
        }
    }

    public static Boolean IsUnitUsed(int unitID)
    {
        int counter = 0;
        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM Habit where UnitID = {unitID}";

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

    public static Boolean UnitExist(int unitId)
    {
        int counter = 0;
        using (var connection = new SqliteConnection(Database.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM Unit where ID = {unitId}";

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