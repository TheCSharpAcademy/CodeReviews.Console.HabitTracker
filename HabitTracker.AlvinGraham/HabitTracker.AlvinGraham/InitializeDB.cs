using Microsoft.Data.Sqlite;

namespace HabitTracker;

internal class InitializeDB
{
	internal static void CreateDatabase()
	{
		using (SqliteConnection connection = new(Utility.connectionString))
		{
			using (SqliteCommand tableCmd = connection.CreateCommand())
			{
				connection.Open();

				tableCmd.CommandText =
					@"CREATE TABLE IF NOT EXISTS records (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					Date TEXT,
					Quantity INTEGER,
					HabitId INTEGER,
					FOREIGN KEY(habitId) REFERENCES habits(Id) ON DELETE CASCADE
				)";
				tableCmd.ExecuteNonQuery();

				tableCmd.CommandText =
					@"CREATE TABLE IF NOT EXISTS habits (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					Name TEXT,
					MeasurementUnit TEXT
					)";
				tableCmd.ExecuteNonQuery();
			}
		}

		SeedData();
	}

	internal static bool IsTableEmpty(string tableName)
	{
		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();

			using (var command = connection.CreateCommand())
			{
				command.CommandText = $"SELECT COUNT(*) FROM {tableName}";
				long count = (long)command.ExecuteScalar()!;

				return count == 0;
			}
		}
	}

	internal static void SeedData()
	{
		bool recordsTableEmpty = IsTableEmpty("records");
		bool habitsTableEmpty = IsTableEmpty("habits");

		if (!recordsTableEmpty || !habitsTableEmpty)
			return;

		string[] habitNames = { "Reading", "Running", "Chocolate", "Drinking Water", "Glasses of Wine" };
		string[] habitUnits = { "Pages", "Meters", "Grams", "Mililiters", "Mililiters" };
		string[] dates = GenerateRandomDates(100);
		int[] quantities = GenerateRandomQuantities(100, 0, 2000);
		using (var connection = new SqliteConnection(Utility.connectionString))
		{
			connection.Open();

			for (int i = 0; i < habitNames.Length; i++)
			{
				var insertSql = $"INSERT INTO habits (Name, MeasurementUnit) VALUES ('{habitNames[i]}', '{habitUnits[i]}');";
				var command = new SqliteCommand(insertSql, connection);

				command.ExecuteNonQuery();
			}

			for (int i = 0; i < 100; i++)
			{
				var insertSql = $"INSERT INTO records (Date, Quantity, HabitId) VALUES ('{dates[i]}', {quantities[i]}, {GetRandomHabitId()});";
				var command = new SqliteCommand(insertSql, connection);

				command.ExecuteNonQuery();
			}
		}
	}

	internal static int[] GenerateRandomQuantities(int count, int min, int max)
	{
		Random random = new Random();
		int[] quantites = new int[count];

		for (int i = 0; i < count; i++)
			quantites[i] = random.Next(min, max + 1);

		return quantites;
	}

	internal static string[] GenerateRandomDates(int count)
	{
		DateTime startDate = new DateTime(2023, 1, 1);
		DateTime endDate = new DateTime(2024, 12, 31);
		TimeSpan range = endDate - startDate;

		string[] randomDateStrings = new string[count];
		Random random = new Random();

		for (int i = 0; i < count; i++)
		{
			int daysToAdd = random.Next(0, (int)range.TotalDays);
			DateTime randomDate = startDate.AddDays(daysToAdd);
			randomDateStrings[i] = randomDate.ToString("dd-MM-yy");
		}

		return randomDateStrings;
	}

	internal static int GetRandomHabitId()
	{
		Random random = new Random();
		return random.Next(1, 6);
	}
}
