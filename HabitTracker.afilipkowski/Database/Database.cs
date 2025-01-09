using Microsoft.Data.Sqlite;

namespace Database
{
    public class DatabaseHandler
    {
        private string connectionString = "Data Source=database.db";
        public DatabaseHandler()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS read_pages(
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
                        INSERT INTO read_pages (date, amount)
                        VALUES ('{date}', {amount})
                    ";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
       
    }
}
