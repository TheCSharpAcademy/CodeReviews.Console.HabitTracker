using Microsoft.Data.Sqlite;


string connectionString = @"Data Source=habitTracker.db";
using (var connection = new SqliteConnection(connectionString))
{
	connection.Open();
	var command = connection.CreateCommand();

	command.CommandText = 
		@"CREATE TABLE IF NOT EXISTS drinking(
		Id INTEGER PRIMARY KEY AUTOINCREMENT,
		Date TEXT,
		Quantity INTEGER)";
	command.ExecuteNonQuery();
	connection.Close();
}



void GetInput()
{
	Console.Clear();
	bool closeApp = false;
	while (closeApp == false)
	{
        Console.WriteLine("Welcome to this habit tracker");
		Console.WriteLine("---------------------------------");
		Console.WriteLine("Please make a selection:");
		Console.WriteLine("Type 1 to View All Records");
		Console.WriteLine("Type 2 to Insert a Record");
        Console.WriteLine("Type 3 to Delete a Record");
        Console.WriteLine("Type 4 to Update a Record");
        Console.WriteLine("Type 0 to Close this Application");
        Console.WriteLine("---------------------------------");
		string input = Console.ReadLine();

		switch (input)
		{
			case "0": Environment.Exit(0);
				break;
			case "1": ViewRecords();
				break;
			case "2": InsertRecord();
				break;
			case "3": DeleteRecord();
				break;
			case "4": UpdateRecord();
				break;
			default:
                Console.WriteLine("Invalid input, please enter a correct number");
                break;
		}
	}
}

void UpdateRecord()
{
	throw new NotImplementedException();
}

void DeleteRecord()
{
	throw new NotImplementedException();
}

void InsertRecord()
{
	throw new NotImplementedException();
}

void ViewRecords()
{
	throw new NotImplementedException();
}