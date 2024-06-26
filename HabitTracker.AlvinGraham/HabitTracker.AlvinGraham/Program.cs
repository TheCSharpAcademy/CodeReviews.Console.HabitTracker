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
				"Add Record",
				"Delete Record",
				"View Records",
				"Update Record",
				"Quit")
			);

		switch (userChoice)
		{
			case "Add Record":
				AddRecord();
				break;
			case "Delete Record":
				//DeleteRecord();
				break;
			case "View Records":
				GetRecords();
				break;
			case "Update Record":
				//UpdateRecord();
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

void AddRecord()
{
	string date = GetDate("\nEnter the date (format - dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");
	int quantity = GetNumber("\nPlease enter number of meters waled (no decimals or negatives allowed) or enter 0 to Go Back to Main Menu:\n");
	Console.Clear();
	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var tableCmd = connection.CreateCommand();

		tableCmd.CommandText = $"INSERT INTO walkingHabit(date, quantity) VALUES ('{date}', {quantity})";

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
	List<WalkingRecord> records = new();
	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var tableCmd = connection.CreateCommand();
		tableCmd.CommandText = "SELECT * FROM walkingHabit";

		using (SqliteDataReader reader = tableCmd.ExecuteReader())
		{
			if (reader.HasRows)
			{
				while (reader.Read())
					try
					{
						records.Add(
							new WalkingRecord(
								reader.GetInt32(0),
								DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
								reader.GetInt32(2)
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

void ViewRecords(List<WalkingRecord> records)
{
	var table = new Table();
	table.AddColumn("Id");
	table.AddColumn("Date");
	table.AddColumn("Amount");

	foreach (var record in records)
	{
		table.AddRow(record.Id.ToString(), record.Date.ToString(), record.Meters.ToString());
	}

	AnsiConsole.Write(table);
}

void CreateDatabase()
{
	using (SqliteConnection connection = new(connectionString))
	{
		using (SqliteCommand tableCmd = connection.CreateCommand())
		{
			connection.Open();
			tableCmd.CommandText =
				@"CREATE TABLE IF NOT EXISTS walkingHabit (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					Date TEXT,
					Quantity INTEGER
					)";
			tableCmd.ExecuteNonQuery();
		}
	}
}

record WalkingRecord(int Id, DateTime Date, int Meters);