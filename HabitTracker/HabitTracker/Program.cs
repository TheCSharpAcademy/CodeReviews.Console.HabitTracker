using HabitTracker;
using System.Globalization;


namespace habitTracker
{
    class Program
    {
        static string connectionString = @"Data Source=habitTracker.db";
        static HabitDatabase habits = new HabitDatabase();
        static void Main(string[] args)
        {
            habits.CreateTable();
            habits.SeedData();
            Console.Clear();
            GetUserInput();
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.Clear();
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Record.");
                Console.WriteLine("Type 3 to Delete Record.");
                Console.WriteLine("Type 4 to Update Record.");
                Console.WriteLine("Type 5 to Create New Habit.");
                Console.WriteLine("------------------------------------------\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
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
                    case "5":
                        CreateHabit();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
                        break;
                }
            }

        }


        private static void GetAllRecords()
        {
            Console.Clear();
            Console.WriteLine("\t*** RECORDS ***");
            List<Habit> habitRecords = habits.AllRecordsDb();

            DisplayHabitRecords(habitRecords);
        }

        private static void DisplayHabitRecords(List<Habit> habitRecords)
        {
            if (habitRecords == null || habitRecords.Count == 0)
            {
                Console.WriteLine("No records found.");
            }
            else
            {
                Console.WriteLine("------------------------------------------\n");
                foreach (var record in habitRecords)
                {
                    Console.WriteLine($"#{record.Id} Date: {record.Date:dd-MM-yy}  || {record.Quantity} {record.Unit} {record.HabitType}");
                }
                Console.WriteLine("------------------------------------------\n");
            }

            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }


        private static void Insert()
        {
            Console.Clear();
            Console.WriteLine("Please input the habit you would like to track: Type 0 to go back to the menu..");
            string habitName = Console.ReadLine();
            if (CheckReturnToMenu(habitName)) return;

            var habit = habits.FindHabitByName(habitName);
            if (habit == null)
            {
                Console.WriteLine($"Habit '{habitName}' not found. Would you like to create it? (y/n)");
                string response = Console.ReadLine()?.ToLower();

                if (response == "y")
                {
                    CreateHabit();
                    habit = habits.FindHabitByName(habitName);
                }
                else
                {
                    Console.WriteLine("Habit not created. Returning to menu.");
                    GetUserInput();
                    return;
                }
            }
            Console.WriteLine("You have selected the habit: " + habit.HabitType);
            Console.WriteLine($"Unit of measurement for this habit is '{habit.Unit}'.");

            string date = GetDateInput();
            int quantity = GetNumberInput($"Insert quantity in {habit.Unit}:");
            Console.Clear();
            habits.InsertHabitRecord(habit.Id, date, quantity);
            Console.WriteLine($"Record for '{habit.HabitType}' on {date} with quantity {quantity} {habit.Unit} has been added successfully.");
            Console.ReadLine();
        }

        private static void Delete()
        {
            while (true)
            {
                Console.Clear();
                GetAllRecords();

                var recordId = GetNumberInput("Please type the Id of the record you want to delete or type 0 to go back to Main Menu");
                if (CheckReturnToMenu(recordId.ToString())) return;

                bool deleted = habits.DeleteHabitRecordById(recordId);

                if (deleted)
                {
                    Console.WriteLine($"\nRecord with Id {recordId} was deleted.");
                    Console.WriteLine("Press any key to return to the main menu.");
                    Console.ReadKey();
                    GetUserInput();
                    return;
                }
                else
                {
                    Console.WriteLine($"\nRecord with Id {recordId} doesn't exist. Try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        internal static void Update()
        {
            GetAllRecords();

            int recordId;

            while (true)
            {
                recordId = GetNumberInput("Please type the Id of the record you would like to update. Type 0 to return to main menu.");
                if (CheckReturnToMenu(recordId.ToString())) return;

                if (habits.RecordExists(recordId))
                {
                    break;
                }

                Console.WriteLine($"\nRecord with Id {recordId} doesn't exist. Please try again.");
            }

            string date = GetDateInput();
            int quantity = GetNumberInput("Please insert the new quantity (no decimals allowed):");

            bool updated = habits.UpdateHabitRecord(recordId, date, quantity);

            if (!updated)
            {
                Console.WriteLine($"\nRecord with Id {recordId} couldn't be updated.");
            }
            else
            {
                Console.WriteLine($"\nRecord with Id {recordId} was updated.");
            }

            Console.WriteLine("\nPress any key to return to the main menu.");
            Console.ReadKey();
            GetUserInput();
        }

        internal static void CreateHabit()
        {
            Console.Clear();
            Console.WriteLine("Please input the habit name you want to create: Type 0 to return to the menu");
            string habitName = Console.ReadLine();
            if (CheckReturnToMenu(habitName)) return;

            if (string.IsNullOrWhiteSpace(habitName))
            {
                Console.WriteLine("Invalid habit name.");
                return;
            }

            Console.WriteLine("Please insert the unit of the new habit (e.g. glasses, minutes, etc.). Type 0 to return to the menu:");
            string unit = Console.ReadLine();
            if (CheckReturnToMenu(unit)) return;

            while (string.IsNullOrWhiteSpace(unit))
            {
                Console.WriteLine("Invalid unit. Please insert the unit of the new habit:");
                unit = Console.ReadLine();
            }

            habits.InsertRecord(habitName, unit);

            Console.WriteLine($"\nHabit '{habitName}' with unit '{unit}' has been created successfully.");
            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
            GetUserInput();
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static bool CheckReturnToMenu(string input)
        {
            if (input == "0")
            {
                GetUserInput();
                return true;
            }
            return false;
        }


        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }

    public class Habit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string HabitType { get; set; }
        public string Unit { get; set; }
    }
}


