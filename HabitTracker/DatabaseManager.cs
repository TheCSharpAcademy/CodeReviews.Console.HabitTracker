using System.Globalization;
using Microsoft.Data.Sqlite;
using Spectre.Console;

// For random generation

namespace HabitTracker;

public class DatabaseManager
{
    private readonly string _connectionString;
    internal const string DatabaseFileName = "habit_logger.db";
    private const string DateFormat = "yyyy-MM-dd"; // Centralized date format

    public DatabaseManager()
    {
        _connectionString = $"Data Source={DatabaseFileName}";
        InitializeDatabaseAndSeedData(); // Combined initialization and seeding check
    }

    // Ensures database and tables exist, and seeds if necessary
    private void InitializeDatabaseAndSeedData()
    {
        var dbJustCreated = !File.Exists(DatabaseFileName); // Check if file exists *before* connection tries to create it

        try
        {
            using var connection = GetOpenConnection();
            // Create Habits Table
            using (var command = connection.CreateCommand())
            {
                command.CommandText = """
                                      
                                                              CREATE TABLE IF NOT EXISTS Habits (
                                                                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                  Name TEXT NOT NULL UNIQUE,
                                                                  UnitOfMeasurement TEXT NOT NULL
                                                              );
                                      """;
                command.ExecuteNonQuery();
            }

            // Create HabitLog Table with Foreign Key
            using (var command = connection.CreateCommand())
            {
                command.CommandText = """
                                      
                                                              CREATE TABLE IF NOT EXISTS HabitLog (
                                                                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                  Date TEXT NOT NULL,
                                                                  Quantity INTEGER NOT NULL,
                                                                  HabitId INTEGER NOT NULL,
                                                                  FOREIGN KEY (HabitId) REFERENCES Habits(Id) ON DELETE CASCADE -- Cascade delete logs if habit is deleted
                                                              );
                                      """;
                command.ExecuteNonQuery();
            }

            // Check if seeding is needed (only if DB was just created OR Habits table is empty)
            bool needsSeeding;
            if (dbJustCreated) {
                needsSeeding = true;
            } else
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Habits";
                var count = (long?)command.ExecuteScalar(); // Can return null or long
                needsSeeding = (count == 0);
            }

            if (!needsSeeding) return;
            Console.WriteLine("Database is new or empty. Seeding initial data...");
            SeedDatabase(connection); // Pass the open connection
            Console.WriteLine("Seeding complete.");
        }
        catch (Exception ex) // Catch broader exceptions during init/seed
        {
            Console.WriteLine($"Database initialization or seeding error: {ex.Message}");
            // Consider if the app should exit if initialization fails critically
        }
    }

    // Seeds initial habits and random log data
        // Seeds initial habits and random log data
    private static void SeedDatabase(SqliteConnection connection) // Reuse open connection
    {
        // Use a transaction for efficiency when inserting multiple rows
        using var transaction = connection.BeginTransaction();
        try
        {
            var habitsToSeed = new List<(string Name, string Unit)>
            {
                ("Water Intake", "glasses"),
                ("Running", "km"),
                ("Coding", "hours"),
                ("Reading", "pages")
            };

            var habitIds = new List<long>(); // Store generated IDs

            // Seed Habits
            // Need separate commands: one for INSERT, one for getting the ID
            using (var insertCommand = connection.CreateCommand())
            using (var selectIdCommand = connection.CreateCommand()) // Command to get last ID
            {
                insertCommand.Transaction = transaction; // Associate commands with the transaction
                selectIdCommand.Transaction = transaction;

                insertCommand.CommandText = "INSERT INTO Habits (Name, UnitOfMeasurement) VALUES (@Name, @Unit);";
                // The SQLite function to get the last inserted ROWID
                selectIdCommand.CommandText = "SELECT last_insert_rowid();";

                foreach (var (name, unit) in habitsToSeed)
                {
                    insertCommand.Parameters.Clear(); // Clear parameters before reuse
                    insertCommand.Parameters.AddWithValue("@Name", name);
                    insertCommand.Parameters.AddWithValue("@Unit", unit);
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // ExecuteScalar returns object?, which needs to be cast to long
                        // This MUST be executed immediately after the INSERT on the same connection/transaction
                        var lastIdResult = selectIdCommand.ExecuteScalar();
                        if (lastIdResult != null && lastIdResult != DBNull.Value)
                        {
                            var lastId = Convert.ToInt64(lastIdResult); // Safely convert to long
                            habitIds.Add(lastId);
                        }
                        else
                        {
                            // This shouldn't happen if insert succeeded, but good to handle
                            AnsiConsole.MarkupLine($"[red]Warning: Could not retrieve last insert ID after inserting habit '{name}'.[/]");
                            // Decide how critical this is. Maybe throw an exception or just log it.
                        }
                    }
                    else
                    {
                        // Use AnsiConsole for consistency if Program.cs uses it
                        AnsiConsole.MarkupLine($"[yellow]Warning: Failed to insert habit '{name}'. Skipping seeding logs for this potential habit ID.[/]");
                    }
                }
            } // End of using insertCommand and selectIdCommand

            if (habitIds.Count == 0) {
                AnsiConsole.MarkupLine("[red]Warning: No habits were successfully seeded, cannot seed logs.[/]");
                transaction.Rollback(); // Rollback if no habits created
                return;
            }

            // Seed HabitLog with random data (using the collected habitIds)
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO HabitLog (Date, Quantity, HabitId) VALUES (@Date, @Quantity, @HabitId);";
                var random = new Random();
                var today = DateTime.Today;

                for (var i = 0; i < 100; i++)
                {
                    command.Parameters.Clear();
                    var randomHabitIndex = random.Next(habitIds.Count);
                    var randomHabitId = habitIds[randomHabitIndex];
                    var randomDate = today.AddDays(-random.Next(0, 366)); // Logs within the last year
                    var randomQuantity = random.Next(1, 21); // Quantity between 1 and 20

                    command.Parameters.AddWithValue("@Date", randomDate.ToString(DateFormat));
                    command.Parameters.AddWithValue("@Quantity", randomQuantity);
                    command.Parameters.AddWithValue("@HabitId", randomHabitId);
                    command.ExecuteNonQuery();
                }
            }

            transaction.Commit(); // Commit all changes if successful
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error during seeding: {ex.Message}[/]");
            try
            {
                transaction.Rollback(); // Rollback on error
            }
            catch (Exception rbEx)
            {
                AnsiConsole.MarkupLine($"[red]Exception during transaction rollback: {rbEx.Message}[/]");
            }
            // Handle or rethrow as needed
        }
    }

    // Helper method to create and open a connection (DRY) - unchanged
    private SqliteConnection GetOpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error opening database connection: {ex.Message}");
            connection.Dispose();
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred opening the connection: {ex.Message}");
            connection.Dispose();
            throw;
        }
    }


    // --- Habit Management ---

    public bool InsertHabit(string name, string unit)
    {
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Habits (Name, UnitOfMeasurement) VALUES (@Name, @Unit)";
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Unit", unit);

            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        catch (SqliteException ex)
        {
            // Check for unique constraint violation
            if (ex.SqliteErrorCode == 19 && ex.Message.Contains("UNIQUE constraint failed: Habits.Name")) {
                Console.WriteLine($"Error: Habit with name '{name}' already exists.");
            } else {
                Console.WriteLine($"Error inserting habit: {ex.Message}");
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred inserting habit: {ex.Message}");
            return false;
        }
    }

    public List<Habit> GetAllHabits()
    {
        var habits = new List<Habit>();
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, UnitOfMeasurement FROM Habits ORDER BY Name";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                habits.Add(new Habit
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    UnitOfMeasurement = reader.GetString(2)
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving habits: {ex.Message}");
        }
        return habits;
    }

    // Optional: Get a single habit by ID (useful for validation/reporting)
    public Habit? GetHabitById(int habitId)
    {
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, UnitOfMeasurement FROM Habits WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", habitId);

            using var reader = command.ExecuteReader();
            if (reader.Read()) // If a record was found
            {
                return new Habit
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    UnitOfMeasurement = reader.GetString(2)
                };
            }

            return null; // Habit not found
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving habit by ID: {ex.Message}");
            return null; // Return null on error
        }
    }


    // --- Habit Log Management (Modified for HabitId) ---

    // Inserts a new habit log record for a specific habit
    public bool InsertRecord(int habitId, string date, int quantity)
    {
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO HabitLog (HabitId, Date, Quantity) VALUES (@HabitId, @Date, @Quantity)";
            command.Parameters.AddWithValue("@HabitId", habitId);
            command.Parameters.AddWithValue("@Date", date); // Assuming date is already validated string
            command.Parameters.AddWithValue("@Quantity", quantity);

            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        catch (SqliteException ex)
        {
            // Check for foreign key constraint violation
            if (ex.SqliteErrorCode == 19 && ex.Message.Contains("FOREIGN KEY constraint failed")) {
                Console.WriteLine($"Error: Habit with ID {habitId} does not exist. Cannot insert log.");
            } else {
                Console.WriteLine($"Error inserting record: {ex.Message}");
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during insertion: {ex.Message}");
            return false;
        }
    }

    // Retrieves all habit log records, joining with Habits table
    public List<HabitRecord> GetAllRecords()
    {
        var records = new List<HabitRecord>();
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            // JOIN to get Habit Name and Unit directly
            command.CommandText = """
                                  
                                                      SELECT l.Id, l.Date, l.Quantity, l.HabitId, h.Name, h.UnitOfMeasurement
                                                      FROM HabitLog l
                                                      JOIN Habits h ON l.HabitId = h.Id
                                                      ORDER BY h.Name, l.Date DESC
                                  """; // Order by habit then date

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new HabitRecord
                {
                    Id = reader.GetInt32(0),
                    // Use ParseExact for reliable date parsing
                    Date = DateTime.ParseExact(reader.GetString(1), DateFormat, CultureInfo.InvariantCulture),
                    Quantity = reader.GetInt32(2),
                    HabitId = reader.GetInt32(3),
                    HabitName = reader.GetString(4),
                    UnitOfMeasurement = reader.GetString(5)
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving records: {ex.Message}");
        }
        return records;
    }

    // Deletes a specific habit log record by its ID
    public bool DeleteRecord(int logId) // Renamed parameter for clarity
    {
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM HabitLog WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", logId);

            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting record: {ex.Message}");
            return false;
        }
    }

    // Updates a specific habit log record (Date and Quantity only)
    public bool UpdateRecord(int logId, string newDate, int newQuantity)
    {
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            // We are not allowing changing the HabitId of an existing log easily here
            // If needed, it would require adding HabitId parameter and updating the SET clause
            command.CommandText = "UPDATE HabitLog SET Date = @Date, Quantity = @Quantity WHERE Id = @Id";
            command.Parameters.AddWithValue("@Date", newDate);
            command.Parameters.AddWithValue("@Quantity", newQuantity);
            command.Parameters.AddWithValue("@Id", logId);

            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating record: {ex.Message}");
            return false;
        }
    }

    // Checks if a specific *log* record exists
    public bool LogRecordExists(int logId)
    {
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1 FROM HabitLog WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", logId);

            using var reader = command.ExecuteReader();
            return reader.HasRows;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking if log record exists: {ex.Message}");
            return false;
        }
    }

    // --- Reporting ---

    // Gets total quantity and count for a specific habit in a given year
    public (int TotalQuantity, int RecordCount)? GetYearlyHabitSummary(int habitId, int year)
    {
        try
        {
            using var connection = GetOpenConnection();
            using var command = connection.CreateCommand();
            // Use SQLite's strftime function to extract the year
            command.CommandText = @"
                    SELECT SUM(Quantity), COUNT(*)
                    FROM HabitLog
                    WHERE HabitId = @HabitId AND strftime('%Y', Date) = @Year";

            command.Parameters.AddWithValue("@HabitId", habitId);
            // strftime expects year as a string
            command.Parameters.AddWithValue("@Year", year.ToString());

            using var reader = command.ExecuteReader();
            if (reader.Read() && !reader.IsDBNull(0)) // Check if data exists and SUM is not NULL
            {
                // SUM returns an integer type if all inputs are integers in SQLite
                // COUNT always returns an integer
                // Need to handle potential type differences depending on SQLite version/data
                // Using Convert.ToInt32 is generally safe here.
                var totalQuantity = Convert.ToInt32(reader.GetValue(0));
                var recordCount = Convert.ToInt32(reader.GetValue(1));
                return (totalQuantity, recordCount);
            }

            return null; // No records found for that habit and year
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating yearly report: {ex.Message}");
            return null; // Return null on error
        }
    }
}