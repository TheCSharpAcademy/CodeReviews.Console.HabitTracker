using System;
using System.Data.SQLite;
namespace HabitLogger.Models
{
	public class DBStorage
	{
        private SQLiteConnection _con;
        public DBStorage(string path, string tableName)
		{
            _con = OpenConnection(path);
            CreateTable(_con, tableName);
        }
		private SQLiteConnection OpenConnection(string path)
		{
            var con = new SQLiteConnection($"Data Source={path};Version=3;");
            con.Open();
            return con;
        }
   
        private SQLiteCommand GetSQLCommand(SQLiteConnection con)
        {
            return new SQLiteCommand(con);
        }
        private void CreateTable(SQLiteConnection con, string tableName)
        {
            try
            {
                var cmd = GetSQLCommand(con);
                cmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {tableName} (
                                        id INTEGER PRIMARY KEY,
                                        name TEXT,
                                        quantity TEXT
                                    );";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating table {tableName}: {ex.Message}");
            }
        }
        public void CloseConnection()
        {
            if (_con != null && _con.State == System.Data.ConnectionState.Open)
            {
                _con.Close();
                _con.Dispose();
            }
        }
    }
}

