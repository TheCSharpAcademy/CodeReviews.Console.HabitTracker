using Microsoft.Data.Sqlite;
using System;
using System.Globalization;
using System.Security;

namespace habit_tracker

{
    public static class Repository
    {
        static string currentHabit = "C_Sharp_Lessons";
        static string connectionString = @"Data Source=habit-Tracker.db";
        public static void GetUserInput()
        {

            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Menu.DisplayMenu();
                
                bool invalid;
                int ChosenOption;
                do
                {
                    invalid = true;
                    String menuInput = Console.ReadLine();
                    if (int.TryParse(menuInput, out ChosenOption))
                    {
                        invalid = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Please, enter number from 0 to 4.\n");
                    }
                } while (invalid);

                switch (ChosenOption)
                {
                    case 0:
                        Console.WriteLine("Bye Bye!");
                        Thread.Sleep(1000);
                        closeApp = true;
                        break;

                    case 1:
                        GetRecords();
                        Console.WriteLine("Press any key to return back to main menu");
                        Console.ReadKey();
                        break;

                    case 2:
                        InsertRecord();
                        break;

                    case 3:
                        DeleteRecord();
                        Console.Clear();
                        break;

                    case 4:
                        UpdateRecord();
                        break;

                    case 5:
                        CreateTable(false);

                        break;

                    case 6:
                        ChangeCurrentHabbit();
                        Console.WriteLine("Press any key to return back to main menu");
                        Console.ReadKey();
                        break;

                    case 7:
                        GetHabitTables();
;
                        break;

                    default:
                        Console.WriteLine("\nInvalid command. Please, enter number from 0 to 4\n");
                        Thread.Sleep(1500);
                        break;
                }
            }
        }
        public static string GetDateInput(string message)
        {
            Console.WriteLine(message);
            string? dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();
            else if (dateInput == "today")
                dateInput = DateTime.Now.ToString("dd-MM-yy");

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\n**Enter 0 to go to main menu**\nInvalid date. Format: dd-mm-yy.");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string? numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();
            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) <= 0)
            {
                Console.WriteLine("\n**Enter 0 to go to main menu**\nInvalid number. Number must be integer and no decimals allowed. Enter correct number:");
                numberInput = Console.ReadLine();
            }
            int numberInputConverted = int.Parse(numberInput);
            return numberInputConverted;
        }
        public static void GetRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM {currentHabit}_table";
                List<HabitTable> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new HabitTable
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No records found");
                }

                connection.Close();
                Console.Clear();
                Console.WriteLine("\n------------------------------------------------\n\n");
                foreach (var cl in tableData)
                {
                    Console.WriteLine($"{cl.Id} - {cl.Date.ToString("dd-MMM-yyyy")} - Quantity: {cl.Quantity}");
                }
                Console.WriteLine("\n\n------------------------------------------------\n");



            }
        }
        public static void InsertRecord()
        {
            string date = GetDateInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the date: (format: dd-mm-yy) or enter \"today\" to insert current date.\n");
            int quantity = GetNumberInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the number of lessons: (number must be integer, no decimals allowed)\n");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO {currentHabit}_table (date, habit, quantity) VALUES('{date}','{currentHabit}','{quantity})')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            Console.WriteLine($"\nnew record was inserted succesfully.\n");
            Console.WriteLine("Press any key to return back to main menu");
            Console.ReadKey();
        }
        public static void DeleteRecord()
        {
            Console.Clear();
            GetRecords();
            var recordId = GetNumberInput("**Enter 0 to go to main menu**\nPlease, enter the Id of the record that you want to delete");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from {currentHabit} WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.\n");
                    Thread.Sleep(1500);
                    connection.Close();
                    DeleteRecord();
                }
            }
            Console.WriteLine($"\nRecord with Id {recordId} was deleted succesfully.\n");
            Console.WriteLine("Press any key to return back to main menu");
            Console.ReadKey();

        }
        public static void UpdateRecord()
        {
            Console.Clear();
            GetRecords();
            var recordId = GetNumberInput("**Enter 0 to go to main menu**\nPlease, enter the Id of the record that you want to update");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCMD = connection.CreateCommand();
                checkCMD.CommandText = $"SELECT EXISTS(SELECT 1 FROM {currentHabit} WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCMD.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"record with Id {recordId} doesn't exist.\n\n");
                    Thread.Sleep(1500);
                    connection.Close();
                    UpdateRecord();
                }

                string date = GetDateInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the date: (format: dd-mm-yy).\n");
                int quantity = GetNumberInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the number of lessons: (number must be integer, no decimals allowed)\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE {currentHabit} SET date = '{date}', quantity = '{quantity}' WHERE Id = '{recordId}'";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Console.WriteLine($"\nRecord with Id {recordId} was updated succesfully.\n");
            Console.WriteLine("Press any key to return back to main menu");
            Console.ReadKey();

        }
        public static void CreateTable(bool firstLaunch)
        {
            string? userHabit;
            bool showHabitCreated = false;
            string connectionString = @"Data Source=habit-Tracker.db";
            if (!firstLaunch)
            {

                Console.WriteLine("Enter your Habit name:");
                userHabit = Console.ReadLine();
                if (userHabit != null)
                    showHabitCreated = true;

            }
            else userHabit = "C_Sharp_Lessons";


            if (userHabit != null)
            {
                using (var connection = new SqliteConnection(connectionString))

                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();


                    tableCmd.CommandText = @$"Create Table if not exists {userHabit}_table (
                                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                Habit TEXT,
                                                Date TEXT, 
                                                Quantity INTEGER
                                                )";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }

                if (showHabitCreated)
                {
                    Console.WriteLine($"Habit {userHabit} is created!");
                    Console.WriteLine("Now you can select it from menu");
                    Thread.Sleep(2500);
                }
            }
        }
        public static string ShowCurrentHabbit()
        {
            return currentHabit;
        }
        public static string ChangeCurrentHabbit()
        {
            Console.Clear();
            GetHabitTables();
            Console.WriteLine($"Current selected habit is: {ShowCurrentHabbit()}\n");
            return "hello";
        }
        public static void GetHabitTables()
        {
            Console.WriteLine("List of all your habits:\n");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT name FROM sqlite_schema WHERE type='table' ORDER BY name;";
                List<TablesList> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new TablesList
                        {
                            TableName = reader.GetString(0),
                        });
                    }
                    tableData.RemoveAll(td => td.TableName.Contains("sqlite_sequence")); //This is to remove "sqlite_sequence" table from list of users created tables.
                    
                }


                connection.Close();
                Console.WriteLine("\n------------------------------------------------\n\n");
                foreach (var td in tableData)
                {
                    string displayedName = td.TableName.Remove(td.TableName.Length - 6);  // This is to remove "_table" on the end of the Habit name.
                    displayedName = displayedName.Replace('_', ' ');
                    Console.WriteLine(displayedName);
                    Console.WriteLine();
                }
                Console.WriteLine("\n\n------------------------------------------------\n");
            }
        }   
    }
}