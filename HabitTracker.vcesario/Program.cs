using System.Runtime.Intrinsics.Arm;
using Microsoft.Data.Sqlite;

using (var connection = new SqliteConnection("Data Source=habittracker.db"))
{
    connection.Open();

    InitializeDBs(connection);

    int menuOption = -1;
    do
    {
        menuOption = AskMenuOption();

        switch (menuOption)
        {
            case 1:
                ViewAllEntries(connection);
                break;
            case 2:
                LogNewEntry(connection);
                break;
            case 5:
                AddHabitDefinitionScreen(connection);
                break;
            case 9:
                FillDbWithRandomEntries(connection);
                break;
            default:
                break;
        }

    } while (menuOption > 0);


    /* Sqlite Snippet for reference
    
    // var command = connection.CreateCommand();
    // command.CommandText =
    // @"
    //     SELECT *
    //     FROM user
    //     WHERE id = $id
    // ";
    // command.Parameters.AddWithValue("$id", 1);

    // using (var reader = command.ExecuteReader())
    // {
    //     while (reader.Read())
    //     {
    //         var name = reader.GetString(0);

    //         Console.WriteLine($"Hello, {name}!");
    //     }
    // }
    */
}

void InitializeDBs(SqliteConnection connection)
{
    // check if habitdefs exist. if not, creates it
    try
    {
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM habitdefs";

        command.ExecuteReader();

        Console.WriteLine("Table 'habitdefs' found.");
    }
    catch (SqliteException) // assuming this will happen only in case table doesn't exist
    {
        var command = connection.CreateCommand();
        command.CommandText = @"CREATE TABLE habitdefs(
                                    habit_name VARCHAR(20) PRIMARY KEY,
                                    unit VARCHAR(10) NOT NULL
                                )";

        command.ExecuteReader();

        Console.WriteLine("Table 'habitdefs' created.");
    }

    try
    {
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM habitlogs";

        command.ExecuteReader();

        Console.WriteLine("Table 'habitlogs' found.");
    }
    catch (SqliteException)
    {
        var command = connection.CreateCommand();
        command.CommandText = @"CREATE TABLE habitlogs(
                                    id INT PRIMARY KEY,
                                    habit VARCHAR(20) NOT NULL,
                                    date DATE NOT NULL,
                                    measure INT NOT NULL,
                                    FOREIGN KEY (habit) REFERENCES habitdefs(habit_name)
                                        ON DELETE CASCADE
                                        ON UPDATE CASCADE,
                                    UNIQUE (habit, date)
                                )";

        command.ExecuteReader();

        Console.WriteLine("Table 'habitlogs' created.");
    }

    Console.ReadLine();
}

int AskMenuOption()
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("1. View all entries");
    Console.WriteLine("2. Log new entry");
    Console.WriteLine("3. Edit existing entry");
    Console.WriteLine("4. Remove entry");
    Console.WriteLine("5. Add new habit definition");
    Console.WriteLine("6. Edit existing definition");
    Console.WriteLine("7. Remove definition");
    Console.WriteLine("8. See statistics");
    Console.WriteLine("9. [DEBUG] Fill with random entries");
    Console.WriteLine("0. Exit");
    Console.WriteLine();

    Console.Write("Choose an option: ");
    string? input = Console.ReadLine();
    int intInput = -1;
    while (input == null || int.TryParse(input, out intInput) == false || intInput < 0 || intInput > 9)
    {
        Console.Write("Invalid option. Try a digit between 0 and 9: ");
        input = Console.ReadLine();
    }

    return intInput;
}

void ViewAllEntries(SqliteConnection connection)
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("> VIEW ALL ENTRIES");
    Console.WriteLine();

    Console.WriteLine("> Habit definitions");
    Console.WriteLine();

    var command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM habitdefs";
    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            var habitName = reader.GetString(0);
            var unit = reader.GetString(1);

            Console.WriteLine($"{habitName} (in {unit})");
        }
    }

    Console.WriteLine();

    Console.WriteLine("> Habit entries");
    Console.WriteLine();

    command.CommandText = @"SELECT habitlogs.habit, habitlogs.date, habitlogs.measure, habitdefs.unit
                            FROM habitlogs
                            INNER JOIN habitdefs ON habitlogs.habit=habitdefs.habit_name";
    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            var habitName = reader.GetString(0);
            var date = DateOnly.FromDateTime(reader.GetDateTime(1));
            var measure = reader.GetFloat(2);
            var unit = reader.GetString(3);

            Console.WriteLine($"{date} {habitName.PadRight(10 + 3)} {measure} {unit}");
        }
    }

    Console.WriteLine();
    Console.ReadLine();
}

