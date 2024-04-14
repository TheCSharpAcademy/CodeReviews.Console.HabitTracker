using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

bool isRunning = true;
string connectionString = "Data Source=habit_logger.db";
Console.Clear();
Console.WriteLine("C# Academy #3 - Habit Logger");
if (!CheckDatabase(connectionString, false))
    isRunning = false;
while (isRunning)
{
    Console.WriteLine("\nMAIN MENU");
    DisplayCurrentHabit(connectionString);
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("-------------------------------------------------------");
    Console.WriteLine("[Type 0] to Close Application.");
    Console.WriteLine("[Type 1] to Add a New Habit.");
    Console.WriteLine("[Type 2] to Delete an Existing Habit.");
    Console.WriteLine("[Type 3] to Update an Existing Habit.");
    Console.WriteLine("[Type 4] to List all Existing Habits.");
    Console.WriteLine("[Type 5] to Set current Habit.");
    if (HasCurrentHabit(connectionString))
    {
        Console.WriteLine($"[Type 6] to Add a new log entry for '{GetCurrentHabit(connectionString)}'.");
        Console.WriteLine($"[Type 7] to Delete an existing log entry for '{GetCurrentHabit(connectionString)}'.");
        Console.WriteLine($"[Type 8] to Update an existing log entry for '{GetCurrentHabit(connectionString)}'.");
        Console.WriteLine($"[Type 9] to List all log entries for '{GetCurrentHabit(connectionString)}'.");
    }
    else
    {
        Console.WriteLine("[6-9 LOCKED] until working habit is chosen.");
    }
    Console.WriteLine("[Type 10] for log reports.");
    Console.WriteLine("[Type 11] to Reset the database.");
    Console.WriteLine("-------------------------------------------------------\n");
    Console.Write("Choose option: ");
    string? command = Console.ReadLine();
    switch (command)
    {
        case "0":
            Console.WriteLine("\nGoodbye!\n");
            isRunning = false;
            break;
        case "1":
            AddNewHabit(connectionString);
            break;
        case "2":
            DeleteExistingHabit(connectionString);
            break;
        case "3":
            UpdateExistingHabit(connectionString);
            break;
        case "4":
            ListAllHabits(connectionString);
            break;
        case "5":
            SelectCurrentHabit(connectionString);
            break;
        case "6":
            if (HasCurrentHabit(connectionString)) { AddNewHabitLog(connectionString); } else { Console.WriteLine("\nInvalid Command.\n"); }
            break;
        case "7":
            if (HasCurrentHabit(connectionString)) { DeleteHabitLog(connectionString); } else { Console.WriteLine("\nInvalid Command.\n"); }
            break;
        case "8":
            if (HasCurrentHabit(connectionString)) { UpdateHabitLog(connectionString); } else { Console.WriteLine("\nInvalid Command.\n"); }
            break;
        case "9":
            if (HasCurrentHabit(connectionString)) 
            { 
                ListAllHabitLogsForHabit(connectionString, GetCurrentHabitID(connectionString)); 
            } 
            else 
            { 
                Console.WriteLine("\nInvalid Command.\n"); 
            }
            break;
        case "10":
            Reports(connectionString);
            break;
        case "11":
            Console.WriteLine("CAUTION: This will delete ALL data in the habit logger and reset to it's initial state.");
            CheckDatabase(connectionString, true);
            break;
        default:
            Console.WriteLine("\nInvalid Command - please try again.\n");
            break;
    }
}
Environment.Exit(0);

