using System.Globalization;
using HabitLogger.data_and_access;
using HabitLogger.enums;
using HabitLogger.logic.utils;
using HabitLogger.view;
using Spectre.Console;

namespace HabitLogger.logic;

/// <summary>
/// The HabitLogger class manages the logging of habits and records in a habit tracking application.
/// </summary>
internal class HabitLogger
{
    /// <summary>
    /// Manages the database connection and provides query execution functionality.
    /// </summary>
    private readonly DatabaseManager _databaseManager;

    public HabitLogger(string? connectionString)
    {
        var s = string.IsNullOrWhiteSpace(connectionString) ? 
            "Data Source=habit-Tracker.db" : connectionString;
        
        _databaseManager = new DatabaseManager(s);
        
        _databaseManager.CreateDatabase();
    }

    /// <summary>
    /// Adds a new habit to the database.
    /// </summary>
    /// <remarks>
    /// The method prompts the user to enter the name and unit of measurement for the new habit.
    /// It then inserts the habit into the database table 'habits'.
    /// </remarks>
    internal void AddHabit()
    {
        string? name;
        string? unit;
        
        try
        {
            name = Utilities.ValidateTextInput("name");

            unit = Utilities.ValidateTextInput("unit of measurement");
        }
        catch (Utilities.ExitToMainException)
        {
            return;
        }
        
        const string query = "INSERT INTO habits (Name, Unit) VALUES (@name, @unit)";
        var parameters = new Dictionary<string, object>
        {
            { "@name", name },
            { "@unit", unit }
        };
            
        var returnResult = _databaseManager.ExecuteNonQuery(query, parameters);
            
        PrintResult("Habit", "add", returnResult);
    }

    /// <summary>
    /// Deletes a habit from the database.
    /// </summary>
    internal void DeleteHabit()
    {
        int id;
        
        GetHabits();

        try
        {
            id = Utilities.ValidateNumber("\nEnter the ID of the habit to delete:");
        }
        catch (Utilities.ExitToMainException)
        {
            return;
        }

        const string query = "DELETE FROM habits WHERE Id = @id";
        var parameters = new Dictionary<string, object>
        {
            { "@id", id }
        };
        
        var returnResult = _databaseManager.ExecuteNonQuery(query, parameters);

        PrintResult("Habit", "delete", returnResult);
    }

    /// <summary>
    /// Updates a habit in the habit logger.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter the ID of the habit they want to update.
    /// It then asks the user if they want to update the name and unit of measurement for the habit.
    /// If the user confirms the update, it prompts the user to enter the new name and unit of measurement.
    /// The method then constructs a SQL update query for the 'habits' table in the database,
    /// with the provided parameters and executes the query using the DatabaseManager.
    /// Finally, it displays a success or failure message to the console.
    /// </remarks>
    internal void UpdateHabit()
    {
        bool updateName;
        bool updateUnit;
        var parameters = new Dictionary<string, object>();
        
        GetHabits();

        try
        {
            var id = Utilities.ValidateNumber("Please type the id of the habit you want to update.");
            parameters.Add("@id", id);
            
            updateName = AnsiConsole.Confirm("Update name?");
            if (updateName)
            {
                var name = Utilities.ValidateTextInput("new name");
                parameters.Add("@name", name);
            }

            updateUnit = AnsiConsole.Confirm("Update unit?");
            if (updateUnit)
            {
                var unit = Utilities.ValidateTextInput("new unit of measurement");
                parameters.Add("@unit", unit);
            }
        }
        catch (Utilities.ExitToMainException)
        {
            return;
        }
        
        if (!updateName && !updateUnit)
        {
            AnsiConsole.WriteLine("No changes made.");
            return;
        }
        
        var query = Utilities.UpdateQueryBuilder("habits", parameters);
        
        int returnCode = _databaseManager.ExecuteNonQuery(query, parameters);
        
        PrintResult("Habit", "update", returnCode);
    }

