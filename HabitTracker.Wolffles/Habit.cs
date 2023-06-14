using Microsoft.Data.Sqlite;

namespace habit_tracker;
class Habit : IHabit
{
	private string m_connectionString = "";
	private string m_habitName = "";
	private string m_habitTable = "";
	private string m_habitUnit = "";
	public Habit(string connectionString, string habitName, string measuringUnit)
	{
		m_connectionString = connectionString;
		m_habitName = habitName;
		m_habitTable = habitName.ToLower() + "_table";
		m_habitUnit = measuringUnit;
		ExecuteCommand(@$"CREATE TABLE IF NOT EXISTS {m_habitTable}(Id INTEGER PRIMARY KEY AUTOINCREMENT,Date TEXT,{m_habitName} INTEGER)");
	}
	private bool ExecuteCommand(string command)
	{
		using (var connection = new SqliteConnection(m_connectionString))
		{
			DateTime time = DateTime.Now;
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

		return ExecuteCommand(@$"SELECT * FROM {m_habitTable} WHERE Date = '{date}' ");
	}
	public string getConnectionString()
	{
		return m_connectionString;
	}
	public string getTable()
	{
		return m_habitTable;
	}
	public string getMeasureUnit()
	{
		return m_habitUnit;
	}
	public bool Insert(int value, string time)
	{
		if (CheckIfExists(time))
		{
			return false;
		}
		ExecuteCommand(@$"INSERT INTO {m_habitTable} (Date,{m_habitName}) VALUES ('{time}','{value}')");
		return true;
	}
	public void Update(string index, int value)
	{
		ExecuteCommand(@$"UPDATE {m_habitTable} SET {m_habitName} = '{value}' WHERE Date = '{index}'");
	}
	public void Delete(string index)
	{
		ExecuteCommand(@$"DELETE FROM {m_habitTable} WHERE Date = '{index}'");
	}
	public void DeleteAll()
	{
		ExecuteCommand(@$"DELETE FROM {m_habitTable}");
	}
	public void Read()
	{
		using (var connection = new SqliteConnection(m_connectionString))
		{
			connection.Open();
			SqliteCommand tableCommand = connection.CreateCommand();
			tableCommand.CommandText = @$"SELECT * FROM {m_habitTable} ORDER BY Date";
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