static bool CheckDatabase(string cs, bool forceReset)
{
    int creationCount = 0;
    creationCount += TableExists(cs, "current_habit");
    creationCount += TableExists(cs, "habits");
    creationCount += TableExists(cs, "habit_logs");
    if (creationCount < 3 || forceReset)
    {
        Console.WriteLine("The database need to be initialised. Any existing data will be lost. If this is your first use, choose 'y'");
        if (GetValidYesNoResponse("OK to proceed (Y/N)?") == 'N')
        {
            return false;
        }
        else
        {
            Console.WriteLine("Resetting the database...please wait.");
            ExecuteQuery(cs, "DROP TABLE IF EXISTS current_habit");
            ExecuteQuery(cs, "DROP TABLE IF EXISTS habits");
            ExecuteQuery(cs, "DROP TABLE IF EXISTS habit_logs");
            ExecuteQuery(cs, @"CREATE TABLE IF NOT EXISTS habit_logs (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date_Logged TEXT, Quantity INTEGER, Type INTEGER)");
            ExecuteQuery(cs, @"CREATE TABLE IF NOT EXISTS habits (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Units TEXT)");
            ExecuteQuery(cs, @"CREATE TABLE IF NOT EXISTS current_habit (Id INTEGER)");
            if (GetValidYesNoResponse("This is a freshly created database. Do you wish to populate it with test data (Y/N)?") == 'Y')
            {
                Console.WriteLine("Seeding test data, this might take some time...please wait.");
                SeedTestData(cs);
                Console.WriteLine("\nTest data seeding is COMPLETE. Please see habit and habit log lists. No current habit status has been set.");
            }
        }
    }
    return true;
}

static void SeedTestData(string cs)
{
    int habitNumber = 3;
    Random random = new Random();
    string[] habitNameData = { "Drinking water", "Exercise", "Mindfulness" };
    string[] habitUnitsData = { "glasses", "workouts", "sessions" };
    string randomDate = string.Empty;
    int randomAmount = 0;
    Habit currentHabit = null;
    for (int j = 0; j < habitNumber; j++)
    {
        Console.Write($"Inserting test habit {habitNameData[j]}...");
        ExecuteQuery(cs, $"INSERT INTO habits(Name, Units) VALUES ('{habitNameData[j]}', '{habitUnitsData[j]}')");
        Console.Write($"DONE.\nInserting log records for {habitNameData[j]}...\n");
        currentHabit = GetHabitFromName(cs, $"{habitNameData[j]}");
        for (int i = 0; i < 33; i++)
        {
            do
            {
                Console.WriteLine("Getting non-duplicate date...");
                randomDate = GetRandomDate(2023, 2024);
            } while (CheckDuplicateHabitLogDate(cs, randomDate, currentHabit.Id));
            randomAmount = random.Next(1, 31);
            Console.Write($"{i} - Inserting {randomDate} - {randomAmount} for habit [{currentHabit.Id}]{currentHabit.Name}...");
            ExecuteQuery(cs, $"INSERT INTO habit_logs(Date_Logged, Quantity, Type) VALUES ('{randomDate}', {randomAmount}, {currentHabit.Id})");
            Console.Write("DONE\n");
        }
    }
}

static void Reports(string cs)
{
    bool isReportMenu = true;
    while (isReportMenu)
    {
        Console.WriteLine("\nREPORTS MENU");
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine("[Type 0] to Return to the Man Menu.");
        Console.WriteLine("[Type 1] to show all habits for the last 30 days");
        Console.Write("\nPlease choose an option: ");
        string? command = Console.ReadLine();
        switch (command)
        {
            case "0":
                isReportMenu = false;
                break;
            case "1":
                Report30Days(cs);
                break;
            default:
                Console.WriteLine("\nInvalid command - please try again\n");
                break;
        }
    }
}

