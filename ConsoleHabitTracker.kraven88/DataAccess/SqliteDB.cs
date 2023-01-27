using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHabitTracker.kraven88.DataAccess;
internal class SqliteDB
{
    public readonly string connectionString;

	public SqliteDB(string connectionString)
	{
		this.connectionString = connectionString;
	}

	public void CreateTable(string tableName)
	{
		using (var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();

			var sql = connection.CreateCommand();
			sql.CommandText = 
				@$"CREATE TABLE IF NOT EXISTS {tableName}(
					Id INTEGER [PRIMARY KEY] [NOT NULL] [AUTOINCREMENT],
					Date TEXT,
					Quantity INTEGER);";

			sql.ExecuteNonQuery();

			connection.Close();
		}
	}
}
