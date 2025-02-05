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
            case 5: // Add new habit definition
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
                                    measure FLOAT NOT NULL,
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

    try
    {
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO habitdefs (habit_name, unit)
                                    VALUES ('cycling', 'kilometers')";
        command.ExecuteReader();

        Console.WriteLine("'Cycling (kilometers)' habit created.");
    }
    catch (SqliteException)
    {
        // habit already defined
        Console.WriteLine("'Cycling (kilometers)' habit already defined.");
    }

    try
    {
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO habitdefs (habit_name, unit)
                                    VALUES ('walking', 'steps')";
        command.ExecuteReader();
        Console.WriteLine("'Walking (steps)' habit created.");
    }
    catch (SqliteException)
    {
        // habit already defined
        Console.WriteLine("'Walking (steps)' habit already defined.");
    }

    try
    {
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO habitdefs (habit_name, unit)
                                    VALUES ('water', 'glasses')";
        command.ExecuteReader();
        Console.WriteLine("'Water (glasses)' habit created.");
    }
    catch (SqliteException)
    {
        // habit already defined
        Console.WriteLine("'Water (glasses)' habit already defined.");
    }

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

            AddEntry("cycling", date, randomMeasure);
        }

        if (random.NextSingle() >= 0.5f) // walking
        {
            int randomMin = measureRangesPerHabit["walking"][0];
            int randomMax = measureRangesPerHabit["walking"][1];
            int randomMeasure = random.Next(randomMin, randomMax + 1);

            AddEntry("walking", date, randomMeasure);
        }

        if (random.NextSingle() >= 0.5f) // water
        {
            int randomMin = measureRangesPerHabit["water"][0];
            int randomMax = measureRangesPerHabit["water"][1];
            int randomMeasure = random.Next(randomMin, randomMax + 1);

            AddEntry("water", date, randomMeasure);
        }
    }

    Console.ReadLine();

    void AddEntry(string habit, DateOnly date, int randomMeasure)
    {
        try
        {
            var command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO habitlogs (habit, date, measure)
                                            VALUES ('{habit}', '{date}', '{randomMeasure}')";
            command.ExecuteReader();

            Console.WriteLine($"Added entry ['{habit}', '{date}', '{randomMeasure}']");
        }
        catch (SqliteException)
        {
            // entry already defined for this habit and date
            Console.WriteLine($"Entry ['{habit}', '{date}'] already defined.");
        }
    }
}