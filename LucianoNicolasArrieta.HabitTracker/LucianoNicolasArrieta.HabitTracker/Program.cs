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
                        break;
                    case "d":
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

            Console.WriteLine("Record added to database successfully!");
        }

        static string DateInput()
        {
            Console.Write("\n\nEnter the date (format: dd/mm/yy) or type 0 to return to main menu: ");
            string input = Console.ReadLine();

            if (input == "0") GetUserInput();

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
            if (num_input == 0) GetUserInput();

            return num_input;
        }
        
        static void Update()
        {
            Console.Write("Please enter the ID of the record you want to update or type 0 to return to main menu: ");
            string input = Console.ReadLine();
            int id_input;
            while (!int.TryParse(input, out id_input))
            {
                Console.Write("Type a integer please: ");
                input = Console.ReadLine();
            }
            
            if (!checkIdExists(id_input))
            {
                Update();
            }

            string new_date = DateInput();
            int new_quantity = NumberInput();

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string query = $"UPDATE reading_habit SET date = {new_date}, quantity = {new_quantity} WHERE Id={id_input}";

                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                command.ExecuteNonQuery();

                myConnection.Close();
            }       
        }
        
        static bool checkIdExists(int id)
        {
            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                var cmd = myConnection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM reading_habit WHERE Id={id}";
                SQLiteDataReader reader = cmd.ExecuteReader();
                
                if (!reader.HasRows)
                {
                    Console.WriteLine($"Record with Id = {id} doesn't exist. Try again.");
                    return false;
                }

                myConnection.Close();
            }

            return true;
        }
    }
}