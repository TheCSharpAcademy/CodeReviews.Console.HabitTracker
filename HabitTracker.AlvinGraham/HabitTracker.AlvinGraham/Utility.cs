using System.Globalization;

namespace HabitTracker;

internal class Utility
{
	public static string connectionString = @"Data Source=habit-Tracker.db";

	public record Habit(int Id, string Name, string UnitOfMeasurement);
	public record RecordWithHabit(int Id, DateTime Date, int Quantity, string HabitName, string MeasurementUnit);

	internal static int GetNumber(string message)
	{
		Console.WriteLine(message);
		string? numberInput = Console.ReadLine();

		if (numberInput == "0")
			Menu.MainMenu();

		int outpout = 0;
		while (!int.TryParse(numberInput, out outpout) || Convert.ToInt32(numberInput) < 0)
		{
			Console.WriteLine("\n\nInvalid number. Try again.\n\n");
			numberInput = Console.ReadLine();
		}

		return outpout;
	}

	internal static string GetDate(string message)
	{
		Console.WriteLine(message);
		string? dateInput = Console.ReadLine();

		if (dateInput == "0")
			Menu.MainMenu();

		while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Please try again\n\n");
			dateInput = Console.ReadLine();
		}

		return dateInput;
	}

	internal static void ClearScreen(string message)
	{
		Console.Clear();
		Console.WriteLine(message);
		Console.WriteLine("--------------------------------------------");
	}

}
