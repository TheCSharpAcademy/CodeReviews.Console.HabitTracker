using Microsoft.Data.Sqlite;

namespace HabitTracker.TruthfulUK;
public class DatabaseUtilities
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
            CreatedAt TEXT NOT NULL DEFAULT (date('now'))
        );
        CREATE TABLE IF NOT EXISTS HabitOccurences (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            HabitId INTEGER NOT NULL,
            Amount INTEGER,
            OccurrenceDate TEXT NOT NULL DEFAULT (date('now')),
            FOREIGN KEY (HabitId) REFERENCES Habits(Id)
        );
        ";
        createTableCommand.ExecuteNonQuery();
    }

    // Debug method to seed the database with initial data
    public static void SeedDatabase() {         
        using var connection = GetOpenConnection();
        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = @"
        -- HABITS (static)
        INSERT INTO Habits (Name, Measurement, CreatedAt) VALUES
        ('Push-ups', 'reps', '2025-05-01'),
        ('Water Intake', 'ml', '2025-05-03'),
        ('Meditation', 'minutes', '2025-05-02'),
        ('Running', 'km', '2025-05-05'),
        ('Reading', 'pages', '2025-05-04'),
        ('Sleep', 'hours', '2025-05-06'),
        ('Study', 'minutes', '2025-05-07'),
        ('Stretching', 'minutes', '2025-05-08'),
        ('Writing', 'words', '2025-05-09'),
        ('Journaling', 'entries', '2025-05-10');

        -- HABIT OCCURRENCES (randomized)
        INSERT INTO HabitOccurences (HabitId, Amount, OccurrenceDate) VALUES
        (2, 2100, '2025-06-01'),
        (4, 6, '2025-06-01'),
        (1, 25, '2025-06-02'),
        (9, 200, '2025-06-02'),
        (6, 7, '2025-06-02'),
        (3, 10, '2025-06-03'),
        (2, 1800, '2025-06-03'),
        (5, 20, '2025-06-03'),
        (7, 45, '2025-06-03'),
        (10, 1, '2025-06-03'),
        (8, 10, '2025-06-04'),
        (4, 5, '2025-06-04'),
        (1, 30, '2025-06-04'),
        (6, 8, '2025-06-04'),
        (9, 300, '2025-06-04'),
        (5, 25, '2025-06-05'),
        (3, 15, '2025-06-05'),
        (7, 60, '2025-06-05'),
        (10, 1, '2025-06-05'),
        (2, 1500, '2025-06-06'),
        (1, 20, '2025-06-06'),
        (6, 6, '2025-06-06'),
        (4, 3, '2025-06-06'),
        (9, 250, '2025-06-07'),
        (3, 12, '2025-06-07'),
        (8, 15, '2025-06-07'),
        (5, 30, '2025-06-07'),
        (1, 35, '2025-06-08'),
        (6, 7, '2025-06-08'),
        (7, 90, '2025-06-08'),
        (10, 1, '2025-06-08'),
        (4, 4, '2025-06-08'),
        (2, 2000, '2025-06-09'),
        (3, 18, '2025-06-09'),
        (9, 220, '2025-06-09'),
        (5, 15, '2025-06-09'),
        (8, 20, '2025-06-09'),
        (7, 120, '2025-06-09'),
        (1, 50, '2025-06-09'),
        (6, 7, '2025-06-09'),
        (4, 6, '2025-06-10'),
        (2, 2300, '2025-06-10'),
        (3, 14, '2025-06-10'),
        (10, 1, '2025-06-10'),
        (9, 275, '2025-06-10');
        ";
        insertCommand.ExecuteNonQuery();
    }
}
