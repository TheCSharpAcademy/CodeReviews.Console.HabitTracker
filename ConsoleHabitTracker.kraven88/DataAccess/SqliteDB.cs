using ConsoleHabitTracker.kraven88.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

	private void SaveData(string sqlCommand)
	{
		using (var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();
			var sql = connection.CreateCommand();
			sql.CommandText = sqlCommand;
			sql.ExecuteNonQuery();
			connection.Close();
		}
	}
	
    private void CreateDailyProgressTable()
    {
		var sql =
                @"CREATE TABLE ""DailyProgress""(
				""Id""    INTEGER NOT NULL,
				""HabitId""   INTEGER NOT NULL,
				""Date""  TEXT UNIQUE,
				""Quantity""  INTEGER,
				""DailyGoal"" INTEGER,
				PRIMARY KEY(""Id"" AUTOINCREMENT)
				)";

        SaveData(sql);
    }

    private void CreateHabitsTable()
	{
		var sql =
                @"CREATE TABLE ""Habits"" (
				""Id""	INTEGER NOT NULL,
				""Name""	TEXT,
				""Unit""	TEXT,
				""CurrentGoal""	INTEGER,
				PRIMARY KEY(""Id"" AUTOINCREMENT)
				)";
        
		SaveData(sql);
	}

	internal List<DailyProgress> LoadCurrentProgress(Habit habit)
	{
        var sql =
                $@"SELECT *
				FROM DailyProgress
				WHERE HabitId = {habit.Id}
				ORDER BY Date DESC
				LIMIT 1;";

		return LoadProgressList(sql);
    }

	internal List<DailyProgress> LoadAllProgress(Habit habit)
	{
		var sql =
                $@"SELECT *
				FROM DailyProgress
				WHERE HabitId = {habit.Id};";

		return LoadProgressList(sql);
    }

	private List<DailyProgress> LoadProgressList(string sqlCommand)
	{
		var progressList = new List<DailyProgress>();
		using (var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();
			var sql = connection.CreateCommand();
			sql.CommandText = sqlCommand;

            var reader = sql.ExecuteReader();
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					var daily = new DailyProgress();
					daily.Id = reader.GetInt32(0);
					daily.Date = DateOnly.ParseExact(reader.GetString(2), "dd.MM.yyyy");
					daily.Quantity = reader.GetInt32(3);
					daily.DailyGoal = reader.GetInt32(4);

					progressList.Add(daily);
				}
			} else throw new ArgumentException($"Couldn't find Progress List.");

			reader.Close();
			connection.Close();
        }

		return progressList;
	}

	internal Habit LoadHabit(string habitName)
	{
		using (var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();
			var sql = connection.CreateCommand();
			var habit = new Habit();

			sql.CommandText =
				$@"SELECT *
				FROM Habits
				WHERE Name=""{habitName}""";

			var reader = sql.ExecuteReader();

			if (reader.HasRows)
			{
				while (reader.Read())
				{
					habit.Id = reader.GetInt32(0);
					habit.Name = reader.GetString(1);
					habit.UnitOfMeasurement = reader.GetString(2);
					habit.CurrectGoal = reader.GetInt32(3);
				}
			}
			else throw new ArgumentException($"Couldn't find habit with the name: {habitName}");

			reader.Close();

			return habit;
		}
	}

    internal void SaveNewGoal(Habit habit, int newGoal)
    {
		var sql = 
				$@"UPDATE DailyProgress
				SET DailyGoal = {newGoal}
				WHERE HabitId = {habit.Id} AND Date = ""{DateOnly.FromDateTime(DateTime.Now):dd.MM.yyyy}""";

		SaveData(sql);

		sql =
			$@"UPDATE Habits
			SET CurrentGoal = {newGoal}
			WHERE Id = {habit.Id}";
		SaveData(sql);
    }

    internal void SaveProgress(Habit habit, string date, int newProgress)
    {
        var sql =
                $@"INSERT INTO DailyProgress (HabitId, Date, Quantity, DailyGoal)
				VALUES ({habit.Id}, ""{date}"", {newProgress}, {habit.CurrectGoal})
				ON CONFLICT (Date) DO UPDATE SET Quantity = Quantity + {newProgress}";

		SaveData(sql);
    }

    internal void DeleteCurrentProgress(Habit habit, string date)
    {
		var sql =
                $@"DELETE FROM DailyProgress
				WHERE Date = ""{date}"" AND HabitId = {habit.Id}";

		SaveData(sql);
    }

    internal void DeleteAllProgress(Habit habit)
    {
		var sql =
                $@"DELETE FROM DailyProgress
				WHERE HabitId = {habit.Id}";

		SaveData(sql);
    }

    internal List<Habit> LoadExistingHabits()
    {
		var habits = new List<Habit>();
		using (var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();
			var sql = connection.CreateCommand();
			sql.CommandText = "SELECT * FROM Habits";

			var reader = sql.ExecuteReader();
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					var habit = new Habit();
					habit.Id = reader.GetInt32(0);
					habit.Name = reader.GetString(1);
					habit.UnitOfMeasurement = reader.GetString(2);
					habit.CurrectGoal = reader.GetInt32(3);

					habits.Add(habit);
				}
			}
			else
				throw new ArgumentException("No habits found.");

			reader.Close();
			connection.Close();
		}

		return habits;
    }

    internal void SaveNewHabit(string name, string unit)
    {
		var sql = 
				$@"INSERT INTO Habits (Name, Unit)
				VALUES (""{name}"", ""{unit}"")";

		SaveData(sql);
    }
}
