using Spectre.Console;

namespace HabitTracker;

internal class Menu
{
	internal static void MainMenu()
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
					"View Reports",
					"Quit")
				);
			try
			{
				switch (userChoice)
				{
					case "Add Habit":
						Utility.ClearScreen("Adding New Habbit");
						HabitDB.AddHabit();
						break;
					case "Delete Habit":
						Utility.ClearScreen("Deleting Habbit");
						HabitDB.DeleteHabit();
						break;
					case "Update Habit":
						Utility.ClearScreen("Updating Habbit");
						HabitDB.UpdateHabit();
						break;
					case "Add Record":
						Utility.ClearScreen("Adding New Record");
						RecordDB.AddRecord();
						break;
					case "Delete Record":
						Utility.ClearScreen("Deleting Record");
						RecordDB.DeleteRecord();
						break;
					case "View Records":
						Utility.ClearScreen("Viewing Records");
						RecordDB.GetRecords();
						break;
					case "Update Record":
						Utility.ClearScreen("Updating Record");
						RecordDB.UpdateRecord();
						break;
					case "View Reports":
						Utility.ClearScreen("View Reports");
						ReportsMenu();
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
			catch (InvalidOperationException)
			{
				Utility.ClearScreen("Returned to Main Menu");

			}
		}
	}

	internal static void ReportsMenu()
	{
		var isMenuRunning = true;

		while (isMenuRunning)
		{
			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
				.Title("What report would you like to run?")
				.AddChoices(
					"Total Quantities by Habit",
					"Total Count by Habit",
					"Return to Main Menu")
				);

			try
			{
				switch (userChoice)
				{
					case "Total Quantities by Habit":
						Utility.ClearScreen("Quantities Report:");
						Reports.QuantitiesReport();
						break;
					case "Total Count by Habit":
						Utility.ClearScreen("Counts Report:");
						Reports.CountsReport();
						break;
					case "Return to Main Menu":
						Utility.ClearScreen("Returned to Main Menu");
						return;
					default:
						Console.WriteLine("Invalid Choice. Please choose one of the above");
						break;
				}
			}
			catch (InvalidOperationException)
			{
				Utility.ClearScreen("Returned to Reports Menu");
			}
		}
	}
}
