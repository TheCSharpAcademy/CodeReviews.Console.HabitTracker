using System.Data.SQLite;

class Program
{

    static string connectionString = "Data Source=database.db;Version=3;";
    static void Main(string[] args)
    {
        string query = @"
            CREATE TABLE IF NOT EXISTS Habits (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Habit TEXT NOT NULL,
                Quantity TEXT NOT NULL
            );";

        RunQuery(query);
        MainMenu();
    }

    static void MainMenu()
    {
        Console.WriteLine(
@"  Habit Tracker Menu
    ------------------
    0. Exit Application
    1. View Records
    2. Log Record
    3. Modify Record
    4. Remove Record
    5. Add Habit
    6. Remove Habit
    7. Modify Habit
    ------------------"
        );
        string response = CleanResponse(Console.ReadLine());

        if (response == "1")
            ViewRecords();
        else if (response == "2")
            LogRecord();
        else if (response == "3")
            ChangeRecord();
        else if (response == "4")
            RemoveRecord();
        else if (response == "5")
            AddHabit();
        else if (response == "6")
            RemoveHabit();
        else if (response == "7")
            ModifyHabit();
        else
            Console.WriteLine("Please choose a valid input\n");

        MainMenu();

    } // end of MainMenu method

    static void LogRecord()
    {
        Tuple<string, List<string>, List<string>> viewRecords = ViewRecords();

        Console.WriteLine("What is the records date?");
        string date = CleanResponse(Console.ReadLine());
        Console.WriteLine("What is the quantity");
        string quantity = CleanResponse(Console.ReadLine());

        RunQuery($"INSERT INTO {viewRecords.Item1} (Date, Quantity) VALUES ('{date}','{quantity}');");
    } // end of LogRecord method

    static void ChangeRecord()
    {
        Tuple<string, List<string>, List<string>> viewRecords = ViewRecords();
        Console.WriteLine("Which Record would you like to modify?");
        int response = GetValidResponse(viewRecords.Item2.Count);

        Console.WriteLine("What is the records date?");
        string date = CleanResponse(Console.ReadLine());
        Console.WriteLine("What is the quantity");
        string quantity = CleanResponse(Console.ReadLine());

        string query = $"SELECT Id FROM {viewRecords.Item1} LIMIT 1 OFFSET {response - 1}";
        int id = GetID(query);
        RunQuery($"UPDATE {viewRecords.Item1} SET Date = '{date}', Quantity = '{quantity}' WHERE Id = {id}");
    } // end of ChangeRecord method

    static void RemoveRecord()
    {
        Tuple<string, List<string>, List<string>> viewRecords = ViewRecords();

        Console.WriteLine("Which Record would you like to remove?");
        int response = GetValidResponse(viewRecords.Item2.Count);

        int id = GetID($"SELECT Id FROM {viewRecords.Item1} LIMIT 1 OFFSET {response - 1}");
        RunQuery($"DELETE FROM {viewRecords.Item1} WHERE Id = {id}");
    } // end of RemoveRecord method

    static Tuple<string, List<string>, List<string>> ViewRecords()
    {
        Console.WriteLine("Which Habit would you like to view?");
        Tuple<List<string>, List<string>> habitsTable = DisplayHabits();
        int response = GetValidResponse(habitsTable.Item1.Count);
        string habit = habitsTable.Item1[response - 1];
        string measurement = habitsTable.Item2[response - 1];

        Tuple<List<string>, List<string>> results = SelectData("SELECT * FROM " + habit);
        for (int i = 0; i < results.Item1.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {results.Item1[i].PadRight(10)} {$"{results.Item2[i]} {measurement}".PadLeft(10)}");
        }
        return new Tuple<string, List<string>, List<string>>(habit, results.Item1, results.Item2);
    } // end of ViewRecords method

