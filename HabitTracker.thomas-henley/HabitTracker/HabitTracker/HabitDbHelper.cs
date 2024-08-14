using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker;

public class HabitDbHelper(string tableName = "heart_points")
{
    private static readonly string connectionString = @"Data Source=habit-tracker.db";
    private readonly string tableName = tableName;

    /// <summary>
    /// Build the table.
    /// </summary>
    /// <param name="tableName"></param>
    public void InitializeDB()
    {
        // Create table
        string commandText = $@"CREATE TABLE IF NOT EXISTS {tableName} (
                                   Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                   Date TEXT,
                                   Quantity INTEGER
                               )";
        SqlNonQuery(commandText);
    }

    /// <summary>
    /// Remove the table.
    /// </summary>
    /// <param name="tableName"></param>
    public void TeardownDB()
    {
        // Destroy table
        string commandText = $@"DROP TABLE {tableName}";
        SqlNonQuery(commandText);
    }

    /// <summary>
    /// Fills the database with fake data for demonstration and testing.
    /// </summary>
    public void PopulateDB()
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

    public bool IsDbEmpty()
    {
        return GetAllRecords().Count == 0;
    }

    /// <summary>
    /// Connect to the database, run the query command, disconnect, and return the results.
    /// </summary>
    /// <param name="cmd">The SQL command to execute.</param>
    /// <returns>A List of HeartPoints objects from the database.</returns>
    public static List<HeartPoints> SqlQuery(string cmd)
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
    public static void SqlNonQuery(string cmd)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = cmd;
        tableCmd.ExecuteNonQuery();
        connection.Close();
        return;
    }

    /// <summary>
    /// Connects to the database, runs the command, and disconnects.
    /// Use for SQL commands that return an integet value (COUNT(), etc.).
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    public static int SqlScalarQuery(string cmd)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = cmd;
        var result = tableCmd.ExecuteScalar();
        if (result is DBNull)
        {
            return 0;
        }
        return Convert.ToInt32(result);
    }

    /// <summary>
    /// Create a new record in the database.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="quantity"></param>
    public void Insert(string date, int quantity)
    {
        SqlNonQuery($"INSERT INTO {tableName}(date, quantity) VALUES('{date}', {quantity})");
    }

    /// <summary>
    /// Retrieves all record from the database.
    /// </summary>
    /// <returns>A List of all entries in the {tableName} table.</returns>
    public List<HeartPoints> GetAllRecords()
    {
        return SqlQuery($"SELECT * FROM {tableName}");
    }

    /// <summary>
    /// Attempt to retrieve a record from the database.
    /// </summary>
    /// <param name="id">The ID of the record to search for.</param>
    /// <param name="entry">Out parameter to hold the record if it is found. Null if no matching record.</param>
    /// <returns>True if the record is found, false otherwise.</returns>
    public bool TryGetById(int id, out HeartPoints? entry)
    {
        var results = SqlQuery($"SELECT * FROM {tableName} WHERE id = {id}");

        if (results.Count > 0)
        {
            entry = results[0];
            return true;
        }

        entry = null;
        return false;
    }

    /// <summary>
    /// Update a record in the database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newDate"></param>
    /// <param name="newQuantity"></param>
    public void Update(int id, string newDate, int newQuantity)
    {
        SqlNonQuery($"UPDATE {tableName} SET date = '{newDate}', quantity = {newQuantity} WHERE id = {id};");
    }

    /// <summary>
    /// Remove a record from the database.
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {

        SqlNonQuery($"DELETE FROM {tableName} WHERE id = {id}");
    }

    /// <summary>
    /// Return the total number of days in which cardio activity has been logged.
    /// </summary>
    /// <returns>The integer count of days.</returns>
    public int GetTotalDays()
    {
        return SqlScalarQuery($"SELECT COUNT() FROM (SELECT DISTINCT date FROM {tableName});");
    }

    /// <summary>
    /// Returns the total number of heart points recorded.
    /// </summary>
    /// <param name="year">Optionally filter to an individual year.</param>
    /// <returns></returns>
    public int GetTotalPoints(int year = -1)
    {
        string yearOption = ((year < 0) ? "" : $" WHERE SUBSTR(date, -2) = '{year:00}'");

        return SqlScalarQuery($"SELECT SUM(quantity) FROM {tableName}" + yearOption);
    }
}
