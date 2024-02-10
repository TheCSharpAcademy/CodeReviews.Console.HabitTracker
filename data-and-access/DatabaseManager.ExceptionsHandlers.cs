using Microsoft.Data.Sqlite;

namespace HabitLogger.data_and_access;

/// <summary>
/// Represents a database manager for handling database operations.
/// </summary>
public partial class DatabaseManager
{
    /// <summary>
    /// Method to print error messages to the console.
    /// </summary>
    /// <param name="e">The exception that occurred.</param>
    /// <param name="transaction">Optional transaction parameter of type SqliteTransaction.</param>
    private static void ErrorMessagePrinter(Exception e, SqliteTransaction? transaction = null)
    {
        if (e is SqliteException sqlException)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SQLite error occured:");
            Console.WriteLine($"""
                               Message: {sqlException.Message},
                               ErrorCode: {sqlException.SqliteErrorCode},
                               StackTrace: {sqlException.StackTrace}
                               """);
            Console.ResetColor();
        } 
        else if (e is KeyNotFoundException) 
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Key not found error occured.");
            Console.WriteLine($"""
                               Message: {e.Message},
                               StackTrace: {e.StackTrace}
                               """);
            Console.ResetColor();
        }
        else if (e is Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error occured:");
            Console.WriteLine($"""
                               Message: {e.Message},
                               StackTrace: {e.StackTrace}
                               """);
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Unknown error occured.");
            Console.WriteLine($"""
                               Message: {e.Message},
                               StackTrace: {e.StackTrace}
                               """);
        }

        transaction?.Rollback();

        Console.ResetColor();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}