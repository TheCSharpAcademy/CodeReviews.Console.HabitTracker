using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Globalization;

internal class Program
{
	static string connectionString = @"Data Source=habit-Tracker.db";
	private static void Main(string[] args)
	{
		Batteries.Init();
		string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "habit-Tracker.db");

		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();

			var tableCmd = connection.CreateCommand();
			tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT NOT NULL, Quantity INTEGER NOT NULL)";

			tableCmd.ExecuteNonQuery();

			connection.Close();
		}

		GetUserInfo();
	}

	static void GetUserInfo()
	{
		Console.Clear();
		var closeApp = false;
		while (closeApp == false)
		{
			Console.WriteLine("\nMENU");
			Console.WriteLine("\nHere you can chose what habit you would like to log.");
			Console.WriteLine("0 - Quit the application");
			Console.WriteLine("1 - GET all logged habits");
			Console.WriteLine("2 - ADD a new habit");
			Console.WriteLine("3 - DELETE a habit");
			Console.WriteLine("4 - UPDATE a habit");
			Console.WriteLine("----------------------------------------------------");

			string getUserInput = Console.ReadLine();

			switch (getUserInput)
			{
				case "0":
					closeApp = true;
					Console.WriteLine("\nGoodBye!\n");
					Environment.Exit(0);
					break;
				case "1":
					GetAllHabits();
					break;
				case "2":
					AddHabit();
					break;
				case "3":
					DeleteHabit();
					break;
				case "4":
					UpdateHabit();
					break;
				default:
					Console.WriteLine("Invalid command! Please, try again!");
					break;
			}
		}
	}

	private static string GetDateInput(string message)
	{
		Console.WriteLine(message);

		string input = Console.ReadLine();

		if (input == "0") GetUserInfo();

		DateTime dt;
		while (!DateTime.TryParseExact(input, "dd/MM/yyyy", null, DateTimeStyles.None, out dt))
		{
			Console.WriteLine("Invalid date, please retry");
			input = Console.ReadLine();
		}

		return input;
	}

	private static int GetNumberInput(string message)
	{
		Console.WriteLine(message);

		string input = Console.ReadLine();

		if (input == "0") GetUserInfo();

		int number;

		while (!int.TryParse(input, out number) || int.Parse(input) <= 0)
		{
			Console.WriteLine("Invalid input for habit");
			input = Console.ReadLine();
		}

		return number;
	}
	private static void AddHabit()
	{
		Console.Clear();

		var dateInput = GetDateInput("\nPlease put the date for the habit or press 0 to return to Menu!\n");

		var numberInput = GetNumberInput("\nPlease put the quantity for the habit or press 0 to return to Menu!\n");

		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();

			var tableCmd = connection.CreateCommand();
			tableCmd.CommandText = $"INSERT INTO drinking_water (date, quantity) VALUES ('{dateInput}', '{numberInput}')";

			tableCmd.ExecuteNonQuery();

			connection.Close();
		}

		Console.WriteLine("\nYou have successfully added a new habit! :)\n");
	}

	private static void GetAllHabits()
	{
		Console.Clear();

		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var tableCmd = connection.CreateCommand();

			tableCmd.CommandText = "SELECT * FROM drinking_water";

			var tableData = new List<DrinkingWater>();

			SqliteDataReader reader = tableCmd.ExecuteReader();

			while (reader.Read())
			{
				if (reader.HasRows)
				{
					tableData.Add(new DrinkingWater
					{
						Id = reader.GetInt32(0),
						Date = DateTime.ParseExact(reader.GetString(1), "dd/MM/yyyy", new CultureInfo("en-US")), //possible bug
						Quantity = reader.GetInt32(2),
					});
				}
				else
				{
					Console.WriteLine("No rows to show :(.");
				}
			}

			foreach (var drinkingWater in tableData)
			{
				Console.WriteLine($"Habit with Id - {drinkingWater.Id} => {drinkingWater.Date.ToString("dd/MM/yyyy")} - {drinkingWater.Quantity}");
			}

			Console.WriteLine("----------------------------------------------------");
		}
	}

	private static void UpdateHabit()
	{
		Console.Clear();
		GetAllHabits();

		var id = GetNumberInput("Chose the Id you would like to update");

		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var checkCmd = connection.CreateCommand();
			checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water where Id = {id})";

			var rows = Convert.ToInt32(checkCmd.ExecuteScalar());

			if (rows == 0)
			{
				Console.WriteLine("Habit with this Id doesn't exist");
				connection.Close();
				UpdateHabit();
			}

			string date = GetDateInput("\nPlease put the date for the habit or press 0 to return to Menu!\n");
			int quantity = GetNumberInput("\nPlease insert number of glasses or other measure of your choice(no decimals allowed)\n");

			var tableCmd = connection.CreateCommand();
			tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {id}";

			tableCmd.ExecuteNonQuery();

			connection.Close();

			Console.WriteLine($"\nRecord with an Id {id} was updated");

		}
	}

	private static void DeleteHabit()
	{
		Console.Clear();
		GetAllHabits();
		var numberInput = GetNumberInput("\nPut the Id of the habit you want to delete or press 0 to return to Menu!\n");

		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var tableCmd = connection.CreateCommand();

			tableCmd.CommandText = $"DELETE FROM drinking_water where Id = {numberInput}";

			var rows = tableCmd.ExecuteNonQuery();

			if (rows == 0)
			{
				Console.WriteLine("Habit with this Id doesn't exist");
				DeleteHabit();
			}

			Console.WriteLine($"Record with an Id {numberInput} was deleted");

			GetUserInfo();
		}
	}

	public class DrinkingWater
	{
		public int Id { get; set; }

		public DateTime Date { get; set; }

		public int Quantity { get; set; }
	}
}