void LogNewEntry(SqliteConnection connection)
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("> LOG NEW HABIT ENTRY");
    Console.WriteLine();
    Console.WriteLine("Answer all prompts to log a new entry. Leave blank to cancel and return to main menu.");

    // get all habit definitions
    Dictionary<string, string> habitToUnit = new();

    var command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM habitdefs";
    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            var habitName = reader.GetString(0);
            var unit = reader.GetString(1);

            habitToUnit.Add(habitName, unit);
        }
    }

    if (habitToUnit.Count == 0)
    {
        Console.WriteLine();
        Console.WriteLine("You need to have at least 1 habit defined.");
        Console.ReadLine();
        return;
    }

    Console.WriteLine();
    string? habit;
    bool isSuccessfulInput = false;
    do
    {
        Console.Write("Enter habit: ");
        habit = Console.ReadLine();

        if (string.IsNullOrEmpty(habit))
        {
            return;
        }

        if (habitToUnit.ContainsKey(habit))
        {
            isSuccessfulInput = true;
        }
        else
        {
            Console.Write("Habit not defined. ");
        }
    }
    while (!isSuccessfulInput);

    Console.WriteLine();
    string? inputMeasure;
    int measure = -1;
    isSuccessfulInput = false;
    do
    {
        Console.Write($"How many (in {habitToUnit[habit]}): ");
        inputMeasure = Console.ReadLine();

        if (string.IsNullOrEmpty(inputMeasure))
        {
            return;
        }

        if (int.TryParse(inputMeasure, out measure) && measure > 0)
        {
            isSuccessfulInput = true;
        }
        else
        {
            Console.Write($"Couldn't parse value. ");
        }
    }
    while (!isSuccessfulInput);

    Console.WriteLine();
    string? inputDate;
    DateOnly date;
    DateOnly today = DateOnly.FromDateTime(DateTime.Now);
    isSuccessfulInput = false;
    do
    {
        Console.Write("When (yyyy-MM-dd or \"today\"): ");
        inputDate = Console.ReadLine();

        if (string.IsNullOrEmpty(inputDate))
        {
            return;
        }

        if (inputDate.Equals("today"))
        {
            date = today;
            isSuccessfulInput = true;
        }
        else if (DateOnly.TryParseExact(inputDate, "yyyy-MM-dd", out date) && date <= today)
        {
            isSuccessfulInput = true;
        }
        else
        {
            Console.Write($"Couldn't parse date. ");
        }
    }
    while (!isSuccessfulInput);

    Console.WriteLine();
    string? inputConfirm;
    isSuccessfulInput = false;
    do
    {
        Console.Write($"Confirm \"{measure} {habitToUnit[habit]} of {habit} in {date}\" (y/n): ");
        inputConfirm = Console.ReadLine();

        if (string.IsNullOrEmpty(inputConfirm) || inputConfirm.ToLower().Equals("n"))
        {
            return;
        }

        if (inputConfirm.ToLower().Equals("y"))
        {
            isSuccessfulInput = true;
        }
        else
        {
            Console.Write("Couldn't parse input. ");
        }
    }
    while (!isSuccessfulInput);

    InsertHabitEntry(habit, date, measure, connection);
    Console.ReadLine();
}