static void Report30Days(string cs)
{
    Console.Clear();
    string query = "SELECT * FROM habit_logs WHERE Date_Logged BETWEEN strftime('%Y-%m-%d', 'now', '-30 days') AND strftime('%Y-%m-%d', 'now')";
    List<HabitLog> last30days = GetHabitLogResults(cs, query);
    if (last30days.Count > 0)
    {
        foreach (var habitlog in last30days)
        {
            Habit habit = GetHabitFromId(cs, habitlog.Type);
            DateTime parsedDate = DateTime.ParseExact(habitlog.Date.ToString(), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            string formattedDate = parsedDate.ToString("dd-MM-yy");
            Console.WriteLine($"{habitlog.Id} - {formattedDate} - {habit.Name}, {habitlog.Quantity} {habit.Units}");
        }
    }
    else
    {
        Console.WriteLine("No habit logs found for the last 30 days");
    }
}

static void AddNewHabitLog(string cs)
{
    Console.Clear();
    Habit currentHabit = GetHabitFromId(cs, GetCurrentHabitID(cs));
    string? date;
    date = GetLogDateInput();
    if (!CheckDuplicateHabitLogDate(cs, date, currentHabit.Id))
    {
        if (date == null) return;
        int quantity = GetNumberInput($"Please insert number of {currentHabit.Units} (no decimals allowed)");
        if (quantity.Equals(-1)) return;
        ExecuteQuery(cs, $"INSERT INTO habit_logs(Date_Logged, Quantity, Type) VALUES ('{date}', {quantity}, {currentHabit.Id})");
    } 
    else
    {
        Console.WriteLine($"There is already an existing log for the habit '{currentHabit.Name}' for date {date}. You probably want to UPDATE rather than ADD.");
        AddNewHabitLog(cs);
    }
}

static void DeleteHabitLog(string cs)
{
    Console.Clear();
    int? habit = GetCurrentHabitID(cs);
    ListAllHabitLogsForHabit(cs, habit);
    var recordId = GetNumberInput("Please type the Id of the record you want to delete or type 0 to go back to Main Menu");
    if (recordId.Equals(-1)) return;
    int numDeleted = ExecuteQuery(cs, $"DELETE from habit_logs WHERE Id = '{recordId}'");
    if (numDeleted == 0)
    {
        Console.WriteLine($"\nRecord with Id {recordId} doesn't exist. \n");
        DeleteHabitLog(cs);
    }
}

static void UpdateHabitLog(string cs)
{
    Console.Clear();
    Habit currentHabit = GetHabitFromId(cs, GetCurrentHabitID(cs));
    ListAllHabitLogsForHabit(cs, currentHabit.Id);
    var recordId = GetNumberInput("Please type the Id of the record you like to update or type 0 to go back to Main Menu");
    if (recordId.Equals(-1)) return;
    if (DoesHabitLogExist(cs, recordId))
    {
        string? date = GetHabitLogDateInput();
        if (date == null) return;
        if (!CheckDuplicateHabitLogDate(cs, date, currentHabit.Id))
        {
            int quantity = GetNumberInput($"Please enter number of {currentHabit.Units} (no decimals allowed)");
            if (quantity.Equals(-1)) return;
            ExecuteQuery(cs, $"UPDATE habit_logs SET Date_Logged = '{date}', Quantity = {quantity} WHERE Id = {recordId} AND Type = {currentHabit.Id}");
        }
        else
        {
            Console.WriteLine($"This date clashes with an existing log entry date {date} for the habit of {currentHabit.Name}");
            UpdateHabitLog(cs);
        }
    }
    else
    {
        Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
        UpdateHabitLog(cs);
    }
}

static string? GetHabitLogDateInput()
{
    Console.WriteLine("\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to Main Menu.\n");
    string? dateInput = Console.ReadLine();
    if (dateInput == "0") return null;
    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("\nInvalid date. (Format: dd-mm-yy). Type 0 to return to Main Menu or try again:\n");
        dateInput = Console.ReadLine();
    }
    return dateInput;
}

