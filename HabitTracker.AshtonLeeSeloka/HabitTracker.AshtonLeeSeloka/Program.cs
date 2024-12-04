
using DataService;

bool closeApplication = false;
DataServices data = new DataServices();

StartMenu();

 void StartMenu()
{
	while (!closeApplication) 
	{
		Console.WriteLine("\n\n\n+==============================================================================+\r\n|  _   _    _    ____ ___ _____   _____ ____      _    ____ _  _______ ____    |\r\n| | | | |  / \\  | __ )_ _|_   _| |_   _|  _ \\    / \\  / ___| |/ / ____|  _ \\   |\r\n| | |_| | / _ \\ |  _ \\| |  | |     | | | |_) |  / _ \\| |   | ' /|  _| | |_) |  |\r\n| |  _  |/ ___ \\| |_) | |  | |     | | |  _ <  / ___ \\ |___| . \\| |___|  _ <   |\r\n| |_| |_/_/   \\_\\____/___| |_|     |_| |_| \\_\\/_/   \\_\\____|_|\\_\\_____|_| \\_\\  |\r\n+==============================================================================+");
		Console.WriteLine("\nSelect from the below menu");
		Console.WriteLine("\n1) Insert Habit Data");
		Console.WriteLine("2) Update Habit Data");
		Console.WriteLine("3) Delete Habit Data");
		Console.WriteLine("4) View Stored Habbits");
		Console.WriteLine("5) Generate report");
		Console.WriteLine("\nType 0 to exit\n");

		string userInput =  Console.ReadLine();

		switch (userInput) 
		{
			case "1":
				data.InsertRecord();
				break;
			case "2":
				data.UpdateRecords();
				break;
			case "3":
				data.DeleteRecord();
				break;
			case "4":
				data.GetAllRecords();
				break;
			case "5":
				data.GenerateReport();
				break;
			case "0":
				Console.WriteLine("Good Bye...");
				Environment.Exit(0);
				break;
			default:
				Console.WriteLine("\nPlease Enter a valid menu item");
				break;
		}
	}
}
