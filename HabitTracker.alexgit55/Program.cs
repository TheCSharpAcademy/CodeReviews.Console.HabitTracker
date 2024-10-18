using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker.alexgit55
{
    public class Habit
    //Class to represent a habit record in the database
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }

    internal class Program
    {
        static string connectionString = @"Data Source=water_tracker.db";
        static string databaseName = "water_tracker";

        static void Main(string[] args)
        {

            int success = RunNonQueryCommandOnDatabase($"CREATE TABLE IF NOT EXISTS {databaseName} (id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Quantity INTEGER)");

            if (success != 0)
            {
                Console.WriteLine($"Unable to create table. Press any key to exit.\n\n");
                Console.ReadKey();
                return;
            }

            GetUserInput();
        }

        static void GetUserInput()
        // Display the main menu and get user input
        {
            Console.Clear();

            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\n0. Close Application");
                Console.WriteLine("1. View All Records");
                Console.WriteLine("2. Add New Record");
                Console.WriteLine("3. Delete A Record");
                Console.WriteLine("4. Update a Record");
                Console.WriteLine("------------------------------\n");

                var userInput = Console.ReadKey();
                Console.WriteLine("\n");

                switch (userInput.KeyChar)
                {
                    case '0':
                        Console.WriteLine("Closing Application. Goodbye!");
                        closeApp = true;
                        break;
                    case '1':
                        ViewAllRecords();
                        break;
                    case '2':
                        InsertRecord();
                        break;
                    case '3':
                        DeleteRecord();
                        break;
                    case '4':
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Please try again.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                Console.Clear();

            }
        }

        private static int CheckDatabaseForRecord(int id)
        /* Check if a record with the given ID exists in the database
         * Return 1 if the record exists, 0 if it does not */
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT EXISTS(SELECT 1 FROM {databaseName} WHERE Id = {id})";
            int query = Convert.ToInt32(command.ExecuteScalar());

            connection.Close();

            return query;
        }


        public static int RunNonQueryCommandOnDatabase(string commandText)
        // Create a connection to the database and execute a command
        {

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = commandText;
            int result = command.ExecuteNonQuery();

            connection.Close();

            return result;
        }

        private static void ViewAllRecords()
        /* Display all records in the database
         * Query the database and store the records in a list of Habit objects
         * If no records are found, display a message */
        {
            Console.Clear();
            Console.WriteLine("Viewing all records\n");
            string commandText = $"SELECT * FROM {databaseName}";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = commandText;

            List<Habit> habits = new();

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", new CultureInfo("en-us")),
                        Quantity = reader.GetInt32(2)
                    });
                }

            }
            else
                Console.WriteLine("No records found");

            connection.Close();
            Console.WriteLine("--------------------------------");
            Console.WriteLine("ID\tDate\t\tQuantity");
            Console.WriteLine("--------------------------------");
            foreach (var habit in habits)
            {
                Console.WriteLine($"{habit.Id}\t{habit.Date:yyyy-MM-dd}\t{habit.Quantity}");
            }
        }

        private static string GetDateInput()
        /* Get a date input from the user. Repeat until a valid date is entered.
         * Special options: 0 to return to main menu, 1 for today's date */

        {
            bool validDate = false;
            DateTime date = new();
            while (validDate == false)
            {
                Console.WriteLine("Please enter the date: (Format: yyyy-MM-dd). Type 0 to return to main menu, Type 1 for today's date.");
                var userInput = Console.ReadLine();
                if (userInput == "0")
                    return "0";
                if (userInput == "1")
                    return DateTime.Now.ToString("yyyy-MM-dd");
                validDate = DateTime.TryParseExact(userInput, "yyyy-MM-dd", new CultureInfo("en-us"), DateTimeStyles.None, out date);

                if (validDate == true)
                    break;

                Console.WriteLine("Invalid Date format, please try again.");
            }


            return date.ToString("yyyy-MM-dd");

        }

        private static int GetNumberInput(string message)
        /* Get a number input from the user, keep asking until a valid number is entered.
         * A valid number is a non-negative integer. */

        {
            Console.WriteLine(message);
            int quantity = -1;
            bool validInput = false;

            while (validInput == false)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out quantity) && quantity >= 0)
                    validInput = true;
                else
                    Console.WriteLine("Invalid input. Please try again.");
            }

            return quantity;
        }

        private static void InsertRecord()
        /* Insert a new record into the database
         * Prompt the user for a date and quantity of water drank, then insert the record into the database
         * If the user enters 0 for the date or quantity, return to the main menu
         */
        {
            Console.Clear();
            Console.WriteLine("\nAdd New Record to Database\n");
            Console.WriteLine("----------------------------\n\n");
            string date = GetDateInput();
            if (date == "0")
            {
                Console.WriteLine("No records will be added.");
                return;
            }

            int quantity = GetNumberInput("Please insert number of glasses of water drank (no decimals allowed, type 0 to return to main menu): ");

            if (quantity == 0)
            {
                Console.WriteLine("No records will be added.");
                return;
            }

            string commandText = $"INSERT INTO {databaseName} (Date, Quantity) VALUES ('{date}', {quantity})";
            int success = RunNonQueryCommandOnDatabase(commandText);

            if (success == 0)
                Console.WriteLine($"Unable add record.\n\n");
            else
                Console.WriteLine($"Record has been added successfully.\n\n");
        }

        private static void DeleteRecord()
        /* Delete a record from the database
         * Prompt the user for the ID of the record to delete, then delete the record from the database
         * If no record with the given ID is found, display a message and return to the main menu
         */
        {
            Console.Clear();
            ViewAllRecords();

            int id = GetNumberInput("\nPlease enter the ID of the record you want to delete (type 0 to return to main menu): ");

            if (id == 0)
            {
                Console.WriteLine("No records will be deleted.\n\n");
                return;
            }

            int recordExists = CheckDatabaseForRecord(id);

            if (recordExists == 0)
            {
                Console.WriteLine($"No record with ID {id} found. No records were deleted.\n\n");
                return;
            }

            string commandText = $"DELETE FROM {databaseName} WHERE id = {id}";

            int success = RunNonQueryCommandOnDatabase(commandText);

            if (success == 0)
                Console.WriteLine($"Unable to locate record with ID {id}.\n\n");
            else
                Console.WriteLine($"Record with ID {id} was deleted.\n\n");

        }

        private static void UpdateRecord()
        /* Update a record in the database
         * Prompt the user for the ID of the record to update, then prompt for a new date and quantity
         * If no record with the given ID is found, display a message and return to the main menu
         */
        {
            Console.Clear();
            ViewAllRecords();

            int id = GetNumberInput("\nPlease enter the ID of the record you want to delete (type 0 to return to main menu): ");

            if (id == 0)
            {
                Console.WriteLine("No records will be updated.\n\n");
                return;
            }

            int recordExists = CheckDatabaseForRecord(id);

            if (recordExists == 0)
            {
                Console.WriteLine($"No record with ID {id} found. No records will be updated.\n\n");
                return;
            }

            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert number of glasses of water drank (no decimals allowed, type 0 to return to main menu): ");

            if (quantity == 0)
            {
                Console.WriteLine("No records will be updated.");
                return;
            }

            string commandText = $"UPDATE {databaseName} SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";

            int success = RunNonQueryCommandOnDatabase(commandText);

            if (success == 0)
                Console.WriteLine($"Unable to update record {id}.\n\n");
            else
                Console.WriteLine($"Record with ID {id} was updated.\n\n");

        }
    }
}
