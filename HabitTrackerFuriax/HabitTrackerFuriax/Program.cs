using HabitTrackerFuriax.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

string connectionString = @"Data Source=habitTracker.db";
string habit = "drink water";
string measurement = "cl";

//GetHabit();

//create db if it doesnt exist yet
using (var connection = new SqliteConnection(connectionString))
{
	connection.Open();
	var command = connection.CreateCommand();

	command.CommandText = 
		@"CREATE TABLE IF NOT EXISTS habit(
		Id INTEGER PRIMARY KEY AUTOINCREMENT,
		Date TEXT,
		Quantity INTEGER)";
	command.ExecuteNonQuery();
	connection.Close();
}
GetInput();



void GetInput()
{
	Console.Clear();
	bool closeApp = false;
	while (closeApp == false)
	{
		
		Console.WriteLine($"Welcome to the {habit} tracker");
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
			case "0": closeApp = true;  Environment.Exit(0);
				break;
			case "1": ViewRecords(); Console.ReadLine(); Console.Clear();
				break;
			case "2": InsertRecord(); Console.Clear();
				break;
			case "3": DeleteRecord(); Console.Clear();
				break;
			case "4": UpdateRecord(); Console.Clear();
				break;
			default:
                Console.WriteLine("Invalid input, please enter a correct number"); Console.Clear();
                break;
		}
	}
}

void ViewRecords()
{
	
	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = "SELECT * FROM habit";
		List<DrinkWater> records = new();
		SqliteDataReader reader = command.ExecuteReader();
		if (reader.HasRows)
		{
			while (reader.Read())
			{
				records.Add(new DrinkWater
				{
					Id = reader.GetInt32(0),
					Date = DateTime.ParseExact(reader.GetString(1), "dd/MM/yy", new CultureInfo("nl-BE")),
					Quantity = reader.GetInt32(2)
				}); ;
			}
		}
		else
		{
			Console.Clear();
			Console.WriteLine("No data found in database");
			Console.ReadLine();
			GetInput();
		}

        connection.Close();

		Console.Clear();
		Console.WriteLine("***********************************************");
		foreach (var record in records)
		{
            Console.WriteLine($"{record.Id} - {record.Date.ToString("dd/MM/yyyy")} - {record.Quantity} {measurement}");
        }
		Console.WriteLine("***********************************************");
	}
}
void UpdateRecord()
{
	Console.Clear();
	ViewRecords();
	Console.Write("Enter the id of the record you want to update: ");
	string input = CheckForValidNumber( Console.ReadLine());
	int idToUpdate = Convert.ToInt32(input);

	using (var connection = new SqliteConnection(connectionString))
	{
		int checkQuery = 0;
		
		while (checkQuery == 0)
		{
			connection.Open();
			var cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit WHERE Id ={idToUpdate})";
			checkQuery = Convert.ToInt32(cmd.ExecuteScalar());

			if (checkQuery == 0)
			{
				Console.WriteLine($"Record nr {idToUpdate} doesn't exists, please enter an existing recordid or enter 0 to return to main menu: ");
				input = CheckForValidNumber(Console.ReadLine());
				idToUpdate = Convert.ToInt32(input);
				if (idToUpdate == 0)
					GetInput();
			}		
			connection.Close();
		}
		connection.Open();
		var command = connection.CreateCommand();
		string newDate = GetDate("Enter a new value for date: ");
		int newQuantity = GetQuantity("Enter a new value for quantity: ");
		command.CommandText = @$"UPDATE habit SET Date = '{newDate}', Quantity = '{newQuantity}' WHERE Id ='{idToUpdate}'";
		connection.Close() ;
	}
	Console.WriteLine($"Record nr {idToUpdate} succesfully updated.");
	Console.ReadLine();

}



void DeleteRecord()
{
	Console.Clear();
	ViewRecords();
	
	Console.Write("Enter the id of the record you want to delete: ");
	string input = CheckForValidNumber(Console.ReadLine());
	int idToDelete = Convert.ToInt32(input);

	using (var connection = new SqliteConnection(connectionString))
	{
		int row = 0;
		
		
		while (row == 0)
		{
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandText = $"DELETE FROM habit WHERE id = '{idToDelete}'";
			row = command.ExecuteNonQuery();
			if (row == 0)
			{
				Console.Write($"Record nr {idToDelete} doesn't exists, enter an existing record id or 0 to return to main menu : ");
				input = CheckForValidNumber(Console.ReadLine());
				idToDelete = Convert.ToInt32(input);
				if (idToDelete == 0)
					GetInput();
			}
			connection.Close();
		}
	}
    Console.WriteLine($"Record nr {idToDelete} succesfully deleted.");
	Console.ReadLine ();
}

void InsertRecord()
{
	Console.Clear();
	string date = GetDate("Enter the date (dd/mm/yy), type 0 to return to main menu");
	int quantity = GetQuantity($"How many {measurement} ?");

	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = $"INSERT INTO habit(date, quantity) VALUES ('{date}','{quantity}')";
		command.ExecuteNonQuery();
		connection.Close();
	}
}

int GetQuantity(string question)
{
    Console.WriteLine(question);
	string input = Console.ReadLine();
	if (input == "0")
		GetInput();
	while (!Int32.TryParse(input, out _) || Convert.ToInt32(input) < 0)
	{
		Console.Write("Invalid number, please enter a valid integer: ");
		input = Console.ReadLine();
	}
	int output = Convert.ToInt32(input);
	return output;
}

string GetDate(string question)
{
    Console.WriteLine(question);
	string input = Console.ReadLine();
	if (input == "0")
		GetInput();
	while (!DateTime.TryParseExact(input, "dd/MM/yy", new CultureInfo("nl-BE"), DateTimeStyles.None, out _))
	{
		Console.Write("Invalid date, format need to be dd/mm/yy, please enter a valid date: ");
		input = Console.ReadLine();
	}
	return input;
}

void GetHabit()
{
	Console.Write("Wich habit would you like to track : ");
	habit = Console.ReadLine();
	while (string.IsNullOrEmpty(habit))
	{
		Console.Write("This can't be empty, please enter a habit: ");
		habit = Console.ReadLine();
	}
	Console.Title = $"{habit} tracker";

	Console.Write("Which measurement unit should be used ?: ");
	measurement = Console.ReadLine();
	while (string.IsNullOrEmpty(measurement))
	{
		Console.Write("This can't be empty, please enter a measurement unit: ");
		measurement = Console.ReadLine();
	}
}
string CheckForValidNumber(string? input)
{
	while (string.IsNullOrEmpty(input) || Int32.TryParse(input, out _) == false)
	{
		Console.Write("Not a valid value, try again: ");
		input = Console.ReadLine();
	}
	return input;
}