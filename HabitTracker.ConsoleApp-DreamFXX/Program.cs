using System.Data.SQLite;
using System.Globalization;

internal class Program
{
    static string connectionString = @"Data Source=HabitTrackerPersonal-ConsoleApp.db";

    static void Main()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Unit TEXT NOT NULL
                    )";
            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS HabitRecords (
                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                  HabitId INTEGER,
                  Date Text,
                  Time Text,
                  Quantity INTEGER,
                  FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                  )";
            tableCmd.ExecuteNonQuery();

            connection.Close();

        }

        //FillDatatables();
        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();

        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("Welcome to Habit Tracker!\n\n");
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("0 -> Save and Exit App\n");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("1 -> Show All records.");
            Console.WriteLine("2 -> Add a record.");
            Console.WriteLine("3 -> Delete a record");
            Console.WriteLine("4 -> Modify a record.");
            Console.WriteLine("5 -> Add your own Habit to this App.");
            Console.WriteLine("------------------------------------------");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nHave a good day!\n");
                    closeApp = true;
                    Environment.Exit(1);
                    break;
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    AddRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    ChangeRecord();
                    break;
                case "5":
                    AddNewHabit();
                    break;
                default:
                    Console.WriteLine("\n\nInvalid number of operation. Try again, valid operations are in range 0 - 5.\n\n");
                    break;
            }
        }
    }

    private static void ViewAllRecords()
    {
        Console.Clear();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM HabitRecords";

            List<HabitRecord> tableData = new List<HabitRecord>();
            SQLiteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new HabitRecord
                        {
                            Id = reader.GetInt32(0),
                            HabitId = reader.GetInt32(1),
                            Date = reader.GetString(2),
                            Time = reader.GetString(3),
                            Quantity = reader.GetInt32(4),
                        });
                }
            }
            else
            {
                Console.WriteLine("\n\nNo records were found!\n\n");
            }

            connection.Close();

            Console.WriteLine("------------HABIT RECORDS-----------\n");
            foreach (var record in tableData)
            {
                Console.WriteLine($"{record.Id} -> {record.Date} in {record.Time}h // {record.Quantity}");
            }
            Console.WriteLine("------------------------------------\n");
        }
    }

    private static void AddRecord()
    {
        Console.WriteLine("Choose a habit by entering Habits ID number below!");
        ViewHabits();

        int habitId = GetNumberInput("Habit ID:");
        string date = GetDate();
        string time = GetTime();
        int quantity = GetNumberInput("Enter quantity of habit you consumed // ran // did in units you selected in specified habit tracking.");

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO HabitRecords (HabitId, Date, Time, Quantity) VALUES ({habitId}, '{date}', '{time}', {quantity})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        Console.WriteLine("\nNew record was added sucessfully!\n\n");
    }

    internal static void ChangeRecord()
    {
        ViewAllRecords();

        var recordId = GetNumberInput("\n\nEnter ID number of record you want to modify.\n\n");

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM HabitRecords WHERE Id = {recordId})";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                connection.Close();
                ChangeRecord();
            }

            string date = GetDate();
            string time = GetTime();
            int quantity = GetNumberInput("\n\nEnter the quantity:\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE HabitRecords SET Date = '{date}', Time = '{time}', Quantity = {quantity} WHERE Id = {recordId}";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    private static void DeleteRecord()
    {
        Console.Clear();
        ViewAllRecords();

        var recordId = GetNumberInput("Enter ID number of the record you want to DELETE.");


        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM HabitRecords WHERE Id = {recordId}";

            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"Record with ID {recordId} does not exist. Try Again.");
                DeleteRecord();
            }
            connection.Close();
        }

        Console.WriteLine($"Record with {recordId} was succesfully deleted. Press ENTER to go back to the MENU.");
        Console.ReadLine();
        GetUserInput();
    }

    // Get user values section

    internal static string GetTime()
    {
        Console.WriteLine("\n\nEnter what time was when you did your Habit. // Type 0 to go back to Main Menu.");
        Console.Write("Please enter time in this format -> hh:mm - ");

        string timeinput = Console.ReadLine();

        if (timeinput == "0") GetUserInput();

        return timeinput;
    }

    internal static string GetDate()
    {
        Console.WriteLine("\n\nEnter a date. // Enter 0 to go back to the menu.\n\n");
        Console.Write("Type the date in this order -> DD-MM-YYYY - ");

        string dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nDate is typed in wrong format. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string countInput = Console.ReadLine();

        while (!Int32.TryParse(countInput, out _) || Convert.ToInt32(countInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            countInput = Console.ReadLine();
        }

        if (countInput == "0") GetUserInput();

        int intCountInput = Convert.ToInt32(countInput);

        return intCountInput;
    }

    // Users own Habit to Add

    static void AddNewHabit()
    {
        Console.WriteLine("\nEnter the name of the habit you want to track: ");
        string? habitName = Console.ReadLine();

        Console.WriteLine("\nEnter the unit of measurement (e.g: Amount consumed {ml, g, liters Etc.} or anything like minutes, hours, kilometers): ");
        string? habitUnit = Console.ReadLine();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT into Habits (Name, Unit) VALUES ('{habitName}', '{habitUnit}')";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Console.WriteLine($"New Habit named - '{habitName}' was sucessfully added");
    }

    static void ViewHabits()
    {
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM Habits";

            using(var reader = tableCmd.ExecuteReader())
            {
                Console.WriteLine("\nHabits available to track in this App: ");

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string unit = reader.GetString(2);
                    Console.WriteLine($"{id}. {name} ({unit})");
                }
            }
            connection.Close();
        }
    }

    static void FillDatatables()
    {
        using(var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT Id FROM Habits LIMIT 1";
            var habitIdObj = tableCmd.ExecuteScalar();

            int habitId;
            if (habitIdObj == null)
            {
                tableCmd.CommandText = "INSERT INTO Habits (Name, Unit) VALUES ('Cycling', 'kilometers')";
                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText = "SELECT Id FROM Habits LIMIT 1";
                habitId = Convert.ToInt32(tableCmd.ExecuteScalar());
            }
            else
            {
                habitId = Convert.ToInt32(habitIdObj);
            }

            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                var date = DateTime.Today.AddDays(-random.Next(0, 365)).ToString("dd-MM-yy");

                DateTime datetime = DateTime.Now;
                var time = TimeOnly.FromDateTime(datetime).ToString(); ; // Just now time..

                int quantity = random.Next(1, 100);
                tableCmd.CommandText = $"INSERT INTO HabitRecords (HabitId, Date, Time, Quantity) VALUES({habitId}, '{date}', '{time}', {quantity})";
                tableCmd.ExecuteNonQuery();
            }
            connection.Close();
        }
        Console.WriteLine("Testing records (100) were sucessfully created and added to their specified tables.");
    }

    // Properties class

    public class HabitRecord
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
    }
}