void AddHabitDefinitionScreen(SqliteConnection connection)
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("> ADD NEW HABIT DEFINITION");
    Console.WriteLine();

    string? habitName;
    do
    {
        Console.Write("Name the new habit you want to track in this application (max. 20 characters): ");
        habitName = Console.ReadLine();
    } while (habitName == null);

    string? habitUnit;
    do
    {
        Console.Write("What should this habit be measured in (max. 10 characters)? ");
        habitUnit = Console.ReadLine();
    } while (habitUnit == null);

    string? confirm = null;
    do
    {
        Console.Write($"Are you sure you want to add \"{habitName} (in {habitUnit})\" as a habit? (y/n) ");
        confirm = Console.ReadLine();
    } while (confirm == null || (!confirm.ToLower().Equals("y") && !confirm.ToLower().Equals("n")));

    if (confirm.ToLower().Equals("y"))
    {
        try
        {
            var command = connection.CreateCommand();

            command.CommandText = $@"INSERT INTO habitdefs (habit_name, unit)
                                    VALUES ('{habitName}', '{habitUnit}')";
            command.ExecuteReader();

            Console.WriteLine();
            Console.Write("New habit definition created!");
            Console.ReadLine();
        }
        catch (SqliteException)
        {
            Console.WriteLine();
            Console.Write($"Couldn't create habit '{habitName}' (was already registered).");
            Console.ReadLine();
        }
    }
    else
    {
        Console.WriteLine();
        Console.Write("Habit definition canceled.");
        Console.ReadLine();
    }
}

void FillDbWithRandomEntries(SqliteConnection connection)
{
    Console.Clear();

    // define 3 habits
    //  cycling in kilometers
    //  walking in steps
    //  water in glasses
    //
    // loop 1 to a 100, date d in reversed order starting from today
    // for each of the above habits
    //  50% chance of adding an entry with date d and random measure value

    InsertHabitDefinition("cycling", "kilometers", connection);
    InsertHabitDefinition("walking", "steps", connection);
    InsertHabitDefinition("water", "glasses", connection);

    Dictionary<string, int[]> measureRangesPerHabit = new()
    {
        { "cycling", new int[2] { 5, 50 } },
        { "walking", new int[2] { 500, 3000 } },
        { "water",   new int[2] { 1, 10 } },
    };

    DateOnly today = DateOnly.FromDateTime(DateTime.Today);
    Random random = new Random();

    for (int i = 0; i < 100; i++)
    {
        DateOnly date = today.AddDays(-i);

        if (random.NextSingle() >= 0.5f) // cycling
        {
            int randomMin = measureRangesPerHabit["cycling"][0];
            int randomMax = measureRangesPerHabit["cycling"][1];
            int randomMeasure = random.Next(randomMin, randomMax + 1);

            InsertHabitEntry("cycling", date, randomMeasure, connection);
        }

        if (random.NextSingle() >= 0.5f) // walking
        {
            int randomMin = measureRangesPerHabit["walking"][0];
            int randomMax = measureRangesPerHabit["walking"][1];
            int randomMeasure = random.Next(randomMin, randomMax + 1);

            InsertHabitEntry("walking", date, randomMeasure, connection);
        }

        if (random.NextSingle() >= 0.5f) // water
        {
            int randomMin = measureRangesPerHabit["water"][0];
            int randomMax = measureRangesPerHabit["water"][1];
            int randomMeasure = random.Next(randomMin, randomMax + 1);

            InsertHabitEntry("water", date, randomMeasure, connection);
        }
    }

    Console.ReadLine();
}

void InsertHabitDefinition(string habitName, string unit, SqliteConnection connection)
{
    try
    {
        var command = connection.CreateCommand();
        command.CommandText = $@"INSERT INTO habitdefs (habit_name, unit)
                                    VALUES ('{habitName}', '{unit}')";
        command.ExecuteReader();

        Console.WriteLine($"'{habitName} ({unit})' habit created.");
    }
    catch (SqliteException)
    {
        // habit already defined
        Console.WriteLine($"'{habitName} ({unit})' habit already defined.");
    }
}

void InsertHabitEntry(string habit, DateOnly date, int measure, SqliteConnection connection)
{
    try
    {
        var command = connection.CreateCommand();
        command.CommandText = $@"INSERT INTO habitlogs (habit, date, measure)
                                            VALUES ('{habit}', '{date.ToString("yyyy-MM-dd")}', '{measure}')";
        command.ExecuteReader();

        Console.WriteLine($"Added entry ['{habit}', '{date.ToString("yyyy-MM-dd")}', '{measure}']");
    }
    catch (SqliteException)
    {
        // entry already defined for this habit and date
        Console.WriteLine($"Entry ['{habit}', '{date}'] already defined.");
    }
}