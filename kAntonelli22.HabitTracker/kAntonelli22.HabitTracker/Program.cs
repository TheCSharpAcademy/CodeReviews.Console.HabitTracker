using System.Data.SQLite;

class Program
{
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
        string response = "";
        Console.WriteLine(
            @"  Habit Tracker Menu
    ------------------
    0. Exit Application
    1. View Records
    2. Log Record
    3. Remove Record
    4. View Records
    5. Add Habit
    6. Remove Habit
    7. Modify Habit
    ------------------"
        );
        response = CleanResponse(Console.ReadLine());

        if (response == "1")
        {
            LogRecord();
        }
        else if (response == "2")
        {
            ChangeRecord();
        }
        else if (response == "3")
        {
            RemoveRecord();
        }
        else if (response == "4")
        {
            ViewRecords();
        }
        else if (response == "5")
        {
            AddHabit();
        }
        else if (response == "6")
        {
            RemoveHabit();
        }
        else if (response == "7")
        {
            ModifyHabit();
        }
        else
        {
            Console.WriteLine("Please choose a valid input\n");
            MainMenu();
        }

    } // end of MainMenu method

    static void LogRecord()
    {
        Console.WriteLine("Log Records");
        // display all habits and ask which the user would like to edit. ask for the record and add it
        MainMenu();
    } // end of LogRecord method

    static void ChangeRecord()
    {
        Console.WriteLine("Change Record");
        // display all habits and ask which the user would like to edit. display those records and ask which index they want to replace
        MainMenu();
    } // end of ChangeRecord method

    static void RemoveRecord()
    {
        Console.WriteLine("Remove Record");
        // display all habits and ask which the user would like to edit. display those records and ask which index they want gone
        MainMenu();
    } // end of RemoveRecord method

    static void ViewRecords()
    {
        Console.WriteLine("View Records");
        // display all habits and ask which the user would like to view. display that table
        MainMenu();
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
                Habit TEXT NOT NULL,
                Quantity TEXT NOT NULL
            );";
        CreateTable(query);

        // connect to database and insert new habit into habit table
        query = "INSERT INTO Habits (Name, Quantity) VALUES (?, ?);";
        List < KeyValuePair<string, object>> data = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>(name, quantity)
        };
        InsertData(query, data);

        MainMenu();
    } // end of ChangeRecord method

    static void RemoveHabit()
    {
        Console.WriteLine("Remove Record");
        // find and remove the habits entry in the habit table and delete its records
        MainMenu();
    } // end of RemoveRecord method

    static void ModifyHabit()
    {
        Console.WriteLine("View Records");
        // replace the habit and its quantity and rename its table
        MainMenu();
    } // end of ViewRecords method


    static string CleanResponse(string? response)
    {
        string cleanResponse = response.Trim().ToLower();
        if (cleanResponse == "0")
            Environment.Exit(0);
        return cleanResponse;
    } // end of CleanResponse method

    static void CreateTable(string query)
    {
        // connect to database and insert new habit into habit table
        try
        {
            string connectionString = "Data Source=database.db;Version=3;";
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

    static void InsertData(string query, List<KeyValuePair<string, object>> data)
    {
        string connectionString = "Data Source=database.db;Version=3;";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand insertCommand = new SQLiteCommand(query, connection);

            foreach (var item in data)
                insertCommand.Parameters.Add(item.Value);
            try
            {
                insertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
            }
        }
  
    }
}