static void AddNewHabit(string cs)
{
    bool isDuplicateHabit = true;
    string? name;
    string? units;
    do
    {
        name = GetHabitNameInput(cs);
        if (name == null) return;
        isDuplicateHabit = CheckDuplicateHabit(cs, name);
        if (isDuplicateHabit)
        {
            Console.WriteLine("This habit already exists! Perhaps you wanted to Update instead?");
        }
    } while (isDuplicateHabit);
    units = GetHabitUnitsInput();
    if (units == null) return;
    ExecuteQuery(cs, $"INSERT INTO habits(Name, Units) VALUES ('{name}', '{units}')");
    if (GetValidYesNoResponse("Do you want to set the new habit as the current working habit (Y/N)? ") == 'Y')
    {

        Habit currentHabit =  GetHabitFromName(cs, name);
        SetCurrentHabit(cs, currentHabit.Id);
    }
}

static void DeleteExistingHabit(string cs)
{
    ListAllHabits(cs);
    var recordId = GetNumberInput("Please type the Id of the record you want to delete or type 0 to go back to Main Menu");
    if (recordId.Equals(-1)) return;
    Console.WriteLine("WARNING: Deleting a habit will also delete any logs associated with this habit!");
    if (GetValidYesNoResponse("Do you still want to delete this habit (Y/N)?") == 'Y')
    {
        int numDeleted = ExecuteQuery(cs, $"DELETE from habits WHERE Id = '{recordId}'");
        if (numDeleted == 0)
        {
            Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.\n");
            DeleteExistingHabit(cs);
        }
        ExecuteQuery(cs, $"DELETE from habit_logs WHERE Type = '{recordId}'");
        numDeleted = ExecuteQuery(cs, $"DELETE from current_habit WHERE Id = '{recordId}'");
        if (numDeleted > 0)
        {
            Console.WriteLine("Warning: You have deleted the habit which was the currently active habit. This will set set to null and you'll need to select or create another");
            SetCurrentHabit(cs, null);
        }
    }
}

static void UpdateExistingHabit(string cs)
{
    ListAllHabits(cs);
    var recordId = GetNumberInput("Please type the Id of the habit you like to update or type 0 to go back to Main Menu");
    if (recordId.Equals(-1)) return;
    if (DoesHabitExist(cs, recordId))
    {
        string? name = GetHabitNameInput(cs);
        if (name == null) return;
        string? units = GetHabitUnitsInput();
        if (units.Equals(null)) return;
        ExecuteQuery(cs, $"UPDATE habits SET Name = '{name}', Units = {units} WHERE Id = {recordId}");
    }
    else
    {
        Console.WriteLine($"\nHabit with Id {recordId} doesn't exist.\n\n");
        UpdateExistingHabit(cs);
    }
}

static void ListAllHabits(string cs)
{
    Console.Clear();
    Console.WriteLine("List of current habits stored:-");
    List<Habit> habits = GetHabitResults(cs, "SELECT * FROM habits");
    if (habits.Count > 0)
    {
        foreach (var habit in habits)
        {
            Console.WriteLine($"{habit.Id} - {habit.Name} using unit of measurement of '{habit.Units}'");
        }
    }
    else
    {
        Console.WriteLine("No habits stored");
    }
    Console.WriteLine("Press enter to continue");
    Console.ReadLine();
}

static void SetCurrentHabit(string cs, int? Id)
{
    ExecuteQuery(cs, "DELETE FROM current_habit");
    Habit newCurrentHabit = GetHabitFromId(cs, Id);
    if (newCurrentHabit != null)
    {
        ExecuteQuery(cs, $"INSERT INTO current_habit(Id) VALUES ({newCurrentHabit.Id})");
    }
}

static void SelectCurrentHabit(string cs)
{
    ListAllHabits(cs);
    var recordId = GetNumberInput("Please type the Id of the habit you'd like to select as the current working habit or type 0 to go back to Main Menu");
    if (recordId.Equals(-1)) return;
    if (DoesHabitExist(cs, recordId))
    {
        SetCurrentHabit(cs, recordId);
    }
    else
    {
        Console.WriteLine($"\nHabit with Id {recordId} doesn't exist.\n");
        SelectCurrentHabit(cs);
    }
}

