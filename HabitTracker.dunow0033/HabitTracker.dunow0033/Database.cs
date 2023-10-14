using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker
{
	internal static class Database
	{
		private static readonly string dbName = "habit-Tracker.db";
		private static string connectionString = @"Data Source=habit-Tracker.db";
		private static string date = DateTime.Now.ToString("dd-MM-yyyy");

		public static void SetupDB()
		{
			if (!DoesDatabaseExist())
			{
				Console.WriteLine("Database does not exist, creating it now");
				CreateDatabase();
			}
		}
		private static bool DoesDatabaseExist()
		{
			bool doesExist = false;
			string dbLocation = Path.Combine(Environment.CurrentDirectory, dbName);

			doesExist = File.Exists(dbLocation);
			return doesExist;
		}

		private static void CreateDatabase()
		{
			using (SqliteConnection con = new SqliteConnection(connectionString))
			{
				con.Open();

				var command = con.CreateCommand();
				command.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        );";

				command.ExecuteNonQuery();
				con.Close();
			}
		}

		public static List<DrinkingWater> GetEntries()
		{
			List<DrinkingWater> entries = new();

			using (var con = new SqliteConnection(connectionString))
			{
				con.Open();

				var tableCmd = con.CreateCommand();
				tableCmd.CommandText = $"SELECT * FROM drinking_water";

				using (SqliteDataReader reader = tableCmd.ExecuteReader())
				{
					if (!reader.HasRows)
					{
						return null;
					}
					else
					{
						while (reader.Read())
						{
							entries.Add(
							new DrinkingWater
							{
								Id = reader.GetInt32(0),
								Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
								Quantity = reader.GetInt32(2)
							});
						}
					}

					con.Close();
				}
			}

			return entries;
		}

		public static void AddEntry(int quantity)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var tableCmd = connection.CreateCommand();

				tableCmd.CommandText =
					$"INSERT INTO drinking_water (date, quantity) VALUES ('{date}', {quantity})";

				tableCmd.ExecuteNonQuery();

				connection.Close();
			}
		}

		public static void DeleteEntry(int id)
		{
			using (var con = new SqliteConnection(connectionString))
			{
				con.Open();

				var tableCmd = con.CreateCommand();

				tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{id}'";

				tableCmd.ExecuteNonQuery();

				con.Close();
			}
		}

		public static void UpdateEntry(int quantity, int id)
		{
			using (var con = new SqliteConnection(connectionString))
			{
				con.Open();

				var tableCmd = con.CreateCommand();
				tableCmd.CommandText =
					$"UPDATE drinking_water SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";

				tableCmd.ExecuteNonQuery();

				con.Close();
			}
		}
	}
}