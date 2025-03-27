using System.Globalization;
using System.Net.Quic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Habit.Tracker
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            //program.MainMenu();
            //string choice = program.MainMenuInput();
            //if (choice == "2")
            //{
            //    program.CreateNewHabit();
            //    Console.WriteLine("Created a new Habit!");
            //    Console.ReadLine();
            //}
            //if (choice == "1")
            //{
            //    program.ListAllHabits();
            //    Console.ReadLine();
            //}
            
            //program.AddExistingHabitToMasterList(); 
            

            bool quit = false;
            while (!quit)
            {
                Console.Clear();
                Thread.Sleep(600);

                program.DataMenu();
                quit = program.UserInput();

                if (quit == false)
                {
                    Console.WriteLine("\nPress Enter to Continue.");
                    Console.ReadLine();
                }
            }
        }

        public void AddExistingHabitToMasterList()
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=habit-tracker.db"))
                {
                    connection.Open();

                    // First create the master list table if it doesn't exist
                    using (var createTableCommand = connection.CreateCommand())
                    {
                        createTableCommand.CommandText =
                            "CREATE TABLE IF NOT EXISTS habit_list (" +
                            "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                            "HabitName TEXT NOT NULL, " +
                            "TableName TEXT NOT NULL, " +
                            "CreatedDate TEXT NOT NULL)";

                        createTableCommand.ExecuteNonQuery();
                    }

                    // Insert the daily_water habit
                    using (var insertCommand = connection.CreateCommand())
                    {
                        insertCommand.CommandText =
                            "INSERT INTO habit_list (HabitName, TableName, CreatedDate) " +
                            "VALUES (@name, @table, @date)";

                        insertCommand.Parameters.AddWithValue("@name", "Daily Water");
                        insertCommand.Parameters.AddWithValue("@table", "daily_water");
                        insertCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));

                        insertCommand.ExecuteNonQuery();
                        Console.WriteLine("Added 'Daily Water' to habit list!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to habit list: {ex.Message}");
            }
        }


        public void ListAllHabits()
        {
            using (var connection = new SqliteConnection("Data Source=habit-tracker.db"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT HabitName, CreatedDate FROM habit_list ORDER BY Id";

                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\nYour Habits:");
                        Console.WriteLine("----------------------");
                        while (reader.Read())
                        {
                            int i = 1;
                            string name = reader.GetString(0);
                            string date = reader.GetString(1);
                            Console.WriteLine($"{i}. {name} (created: {date})");
                            i++;
                        }
                    }
                }
            }
        }

        private void SaveHabitToMasterList(string habitName)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=habit-tracker.db"))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"CREATE TABLE IF NOT EXISTS habit_list (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                HabitName TEXT NOT NULL,
                                TableName TEXT NOT NULL,
                                CreatedDate TEXT NOT NULL
                            )";
                        command.ExecuteNonQuery();
                    }

                    // Save habit to master list
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"INSERT INTO habit_list (HabitName, TableName, CreatedDate) VALUES (@name, @table, @date)";

                        command.Parameters.AddWithValue("@name", habitName);
                        command.Parameters.AddWithValue("@table", SanitizeTableName(habitName));
                        command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to the habit list: {ex.Message}");
            }
        }

        public void CreateNewHabit()
        {
            while (true)
            {
                Console.WriteLine("\nEnter the name of the habit you want to track (letters only) OR enter 0 to cancel.");
                string? habitName = Console.ReadLine();

                if (habitName == "0")
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(habitName))
                {
                    Console.WriteLine("Habit name cannot be empty. PLease try again.");
                    continue;
                }

                if (habitName.Length > 30)
                {
                    Console.WriteLine("Habit name too long. Max is 30 characters.");
                    continue;
                }

                if (habitName.Any(char.IsDigit))
                {
                    Console.WriteLine("habit name cannot contain numbers");
                    continue;
                }

                if (CreateHabitTable(habitName))
                {
                    SaveHabitToMasterList(habitName);
                    break;
                }
            }
        }

        public bool CreateHabitTable(string habitName)
        {
            try
            {
                string tableName = SanitizeTableName(habitName);

                if (string.IsNullOrEmpty(tableName))
                {
                    Console.WriteLine("Invalid habit name. Only use letters.");
                    return false;
                }

                // Checking if table exists
                using (var connection = new SqliteConnection("Data Source=habit-tracker.db"))
                {
                    connection.Open();

                    using (var checkCommand = connection.CreateCommand())
                    {
                        checkCommand.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
                        var result = checkCommand.ExecuteScalar();

                        if (result != null)
                        {
                            Console.WriteLine($"A habit tracker for '{tableName}' already exists!'");
                            return false;
                        }
                    }

                    // Create Table
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            $@"CREATE TABLE IF NOT EXISTS {tableName} (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT NOT NULL,
                                Quantity INTEGER NOT NULL
                            )";

                        command.ExecuteNonQuery();
                        Console.WriteLine($"Created new habit tracker for '{habitName}'!");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating new habit: {ex.Message}");
                return false;
            }
        }

        private string SanitizeTableName(string habitName)
        {
            string safeName = new string(habitName.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());

            safeName = safeName.ToLower().Trim();

            safeName = safeName + "_tracker";

            return safeName;
        }

        //public void ConnectToDatabase(string tableName)
        //{
        //    var connection = new SqliteConnection("Data Source=habit-tracker.db");
        //    connection.Open();

        //    var command = connection.CreateCommand();
        //    command.CommandText =
        //        @$"CREATE TABLE IF NOT EXISTS {tableName} (
        //            Id INTEGER PRIMARY KEY AUTOINCREMENT,
        //            Date TEXT,
        //            Quantity INTEGER
        //        )";
        //    command.ExecuteNonQuery();

        //    connection.Close();
        //}

        public void MainMenu()
        {
            Console.WriteLine("MAIN MENU\n\n" +
                "What would you like to do?\n\n" +
                "Type 0 to Close Application\n" +
                "Type 1 to Select Habit Database\n" +
                "Type 2 to Add New Habit to Track\n" +
                "----------------------------------------");
        }
        public string MainMenuInput()
        {
            while (true)
            {
                Console.Write("Enter your input: ");
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Input can't be empty");
                    continue;
                }

                switch (input)
                {
                    case "0":
                        return input;
                    case "1":
                        return input;
                    case "2":
                        return input;
                    default:
                        Console.WriteLine("Please enter 0, 1, or 2.");
                        break;
                }
            }
        }


        public void DataMenu()
        {
            Console.WriteLine("DATABASE MENU\n\n" +
                "What would you like to do?\n\n" +
                "Type 0 to Close Application\n" +
                "Type 1 to View All Records\n" +
                "Type 2 to Insert Record\n" +
                "Type 3 to Delete Record\n" +
                "Type 4 to Update Record\n" +
                "-----------------------------------------"
            );
        }

        public bool UserInput()
        {
            while (true)
            {
                string? input;

                do
                {
                    Console.Write("Enter your input: ");
                    input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Input cannot be empty. Try again.\n");
                    }

                } while (string.IsNullOrEmpty(input));

                switch (input)
                {
                    case "0":
                        return true;
                    case "1":
                        Get();
                        return false;
                    case "2":
                        Insert();
                        return false;
                    case "3":
                        Delete();
                        return false;
                    case "4":
                        Update();
                        return false;
                    default:
                        Console.WriteLine("Please enter a number from 0 to 4.");
                        break;
                }
            }
        }

        public void Delete()
        {
            Get();
            Console.Write("\nEnter the ID number of the row you want to delete: ");
            var id = Console.ReadLine();

            var connection = new SqliteConnection("Data Source=habit-tracker.db");
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "SELECT COUNT(*) FROM daily_water WHERE id = @Id";
            command.Parameters.AddWithValue("@Id", id);

            int recordExists = Convert.ToInt32(command.ExecuteScalar());
            if (recordExists == 0)
            {
                Console.WriteLine($"\nNo record with the ID: {id} exists");
                return;
            }

            command.CommandText = "DELETE FROM daily_water WHERE id = @Id";
                
            var rowsDeleted = command.ExecuteNonQuery();

            connection.Close();

            Console.WriteLine("Row successfully deleted!");
        }

        public void Update()
        {
            Get();

            Console.Write("\nEnter the ID number of the row you want to update: ");
            var id = Console.ReadLine();
            var date = ValidDate();
            var quantity = ValidQuantity();

            var connection = new SqliteConnection("Data Source=habit-tracker.db");
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "SELECT COUNT(*) FROM daily_water WHERE id = @Id";
            command.Parameters.AddWithValue("@Id", id);

            int recordExists = Convert.ToInt32(command.ExecuteScalar());
            if (recordExists == 0)
            {
                Console.WriteLine($"\nNo record with the ID: {id} exists");
                return;
            }

            command.CommandText =
                "UPDATE daily_water " +
                "Set date = @Date, quantity = @Quantity " +
                "WHERE id = @Id";

            
            command.Parameters.AddWithValue("@Date", date);
            command.Parameters.AddWithValue("@Quantity", quantity);

            var rowInserted = command.ExecuteNonQuery();

            

            connection.Close();
        }

        public void Get()
        {
            var connection = new SqliteConnection("Data Source=habit-tracker.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM daily_water";
                
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                Console.WriteLine("\nID\tDate\t\t# of glasses");
                Console.WriteLine("---------------------------------");
                while(reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var date = reader.GetString(1);
                    var quantity = reader.GetString(2);
                    Console.WriteLine($"{id}\t{date}\t{quantity}");
                }
            }
            else
            {
                Console.WriteLine("No data to retrieve.");
            }

            connection.Close();
        }

        public void Insert()
        {
            var date = ValidDate();
            
            var quantity = ValidQuantity();

            var connection = new SqliteConnection("Data Source=habit-tracker.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "INSERT INTO daily_water (date, quantity) " +
                "VALUES (@Date, @Quantity)";

            command.Parameters.AddWithValue("@Date", date);
            command.Parameters.AddWithValue("@Quantity", quantity);

            command.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine($"\nSuccessfully added the date of {date} and the amount of glasses: {quantity}");
        }

        public string ValidDate()
        {
            while (true)
            {
                Console.Write("\n\nEnter the date (MM-dd-yy) OR Type 1 to enter today's date: ");
                string? date = Console.ReadLine();

                if (string.IsNullOrEmpty(date))
                {
                    Console.WriteLine("Date can't be empty. Try again.");
                    continue;
                }

                if (date == "1")
                {
                     return date = DateTime.Now.ToString("MM-dd-yy");
                }

                if (!DateTime.TryParseExact(date, "MM-dd-yy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dateInput))
                {
                    Console.WriteLine("Invalid date entered. Use this format: MM-dd-yy");
                    continue;
                }

                if (dateInput.Date > DateTime.Now.Date)
                {
                    Console.WriteLine("Can't enter future dates. Please try again.");
                    continue;
                }
                return date;
            }
        }

        public string ValidQuantity()
        {
            while (true)
            {
                Console.Write("\n\nEnter the number of glasses drank: ");
                string? quantity = Console.ReadLine();

                if (string.IsNullOrEmpty(quantity))
                {
                    Console.WriteLine("Quantity can't be empty");
                    continue;
                }

                if (!int.TryParse(quantity, out int quantityInput) || quantityInput < 0)
                {
                    Console.WriteLine("Invalid number. Try again. Needs to be a positive number.");
                    continue;
                }
                return quantity;
            }
        }
    }
}