    /// <summary>
    /// Retrieves a list of habits from the database.
    /// </summary>
    /// <remarks>
    /// This method executes a query to select all habits from the "Habits" table in the database.
    /// It maps the retrieved rows to Habit objects and returns a list of habits.
    /// </remarks>
    internal void GetHabits()
    {
        List<Habit> habits = new();
        const string query = "SELECT * FROM Habits";
        var result = _databaseManager.ExecuteQuery(query);

        if (result != null)
        {
            foreach (var row in result)
            {
                habits.Add(
                    new Habit(
                        Convert.ToInt32(row["Id"]),
                        (string)row["Name"],
                        (string)row["Unit"]
                    )
                );
            }
        }

        ContentView.ViewHabits(habits);
    }

    /// <summary>
    /// Adds a record for a habit to the database.
    /// </summary>
    internal void AddRecord()
    {
        int habitId;
        int quantity;
        string date;
        
        try {
            date = Utilities.ValidateDate("Enter the date of the record (yyyy-MM-dd):");
        
            GetHabits();

            habitId = Utilities.ValidateNumber("Enter the ID of the habit:");
            quantity =
                Utilities.ValidateNumber("Enter the quantity of the record (no decimal or negative numbers):");
        } catch (Utilities.ExitToMainException)
        {
            return;
        }

        const string query = "INSERT INTO records (Date, Quantity, HabitId) VALUES (@date, @quantity, @habitId)";
        var parameters = new Dictionary<string, object>
        {
            { "@date", date },
            { "@quantity", quantity },
            { "@habitId", habitId }
        };
            
        int returnResult = _databaseManager.ExecuteNonQuery(query, parameters);

        PrintResult("Record", "add", returnResult);
    }

    /// <summary>
    /// Deletes a record from the database.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter the ID of the record to be deleted.
    /// It then executes a DELETE query on the 'records' table of the database, using the provided ID as a parameter.
    /// If the deletion is successful, it displays a success message. Otherwise, it displays a failure message.
    /// </remarks>
    internal void DeleteRecord()
    {
        int id;
        GetRecords();

        try
        {
            id = Utilities.ValidateNumber("\nEnter the ID of the record to delete:");
        }
        catch (Utilities.ExitToMainException)
        {
            return;
        }
        
        const string query = "DELETE FROM records WHERE Id = @id";
        var parameters = new Dictionary<string, object>
        {
            { "@id", id }
        };
            
        int returnResult = _databaseManager.ExecuteNonQuery(query, parameters);

        PrintResult("Record", "delete", returnResult);
    }

    /// <summary>
    /// Updates a record in the database.
    /// </summary>
    internal void UpdateRecord()
    {
        bool updateDate;
        bool updateRecord;
        var parameters = new Dictionary<string, object>();

        GetRecords();
        try
        {
            var id = Utilities.ValidateNumber("Please type the id of the record you want to delete.");
            parameters.Add("@id", id);

            updateDate = AnsiConsole.Confirm("Update date?");
            if (updateDate)
            {
                var date = Utilities.ValidateDate(
                    "\nEnter the date of the record (yyyy-MM-dd):"
                );
                parameters.Add("@date", date);
            }

            updateRecord = AnsiConsole.Confirm("\nUpdate amount?");
            if (updateRecord)
            {
                var amount = Utilities.ValidateNumber("Please, enter amount:");
                parameters.Add("@quantity", amount);
            }
        } catch (Utilities.ExitToMainException)
        {
            return;
        }

        if (!updateDate && !updateRecord)
        {
            AnsiConsole.WriteLine("No changes made.");
            return;
        }

        var query = Utilities.UpdateQueryBuilder("records", parameters);
        
        int returnCode = _databaseManager.ExecuteNonQuery(query, parameters);

        PrintResult("Record", "update", returnCode);
    }

