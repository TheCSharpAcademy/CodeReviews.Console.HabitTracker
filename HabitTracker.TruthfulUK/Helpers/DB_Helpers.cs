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
        INSERT INTO Habits (Name, Measurement, CreatedAt) VALUES 
        ('Water Intake', 'ml', '2025-05-03'),
        ('Push-ups', 'reps', '2025-05-01'),
        ('Walking', 'steps', '2025-05-05');
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
        occurrenceDate = CheckEmptyDate(occurrenceDate);

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

    public static void UpdateHabitLog(int rowId, double newAmount, DateOnly newDate)
    {
        newDate = CheckEmptyDate(newDate);

        using var connection = GetOpenConnection();
        using var updateCommand = connection.CreateCommand();
        updateCommand.CommandText = @"
            UPDATE HabitLogs 
            SET Amount = @newAmount, OccurrenceDate = @newDate 
            WHERE Id = @rowId";
        updateCommand.Parameters.AddWithValue("@newAmount", newAmount);
        updateCommand.Parameters.AddWithValue("@newDate", newDate.ToString("yyyy-MM-dd"));
        updateCommand.Parameters.AddWithValue("@rowId", rowId);
        
        updateCommand.ExecuteNonQuery();
    }

    public static void AddHabit(string habitName, string measurement)
    {
        using var connection = GetOpenConnection();
        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = @"
            INSERT INTO Habits (Name, Measurement, CreatedAt) 
            VALUES (@habitName, @measurement, @createdAt)";
        insertCommand.Parameters.AddWithValue("@habitName", habitName);
        insertCommand.Parameters.AddWithValue("@measurement", measurement);
        insertCommand.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd"));
        
        insertCommand.ExecuteNonQuery();
    }

    public static List<(string, double, string)> HabitDayReport(DateOnly date)
    {
        date = CheckEmptyDate(date);

        using var connection = GetOpenConnection();
        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = @"
            SELECT h.Name, hl.Amount, h.Measurement 
            FROM HabitLogs hl
            JOIN Habits h ON hl.HabitId = h.Id
            WHERE hl.OccurrenceDate = @date
            ORDER BY h.Name";
        selectCommand.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
        
        using var reader = selectCommand.ExecuteReader();

        var dayReport = new List<(string name, double amount, string measurement)>();

        while (reader.Read())
        {
            dayReport.Add((reader.GetString(0), reader.GetDouble(1), reader.GetString(2)));
        }

        return dayReport;
    }

    public static List<(string, double, string)> TotalLoggedReport()
    {
        using var connection = GetOpenConnection();
        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = @"
            SELECT h.Name, SUM(hl.Amount) AS TotalAmount, h.Measurement 
            FROM HabitLogs hl
            JOIN Habits h ON hl.HabitId = h.Id
            GROUP BY h.Name, h.Measurement
            ORDER BY h.Name";
        
        using var reader = selectCommand.ExecuteReader();
        var totalLoggedReport = new List<(string name, double amount, string measurement)>();
        while (reader.Read())
        {
            totalLoggedReport.Add((reader.GetString(0), reader.GetDouble(1), reader.GetString(2)));
        }
        return totalLoggedReport;
    }

    public static DateOnly CheckEmptyDate(DateOnly date)
    {
        return date == default ? DateOnly.FromDateTime(DateTime.Now) : date;
    }

    public static void SeedDatabase() {         
        using var connection = GetOpenConnection();
        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = @"
        -- HABITS (static)
        INSERT INTO Habits (Name, Measurement, CreatedAt) VALUES
        ('Running', 'km', '2025-05-06'),
        ('Cycling', 'km', '2025-05-07');

        -- HABIT Logs (randomized)
        INSERT INTO HabitLogs (HabitId, Amount, OccurrenceDate) VALUES
        (1, 1800, '2025-05-06'),
        (1, 1500, '2025-05-03'),
        (2, 20, '2025-05-04'),
        (3, 5000, '2025-05-04'),
        (4, 2.5, '2025-05-05'),
        (1, 1800, '2025-05-06'),
        (3, 6000, '2025-05-06'),
        (2, 15, '2025-05-07'),
        (1, 2000, '2025-05-07'),
        (1, 2500, '2025-05-08'),
        (3, 8520, '2025-05-08'),
        (2, 25, '2025-05-09'),
        (4, 5.5, '2025-05-10'),
        (1, 3000, '2025-05-10'),
        (5, 15.2, '2025-05-11'),
        (1, 2000, '2025-05-12'),
        (3, 12034, '2025-05-12'),
        (2, 30, '2025-05-12'),
        (3, 7500, '2025-05-13'),
        (1, 2200, '2025-05-14'),
        (4, 3.1, '2025-05-15'),
        (1, 2800, '2025-05-15'),
        (2, 20, '2025-05-16'),
        (3, 9800, '2025-05-17'),
        (1, 2600, '2025-05-17'),
        (5, 22.5, '2025-05-18'),
        (1, 1800, '2025-05-19'),
        (3, 11000, '2025-05-19'),
        (2, 35, '2025-05-20'),
        (1, 2400, '2025-05-20'),
        (4, 6.2, '2025-05-21'),
        (3, 5200, '2025-05-22'),
        (1, 2900, '2025-05-22'),
        (2, 15, '2025-05-23'),
        (5, 18.0, '2025-05-24'),
        (1, 2100, '2025-05-24'),
        (3, 13000, '2025-05-25'),
        (1, 2700, '2025-05-25'),
        (2, 40, '2025-05-25'),
        (1, 2300, '2025-06-01'),
        (3, 9200, '2025-06-01'),
        (2, 22, '2025-06-02'),
        (4, 4.0, '2025-06-03'),
        (1, 3100, '2025-06-03'),
        (5, 12.0, '2025-06-04'),
        (1, 1900, '2025-06-05'),
        (3, 14500, '2025-06-05'),
        (2, 28, '2025-06-06'),
        (3, 6800, '2025-06-07'),
        (1, 2500, '2025-06-07'),
        (4, 7.5, '2025-06-08'),
        (2, 18, '2025-06-09'),
        (1, 2200, '2025-06-09'),
        (3, 10500, '2025-06-10'),
        (5, 25.0, '2025-06-11'),
        (1, 2000, '2025-06-11'),
        (3, 11200, '2025-06-12'),
        (2, 32, '2025-06-12'),
        (1, 2600, '2025-06-13'),
        (4, 2.5, '2025-06-14'),
        (3, 8800, '2025-06-14'),
        (1, 2900, '2025-06-15'),
        (2, 25, '2025-06-16'),
        (5, 30.0, '2025-06-17'),
        (1, 2400, '2025-06-17'),
        (3, 9500, '2025-06-18'),
        (1, 2100, '2025-06-18'),
        (4, 8.0, '2025-06-19'),
        (2, 20, '2025-06-19'),
        (1, 2700, '2025-06-20'),
        (3, 12500, '2025-06-21'),
        (5, 16.5, '2025-06-22'),
        (1, 2300, '2025-06-22'),
        (2, 38, '2025-06-23'),
        (3, 7200, '2025-06-24'),
        (1, 3000, '2025-06-24'),
        (4, 5.0, '2025-06-25'),
        (1, 2200, '2025-07-01'),
        (3, 10100, '2025-07-01'),
        (2, 45, '2025-07-02'),
        (1, 2800, '2025-07-02'),
        (5, 19.8, '2025-07-03'),
        (4, 9.0, '2025-07-04'),
        (1, 2600, '2025-07-04'),
        (3, 15000, '2025-07-05'),
        (1, 2400, '2025-07-05'),
        (2, 50, '2025-07-05'),
        (3, 8200, '2025-07-06'),
        (1, 2000, '2025-07-07'),
        (4, 3.5, '2025-07-07'),
        (2, 25, '2025-07-08'),
        (5, 21.0, '2025-07-09'),
        (1, 3200, '2025-07-09'),
        (3, 9900, '2025-07-10'),
        (1, 2500, '2025-07-10'),
        (2, 30, '2025-07-11'),
        (4, 6.8, '2025-07-12'),
        (3, 11500, '2025-07-12'),
        (1, 2700, '2025-07-13'),
        (5, 28.2, '2025-07-14'),
        (2, 33, '2025-07-14'),
        (3, 10800, '2025-07-15'),
        (1, 2300, '2025-07-15'),
        (4, 4.2, '2025-07-16'),
        (1, 2900, '2025-07-16'),
        (2, 28, '2025-07-17'),
        (3, 9300, '2025-07-17'),
        (5, 14.0, '2025-07-18'),
        (1, 2100, '2025-07-19'),
        (1, 2250, '2025-07-24'),
        (2, 55, '2025-07-24'),
        (3, 5000, '2025-07-24'),
        (5, 2.5, '2025-07-24');
        ";
        insertCommand.ExecuteNonQuery();
    }
}
