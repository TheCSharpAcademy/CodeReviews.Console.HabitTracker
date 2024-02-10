using Microsoft.Data.Sqlite;

namespace HabitLogger.data_and_access;

/// <summary>
/// The <see cref="data_and_access.DatabaseManager"/> class provides methods to interact with a database.
/// </summary>
public partial class DatabaseManager
{
    /// <summary>
    /// Executes a non-query SQL command on the database.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional dictionary of parameters to use in the query.</param>
    /// <returns>
    /// The number of rows affected by the command. Returns -1 if an error occurs.
    /// </returns>
    public int ExecuteNonQuery(string query, Dictionary<string, object> parameters)
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();
        using var command = CreateCommand(query, connection, parameters);
        command.Transaction = transaction;

        try
        {
            int affectedRows = command.ExecuteNonQuery();
            transaction.Commit();
            
            return affectedRows;
        }
        catch (SqliteException sqlEx)
        {
            ErrorMessagePrinter(sqlEx, transaction);
            return -1;
        }
        catch (Exception e)
        {
            ErrorMessagePrinter(e, transaction);
            return -1;
        }
        finally
        {
            CloseConnection(connection);
        }
    }

    /// <summary>
    /// Executes a non-query SQL statement on the database.
    /// </summary>
    /// <param name="query">The SQL statement to execute.</param>
    /// <returns>The number of rows affected by the execution of the SQL statement.</returns>
    public int ExecuteNonQuery(string query)
    {
        return ExecuteNonQuery(query, new Dictionary<string, object>());
    }

    /// <summary>
    /// Executes a SELECT query on the database.
    /// </summary>
    /// <param name="query">The SQL SELECT query to execute.</param>
    /// <param name="parameters">The parameters to be used in the query.</param>
    /// <returns>A list of dictionaries containing the query results, or null if an exception occurs.</returns>
    /// <remarks>
    /// This method executes a SQL SELECT query on the database and returns the results as a list of dictionaries.
    /// Each dictionary represents a row in the query results, with the column names as the keys and the column values as the values.
    /// The query can contain parameter placeholders (e.g., @param) that can be replaced with actual parameter values using the parameters argument.
    /// If no parameters are required, the parameters argument can be null.
    /// If an exception occurs during the query execution, the method prints an error message and returns null.
    /// </remarks>
    public List<Dictionary<string, object>>? ExecuteQuery(string query, Dictionary<string, object> parameters)
    {
        var results = new List<Dictionary<string, object>>();
        using var connection = OpenConnection();

        try
        {
            using var command = CreateCommand(query, connection, parameters);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = (reader.IsDBNull(i) ? null : reader.GetValue(i)) ?? "NIL";
                }

                results.Add(row);
            }
        }
        catch (SqliteException sqlEx)
        {
            ErrorMessagePrinter(sqlEx);
            return null;
        }
        catch (Exception e)
        {
            ErrorMessagePrinter(e);
            return null;
        }
        finally
        {
            CloseConnection(connection);
        }
        
        return results;
    }

    /// <summary>
    /// Executes the specified SQL query and returns a list of dictionaries representing the result set.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <returns>A list of dictionaries representing the result set.</returns>
    public List<Dictionary<string, object>>? ExecuteQuery(string query)
    {
        return ExecuteQuery(query, new Dictionary<string, object>());
    }

    /// <summary>
    /// Executes a SQL query that returns a single value from the database.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional parameters for the SQL query.</param>
    /// <returns>
    /// The first column of the first row in the result set, or null if the result set is empty.
    /// </returns>
    public object? ExecuteScalar(string query, Dictionary<string, object> parameters)
    {
        using var connection = OpenConnection();

        try
        {
            using var command = CreateCommand(query, connection, parameters);

            return command.ExecuteScalar();
        }
        catch (SqliteException sqlEx)
        {
            ErrorMessagePrinter(sqlEx);
            return null;
        }
        catch (Exception e)
        {
            ErrorMessagePrinter(e);
            return null;
        }
        finally
        {
            CloseConnection(connection);
        }
    }

    /// <summary>
    /// Executes a SQL query and returns the first column of the first row in the result set as an <see cref="object"/>.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <returns>
    /// The first column of the first row in the result set as an <see cref="object"/>, or null if the query execution fails.
    /// </returns>

    public object? ExecuteScalar(string query)
    {
        return ExecuteScalar(query, new Dictionary<string, object>());
    }
}