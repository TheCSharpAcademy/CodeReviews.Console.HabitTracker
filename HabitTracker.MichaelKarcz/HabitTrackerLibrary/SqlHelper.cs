using Microsoft.Data.Sqlite;
using HabitTrackerLibrary.Models;

namespace HabitTrackerLibrary;
public static class SqlHelper
{

    private static readonly string CONNECTION_STRING = @"Data Source=habit-Tracker.db";
    private static readonly string TABLENAME = "bike_riding";

    public static void CreateDatabaseIfNotExists() 
    {
        try
        {
            using (var connection = new SqliteConnection(CONNECTION_STRING))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {TABLENAME} (
                                              Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                              Date TEXT,
                                              Miles INTEGER
                                              )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"\n\nThere was an error trying to instantiate the database. Error message: {e.Message}\n\n");
        }
    }

    public static List<Habit> GetAllRecords()
    {
        List<Habit> allEntries = new List<Habit>();

        try
        {
            using (var connection = new SqliteConnection(CONNECTION_STRING))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT * FROM {TABLENAME}";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        allEntries.Add(
                            new Habit
                            {
                                Id = reader.GetInt32(0),
                                Date = reader.GetString(1),
                                Miles = reader.GetInt32(2)
                            });
                    }
                }
                else
                {
                    return new List<Habit>();
                }

                connection.Close();
            }

        }
        catch (SqliteException e)
        {
            Console.WriteLine($"\n\nThere was an error trying to retrieve the database records. Error message: {e.Message}\n\n");
            return new List<Habit>();
        }

            return allEntries;
    }

    public static List<int> GetAllIds()
    {
        List<int> allIds = new List<int>();
        try
        {
            using (var connection = new SqliteConnection(CONNECTION_STRING))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT Id FROM {TABLENAME}";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        allIds.Add(reader.GetInt32(0));
                    }
                }
                else
                {
                    return new List<int>();
                }

                connection.Close();
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"\n\nThere was an error trying to retrieve the database records. Error message: {e.Message}\n\n");
            return new List<int>();
        }

        return allIds;
    }

    public static bool InsertSingleRecord(Habit habit)
    {
        string commandText = $@"INSERT INTO {TABLENAME} (Date, Miles)
                                 VALUES ('{habit.Date}', '{habit.Miles}');";

        return PerformCudOperation(commandText);
    }

    public static bool DeleteRecord(int id)
    {
        string commandText = $@"DELETE FROM {TABLENAME}
                                 WHERE Id={id};";

        return PerformCudOperation(commandText);
    }

    public static bool UpdateRecord(int id, Habit habit)
    {
        string commandText = $@"UPDATE {TABLENAME}
                                 SET Date='{habit.Date}', Miles={habit.Miles}
                                 WHERE Id={id};";

        return PerformCudOperation(commandText);
    }

    private static bool PerformCudOperation(string commandText)
    {
        bool commandSuccessful = false;

        try
        {
            using (var connection = new SqliteConnection(CONNECTION_STRING))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = commandText;

                int rowsAffected = tableCmd.ExecuteNonQuery();

                if (rowsAffected > 0) commandSuccessful = true;

                connection.Close();
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"\n\n{e.Message}\n\n");
            commandSuccessful = false;
        }

        return commandSuccessful;
    }
}
