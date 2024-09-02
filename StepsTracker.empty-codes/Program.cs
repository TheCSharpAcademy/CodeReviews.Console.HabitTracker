using Microsoft.Data.Sqlite;

string DbPath = "steps.db";

if (!File.Exists(DbPath))
{
    using (var conn = new SqliteConnection($"Data Source={DbPath}"))
    {
        conn.Open();

        string createHabitTableQuery = @"
            CREATE TABLE IF NOT EXISTS Steps (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Quantity INTEGER NOT NULL,
                Unit TEXT NOT NULL,
                DateLogged TEXT NOT NULL
            );";

        using var cmd = new SqliteCommand(createHabitTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    Console.WriteLine($"Database file {DbPath} successfully created.");
}
else
{
    Console.WriteLine($"Database file {DbPath} already exists");
}

int choice;

while (true)
{
    Console.WriteLine("MAIN MENU");
    Console.WriteLine("________________________");
    Console.WriteLine("Welcome to empty's Step Logger");
    Console.WriteLine("Choose an option using the numbers below:");
    Console.WriteLine("1 to View all Steps Logged");
    Console.WriteLine("2 to Insert a Log");
    Console.WriteLine("3 to Update a Log");
    Console.WriteLine("4 to Delete a Log");
    Console.WriteLine("5 to Exit this Application");

    if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
    {
        Console.WriteLine("Error: Unrecognized input.");
        continue;
    }
    else
    {
        break;
    }
}

switch (choice)
{
    case 1:
        break;
    case 2:
        break;
    case 3:
        break;
    case 4:
        break;
    case 5:
        return;
    default:
        Console.WriteLine("Error: Unrecognized input.");
        break;
}



class Steps
{
    public int? Quantity { get; set; }
    public string? Unit { get; set; }
    public string? Date { get; set; }

    public Steps(int quantity, string unit, string date)
    {
        Quantity = quantity;
        Unit = unit;
        Date = date;
    }

    public int InsertSteps(Steps log)
    {
        int result = -1;
        using (var conn = new SqliteConnection($"Data Source={"steps.db"}"))
        {
            conn.Open();

            string query = "INSERT INTO Steps(Quantity, Unit, DateLogged) VALUES(@quantity, @unit, @date)";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@quantity", log.Quantity);
            cmd.Parameters.AddWithValue("@unit", log.Unit);
            cmd.Parameters.AddWithValue("@date", log.Date);
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to log your steps\n - Details: " + e.Message);
            }
            conn.Close();
        }
        return result;
    }
}