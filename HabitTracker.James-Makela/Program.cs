using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker.James_Makela
{
    internal static class Program
    {
        private const string ConnectionString = @"Data Source=habit-tracker.db";

        private static void Main()
        {
            if (GetTableNames() == null)
            {
                AddHabit();
            }

            GetUserInput();
        }

        // Main menu
        private static void GetUserInput()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.Clear();
                Console.WriteLine("+--------------------------------------+");
                Console.WriteLine("| MAIN MENU                            |");
                Console.WriteLine("| What would you like to do?           |");
                Console.WriteLine("|\t- 0 to close Application       |");
                Console.WriteLine("|\t- 1 to View All Records        |");
                Console.WriteLine("|\t- 2 to Insert a Record         |");
                Console.WriteLine("|\t- 3 to Delete a Record         |");
                Console.WriteLine("|\t- 4 to Update a Record         |");
                Console.WriteLine("|\t- 5 to Add a New Habit         |");
                Console.WriteLine("|\t- 6 to Remove a Habit          |");
                Console.WriteLine("|\t- 7 to Query a Table           |");
                Console.WriteLine("+--------------------------------------+\n");

                char commandInput = Console.ReadKey(true).KeyChar;

                switch(commandInput)
                {
                    case '0':
                        Console.WriteLine("\nGoodbye\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case '1':
                        GetAllRecords();
                        Console.WriteLine("Press any key to go back to the menu.");
                        Console.ReadKey();
                        break;
                    case '2':
                        Insert();
                        break;
                    case '3':
                        Delete();
                        break;
                    case '4':
                        Update();
                        break;
                    case '5':
                        AddHabit();
                        break;
                    case '6':
                        RemoveHabit();
                        break;
                    case '7':
                        QueryTable();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4,\n");
                        break;
                } 
            }
        }

        private static void AddHabit()
        {
            string?[] habitDetails = GetHabitDetails();
            string? habitName = habitDetails[0];
            string? habitUnit = habitDetails[1];
            CreateTable(habitName, habitUnit);
        }

        private static string?[] GetHabitDetails()
        {
            // Need input validation here
            string?[] details = {"", ""};
            Console.WriteLine("\nWhat habit would you like to track?");
            while (details[0] == "")
            {
                string? userInput = Console.ReadLine();
                if (userInput is not null && char.IsLetter(userInput[0]))
                {
                    details[0] = userInput;
                }
                else
                {
                    Console.WriteLine("Invalid table name");
                }
            }
            Console.WriteLine("What is the unit you would like to use to track that habit?");
            while (details[1] == "")
            {
                string? userInput = Console.ReadLine();
                if (userInput is not null && char.IsLetter(userInput[0]))
                {
                    details[1] = userInput;
                }
                else
                {
                    Console.WriteLine("Invalid unit name");
                }
            }
            return details;
        }

        private static bool GetAllRecords(string? table=null)
        {
            table ??= ChooseTable("view");
            
            Console.Clear();
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                
                tableCmd.CommandText =
                    $"SELECT * FROM {table} ";
                
                List<BicycleRides> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        tableData.Add(
                        new BicycleRides
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return false;
                }

                connection.Close();

                Console.WriteLine("+------------------------------+");

                foreach (BicycleRides dw in tableData)
                {
                    Console.WriteLine($"| {dw.Id} - {dw.Date:dd-MM-yyyy} - Minutes: {dw.Quantity} |");
                    Console.WriteLine("+------------------------------+");
                }
            }
            return true;
        }

        private static string[] GetTableNames()
        {
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    """
                    SELECT 
                        name
                    FROM 
                        sqlite_master
                    WHERE 
                        type ='table' AND 
                        name NOT LIKE 'sqlite_%'
                        AND name <> 'tableunits'
                    """;

                List<TableList> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        tableData.Add(
                            new TableList
                            {
                                TableNames = reader.GetString(0)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No data found");
                    Console.WriteLine("Press any key to add a habit, or press 0 to go to the main menu");
                    if (Console.ReadKey().KeyChar == '0')
                    {
                        GetUserInput();
                    }
                    else
                    {
                        AddHabit();
                    }
                    
                }

                connection.Close();

                string[] tableNames = new string[tableData.Count];
                for (int i = 0; i < tableData.Count; i++)
                {
                    foreach (TableList unused in tableData)
                    {
                        tableNames[i] = tableData[i].TableNames!;
                    }
                }

                return tableNames;
            }
        }

        private static string ChooseTable(string action)
        {
            Console.Clear();
            string[] tableNames = GetTableNames();
            Console.WriteLine($"Please select a table to {action}");
            PrintTableNames(tableNames);
            int chosenTable = -1;
            while (chosenTable < 1 || chosenTable > tableNames.Length)
            {
                int.TryParse(Console.ReadKey().KeyChar.ToString(), out chosenTable);
                if (chosenTable < 1 || chosenTable > tableNames.Length)
                {
                    Console.WriteLine("\nPlease enter a valid table number");
                }
            }

            string table = tableNames[chosenTable - 1];
            return table;
        }

        private static void InsertScreen(string units)
        {
            Console.WriteLine("+--------------------------------------+");
            Console.WriteLine("| Enter date (dd-mm-yy):               |");
            Console.WriteLine($"| {units,21}:               |");
            Console.WriteLine("+--------------------------------------+");
            Console.WriteLine("Enter 0 to return to the main menu");
        }

        private static void CreateTable(string? name, string? unit)
        {
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"""
                     CREATE TABLE IF NOT EXISTS {name} (
                                             id INTEGER PRIMARY KEY AUTOINCREMENT,
                                             date TEXT,
                                             {unit} INTEGER
                                             );
                                             
                     CREATE TABLE IF NOT EXISTS tableunits (
                                            tablename TEXT,
                                            unit TEXT
                                            );
                     
                     INSERT INTO tableunits VALUES ('{name}', '{unit}')
                     """;

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static void PrintTableNames(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"\t- {i + 1} {names[i]}");
            }
        }

        private static void Insert()
        {
            string tableName = ChooseTable("insert");
            string unitName = GetUnitName(tableName);
            Console.Clear();
            InsertScreen(unitName);
            Console.SetCursorPosition(25, 1);
            string date = GetDateInput(unitName);
            Console.SetCursorPosition(25, 2);
            int units = GetNumberInput();
            

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = 
                    $"INSERT INTO {tableName}(date, {unitName}) VALUES('{date}', {units})";

                tableCmd.ExecuteNonQuery();
                connection .Close();
            }
        }

        private static string GetDateInput(string unit)
        {
            string? dateInput = Console.ReadLine();
            
            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.SetCursorPosition(0, Console.CursorTop - 2);
                InsertScreen(unit);
                Console.WriteLine("\nInvalid date. (Format: dd-mm-yy). Type 0 to return to the main menu or try again:");
                Console.SetCursorPosition(25, Console.CursorTop - 7);
                dateInput = GetDateInput(unit);
            }
            

            return dateInput;
        }

        private static int GetNumberInput()
        {
            int.TryParse(Console.ReadLine(), out int number);

            if (number == 0) GetUserInput();

            return number;
        }

        private static void Delete()
        {
            string tableName = ChooseTable("delete data from");
            Console.Clear();
            if (GetAllRecords(tableName))
            {
                Console.WriteLine(
                    "Please type the ID of the record you want to delete, or type 0 to return to the main menu\n\n");
                var recordId = GetNumberInput();

                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = $"DELETE FROM {tableName} WHERE id = '{recordId}'";

                    int rowCount = tableCmd.ExecuteNonQuery();

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"\n\nRecord with ID {recordId} does not exist. \n\n");
                        Delete();
                    }
                }

                Console.WriteLine($"\n\nRecord with ID {recordId} was deleted.\n\n");
            }
            else
            {
                Console.WriteLine("Press any key to return to the main menu");
                Console.ReadKey();
            }
        }

        private static void Update()
        {
            Console.WriteLine("Which table would you like to update?");
            string table = ChooseTable("update");
            string unit = GetUnitName(table);
            Console.Clear();
            if (GetAllRecords(table))
            { 
                Console.WriteLine("\n\nPlease type the ID of the record you would like to update. Or type 0 to return to the main menu\n\n");
                var recordId = GetNumberInput();

                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();

                    var checkCmd = connection.CreateCommand();
                    checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {table} WHERE id = {recordId})";
                    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (checkQuery == 0)
                    {
                        Console.WriteLine($"\n\nRecord with ID {recordId} does not exist\n\n");
                        connection.Close();
                        Update();
                    }


                    InsertScreen(unit);
                    Console.SetCursorPosition(25, Console.CursorTop - 4);
                    string date = GetDateInput(unit);
                    Console.SetCursorPosition(25, Console.CursorTop);
                    int units = GetNumberInput();

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"UPDATE {table} SET date = '{date}', {unit} = {units} WHERE id = {recordId}";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
            else
            {
                Console.WriteLine("No data found, press any key to return");
                Console.ReadKey();
            }
        }

        private static string GetUnitName(string table)
        {
            string unitName = "";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"""
                     SELECT unit FROM tableunits WHERE tablename='{table}'
                     """;

                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    unitName = reader.GetString(0);
                }
                connection.Close();
            }

            return unitName;
        }

        private static void QueryTable()
        {
            string table = ChooseTable("query");
            string unit = GetUnitName(table);
            string spaces = " ".PadRight(31 - table.Length);
            Console.Clear();
            Console.WriteLine("+-------------------------------------------+");
            Console.WriteLine("| What query would you like to run on       |");
            Console.WriteLine($"| the {table} table?{spaces}|");
            Console.WriteLine("|\t- 0 Exit to main menu               |");
            Console.WriteLine($"|\t- 1 View average {unit,-16}   |");
            Console.WriteLine($"|\t- 2 View highest {unit,-18} |");
            Console.WriteLine($"|\t- 3 View total {unit,-21}|");
            Console.WriteLine("+-------------------------------------------+");
            
            char commandInput = Console.ReadKey(true).KeyChar;

            switch (commandInput)
            {
                case '0':
                    break;
                case '1':
                    QueryStats(table, "AVG");
                    Console.ReadKey();
                    break;
                case '2':
                    QueryStats(table, "MAX");
                    Console.ReadKey();
                    break;
                case '3':
                    QueryStats(table, "SUM");
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    QueryTable();
                    break;
            }
        }

        private static void QueryStats(string table, string queryType)
        {
            Console.Clear();
            string units = GetUnitName(table);
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"""
                     SELECT
                         {queryType}({units})
                     FROM
                         {table}
                     """;
                SqliteDataReader reader = tableCmd.ExecuteReader();
                int total = 0;
                
                if (GetAllRecords(table))
                {
                    while (reader.Read())
                    {
                        total = reader.GetInt32(0);
                    }
                }
                else
                {
                    Console.WriteLine("No data found, press any key to return");
                    return;
                }

                connection.Close();
                string query = "";
                switch (queryType)
                {
                    case "AVG":
                        query = "Average";
                        break;
                    case "MAX":
                        query = "Maximum";
                        break;
                    case "SUM":
                        query = "Total";
                        break;
                }
                Console.WriteLine($"{query} {units} {table} = {total}");
                Console.WriteLine("Press any key to return to the main menu");
            }
        }

        private static void RemoveHabit()
        {
            string tableName = ChooseTable("remove");
            Console.Clear();
            GetAllRecords(tableName);

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DROP TABLE {tableName}";

                Console.WriteLine($"Are you sure you want to remove the habit {tableName}? (y/n)");
                if (Console.ReadKey().KeyChar == 'y')
                {
                    tableCmd.ExecuteNonQuery();
                    Console.WriteLine("\nTable deleted.");
                    Console.WriteLine("Press any key to return to the main menu.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nTable not deleted, press any key to return to the main menu");
                    Console.ReadKey();
                }
            }
        }
    }
    public class BicycleRides
    {
        public int Id { get; init;}
        public DateTime Date { get; init; }
        public int Quantity { get; init; }
    }

    public class TableList
    {
        public string? TableNames { get; init; }
    }
}