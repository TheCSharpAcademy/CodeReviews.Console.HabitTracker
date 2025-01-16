using Microsoft.Data.Sqlite;

namespace Database;
public class DatabaseHandler
{
    private string connectionString = "Data Source=database.db";
    private string tableName = "read_pages";
    public DatabaseHandler()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @$"
                    CREATE TABLE IF NOT EXISTS {tableName}(
                    id INTEGER PRIMARY KEY,
                    date TEXT,
                    amount INT)
                ";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void InsertRecord(string? date, int amount)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                $@"
                        INSERT INTO {tableName} (date, amount)
                        VALUES ('{date}', {amount})
                    ";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public List<DatabaseRecord> GetAllRecords()
    {
        List<DatabaseRecord> databaseRecords = new List<DatabaseRecord>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"SELECT * from {tableName}";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    databaseRecords.Add(
                    new DatabaseRecord
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetString(1),
                        Amount = reader.GetInt32(2)
                    });
                }
                reader.Close();
            }
            connection.Close();
        }
        return databaseRecords;
    }
    public void DeleteRecord(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"DELETE FROM {tableName} WHERE Id={id}";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void UpdateRecord(int id, string date, int amount)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"
            UPDATE {tableName}
            SET date = '{date}', amount = {amount}
            WHERE Id={id}
            ";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public bool RecordExists(int id)
    {
        int result;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"SELECT COUNT(*) FROM {tableName} WHERE Id={id}";
            result = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
        if (result == 1) return true;
        else return false;
    }
}

public class DatabaseRecord
{
    public int Id { get; set; }
    public string Date { get; set; }
    public int Amount { get; set; }
}

