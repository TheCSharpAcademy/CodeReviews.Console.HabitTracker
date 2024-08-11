using Microsoft.Data.Sqlite;
using System.Text;

namespace HabitLoggerLibrary;

public class SetupDatabase
{
    public void InitializeDatabase()
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();

                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS ""DrinkWater"" (
	                            ""Id""	INTEGER NOT NULL,
	                            ""Day""	TEXT NOT NULL,
	                            ""Quantity""	INTEGER NOT NULL,
	                            PRIMARY KEY(""Id"" AUTOINCREMENT)
                                );";
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"An error ocurred: {ex.Message}");
        }
    }

    public void SeedData()
    {
        string table = "BooksRead";
        try
        {
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();

                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = @$"CREATE TABLE IF NOT EXISTS ""{table}"" (
	                            ""Id""	INTEGER NOT NULL,
	                            ""Day""	TEXT NOT NULL,
	                            ""Quantity""	INTEGER NOT NULL,
	                            PRIMARY KEY(""Id"" AUTOINCREMENT)
                                ); ";
                cmd.ExecuteNonQuery();

                cmd.CommandText = RandomInsertValues(table);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"An error ocurred: {ex.Message}");
        }
    }

    private string RandomInsertValues(string table)
    {
        Random rnd = new Random();
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 100; i++)
        {
            int year = rnd.Next(2003, 2024);
            int month = rnd.Next(1, 13);
            int day = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
            sb.Append($"\nINSERT INTO {table}(Day, Quantity) VALUES('{year}-{month:00}-{day:00}', {rnd.Next(0, 51)});");
        }

        return sb.ToString();
    }
}
