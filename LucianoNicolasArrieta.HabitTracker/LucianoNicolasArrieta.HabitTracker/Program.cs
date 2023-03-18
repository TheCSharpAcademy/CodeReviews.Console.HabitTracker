using LucianoNicolasArrieta.HabitTracker;
using System.Data.SQLite;

namespace habit_tracker
{
    class Program
    {
        static Database database = new Database();
        static void Main(string[] args)
        {
            database.OpenConnection();

            string createTableQuery = 
                @"CREATE TABLE IF NOT EXISTS reading_habit (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Date TEXT,
                    Quantity INTEGER
                    )";

            SQLiteCommand command = new SQLiteCommand(createTableQuery, database.myConnection);
            command.ExecuteNonQuery();

            database.CloseConnection();

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

            string query = "INSERT INTO reading_habit ('date', 'quantity') VALUES (@date, @quantity)";
            SQLiteCommand command = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.ExecuteNonQuery();
            database.CloseConnection();

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
    }
}