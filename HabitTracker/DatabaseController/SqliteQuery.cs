using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Data;
namespace DatabaseController;


internal class SqliteQuery
{
    internal static bool ExecuteWriteQuery(string query, string databasePath, string habitName, int quantity, string measurementUnit, string outputDate)
    {
        using (var connection = new SqliteConnection($"Data Source={databasePath}"))
        {
            connection.Open();

            var writeQuery = connection.CreateCommand();
            writeQuery.CommandText = query;
            try
            {
                writeQuery.Parameters.AddWithValue("@habitname", habitName);
                writeQuery.Parameters.AddWithValue("@quantity", quantity);
                writeQuery.Parameters.AddWithValue("@measurementunit", measurementUnit);
                writeQuery.Parameters.AddWithValue("@outputdate", outputDate);
                writeQuery.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (SqliteException sqlEx)
            {
                if (sqlEx.Message.Contains("UNIQUE constraint failed"))
                {
                    Console.WriteLine("Unique constraint violated - this habit already exists on this date.");
                    connection.Close();
                    return false;
                }
                throw;

            }



        }
    }

    // Reads all records.
    internal static string ExecuteReadQuery(string query, string databasePath)
    {
        using (var connection = new SqliteConnection($"Data Source={databasePath}"))
        {
            connection.Open();
            string rowsRead = "";
            using (var readQuery = new SqliteCommand(query, connection))
            {
                using var reader = readQuery.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var ID = reader.GetInt32(0);
                        var habitName = reader.GetString(1);
                        var quantity = reader.GetInt32(2);
                        var measurementUnit = reader.GetString(3);
                        var ocurranceDate = reader.GetString(4);
                        rowsRead += $"ID={ID},HabitName={habitName},Ocurrances={quantity},Unit of measurement={measurementUnit},Ocurrance Date={ocurranceDate}.";
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]No records found![/]");
                }
            }
            connection.Close();
            return rowsRead;
        }
    }
    internal static string ExecuteReadQuery(string query, string databasePath,string selectedHabitName)
    {
        using (var connection = new SqliteConnection($"Data Source={databasePath}"))
        {
            connection.Open();
            string rowsRead = "";
            using (var readQuery = new SqliteCommand(query, connection))
            {
                readQuery.Parameters.AddWithValue("@habitname", selectedHabitName);
                using var reader = readQuery.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var ID = reader.GetInt32(0);
                        var habitName = reader.GetString(1);
                        var quantity = reader.GetInt32(2);
                        var measurementUnit = reader.GetString(3);
                        var ocurranceDate = reader.GetString(4);
                        rowsRead += $"ID={ID},HabitName={habitName},Ocurrances={quantity},Unit of measurement={measurementUnit},Ocurrance Date={ocurranceDate}.";
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]No records found![/]");
                }
            }
            connection.Close();
            return rowsRead;
        }
    }

    internal static void ExecuteDeleteQuery(string query, string databasePath, int habitID)
    {
        using (var connection = new SqliteConnection($"Data Source={databasePath}"))
        {
            connection.Open();
            using (var deleteCommand = new SqliteCommand(query, connection))
            {
                deleteCommand.Parameters.AddWithValue("@id", habitID);
                deleteCommand.ExecuteNonQuery();
            }
            connection.Close();

        }
    }

    internal static bool ExecuteUpdateQuery(string query, string databasePath, int habitID, string habitName = "", int quantity = -1, string MeasurementUnit = "", string newDate = "")
    {
        using (var connection = new SqliteConnection($"Data Source={databasePath}"))
        {
            connection.Open();

            string fetchHabitQuery =
            @"
            SELECT * FROM habit 
            WHERE id = @id;
            ";

            // Declare variables outside the loop
            string currentHabitName = "";
            int currentQuantityNumber = 0;
            string currentMeasurementUnit = "";
            string currentDate = "";

            // Execute read query to fetch existing values
            using (var readQuery = new SqliteCommand(fetchHabitQuery, connection))
            {
                readQuery.Parameters.AddWithValue("@id", habitID);
                var reader = readQuery.ExecuteReader();

                while (reader.Read())
                {
                    currentHabitName = reader.GetString(1);  // Habit name
                    currentQuantityNumber = reader.GetInt32(2);  // Occurrence number
                    currentMeasurementUnit = reader.GetString(3);
                    currentDate = reader.GetString(4);  // Date
                }
            }

            // Execute update query using either provided or current values
            using (var updateCommand = new SqliteCommand(query, connection))
            {
                updateCommand.Parameters.AddWithValue("@id", habitID);
                updateCommand.Parameters.AddWithValue("@habitname", string.IsNullOrEmpty(habitName) ? currentHabitName : habitName);
                updateCommand.Parameters.AddWithValue("@quantity", quantity == -1 ? currentQuantityNumber : quantity);
                updateCommand.Parameters.AddWithValue("@measurementunit", string.IsNullOrEmpty(MeasurementUnit) ? currentMeasurementUnit : MeasurementUnit);
                updateCommand.Parameters.AddWithValue("@newdate", string.IsNullOrEmpty(newDate) ? currentDate : newDate);

                try
                {
                    updateCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqliteException sqlEx)
                {
                    if (sqlEx.Message.Contains("UNIQUE constraint failed"))
                    {
                        Console.WriteLine("Unique constraint violated - this habit already exists on this date.\nIf you no longer wish to update this record click Save Changes.");
                        connection.Close();
                        return false;
                    }
                    throw;
                }
            }


        }
    }

}
