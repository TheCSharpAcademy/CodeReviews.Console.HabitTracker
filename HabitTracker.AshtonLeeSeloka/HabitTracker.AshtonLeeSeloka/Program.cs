
bool closeApplication = false;

StartMenu();

 void StartMenu()
{
	while (!closeApplication) 
	{
		Console.WriteLine("\n\n\n+==============================================================================+\r\n|  _   _    _    ____ ___ _____   _____ ____      _    ____ _  _______ ____    |\r\n| | | | |  / \\  | __ )_ _|_   _| |_   _|  _ \\    / \\  / ___| |/ / ____|  _ \\   |\r\n| | |_| | / _ \\ |  _ \\| |  | |     | | | |_) |  / _ \\| |   | ' /|  _| | |_) |  |\r\n| |  _  |/ ___ \\| |_) | |  | |     | | |  _ <  / ___ \\ |___| . \\| |___|  _ <   |\r\n| |_| |_/_/   \\_\\____/___| |_|     |_| |_| \\_\\/_/   \\_\\____|_|\\_\\_____|_| \\_\\  |\r\n+==============================================================================+");
		Console.WriteLine("\nSelect from the below menu");
		Console.WriteLine("\ni) Insert Habit Data");
		Console.WriteLine("u) Update Habit Data");
		Console.WriteLine("m) Modify Habit Data");
		Console.WriteLine("d) Delete Habit Data\n");

		string userInput =  Console.ReadLine();

		switch (userInput) 
		{
			case "i":
				break;
			case "u":
				break;
			case "m":
				break;
			case "d":
				break;
			default:
				Console.WriteLine("\nPlease Enter a valid menu item");
				break;
		}
	}
}
