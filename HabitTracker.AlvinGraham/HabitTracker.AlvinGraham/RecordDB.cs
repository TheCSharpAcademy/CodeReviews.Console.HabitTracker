using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

namespace HabitTracker;

internal class RecordDB
{
	internal static void UpdateRecord()
	{
		GetRecords();
		var id = Utility.GetNumber("Please type the id of the record you want to update.");

		string date = "";
		bool updateDate = AnsiConsole.Confirm("Update date?");
		if (updateDate)
		{
			date = Utility.GetDate("\nEnter the date (format : dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");
		}

		int quantity = 0;
		bool updateQuantity = AnsiConsole.Confirm("Update quantity?");
		if (updateQuantity)
		{
			quantity = Utility.GetNumber("\nPlease enter the new quantity (no deicmals or negatives allowed) or enter 0 to Go Back to Main Menu.");
		}

		string query;
		if (updateDate && updateQuantity)
		{
			query = $"UPDATE records SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";
		}
		else if (updateDate && !updateQuantity)
		{
			query = $"UPDATE records SET Date = '{date}' WHERE Id = {id}";
		}
		else
		{
			query = $"UPDATE records SET Quantity = '{quantity}' WHERE Id = {id}";
		}

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();
			var tableCmd = connection.CreateCommand();

			tableCmd.CommandText = query;

			tableCmd.ExecuteNonQuery();
		}
	}

	internal static void AddRecord()
	{
		string date = Utility.GetDate("\nEnter the date (format - dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");

		HabitDB.GetHabits();
		int habitId = Utility.GetNumber("\nPlease enter the id of the habit for this record");
		int quantity = Utility.GetNumber("\nPlease enter quantity (no decimals or negatives allowed) or enter 0 to Go Back to Main Menu:\n");

		Console.Clear();
		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();
			var tableCmd = connection.CreateCommand();

			tableCmd.CommandText = $"INSERT INTO records (date, quantity, habitId) VALUES ('{date}', {quantity}, {habitId})";

			tableCmd.ExecuteNonQuery();
		}
	}

	internal static void GetRecords()
	{
		List<Utility.RecordWithHabit> records = new();

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();
			var tableCmd = connection.CreateCommand();
			tableCmd.CommandText = @"
	SELECT records.Id, records.Date, records.Quantity, records.HabitId, habits.Name AS HabitName, habits.MeasurementUnit
	FROM records
	INNER JOIN habits ON records.HabitId = habits.Id";

			using (SqliteDataReader reader = tableCmd.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
						try
						{
							records.Add(
								new Utility.RecordWithHabit(
									reader.GetInt32(0),
									DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
									reader.GetInt32(2),
									reader.GetString(4),
									reader.GetString(5)
									)
								);
						}
						catch (FormatException ex)
						{
							Console.WriteLine($"Error parsing date: {ex.Message}. Skipping this record.");
						}
				}
				else
				{
					Console.WriteLine("No rows found");
				}
			}
		}

		ViewRecords(records);
	}

	internal static void ViewRecords(List<Utility.RecordWithHabit> records)
	{
		var table = new Table();
		table.AddColumn("Id");
		table.AddColumn("Date");
		table.AddColumn("Amount");
		table.AddColumn("Habit");

		foreach (var record in records)
		{
			table.AddRow(record.Id.ToString(), record.Date.ToString("D"), $"{record.Quantity} {record.MeasurementUnit}", record.HabitName.ToString());
		}

		AnsiConsole.Write(table);
	}

	internal static void DeleteRecord()
	{
		GetRecords();

		var id = Utility.GetNumber("Please type the id of the record you want to delete.");

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			using (var command = connection.CreateCommand())
			{
				connection.Open();

				command.CommandText =
					$@"DELETE FROM records WHERE ID = {id}";
				command.ExecuteNonQuery();
			}
		}
	}
}

