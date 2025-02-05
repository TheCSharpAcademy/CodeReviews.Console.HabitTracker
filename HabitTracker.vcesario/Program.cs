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
                                        ON UPDATE CASCADE
                                )";

        command.ExecuteReader();

        Console.WriteLine("Table 'habitlogs' created.");
    }
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

            command.CommandText = $@"INSERT INTO habitdefs
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