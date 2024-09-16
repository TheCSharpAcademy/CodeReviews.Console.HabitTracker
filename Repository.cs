using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Text.RegularExpressions;

namespace habit_tracker

{
    public static class Repository
    {
        static string currentHabitName = "C_Sharp_Lessons";
        static string currentUnit = "lessons";
        static List<string> habitNames = new List<string>();
        static string connectionString = @"Data Source=habit-Tracker.db";
        static bool ReviewInfoNeeded = true;

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
                    string menuInput = Console.ReadLine();
                    if (int.TryParse(menuInput, out ChosenOption))
                    {
                        invalid = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Please, enter number from 0 to 7.\n");
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
                        GetCurrentUnitName();
                        GetRecords();
                        if (ReviewInfoNeeded)
                            GetReviewInfo();
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
                        CreateHabit(false);

                        break;

                    case 6:
                        ChangeCurrentHabbit();
                        break;

                    case 7:
                        Console.Clear();
                        GetHabitTables();
                        Console.WriteLine("Press any key to return back to main menu");
                        Console.ReadKey();
                        break;
                    case 8:
                        Console.Clear();
                        int generatedRecordsAmount = GetNumberInput("\n\n**Enter 0 to go to main menu**\nPlease, enter the number of records you want to generate(maximum is 500):\n", 0, 500);
                        int generatedRecordsMaxQuantity = GetNumberInput("\n\n**Enter 0 to go to main menu**\nPlease, enter the max quantity number for records you want to generate(maximum is 100 000):\n", 0, 100000);
                        int generatedRecordsYear = GetNumberInput("\n\n**Enter 0 to go to main menu**\nPlease, enter the year you want to generate records in (from 2000 to 2024):\n", 2000, 2024);
                        GenerateRandomRecords(generatedRecordsAmount, generatedRecordsMaxQuantity, generatedRecordsYear);
                        Console.WriteLine($"\n{generatedRecordsAmount} records are succesfully added.\n");
                        Console.WriteLine("Press any key to return back to main menu");
                        Console.ReadKey();
                        break;


                    default:
                        Console.WriteLine("\nInvalid command. Please, enter number from 0 to 7\n");
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
                Console.WriteLine("\n\n**Enter 0 to go to main menu**\nInvalid date. Format: dd-mm-yy. Or Enter \"today\":");
                dateInput = Console.ReadLine();
                if (dateInput == "today")
                    dateInput = DateTime.Now.ToString("dd-MM-yy");
            }
            return dateInput;
        }
        public static int GetNumberInput(string message, int numberMinLimit, int numberMaxLimit)
        {
            Console.WriteLine(message);
            string? numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();
            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) <= numberMinLimit || Convert.ToInt32(numberInput) > numberMaxLimit)
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
                tableCmd.CommandText = $"SELECT * FROM {currentHabitName}_table";
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
                            Quantity = reader.GetInt32(3),
                            Unit = reader.GetString(4)
                        });
                    }
                    ReviewInfoNeeded = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("No records found!\n");
                    ReviewInfoNeeded = false;
                    return;
                }

                connection.Close();
                Console.Clear();
                Console.WriteLine("\n------------------------------------------------\n\n");
                foreach (var cl in tableData)
                {
                    Console.WriteLine($"{cl.Id} - {cl.Date.ToString("dd-MMM-yyyy")} - Quantity: {cl.Quantity} {cl.Unit}");
                }
                Console.WriteLine("\n\n------------------------------------------------\n");



            }
        }
        public static void InsertRecord()
        {
            string date = GetDateInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the date: (format: dd-mm-yy) or enter \"today\" to insert current date.\n");
            int quantity = GetNumberInput($"\n\n**Enter 0 to go to main menu**\nPlease, insert the number of {currentUnit}: (number must be integer, no decimals allowed)\n", 0, Int32.MaxValue);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO {currentHabitName}_table (date, habit, quantity, {currentUnit}) VALUES('{date}','{currentHabitName}','{quantity}', '{currentUnit}')";

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
            var recordId = GetNumberInput("**Enter 0 to go to main menu**\nPlease, enter the Id of the record that you want to delete", 0, Int32.MaxValue);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from {currentHabitName} WHERE Id = '{recordId}'";

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
            var recordId = GetNumberInput("**Enter 0 to go to main menu**\nPlease, enter the Id of the record that you want to update", 0, Int32.MaxValue);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCMD = connection.CreateCommand();
                checkCMD.CommandText = $"SELECT EXISTS(SELECT 1 FROM {currentHabitName} WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCMD.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"record with Id {recordId} doesn't exist.\n\n");
                    Thread.Sleep(1500);
                    connection.Close();
                    UpdateRecord();
                }

                string date = GetDateInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the date: (format: dd-mm-yy).\n");
                int quantity = GetNumberInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the number of lessons: (number must be integer, no decimals allowed)\n", 0, Int32.MaxValue);

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE {currentHabitName} SET date = '{date}', quantity = '{quantity}' WHERE Id = '{recordId}'";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Console.WriteLine($"\nRecord with Id {recordId} was updated succesfully.\n");
            Console.WriteLine("Press any key to return back to main menu");
            Console.ReadKey();

        }
        public static void CreateHabit(bool firstLaunch)
        {
            string userHabit = "";
            string userUnit = "";
            string connectionString = @"Data Source=habit-Tracker.db";
            if (!firstLaunch)
            {
                userHabit = GetValidNameInput(userHabit, "habit");
                userUnit = GetValidNameInput(userUnit, "unit");
            }
            else
            {
                userHabit = "C_Sharp_Lessons"; //this is main habit tracked, thats why its hardcoded to be created on the very first launch
                userUnit = "lessons";
            }


            using (var connection = new SqliteConnection(connectionString))

            {
                connection.Open();
                var tableCmd = connection.CreateCommand();


                tableCmd.CommandText = @$"Create Table if not exists {userHabit}_table (
                                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                Habit TEXT,
                                                Date TEXT, 
                                                Quantity INTEGER,
                                                {userUnit} TEXT
                                                )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            if (!firstLaunch)
            {
                Console.WriteLine($"Habit {userHabit} with \"{userUnit}\" is created!");
                Console.WriteLine("Now you can select it from menu");
                Thread.Sleep(2500);
            }

        }
        public static string ShowCurrentHabbit()
        {
            return currentHabitName;
        }
        public static void ChangeCurrentHabbit()
        {
            Console.Clear();
            GetHabitTables();
            Console.WriteLine($"Current selected habit is: {ShowCurrentHabbit()}\n");

            int selectedHabit = GetNumberInput($"\n**Enter 0 to go to main menu**\nselect the habit by entering its number:\n", 0, Int32.MaxValue);
            while (!(selectedHabit <= habitNames.Count))
            {
                Console.WriteLine("Inputted number does not exist. Select the habit by entering its number: ");
                selectedHabit = GetNumberInput("Inputted number does not exist. Select the habit by entering its number: ", 0, Int32.MaxValue);
            }
            currentHabitName = habitNames[selectedHabit - 1];
            currentUnit = GetCurrentUnitName();

        }
        public static void GetHabitTables()
        {
            Console.WriteLine("List of all your habits:\n");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT name FROM sqlite_schema WHERE type='table';";
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
                int i = 1;

                foreach (var td in tableData)
                {
                    string nameInTable = td.TableName.Remove(td.TableName.Length - 6);  // This is to remove "_table" on the end of the Habit name.
                    string displayedName = nameInTable;
                    displayedName = i + " " + displayedName.Replace('_', ' ');
                    Console.WriteLine(displayedName);
                    Console.WriteLine();
                    habitNames.Add(nameInTable); //numbers in list are dynamic, so we just show the number to user but do not store it. e.g. we can add alphabetical sort if needed.
                    i++;
                }
                Console.WriteLine("\n\n------------------------------------------------\n");
            }
        }
        public static string GetValidNameInput(string userInput, string name)
        {
            bool validInput = false;
            string checkPattern = @"^[a-zA-Z0-9\s]+$"; //symbols that are not allowed 
            Console.WriteLine($"Enter your {name} name (Use only letters, numbers and spaces, no symbols are allowed:\n");

            while (!validInput)
            {
                userInput = Console.ReadLine();
                if (Regex.IsMatch(userInput, checkPattern) && userInput != null) //check if the input matches the pattern
                {
                    userInput = userInput.Replace(' ', '_');
                    validInput = true;
                }
                else
                    Console.WriteLine("Use only letters, numbers and spaces, no symbols are allowed:\n");
            }
            return userInput;
        }
        public static void GetReviewInfo()
        {
            Int64 rowCount;
            object sumOfHabit;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"SELECT COUNT(*) FROM {currentHabitName}_table";

                rowCount = (Int64)tableCmd.ExecuteScalar();


                tableCmd.CommandText = $"SELECT SUM(Quantity) FROM {currentHabitName}_table";

                sumOfHabit = tableCmd.ExecuteScalar();

                connection.Close();


            }
            Console.WriteLine($"You did {currentHabitName} {rowCount} times. Your total {currentUnit} number is {sumOfHabit}!\n");
        }
        public static string GetCurrentUnitName()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"PRAGMA table_info({currentHabitName}_table)";

                using (var reader = tableCmd.ExecuteReader())
                {
                    int columnIndex = 0;
                    while (reader.Read())
                    {
                        if (columnIndex == 4)
                        {
                            string columnName = reader.GetString(1);
                            return columnName;
                        }
                        columnIndex++;
                    }
                    return "default-units";
                }
            }
        }
        public static DateTime GenerateRandomDate(int yearStartingFrom)
        {
            // Date Generation
            DateTime startDate = new DateTime(yearStartingFrom, 1, 1);
            DateTime endDate = DateTime.Today;

            Random random = new Random();
            int range = (endDate - startDate).Days;
            int randomDays = random.Next(range);
            return startDate.AddDays(randomDays);
        }
        public static void GenerateRandomRecords(int amount, int maxQuantity, int yearStartingFrom)
        {
            using (var connection = new SqliteConnection(connectionString))

            {
                connection.Open();

                for (int i = 1; i <= amount; i++)
                {
                    //Generating random date
                    string dateGenerated = GenerateRandomDate(yearStartingFrom).ToString("dd-MM-yy");

                    //Generating random quantity
                    Random random = new Random();
                    int quantityGenerated = random.Next(0, maxQuantity);

                    //Inserting into the database
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"INSERT INTO {currentHabitName}_table (date, habit, quantity, {currentUnit}) VALUES('{dateGenerated}','{currentHabitName}','{quantityGenerated}', '{currentUnit}')";
                    tableCmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
