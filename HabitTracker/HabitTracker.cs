using System;
using Microsoft.Data.Sqlite;

namespace HabitTracker.SmithboyJr
{
    public class HabitTracker
    {
        private static readonly string connectionString = @"Data Source=HabitTracker.db";

        // SQL Query Constants
        private const string InsertQuery = "INSERT INTO drinking_water (Date, Quantity) VALUES (@Date, @Quantity)";
        private const string DeleteQuery = "DELETE FROM drinking_water WHERE Id = @Id";
        private const string UpdateQuery = "UPDATE drinking_water SET Date = @Date, Quantity = @Quantity WHERE Id = @Id";
        private const string SelectAllQuery = "SELECT * FROM drinking_water";

        // Success/Error Messages Constants
        private const string RecordNotFoundMessage = "Error: No record found with the specified Id.";
        private const string RecordInsertedMessage = "Success: Record inserted successfully.";
        private const string RecordUpdatedMessage = "Success: Record updated successfully.";
        private const string RecordDeletedMessage = "Success: Record deleted successfully.";
        private const string RecordDisplayedMessage = "Success: Record displayed successfully.";
        private const string DatabaseErrorMessage = "An error occurred while accessing the database. Please try again later.";
        private const string UnexpectedErrorMessage = "An unexpected error occurred. Please contact support.";
        private const string NoRecordsFoundMessage = "No records found.";

        static void Main(string[] args)
        {
            Log("Application starting...");
            try
            {
                InitializeDatabase(); // Initialize the database (create table if it doesn't exist)
                MainMenu();           // Start the main menu
            }
            catch (SqliteException ex)
            {
                Log($"Database error during application startup. Error: {ex.Message}");
                Console.WriteLine(DatabaseErrorMessage);
            }
            catch (Exception ex)
            {
                Log($"Unexpected error while processing user request. Error: {ex.Message}");
                Console.WriteLine(UnexpectedErrorMessage);
            }
            finally
            {
                Log("Application shutting down.");
                Console.WriteLine("Thank you for using Habit Tracker. Goodbye!");
            }
        }