static void DisplayCurrentHabit(string cs)
{
    string? currentHabit = GetCurrentHabit(cs);
    if (currentHabit == null ) currentHabit = "NO HABIT SELECTED";
    Console.WriteLine("-------------------------------------------------------");
    Console.WriteLine($"The currently selected habit is: {currentHabit}");
    Console.WriteLine("-------------------------------------------------------\n");
}

static string? GetCurrentHabit(string cs)
{
    string? currentHabitName = null;
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT habits.Name FROM habits WHERE habits.Id = (SELECT Id FROM current_habit);";
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                currentHabitName = reader["Name"].ToString();
            }
            else
            {
                currentHabitName = null;
            }
        }
    }
    return currentHabitName;
}

static int? GetCurrentHabitID(string cs)
{
    int? currentHabitId = null;
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Id FROM current_habit";
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                currentHabitId = reader.GetInt32(0);
            }
            else
            {
                currentHabitId = null;
            }
        }
    }
    return currentHabitId;
}

static bool DoesHabitExist(string cs, int id)
{
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id = {id})";
            int record = Convert.ToInt32(command.ExecuteScalar());
            return record > 0 ? true : false;
        }
    }
}

static bool DoesHabitLogExist(string cs, int id)
{
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_logs WHERE Id = {id})";
            int record = Convert.ToInt32(command.ExecuteScalar());
            return record > 0 ? true : false;
        }
    }
}

static int ExecuteQuery(string cs, string query)
{
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            return command.ExecuteNonQuery();
        }
    }
}

static int TableExists(string cs, string tableName)
{
    int result = 0;
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '{tableName}'";
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            reader.Close();
        }
    }
    return result;
}

static void ListAllHabitLogsForHabit(string cs, int? htype)
{
    if (!htype.HasValue)
    {
        Console.WriteLine("ERROR: Current habit cannot be determined");
        return;
    }
    Habit currentHabit = GetHabitFromId(cs, htype);
    List<HabitLog> habitLogs = GetHabitLogResults(cs, $"SELECT * FROM habit_logs WHERE Type = {currentHabit.Id}");
    Console.WriteLine($"\nHabit logs stored for the habit: {currentHabit.Name}");
    if (habitLogs.Count > 0)
    {
        foreach (var habitLog in habitLogs)
        {
            Console.WriteLine($"{habitLog.Id} - {habitLog.Date.ToString("dd-MM-yy")} - Quantity: {habitLog.Quantity} {currentHabit.Units}");
        }
    }
    else
    {
        Console.WriteLine($"No habit logs stored for the habit: {currentHabit.Name}");
    }
    Console.WriteLine("Press enter to continue");
    Console.ReadLine();
}

static List<Habit> GetHabitResults(string cs, string query)
{
    List<Habit> tableData = new();
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Units = reader.GetString(2)
                    });
                }
            }
            reader.Close();
        }
    }
    return tableData;
}

static List<HabitLog> GetHabitLogResults(string cs, string query)
{
    List<HabitLog> tableData = new();
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new HabitLog
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Quantity = reader.GetInt32(2),
                        Type = reader.GetInt32(3)
                    });
                }
            }
            reader.Close();
        }
    }
    return tableData;
}

static bool HasCurrentHabit(string cs)
{
    int result = 0;
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT COUNT(*) FROM current_habit";
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            reader.Close();
        }
    }
    return result > 0;
}

static Habit GetHabitFromName(string cs, string name)
{
    List<Habit> tableData = new();
    tableData = GetHabitResults(cs, $"SELECT Id, Name, Units FROM habits WHERE Name ='{name}'");
    return Habit.CreateNewHabit(tableData[0].Id, tableData[0].Name, tableData[0].Units);
}

