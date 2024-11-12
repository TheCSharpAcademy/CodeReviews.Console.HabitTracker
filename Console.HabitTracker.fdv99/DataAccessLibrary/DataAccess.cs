using System.Data.SQLite;
namespace DataAccessLibrary;

public class DataAccess
{
    private SQLiteConnection CreateConnection()
    {
        var cnn = new SQLiteConnection(DataAccessHelpers.connectionString);
        cnn.Open();
        return cnn;
    }

    public void Insert(string sqlStatement, params object[] parameters)
    {
        using (var cnn = CreateConnection())
        {
            using (var command = new SQLiteCommand(sqlStatement, cnn))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }
    }
}
