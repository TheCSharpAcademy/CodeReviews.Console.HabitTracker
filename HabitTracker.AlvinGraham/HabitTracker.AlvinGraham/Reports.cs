using Microsoft.Data.Sqlite;

namespace HabitTracker;

internal class Reports
{
	internal static void QuantitiesReport()
	{
		HabitDB.GetHabits();
		var id = Utility.GetNumber("Please enter the number of the habit you want quantities for: ");
		long count = 0;
		string habitName = "";
		string unitsOfMeasure = "";

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();

			using (var command = connection.CreateCommand())
			{
				command.CommandText = @$"SELECT SUM (records.Quantity) FROM records
					JOIN habits on habits.Id = records.HabitId
					WHERE HabitId = {id};";
				count = (long)command.ExecuteScalar()!;

				command.CommandText = $@"SELECT Name, MeasurementUnit FROM habits WHERE Id = {id};";

				using (SqliteDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
						try
						{
							habitName = reader.GetString(0);
							unitsOfMeasure = reader.GetString(1);
						}
						catch (FormatException ex)
						{
							Console.WriteLine($"Error parsing date: {ex.Message}. Skipping this record.");
						}
				}
			}
		}

		Console.WriteLine($"There are {(count):N0} total {unitsOfMeasure} for the habit - {habitName}");
	}

	internal static void CountsReport()
	{
		HabitDB.GetHabits();
		var id = Utility.GetNumber("Please enter the number of the habit you want counts of: ");
		long count = 0;
		string habitName = "";

		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();

			using (var command = connection.CreateCommand())
			{
				command.CommandText = @$"SELECT COUNT (records.Quantity) FROM records
					JOIN habits on habits.Id = records.HabitId
					WHERE HabitId = {id};";
				count = (long)command.ExecuteScalar()!;

				command.CommandText = $@"SELECT Name FROM habits WHERE Id = {id};";

				using (SqliteDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
						try
						{
							habitName = reader.GetString(0);
						}
						catch (FormatException ex)
						{
							Console.WriteLine($"Error parsing date: {ex.Message}. Skipping this record.");
						}
				}
			}
		}

		Console.WriteLine($"There are {count} occurences of the habit - {habitName}");
	}
}
