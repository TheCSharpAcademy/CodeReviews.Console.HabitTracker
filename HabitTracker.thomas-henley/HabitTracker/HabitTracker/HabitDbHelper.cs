using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace HabitTracker;

internal class HabitDbHelper
{
    private static readonly string connectionString = @"Data Source=habit-tracker.db";

    public static void Initialize()
    {
        // Create table
        string commandText = @"CREATE TABLE IF NOT EXISTS heart_points (
                                   Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                   Date TEXT,
                                   Quantity INTEGER
                               )";
        SqlNonQuery(commandText);
    }

    public static void PopulateDB()
    {
        Random rand = new();
        DateTime[] dates = new DateTime[100];
        int[] quantities = new int[100];

        for (int i = 0; i < 100; i++)
        {
            string date = $"{rand.Next(1, 30):00}-{rand.Next(1, 8):00}-{rand.Next(24, 25):00}";
            dates[i] = DateTime.ParseExact(date, "dd-MM-yy", new CultureInfo("en-US"));
            quantities[i] = rand.Next(1, 100);
        }

        Array.Sort(dates);

        for (int i = 0; i < 100; i++)
        {
            Insert(dates[i].ToString("dd-MM-yy"), quantities[i]);
        }
    }

    public static bool IsDbEmpty()
    {
        return GetAllRecords().Count == 0;
    }

    /// <summary>
    /// Connect to the database, run the query command, disconnect, and return the results.
    /// </summary>
    /// <param name="cmd">The SQL command to execute.</param>
    /// <returns>A List of HeartPoints objects from the database.</returns>
    private static List<HeartPoints> SqlQuery(string cmd)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = cmd;
        var reader = tableCmd.ExecuteReader();

        List<HeartPoints> heartPoints = [];
        while (reader.Read())
        {
            HeartPoints entry = new()
            {
                Id = reader.GetInt32(0),
                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                Quantity = reader.GetInt32(2)
            };

            heartPoints.Add(entry);
        }

        connection.Close();
        return heartPoints;
    }

    /// <summary>
    /// Connects to the database, runs the command, and disconnects.
    /// Use for SQL commands that do not require a response.
    /// </summary>
    /// <param name="cmd">The SQL command to execute.</param>
    private static void SqlNonQuery(string cmd)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = cmd;
        var result = tableCmd.ExecuteNonQuery();
        connection.Close();
        return;
    }

    private static int SqlScalarQuery(string cmd)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = cmd;
        var result = tableCmd.ExecuteScalar();
        return Convert.ToInt32(result);
    }

    public static void Insert(string date, int quantity)
    {
        SqlNonQuery($"INSERT INTO heart_points(date, quantity) VALUES('{date}', {quantity})");
    }

    /// <summary>
    /// Retrieves all record from the database.
    /// </summary>
    /// <returns>A List of all entries in the heart_points table.</returns>
    public static List<HeartPoints> GetAllRecords()
    {
        return SqlQuery("SELECT * FROM heart_points");
    }

    public static bool TryGetById(int id, out HeartPoints? entry)
    {
        var results = SqlQuery($"SELECT * FROM heart_points WHERE id = {id}");

        if (results.Count > 0)
        {
            entry = results[0];
            return true;
        }

        entry = null;
        return false;
    }

    public static void Update(int id, string newDate, int newQuantity)
    {
        SqlNonQuery($"UPDATE heart_points SET date = '{newDate}', quantity = {newQuantity} WHERE id = {id};");
    }

    public static void Delete(int id)
    {

        SqlNonQuery($"DELETE FROM heart_points WHERE id = {id}");
    }

    public static int GetTotalDays()
    {
        return SqlScalarQuery($"SELECT COUNT() FROM (SELECT DISTINCT date FROM heart_points);");
    }

    public static int GetTotalPoints(int year = -1)
    {
        //TODO implement get sum of all points
        return -1;
    }
}
