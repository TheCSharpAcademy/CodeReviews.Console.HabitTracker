using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

string? DbPath = "steps.db";
int choice;

CreateDatabase();

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
    Console.WriteLine("5 to View a Tailored Report");
    Console.WriteLine("6 to Exit this Application");

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
        ViewAllSteps();
        break;
    case 2:
        CreateStepsLog();
        break;
    case 3:
        UpdateStepsLog();
        break;
    case 4:
        DeleteStepsLog();
        break;
    case 5:
        break;
    case 6:
        return;
    default:
        Console.WriteLine("Error: Unrecognized input.");
        break;
}

void CreateStepsLog()
{
    Steps insertSteps = new Steps();

    Console.WriteLine("Input 0 to Add Today's Date or 1 to Add a Custom Date:");
    bool state = int.TryParse(Console.ReadLine(), out int dateChoice);

    while(!state || dateChoice != 1 || dateChoice != 0)
    {
        Console.WriteLine("Error: Choose between options 0 and 1.");
        state = int.TryParse(Console.ReadLine(), out dateChoice);
    }
    if (state && dateChoice == 0)
    {
        insertSteps.DateLogged = DateTime.Now.ToString("dd-MM-yy");
    }
    if (state && dateChoice == 1)
    {
        Console.Write("Enter the date in the 'dd-MM-yy' format: ");
        string? date = Console.ReadLine();

        while (date == null || !Regex.IsMatch(date, @"^\d{2}-\d{2}-\d{2}$"))
        {
            Console.WriteLine("Error: Date must be in 'dd-MM-yy' format, Enter again: ");
            date = Console.ReadLine();
        }
        insertSteps.DateLogged = date;
    }

    Console.WriteLine("Enter the number of steps walked:");
    int.TryParse(Console.ReadLine(), out int quantity);
    insertSteps.Quantity = quantity;

    insertSteps.Unit = "steps";

    if (insertSteps.InsertSteps(insertSteps) == 1)
    {
        Console.WriteLine("Steps successfully logged.");
    }
}
void ViewAllSteps()
{
    Steps viewSteps = new Steps();
    Console.WriteLine(viewSteps.ViewSteps());
}  
void UpdateStepsLog()
{
    Steps updateSteps = new Steps();
    Console.WriteLine("Enter the Id of the log you wish to update:");
    int.TryParse(Console.ReadLine(), out int updateId);
    updateSteps.Id = updateId;
    Console.WriteLine("Enter the new number of steps:");
    int.TryParse(Console.ReadLine(), out int quantity);
    updateSteps.Quantity = quantity;
    if (updateSteps.UpdateSteps(updateSteps) == 1)
    {
        Console.WriteLine("Steps count successfully changed.");
    }
}
void DeleteStepsLog()
{
    Steps deleteSteps = new Steps();
    Console.WriteLine("Enter the Id of the log you wish to delete:");
    int.TryParse(Console.ReadLine(), out int deleteId);
    deleteSteps.Id = deleteId;
    if (deleteSteps.DeleteSteps(deleteSteps) == 1)
    {
        Console.WriteLine("Steps log successfully deleted.");
    }
}
void CreateDatabase()
{
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
        SeedDatabase();
    }
    else
    {
        Console.WriteLine($"Database file {DbPath} already exists");
    }
}
void SeedDatabase()
{
    using (var conn = new SqliteConnection($"Data Source={DbPath}"))
    {
        conn.Open();
        Console.WriteLine("Seeding database with 100 records...");

        Random random = new Random();

        for (int i = 0; i < 100; i++)
        {
            string query = "INSERT INTO Steps (Quantity, Unit, DateLogged) VALUES (@quantity, @unit, @date)";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@quantity", random.Next(5000, 10000));
            cmd.Parameters.AddWithValue("@unit", "steps");
            cmd.Parameters.AddWithValue("@date", DateTime.Now.AddDays(-i).ToString("dd-MM-yy"));

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to seed the database\n - Details: " + e.Message);
            }
        }

        Console.WriteLine("Seeding completed.");
    }
}
class Steps
{
    public int? Id { get; set; }
    public int? Quantity { get; set; }
    public string? Unit { get; set; }
    public string? DateLogged { get; set; }

    public const string? DbPath = "steps.db";

    public Steps() { }

    public int InsertSteps(Steps log)
    {
        int result = -1;
        using (var conn = new SqliteConnection($"Data Source={DbPath}"))
        {
            conn.Open();

            string query = "INSERT INTO Steps(Quantity, Unit, DateLogged) VALUES(@quantity, @unit, @date)";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@quantity", log.Quantity);
            cmd.Parameters.AddWithValue("@unit", log.Unit);
            cmd.Parameters.AddWithValue("@date", log.DateLogged);
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to log your steps\n - Details: " + e.Message);
            }
        }
        return result;
    }

    public List<Steps> ViewSteps()
    {
        var logs = new List<Steps>();

        using (var conn = new SqliteConnection($"Data Source={DbPath}"))
        {
            conn.Open();

            string query = "SELECT Quantity, Unit, DateLogged FROM Steps";

            using var cmd = new SqliteCommand(query, conn);

            try
            {
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var entry = new Steps
                    {
                        Quantity = reader.GetInt32(1),
                        Unit = reader.GetString(2),
                        DateLogged = reader.GetString(3)
                    };
                    logs.Add(entry);
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to access your steps log\n - Details: " + e.Message);
            }
        }
        return logs;
    }
    public int UpdateSteps(Steps log)
    {
        int result = -1;
        using (var conn = new SqliteConnection($"Data Source={DbPath}"))
        {
            conn.Open();

            string query = "UPDATE Steps SET Quantity = @quantity WHERE Id = @id";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@quantity", log.Quantity);
            cmd.Parameters.AddWithValue("@id", log.Id);

            try
            {
                result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    Console.WriteLine($"No record found with the provided Id: {log.Id}");
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to edit your steps count\n - Details: " + e.Message);
            }
        }
        return result;
    }

    public int DeleteSteps(Steps log)
    {
        int result = -1;
        using (var conn = new SqliteConnection($"Data Source={DbPath}"))
        {
            conn.Open();

            string query = "DELETE FROM Steps WHERE Id = @id";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", log.Id);

            try
            {
                result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    Console.WriteLine($"No record found with the provided Id: {log.Id}");
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to delete this log\n - Details: " + e.Message);
            }
        }
        return result;
    }
}