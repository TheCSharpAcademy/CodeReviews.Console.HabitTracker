using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace HabitLogger;

internal class Program
{
	private static void Main(string[] args)
	{
		const string connectionString = "Data Source=habits.db";
		try
		{
			using (var db = new SqliteConnection(connectionString))
			{
				db.Open();
				using (var command = new SqliteCommand { Connection = db })
				{
					command.CommandText =
						@" 
						CREATE TABLE IF NOT EXISTS Habits
						(
						Id INTEGER PRIMARY KEY,
						Title TEXT NOT NULL,
						Date DATE NOT NULL,
						Quantity INTEGER NOT NULL
						)
					";
					command.ExecuteNonQuery();

					bool running = true;
					while (running)
					{
						PrintMenu();
						var option = Console.ReadLine();
						while (!Regex.IsMatch(option!, "[0|1|2|3|4|5]"))
						{
							Console.WriteLine("Not a valid option. Try again.");
							option = Console.ReadLine();
						}
						try
						{
							switch (option)
							{
								case "0":
									Console.WriteLine("Closing app...\nGoodbye!");
									running = false;
									break;
								case "1":
									GetAllHabits(db);
									break;
								case "2":
									(string, DateTime, int) habit = GetHabitFromUser();
									using (var newCom = InsertHabit(habit, db))
									{
										newCom.ExecuteNonQuery();
									}
									break;
								case "3":
									int toDelete = GetId();
									if (!HabitExists(db, toDelete))
									{
										Console.WriteLine("Habit does not exist.");
										break;
									}
									Console.Write("-- DELETED ROW -- \n");
									GetSingleHabit(db, toDelete);
									using (var delCom = DeleteHabit(toDelete, db))
									{
										delCom.ExecuteNonQuery();
									}
									break;
								case "4":
									Console.WriteLine("What would you like to update?");
									int toUpdate = GetId();
									if (!HabitExists(db, toUpdate))
									{
										Console.WriteLine("Habit does not exist.");
										break;
									}
									(string, DateTime, int) habitTuple = GetHabitFromUser();
									Console.Write("-- UPDATED ROW -- \n");
									GetSingleHabit(db, toUpdate);
									using (var updateCom = UpdateHabit(toUpdate, habitTuple, db))
									{
										updateCom.ExecuteNonQuery();
									}

									break;
								case "5":
									SeedDb(db);
									break;
							}
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							throw;
						}

					}

				}
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

	}


	private static void SeedDb(SqliteConnection dbConnection)
	{

		try
		{
			Console.WriteLine("How many rows would you like to seed?");
			string? input = Console.ReadLine();
			int rowsToSeed;
			while (!int.TryParse(input, out rowsToSeed))
			{
				Console.WriteLine("Not valid input. Enter a number.");
				input = Console.ReadLine();
			}

			var command = new SqliteCommand { Connection = dbConnection };
			Random rand = new Random();

			List<string> habits =
			[
				"Cups of Coffee Drank",
				"Miles Ran",
				"Pages Read",
				"Glasses of Water Drank",
				"Songs Listened To",
				"Lines of Code Written",
				"Videos Watched",
				"Cigarettes Smoked",
				"Alcoholic Drinks Consumed",
				"Meals Eaten",
				"Snacks Eaten",
				"Steps Taken",
				"Push-ups Done",
				"Emails Sent",
				"People Talked To",
				"Photos Taken",
				"Chores Completed"
			];

			DateTime RandomDay()
			{
				DateTime start = new DateTime(2000, 1, 1);
				int range = (DateTime.Today - start).Days;
				return start.AddDays(rand.Next(range));
			}
			for (int i = 0; i < rowsToSeed; i++)
			{

				command.CommandText = $"INSERT INTO Habits (Title, Date, Quantity) VALUES ('{habits[rand.Next(habits.Count)]}', '{RandomDay():d}', {rand.Next(1, 10)});";
				command.ExecuteNonQuery();
			}

			Console.WriteLine($"{rowsToSeed} rows inserted.");
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

	}

	private static SqliteCommand UpdateHabit(int toUpdate, (string, DateTime, int) newHabitTuple, SqliteConnection dbConnection)
	{
		var command = new SqliteCommand { Connection = dbConnection };
		command.CommandText = @"UPDATE Habits SET Title = $title, Date = $date, Quantity = $quant WHERE ID = $id";
		command.Parameters.AddWithValue("$title", newHabitTuple.Item1);
		command.Parameters.AddWithValue("$date", newHabitTuple.Item2);
		command.Parameters.AddWithValue("$quant", newHabitTuple.Item3);
		command.Parameters.AddWithValue("$id", toUpdate);
		return command;
	}

	private static bool HabitExists(SqliteConnection dbConnection, int id)
	{
		var command = new SqliteCommand { Connection = dbConnection };
		command.CommandText = @"SELECT Count(1) FROM Habits WHERE ID = $id";
		command.Parameters.AddWithValue("$id", id);
		return Convert.ToInt32(command.ExecuteScalar()) > 0;
	}

	private static SqliteCommand DeleteHabit(int id, SqliteConnection dbConnection)
	{
		var command = new SqliteCommand { Connection = dbConnection };
		command.CommandText = @"DELETE FROM Habits WHERE ID = $id";
		command.Parameters.AddWithValue("$id", id);
		return command;
	}

	private static int GetId()
	{
		Console.WriteLine("Insert the ID of the row you want to update/delete.");
		string? input = Console.ReadLine();
		int id;
		while (!int.TryParse(input, out id))
		{
			input = Console.ReadLine();
		}
		return id;
	}

	private static void PrintMenu()
	{
		Console.WriteLine("\n----------------------------");
		Console.WriteLine("\tMAIN MENU\t\t");
		Console.WriteLine("What would you like to do?\n");
		Console.WriteLine("Type 0 to Close Application");
		Console.WriteLine("Type 1 to View All Records");
		Console.WriteLine("Type 2 to Insert Record");
		Console.WriteLine("Type 3 to Delete Record");
		Console.WriteLine("Type 4 to Update Record");
		Console.WriteLine("Type 5 to Seed Database");
		Console.WriteLine("----------------------------");
	}

	public static void GetSingleHabit(SqliteConnection dbConnection, int id)
	{
		try
		{
			var command = new SqliteCommand { Connection = dbConnection };
			command.CommandText = @"SELECT * FROM Habits WHERE Id = $id";
			command.Parameters.AddWithValue("$id", id);
			command.ExecuteNonQuery();
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var idPk = reader.GetInt32(0);
					var title = reader.GetString(1);
					var date = reader.GetDateTime(2);
					var quantity = reader.GetInt32(3);
					Console.Write($"Id: {idPk}\nTitle: {title}\nDate: {date:d}\nQuantity: {quantity}\n");
				}
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

	}

	private static void GetAllHabits(SqliteConnection dbConnection)
	{
		try
		{
			var command = new SqliteCommand { Connection = dbConnection };
			command.CommandText = @"SELECT * FROM Habits";
			command.ExecuteNonQuery();
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var id = reader.GetInt32(0);
					var title = reader.GetString(1);
					var date = reader.GetDateTime(2);
					var quantity = reader.GetInt32(3);
					Console.WriteLine($"{id}: {title} {date:d} - {quantity}");
				}
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

	}

	private static (string, DateTime, int) GetHabitFromUser()
	{
		Console.WriteLine("Enter a name (STRING) for your habit");
		string? title = Console.ReadLine();
		Console.WriteLine("Enter a date ('MM-DD-YYYY') this was completed.");
		string? date = Console.ReadLine();
		DateTime time;
		while (!DateTime.TryParse(date, out time))
		{
			Console.WriteLine("Try again. 'MM-DD-YYYY' format only");
			date = Console.ReadLine();
		};
		Console.WriteLine("Enter a quantity (INTEGER) of times this occurred");
		string? quantity = Console.ReadLine();
		int outQuantity;
		while (!int.TryParse(quantity, out outQuantity))
		{
			Console.WriteLine("Not a valid integer.");
			quantity = Console.ReadLine();
		};
		return (title!, time, outQuantity);
	}

	private static SqliteCommand InsertHabit((string, DateTime, int) habitTup, SqliteConnection dbConnection)
	{
		var command = new SqliteCommand { Connection = dbConnection };
		command.CommandText =
			@"
				INSERT INTO Habits (Title, Date, Quantity) 
				VALUES ($title, $date, $quantity)
			";
		command.Parameters.AddWithValue("$title", habitTup.Item1);
		command.Parameters.AddWithValue("$date", habitTup.Item2);
		command.Parameters.AddWithValue("$quantity", habitTup.Item3);
		return command;
	}
}