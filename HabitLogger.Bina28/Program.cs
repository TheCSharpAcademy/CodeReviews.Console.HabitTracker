
using Microsoft.Data.Sqlite;


namespace HelloApp
{
	class Program
	{
		// Class-level variable for connection string
		static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

		static void Main(string[] args)
		{
			DatabaseManager databaseManager = new();
			databaseManager.CreateTable(connectionString);
			bool continueOperation = true;
			while (continueOperation)
			{
				Console.WriteLine("------------------------------------------------");
				Console.WriteLine("MAIN MENU" +
					"\nWhat would you like to do:   " +
					" \n0 - Close application   " +
					" \n1 - View all data   " +
					" \n2 - Insert new record   " +
					" \n3 - Update record   " +
					" \n4 - Delete record\n");
				Console.WriteLine("------------------------------------------------");
				Console.Write("Enter your option: ");
				string? input = Console.ReadLine();
				Console.WriteLine();
				switch (input.ToLower())
				{
					case "0":
						continueOperation = false;
						break;
					case "1":
						ViewData();
						break;
					case "2":
						InsertData();
						break;
					case "3":
						UpdateData();
						break;
					case "4":
						DeleteHistory();
						break;
					default:
						Console.WriteLine("Invalid option, please try again.");
						break;
				}

				if (continueOperation) // Only ask if we haven't chosen to exit
				{
					Console.WriteLine("\nDo you want to perform another operation? (y/n)");
					string userResponse = Console.ReadLine().ToLower();
					if (userResponse != "y")
					{
						continueOperation = false;
					}
					Console.Clear();
				}
			}

		}
		private static void ViewData()
		{
			using (var db = new SqliteConnection(connectionString))
			{
				db.Open();
				string sqlQuery = "SELECT * FROM coding";
				using (var cmd = new SqliteCommand(sqlQuery, db))
				{
					using (SqliteDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							Console.WriteLine($"Id {reader["Id"]}: Date: {reader["Date"]}, Duration: {reader["Duration"]}");

						}
					}
				}
			}
		}

		private static void InsertData()
		{
			string date = DateInput();
			int duration = DurationInput();
			using (var db = new SqliteConnection(connectionString))
			{
				db.Open();
				string insertQuery = "INSERT INTO coding (Date, Duration) VALUES (@date, @duration)";
				using (var cmd = new SqliteCommand(insertQuery, db))
				{
					cmd.Parameters.AddWithValue("@date", date);
					cmd.Parameters.AddWithValue("@duration", duration);
					cmd.ExecuteNonQuery();
					Console.WriteLine($"\nThe record with the date: {date} and duration: {duration} has been successfully saved to the database.");
				}
			}
		}

		private static void DeleteHistory()
		{
			Console.WriteLine("To delete a record, please provide the ID of that record.");
			int id = IdInput();

			if (!RecordExist(id))
			{
				Console.WriteLine($"No record found with the ID {id}. Update operation cannot be performed.");
				return; // Exit the method if the record doesn't exist
			}

			using (var db = new SqliteConnection(connectionString))
			{
				db.Open();
				string deleteQuery = "DELETE FROM coding WHERE Id=@id";
				using (var cmd = new SqliteCommand(deleteQuery, db))
				{
					cmd.Parameters.AddWithValue("@id", id);
					cmd.ExecuteNonQuery();
					Console.WriteLine($"The record with the id: {id} has been successfully deleted from the database.");
				}
			}
		}

		private static void UpdateData()
		{
			Console.WriteLine("To update a record, please provide the ID along with the new values for the date and duration.");
			int id = IdInput();
			if (!RecordExist(id))
			{
				Console.WriteLine($"No record found with the ID {id}. Update operation cannot be performed.");
				return; // Exit the method if the record doesn't exist
			}
			string date = DateInput();
			int duration = DurationInput();

			using (var db = new SqliteConnection(connectionString))
			{
				db.Open();

				string updateQuery = "UPDATE coding SET Date=@date, Duration=@duration WHERE Id=@id";
				using (var cmd = new SqliteCommand(updateQuery, db))
				{
					cmd.Parameters.AddWithValue("@date", date);
					cmd.Parameters.AddWithValue("@duration", duration);
					cmd.Parameters.AddWithValue("@id", id);
					cmd.ExecuteNonQuery();
					Console.WriteLine($"The record with the id {id} has been successfully updated.");
				}
			}
		}

		private static bool RecordExist(int id)
		{
			using (var db = new SqliteConnection(connectionString))
			{
				db.Open();
				string query = "SELECT COUNT(1) FROM coding WHERE Id=@id";
				using (var cmd = new SqliteCommand(query, db))
				{
					cmd.Parameters.AddWithValue("@id", id);
					return (long)cmd.ExecuteScalar() > 0; // Returns true if the record exists
				}
			}
		}

		private static int DurationInput()
		{
			int duration;
			Console.Write("Enter the duration: ");
			while (!int.TryParse(Console.ReadLine(), out duration))
			{
				Console.WriteLine("Invalid input. Please enter a valid integer for duration.");
				Console.Write("Enter the duration: ");
			}
			return duration;
		}

		private static string DateInput()
		{
			bool isValidInput = false;
			string date = "";
			string format = "yyyy-MM-dd";
			while (!isValidInput)
			{
				Console.WriteLine("Do you want to enter today's day? (y/n)");
				string userInput = Console.ReadLine().ToLower();
				if (userInput == "y")
				{
					isValidInput = true;
					date = DateTime.Now.ToString(format);
				}
				else if (userInput == "n")
				{
					isValidInput = true;
					Console.Write($"Enter a date ({format}): ");
					date = Console.ReadLine();
					while (!IsValidDate(date))
					{
						Console.WriteLine($"Invalid date format. Please use {format}.");
						Console.Write($"Enter a date ({format}): ");
						date = Console.ReadLine();
					}
				}
				else
				{
					Console.WriteLine("Invalid input! Please enter 'y' or 'n'.");
				}
			}
			return date;
		}



		private static bool IsValidDate(string date)
		{
			string format = "yyyy-MM-dd";

			// Try to parse the date string
			return DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out _);
		}

		private static int IdInput()
		{
			int id;
			Console.Write("Enter the ID: ");
			while (!int.TryParse(Console.ReadLine(), out id))
			{
				Console.WriteLine("Invalid input. Please enter a valid integer for ID.");
				Console.Write("Enter the ID: ");
			}
			return id;
		}
	}
}



