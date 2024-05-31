using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Patryk_MM.Console.HabitTracker;

public class Database {
    private string _connectionString = @"Data Source=habit-Tracker.db";
    /// <summary>
    /// Retrieves a list of tables from the SQLite database.
    /// </summary>
    /// <remarks>
    /// This method connects to the SQLite database specified by the connection string,
    /// queries the sqlite_master table to get all table names, and returns a list of those
    /// names excluding the internal 'sqlite_sequence' table.
    /// </remarks>
    /// <returns>A <see cref="List{T}"/> of <see cref="string"/> containing the names of all user-defined tables in the database.</returns>
    public List<string> GetTables() {
        List<string> tablesList = new List<string>();
        using (var connection = new SqliteConnection(_connectionString)) {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table'";
            tableCmd.ExecuteNonQuery();

            using (var reader = tableCmd.ExecuteReader()) {
                while (reader.Read()) {
                    if (reader.GetString(0) == "sqlite_sequence") continue;
                    tablesList.Add(reader.GetString(0));
                }
            }
            return tablesList;
        }
    }
    /// <summary>
    /// Displays the list of tables to the user.
    /// </summary>
    /// <param name="tableList">A list of tables retrieved in the <see cref="GetTables"/> method.</param>
    public void ListTables(List<string> tableList) {
        var tables = new Table();
        tables.AddColumn("Habits");
        foreach (var table in tableList) {
            tables.AddRow(table);
        }

        AnsiConsole.Write(tables);
    }

    /// <summary>
    /// Prompts the user to choose a table from the list of tables retrieved from the database.
    /// </summary>
    /// <returns>
    /// The name of the table chosen by the user as a <see cref="string"/>.
    /// </returns>
    /// <remarks>
    /// This method retrieves a list of table names using the <see cref="GetTables"/> method and 
    /// uses the Spectre.Console library to prompt the user to select one of the tables.
    /// </remarks>
    public string ChooseTable() {
        var tables = GetTables();

        string tableChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose a table:")
            .AddChoices(tables)
            );