    static void AddHabit()
    {
        Console.WriteLine("What is the name of the Habit?");
        string name = CleanResponse(Console.ReadLine());
        Console.WriteLine("How is it measured? ex: mph");
        string quantity = CleanResponse(Console.ReadLine());

        // create a new table to contain the habits records
        string query = @"
            CREATE TABLE IF NOT EXISTS " + name + @" (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT NOT NULL,
                Quantity TEXT NOT NULL
            );";
        RunQuery(query);

        RunQuery($"INSERT INTO Habits (Habit, Quantity) VALUES ('{name}','{quantity}');");
    } // end of AddRecord method

    static void RemoveHabit()
    {
        Console.WriteLine("Which Habit would you like to remove?");
        Tuple<List<string>, List<string>> habitsTable = DisplayHabits();
        int response = GetValidResponse(habitsTable.Item1.Count);

        int id = GetID($"SELECT Id FROM Habits LIMIT 1 OFFSET {response - 1}");
        RunQuery($"DELETE FROM Habits WHERE Id = {id}");
        RunQuery($"DROP TABLE IF EXISTS {habitsTable.Item1[response - 1]}");
    } // end of RemoveRecord method

    static void ModifyHabit()
    {
        Console.WriteLine("Which Habit would you like to modify?");
        Tuple<List<string>, List<string>> habitsTable = DisplayHabits();
        int response = GetValidResponse(habitsTable.Item1.Count);

        Console.WriteLine("What is the name of the Habit?");
        string name = CleanResponse(Console.ReadLine());
        Console.WriteLine("How is it measured? ex: mph");
        string quantity = CleanResponse(Console.ReadLine());

        int id = GetID($"SELECT Id FROM Habits LIMIT 1 OFFSET {response - 1}");
        RunQuery($"UPDATE Habits SET Habit = '{name}', Quantity = '{quantity}' WHERE Id = {id};");
    } // end of ViewRecords method

    static Tuple<List<string>, List<string>> DisplayHabits()
    {
        List<string> Habits = new List<string>();
        List<string> Measurements = new List<string>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand("SELECT * FROM Habits", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    int lineNumber = 1;
                    while (reader.Read())
                    {
                        Console.WriteLine($"{lineNumber}. {reader.GetString(1).PadRight(10)} {reader.GetString(2).PadLeft(10)}");
                        Habits.Add(reader.GetString(1));
                        Measurements.Add(reader.GetString(2));
                        lineNumber++;
                    }
                }
            }
        }
        return new Tuple<List<string>, List<string>>(Habits, Measurements);
    } // end of DisplayHabits method

    static string CleanResponse(string? response)
    {
        Console.Clear();
        if (response == null)
            return "";

        string cleanResponse = response.Trim().ToLower();
        if (cleanResponse == "0")
            Environment.Exit(0);
        else if (cleanResponse == "menu")
            MainMenu();

        return cleanResponse;
    } // end of CleanResponse method

    static int GetValidResponse(int max)
    {
        int response;
        Int32.TryParse(CleanResponse(Console.ReadLine()), out response);
        while (!(response > 0) || !(response <= max))
        {
            Int32.TryParse(CleanResponse(Console.ReadLine()), out response);
            Console.WriteLine("enter the number of one of the listed options or menu to return to the menu");
        }
        return response;
    } // end of GetValidResponse method

    // database methods
    static int GetID(string query)
    {

        int id = -1;
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            id = reader.GetInt32(0);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error has occured: {ex.Message}");
        }
        return id;
    } // end of GetID method

    static void RunQuery(string query)
    {
        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error has occured: {ex.Message}");
        }
    } // end of RunQuery method

    static Tuple<List<string>, List<string>> SelectData(string query)
    {
        List<string> results1 = new List<string>();
        List<string> results2 = new List<string>();
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results1.Add(reader.GetString(1));
                            results2.Add(reader.GetString(2));
                        }
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"An error has occured: {ex.Message}");
        }
        return new Tuple<List<string>, List<string>>(results1, results2);
    } // end of SelectData method
} // end of program