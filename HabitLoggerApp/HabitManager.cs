using System.Data.SQLite;

namespace HabitLoggerApp;

public class HabitManager
{
    private const string ConnectionString = "Data Source=habits.db";

    public HabitManager()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS habit (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Body TEXT,
                Quantity INTEGER)
            """;
        command.ExecuteNonQuery();

        connection.Close();
    }

    public void CreateHabit(string date, string body, int quantity)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        var insertQuery =
            """
            INSERT INTO habit (Date, Body, Quantity) 
                VALUES (@date, @body, @quantity)
            """;
        var command = connection.CreateCommand();
        command.CommandText = insertQuery;
        command.Parameters.Add(new SQLiteParameter("@date", date));
        command.Parameters.Add(new SQLiteParameter("@body", body));
        command.Parameters.Add(new SQLiteParameter("@quantity", quantity));

        try
        {
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public bool UpdateHabit(int id, string body, string date, int quantity)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        var updateQuery =
            """
            UPDATE habit
            SET Body = @body, Date = @date, Quantity = @quantity
            WHERE Id = @id
            """;
        
        var command = connection.CreateCommand();
        command.CommandText = updateQuery;
        command.Parameters.Add(new SQLiteParameter("@id", id));
        command.Parameters.Add(new SQLiteParameter("@body", body));
        command.Parameters.Add(new SQLiteParameter("@date", date));
        command.Parameters.Add(new SQLiteParameter("@quantity", quantity));

        var updatedCount = 0;
        try
        {
            updatedCount = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return updatedCount > 0;
    }

    public bool DeleteHabit(int id)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        var deleteQuery =
            """
            DELETE FROM habit
            WHERE Id = @Id;
            """;

        var command = connection.CreateCommand();
        command.CommandText = deleteQuery;
        command.Parameters.Add(new SQLiteParameter("@Id", id));

        var rowCount = 0;
        try
        {
            rowCount = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return rowCount > 0;
    }

    public List<Habit> GetAllHabits()
    {
        var habits = new List<Habit>();
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Date, Body, Quantity FROM habit
            """;

        try
        {
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var date = reader.GetString(1);
                    var body = reader.GetString(2);
                    var quantity = reader.GetInt32(3);

                    habits.Add(new Habit(id, date, body, quantity));
                }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return habits;
    }
}
