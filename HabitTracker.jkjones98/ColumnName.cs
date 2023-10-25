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

            // Create sqlitedatareader
            SqliteDataReader reader = tableCommand.ExecuteReader();

            // If reader has rows run code inside if statement
            if(reader.HasRows)
            {
                // Not good to hard code the index but this is the best I have for now
                currentColumnName = reader.GetName(2);
            }

            connection.Close();
        }

        return currentColumnName;
    }
}