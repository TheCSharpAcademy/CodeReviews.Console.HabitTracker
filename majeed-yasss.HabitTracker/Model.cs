using System.Globalization;
using Microsoft.Data.Sqlite;

namespace majeed_yasss.HabitTracker;
internal class Model
{
    public static readonly string connectionString = @"Data Source=habit-Tracker.db";
    public static void LoadDatabase()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

        tableCmd.ExecuteNonQuery();


        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM drinking_water)";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
        if (checkQuery == 0) SeedData(10);
    }
    public static List<DrinkingWater> GetAllRecords()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                "SELECT * FROM drinking_water ";

            List<DrinkingWater> tableData = [];

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    });
                }
            }
            else Console.WriteLine("No rows found");
            connection.Close();
            return tableData;
        }
    }
    public static void Insert(string date, int quantity)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
               "INSERT INTO drinking_water(date, quantity) VALUES( @date, @quantity)";

            tableCmd.Parameters.Add("@date", SqliteType.Text).Value = date;
            tableCmd.Parameters.Add("@quantity", SqliteType.Integer).Value = quantity;

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    public static bool Delete(int recordId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "DELETE from drinking_water WHERE id = @id";
            tableCmd.Parameters.Add("@id", SqliteType.Integer).Value = recordId;

            int rowCount = tableCmd.ExecuteNonQuery();

            return (rowCount != 0);
        }
    }
    public static bool Update(int recordId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = @id)";
            checkCmd.Parameters.Add("@id", SqliteType.Integer).Value = recordId;

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                connection.Close();
                return false;
            }

            // Ideally we don't want the model to intract with the view directly
            // this one might pass though as we're just asking for input (right?)
            // (maybe override Insert() with a recordId parameter?)
            int quantity = View.GetPositiveInt("\n\nNumber of glasses (no decimals allowed)\n\n");
            string date = View.GetDateInput();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "UPDATE drinking_water SET date = @date, quantity = @quantity WHERE id = @id";

            tableCmd.Parameters.Add("@id", SqliteType.Integer).Value = recordId;
            tableCmd.Parameters.Add("@date", SqliteType.Text).Value = date;
            tableCmd.Parameters.Add("@quantity", SqliteType.Integer).Value = quantity;

            tableCmd.ExecuteNonQuery();

            connection.Close();
            return true;
        }
    }
    private static void SeedData(int elements) 
    {
        Random random = new();
        while(--elements > 0)
        {
            int year = random.Next(20,25);
            int month = random.Next(1,13);
            int day = random.Next(1,31);
            string date = string.Format("{0:D2}-{1:D2}-{2:D2}",
                                       day, month, year);
            Insert(date, random.Next(1,10));
        }
    }
}
