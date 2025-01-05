using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-tracker.db";

        static void Main(string[] args)
        {
            Console.Clear();

            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habits_table (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, 
                Date TEXT, Quantity INTEGER)";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            MainMenu();
        }

        static void MainMenu()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to close the application.");
                Console.WriteLine("Type 1 to view all habit records.");
                Console.WriteLine("Type 2 to insert habit record.");
                Console.WriteLine("Type 3 to delete habit record.");
                Console.WriteLine("Type 4 to update habit record.");
                Console.WriteLine("------------------------------------");

                string commandInput = Console.ReadLine();

                switch(commandInput)
                {
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Goodbye!");
                        System.Threading.Thread.Sleep(1500);
                        Console.Clear();
                        closeApp = true;
                        Environment.Exit(0);
                    break;
                    case "1":
                        GetAllRecords();
                    break;
                    case "2":
                        Insert();
                    break;
                    case "3":
                        Delete();
                    break;
                    case "4":
                        Update();
                    break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid command. Please type a number from 0 to 4.\nPress enter to continue...");
                        Console.ReadLine();
                        Console.Clear();
                    break;
                }
            }
        }

        private static void GetAllRecords()
        {
            Console.Clear();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand getRecordsCmd = connection.CreateCommand();
                getRecordsCmd.CommandText = $"SELECT * FROM habits_table ORDER BY Date DESC";
                SqliteDataReader reader = getRecordsCmd.ExecuteReader();

                List<Habit> tableData = new List<Habit>();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        tableData.Add(new Habit
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No entries found");
                }

                connection.Close();

                Console.WriteLine("--------------------------------------------");
                foreach (Habit dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Name} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("--------------------------------------------\n");

                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static void Insert()
        {
            Console.Clear();

            string name = GetNameInput("Please enter the name of the Habit to be logged. Type 0 to return to the main menu.");
            string date = GetDateInput("Please enter the date the habit was performed: (Format: dd-mm-yy). Type 'today' for today's date, or type 0 to return to main menu.");
            int quantity = GetNumberInput("Please enter the number of times the habit was performed. Type 0 to go back to the main menu.");

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO habits_table(name, date, quantity) VALUES('{name}', '{date}', {quantity})";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Console.Clear();
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            int recordId = GetNumberInput("Please type the Id of the record you want to delete or type 0 to go back to the main menu.");

            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand checkDeleteCmd = connection.CreateCommand();
                checkDeleteCmd.CommandText = $"Select COUNT(1) from habits_table Where Id = '{recordId}'";
                int checkDeleteCmdOutput = Convert.ToInt32(checkDeleteCmd.ExecuteScalar());

                if(checkDeleteCmdOutput == 1)
                {
                    checkDeleteCmd.CommandText = $"DELETE from habits_table WHERE Id = '{recordId}'";
                    checkDeleteCmd.ExecuteNonQuery();
                }
                else if(checkDeleteCmdOutput == 0)
                {
                    connection.Close();
                    Console.WriteLine($"Record with Id {recordId} doesn't exist.\nPress enter to continue...");
                    Console.ReadLine();
                    Delete();
                }

                connection.Close();
            }

            Console.WriteLine($"Record with Id {recordId} was deleted.\nPress enter to continue...");
            Console.ReadLine();
            Console.Clear();

            MainMenu();
        }

        private static void Update()
        {
            Console.Clear();
            GetAllRecords();

            int recordId = GetNumberInput("Please type the Id of the record you would like to update. Type 0 to return to the main menu.");

            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand checkUpdateCmd = connection.CreateCommand();
                checkUpdateCmd.CommandText = $"Select COUNT(1) from habits_table Where Id = '{recordId}'";
                int checkUpdateCmdOutput = Convert.ToInt32(checkUpdateCmd.ExecuteScalar());

                if(checkUpdateCmdOutput == 1)
                {
                    string name = GetNameInput("Please enter the name of the Habit to be logged. Type 0 to return to the main menu.");
                    string date = GetDateInput("Please enter the date the habit was performed: (Format: dd-mm-yy). Type 'today' for today's date, or type 0 to return to main menu.");
                    int quantity = GetNumberInput("Please enter the number of times the habit was performed.");

                    SqliteCommand updateCmd = connection.CreateCommand();
                    updateCmd.CommandText = $"UPDATE habits_table SET name = '{name}', date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
                    updateCmd.ExecuteNonQuery();

                    Console.WriteLine($"Record with Id {recordId} has been updated.\nPress enter to continue...");
                    Console.ReadLine();
                }
                else if(checkUpdateCmdOutput == 0)
                {
                    Console.WriteLine($"Record with Id {recordId} doesn't exist.\nPress enter to continue...");
                    Console.ReadLine();
                    Update();
                }

                connection.Close();
            }

            Console.Clear();
        }

        internal static string GetNameInput(string message)
        {
            Console.WriteLine(message);
            string nameInput = Console.ReadLine();

            if(nameInput == "0")
            {
                Console.Clear();
                MainMenu();
            }

            if(nameInput == "")
            {
                nameInput = "Habit";
            }

            Console.WriteLine();

            return nameInput;
        }

        internal static string GetDateInput(string message)
        {
            Console.WriteLine(message);
            string dateInput = Console.ReadLine();

            if(dateInput == "0")
            {
                Console.Clear();
                MainMenu();
            }

            if(dateInput == "today")
            {
                dateInput = DateTime.Now.ToString("dd-MM-yy");
            }

            while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nInvalid date format (Correct format -> dd-MM-yy). Please enter a date following the correct date format.");
                dateInput = Console.ReadLine();
            }

            Console.WriteLine();
            
            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            if(numberInput == "0")
            {
                Console.Clear();
                MainMenu();
            }

            while(!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\nInvalid number. Try again.");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            Console.WriteLine();

            return finalInput;
        }
    }

    public class Habit
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}