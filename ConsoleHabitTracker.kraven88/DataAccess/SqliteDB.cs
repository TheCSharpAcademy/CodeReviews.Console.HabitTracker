using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHabitTracker.kraven88.DataAccess;
internal class SqliteDB
{
    private readonly string connectionString;

	public SqliteDB(string nameOfDatabase)
	{
		connectionString = $"Data Source={nameOfDatabase}.db; Version=3";
        if (File.Exists(nameOfDatabase) == false)
            CreateDatabase(nameOfDatabase);

    }

	public void CreateDatabase(string NameOfDatabase)
	{
		SQLiteConnection.CreateFile(NameOfDatabase);
		CreateHabitsTable();
		CreateDailyProgressTable();
	}

    private void CreateDailyProgressTable()
    {
		using (var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();
			var sql = connection.CreateCommand();
			sql.CommandText =
				@"CREATE TABLE IF NOT EXISTS DailyProgress(
					Id INTEGER [PRIMARY KEY] [NOT NULL] [AUTOINCREMENT],
					HabitId INTEGER [NOT NULL],
					Date TEXT,
					Quantity INTEGER,
					DailyGoal INTEGER)";

			sql.ExecuteNonQuery();

			connection.Close();
		}
    }

    public void CreateHabitsTable()
	{
		using (var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();

			var sql = connection.CreateCommand();
			sql.CommandText = 
				@"CREATE TABLE IF NOT EXISTS Habits(
					Id INTEGER [PRIMARY KEY] [NOT NULL] [AUTOINCREMENT],
					Name TEXT,
					Unit TEXT);";

			sql.ExecuteNonQuery();

			connection.Close();
		}
	}
}
