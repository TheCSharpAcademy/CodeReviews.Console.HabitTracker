using Microsoft.Data.Sqlite;
using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

const string ConnectionString = @"Data Source=habitlogger.db";
const string TableName = "Habits";

CreateTable();

var isRunning = true;

while (isRunning)
{
    MainMenu();
    isRunning = UserAction();
}

static void CreateTable()
{
    using var connection = new SqliteConnection(ConnectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = $"CREATE TABLE IF NOT EXISTS {TableName} (" +
    "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
    "Name TEXT," +
    "Quantity INTEGER," +
    "Measurement TEXT," +
    "Date TEXT);";

    command.ExecuteNonQuery();
}

static void MainMenu()
{
    Console.Clear();
    Console.WriteLine("\n\nMAIN MENU\n\n" +
        "What would you like to do?\n\n" +
        "Type 0 to Close Application.\n" +
        "Type 1 to View All Records.\n" +
        "Type 2 to Insert Record.\n" +
        "Type 3 to Delete Record.\n" +
        "Type 4 to Update Record.\n" +
        "Type 5 to Generate Records.\n" +
        "Type 6 to Get a Records Summary.\n" +
        "--------------------------------\n");
}

static bool UserAction()
{
    var action = Console.ReadLine();

    switch (action)
    {
        case "0":
            Console.WriteLine("Press any key to Exit.");
            Console.ReadKey();
            return false;
        case "1":
            GetAll();
            Console.ReadKey();
            return true;
        case "2":
            Insert();
            return true;
        case "3":
            Delete();
            Console.ReadKey();
            return true;
        case "4":
            Update();
            Console.ReadKey();
            return true;
        case "5":
            GenerateHabits(10);
            Console.ReadKey();
            return true;
        case "6":
            PrintSummary();
            Console.ReadKey();
            return true;
        default:
            Console.WriteLine($"{action} is not a valid input. Press any key to continue.");
            return true;
    }
}

static void GenerateHabits(int amountToGenerate)
{
    var randomHabits = new RandomHabits();
    InsertHabits(randomHabits.Generate(amountToGenerate));
    Console.WriteLine($"{amountToGenerate} habits generated.");
}

static IEnumerable<Habit> ReadAllHabits()
{
    using var connection = new SqliteConnection(ConnectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = $"SELECT * FROM {TableName}";

    var habits = new List<Habit>();
    var dataReader = command.ExecuteReader();
    while (dataReader.Read())
    {
        habits.Add(new Habit()
        {
            Id = dataReader.GetInt32(0),
            Name = dataReader.GetString(1),
            Quantity = dataReader.GetInt32(2),
            Measurement = dataReader.GetString(3),
            Date = DateOnly.Parse(dataReader.GetString(4)),
        });
    }
    return habits;
}

static bool GetAll()
{
    var habits = ReadAllHabits();

    if (!habits.Any())
    {
        Console.WriteLine("There are no habits registered.");
        return false;
    }

    foreach (Habit habit in habits)
    {
        Console.WriteLine(habit);
    }

    return true;
}

static void Insert()
{
    var habit = CreateRecord();

    using var connection = new SqliteConnection(ConnectionString);
    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText = $"INSERT INTO {TableName} (Name, Quantity, Measurement, Date) VALUES ($Name, $Quantity, $Measurement, $Date)";
    command.Parameters.AddWithValue("$Name", habit.Name);
    command.Parameters.AddWithValue("$Quantity", habit.Quantity);
    command.Parameters.AddWithValue("$Measurement", habit.Measurement);
    command.Parameters.AddWithValue("$Date", habit.Date);

    command.ExecuteNonQuery();
}

static void InsertHabits(IEnumerable<Habit> habits)
{
    using var connection = new SqliteConnection(ConnectionString);
    connection.Open();

    foreach (Habit habit in habits)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"INSERT INTO {TableName} (Name, Quantity, Measurement, Date) VALUES ($Name, $Quantity, $Measurement, $Date)";
        command.Parameters.AddWithValue("$Name", habit.Name);
        command.Parameters.AddWithValue("$Quantity", habit.Quantity);
        command.Parameters.AddWithValue("$Measurement", habit.Measurement);
        command.Parameters.AddWithValue("$Date", habit.Date);
        command.ExecuteNonQuery();
    }
}

static Habit CreateRecord()
{
    const int TempHabitId = 1;

    Console.WriteLine("Name of Habit?");
    var name = Console.ReadLine();

    int quantity = ParseInt("Quantity/Amount in numbers?");

    Console.WriteLine("Unit of measurement?");
    var measurement = Console.ReadLine();

    string? dateInString;
    DateOnly date;
    do
    {
        Console.WriteLine("Create date mm/dd/yyyy");
        dateInString = Console.ReadLine();
    } while (!DateOnly.TryParse(dateInString, out date));

    return new Habit() { Id = TempHabitId, Name = name, Quantity = quantity, Measurement = measurement, Date = date };
}

static void Delete()
{
    var anyHabits = GetAll();

    if (anyHabits)
    {
        var id = ParseInt("Pick the Habits ID to delete");

        if (DeleteHabit(id) == 1)
            Console.WriteLine("Habit Deleted!");
        else
            Console.WriteLine("Failed to Delete Habit!");
    }
}

static int DeleteHabit(int id)
{
    using var connection = new SqliteConnection(ConnectionString);
    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText = $"DELETE FROM {TableName} WHERE Id = $Id";
    command.Parameters.AddWithValue("$Id", id);
    return command.ExecuteNonQuery();
}

static void Update()
{
    var anyHabits = GetAll();

    if (anyHabits)
    {
        var id = ParseInt("Pick the Habits ID to update");
        if (UpdateHabit(id) == 1)
            Console.WriteLine("Habit Updated!");
        else
            Console.WriteLine("Failed to Update Habit");
    }
}

static int UpdateHabit(int id)
{
    var habit = CreateRecord();

    using var connection = new SqliteConnection(ConnectionString);
    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText = $"UPDATE {TableName} SET Name = $Name, Quantity = $Quantity, Measurement = $Measurement, Date = $Date WHERE Id = $Id";
    command.Parameters.AddWithValue("$Name", habit.Name);
    command.Parameters.AddWithValue("$Quantity", habit.Quantity);
    command.Parameters.AddWithValue("$Measurement", habit.Measurement);
    command.Parameters.AddWithValue("$Date", habit.Date);
    command.Parameters.AddWithValue("$Id", id);
    return command.ExecuteNonQuery();
}

static void PrintSummary()
{
    var habits = ReadAllHabits();
    if (habits.Any())
    {
        var habitNamesGroup = habits.GroupBy(habit => habit.Name);

        Console.WriteLine("Which Habit do you want to summarize?");
        foreach (var group in habitNamesGroup)
        {
            Console.WriteLine(group.Key);
        }
        var habitToSummarize = Console.ReadLine();

        if (habitToSummarize is not null)
        {
            var habitNameGroup = habitNamesGroup.SingleOrDefault(habit => habit.Key?.ToLower() == habitToSummarize.Trim().ToLower());
            if (habitNameGroup is null)
            {
                Console.WriteLine("Habit Name does not exist.");
                return;
            }

            Console.Clear();
            var habitName = habitNameGroup.Key;

            var maxQuantity = habitNameGroup.MaxBy(habit => habit.Quantity);
            var minQuantity = habitNameGroup.MinBy(habit => habit.Quantity);

            var latestEntry = habitNameGroup.MaxBy(habit => habit.Date);
            var firstEntry = habitNameGroup.MinBy(habit => habit.Date);

            var measurement = habitNameGroup.First().Measurement;

            var totalEntries = habitNameGroup.Count();

            Console.WriteLine($"Habit Name: {habitName}");
            Console.WriteLine($"Total Entries: {totalEntries}");
            Console.WriteLine($"Max: {maxQuantity?.Quantity} {measurement} at {maxQuantity?.Date}");
            Console.WriteLine($"Min: {minQuantity?.Quantity} {measurement} at {minQuantity?.Date}");
            Console.WriteLine($"Latest Entry: {latestEntry?.Date} quantity {latestEntry?.Quantity}");
            Console.WriteLine($"First Entry: {firstEntry?.Date} quantity {firstEntry?.Quantity}");
        }
    }
    else
    {
        Console.WriteLine("No Habits to Summarize!");
    }
}

static int ParseInt(string message)
{
    string? stringInput;
    int input;
    do
    {
        Console.WriteLine(message);
        stringInput = Console.ReadLine();
    } while (!int.TryParse(stringInput, out input) || input < 0);
    return input;
}

public class Habit
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public int Quantity { get; init; }
    public string? Measurement { get; init; }
    public DateOnly Date { get; init; }
    public override string ToString()
    {
        return $"ID {Id}) {Name}: {Quantity} {Measurement} at {Date}";
    }
}

public class RandomHabits()
{
    private const int MaxQuantity = 10;
    private readonly Dictionary<string, string> nameAndMeasurement = new()
    {
        { "Running" , "km"},
        { "Studying" ,"hours"},
        { "Drinking water" ,"cups"},
        { "Reading" ,"pages"},
        { "Meditating", "minutes" }
    };

    public IEnumerable<Habit> Generate(int amount)
    {
        var habits = new List<Habit>();
        var random = new Random();

        for (int i = 0; i < amount; i++)
        {
            var randomNameMeas = nameAndMeasurement.ElementAt(random.Next(0, nameAndMeasurement.Count));
            habits.Add(new Habit
            {
                Id = i,
                Name = randomNameMeas.Key,
                Measurement = randomNameMeas.Value,
                Quantity = random.Next(0, MaxQuantity),
                Date = new DateOnly(random.Next(2020, 2025), random.Next(1, 13), random.Next(1, 29))
            });
        }
        return habits;
    }
}