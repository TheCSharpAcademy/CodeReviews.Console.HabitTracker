using Microsoft.Data.Sqlite;

namespace HabitLogger;

public class HabitLoggerDatabase
{
    public SqliteConnection GetConnection()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var dbPath = Path.Combine(basePath, "habits.db");
        var connectionString = $"Data Source={dbPath}";

        var connection = new SqliteConnection(connectionString);

        try
        {
            connection.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return connection;
    }

    public void CreateDatabase()
    {
        using var connection = GetConnection();

        try
        {
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText =
                """
                CREATE TABLE IF NOT EXISTS Habits (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitName TEXT NOT NULL,
                    Unit TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS HabitLogs (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitID INTEGER,
                    Quantity INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    FOREIGN KEY (HabitID) REFERENCES Habits(ID)
                );

                """;
            createTableCommand.ExecuteNonQuery();


            var insertSeedDataCommand = connection.CreateCommand();
            insertSeedDataCommand.CommandText =
                """
                -- Seed data for the Habits
                INSERT INTO Habits (HabitName, Unit) VALUES ('Drinking Water', 'glasses');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Reading', 'pages');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Running', 'meters');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Sketching', 'drawings');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Pull-ups', 'repetitions');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Guitar', 'songs');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Photography', 'pictures');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Yoga', 'sessions');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Dancing', 'moves');
                INSERT INTO Habits (HabitName, Unit) VALUES ('Cooking', 'dishes');

                -- Seed data for "Drinking Water"
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (1, 8, '2024-01-29');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (1, 7, '2024-01-30');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (1, 9, '2024-01-31');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (1, 8, '2024-02-01');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (1, 7, '2024-02-02');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (1, 8, '2024-02-03');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (1, 9, '2024-02-04');

                -- Seed data for "Reading"
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (2, 30, '2024-01-29');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (2, 25, '2024-01-30');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (2, 35, '2024-01-31');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (2, 40, '2024-02-01');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (2, 20, '2024-02-02');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (2, 45, '2024-02-03');
                INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES (2, 50, '2024-02-04');

                """;
            insertSeedDataCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}