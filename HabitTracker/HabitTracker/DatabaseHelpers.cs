using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTracker
{
    internal class DatabaseHelpers
    {
        private static string connectionString = @"Data Source=..\..\Files\Library.db;Version=3";
        private static List<string> databases = new List<string>();
        public static void InitializeDataBase()
        {
            // Extract the database file path from the connection string.
            var dbFilePath = @"..\..\Files\Library.db";

            // Ensure the directory exists before creating the file.
            var dbDirectory = System.IO.Path.GetDirectoryName(dbFilePath);
            if (!System.IO.Directory.Exists(dbDirectory))
            {
                System.IO.Directory.CreateDirectory(dbDirectory);
            }

            // Check if the database file exists before creating it.
            if (!System.IO.File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }

        }
        public static void AddHabits()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("\n\nWrite a habit name that you want to initialize to database" +
                    "(Your habit name must be without any white space and only english letters are acceptable)\n\n");

                string tableName = Console.ReadLine();
                while (string.IsNullOrEmpty(tableName) || !Regex.IsMatch(tableName, @"^[A-Za-z]+$"))
                {
                    Console.WriteLine("Invalid habit name. Try again!");
                    tableName = Console.ReadLine();
                }
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $@"CREATE TABLE IF NOT EXISTS {tableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Measure TEXT,
                        Quantity INTEGER
                    )";
                AddHabitToDatabase(tableName);

                tableCmd.ExecuteNonQuery(); // Ensure the command is executed.
            }
        }
        public static void CreateTableOfHabits()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    "CREATE TABLE IF NOT EXISTS HabitsList (HabitName TEXT PRIMARY KEY)";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        public static void LoadHabits()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT HabitName FROM HabitsList";

                using (var reader = tableCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databases.Add(reader.GetString(0));
                    }
                }

                connection.Close();
            }
        }
        public static void AddHabitToDatabase(string habitName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO HabitsList (HabitName) VALUES ('{habitName}')";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            databases.Add(habitName);  // Also add it to the in-memory list.
        }
        public static void Insert()
        {
            if (databases.Count == 0)
            {
                Console.WriteLine("\n\nThere are no habits to track added, do you want to add it?yes/no" +
                    "(Your habit name must be without any white space and only english letters are acceptable)\n\n");
                string answer = Console.ReadLine();
                while(string.IsNullOrEmpty(answer) && !Regex.IsMatch(answer.Trim(), @"^[A-Za-z]$"))
                {
                    Console.WriteLine("Invalid habit name. Try again!");
                    answer = Console.ReadLine();
                }
                if (answer == "yes")
                {
                    AddHabits();
                    Console.WriteLine("\n\nInserted succsessfuly. Press any key to return to main menu!");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Insertion menu:\n\n");
                Console.WriteLine("a. Add new habit to track");
                Console.WriteLine("b. Add new habit's record");
                Console.WriteLine("c. Main Menu");
                string answer = Console.ReadLine();

                while (!Regex.IsMatch(answer, @"^[abc]$"))
                {
                    Console.WriteLine("There is no option shown that you entered! Try again!");
                    answer = Console.ReadLine();
                }

                if (answer == "c") Menu.GetUserInput();

                
                if (answer == "b")
                {
                    Console.Clear();
                    for(int i = 0; i < databases.Count; i++)
                    {
                        string db = databases[i];
                        Console.WriteLine($"{i + 1}. {db}");
                    }
                    Console.WriteLine("\n\nWrite full name of habit that you want to insert a record, that are shown above" +
                        "(Your habit name must be without any white space and only english letters are acceptable)\n\n");
                    string habitName = Console.ReadLine();
                    while (!databases.Contains(habitName))
                    {
                        Console.WriteLine("\n\nThere are no habit with the name you entered! Enter it again, please.\n\n");
                        habitName = Console.ReadLine();
                    }

                    string date = Menu.GetDateInput();
                    string measure = Menu.GetMeasureInput("\n\nEnter in what you want to measure your habit\n\n");
                    int quantity = Menu.GetNumberInput("\n\nEnter quantity of the thing you want to do\n\n");
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        var tableCmd = connection.CreateCommand();
                        tableCmd.CommandText =
                            $"INSERT INTO {habitName}(date, quantity, measure) Values('{date}', {quantity}, '{measure}')";
                        tableCmd.ExecuteNonQuery(); // Ensure the command is executed.
                        connection.Close();
                    }
                    Console.WriteLine("\n\nInserted succsessfuly. Press any key to return to main menu!");
                    Console.ReadLine();
                }
                else
                {
                    AddHabits();
                    Console.WriteLine("\n\nInserted succsessfuly. Press any key to return to main menu!");
                    Console.ReadLine();
                }
            }
            
        }
        
        public static void Delete()
        {
            string habitName = "";
            Console.Clear();
            if (databases.Count == 0)
            {
                Insert();
                return;
            }
            Console.WriteLine("Delete menu:\n\n");
            Console.WriteLine("a. Delete a habit that is tracked");
            Console.WriteLine("b. Delete habit's record");
            Console.WriteLine("c. Main Menu");
            string answer = Console.ReadLine(); 
            while (!Regex.IsMatch(answer, @"^[abc]$"))
            {
                Console.WriteLine("\n\nThere is no option shown that you entered! Try again!");
                answer = Console.ReadLine();
            }
            if (answer == "c") Menu.GetUserInput();

            if(answer == "a")
            {
                for (int i = 0; i < databases.Count; i++)
                {
                    string db = databases[i];
                    Console.WriteLine($"{i + 1}. {db}");
                }

                Console.WriteLine("\n\nWrite full name of habit that you want to remove");
                habitName = Console.ReadLine();
                while (!databases.Contains(habitName))
                {
                    Console.WriteLine("\n\nThere are no habit with the name you entered! Enter it again, please.");
                    habitName = Console.ReadLine();
                }
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"DELETE FROM HabitsList WHERE HabitName = '{habitName}'";
                    tableCmd.ExecuteNonQuery();
                    tableCmd.CommandText = $"DROP TABLE IF EXISTS {habitName};";
                    databases.Remove(habitName);

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                    Console.WriteLine("\n\nDeleted succsessfuly. Press any key to return to main menu!");
                    Console.ReadLine();
                }

            }
            else
            {
                ViewHabits(ref habitName);  
                var record = Menu.GetNumberInput("\n\nPlease type Id of the record would like to Delete. Type 0 to return to main menu.\n\n");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"DELETE FROM {habitName} WHERE Id = '{record}'";

                    int rowCount = tableCmd.ExecuteNonQuery();

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"\n\nNo rows were deleted");
                    }
                    else
                    {
                        Console.WriteLine($"{rowCount} rows were deleted");
                    }
                    connection.Close();
                    Console.WriteLine("\n\nDeleted succsessfuly. Press any key to return to main menu!");
                    Console.ReadLine();
                }

            }
        }
        public static void Update()
        {
            string habitName = "";
            Console.Clear();
            if (databases.Count == 0)
            {
                Insert();
            }
            Console.WriteLine("Update menu:\n\n");
            Console.WriteLine("a. Update a habit's name");
            Console.WriteLine("b. Update a habit's record");
            Console.WriteLine("c. Main Menu");
            string answer = Console.ReadLine();
            while (!Regex.IsMatch(answer, @"^[abc]$"))
            {
                Console.WriteLine("\n\nThere is no option shown that you entered! Try again!");
                answer = Console.ReadLine();
            }
            if (answer == "c") Menu.GetUserInput();

            if (answer == "a")
            {
                for (int i = 0; i < databases.Count; i++)
                {
                    string db = databases[i];
                    Console.WriteLine($"{i + 1}. {db}");
                }

                Console.WriteLine("\n\nWrite full name of habit that you want to update\n\n");
                habitName = Console.ReadLine();
                while (!databases.Contains(habitName))
                {
                    Console.WriteLine("\n\nThere are no habit with the name you entered! Enter it again, please.\n\n");
                    habitName = Console.ReadLine();
                }

                Console.WriteLine("\n\nWrite new habit name: \n\n");
                string newHabitName = Console.ReadLine();
                while (string.IsNullOrEmpty(newHabitName) || !Regex.IsMatch(newHabitName.Trim(), @"^[A-Za-z]+$"))
                {
                    Console.WriteLine("Invalid habit name. Try again!");
                    newHabitName = Console.ReadLine();
                }
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableCMD = connection.CreateCommand();
                    tableCMD.CommandText = $"UPDATE HabitsList SET HabitName = '{newHabitName}' WHERE HabitName = '{habitName}'";
                    tableCMD.ExecuteNonQuery();
                    tableCMD.CommandText =  $"ALTER TABLE {habitName} RENAME TO {newHabitName}";
                    tableCMD.ExecuteNonQuery();
                    databases.Remove(habitName);
                    databases.Add(newHabitName);


                    connection.Close();
                    Console.WriteLine("\n\nUpdated succsessfuly. Press any key to return to main menu!");
                    Console.ReadLine();
                }

            }
            else
            {
                ViewHabits(ref habitName);
                var recordId = Menu.GetNumberInput("\n\nPlease enter habit's id that you want to update\n\n");
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"SELECT EXISTS(SELECT 1 FROM {habitName} WHERE id = '{recordId}')";

                    int rowCount = Convert.ToInt32(tableCmd.ExecuteScalar());

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                        connection.Close();
                        Update();
                    }
                    string date = Menu.GetDateInput();
                    string measure = Menu.GetMeasureInput("\n\nEnter in what you want to measure your record\n\n");
                    int quantity = Menu.GetNumberInput("\n\nEnter quantity of thing you want to do\n\n");

                    var tableCMD = connection.CreateCommand();
                    tableCMD.CommandText = $"UPDATE {habitName} SET date = '{date}', quantity = {quantity}, measure = '{measure}' WHERE Id = '{recordId}'";
                    tableCMD.ExecuteNonQuery();

                    connection.Close();
                    Console.WriteLine("\n\nUpdated succsessfuly. Press any key to return to main menu!");
                    Console.ReadLine();
                }

            }
            
        }
        public static void ViewHabits(ref string habitName)
        {
            habitName = "";
            if (databases.Count == 0) 
            {
                Insert();
            }
            else 
            {
                for (int i = 0; i < databases.Count; i++)
                {
                    string db = databases[i];
                    Console.WriteLine($"{i + 1}. {db}");
                }
                Console.WriteLine("\n\nWrite full name of habit that you want to view records\n\n");
                habitName = Console.ReadLine();
                while (!databases.Contains(habitName))
                {
                    Console.WriteLine("\n\nThere are no habit with the name you entered! Enter it again, please.\n\n");
                    habitName = Console.ReadLine();
                }
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"SELECT * FROM {habitName}";

                    List<Habit> tableData = new List<Habit>();
                   
                    SQLiteDataReader reader = tableCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(
                                new Habit
                                {
                                    Id = reader.GetInt32(0),
                                    Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                    Measure = reader.GetString(2),
                                    Quantity = reader.GetInt32(3)
                                }); ;

                        }
                    }
                    else
                    {
                        Console.WriteLine("\n\nNo rows found!");
                        Console.WriteLine("------------------------------------------\n");
                        Console.WriteLine("Press any key to return to main menu!");
                        Console.ReadLine();
                        Menu.GetUserInput();

                    }
                    connection.Close();
                    Console.WriteLine("------------------------------------------\n");
                    foreach (var dw in tableData)
                    {
                        Console.WriteLine($"{dw.Id} - {dw.Date:yyyy-MM-dd} - Quantity: {dw.Quantity} {dw.Measure}");
                    }
                    Console.WriteLine("------------------------------------------\n");
                    Console.WriteLine("\n\nPress any key to continue!");
                    Console.ReadLine();
                }
            }
            
            
        }
        public class Habit
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string Measure {  get; set; }
            public int Quantity { get; set; }

        }
    }
}
