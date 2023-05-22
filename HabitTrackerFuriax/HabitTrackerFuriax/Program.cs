using HabitTrackerFuriax.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;


Console.Write("Wich habit would you like to track : ");
string habit = Console.ReadLine();
Console.Title = habit;
Console.Write("Which measurement unit should be used ?: ");
string measurement = Console.ReadLine();

string connectionString = @"Data Source=habitTracker.db";

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
			case "0": Environment.Exit(0);
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
	Console.Write("Enter the id of the record you want to update: ");
	int idToUpdate = Convert.ToInt32(Console.ReadLine());
	string newDate = GetDate("Enter a new value for date: ");
	int newQuantity = GetQuantity("Enter a new value for quantity: ");

	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var command = connection.CreateCommand();
		command.CommandText = $"SELECT 1 FROM habit WHERE Id ={idToUpdate}";




		command.CommandText =
			@$"UPDATE habit SET Date = '{newDate}', Quantity = '{newQuantity}' WHERE Id ='{idToUpdate}'";
		int rowCount = command.ExecuteNonQuery();
		if (rowCount == 0) 
		{
			Console.WriteLine($"Record nr {idToUpdate} doesn't exists, please enter an existing recordid");
			Console.ReadLine();
			connection.Close();
			UpdateRecord();
		}
		connection.Close();
	}
	Console.WriteLine($"Record nr {idToUpdate} succesfully updated.");
	Console.ReadLine();

}

void DeleteRecord()
{
	Console.Clear();
	ViewRecords();
	
	Console.Write("Enter the id of the record you want to delete: ");
	int idToDelete = Convert.ToInt32(Console.ReadLine());

	using (var connection = new SqliteConnection(connectionString))
	{
		connection.Open();
		var command = connection.CreateCommand();

		command.CommandText = $"DELETE FROM habit WHERE id = '{idToDelete}'";
		int rowCount = command.ExecuteNonQuery();
		
		if (rowCount == 0)
		{
			Console.WriteLine($"Record nr {idToDelete} doesn't exists, please enter an existing recordid");
			Console.ReadLine();
			connection.Close();
			DeleteRecord();
		}

		connection.Close();
	}
    Console.WriteLine($"Record nr {idToDelete} succesfully deleted.");
	Console.ReadLine ();
}

void InsertRecord()
{
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
	int input = Convert.ToInt32(Console.ReadLine());
	if (input == 0)
		GetInput();
	return input;
}

string GetDate(string question)
{
    Console.WriteLine(question);
	string input = Console.ReadLine();
	if (input == "0")
		GetInput();
	return input;
}

