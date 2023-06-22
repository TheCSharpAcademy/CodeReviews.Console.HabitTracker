using Microsoft.Data.Sqlite;

namespace habit_tracker;
class Habit : IHabit
{
	public string ConnectionString {get;}
	public string HabitName  {get;}
	public string HabitTable {get;}
	public string HabitUnit {get;}
	public Habit(string connectionString, string habitName, string measuringUnit)
	{
		ConnectionString = connectionString;
		HabitName = habitName;
		HabitTable = habitName.ToLower() + "_table";
		HabitUnit = measuringUnit;
		ExecuteCommand(@$"CREATE TABLE IF NOT EXISTS {HabitTable}(Id INTEGER PRIMARY KEY AUTOINCREMENT,Date TEXT,{HabitName} INTEGER)");
	}
	private bool ExecuteCommand(string command)
	{
		using (var connection = new SqliteConnection(ConnectionString))
		{
			
			connection.Open();

			SqliteCommand tableCommand = connection.CreateCommand();
			tableCommand.CommandText = command;

			bool result = tableCommand.ExecuteReader().HasRows;
			connection.Close();

			return result;
		}
	}
	private bool CheckIfExists(string date)
	{
		bool commandCheck = ExecuteCommand(@$"SELECT * FROM {HabitTable} WHERE Date = '{date}' ");

		return commandCheck;
	}
	public bool Insert(string time, int value)
	{
		if (CheckIfExists(time))
		{
			return false;
		}
		ExecuteCommand(@$"INSERT INTO {HabitTable} (Date,{HabitName}) VALUES ('{time}','{value}')");
		return true;
	}
	public bool Update(string time, int value)
	{
		if (!CheckIfExists(time))
		{
			return false;
		}

		ExecuteCommand(@$"UPDATE {HabitTable}  SET  {HabitName} = '{value}' WHERE Date = '{time}'");
		return true;
	}
	public void Delete(string index)
	{
		ExecuteCommand(@$"DELETE FROM {HabitTable} WHERE Date = '{index}'");
	}
	public void DeleteAll()
	{
		ExecuteCommand(@$"DELETE FROM {HabitTable}");
	}
	public void Read()
	{
		using (var connection = new SqliteConnection(ConnectionString))
		{
			connection.Open();
			SqliteCommand tableCommand = connection.CreateCommand();
			tableCommand.CommandText = @$"SELECT * FROM {HabitTable} ORDER BY Date";
			SqliteDataReader dataReader = tableCommand.ExecuteReader();

			while (dataReader.Read())
			{
				string date = dataReader.GetString(1);
				string tracked_value = dataReader.GetString(2);
				Console.WriteLine($"{date,0} | {tracked_value,0}");
			}
			connection.Close();
		}
	}
}