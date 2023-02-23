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
    public int TimesPerYear(string? tableName,int year)
    {
        int times = -1;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT COUNT(*) FROM " + tableName + $"WHERE Date LIKE '%{year}'";

            times = tableCmd.ExecuteNonQuery();
        }
        return times;
    }
}
