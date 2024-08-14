using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Globalization;

string? userMenuChoice;
bool closeApp = false;

string? habit = "";
string? unitOfMeasure = "";

string connectionString = @"Data Source=HabitLogger.db";

string? userInput;

List<string> habitList = new List<string>();
List<string> habitUnitsList = new List<string>();
int habitIndex = 0;

// Function for user to select from list of habits
void ViewHabitsLogged()
{
    Console.WriteLine("\nThese are the habits you currently have logged:");

    int habitNum = 1;

    habitList.Clear();
    habitUnitsList.Clear();

    try
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table';", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string? habitName = reader["name"].ToString();

                        // Skip the sqlite_sequence table
                        if (habitName == "sqlite_sequence")
                        {
                            continue;
                        }

                        if (habitName != null)
                        {
                            habitList.Add(habitName);
                            Console.WriteLine($"{habitNum}) {habitName}");
                            habitNum++;

                            using (SQLiteCommand columnCommand = new SQLiteCommand($"PRAGMA table_info({habitName})", connection))
                            {
                                using (SQLiteDataReader columnReader = columnCommand.ExecuteReader())
                                {
                                    while (columnReader.Read())
                                    {
                                        string? columnName = columnReader["name"].ToString();
                                        if (columnName != "Id" && columnName != "Date" &&  columnName != "Quantity")
                                        {
                                            unitOfMeasure = columnName;
                                            break;
                                        }
                                    }
                                    habitUnitsList.Add(unitOfMeasure!);
                                }
                            }
                        }

                    }
                }
            }
        }
    }
    catch (SQLiteException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

// Function to show options for list of habits
void HabitsListResult()
{
    if (habitList.Count < 1)
    {
        Console.WriteLine("You have no habits logged yet.");
        CreateHabit();
    }
    else
    {
        Console.WriteLine("\nEnter the number for the habit you want to track, or enter 0 to create a new habit:");

        userInput = Console.ReadLine();

        if (int.TryParse(userInput, out habitIndex) && habitIndex > 0 && habitIndex <= habitList.Count)
        {
            habitIndex -= 1;
            habit = habitList[habitIndex];
            UserSelectMenuOption(habit);
        }
        else if (userInput == "0")
        {
            CreateHabit();
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again.");
            HabitsListResult();
        }
    }
}

// Function to create new habit
void CreateHabit()
{
    do
    {
        Console.WriteLine("\nWhat habit would you like to track?");
        habit = Console.ReadLine();
        if (habit != null)
        {
            habit = habit.Trim();
            habit = habit.ToLower();
            habit = habit.Replace(" ", "_");
        }
    } while (habit == null);

    do
    {
        Console.WriteLine("\nWhat unit of measure would you like to use (e.g., glasses of water, hours of sleep, miles run, etc.)?");
        unitOfMeasure = Console.ReadLine();
        if (unitOfMeasure != null)
        {
            unitOfMeasure = unitOfMeasure.Trim();
            unitOfMeasure = unitOfMeasure.ToLower();
            unitOfMeasure = unitOfMeasure.Replace(" ", "_");
        }
    } while (unitOfMeasure == null);

    habitUnitsList.Add(unitOfMeasure);

    using (var connection = new SQLiteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {habit} (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity REAL,
                                {unitOfMeasure} TEXT
                                )";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
}

// Function for Main Menu
void UserSelectMenuOption(string habit)
{
    //Console.Clear();
    Console.WriteLine($"\nHABIT - {habit}\n");
    Console.WriteLine("What would you like to do?\n");
    Console.WriteLine("Type 0 to Close Application.");
    Console.WriteLine("Type 1 to View All Records.");
    Console.WriteLine("Type 2 to Insert Record.");
    Console.WriteLine("Type 3 to Delete Record.");
    Console.WriteLine("Type 4 to Update Record.");
    Console.WriteLine("Type 5 to Delete This Habit.");
    Console.WriteLine("Type 6 to View or Create Other Habits.\n");
    string[] menuOptions = { "0", "1", "2", "3", "4", "5", "6" };
    do
    {
        userMenuChoice = Console.ReadLine();
        if (!menuOptions.Contains(userMenuChoice))
        {
            Console.WriteLine("That is not a valid option. You must enter 0, 1, 2, 3, 4, 5, or 6");
        }
    } while (!menuOptions.Contains(userMenuChoice));

    switch (userMenuChoice)
    {
        case "0":
            Console.WriteLine("Goodbye!");
            closeApp = true;
            Environment.Exit(0);
            break;
        case "1":
            ViewAllRecords(habit);
            break;
        case "2":
            InsertRecord(habit);
            break;
        case "3":
            DeleteRecord(habit);
            break;
        case "4":
            UpdateRecord(habit);
            break;
        case "5":
            DeleteHabit(habit);
            ViewHabitsLogged();
            HabitsListResult();
            break;
        case "6":
            ViewHabitsLogged();
            HabitsListResult();
            break;
    }
}

// Function to view all records (1)
void ViewAllRecords(string habit)
{
    using (var connection = new SQLiteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            $"SELECT * FROM {habit}";

        List<HabitBeingLogged> tableData = new();

        SQLiteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                new HabitBeingLogged
                {
                    ID = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "M-d-yy", new CultureInfo("en-US")),
                    Quantity = reader.GetDecimal(2)
                });
            }
        }
        else
        {
            Console.WriteLine("\nNo rows found");
        }

        connection.Close();

        string habitUnit = habitUnitsList[habitIndex];

        Console.WriteLine("------------------------------------------------------------------");
        Console.WriteLine($"Habit Tracked: {habit}; Unit of Measure: {habitUnit}");
        foreach (var habitRow in tableData)
        {
            Console.WriteLine($"{habitRow.ID} - {habitRow.Date.ToString("MMM-dd-yyyy")} - Quantity: {habitRow.Quantity}");
        }
        Console.WriteLine("------------------------------------------------------------------\n");
    }
}

