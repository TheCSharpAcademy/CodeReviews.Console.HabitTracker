using Microsoft.Data.Sqlite;

namespace HabitTracker.alvaromosconi
{
    internal class Program
    {
        private const string TABLE_NAME = "studying_cSharp";
        static void Main(string[] args)
        {
            InitializeApplication();
        }
        static void InitializeApplication()
        {
            CreateTable();
            string userInput = GetUserInput();
            ExecuteOption(userInput);
        }

        private static void ExecuteOption(string userInput)
        {
            switch(userInput)
            {
                case "1":
                    ShowAllRecords();
                    break;
                case "2":
                    InsertNewRecord();
                    break;
                case "3":
                    UpdateAnExistingRecord();
                    break;
                case "4":
                    DeleteAnExistingRecord();
                    break;
            }
        }

        private static void DeleteAnExistingRecord()
        {
            throw new NotImplementedException();
        }

        private static void UpdateAnExistingRecord()
        {
            throw new NotImplementedException();
        }

        private static void InsertNewRecord()
        {
            throw new NotImplementedException();
        }

        private static void ShowAllRecords()
        {
            throw new NotImplementedException();
        }

        static void CreateTable()
        {
            using (var connection = new SqliteConnection(@"Data Source=habit-tracker.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    $@"
                        CREATE table IF NOT EXISTS {TABLE_NAME} 
                            (id INTEGER PRIMARY KEY AUTOINCREMENT,
                             date TEXT,
                             quantity INTEGER
                            )
                    ";

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        static string GetUserInput()
        {
            Console.Clear();
            string userInput;
            bool isAValidInput = false;

            do
            {
                Console.WriteLine("\n==============Welcome to Habits Tracker!==============\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("1. View All Records.");
                Console.WriteLine("2. Insert A New Record.");
                Console.WriteLine("3. Update An Existing Record.");
                Console.WriteLine("4. Delete An Existing Record.");
                Console.WriteLine("5. Close App.\n");
                userInput = Console.ReadLine();
                isAValidInput = IsAValidInput(userInput);
                
                if (!isAValidInput)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input! Please enter a valid choice. Press any key to retry.");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (!isAValidInput);

            return userInput!;
        }

        private static bool IsAValidInput(string? choice)
        {
            if (string.IsNullOrWhiteSpace(choice))
                return false;

            if (!int.TryParse(choice, out int parsedChoice))
                return false;

            return IsChoiceInRange(parsedChoice);
        }

        private static bool IsChoiceInRange(int choice)
        {
            return choice >= 1 && choice <= 5;
        }

    }
}