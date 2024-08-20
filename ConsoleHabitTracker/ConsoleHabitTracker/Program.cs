using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConsoleHabitTracker;

public static class Program
{
    private static Random _random = new Random();

    static void Main()
    {
        var continueProgram = true;

        string connectionString = "Data Source = habits.db; Version=3;";

        using var connection = new SQLiteConnection(connectionString);

        connection.Open();
        if (connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Failed to connect to the database.");
            return;
        }

        Console.WriteLine("Connected to the database.");

        string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='habitsTable';";
        var tableExists = false;

        using (SQLiteCommand command = new (checkTableQuery, connection))
        {
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                tableExists = reader.HasRows;
            }
        }

        // if table doesn't exit Create a table

        if (!tableExists)
        {
            string createTableQuery = "CREATE TABLE habitsTable " +
                                      "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                      "Date TEXT NOT NULL , " +
                                      "HabitName TEXT NOT NULL, " +
                                      "Quantity INTEGER, " +
                                      "Units TEXT);";

            using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            CreateAndPopulateData(connection);
        }

        while (continueProgram)
        {
            DisplayMenu();
            var selection = Console.ReadKey().KeyChar;

            switch (selection)
            {
                case '0':
                    Console.WriteLine("\n\nClosing application");
                    continueProgram = false;
                    break;
                case '1':
                    Console.WriteLine("\n\nView All Records");
                    ViewRecords(connection);
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    break;
                case '2':
                    Console.WriteLine("\n\nAdd a Record");
                    AddNewHabit(connection);
                    break;
                case '3':
                    Console.WriteLine("\n\nDelete a Record");
                    ViewRecords(connection);
                    DeleteEntry(connection);
                    break;
                case '4':
                    Console.WriteLine("\n\nEdit a Record");
                    ViewRecords(connection);
                    UpdateEntry(connection);
                    break;
                default:
                    Console.WriteLine("\n\nInvalid Selection press enter to try again");
                    Console.ReadLine();
                    break;
            }
        }