        // This method initializes the database by creating the drinking_water table if it doesn't exist
        private static void InitializeDatabase()
        {
            Log("Initializing database...");
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                Date TEXT, 
                Quantity INTEGER)";
                    tableCmd.ExecuteNonQuery();
                    Log("Database initialized successfully.");
                }
            }
            catch (Exception ex)
            {
                Log($"[Error] Failed to initialize database. Error: {ex.Message}");
                throw; // Re-throw the exception to stop execution if the database cannot be initialized
            }
        }

        private static void ExecuteNonQuery(string commandText, Action<SqliteCommand> configureCommand, Action<int>? handleRowsAffected = null)
        {
            SqliteCommand? command = null; // Declare command outside the try block
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    command = connection.CreateCommand(); // Initialize command
                    command.CommandText = commandText;

                    // Configure parameters securely
                    configureCommand(command);

                    // Execute the command and get the number of rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    // Optional callback to handle rows affected
                    handleRowsAffected?.Invoke(rowsAffected);
                }
            }
            catch (SqliteException ex)
            {
                string parameters = command != null ? GetCommandParameters(command) : "No parameters";
                Log($"[Database Error] Query: {commandText} | Parameters: {parameters} | Error: {ex.Message}");
                Console.WriteLine(DatabaseErrorMessage);
            }
            catch (Exception ex)
            {
                Log($"[Unexpected Error] Query: {commandText} | Error: {ex.Message}");
                Console.WriteLine(UnexpectedErrorMessage);
            }
        }

        private static void ExecuteReader(string commandText, Action<SqliteDataReader> processRows)
        {
            SqliteCommand? command = null; // Declare command outside the try block
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    command = connection.CreateCommand(); // Initialize command
                    command.CommandText = commandText;

                    Log($"Executing query: {commandText}");
                    using (var reader = command.ExecuteReader())
                    {
                        Log("Query executed successfully. Processing rows...");
                        processRows(reader); // Process the rows using the provided callback
                    }
                }
            }
            catch (SqliteException ex)
            {
                string parameters = command != null ? GetCommandParameters(command) : "No parameters";
                Log($"[Database Error] Query: {commandText} | Parameters: {parameters} | Error: {ex.Message}");
                Console.WriteLine(DatabaseErrorMessage);
            }
            catch (Exception ex)
            {
                Log($"[Unexpected Error] Query: {commandText} | Error: {ex.Message}");
                Console.WriteLine(UnexpectedErrorMessage);
            }
        }

        private static string GetValidatedInput(string prompt, Func<string, bool> validation, string errorMessage)
        {
            string input;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine() ?? string.Empty;
                if (!validation(input))
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!validation(input));

            return input;
        }

        private static int GetValidatedInt(string prompt)
        {
            while (true)
            {
                try
                {
                    return int.Parse(GetValidatedInput(
                        prompt,
                        input => int.TryParse(input, out _),
                        "Invalid input. Please enter a valid integer."
                    ));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("The number is too large. Please enter a smaller integer.");
                }
            }
        }

        private static string GetValidatedDate(string prompt)
        {
            return GetValidatedInput(
                prompt,
                input => DateTime.TryParseExact(input, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _),
                "Invalid input. Please enter a valid date in the format DD-MM-YYYY."
            );
        }

        private static void Log(string message)
        {
            Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
        }

        private static string GetCommandParameters(SqliteCommand command)
        {
            return string.Join(", ", command.Parameters.Cast<SqliteParameter>().Select(p => $"{p.ParameterName}={p.Value}"));
        }

        static void MainMenu()
        {
            string? readResult;
            string menuSelection = "";

            do
            {
                Console.Clear(); // Clear the console at the start of the loop
                // Where the text menu is located
                DisplayMenu();

                readResult = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(readResult) && "12345".Contains(readResult))
                {
                    menuSelection = readResult;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
                    continue;
                }

                switch (menuSelection)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                        HandleRecords(menuSelection);
                        break;
                }

            } while (menuSelection != "5");
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("\nWelcome to Habit Tracker!\n");
            Console.WriteLine("\nThis application will help you track your daily water intake.\n");
            Console.WriteLine("Please select an option below:");
            Console.WriteLine("Type 1 to Insert Record.");
            Console.WriteLine("Type 2 to Delete Record.");
            Console.WriteLine("Type 3 to Update Record.");
            Console.WriteLine("Type 4 to View All Records.");
            Console.WriteLine("Type 5 to Close Application.");
        }

        private static bool HandleRecords(string record)
        {
            try
            {
                switch (record)
                {
                    case "1":
                        Log($"[Operation] Insert Record");
                        Console.WriteLine("\nInsert Record\n");
                        string date = GetValidatedDate("Enter Date (DD-MM-YYYY):");
                        int quantity = GetValidatedInt("Enter Quantity (NO DECIMALS):");

                        ExecuteNonQuery(InsertQuery, cmd =>
                        {
                            cmd.Parameters.AddWithValue("@Date", date);
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                        });

                        Console.WriteLine(RecordInsertedMessage);
                        break;

                    case "2":
                        Log($"[Operation] Delete Record");
                        Console.WriteLine("\nDelete Record\n");
                        int idToDelete = GetValidatedInt("Enter Id:");

                        ExecuteNonQuery(DeleteQuery, cmd =>
                        {
                            cmd.Parameters.AddWithValue("@Id", idToDelete);
                        }, rowsAffected =>
                        {
                            if (rowsAffected == 0)
                            {
                                Console.WriteLine(RecordNotFoundMessage);
                            }
                            else
                            {
                                Console.WriteLine(RecordDeletedMessage);
                            }
                        });
                        break;

                    case "3":
                        Log($"[Operation] Update Record");
                        Console.WriteLine("\nUpdate Record\n");
                        int idToUpdate = GetValidatedInt("Enter Id:");
                        string updatedDate = GetValidatedDate("Enter Date (DD-MM-YYYY):");
                        int updatedQuantity = GetValidatedInt("Enter Quantity (NO DECIMALS):");

                        ExecuteNonQuery(UpdateQuery, cmd =>
                        {
                            cmd.Parameters.AddWithValue("@Date", updatedDate);
                            cmd.Parameters.AddWithValue("@Quantity", updatedQuantity);
                            cmd.Parameters.AddWithValue("@Id", idToUpdate);
                        }, rowsAffected =>
                        {
                            if (rowsAffected == 0)
                            {
                                Console.WriteLine(RecordNotFoundMessage);
                            }
                            else
                            {
                                Console.WriteLine(RecordUpdatedMessage);
                            }
                        });
                        break;

                    case "4":
                        Log($"[Operation] Viewed Record");
                        Console.WriteLine("\nView All Records\n");
                        bool hasRecords = false;

                        ExecuteReader(SelectAllQuery, reader =>
                        {
                            while (reader.Read())
                            {
                                hasRecords = true;
                                Console.WriteLine($"Id: {reader.GetInt32(0)}");
                                Console.WriteLine($"Date: {reader.GetString(1)}");
                                Console.WriteLine($"Quantity: {reader.GetInt32(2)}");
                                Console.WriteLine();
                            }
                        });

                        if (!hasRecords)
                        {
                            Console.WriteLine($"{NoRecordsFoundMessage} The database is currently empty.");
                            Console.WriteLine("Press Enter to return to the menu...");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine(RecordDisplayedMessage);
                        }

                        // Add a pause to keep the records visible
                        Console.WriteLine("Press Enter to return to the menu...");
                        Console.ReadLine();
                        break;

                    case "5":
                        Console.WriteLine("Are you sure you want to exit? (y/n)");
                        string confirmation;
                        do
                        {
                            confirmation = (Console.ReadLine() ?? string.Empty).ToLower();
                            if (confirmation == "y")
                            {
                                return true; // Exit the application
                            }
                            else if (confirmation == "n")
                            {
                                return false; // Stay in the application
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                            }
                        } while (confirmation != "y" && confirmation != "n");
                        break;
                }
            }
            catch (SqliteException ex)
            {
                Log($"[Database Error] Operation: HandleRecords, Record type {record}. Error: {ex.Message}");
                Console.WriteLine(DatabaseErrorMessage);
            }
            catch (Exception ex)
            {
                Log($"[Unexpected Error] Operation: HandleRecords, Record type {record}. Error: {ex.Message}");
                Console.WriteLine(UnexpectedErrorMessage);
            }

            return false; // Default: Stay in the application
        }
    }
}