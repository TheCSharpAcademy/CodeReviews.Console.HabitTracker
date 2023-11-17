using Microsoft.Data.Sqlite;

namespace ColumnName;

public class GetColumnName
{
    string connectionString = @"Data Source=habit-Tracker2.db";
    public string GetColName()
    {
        string currentColumnName = "";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM drinking_water";
            SqliteDataReader reader = tableCommand.ExecuteReader();

            if(reader.HasRows)
            {
                currentColumnName = reader.GetName(2);
            }

            connection.Close();
        }

        return currentColumnName;
    }
}