// Function to insert record (2)
void InsertRecord(string habit)
{
    Console.WriteLine("Please insert the date: (Format: mm-dd-yy). Type 0 to return to main menu:");

    string? dateInput = Console.ReadLine();

    while ((!DateTime.TryParseExact(dateInput, "MM-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _)) && (dateInput != "0"))
    {
        Console.WriteLine("\nInvalid date. (Format: mm-dd-yy). Try again or type 0 to return to main menu:\n");
        dateInput = Console.ReadLine();
    }

    if (dateInput == "0")
    {
        return;
    }

    string date = "";

    if (dateInput != null)
    {
        date = dateInput;
    }

    using (var connection = new SQLiteConnection(connectionString))
    {
        connection.Open();
        using (SQLiteCommand command = new SQLiteCommand($"PRAGMA table_info({habit})", connection))
        {
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    unitOfMeasure = reader["name"].ToString();
                    if (unitOfMeasure != "Id" && unitOfMeasure != "Date" && unitOfMeasure != "Quantity")
                    {
                        Console.WriteLine($"Please insert {unitOfMeasure} achieved:");
                    }
                }
            }
        }
    }

    string? numberInput = Console.ReadLine();

    while (Convert.ToDecimal(numberInput) < 0)
    {
        Console.WriteLine("\nInvalid number. Enter a positve number.");
        numberInput = Console.ReadLine();
    }

    decimal finalInput = Convert.ToDecimal(numberInput);

    decimal quantity = finalInput;

    using (var connection = new SQLiteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            $"INSERT INTO {habit}(date, quantity) VALUES('{date}', {quantity})";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
}

// Function to delete record (3)
void DeleteRecord(string habit)
{
    ViewAllRecords(habit);

    Console.WriteLine("Please type the ID of the record you want to delete or type 0 to return to Main Menu\n");

    string? idInput = Console.ReadLine();

    if (idInput == "0")
    {
        return;
    }

    while (!Int32.TryParse(idInput, out _) || Convert.ToInt32(idInput) < 0)
    {
        Console.WriteLine("\nInvalid number, try again.\n");
        idInput = Console.ReadLine();
    }

    var recordID = Convert.ToInt32(idInput);

    using (var connection = new SQLiteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"DELETE from {habit} WHERE ID = '{recordID}'";

        int rowCount = tableCmd.ExecuteNonQuery();

        if (rowCount == 0)
        {
            Console.WriteLine($"\nRecord with ID {recordID} doesn't exist.\n");
            DeleteRecord(habit);
        }
    }

    Console.WriteLine($"\nRecord with ID {recordID} was deleted.\n");

    return;
}

// Function to update record (4)
void UpdateRecord(string habit)
{
    while (true)
    { 
        ViewAllRecords(habit);

        Console.WriteLine("Please type the ID of the record you would like to update. Type 0 to return to Main Menu.");

        string? idInput = Console.ReadLine();

        if (idInput == "0")
        {
            return;
        }

        while (!Int32.TryParse(idInput, out _) || Convert.ToInt32(idInput) < 0)
        {
            Console.WriteLine("\nInvalid number, try again.\n");
            idInput = Console.ReadLine();
        }

        var recordID = Convert.ToInt32(idInput);

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habit} WHERE ID = {recordID})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\nRecord with ID {recordID} doesn't exist.\n");
                continue;
            }

            Console.WriteLine("Please insert the date: (Format: mm-dd-yy). Type 0 to return to main menu");

            string? dateInput = Console.ReadLine();

            while ((!DateTime.TryParseExact(dateInput, "MM-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _)) && (dateInput != "0"))
            {
                Console.WriteLine("\nInvalid date. (Format: mm-dd-yy). Type 0 to return to main menu.\n");
                dateInput = Console.ReadLine();
            }

            if (dateInput == "0")
            {
                return;
            }

            string date = "";

            if (dateInput != null)
            {
                date = dateInput;
            }

            Console.WriteLine($"Please insert {unitOfMeasure} or other measure of your choice (no decimals allowed)");

            string? numberInput = Console.ReadLine();

            while (!Int32.TryParse(numberInput, out _) || (Convert.ToInt32(numberInput) < 0))
            {
                Console.WriteLine("\nInvalid number. Enter a whole number of 0 or greater.");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            int quantity = finalInput;

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE {habit} SET date = '{date}', quantity = {quantity} WHERE ID = {recordID}";

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            tableCmd.ExecuteNonQuery();

            connection.Close();

            Console.WriteLine($"\nRecord with ID {recordID} was updated.");

            break;
        }
    }
}

void DeleteHabit(string habit)
{
    Console.WriteLine($"Are you sure you want to delete the {habit} habit and all of its records? (y/n)");
    userInput = Console.ReadLine();
    userInput = userInput!.Trim().ToLower();
    if ((userInput != null) && (userInput == "y" || userInput == "yes"))
    {
        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"DROP TABLE IF EXISTS {habit}";
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    else
    {
        return;
    }
    
}


do
{
    ViewHabitsLogged();
    HabitsListResult();
    UserSelectMenuOption(habit);
} while (closeApp == false);


public class HabitBeingLogged
{
    public int ID { get; set; }

    public DateTime Date { get; set; }

    public decimal Quantity { get; set; }

}