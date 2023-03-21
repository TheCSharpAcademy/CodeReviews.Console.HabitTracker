using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
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
                Console.WriteLine("Database file created.");
            }

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            { 
                myConnection.Open();

                string createTableQuery = 
                    @"CREATE TABLE IF NOT EXISTS reading_habit (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        Date TEXT,
                        Quantity INTEGER
                        )";

                SQLiteCommand command = new SQLiteCommand(createTableQuery, myConnection);
                command.ExecuteNonQuery();

                myConnection.Close();
            }

                Console.WriteLine("\nWelcome to Reading Habit Tracker");
                GetUserInput();
        }

        static void PrintMenu()
        {
            Console.WriteLine("\n---------------Menu---------------");
            Console.WriteLine(@"Type 'i' to Insert Record
Type 'u' to Update Record
Type 'v' to View All Records
Type 'd' to Delete Record
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
                    case "0":
                        close = true;
                        Console.WriteLine("See you!");
                        break;
                    default:
                        break;
                }
            }
        }

        static string DateInput()
        {
            Console.Write("\n\nEnter the date (format: dd/mm/yy) or type 0 to return to main menu: ");
            string input = Console.ReadLine();

            if (input == "0")
            {
                Console.Clear();
                GetUserInput();
            }

            return input;
        }

        static int NumberInput()
        {
            Console.Write("Enter the number of pages you read or type 0 to return to main menu: ");
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
                cmd.CommandText = $"SELECT * FROM reading_habit WHERE Id={id}";
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
                string query = "INSERT INTO reading_habit ('date', 'quantity') VALUES (@date, @quantity)";
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
            Console.Write("Please enter the ID of the record you want to update or type 0 to return to main menu: ");
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

                string query = $"UPDATE reading_habit SET date = '{new_date}', quantity = {new_quantity} WHERE Id = {id_input}";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                command.ExecuteNonQuery();

                myConnection.Close();
            }

            Console.Clear();
            Console.WriteLine("Record updated successfully!");
        }
        
        static void Delete()
        {
            Console.Write("Please enter the ID of the record you want to delete or type 0 to return to main menu: ");
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

                string query = $"DELETE FROM reading_habit WHERE Id = {id_input}";
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

                string query = "SELECT * FROM reading_habit";
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
    }
}