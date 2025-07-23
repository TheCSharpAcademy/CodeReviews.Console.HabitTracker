using Microsoft.Data.Sqlite;

namespace HabitTracker.TruthfulUK.Helpers;
public class DB_Helpers
{
    public static SqliteConnection GetOpenConnection() { 
        var connection = new SqliteConnection("Data Source=HabitsTrackerLocalDB.db");
        connection.Open();

        return connection;
    }
    public static void InitializeDatabase()
    {
        using var connection = GetOpenConnection();

        using var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = @"
        CREATE TABLE IF NOT EXISTS Habits (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Measurement TEXT NOT NULL,
            CreatedAt DATE NOT NULL
        );
        CREATE TABLE IF NOT EXISTS HabitLogs (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            HabitId INTEGER NOT NULL,
            Amount DOUBLE,
            OccurrenceDate DATE NOT NULL,
            FOREIGN KEY (HabitId) REFERENCES Habits(Id)
        );
        ";
        createTableCommand.ExecuteNonQuery();
    }

    public static List<string> SelectHabits()
    {
        using var connection = GetOpenConnection();
        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = "SELECT * FROM Habits ORDER BY CreatedAt DESC";
        using var reader = selectCommand.ExecuteReader();

        List<string> habits = new List<string>();
        while (reader.Read())
        {
            if (reader["Name"] is string habitName)
            {
                habits.Add(habitName);
            }
        }
        return habits;
    }

    public static string SelectMeasurement(string habitName)
    {
        using var connection = GetOpenConnection();
        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = @"
            SELECT Measurement 
            FROM Habits 
            WHERE Name = @habitName";
        selectCommand.Parameters.AddWithValue("@habitName", habitName);
        
        using var reader = selectCommand.ExecuteReader();
        if (reader.Read())
        {
            return reader["Measurement"] as string ?? string.Empty;
        }
        return string.Empty;
    }

    public static void AddHabitLog(string habitName, double amount, DateOnly occurrenceDate)
    {
        if (occurrenceDate == default)
        {
            occurrenceDate = DateOnly.FromDateTime(DateTime.Now);
        }
        
        using var connection = GetOpenConnection();
        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = @"
            INSERT INTO HabitLogs (HabitId, Amount, OccurrenceDate)
            VALUES (
                (SELECT Id FROM Habits WHERE Name = @habitName),
                @amount,
                @occurrenceDate
            )";
        insertCommand.Parameters.AddWithValue("@habitName", habitName);
        insertCommand.Parameters.AddWithValue("@amount", amount);
        insertCommand.Parameters.AddWithValue("@occurrenceDate", occurrenceDate.ToString("yyyy-MM-dd"));
        
        insertCommand.ExecuteNonQuery();
    }

    public static List<(int, string, double)> ViewHabitLogs(string habitName)
    {
        using var connection = GetOpenConnection();
        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = @"
            SELECT hl.id, hl.OccurrenceDate, hl.Amount
            FROM HabitLogs hl
            JOIN Habits h ON hl.HabitId = h.Id
            WHERE h.Name = @habitName
            ORDER BY hl.OccurrenceDate DESC";
        selectCommand.Parameters.AddWithValue("@habitName", habitName);
        using var reader = selectCommand.ExecuteReader();

        var habitLogs = new List<(int id, string date, double amount)>();

        while (reader.Read())
        {
            habitLogs.Add((reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2)));
        }

        return habitLogs;
    }

    public static void DeleteHabitLog(int rowId)
    {
        using var connection = GetOpenConnection();
        using var deleteCommand = connection.CreateCommand();
        deleteCommand.CommandText = "DELETE FROM HabitLogs WHERE Id = @rowId";
        deleteCommand.Parameters.AddWithValue("@rowId", rowId);
        deleteCommand.ExecuteNonQuery();
    }

    public static void SeedDatabase() {         
        using var connection = GetOpenConnection();
        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = @"
        -- HABITS (static)
        INSERT INTO Habits (Name, Measurement, CreatedAt) VALUES
        ('Water Intake', 'ml', '2025-05-03'),
        ('Push-ups', 'reps', '2025-05-01'),    
        ('Walking', 'steps', '2025-05-05'),
        ('Running', 'km', '2025-05-06'),
        ('Cycling', 'km', '2025-05-07');

        -- HABIT Logs (randomized)
        INSERT INTO HabitLogs (HabitId, Amount, OccurrenceDate) VALUES
        (1, 2000, '2025-05-03'),
        (2, 30, '2025-05-04'),
        (3, 8000, '2025-05-06'),
        (4, 20, '2025-05-05'),
        (1, 1800, '2025-05-07'),
        (2, 50, '2025-05-08'),
        (3, 10000, '2025-05-09'),
        (4, 15, '2025-05-10'),
        (1, 2500, '2025-05-12'),
        (2, 35, '2025-05-13'),
        (3, 9000, '2025-05-14'),
        (4, 25, '2025-05-15'),
        (1, 1900, '2025-05-17'),
        (2, 40, '2025-05-18'),
        (3, 12000, '2025-05-19'),
        (4, 18, '2025-05-20'),
        (1, 2200, '2025-05-22'),
        (2, 55, '2025-05-23'),
        (3, 9500, '2025-05-24'),
        (4, 30, '2025-05-25'),
        (1, 2100, '2025-06-01'),
        (2, 45, '2025-06-02'),
        (3, 11000, '2025-06-03'),
        (4, 22, '2025-06-04'),
        (1, 2300, '2025-06-06'),
        (2, 60, '2025-06-07'),
        (3, 8700, '2025-06-08'),
        (4, 28, '2025-06-09'),
        (1, 2400, '2025-06-11'),
        (2, 38, '2025-06-12'),
        (3, 10500, '2025-06-13'),
        (4, 20, '2025-06-14'),
        (1, 2000, '2025-06-16'),
        (5, 5.4, '2025-06-17'),
        (5, 2.1, '2025-06-17'),
        (5, 3.33, '2025-06-17'),
        (5, 2.44, '2025-06-17'),
        (5, 4, '2025-07-19');
        ";
        insertCommand.ExecuteNonQuery();
    }
}
