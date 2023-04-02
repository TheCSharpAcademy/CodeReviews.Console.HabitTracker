using DataBaseLibrary;
using Microsoft.Data.Sqlite;

namespace HabitsLibrary;

public class MainTable
{
    private static DataBaseCommands dbCommands = new();
    public string? tableName;
    public string? connectionString;

    public MainTable(string tableName, string connectionString) 
    {
        this.tableName = tableName;
        this.connectionString = connectionString;
    }
    
    public string? TransformToSubTableName(string? name) { return $"[{name}]"; }
    
    public bool CheckForTableName(string? testTableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 2 FROM " +
                tableName +
                $" WHERE HabitTableName = '{testTableName}')";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }
    
    public void InsertNew(string? name, string? unit)
    {
       string? tableName = TransformToSubTableName(name);
       dbCommands.Insert(this.tableName, tableName, unit);
       dbCommands.CreateSubTable(tableName);
    }
}