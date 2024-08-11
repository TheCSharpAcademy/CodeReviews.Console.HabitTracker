using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary;

public class UserDefinedHabitsController
{
    // To create habits for the user we create a new table
    public static void CreateHabit(string habitName, string quantityType)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();

                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = @$"CREATE TABLE IF NOT EXISTS ""{habitName.Trim()}"" (
	                            ""Id""	INTEGER NOT NULL,
	                            ""Day""	TEXT NOT NULL,
	                            ""Quantity""	{quantityType} NOT NULL,
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

    public static void DeleteHabit(string habitName)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();

                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = @$"DROP TABLE IF EXISTS ""{habitName.Trim()}"";";
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"An error ocurred: {ex.Message}");
        }
    }

    public static Dictionary<int, string> GetHabits()
    {
        Dictionary<int, string> tables = new Dictionary<int, string> ();
        try
        {
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();

                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = @$"SELECT row_number() over() AS id, name FROM sqlite_schema WHERE type='table' AND name NOT LIKE 'sqlite_%';";
                SqliteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tables.Add(Convert.ToInt32(reader["ID"]), reader["name"]!.ToString());
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"An error ocurred: {ex.Message}");
        }

        return tables;
    }
}