        return tableChoice;
    }
    /// <summary>
    /// Establishes a connection to the SQLite database and creates the 'drinking_water' table if it does not already exist.
    /// </summary>
    /// <remarks>
    /// The method creates a table named 'drinking_water' with columns for Id (auto-increment primary key), DATE, and QUANTITY.
    /// This ensures that the necessary table structure is in place for storing drinking water data.
    /// </remarks>
    public void Connect() {
        using (var connection = new SqliteConnection(_connectionString)) {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                     DATE DATE,
                                     QUANTITY DOUBLE)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    /// <summary>
    /// Creates a new habit table in the SQLite database.
    /// </summary>
    /// <remarks>
    /// Prompts the user to provide a name for the new habit table. If the user types "exit", the operation is cancelled.
    /// The method displays a warning that the habit tracker only supports numeric values (e.g., litres of water drank or kilometers ran per day).
    /// The habit table name provided by the user is transformed to lowercase and spaces are replaced with underscores to form the table name.
    /// A new table is then created in the database with columns for Id (auto-increment primary key), DATE, and QUANTITY.
    /// </remarks>
    /// <exception cref="SqliteException">
    /// Thrown when there is an issue executing the SQL command.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided table name is invalid for SQL.
    /// </exception>
    public void CreateHabit() {
        string warning = "Please be aware that at this point \"Habit Tracker\" only allows for tracking habits with numeric values" +
            " (e.g. litres of water drank or kilometers ran per day).";

        AnsiConsole.MarkupLine($"[red]{warning}[/]");

        string tableName = AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan]Please provide a name of your new habit or type \"exit\" to cancel:[/]")
            );

        if (tableName == "exit") {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }


        using (var connection = new SqliteConnection(_connectionString)) {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {tableName.ToLower().Replace(" ", "_")} (
                                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                     DATE DATE,
                                     QUANTITY DOUBLE)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    /// <summary>
    /// Retrieves a list of habits from the specified table in the SQLite database.
    /// </summary>
    /// <param name="table">The name of the table from which to retrieve the habits.</param>
    /// <returns>
    /// A <see cref="List{T}"/> of <see cref="Habit"/> objects representing the habits retrieved from the specified table.
    /// </returns>
    /// <remarks>
    /// This method connects to the SQLite database and executes a SELECT query to retrieve all rows from the specified table.
    /// Each row is mapped to a <see cref="Habit"/> object and added to the list of habits.
    /// </remarks>
    /// <exception cref="SqliteException">
    /// Thrown when there is an issue executing the SQL command.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the specified table does not exist or the query fails.
    /// </exception>
    public List<Habit> GetHabits(string table) {

        var habits = new List<Habit>();

        using (var connection = new SqliteConnection(_connectionString)) {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM {table}";
            tableCmd.ExecuteNonQuery();

            using (var reader = tableCmd.ExecuteReader()) {
                while (reader.Read()) {
                    var habit = new Habit() {
                        Id = reader.GetInt32(0),
                        Date = reader.GetString(1),
                        Quantity = reader.GetDouble(2)
                    };

                    habits.Add(habit);
                }
            }
        }
        return habits;
    }
    /// <summary>
    /// Displays the list of habits to the user in a tabular format.
    /// </summary>
    /// <param name="habits">A list of <see cref="Habit"/> objects to be displayed.</param>
    /// <remarks>
    /// If the list of habits is empty, a message indicating that no habits were found is displayed.
    /// Otherwise, the habits are displayed in a table with columns for ListId, Date, and Quantity.
    /// The table is rendered using the Spectre.Console library.
    /// </remarks>
    public void ViewHabits(List<Habit> habits) {
        if (!habits.Any()) {
            AnsiConsole.MarkupLine("[red]No habits found.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("ListId");
        table.AddColumn("Date");
        table.AddColumn("Quantity");

        for (int i = 0; i < habits.Count; i++) {
            var habit = habits[i];
            table.AddRow((i + 1).ToString(), habit.Date, habit.Quantity.ToString());
        }

        AnsiConsole.Write(table);
    }
    /// <summary>
    /// Updates an existing habit entry in the SQLite database.
    /// </summary>
    /// <remarks>
    /// This method allows the user to update the date and quantity of an existing habit in the specified table.
    /// The user is prompted to choose a table, select an entry to update, and provide new values for the selected entry.
    /// If the user cancels at any step, the operation is aborted.
    /// </remarks>
    /// <exception cref="SqliteException">
    /// Thrown when there is an issue executing the SQL command.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided table name or entry ID is invalid.
    /// </exception>
    public void UpdateHabit() {
        //Choose the table to perform an operation on.
        string table = ChooseTable();
        //Get the list of habits from the chosen table.
        var habits = GetHabits(table);

        //Check if there are any habits in the table.
        if (!habits.Any()) {
            AnsiConsole.MarkupLine("[red]No habits found.[/]");
            return;
        }

        //Display the habits in the table.
        ViewHabits(habits);
        
        //Input an Id of the habit.
        int listId = AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan]Please provide the ListId of the entry you want to update or type 0 (zero) to exit:[/]")
            .Validate(id => {
                return id == 0 ? ValidationResult.Success() :
                id < 0 || id > habits.Count ? ValidationResult.Error("Invalid ListId.") :
                ValidationResult.Success();
            }));

        //Id escape check.
        if (listId == 0) {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }

        Habit habit = habits[listId - 1]; // ListId is 1-based, so adjust for 0-based index.

        //Input a new date.
        string dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan]Please provide a new date (yyyy-MM-dd) or type 'exit' to cancel operation:[/]")
            .Validate(date => {
                if (date.ToLower() == "exit") return ValidationResult.Success();
                return DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Invalid date format.[/]");
            }));

        //Date escape check.
        if (dateInput.ToLower() == "exit") {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }

        habit.Date = dateInput;

        //Input a new quantity.
        double quantity = AnsiConsole.Prompt(
            new TextPrompt<double>("[cyan]Please provide a new quantity or type 0 (zero) to cancel operation:[/]")
            .Validate(q => {
                return q switch {
                    < 0 => ValidationResult.Error("Quantity must be a positive number."),
                    _ => ValidationResult.Success()
                };
            }));

        //Quantity escape check.
        if (quantity == 0) {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }

        habit.Quantity = quantity;

        //Connect to database and execute the query.
        using (var connection = new SqliteConnection(_connectionString)) {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE {table} SET Date=@date, Quantity=@quantity WHERE Id=@id";
            tableCmd.Parameters.AddWithValue("@date", habit.Date);
            tableCmd.Parameters.AddWithValue("@quantity", habit.Quantity);
            tableCmd.Parameters.AddWithValue("@id", habit.Id);

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        //Clear the console and display an updated table to the user.
        AnsiConsole.Clear();
        ViewHabits(habits);
        AnsiConsole.MarkupLine("[green]Habit successfully updated![/]");
    }

    /// <summary>
    /// Adds a new habit entry to the specified table in the SQLite database.
    /// </summary>
    /// <remarks>
    /// This method allows the user to add a new habit entry to a selected table. The user is prompted to provide a date
    /// and a quantity for the new habit entry. If the user cancels at any step, the operation is aborted.
    /// </remarks>
    /// <exception cref="SqliteException">
    /// Thrown when there is an issue executing the SQL command.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided table name is invalid.
    /// </exception>
    public void AddHabit() {
        AnsiConsole.Clear();
        //Choose the table to perform an operation on.
        var table = ChooseTable();

        //Prompt user to input a date.
        string dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan]Please provide a date (yyyy-MM-dd) or type 'exit' to cancel operation:[/]")
            .Validate(date => {
                if (date.ToLower() == "exit") return ValidationResult.Success();
                return DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Invalid date format.[/]");
            }));
        if (dateInput.ToLower() == "exit") {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }

        //Prompt user to input a quantity.
        double quantity = AnsiConsole.Prompt(
            new TextPrompt<double>("[cyan]Please provide a quantity or type 0 (zero) to cancel operation:[/]")
            .Validate(q => {
                return q switch {
                    < 0 => ValidationResult.Error("Quantity must be a positive number."),
                    _ => ValidationResult.Success()
                };
            }));
        if (quantity == 0) {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }

        //Connect to database and execute the query.
        using (var connection = new SqliteConnection(_connectionString)) {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO {table} (date, quantity) VALUES (@date, @quantity)";
            tableCmd.Parameters.AddWithValue("@date", dateInput);
            tableCmd.Parameters.AddWithValue("@quantity", quantity);

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        AnsiConsole.MarkupLine("[green]Habit successfully added![/]");
    }
    /// <summary>
    /// Deletes a habit entry from the specified table in the SQLite database.
    /// </summary>
    /// <remarks>
    /// This method allows the user to delete a habit entry from a selected table. The user is prompted to choose a table,
    /// select an entry to delete, and confirm the deletion. If the user cancels at any step, the operation is aborted.
    /// </remarks>
    /// <exception cref="SqliteException">
    /// Thrown when there is an issue executing the SQL command.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided table name or entry ID is invalid.
    /// </exception>
    public void DeleteHabit() {
        //Choose the table to perform an operation on.
        string table = ChooseTable();
        //Get the list of habits from the chosen table.
        var habits = GetHabits(table);

        //Check if there are any habits in the table.
        if (!habits.Any()) {
            AnsiConsole.MarkupLine("[red]No habits found.[/]");
            return;
        }

        //Display the habits in the table.
        ViewHabits(habits);

        int listId = AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan]Please provide the ListId of the habit you want to delete or type 0 (zero) to exit:[/]")
            .Validate(id => {
                return id == 0 ? ValidationResult.Success() :
                id < 0 || id > habits.Count ? ValidationResult.Error("Invalid ListId.") :
                ValidationResult.Success();
            }));

        //Escape check
        if (listId == 0) {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }

        Habit habit = habits[listId - 1]; // ListId is 1-based, so adjust for 0-based index.

        //Connect to database and execute the query
        using (var connection = new SqliteConnection(_connectionString)) {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM {table} WHERE Id = @id";
            tableCmd.Parameters.AddWithValue("@id", habit.Id);
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        //If there are any habits in the table after the execution, display the table.
        AnsiConsole.Clear();
        habits = GetHabits(table);
        if (habits.Any()) {
            ViewHabits(habits);
        }
        AnsiConsole.MarkupLine("[green]Habit successfully deleted.[/]");
    }

    /// <summary>
    /// Generates a report based on habits recorded within a given period of time.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to select the type of report they want to generate: sum of quantities or average of quantities.
    /// The user is then prompted to input the start and end dates for the report period.
    /// Habits within the specified date range are filtered and used to calculate the desired report metric.
    /// The calculated metric is displayed to the user.
    /// </remarks>
    public void GenerateReport() {
        //Choose the table to perform an operation on.
        var table = ChooseTable();
        //Get the list of habits from the chosen table.
        var habits = GetHabits(table);

        //Check if there are any habits in the table.
        if (!habits.Any()) {
            AnsiConsole.MarkupLine("[red]No habits found.[/]");
            return;
        }

        //Prompt the user to choose a type of report.
        var typeChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the type of report:")
            .AddChoices(["Sum of quantities for a given period of time", "Average of quantities for a given period of time"])
            );

        // Prompt the user to input the start and end dates.
        string startDateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan]Please provide the start date (yyyy-MM-dd):[/]")
            .Validate(date => {
                return DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("Invalid date format.");
            }));

        string endDateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan]Please provide the end date (yyyy-MM-dd):[/]")
            .Validate(date => {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                    return ValidationResult.Error("Invalid date format.");

                // Check if end date is older than start date.
                if (DateTime.Parse(date) < DateTime.Parse(startDateInput))
                    return ValidationResult.Error("End date cannot be older than start date.");

                return ValidationResult.Success();
            }));

        // Filter habits based on the provided date range.
        var filteredHabits = habits.Where(habit =>
            DateTime.Parse(habit.Date) >= DateTime.Parse(startDateInput) &&
            DateTime.Parse(habit.Date) <= DateTime.Parse(endDateInput));

        switch (typeChoice) {
            case "Sum of quantities for a given period of time":
                // Calculate the sum of quantities.
                double sum = filteredHabits.Sum(habit => habit.Quantity);
                AnsiConsole.WriteLine($"Sum of quantities from {startDateInput} to {endDateInput}: {sum}");
                break;
            case "Average of quantities for a given period of time":
                //Calculate the average of quantities.
                double avg = filteredHabits.Average(habit => habit.Quantity);
                AnsiConsole.WriteLine($"Average of quantities from {startDateInput} to {endDateInput}: {avg}");
                break;
        }
    }
}
