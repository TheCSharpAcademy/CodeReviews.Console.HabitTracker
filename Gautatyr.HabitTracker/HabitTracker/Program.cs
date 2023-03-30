using Microsoft.Data.Sqlite;
using System.Globalization;

internal class Program
{
    static string connectionString = "Data Source=HabitTracker.db";

    private static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS DRINKING_WATER (
                                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                 Date TEXT,
                                 Quantity INTEGER)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        GetUserInput();

        static void GetUserInput()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do ?\n");
                Console.WriteLine("- Type 0 to Close the Application."); //DONE
                Console.WriteLine("- Type 1 to View All Records.");  //DONE
                Console.WriteLine("- Type 2 to Insert Record."); //DONE
                Console.WriteLine("- Type 3 to Delete Record.");
                Console.WriteLine("- Type 4 to Update Record.");
                Console.WriteLine("- Type 5 to create a new habit.\n\n"); //DONE

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye !");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords(ChooseTable());
                        break;
                    case "2":
                        Insert(ChooseTable());
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    case "5":
                        HabitCreation();
                        break;
                    default:
                        Console.WriteLine("\n|---> Invalid Command. Please type a number from 0 to 5 <---|\n");
                        break;
                }
            }
        }

        static void Insert(string tableName)
        {
            Console.Clear();

            string habitUnit = HabitUnit(tableName);

            string date = GetDateInput();

            int quantity = GetNumberInput($"Please insert {habitUnit}" +
                                          "\n\nType 0 to return to the menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                      $"INSERT INTO {tableName}(date, {habitUnit}) VALUES('{date}', {quantity})";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n|---> Invalid number <---|\n");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }

        static string GetDateInput()
        {
            Console.Clear();

            Console.WriteLine("Please insert the date: (Format: dd-mm-yy).\n\nType 0 to return to main menu");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n|---> Invalid date (Format: dd-mm-yy) Try again <---|\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        static void GetAllRecords(string tableName)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM {tableName}";

                List<Habit> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new Habit
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Unit = reader.GetInt32(2)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("-----------------------------------------------------\n");
                foreach (var habit in tableData)
                {
                    Console.WriteLine($"{habit.Id} - {habit.Date.ToString("dd-MMM-yyyy")} - {HabitUnit(tableName)}: {habit.Unit}");
                }
                Console.WriteLine("\n-----------------------------------------------------");
            }
        }

        static void Delete()
        {
            string tableName = ChooseTable();

            GetAllRecords(tableName);

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from {tableName} WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n|---> Record with Id {recordId} doesn't exist <---|\n");
                    Delete();
                }
            }
            Console.WriteLine($"\n\nRecord with Id {recordId} has been deleted.\n\n");
            GetUserInput();
        }

        static void Update()
        {
            string tableName = ChooseTable();

            GetAllRecords(tableName);

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you would like to update, or type 0 to get back to the menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n|---> Record with Id {recordId} doesn't exist <---|\n");
                    connection.Close();
                    Update();
                }

                string date = GetDateInput();

                string habitUnit = HabitUnit(tableName);

                int quantity = GetNumberInput($"\n\nPlease insert number of {habitUnit}.");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE {tableName} SET date = '{date}', {habitUnit} = {quantity} WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        static void HabitCreation()
        {
            Console.WriteLine("\nPlease enter the name of your new habit or type 0 to get back to the menu\n");
            string newHabit = Console.ReadLine().ToUpper();

            if (newHabit == "0") GetUserInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{newHabit}';)";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 1)
                {
                    Console.WriteLine($"\n|---> This habit already exists ! <---|\n");
                    connection.Close();
                    HabitCreation();
                    GetUserInput();
                }
            }

            Console.WriteLine("\nPlease enter the unit of measurement of your new habit or type 0 to get back to the menu\n");
            string unit = Console.ReadLine();

            if (unit == "0") GetUserInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {newHabit} (
                                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                 Date TEXT,
                                 {unit} INTEGER)";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Console.WriteLine("\nYour new habit has been created !\n");
        }

        static string ChooseTable()
        {
            Console.WriteLine("\nType the name of the table you wish to see");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name IS NOT 'sqlite_sequence'";

                List<string> tables = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
                else
                {
                    Console.WriteLine("No table found");
                }

                connection.Close();

                Console.WriteLine("-----------------------------------------------------\n");
                foreach (var table in tables)
                {
                    Console.WriteLine(table);
                }
                Console.WriteLine("\n-----------------------------------------------------\n");

                string tableName = Console.ReadLine().ToUpper();
                if (!tables.Contains(tableName))
                {
                    Console.WriteLine("\nThe table doesn't exist, try again.\n");
                    tableName = ChooseTable();
                }
                return tableName;
            }
        }

        static string HabitUnit(string habitName)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT name FROM PRAGMA_TABLE_INFO('{habitName}')";

                SqliteDataReader reader = tableCmd.ExecuteReader();
                reader.Read();
                reader.Read();
                reader.Read();
                return reader.GetString(0);
            }
        }
    }
}

public class Habit
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Unit { get; set; }
}