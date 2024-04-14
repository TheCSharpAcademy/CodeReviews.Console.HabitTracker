using System.Collections.Generic;
using System.Data.SQLite;

namespace HabitTrackerConsole.Util;

/// <summary>
/// SqliteHelper provides utility methods for preparing and executing SQL commands using SQLite.
/// It facilitates the interaction with the SQLite database by abstracting command preparation and execution.
/// </summary>
public class SqliteHelper
{
    /// <summary>
    /// Prepares an SQLite command with optional parameters and transaction.
    /// </summary>
    /// <param name="commandText">SQL command text.</param>
    /// <param name="connection">Active SQLiteConnection object.</param>
    /// <param name="parameters">Optional dictionary of parameters.</param>
    /// <param name="transaction">Optional SQLiteTransaction for transactional command execution.</param>
    /// <returns>A prepared SQLiteCommand.</returns>
    public static SQLiteCommand PrepareCommand(string commandText, SQLiteConnection connection, Dictionary<string, object>? parameters = null, SQLiteTransaction? transaction = null)
    {
        var command = new SQLiteCommand(commandText, connection, transaction);
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }
        return command;
    }

    /// <summary>
    /// Executes a non-query SQL command and returns the number of rows affected.
    /// </summary>
    /// <param name="commandText">SQL command text.</param>
    /// <param name="connection">Active SQLiteConnection object.</param>
    /// <param name="parameters">Optional dictionary of parameters.</param>
    /// <param name="transaction">Optional SQLiteTransaction for transactional execution.</param>
    /// <returns>The number of rows affected by the command.</returns>
    public static int ExecuteCommand(string commandText, SQLiteConnection connection, Dictionary<string, object>? parameters = null, SQLiteTransaction? transaction = null)
    {
        var command = PrepareCommand(commandText, connection, parameters, transaction);
        return command.ExecuteNonQuery();
    }
}