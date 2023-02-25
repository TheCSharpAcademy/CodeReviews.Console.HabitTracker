using Microsoft.Data.Sqlite;

namespace DataBaseLibrary;

public class DataBaseViews
{
    static string connectionString = @"Data Source=habit-Tracker.db";

    //returns the amount of log in a giver year (YY format)
    public int TimesLoggedInYear(string? tableName,int year)
    {
        int times = -1;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT COUNT(*) FROM " + tableName + $"WHERE Date LIKE '%{year}'";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            reader.Read();

            times = reader.GetInt32(0);

            connection.Close();
        }
        return times;
    }
    //returns the amount of quantinty in a given year (YY format)
    public int TotalOfYear(string? tableName, int year)
    {
        int total = -1;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT SUM(Quantity) FROM " + tableName + $"WHERE Date LIKE '%{year}'";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            reader.Read();
            try { total = reader.GetInt32(0); }
            catch { total = -1; }
            
            connection.Close();
        }
        return total;
    }
}