    /// <summary>
    /// Retrieves the records from the database and displays them in the view.
    /// </summary>
    /// <remarks>
    /// This method fetches the records from the 'records' table in the database and joins them with the 'habits' table
    /// to get additional information about the associated habit.
    /// The retrieved records are then displayed using the ContentView.ViewRecords method.
    /// </remarks>
    internal void GetRecords()
    {
        List<RecordWithHabit> records = new();
        const string query = """
                             
                             SELECT records.Id, records.Date, records.Quantity, 
                                    records.Quantity, records.HabitId, habits.Name AS HabitName, 
                                    habits.Unit FROM records
                                    INNER JOIN habits ON records.HabitId = habits.Id
                             """;

        var result = _databaseManager.ExecuteQuery(query);

        if (result != null)
        {
            records = CreateRecordWithHabitList(result);
        }

        ContentView.ViewRecords(records);
    }

    /// <summary>
    /// Generates a habit report based on the specified report type and habit ID.
    /// </summary>
    /// <param name="reportType">The type of report to generate.</param>
    /// <param name="id">The ID of the habit.</param>
    internal void GenerateHabitReport(ReportType reportType, int id)
    {
        var (habitName, measurementUnit) = GetSupportInfo(id);

        var (query, parameters) = Utilities.CreateReportQuery(reportType, id);

        var result = _databaseManager.ExecuteQuery(
            query ?? string.Empty, parameters ?? new Dictionary<string, object>()
            );
        
        if (result != null)
        {
            var records = CreateRecordWithHabitList(result);

            ContentView.ViewHabitReport(records, habitName, measurementUnit);
        }
        else
        {
            AnsiConsole.WriteLine("No records found.");
        }
    }

    /// <summary>
    /// Retrieves the support information for a given habit.
    /// </summary>
    /// <param name="id">The ID of the habit.</param>
    /// <returns>A tuple containing the habit name and measurement units.</returns>
    private (string habitName, string mesurementUnits) GetSupportInfo(int id)
    {
        const string query = "SELECT Name, Unit FROM habits WHERE Id = @id";
        
        var parameters = new Dictionary<string, object>
        {
            { "@id", id }
        };
        
        var result = _databaseManager.ExecuteQuery(query, parameters);
        
        return (result![0]["Name"].ToString(), result[0]["Unit"].ToString())!;
    }

    /// <summary>
    /// Creates a list of records with habits from a result set.
    /// </summary>
    /// <param name="result">The result set containing the records with habits.</param>
    /// <returns>A list of records with habits.</returns>
    private List<RecordWithHabit> CreateRecordWithHabitList(List<Dictionary<string, object>> result)
    {
        var records = new List<RecordWithHabit>();
        
        foreach (var row in result)
        {
            int? id = row.TryGetValue("Id", out var value) ? Convert.ToInt32(value) : null;
            var habitName = row.TryGetValue("HabitName", out value) ? (string)row["HabitName"] : null;
            var unit = row.TryGetValue("Unit", out value) ? (string)row["Unit"] : null;
            
            records.Add(
                new RecordWithHabit(
                    id,
                    DateTime.ParseExact((string)row["Date"], "yyyy-MM-dd", new CultureInfo("en-CA")),
                    Convert.ToInt32(row["Quantity"]),
                    habitName,
                    unit
                )
            );
        }

        return records;
    }

    /// <summary>
    /// Prints the result of an action performed on a habit or record.
    /// </summary>
    /// <param name="type">The type of the object (Habit or Record).</param>
    /// <param name="action">The action performed (add, delete, or update).</param>
    /// <param name="returnResult">The return code indicating the result of the action.</param>
    private void PrintResult(string type, string action, int returnResult)
    {
        switch (returnResult)
        {
            case -1:
                AnsiConsole.WriteLine($"Failed to {action} {type.ToLower()}.");
                break;
            case 0:
                AnsiConsole.WriteLine($"{type} not found.");
                break;
            default:
                AnsiConsole.WriteLine($"{type} deleted successfully!");
                break;
        }
    }
}