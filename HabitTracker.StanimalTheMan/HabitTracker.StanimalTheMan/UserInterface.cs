

namespace HabitTracker.StanimalTheMan;

internal class UserInterface
{
	internal static void ShowMenu()
	{
		Console.WriteLine("MAIN MENU");
		Console.WriteLine();
		Console.WriteLine("What would you like to do?");
		Console.WriteLine();
		Console.WriteLine("Type 0 to Close Application.");
		Console.WriteLine("Type 1 to View All Records.");
		Console.WriteLine("Type 2 to Insert Record.");
		Console.WriteLine("Type 3 to Delete Record.");
		Console.WriteLine("Type 4 to Update Record.");

		string userInput = Console.ReadLine();

		HandleUserInput(userInput);
	}

	private static void HandleUserInput(string userInput)
	{
		switch (userInput)
		{
			case "0":
				Environment.Exit(0);
				break;
			case "1":

		}
	}
}
