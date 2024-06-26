using Microsoft.Data.Sqlite;
using Spectre.Console;

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
				//AddRecord();
				break;
			case "Delete Record":
				//DeleteRecord();
				break;
			case "View Records":
				//ViewRecords();
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