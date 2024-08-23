using Microsoft.Data.Sqlite;
namespace HabitLogger.Models
{
    public class DBStorage
    {
        public SqliteConnection _con;
        public DBStorage(string path, string tableName)
        {
            CreateTable(path, tableName);
        }
        public DBStorage()
        {

        }
        private void CreateTable(string path, string tableName)
        {
            using (_con = new SqliteConnection($"Data Source={path}"))
            {
                _con.Open();
                var command = _con.CreateCommand();
                command.CommandText = $@"CREATE TABLE IF NOT EXISTS {tableName}(id INTEGER PRIMARY KEY, name TEXT, quantity TEXT, date datetime)";
                command.ExecuteNonQuery();
                CheckTableCreation(command, tableName);
            }
        }
        private void CheckTableCreation(SqliteCommand command, string tableName)
        {
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName";
            command.Parameters.AddWithValue("@tableName", tableName);
            var tableExists = command.ExecuteScalar();
            if (tableExists == null)
            {
                Console.WriteLine("Table creation failed.");
            }
            else
            {
                Console.WriteLine("Table created successfully.");
            }
        }


    }
}

