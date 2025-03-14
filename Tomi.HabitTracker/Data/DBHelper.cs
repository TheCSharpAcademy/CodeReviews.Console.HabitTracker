using System.Data.SQLite;

namespace Tomi.HabitTracker.Data;

public class DBHelper
{
    private readonly string _connectionString;

    public DBHelper(string connectionString)
    {
        _connectionString = connectionString;
        InitDB();

    }

    private void InitDB()
    {
        if (File.Exists("../habbit-logger.db")) return;
        try
        {
            CreateTable();
            Console.WriteLine("habit-logger data store initialized successfully");
        }
        catch (Exception err)
        {
            Console.WriteLine($"Can't connect to the data store at the moment: {err}");
            Environment.Exit(0);
        }
    }

    private void CreateTable()
    {
        using (SQLiteConnection connection = new(_connectionString))
        {
            try
            {
                connection.Open();
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS habbit_logger(id INTEGER PRIMARY KEY AUTOINCREMENT, habbit TEXT NOT NULL, quantity REAL NOT NULL, date TEXT NOT NULL)";

                using (SQLiteCommand command = new(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


    private void AddParamsToQuery(SQLiteCommand command, string paramName, object? value)
    {
        if (value != null)
        {
            command.Parameters.AddWithValue(paramName, value);
        }
    }


    public List<HabitCompactGist> ViewAllRecords()
    {
        List<HabitCompactGist> habitGistRecords = [];

        using (SQLiteConnection connection = new(_connectionString))
        {
            try
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM habbit_logger;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        int id = reader.GetInt32(0);
                        string habit = reader.GetString(1);
                        double quantity = reader.GetDouble(2);
                        string habitDateString = reader.GetString(3);

                        if (DateTime.TryParse(habitDateString, out DateTime habitDate))
                        {
                            var habitGist = new HabitCompactGist
                            {
                                Id = id,
                                Habit = habit,
                                Quantity = quantity,
                                HabitDate = habitDate,
                            };
                            habitGistRecords.Add(habitGist);

                        }
                        else Console.WriteLine("Internal error while processing habit gist(Invalid Date Format)");

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        return habitGistRecords;
    }


    public void ProcessQuery(string query, List<(string paramName, object value)> paramsList)
    {
        using (SQLiteConnection connection = new(_connectionString))
        {
            try
            {
                using (SQLiteCommand command = new(query, connection))
                {
                    foreach ((string paramName, object value) in paramsList)
                    {
                        AddParamsToQuery(command, paramName, value);
                    }

                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }


    public void InsertRecord((string validatedDateInsert, string habitInsert, double quantityInsert) newRecord)
    {
        string insertQuery = "INSERT INTO habbit_logger (habbit, quantity, date) VALUES(@habbit, @quantity, @date)";

        List<(string, object?)> insertParams = [("@habbit", newRecord.habitInsert), ("@quantity", newRecord.quantityInsert), ("@date", newRecord.validatedDateInsert)];

        ProcessQuery(insertQuery, insertParams);
    }


    public void UpdateRecord(int id, (string validatedDateInsert, string habitInsert, double quantityInsert) updatedRecord)
    {
        string updateQuery = "UPDATE habbit_logger SET habbit = @habbit, quantity = @quantity, date = @date WHERE id = @id";

        List<(string, object?)> updateParams = [("@habbit", updatedRecord.habitInsert), ("@quantity", updatedRecord.quantityInsert), ("@date", updatedRecord.validatedDateInsert), ("@id", id)];

        ProcessQuery(updateQuery, updateParams);
    }

    public void DeleteRecord(int id)
    {
        string deleteQuery = "DELETE FROM habbit_logger WHERE id = @id";

        List<(string, object?)> deleteParams = [("@id", id)];

        ProcessQuery(deleteQuery, deleteParams);
    }


}
