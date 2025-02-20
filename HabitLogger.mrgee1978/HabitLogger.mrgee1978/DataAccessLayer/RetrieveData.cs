using HabitLogger.mrgee1978.DomainLayer;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitLogger.mrgee1978.DataAccessLayer;

public class RetrieveData
{
    /// <summary>
    /// Retrieve records for the database which are tied to a habit
    /// and return them as a List of record objects
    /// </summary>
    /// <returns></returns>
    public List<Record> RetrieveRecords()
    {
        List<Record> records = new List<Record>();

        try
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseCreation.DatabaseConnectionString))
            {
                connection.Open();
                SqliteCommand retrievalCommand = connection.CreateCommand();

                retrievalCommand.CommandText = @"
                    SELECT records.Id, records.Date, records.Quantity, records.HabitId, habits.Name, habits.UnitOfMeasurement FROM records INNER JOIN habits on records.HabitId = habits.Id";

                try
                {
                    using (SqliteDataReader dataReader = retrievalCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    records.Add(
                                        new Record(
                                        dataReader.GetInt32(0),
                                        DateTime.ParseExact(dataReader.GetString(1), "dd-MM-yy", CultureInfo.InvariantCulture),
                                        dataReader.GetInt32(2),
                                        dataReader.GetInt32(3),
                                        dataReader.GetString(4),
                                        dataReader.GetString(5)
                                        ));
                                }
                                catch (FormatException ex)
                                {
                                    Console.WriteLine($"{ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No data found");
                            Console.ResetColor();
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }

        return records;
    }

    /// <summary>
    /// Retrieve habits from the database and return them as a 
    /// list of Habit objects
    /// </summary>
    /// <returns></returns>
    public List<Habit> RetrieveHabits()
    {
        List<Habit> habits = new List<Habit>();

        try
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseCreation.DatabaseConnectionString))
            {
                connection.Open();
                SqliteCommand retrievalCommand = connection.CreateCommand();

                retrievalCommand.CommandText = @"SELECT * FROM habits";
                try
                {
                    using (SqliteDataReader dataReader = retrievalCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    habits.Add(
                                        new Habit(
                                            dataReader.GetInt32(0),
                                            dataReader.GetString(1),
                                            dataReader.GetString(2)));
                                }
                                catch (FormatException ex)
                                {
                                    Console.WriteLine($"{ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No data found");
                            Console.ResetColor();
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }


        return habits;
    }
}
