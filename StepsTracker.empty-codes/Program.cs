using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using ConsoleTableExt;

string? DbPath = "steps.db";
bool isStateValid = false;

CreateDatabase();

while(true)
{
    Console.WriteLine("________________________");
    Console.WriteLine("MAIN MENU");
    Console.WriteLine("________________________");
    Console.WriteLine("Welcome to empty's Step Logger :)");
    Console.WriteLine("Choose an option using the numbers below:\n");
    Console.WriteLine("1 to View all Steps Logged");
    Console.WriteLine("2 to Insert a Log");
    Console.WriteLine("3 to Update a Log");
    Console.WriteLine("4 to Delete a Log");
    Console.WriteLine("5 to View a Tailored Report");
    Console.WriteLine("6 to Exit this Application");
    Console.WriteLine("________________________\n");
    isStateValid = int.TryParse(Console.ReadLine(), out int choice);

    while (!isStateValid || choice < 1 || choice > 6)
    {
        Console.WriteLine("Error: Unrecognized input, Enter a number from 1 to 6: ");
        isStateValid = int.TryParse(Console.ReadLine(), out choice);
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
            ViewGeneralReport();
            break;
        case 6:
            return;
        default:
            Console.WriteLine("Error: Unrecognized input.");
            break;
    }
}
void CreateStepsLog()
{
    Steps insertSteps = new Steps();

    Console.Write("Input 0 to Add Today's Date or 1 to Add a Custom Date:");
    bool isDateChoiceValid = int.TryParse(Console.ReadLine(), out int dateChoice);

    while(!isDateChoiceValid || dateChoice != 1 && dateChoice != 0)
    {
        Console.Write("Error: Choose between options 0 and 1.");
        isDateChoiceValid = int.TryParse(Console.ReadLine(), out dateChoice);
    }
    if (isDateChoiceValid && dateChoice == 0)
    {
        insertSteps.DateLogged = DateTime.Now.ToString("yyyy-MM-dd");
    }
    if (isDateChoiceValid && dateChoice == 1)
    {
        Console.Write("Enter the date in the 'yyyy-MM-dd' format: ");
        string? date = Console.ReadLine();

        while (date == null || !Regex.IsMatch(date, @"^\d{4}-\d{2}-\d{2}$"))
        {
            Console.Write("Error: Date must be in 'yyyy-MM-dd' format, Enter again: ");
            date = Console.ReadLine();
        }
        insertSteps.DateLogged = date;
    }
    Console.Write("Enter the number of steps walked:");
    int.TryParse(Console.ReadLine(), out int quantity);
    insertSteps.Quantity = quantity;
    insertSteps.Unit = "steps";

    insertSteps.InsertSteps(insertSteps);
}
void ViewAllSteps()
{
    Steps viewSteps = new Steps();
    var stepsList = viewSteps.ViewSteps();

    if (stepsList.Count == 0)
    {
        Console.WriteLine("No steps logged.");
        return;
    }
    ConsoleTableBuilder
    .From(stepsList)
    .ExportAndWriteLine();
}  
void UpdateStepsLog()
{
    Steps updateSteps = new Steps();
    Console.Write("Enter the Id of the log you wish to update:");
    int.TryParse(Console.ReadLine(), out int updateId);
    updateSteps.Id = updateId;
    Console.Write("Enter the new number of steps:");
    int.TryParse(Console.ReadLine(), out int quantity);
    updateSteps.Quantity = quantity;
    updateSteps.UpdateSteps(updateSteps);
}
void DeleteStepsLog()
{
    Steps deleteSteps = new Steps();
    Console.Write("Enter the Id of the log you wish to delete:");
    int.TryParse(Console.ReadLine(), out int deleteId);
    deleteSteps.Id = deleteId;
    deleteSteps.DeleteSteps(deleteSteps);
}
void ViewGeneralReport()
{
    Steps viewMetrics = new Steps();
    viewMetrics.ViewReport();
    Console.ReadKey();
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
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to create the database Table\n - Details: " + e.Message);
            }
        }
        Console.WriteLine($"Database file {DbPath} successfully created.\n");
        SeedDatabase();
    }
    else
    {
        Console.WriteLine($"Database file {DbPath} already exists.\n");
    }
}
void SeedDatabase()
{
    using (var conn = new SqliteConnection($"Data Source={DbPath}"))
    {
        conn.Open();
        Console.WriteLine("Seeding database with 100 records...");

        Random random = new Random();
        DateTime startDate = DateTime.Now.AddMonths(-7);
        DateTime endDate = DateTime.Now.AddMonths(-1);

        for (int i = 0; i < 100; i++)
        {
            int range = (endDate - startDate).Days;
            DateTime randomDate = startDate.AddDays(random.Next(range));

            string query = "INSERT INTO Steps (Quantity, Unit, DateLogged) VALUES (@quantity, @unit, @date)";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@quantity", random.Next(5000, 10000));
            cmd.Parameters.AddWithValue("@unit", "steps");
            cmd.Parameters.AddWithValue("@date", randomDate.ToString("yyyy-MM-dd"));

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
    public void InsertSteps(Steps log)
    {
        using (var conn = new SqliteConnection($"Data Source={DbPath}"))
        {
            conn.Open();

            string checkQuery = "SELECT Id FROM Steps WHERE DateLogged = @date";
            using (var cmd = new SqliteCommand(checkQuery, conn))
            {
                cmd.Parameters.AddWithValue("@date", log.DateLogged);

                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int existingId = reader.GetInt32(0);
                            Console.WriteLine($"A log with this date already exists. Existing log ID: {existingId}. No new entry added.\n");
                            return;
                        }
                    }
                }
                catch (SqliteException e)
                {
                    Console.WriteLine("Error occurred while checking for existing log\n - Details: " + e.Message);
                    return;
                }
            }
            string insertquery = "INSERT INTO Steps(Quantity, Unit, DateLogged) VALUES(@quantity, @unit, @date)";
            using (var cmd = new SqliteCommand(insertquery, conn))
            {
                cmd.Parameters.AddWithValue("@quantity", log.Quantity);
                cmd.Parameters.AddWithValue("@unit", log.Unit);
                cmd.Parameters.AddWithValue("@date", log.DateLogged);

                try
                {
                    cmd.ExecuteNonQuery();

                    string getIdQuery = "SELECT last_insert_rowid();";
                    using (var idCmd = new SqliteCommand(getIdQuery, conn))
                    {
                        log.Id = Convert.ToInt32(idCmd.ExecuteScalar());
                    }

                    Console.WriteLine($"Steps successfully logged. (Log Id: {log.Id})\n");
                }
                catch (SqliteException e)
                {
                    Console.WriteLine("Error occurred while trying to log your steps\n - Details: " + e.Message);
                }
            }
        }
    }
    public List<Steps> ViewSteps()
    {
        var logs = new List<Steps>();

        using (var conn = new SqliteConnection($"Data Source={DbPath}"))
        {
            conn.Open();

            string query = "SELECT Id, Quantity, Unit, DateLogged FROM Steps";
            using var cmd = new SqliteCommand(query, conn);

            try
            {
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var entry = new Steps
                    {
                        Id = reader.GetInt32(0),
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
    public void UpdateSteps(Steps log)
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
                    Console.WriteLine($"No record found with the provided Id: {log.Id}\n");
                }
                else
                {
                    Console.WriteLine($"Record with Id: {log.Id} successfully updated.\n");
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to edit your steps count\n - Details: " + e.Message);
            }
        }
    }
    public void DeleteSteps(Steps log)
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
                    Console.WriteLine($"No record found with the provided Id: {log.Id}\n");
                }
                else
                {
                    Console.WriteLine($"Record with Id: {log.Id} successfully deleted.\n");
                }
            }
            catch (SqliteException e)
            {
                Console.WriteLine("Error occured while trying to delete this log\n - Details: " + e.Message);
            }
        }
    }
    public void ViewReport()
    {
        int avgStepsDaily = 0;
        int highestStepsInADay = 0;
        int totalStepsThisYear = 0;
        double totalKmsWalked = 0;

        using (var conn = new SqliteConnection($"Data Source={DbPath}"))
        {
            conn.Open();

            string avgStepsQuery = @"SELECT AVG(Quantity) AS AverageStepsPerDay
                                 FROM Steps 
                                 WHERE strftime('%Y', DateLogged) = strftime('%Y', 'now')";

            using (var cmd = new SqliteCommand(avgStepsQuery, conn))
            {
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            avgStepsDaily = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        }
                    }
                }
                catch (SqliteException e)
                {
                    Console.WriteLine("Error occurred while calculating average steps per day\n - Details: " + e.Message);
                }
            }

            string highestStepsQuery = @"SELECT MAX(Quantity) AS HighestStepsInADay
                                     FROM Steps 
                                     WHERE strftime('%Y', DateLogged) = strftime('%Y', 'now')";

            using (var cmd = new SqliteCommand(highestStepsQuery, conn))
            {
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            highestStepsInADay = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        }
                    }
                }
                catch (SqliteException e)
                {
                    Console.WriteLine("Error occurred while finding the highest steps in a day\n - Details: " + e.Message);
                }
            }
            string totalStepsQuery = @"SELECT SUM(Quantity) AS TotalStepsThisYear
                                   FROM Steps 
                                   WHERE strftime('%Y', DateLogged) = strftime('%Y', 'now')";

            using (var cmd = new SqliteCommand(totalStepsQuery, conn))
            {
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalStepsThisYear = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        }
                    }
                }
                catch (SqliteException e)
                {
                    Console.WriteLine("Error occurred while calculating total steps this year\n - Details: " + e.Message);
                }
            }
            string totalKmsQuery = @"SELECT SUM(Quantity) * 0.000762 AS TotalKilometersWalked
                                 FROM Steps 
                                 WHERE strftime('%Y', DateLogged) = strftime('%Y', 'now')";

            using (var cmd = new SqliteCommand(totalKmsQuery, conn))
            {
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalKmsWalked = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                        }
                    }
                }
                catch (SqliteException e)
                {
                    Console.WriteLine("Error occurred while calculating total kilometers walked\n - Details: " + e.Message);
                }
            }
        }
        Console.WriteLine($"Average Daily Steps This Year: {avgStepsDaily}\n");
        Console.WriteLine($"Highest Steps in a Day This Year: {highestStepsInADay}\n");
        Console.WriteLine($"Total Steps This Year: {totalStepsThisYear}\n");
        Console.WriteLine($"Total Kilometers Walked (Assumes an average step length of 0.762 meters): {totalKmsWalked:F2} km");
    }
}