static Habit GetHabitFromId(string cs, int? id)
{
    List<Habit> tableData = new();
    if (id.HasValue)
    {
        tableData = GetHabitResults(cs, $"SELECT Id, Name, Units FROM habits WHERE Id ={id}");
        return Habit.CreateNewHabit(tableData[0].Id, tableData[0].Name, tableData[0].Units);
    }
    else return null;
}

static bool CheckDuplicateHabit(string cs, string habitname)
{
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE LOWER(Name) = '{habitname.ToLower()}')";
            string? name = command.ExecuteScalar().ToString();
            return name == null ? true : false;
        }
    }
}

static string? GetLogDateInput()
{
    Console.Write("Please insert the date: (Format: dd-mm-yy). Type 0 to return to Main Menu: ");
    string? dateInput = Console.ReadLine();
    if (dateInput == "0") return null;
    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
    {
        Console.Write("\nInvalid date. (Format: dd-mm-yy). Type 0 to return to Main Menu or try again: ");
        dateInput = Console.ReadLine();
    }
    return dateInput;
}

static int GetNumberInput(string message)
{
    Console.WriteLine(message);
    string numberInput = Console.ReadLine();
    if (numberInput == "0") return -1;
    while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
    {
        Console.WriteLine("\n\nInvalid number. Try again.\n\n");
        numberInput = Console.ReadLine();
    }
    int finalInput = Convert.ToInt32(numberInput);
    return finalInput;
}

static string? GetHabitNameInput(string cs)
{
    String? nameInput = String.Empty;
    do
    {
        Console.Write("Please insert the name of the new habit. Type 0 to return to Main Menu: ");
        nameInput = Console.ReadLine();
        if (nameInput == "0") return null;
        if (nameInput != null) 
            if (CheckDuplicateHabit(cs, nameInput)) Console.WriteLine("\nInvalid habit name. Please enter a sensible habit name that is not already present.\n");
    } while (nameInput == null || nameInput.Trim().Equals("") || nameInput.Equals(String.Empty));
    return nameInput;
}

static string? GetHabitUnitsInput()
{
    String? unitsInput = String.Empty;
    do
    {
        Console.Write("Please insert the units measurement of the new habit (e.g. glasses of water). Type 0 to return to Main Menu: ");
        unitsInput = Console.ReadLine();
        if (unitsInput == "0") return null;
    } while (unitsInput == null || unitsInput.Trim().Equals("") || unitsInput.Equals(String.Empty));
    return unitsInput;
}

static char GetValidYesNoResponse(string question)
{
    bool isValid = true;
    string? response;
    do
    {
        Console.Write(question + ": ");
        response = Console.ReadLine();
        if (response == null || !Regex.IsMatch(response.ToLower().Trim(), "^[ynYN]$"))
        {
            isValid = false;
            Console.WriteLine("Please enter 'y' or 'n'");
        }

    } while (!isValid);
    return Convert.ToChar(response.ToUpper());
}

static bool CheckDuplicateHabitLogDate(string cs, string date, int habitId)
{
    using (var connection = new SqliteConnection(cs))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_logs WHERE Date_Logged = '{date}' AND Type = {habitId})";
            int record = Convert.ToInt32(command.ExecuteScalar());
            return record > 0 ? true : false;
        }
    }
}

static string GetRandomDate(int startYear, int endYear)
{
    Random random = new Random();
    DateTime startDate = new DateTime(startYear, 1, 1);
    DateTime endDate = new DateTime(endYear, 12, 31);
    TimeSpan timeSpan = endDate - startDate;
    TimeSpan randomSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
    DateTime randomDate = startDate + randomSpan;
    return randomDate.ToString("yyyy-MM-dd");
}

public class HabitLog
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public int Type { get; set; }
}

public class Habit
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Units { get; set; }

    // Static method to create a new instance of Habit
    public static Habit CreateNewHabit(int id, string name, string units)
    {
        return new Habit
        {
            Id = id,
            Name = name,
            Units = units
        };
    }
}