

using HabitTracker.StanimalTheMan.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.StanimalTheMan;

internal class UserInterface
{
	static bool closeApp = false;
	internal static void ShowMenu(string connectionString)
	{
		Console.Clear();

		while (closeApp == false)
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
			Console.WriteLine("-------------------------------------------\n");

			HandleUserInput(connectionString);
		}
		
	}

	private static void HandleUserInput(string connectionString)
	{
		string userInput = Console.ReadLine();
		switch (userInput)
		{
			case "0":
				Console.WriteLine("\nGoodbye!\n");
				closeApp = true;
				Environment.Exit(0);
				break;
			case "1":
				Read(connectionString);
				break;
			case "2":
				Create(connectionString);
				break;
			case "3":
				Delete(connectionString);
				break;
			case "4":
				Update(connectionString);
				break;
			default:
				Console.WriteLine("\nInvalid Command.  Please type a number from 0 to 4.\n");
				break;
		}
	}

	private static void Read(string connectionString)
	{
		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandText = $"SELECT * FROM run_distance_log";

			List<RunDistanceLogEntry> tableData = new();

			SqliteDataReader reader = command.ExecuteReader();

			if (reader.HasRows)
			{
				while (reader.Read())
				{
					Console.WriteLine(reader.GetString(1));
					tableData.Add(
					new RunDistanceLogEntry
					{
						Id = reader.GetInt32(0),
						Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
						Distance = reader.GetInt32(2)
					});
				}
			}
			else
			{
				Console.WriteLine("No rows found");
			}


			Console.WriteLine("----------------------------------------\n");
			foreach (var runDistanceLogEntry in tableData)
			{
				Console.WriteLine($"{runDistanceLogEntry.Id} - {runDistanceLogEntry.Date.ToString("dd-MMM-yyyy")} - Distance: {runDistanceLogEntry.Distance}");
			}
			Console.WriteLine("----------------------------------------\n");
		}
	}

	private static void Create(string connectionString)
	{
		string date = GetDateInput(connectionString);

		int distance = GetNumberInput("\n\nPlease insert number of miles ran or other measure of your choice (no decimals allowed)\n\n", connectionString);

		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var command = connection.CreateCommand();

			command.CommandText =
				$"INSERT INTO run_distance_log(date, distance) VALUES('{date}', {distance})";

			command.ExecuteNonQuery();
		}
	}

	private static void Delete(string connectionString)
	{
		Console.Clear();
		Read(connectionString);

		var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n", connectionString);

		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var command = connection.CreateCommand();

			command.CommandText = $"DELETE from run_distance_log WHERE Id = '{recordId}'";

			int rowCount = command.ExecuteNonQuery();

			if (rowCount == 0)
			{
				Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
				Delete(connectionString);
			}

		}

		Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
	}

	private static void Update(string connectionString)
	{
		Read(connectionString);

		var recordId = GetNumberInput("\n\nPlease type Id of the record that you would like to update.  Type 0 to return to main menu.\n\n", connectionString);

		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();

			var checkCmd = connection.CreateCommand();
			checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM run_distance_log WHERE Id = {recordId})";
			int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

			if (checkQuery == 0)
			{
				Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
				connection.Close();
				Update(connectionString);
			}

			string date = GetDateInput(connectionString);

			int distance = GetNumberInput("\n\nPlease insert number of miles run or other measure of your choice (no decimals allowed)\n\n", connectionString);

			var command = connection.CreateCommand();
			command.CommandText = $"UPDATE run_distance_log SET date = '{date}', distance = {distance} WHERE id = {recordId}";

			command.ExecuteNonQuery();
		}
	}

	private static string GetDateInput(string connectionString)
	{
		Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy).  Type 0 to return to main menu.\n\n");

		string dateInput = Console.ReadLine();

		if (dateInput == "0") HandleUserInput(connectionString);

		while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
		{
			Console.WriteLine("\n\nInvalid date.  (Format: dd-mm-yy).  Type 0 to return to main menu or try again:\n\n");
			dateInput = Console.ReadLine();
			if (dateInput == "0") HandleUserInput(connectionString);
		}

		return dateInput;
	}

	private static int GetNumberInput(string message, string connectionString)
	{
		Console.WriteLine(message);

		string numberInput = Console.ReadLine();

		if (numberInput == "0") HandleUserInput(connectionString);

		while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
		{
			Console.WriteLine("\n\nInvalid number.  Try again.\n\n");
			numberInput = Console.ReadLine();
			if (numberInput == "0") HandleUserInput(connectionString);
		}

		int finalInput = Convert.ToInt32(numberInput);

		return finalInput;
	}
}