        connection.Close();
    }
    
    private static void CreateAndPopulateData(SQLiteConnection connection)
    {
        List<Habit> prepopulatedData = new();
        int counter = 100;
        while (counter > 0)
        {
            DateOnly randomDate = GenerateRandomDate(new DateOnly(2000, 1, 1), new DateOnly(2024, 7, 31));
            string randomActivity = GenerateRandomActivity();
            int randomDuration = _random.Next(1, 60);
            string randomUnits = GenerateRandomUnits();

            // Create an Habit object and add it to the list
            var entry = new Habit
            {
                Date = randomDate,
                HabitName = randomActivity,
                Quantity = randomDuration,
                Units = randomUnits
            };
            prepopulatedData.Add(entry);
            counter--;
        }
        
        foreach (var entry in prepopulatedData)
        {
            string insertQuery = "INSERT INTO habitsTable (Date, HabitName, Quantity, Units) " +
                                 "VALUES (@date, @habitName, @quantity, @units);";

            using SQLiteCommand command = new SQLiteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@date", entry.Date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@habitName", entry.HabitName);
            command.Parameters.AddWithValue("@quantity", entry.Quantity);
            command.Parameters.AddWithValue("@units", entry.Units);

            command.ExecuteNonQuery();
        }
    }

    static string GenerateRandomActivity()
    {
        string[] possibleActivities = ["swimming", "running", "walking", "cycling", "working", "cooking", "coding", "reading"];
        int chosenEntry = _random.Next(0, 8);
        return possibleActivities[chosenEntry];
    }
    
    static string GenerateRandomUnits()
    {
        string[] possibleUnits = ["minutes", "hours", "miles", "kilometers"];
        int chosenEntry = _random.Next(0, 4);
        return possibleUnits[chosenEntry];
    }
    static DateOnly GenerateRandomDate(DateOnly startDate, DateOnly endDate)
    {
        // Calculate the total number of days between the start and end dates
        int totalDays = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days;

        // Generate a random number of days to add to the start date
        int randomDays = _random.Next(0, totalDays + 1);

        // Return the new random date
        return startDate.AddDays(randomDays);
    }

    private static void UpdateEntry(SQLiteConnection connection)
    {
        Console.WriteLine("Enter the record ID would you like to edit, or E to exit");
        var idToEdit = Console.ReadLine()?.ToLower();
        var sanitizedIdToEdit = SanitizeNullOrWhiteSpace(idToEdit);
        if (IsExit(sanitizedIdToEdit)) return;

        while (!CheckEntryExists(connection, sanitizedIdToEdit))
        {
            Console.WriteLine("invalid entry please try again or press E to exit");
            idToEdit = Console.ReadLine()?.ToLower();
            sanitizedIdToEdit = SanitizeNullOrWhiteSpace(idToEdit);
            if (IsExit(sanitizedIdToEdit)) return;
        }

        Console.WriteLine("What part of the entry would you like to edit:");
        Console.WriteLine("\tEdit the Habit Date select 0 and press enter");
        Console.WriteLine("\tEdit the Habit Name select 1 and press enter");
        Console.WriteLine("\tEdit the Habit Quantity select 2 and press enter");
        Console.WriteLine("\tEdit the Habit Units select 3 and press enter");
        var selectColumnToEdit = Console.ReadLine()?.ToLower();

        while (string.IsNullOrWhiteSpace(selectColumnToEdit) || 
               !Regex.IsMatch(selectColumnToEdit, "^[eE0123]$"))
        {
            if (selectColumnToEdit?.ToLower() == "e")
            {
                Console.WriteLine("Exiting Update Option, press enter to continue");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("invalid entry please try again or press E to exit");
            selectColumnToEdit = Console.ReadLine()?.ToLower();
        }

        switch (selectColumnToEdit)
        {
            case "0":
                Console.WriteLine("\nEditing the Habit Date");
                Console.WriteLine("Enter the new Date, MM-DD-YYYY");
                var newHabitDate = Console.ReadLine();
                var sanitizedNewDate = SanitizeNullOrWhiteSpace(newHabitDate);
                if (IsExit(sanitizedNewDate)) return;
                var newDate = SanitizeDate(sanitizedNewDate);
                string updateQuery = "UPDATE habitsTable SET Date = @date WHERE Id = @id;";

                // Create a command object and pass the query and connection
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    // Add parameters to avoid SQL injection
                    command.Parameters.AddWithValue("@date", $"{newDate}");
                    command.Parameters.AddWithValue("@id", idToEdit);

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }

                break;
            case "1":
                Console.WriteLine("\nEditing the Habit Name");
                Console.WriteLine("Enter the new name");
                var newHabitName = Console.ReadLine();
                var sanitizedNewHabit = SanitizeNullOrWhiteSpace(newHabitName);
                if (IsExit(sanitizedNewHabit)) return;
                // SQL UPDATE command to update the habit with the given Id
                updateQuery = "UPDATE habitsTable SET HabitName = @habitName WHERE Id = @id;";

                // Create a command object and pass the query and connection
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    // Add parameters to avoid SQL injection
                    command.Parameters.AddWithValue("@habitName", $"{sanitizedNewHabit}");
                    command.Parameters.AddWithValue("@id", idToEdit);

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }

                break;
            case "2":
                Console.WriteLine("\nEditing the Habit Quantity");
                Console.WriteLine("Enter the new Quantity");
                var newQuantity = Console.ReadLine();
                var sanitizedQuantity = SanitizeQuantity(newQuantity);
                updateQuery = "UPDATE habitsTable SET Quantity = @quantity WHERE Id = @id;";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@quantity", $"{sanitizedQuantity}");
                    command.Parameters.AddWithValue("@id", idToEdit);

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }

                Console.WriteLine("quantity updated");
                break;
            case "3":
                Console.WriteLine("\nEditing the Habit Units");
                Console.WriteLine("Enter the new Unit");
                var newUnits = Console.ReadLine();
                var sanitizedNewUnits = SanitizeNullOrWhiteSpace(newUnits);
                if (IsExit(sanitizedNewUnits)) return;
                updateQuery = "UPDATE habitsTable SET Units = @units WHERE Id = @id;";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@units", $"{sanitizedNewUnits}");
                    command.Parameters.AddWithValue("@id", idToEdit);

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }

                break;
        }

        Console.WriteLine("Record updated, press enter to continue");
        Console.ReadLine();
    }

    private static void DeleteEntry(SQLiteConnection connection)
    {
        Console.WriteLine("Enter the record ID would you like to delete, or E to exit");
        var entry = Console.ReadLine()?.ToLower();

        while (string.IsNullOrWhiteSpace(entry) || !CheckEntryExists(connection, entry) || entry == "e")
        {
            if (entry == "e")
            {
                Console.WriteLine("Exiting Delete Option, press enter to continue");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("invalid entry please try again or press E to exit");
            entry = Console.ReadLine()?.ToLower();
        }

        Console.WriteLine($"Deleting record ID {entry}");
        string deleteRecord = $"DELETE FROM habitsTable WHERE Id == {entry};";

        using (SQLiteCommand command = new SQLiteCommand(deleteRecord, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    public static bool CheckEntryExists(SQLiteConnection connection, string id)
    {
        if (!int.TryParse(id, out _))
        {
            return false;
        }

        string query = "SELECT COUNT(*) FROM habitsTable WHERE Id = @id;";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);

            // ExecuteScalar returns the first column of the first row in the result set
            int count = Convert.ToInt32(command.ExecuteScalar());

            return count > 0;
        }

    }

    private static void ViewRecords(SQLiteConnection connection)
    {
        string selectDataQuery = "SELECT * FROM habitsTable;";
        using (SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection))
        {
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader["Id"],-4} " +
                                      $"Date: {reader["Date"],-20} " +
                                      $"HabitName: {reader["HabitName"],-20} " +
                                      $"Quantity: {reader["Quantity"],-10} " +
                                      $"Units: {reader["Units"],-10}");
                }
            }
        }
    }

    public class Habit
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string? HabitName { get; set; }
        public int Quantity { get; set; }
        public string? Units { get; set; }
    }

    private static void AddNewHabit(SQLiteConnection connection)
    {
        Console.WriteLine("Enter Date completed mm-dd-yyyy, or type 'E' to exit");
        string? dateEntry = Console.ReadLine();
        string date = SanitizeNullOrWhiteSpace(dateEntry);
        if (IsExit(date)) return;
        DateOnly? dateValue = SanitizeDate(dateEntry);

        Console.WriteLine("Enter Habit Name, or type 'E' to exit");
        string? habitNameEntry = Console.ReadLine();
        string habitName = SanitizeNullOrWhiteSpace(habitNameEntry);
        if (IsExit(habitName)) return;

        Console.WriteLine("Enter Quantity complete");
        string? quantityEntry = Console.ReadLine();
        var quantity = SanitizeQuantity(quantityEntry);

        Console.WriteLine("Enter type of Units tracked");
        string? unitsEntry = Console.ReadLine();
        string units = SanitizeNullOrWhiteSpace(unitsEntry);
        if (IsExit(units)) return;

        string insertDataQuery = $"INSERT INTO habitsTable (Date, HabitName, Quantity, Units) VALUES " +
                                 $"('{dateValue}', '{habitName}', {quantity}, '{units}');";
        using (SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection))
        {
            command.ExecuteNonQuery();
        }

        Console.WriteLine("New entry added, press enter to continue");
        Console.ReadLine();
    }

    private static DateOnly SanitizeDate(string? dateEntry)
    {
        while (true)
        {
            try
            {
                var dateValue = DateOnly.Parse(dateEntry!);
                Console.WriteLine("converted date to convention");
                return dateValue;
            }
            catch (Exception e)
            {
                Console.WriteLine("unable to convert date, please try again");
                Console.WriteLine("Enter Date completed mm-dd-yyyy, or type 'E' to exit");
                string? newDateEntry = Console.ReadLine();
                dateEntry = SanitizeNullOrWhiteSpace(newDateEntry);
                if (IsExit(dateEntry)) return DateOnly.MinValue;
            }
        }
    }

    private static int SanitizeQuantity(string? entry)
    {
        while (true)
        {
            if (int.TryParse(entry, out int validQuantity) && validQuantity > 0)
            {
                return validQuantity;
            }

            Console.WriteLine("Invalid Entry please enter a numerical quantity");
            entry = Console.ReadLine();
        }
    }

    private static bool IsExit(string entry)
    {
        if (entry.ToLower() == "e")
        {
            Console.WriteLine("Exiting to main menu, press enter to continue");
            Console.ReadLine();
            return true;
        }

        return false;
    }

    private static string SanitizeNullOrWhiteSpace(string? entryName)
    {
        while (string.IsNullOrWhiteSpace(entryName))
        {
            Console.WriteLine("invalid entry please try again or press E to exit");
            entryName = Console.ReadLine()?.ToLower();
        }

        return entryName;
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("What do you want to do?\n");
        Console.WriteLine("Type 0 to Close Application");
        Console.WriteLine("Type 1 to View all Records");
        Console.WriteLine("Type 2 to Add a record");
        Console.WriteLine("Type 3 to Delete a record");
        Console.WriteLine("Type 4 to Edit a record");
    }
}