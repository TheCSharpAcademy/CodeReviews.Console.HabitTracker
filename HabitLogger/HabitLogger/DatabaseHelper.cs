using Microsoft.Data.Sqlite;


public static class DatabaseHelper
{
    private const string DatabaseName = "habit_tracker.db";

    public static void InitializeDatabase()
    {
        try
        {
            if (!File.Exists(DatabaseName))
            {
                CreateDatabaseAndTables();
            }
            else
            {
                using (var connection = new SqliteConnection($"Data Source={DatabaseName}"))
                {
                    connection.Open();
                    string tableCheckQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Habits'";
                    var checkTableCommand = new SqliteCommand(tableCheckQuery, connection);
                    var result = checkTableCommand.ExecuteScalar();

                    if (result == null)
                    {
                        CreateDatabaseAndTables();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
        }
    }

    private static void CreateDatabaseAndTables()
    {
        using (var connection = new SqliteConnection($"Data Source={DatabaseName}"))
        {
            connection.Open();

            string createHabitsTable = @"CREATE TABLE IF NOT EXISTS Habits (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Name TEXT NOT NULL,
                                        Unit TEXT NOT NULL
                                    )";

            var createHabitsCommand = new SqliteCommand(createHabitsTable, connection);
            createHabitsCommand.ExecuteNonQuery();

            string createHabitLogTable = @"CREATE TABLE IF NOT EXISTS HabitLog (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            HabitId INTEGER NOT NULL,
                                            Date TEXT NOT NULL,
                                            Quantity INTEGER NOT NULL,
                                            FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                                        )";

            var createHabitLogCommand = new SqliteCommand(createHabitLogTable, connection);
            createHabitLogCommand.ExecuteNonQuery();

            SeedData(connection); // Seed data after creating tables
        }
    }

    private static void SeedData(SqliteConnection connection)
    {
        var insertHabitsCommand = new SqliteCommand();
        insertHabitsCommand.Connection = connection;
        insertHabitsCommand.CommandText = @"
            INSERT INTO Habits (Name, Unit) VALUES 
            ('Drinking Water', 'Glasses'), 
            ('Walking', 'Steps'), 
            ('Reading', 'Pages');";
        insertHabitsCommand.ExecuteNonQuery();

        var random = new Random();
        var habitIds = new[] { 1, 2, 3 };
        for (int i = 0; i < 100; i++)
        {
            var habitId = habitIds[random.Next(habitIds.Length)];
            var date = DateTime.Now.AddDays(-random.Next(30)).ToString("yyyy-MM-dd"); // Correct date format
            var quantity = random.Next(1, 101);

            var insertLogCommand = new SqliteCommand();
            insertLogCommand.Connection = connection;
            insertLogCommand.CommandText = "INSERT INTO HabitLog (HabitId, Date, Quantity) VALUES (@habitId, @date, @quantity)";
            insertLogCommand.Parameters.AddWithValue("@habitId", habitId);
            insertLogCommand.Parameters.AddWithValue("@date", date);
            insertLogCommand.Parameters.AddWithValue("@quantity", quantity);

            insertLogCommand.ExecuteNonQuery();
        }
    }
}
