using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

string connectionString = @"Data Source=habit-Tracker.db";

CreateDatabase();

MainMenu();

void MainMenu()
{
	var isMenuRunning = true;

	while (isMenuRunning)
	{
		var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title("What would you like to do?")
			.AddChoices(
				"Add Habit",
				"Delete Habit",
				"Update Habit",
				"Add Record",
				"Delete Record",
				"View Records",
				"Update Record",
				"Quit")
			);

		switch (userChoice)
		{
			case "Add Habit":
				AddHabit();
				break;
			case "Delete Habit":
				DeleteHabit();
				break;
			case "Update Habit":
				UpdateHabit();
				break;
			case "Add Record":
				AddRecord();
				break;
			case "Delete Record":
				DeleteRecord();
				break;
			case "View Records":
				GetRecords();
				break;
			case "Update Record":
				UpdateRecord();
				break;
			case "Quit":
				Console.WriteLine("Goodbye!");
				isMenuRunning = false;
				break;
			default:
				Console.WriteLine("Invalid Choice. Please choose one of the above");
				break;
		}
	}
}

void UpdateHabit()
{
	GetHabits();
	var id = GetNumber("Please type the id of the habit you want to update.");

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

	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var tableCmd = connection.CreateCommand();

		tableCmd.CommandText = query;
		tableCmd.ExecuteNonQuery();
	}
}

void DeleteHabit()
{
	GetHabits();

	var id = GetNumber("Please type the habit you want to delete.");

	using (var connection = new SqliteConnection(connectionString))
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

void GetHabits()
{
	List<Habit> habits = new();

	using (var connection = new SqliteConnection(connectionString))
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
							new Habit(
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

void ViewHabits(List<Habit> habits)
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

void AddHabit()
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

	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var tableCmd = connection.CreateCommand();
		tableCmd.CommandText = $"INSERT INTO habits(Name, MeasurementUnit) VALUES ('{name}', '{unit}')";

		tableCmd.ExecuteNonQuery();
	}
}

void UpdateRecord()
{
	GetRecords();
	var id = GetNumber("Please type the id of the record you want to update.");

	string date = "";
	bool updateDate = AnsiConsole.Confirm("Update date?");
	if (updateDate)
	{
		date = GetDate("\nEnter the date (format : dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");
	}

	int quantity = 0;
	bool updateQuantity = AnsiConsole.Confirm("Update quantity?");
	if (updateQuantity)
	{
		quantity = GetNumber("\nPlease enter the new quantity (no deicmals or negatives allowed) or enter 0 to Go Back to Main Menu.");
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

	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var tableCmd = connection.CreateCommand();

		tableCmd.CommandText = query;

		tableCmd.ExecuteNonQuery();
	}
}

void AddRecord()
{
	string date = GetDate("\nEnter the date (format - dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");

	GetHabits();
	int habitId = GetNumber("\nPlease enter the id of the habit for this record");
	int quantity = GetNumber("\nPlease enter quantity (no decimals or negatives allowed) or enter 0 to Go Back to Main Menu:\n");

	Console.Clear();
	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var tableCmd = connection.CreateCommand();

		tableCmd.CommandText = $"INSERT INTO records (date, quantity, habitId) VALUES ('{date}', {quantity}, {habitId})";

		tableCmd.ExecuteNonQuery();
	}
}

int GetNumber(string message)
{
	Console.WriteLine(message);
	string? numberInput = Console.ReadLine();

	if (numberInput == "0")
		MainMenu();

	int outpout = 0;
	while (!int.TryParse(numberInput, out outpout) || Convert.ToInt32(numberInput) < 0)
	{
		Console.WriteLine("\n\nInvalid number. Try again.\n\n");
		numberInput = Console.ReadLine();
	}

	return outpout;
}

string GetDate(string message)
{
	Console.WriteLine(message);
	string? dateInput = Console.ReadLine();

	if (dateInput == "0")
		MainMenu();

	while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
	{
		Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Please try again\n\n");
		dateInput = Console.ReadLine();
	}

	return dateInput;
}

void GetRecords()
{
	List<RecordWithHabit> records = new();

	using (var connection = new SqliteConnection(connectionString))
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
							new RecordWithHabit(
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

void ViewRecords(List<RecordWithHabit> records)
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

void DeleteRecord()
{
	GetRecords();

	var id = GetNumber("Please type the id of the record you want to delete.");

	using (var connection = new SqliteConnection(connectionString))
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

void CreateDatabase()
{
	using (SqliteConnection connection = new(connectionString))
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

bool IsTableEmpty(string tableName)
{
	using (var connection = new SqliteConnection(connectionString))
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

void SeedData()
{
	bool recordsTableEmpty = IsTableEmpty("records");
	bool habitsTableEmpty = IsTableEmpty("habits");

	if (!recordsTableEmpty || !habitsTableEmpty)
		return;

	string[] habitNames = { "Reading", "Running", "Chocolate", "Drinking Water", "Glasses of Wine" };
	string[] habitUnits = { "Pages", "Meters", "Grams", "Mililiters", "Mililiters" };
	string[] dates = GenerateRandomDates(100);
	int[] quantities = GenerateRandomQuantities(100, 0, 2000);
	using (var connection = new SqliteConnection(connectionString))
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

int[] GenerateRandomQuantities(int count, int min, int max)
{
	Random random = new Random();
	int[] quantites = new int[count];

	for (int i = 0; i < count; i++)
		quantites[i] = random.Next(min, max + 1);

	return quantites;
}

string[] GenerateRandomDates(int count)
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

int GetRandomHabitId()
{
	Random random = new Random();
	return random.Next(1, 6);
}

record WalkingRecord(int Id, DateTime Date, int Meters);
record Habit(int Id, string Name, string UnitOfMeasurement);
record RecordWithHabit(int Id, DateTime Date, int Quantity, string HabitName, string MeasurementUnit);