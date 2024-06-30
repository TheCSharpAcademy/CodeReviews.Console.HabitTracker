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

			switch (userChoice)
			{
				case "Add Habit":
					HabitDB.AddHabit();
					break;
				case "Delete Habit":
					HabitDB.DeleteHabit();
					break;
				case "Update Habit":
					HabitDB.UpdateHabit();
					break;
				case "Add Record":
					RecordDB.AddRecord();
					break;
				case "Delete Record":
					RecordDB.DeleteRecord();
					break;
				case "View Records":
					RecordDB.GetRecords();
					break;
				case "Update Record":
					RecordDB.UpdateRecord();
					break;
				case "View Reports":
					Console.WriteLine("To Be Implemented!");
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
}
