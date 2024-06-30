using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace HabitTracker;

internal class HabitDB
{
	internal static void UpdateHabit()
	{
		GetHabits();
		var id = Utility.GetNumber("Please type the id of the habit you want to update.");

		string name = "";
		bool updateName = AnsiConsole.Confirm("Update name?");
		if (updateName)
		{
			name = AnsiConsole.Ask<string>("Habit's new name: ");
			while (string.IsNullOrEmpty(name))
			{
				name = AnsiConsole.Ask<string>("Habbit's name can't be empty. Try again: ");
			}
		}

		string unit = "";
		bool updateUnit = AnsiConsole.Confirm("Update Unit of Measurement?");
		if (updateUnit)
		{
			unit = AnsiConsole.Ask<string>("Habit's Unit of Measurement: ");
			while (string.IsNullOrEmpty(unit))
			{
				unit = AnsiConsole.Ask<string>("Habit's unit of measurement can't be empty. Try Again: ");
			}
		}

		string query;
		if (updateName && updateUnit)
		{
			query = $"UPDATE habits SET Name = '{name}', MeasurementUnit = '{unit}' WHERE Id = {id}";
		}
		else if (updateName && !updateUnit)
		{
			query = $"UPDATE habits SET Name = '{name}' WHERE Id = {id}";
		}
		else
		{
			query = $"UPDATE habits SET MeasurementUnit = '{unit}' WHERE Id = {id}";
		}

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();
			var tableCmd = connection.CreateCommand();

			tableCmd.CommandText = query;
			tableCmd.ExecuteNonQuery();
		}
	}

	internal static void DeleteHabit()
	{
		GetHabits();

		var id = Utility.GetNumber("Please type the habit you want to delete.");

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			using (var command = connection.CreateCommand())
			{
				connection.Open();

				command.CommandText =
					$@"DELETE FROM habits WHERE Id = {id}";
				command.ExecuteNonQuery();
			}
		}
	}

	internal static void GetHabits()
	{
		List<Utility.Habit> habits = new();

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();
			var tableCmd = connection.CreateCommand();
			tableCmd.CommandText = "SELECT * FROM Habits";

			using (SqliteDataReader reader = tableCmd.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
						try
						{
							habits.Add(
								new Utility.Habit(
									reader.GetInt32(0),
									reader.GetString(1),
									reader.GetString(2)
									)
								);
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Error getting record: {ex.Message}.");
						}
				}
				else
				{
					Console.WriteLine("No rows found");
				}
			}
		}

		ViewHabits(habits);
	}

	internal static void ViewHabits(List<Utility.Habit> habits)
	{
		var table = new Table();
		table.AddColumn("Id");
		table.AddColumn("Name");
		table.AddColumn("Measurement Unit");

		foreach (var habit in habits)
		{
			table.AddRow(habit.Id.ToString(), habit.Name.ToString(), habit.UnitOfMeasurement.ToString());
		}

		AnsiConsole.Write(table);
	}

	internal static void AddHabit()
	{
		var name = AnsiConsole.Ask<string>("Habit's name: ");
		while (string.IsNullOrEmpty(name))
		{
			name = AnsiConsole.Ask<string>("Habit's name can't be empty. Try again: ");
		}

		var unit = AnsiConsole.Ask<string>("What is the habit's unit of measurement? ");
		while (string.IsNullOrEmpty(unit))
		{
			unit = AnsiConsole.Ask<string>("Unit of measurement can't be empty. Try again: ");
		}

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();
			var tableCmd = connection.CreateCommand();
			tableCmd.CommandText = $"INSERT INTO habits(Name, MeasurementUnit) VALUES ('{name}', '{unit}')";

			tableCmd.ExecuteNonQuery();
		}
	}
}
