using System;
using System.Globalization;
namespace habit_tracker;

static class InputValidation
{
	static public int GetUserInputAsInt()
	{
		int userInput;
		bool isNumerical;

		do
		{
			Console.WriteLine("Please enter an integer: ");
			isNumerical = int.TryParse(Console.ReadLine(), out userInput);
		}
		while (!isNumerical);

		return userInput;
	}
	static public string GetUserInputAsDate()
	{
		string input;
		string format = "M/dd/yyyy";
		bool isDate;
		DateTime date;
		do
		{
			Console.WriteLine($"Please enter date in {format} format.");
			input = Console.ReadLine();
			isDate = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
		}
		while (!isDate);

		return date.ToString(format);
	}
}
