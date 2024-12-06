using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Security.AccessControl;
using System.Reflection.PortableExecutable;

namespace Program
{
    class Program
    {
        static string connectionString = @"Data Source=habitTracker.db";
        static void Main(string[] args)
        {
            Console.WriteLine("Habit tracker in C#\r");
            Console.WriteLine("------------------------\n");

            using (SqliteConnection db = new SqliteConnection(connectionString))
            {
                db.Open();

                SqliteCommand tableCommand = db.CreateCommand();

                tableCommand.CommandText = 
                    @"CREATE TABLE IF NOT EXISTS habit (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        Date TEXT,
                        Quantity REAL,
                        MeasurementUnit TEXT
                        )";
                tableCommand.ExecuteNonQuery();

                db.Close();
            }

            FillDB();

            OpenMainMenu();
        }

        static public void FillDB()
        {
            bool hasRows;

            using (var connection = new SqliteConnection(connectionString))
            {
                SqliteDataReader reader = SelectHabits(connection);

                hasRows = reader.HasRows;

                connection.Close();
            }

            if (!hasRows)
            {
                string[] habits = { "Running", "Drink water", "Meditate", "Eat healthy" };
                string[] measurements = { "KMs", "Glasses", "Minutes", "Meals" };

                DateTime maxDate = DateTime.Today;
                DateTime minDate = DateTime.Parse("01-01-14");

                Random random = new Random();

                for (int i = 0; i < 100; i++)
                {
                    int randomHabit = random.Next(0, habits.Length);

                    string habitName = habits[randomHabit];
                    string habitMeasure = measurements[randomHabit];


                    float habitQuantity = random.Next(0, 20);

                    TimeSpan timeSpan = maxDate - minDate;
                    TimeSpan newSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);
                    DateTime randomDate = minDate + newSpan;

                    InsertHabit(habitName, randomDate.ToString("dd-MM-yy"), habitQuantity, habitMeasure);
                }
            }
        }

        static public void OpenMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n\nWelcome Habit tracker\nMenu:");
                Console.WriteLine("1 - View your tracked habits");
                Console.WriteLine("2 - Insert habit");
                Console.WriteLine("3 - Delete habit");
                Console.WriteLine("4 - Update habit");
                Console.WriteLine("5 - Exit");

                char input = Console.ReadKey().KeyChar;
                Console.Clear();

                switch (input)
                {
                    case '1':
                        ViewHabits();
                        break;
                    case '2':
                        GetHabitValues();
                        break;
                    case '3':
                        DeleteHabits();
                        break;
                    case '4':
                        UpdateHabits();
                        break;
                    case '5':
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }

            }
        }

        static public void ViewHabits()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                List<Habit> tableData = new List<Habit>();

                SqliteDataReader reader = SelectHabits(connection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new Habit
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetFloat(3),
                                Measure = reader.GetString(4)
                            }    
                        );
                    }
                }
                else
                {
                    Console.WriteLine("No habits recorded yet.");
                }

                connection.Close();

                foreach (var habit in tableData)
                {
                    Console.WriteLine($"Id: {habit.Id} - Habit: {habit.Name} - Quantity: {habit.Quantity} {habit.Measure} - Date: {habit.Date.ToString("dd-MMM-yyyy")}");
                    Console.WriteLine($"-----------------------------------------------------------------------------------------------------------");
                }
            }
        }

        static public SqliteDataReader SelectHabits(SqliteConnection connection)
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"Select * from habit";

            return tableCmd.ExecuteReader();
        }

        static public string GetDateInput()
        {
            bool valid = false;
            string date = "";

            do
            {
                Console.WriteLine("Insert date (Format: dd-mm-yy) :");
                Console.WriteLine("(Leave empty and press ENTER to select current date)");
                date = Console.ReadLine();

                if (date == "")
                {
                    date = DateTime.Today.ToString("dd-MM-yy");
                    valid = true;
                }
                else if (DateTime.TryParseExact(date, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid date...");
                }
            }
            while (!valid);

            return date;
        }

        internal static float GetFloatInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            while (!float.TryParse(numberInput, out _) || Convert.ToSingle(numberInput) < 0)
            {
                Console.WriteLine("\nInvalid number. Try again.\n");
                numberInput = Console.ReadLine();
            }

            float finalInput = Convert.ToSingle(numberInput);

            return finalInput;
        }

        static public void GetHabitValues()
        {
            Console.WriteLine("Insert habit (Ex: Drink water, Running, Meditate) :");
            string habitName = Console.ReadLine();

            string habitDate = GetDateInput();

            Console.WriteLine("Insert measurement unit (Ex:Glasses of water, Km, Minutes...) :");
            string habitMeasure = Console.ReadLine();

            float habitQuantity = GetFloatInput("Insert quantity:");

            InsertHabit(habitName, habitDate, habitQuantity, habitMeasure);
        }

        static public void InsertHabit(string habitName, string habitDate, float habitQuantity, string habitMeasure)
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO habit(Name, Date, Quantity, MeasurementUnit) VALUES('{habitName}', '{habitDate}', {habitQuantity}, '{habitMeasure}')";
                
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

        }

        static public void DeleteHabits()
        {
            Console.Clear();
            ViewHabits();

            string recordId;

            using (var connection = new SqliteConnection(connectionString))
            {
                bool validInput;
                connection.Open();

                var tableCmd = connection.CreateCommand();

                do
                {
                    validInput = true;
                    Console.WriteLine("\nWrite the Id of the habit you want to Delete and press ENTER\n");
                    recordId = Console.ReadLine();

                    tableCmd.CommandText = $"DELETE from habit WHERE Id = '{recordId}'";

                    int rowCount = tableCmd.ExecuteNonQuery();

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"\nRecord with Id {recordId} doesn't exist. \n");
                        validInput = false;
                    }
                }
                while (!validInput);

            }

            Console.WriteLine($"\nRecord with Id {recordId} was deleted. \n");
        }

        static public void UpdateHabits()
        {
            ViewHabits();

            string recordId;

            using (var connection = new SqliteConnection(connectionString))
            {
                bool validInput = true;
                connection.Open();

                do
                {
                    Console.WriteLine("\nWrite the Id of the record you would like to update.\n");
                    recordId = Console.ReadLine();

                    var checkCmd = connection.CreateCommand();
                    checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit WHERE Id = {recordId})";
                    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (checkQuery == 0)
                    {
                        Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.\n");
                        connection.Close();
                        validInput = false;
                    }
                }
                while (!validInput);

                Console.WriteLine("Insert habit (Ex: Drink water, Running, Meditate) :");
                string habitName = Console.ReadLine();

                string habitDate = GetDateInput();

                Console.WriteLine("Insert measurement unit (Ex:Glasses of water, Km, Minutes...) :");
                string habitMeasure = Console.ReadLine();

                float habitQuantity = GetFloatInput("Insert quantity:");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE habit SET Name = '{habitName}', Date = '{habitDate}', Quantity = {habitQuantity}, MeasurementUnit = '{habitMeasure}' WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }

    public class Habit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Measure { get; set; }
        public float Quantity { get; set; }
    }

}