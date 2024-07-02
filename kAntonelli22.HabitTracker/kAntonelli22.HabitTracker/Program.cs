using System.Data.SQLite;
using System.Reflection.PortableExecutable;

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

        CreateTable(query);
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
    4. Remove Records
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

        ModifyDB($"INSERT INTO {viewRecords.Item1} (Date, Quantity) VALUES ('{date}','{quantity}');");
    } // end of LogRecord method

    static void ChangeRecord()
    {

        Console.WriteLine("What is the records date?");
        string date = CleanResponse(Console.ReadLine());
        Console.WriteLine("What is the quantity");
        string quantity = CleanResponse(Console.ReadLine());

        Tuple<string, List<string>, List<string>> viewRecords = ViewRecords();

        Console.WriteLine("Which Record would you like to modify?");
        int response;
        Int32.TryParse(CleanResponse(Console.ReadLine()), out response);

        while (!(response > 0) || !(response <= viewRecords.Item2.Count))
        {
            Int32.TryParse(CleanResponse(Console.ReadLine()), out response);
            Console.WriteLine("enter the number of one of the listed habits or menu to return to menu");
        }

        string query = $"SELECT Id FROM {viewRecords.Item1} LIMIT 1 OFFSET {response - 1}";
        int id = GetID(query);
        ModifyDB($"UPDATE {viewRecords.Item1} SET Date = '{date}', Quantity = '{quantity}' WHERE Id = {id}");
    } // end of ChangeRecord method

    static void RemoveRecord()
    {
        Tuple<string, List<string>, List<string>> viewRecords = ViewRecords();

        Console.WriteLine("Which Record would you like to remove?");
        int response;
        Int32.TryParse(CleanResponse(Console.ReadLine()), out response);

        while (!(response > 0) || !(response <= viewRecords.Item2.Count))
        {
            Int32.TryParse(CleanResponse(Console.ReadLine()), out response);
            Console.WriteLine("enter the number of one of the listed habits or menu to return to menu");
        }

        int id = GetID($"SELECT Id FROM {viewRecords.Item1} LIMIT 1 OFFSET {response - 1}");
        ModifyDB($"DELETE FROM {viewRecords.Item1} WHERE Id = {id}");
    } // end of RemoveRecord method

    static Tuple<string, List<string>, List<string>> ViewRecords()
    {
        Console.WriteLine("Which Habit would you like to view?");
        List<string> Habits = DisplayHabits();
        int response;
        Int32.TryParse(CleanResponse(Console.ReadLine()), out response);

        string habit;
        while (!(response > 0) || !(response <= Habits.Count))
        {
            Int32.TryParse(CleanResponse(Console.ReadLine()), out response);
            Console.WriteLine("enter the number of one of the listed habits or menu to return to menu");
        }
        habit = Habits[response - 1];

        Tuple<List<string>, List<string>> results = SelectData("SELECT * FROM " + habit);
        for (int i = 1; i < results.Item1.Count; i++)
        {
            Console.WriteLine($"{i}. {results.Item1[i].PadRight(10)} {results.Item2[i].PadLeft(10)}");
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
        CreateTable(query);

        ModifyDB($"INSERT INTO Habits (Habit, Quantity) VALUES ('{name}','{quantity}');");
    } // end of AddRecord method

    static void RemoveHabit()
    {
        Console.WriteLine("Which Habit would you like to remove?");
        List<string> Habits = DisplayHabits();
        int response;
        Int32.TryParse(CleanResponse(Console.ReadLine()), out response);

        while (!(response > 0) || !(response <= Habits.Count))
        {
            Int32.TryParse(CleanResponse(Console.ReadLine()), out response);
            Console.WriteLine("enter the number of one of the listed habits or menu to return to menu");
        }

        int id = GetID($"SELECT Id FROM Habits LIMIT 1 OFFSET {response - 1}");
        Console.WriteLine($"id: {id}");
        ModifyDB($"DELETE FROM Habits WHERE Id = {id}");

        // find and remove the habits entry in the habit table and delete its records
    } // end of RemoveRecord method

    static void ModifyHabit()
    {
        Console.WriteLine("What is the name of the Habit?");
        string name = CleanResponse(Console.ReadLine());
        Console.WriteLine("How is it measured? ex: mph");
        string quantity = CleanResponse(Console.ReadLine());

        Console.WriteLine("Which Habit would you like to modify?");
        List<string> Habits = DisplayHabits();
        int response;
        Int32.TryParse(CleanResponse(Console.ReadLine()), out response);

        while (!(response > 0) || !(response <= Habits.Count))
        {
            Int32.TryParse(CleanResponse(Console.ReadLine()), out response);
            Console.WriteLine("enter the number of one of the listed habits or menu to return to menu");
        }

        int id = GetID($"SELECT Id FROM Habits LIMIT 1 OFFSET {response - 1}");
        Console.WriteLine($"id: {id}");
        ModifyDB($"UPDATE Habits SET Habit = '{name}', Quantity = '{quantity}' WHERE Id = {id};");
    } // end of ViewRecords method



    static List<string> DisplayHabits()
    {
        List<string> Habits = new List<string>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM Habits";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    int lineNumber = 1;
                    while (reader.Read())
                    {
                        Console.WriteLine($"{lineNumber}. {reader.GetString(1).PadRight(10)} {reader.GetString(2).PadLeft(10)}");
                        Habits.Add(reader.GetString(1));
                        lineNumber++;
                    }
                }
            }
        }
        return Habits;
    }


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

    // database methods
    static int GetID(string query)
    {
        int id = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                        id = reader.GetInt32(0);
                }
            }
        }
        return id;
    }
    static void CreateTable(string query)
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
    }

    static Tuple<List<string>, List<string>> SelectData(string query)
    {
        List<string> results1 = new List<string>();
        List<string> results2 = new List<string>();
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
                        results2.Add(reader.GetString(1));
                    }
                }
            }
        }
        return new Tuple<List<string>, List<string>>(results1, results2);
    }

    static void ModifyDB(string query)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(query, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
            }
            connection.Close();
        }

    }
}