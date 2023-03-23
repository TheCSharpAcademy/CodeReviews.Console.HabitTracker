using System.CodeDom;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace habit_tracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-tracker.db";

        static void Main(string[] args)
        {
            if (!File.Exists("./habit-tracker.db"))
            {
                SQLiteConnection.CreateFile("habit-tracker.db");
                Console.WriteLine("Hi! This is the first time you open the app");
                Console.Write("Please, enter the habit you want to track: ");
                LucianoNicolasArrieta.HabitTracker.Settings1.Default.HabitName = Console.ReadLine();
                Console.Write("Now enter the unit of measurement of that habit (ex. kms, pages, calories, cups, etc.): ");
                LucianoNicolasArrieta.HabitTracker.Settings1.Default.Unit = Console.ReadLine();
                LucianoNicolasArrieta.HabitTracker.Settings1.Default.Save();
            }

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            { 
                myConnection.Open();

                string createTableQuery = 
                    @"CREATE TABLE IF NOT EXISTS habit_tracker (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        Date TEXT,
                        Quantity INTEGER
                        )";

                SQLiteCommand command = new SQLiteCommand(createTableQuery, myConnection);
                command.ExecuteNonQuery();

                myConnection.Close();
            }

            string habit_name = LucianoNicolasArrieta.HabitTracker.Settings1.Default.HabitName;
            Console.WriteLine($"\nWelcome to {habit_name} Habit Tracker");
            GetUserInput();
        }

        static void PrintMenu()
        {
            Console.WriteLine("\n---------------Menu---------------");
            Console.WriteLine(@"Type 'i' to Insert Record
Type 'u' to Update Record
Type 'v' to View All Records
Type 'd' to Delete Record
Type 'r' to View Reports
Type 0 to Close the App
----------------------------------");
        }

        static void GetUserInput()
        {
            bool close = false;
            string user_input;
            while (!close)
            {
                PrintMenu();
                user_input = Console.ReadLine();

                switch (user_input)
                {
                    case "i":
                        Insert();
                        break;
                    case "u":
                        Update();
                        break;
                    case "v":
                        ViewRecords();
                        break;
                    case "d":
                        Delete();
                        break;
                    case "r":
                        ViewReports();
                        break;
                    case "0":
                        close = true;
                        Console.WriteLine("See you!");
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        static string DateInput()
        {
            Console.Write("\n\nEnter the date (format: dd-mm-yy) or type 0 to return to main menu: ");
            string input = Console.ReadLine();

            if (input == "0")
            {
                Console.Clear();
                GetUserInput();
            }
            while (!DateTime.TryParseExact(input, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date. Try again (Remember, format: dd-mm-yy: ");
                input = Console.ReadLine();
            }

            return input;
        }

        static int NumberInput()
        {
            string unit = LucianoNicolasArrieta.HabitTracker.Settings1.Default.Unit;
            Console.Write($"Enter the number of {unit} or type 0 to return to main menu: ");
            string input = Console.ReadLine();
            int num_input;
            while(!int.TryParse(input, out num_input))
            {
                Console.Write("Type a integer please: ");
                input = Console.ReadLine();
            }
            if (num_input == 0)
            {
                Console.Clear();
                GetUserInput();
            }

            return num_input;
        }

        static bool checkIdExists(int id)
        {
            bool exists;
            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                var cmd = myConnection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM habit_tracker WHERE Id={id}";
                SQLiteDataReader reader = cmd.ExecuteReader();

                exists = reader.HasRows;

                reader.Close();
                myConnection.Close();

            }
            return exists;
        }

        static void Insert()
        {
            string date = DateInput();
            int quantity = NumberInput();

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                string query = "INSERT INTO habit_tracker ('date', 'quantity') VALUES (@date, @quantity)";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);

                myConnection.Open();

                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.ExecuteNonQuery();

                myConnection.Close();
            }

            Console.Clear();
            Console.WriteLine("Record added to database successfully!");
        }

        static void Update()
        {
            Console.Write("\n\nPlease enter the ID of the record you want to update or type 0 to return to main menu: ");
            string input = Console.ReadLine();
            int id_input;
            while (!int.TryParse(input, out id_input))
            {
                Console.WriteLine("Invalid input. Type a integer please.");
                input = Console.ReadLine();
            }
            if (id_input == 0)
            {
                Console.Clear();
                GetUserInput();
            }
            if (!checkIdExists(id_input))
            {
                Console.WriteLine($"Record with Id = {id_input} doesn't exist. Please try again.");
                Update();
            }

            string new_date = DateInput();
            int new_quantity = NumberInput();

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string query = $"UPDATE habit_tracker SET date = '{new_date}', quantity = {new_quantity} WHERE Id = {id_input}";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                command.ExecuteNonQuery();

                myConnection.Close();
            }

            Console.Clear();
            Console.WriteLine("Record updated successfully!");
        }
        
        static void Delete()
        {
            Console.Write("\n\nPlease enter the ID of the record you want to delete or type 0 to return to main menu: ");
            string input = Console.ReadLine();
            int id_input;
            while (!int.TryParse(input, out id_input))
            {
                Console.WriteLine("Invalid input. Type a integer please.");
                input = Console.ReadLine();
            }
            if (id_input == 0)
            {
                Console.Clear();
                GetUserInput();
            }
            if (!checkIdExists(id_input))
            {
                Console.WriteLine($"Record with Id = {id_input} doesn't exist. Please try again.");
                Delete();
            }

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string query = $"DELETE FROM habit_tracker WHERE Id = {id_input}";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                command.ExecuteNonQuery();

                myConnection.Close();
            }

            Console.Clear();
            Console.WriteLine("Record deleted successfully!");
        }

        static void ViewRecords()
        {
            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string query = "SELECT * FROM habit_tracker";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                SQLiteDataReader records = command.ExecuteReader();

                if(records != null)
                {
                    Console.WriteLine("\n----------------------");
                    Console.WriteLine(" Id - Date - Quantity");
                    Console.WriteLine("----------------------");

                    while (records.Read())
                    {
                        Console.WriteLine(" {0} - {1} - {2} ", records[0], records[1], records[2]);
                    }
                    Console.WriteLine("----------------------");
                }

                myConnection.Close();

                Console.WriteLine("\nPress any key to continue to main menu");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ViewReports()
        {
            Console.WriteLine("\n\nType the year you want to see reports about (YY) or 0 to go back to main menu: ");
            string year_input = Console.ReadLine();
            if (year_input == "0")
            {
                Console.Clear();
                GetUserInput();
            }
            while (!DateTime.TryParseExact(year_input, "yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.Write("Enter the year in the correct format (YY): ");
                year_input = Console.ReadLine();
            }
            GetReports(year_input);
        }

        static void GetReports(string year)
        {
            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string query = $"SELECT * FROM habit_tracker WHERE date LIKE '%{year}'";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                SQLiteDataReader records = command.ExecuteReader();

                
                if (records.HasRows) // ; records != null
                {
                    int reached = 0, quantity = 0, record = 0;
                    string recordDate = "";
                    while (records.Read())
                    {
                        quantity = Convert.ToInt32(records[2]);
                        reached += quantity;
                        if (quantity > record)
                        {
                            record = quantity;
                            recordDate = Convert.ToString(records[1]);
                        }
                    }
                    string unit = LucianoNicolasArrieta.HabitTracker.Settings1.Default.Unit;
                    Console.WriteLine($"\nIn 20{year}, you reached {reached}{unit}");
                    Console.WriteLine($"On {recordDate}, you reached {record}{unit} in a day!");
                } else
                {
                    Console.WriteLine("There is no record of that year.");
                }

                myConnection.Close();

                Console.WriteLine("\nPress any key to continue to main menu");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}