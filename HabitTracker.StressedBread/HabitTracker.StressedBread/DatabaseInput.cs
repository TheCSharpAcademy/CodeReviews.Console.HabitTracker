using Microsoft.Data.Sqlite;

namespace HabitTracker.StressedBread
{
    internal class DatabaseInput
    {
        Helpers helpers = new();
        DatabaseService databaseService = new();

        // Method to create tables if they do not exist
        internal void CreateTable()
        {
            string createHabit = @"CREATE TABLE IF NOT EXISTS habits ( 
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    HabitName TEXT,
                                    Unit TEXT
                                    )";

            string createHabitData = @"CREATE TABLE IF NOT EXISTS habit_data ( 
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    HabitID INTEGER,
                                    Date TEXT,
                                    Quantity INTEGER,
                                    FOREIGN KEY (HabitID) REFERENCES habits(ID) ON DELETE CASCADE
                                    )";

            databaseService.ExecuteCommand(createHabit);
            databaseService.ExecuteCommand(createHabitData);
        }

        // Method to insert a new habit
        internal void InsertHabit()
        {
            Console.Clear();

            string? name = helpers.ValidateString("Input the name of the habit.");
            string? unit = helpers.ValidateString("Input the unit of measurement.");

            string commandText = @"INSERT INTO habits (HabitName, Unit)
                           VALUES (@name, @unit)
                           ";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"name", name),
                    new SqliteParameter(@"unit", unit)
                };

            databaseService.ExecuteCommand(commandText, parameters);
            Console.WriteLine("Successfully inserted! Press any key to return to menu.");
            Console.ReadKey();
        }

        // Method to insert data for a specific habit
        internal void InsertHabitData()
        {
            Console.Clear();

            int index = helpers.ValidateInt("Choose which habit you want to insert data into by ID.");
            if (!HabitExists(index.ToString(), "habits")) return;

            string? date = helpers.GetDateInput();
            int quantity = helpers.ValidateInt("Insert the quantity.");

            string commandText = @"INSERT INTO habit_data (HabitID, Date, Quantity)
                           VALUES (@index, @date, @quantity)
                           ";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"index", index),
                    new SqliteParameter(@"date", date),
                    new SqliteParameter(@"quantity", quantity)
                };

            databaseService.ExecuteCommand(commandText, parameters);
            Console.WriteLine("Successfully inserted! Press any key to return to menu.");
            Console.ReadKey();
        }

        // Method to control update operations
        internal void UpdateControl()
        {
            Console.Clear();

            Console.WriteLine("Press 1 to update habits or press 2 to edit habit data.");
            ConsoleKeyInfo input = Console.ReadKey();

            int index = helpers.ValidateInt("\nChoose which row you want to update.");

            switch (input.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    UpdateHabit(index);
                    break;

                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    UpdateHabitData(index);
                    break;

                default:
                    Console.WriteLine("Invalid input! Press any key to return to menu.");
                    Console.ReadKey();
                    break;
            }
        }

        // Method to delete a habit
        internal void Delete()
        {
            Console.Clear();

            int index = helpers.ValidateInt("Choose which row you want to update.");
            if (!HabitExists(index.ToString(), "habits")) return;

            string deleteText = @"DELETE FROM drinking_water
                                      WHERE ID = @index";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"index", index),
                };

            databaseService.ExecuteCommand(deleteText, parameters);
            Console.WriteLine("Successfully deleted! Press any key to return to menu.");
            Console.ReadKey();
        }

        // Method to control view operations
        internal void ViewControl()
        {
            Console.Clear();

            Console.WriteLine("Press 1 to view habits or press 2 to view habit data.");
            ConsoleKeyInfo input = Console.ReadKey();
            Console.WriteLine();
            string? tableName = null;
            switch (input.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    tableName = "habits";
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    tableName = "habit_data";
                    break;
                default:
                    Console.WriteLine("Invalid input! Press any key to return to menu.");
                    Console.ReadKey();
                    return;
            }

            switch (tableName)
            {
                case "habits":
                    ViewAll(tableName);
                    break;
                case "habit_data":
                    ViewAll(tableName);
                    break;
            }

            Console.WriteLine("\nDo you wish to filter? Press Y for yes or N to return to menu");
            input = Console.ReadKey();
            if (input.Key == ConsoleKey.N)
            {
                return;
            }
            else if (input.Key == ConsoleKey.Y)
            {
                Console.WriteLine(@"
Press 1 to filter by habit name.
Press 2 to filter by date.
Press 3 to filter by quantity.
Press 4 to filter by habit, date and quantity.
    ");

                input = Console.ReadKey();

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        FilterByHabit();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        FilterByDate();
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        FilterByQuantity();
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        FilterByHabitAndValue();
                        break;

                    default:
                        Console.WriteLine("\nInvalid Input! Press any key to reset.");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input! Press any key to return to menu.");
                Console.ReadKey();
            }
        }

        // Method to update a habit
        internal void UpdateHabit(int index)
        {
            if (!HabitExists(index.ToString(), "habits")) return;

            string? name = helpers.ValidateString("Update the name of the habit.");
            string? unit = helpers.ValidateString("Update the unit of measurement.");

            string commandText = @"UPDATE habits
                                      SET HabitName = @name, Unit = @unit
                                      WHERE ID = @index
                                      ";

            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"index", index),
                    new SqliteParameter(@"name", name),
                    new SqliteParameter(@"unit", unit)
                };

            databaseService.ExecuteCommand(commandText, parameters);
            Console.WriteLine("Successfully updated! Press any key to return to menu.");
            Console.ReadKey();
        }

        // Method to update habit data
        internal void UpdateHabitData(int index)
        {
            if (!HabitExists(index.ToString(), "habit_data")) return;

            string? date = helpers.GetDateInput();
            string? quantity = helpers.ValidateString("Update the quantity of measurement.");

            string commandText = @"UPDATE habit_data
                                      SET Date = @date, Quantity = @quantity
                                      WHERE ID = @index
                                      ";

            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"index", index),
                    new SqliteParameter(@"date", date),
                    new SqliteParameter(@"quantity", quantity)
                };

            databaseService.ExecuteCommand(commandText, parameters);
            Console.WriteLine("Successfully updated! Press any key to return to menu.");
            Console.ReadKey();
        }

        // Method to check if a habit exists
        internal bool HabitExists(string? value, string tableName, bool isName = false)
        {
            string selectText;
            if (isName)
            {
                selectText = $"SELECT ID FROM {tableName} WHERE HabitName = @value";
            }
            else
            {
                selectText = $"SELECT ID FROM {tableName} WHERE ID = @value";
            }

            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"value", value),
                };

            using (SqliteDataReader reader = databaseService.ExecuteRead(selectText, parameters))
            {
                if (reader.Read())
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("Habit doesn't exist! Press any key to return to menu.");
                    Console.ReadKey();
                    return false;
                }
            }
        }

        // Method to view all records from a table
        internal void ViewAll(string tableName)
        {
            string? commandText = "";

            if (tableName == "habits")
            {
                commandText = $@"SELECT * FROM {tableName}
                                       ";
            }
            else if (tableName == "habit_data")
            {
                commandText = $@"SELECT habit_data.ID, habits.HabitName AS Habit, habit_data.Date, habit_data.Quantity
                                            FROM {tableName}
                                            JOIN habits ON habit_data.HabitID = habits.ID;
                                       ";
            }
            else
            {
                Console.WriteLine("Table does not exist! Press any key to return to menu.");
                Console.ReadKey();
                return;
            }

            helpers.DisplayData(commandText);
        }

        // Method to filter records by habit name
        internal void FilterByHabit()
        {
            string? habitName = helpers.ValidateString("\nInput the name of the habit you want to filter by.");
            string commandText = $@"SELECT habit_data.ID, habits.HabitName AS Habit, habit_data.Date, habit_data.Quantity
                                            FROM habit_data
                                            JOIN habits ON habit_data.HabitID = habits.ID
                                            WHERE habits.HabitName = @habitName
                                       ";

            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"habitName", habitName),
                };

            if (!HabitExists(habitName, "habits", true)) return;

            helpers.DisplayData(commandText, parameters);

            Console.WriteLine("Press any key to return to menu.");
            Console.ReadKey();
        }

        // Method to filter records by date range
        internal void FilterByDate()
        {
            string? startDate = helpers.ValidateDate("\nInput start of the date in dd/mm/yyy to filter by.");
            string? endDate = helpers.ValidateDate("\nInput end of the date in dd/mm/yyy to filter by.");
            string commandText = $@"SELECT habit_data.ID, habits.HabitName AS Habit, habit_data.Date, habit_data.Quantity
                                            FROM habit_data
                                            JOIN habits ON habit_data.HabitID = habits.ID
                                            WHERE Date BETWEEN @startDate AND @endDate
                                       ";

            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"startDate", startDate),
                    new SqliteParameter(@"endDate", endDate)
                };

            helpers.DisplayData(commandText, parameters);

            Console.WriteLine("Press any key to return to menu.");
            Console.ReadKey();
        }

        // Method to filter records by quantity range
        internal void FilterByQuantity()
        {
            int minQuantity = helpers.ValidateInt("\nInput the min quantity you want to filter by.");
            int maxQuantity = helpers.ValidateInt("\nInput the max quantity you want to filter by.");
            string commandText = $@"SELECT habit_data.ID, habits.HabitName AS Habit, habit_data.Date, habit_data.Quantity
                                            FROM habit_data
                                            JOIN habits ON habit_data.HabitID = habits.ID
                                            WHERE Quantity BETWEEN @minQuantity and @maxQuantity
                                       ";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"minQuantity", minQuantity),
                    new SqliteParameter(@"maxQuantity", maxQuantity)
                };

            helpers.DisplayData(commandText, parameters);

            Console.WriteLine("Press any key to return to menu.");
            Console.ReadKey();
        }

        // Method to filter records by habit name, date range, and quantity range
        internal void FilterByHabitAndValue()
        {
            string? habitName = helpers.ValidateString("\nInput the name of the habit you want to filter by.");
            string? startDate = helpers.ValidateDate("Enter the start date (dd/MM/yyyy):");
            string? endDate = helpers.ValidateDate("Enter the end date (dd/MM/yyyy):");
            int minQuantity = helpers.ValidateInt("Enter the minimum quantity:");
            int maxQuantity = helpers.ValidateInt("Enter the maximum quantity:");

            string commandText = @"SELECT habit_data.ID, habits.HabitName AS Habit, habit_data.Date, habit_data.Quantity
                                       FROM habit_data
                                       JOIN habits ON habit_data.HabitID = habits.ID
                                       WHERE habits.HabitName = @habitName
                                       AND habit_data.Date BETWEEN @startDate AND @endDate
                                       AND habit_data.Quantity BETWEEN @minQuantity AND @maxQuantity
                                  ";

            List<SqliteParameter> parameters = new List<SqliteParameter>()
                {
                    new SqliteParameter(@"habitName", habitName),
                    new SqliteParameter(@"startDate", startDate),
                    new SqliteParameter(@"endDate", endDate),
                    new SqliteParameter(@"minQuantity", minQuantity),
                    new SqliteParameter(@"maxQuantity", maxQuantity)
                };
            if (!HabitExists(habitName, "habits", true)) return;

            helpers.DisplayData(commandText, parameters);

            Console.WriteLine("Press any key to return to menu.");
            Console.ReadKey();
        }
    }
}
