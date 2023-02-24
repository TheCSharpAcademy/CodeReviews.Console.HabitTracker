using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLibrary;

public class DataBaseViews
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    static string s_MainTableName = "HabitsTable";
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
