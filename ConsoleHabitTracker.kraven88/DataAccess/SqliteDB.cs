using ConsoleHabitTracker.kraven88.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
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

    private void CreateDailyProgressTable()
    {
		using (var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();
			var sql = connection.CreateCommand();
			sql.CommandText =
                @"CREATE TABLE ""DailyProgress""(
				""Id""    INTEGER NOT NULL,
				""HabitId""   INTEGER NOT NULL,
				""Date""  TEXT,
				""Quantity""  INTEGER,
				""DailyGoal"" INTEGER,
				PRIMARY KEY(""Id"" AUTOINCREMENT)
				)";
			

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
                @"CREATE TABLE ""Habits"" (
				""Id""	INTEGER NOT NULL,
				""Name""	TEXT,
				""Unit""	TEXT,
				PRIMARY KEY(""Id"" AUTOINCREMENT)
				)";

			sql.ExecuteNonQuery();

			connection.Close();
		}
	}

	public Habit LoadHabit(string habitName)
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
				}
			}
			else throw new ArgumentException($"Couldn't find habit with the name: {habitName}");

			reader.Close();

			sql.CommandText =
				$@"SELECT dp.*
				FROM DailyProgress dp
				INNER JOIN Habits h ON dp.HabitId=h.Id
				WHERE h.Name=""{habitName}"";";

			reader = sql.ExecuteReader();
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					var daily = new DailyProgress();
					daily.Id = reader.GetInt32(0);
					daily.Date = DateOnly.ParseExact(reader.GetString(2), "dd.MM.yyyy");
					daily.Quantity = reader.GetInt32(3);
					daily.DailyGoal = reader.GetInt32(4);

					habit.ProgressList.Add(daily);
				}
			}

			return habit;
		}
	}

    internal void SetNewGoal(Habit habit, int newGoal)
    {
        using(var connection = new SQLiteConnection(connectionString))
		{
			connection.Open();
			var sql = connection.CreateCommand();

			sql.CommandText = $@"
				UPDATE DailyProgress
				SET DailyGoal = {newGoal}
				WHERE HabitId = {habit.Id} AND Date = ""{DateOnly.FromDateTime(DateTime.Now):dd.MM.yyyy}""";

			sql.ExecuteNonQuery();

			connection.Close();
		}
    }
}
