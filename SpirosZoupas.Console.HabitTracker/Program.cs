using SpirosZoupas.Console.HabitTracker.Model;
using System.Globalization;

namespace habit_tracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static HabitTrackerRepository _habitTrackerRepository = new HabitTrackerRepository();

        static void Main(string[] args)
        {
            _habitTrackerRepository.CreateTables();
            GetUserInput();
        }

        private static void CreateHabit()
        {
            Console.WriteLine("\n\nPlease enter a name for the habit");
            string habit = Console.ReadLine();
            Console.WriteLine("Please enter unit of measurement");
            string uom = Console.ReadLine();

            bool success = _habitTrackerRepository.InsertHabit(habit, uom);

            if (success) Console.WriteLine("Habit Created Successful");
            else Console.WriteLine("Sorry, insert failed.");
        }

        private static void CreateRecord()
        {
            Console.WriteLine("\n\nPlease enter habit name");
            string habit = Console.ReadLine();

            int habitId = _habitTrackerRepository.GetHabitIdByName(habit);
            bool habitFound = habitId != -1;

            if (!habitFound)
            {
                Console.WriteLine("Habit doesn't exist!");
                Thread.Sleep(1000);
                CreateRecord();
                return;
            }

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease enter quantity");

            bool success = _habitTrackerRepository.InsertRecord(habitId, date, quantity);

            if (success) Console.WriteLine("Insert Successful");
            else Console.WriteLine("Sorry, insert failed.");
        }

        private static void UpdateHabit()
        {
            Console.Clear();
            // GetAllHabits();

            Console.WriteLine("\n\nPlease enter the name of the habit you want to update. Press 0 to return to main menu.");
            string habitName = Console.ReadLine();

            int habitId = _habitTrackerRepository.GetHabitIdByName(habitName);
            bool habitFound = habitId != -1;

            if (!habitFound)
            {
                Console.WriteLine("Habit doesn't exist!");
                Thread.Sleep(1000);
                UpdateHabit();
                return;
            }

            Console.WriteLine("Enter habit.");
            string name = Console.ReadLine();
            Console.WriteLine("Enter measurement unit.");
            int measurementUnit = GetNumberInput("\n\nPleaser enter measurement unit");

            bool success = _habitTrackerRepository.UpdateHabit(habitId, name, measurementUnit);

            if (success) Console.WriteLine($"Habit with ID {habitId} was updated.");
            else Console.WriteLine("Sorry, update failed.");
        }

        private static void UpdateRecord()
        {
            Console.Clear();
            GetAllRecords();

            int idInput = GetNumberInput("\n\nPlease enter the ID of the record you want to update. Press 0 to return to main menu.");

            if (!_habitTrackerRepository.DoesRecordExist(idInput))
            {
                Console.WriteLine("Record doesn't exist!");
                Thread.Sleep(1000);
                UpdateRecord();
                return;
            }

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPleaser enter quantity");

            bool success = _habitTrackerRepository.UpdateRecord(idInput, date, quantity);

            if (success) Console.WriteLine($"Row with ID {idInput} was updated.");
            else Console.WriteLine("Sorry, update failed.");
        }

        private static void DeleteHabit()
        {
            Console.Clear();
            Console.WriteLine("\n\nPlease enter the name of the habit you want to delete or 0 to go back to main menu");
            string habitName = Console.ReadLine();

            int habitId = _habitTrackerRepository.GetHabitIdByName(habitName);
            bool habitFound = habitId != -1;

            if (!habitFound)
            {
                Console.WriteLine("Habit doesn't exist!");
                Thread.Sleep(1000);
                UpdateHabit();
                return;
            }

            bool success = _habitTrackerRepository.DeleteHabit(habitId);

            if (success) Console.WriteLine($"Habit '{habitName}' was deleted.");
            else Console.WriteLine("Sorry, Delete failed.");
        }

        private static void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();

            int idInput = GetNumberInput("\n\nPlease enter the ID of the record you want to delete or 0 to go back to main menu");

            if (!_habitTrackerRepository.DoesRecordExist(idInput))
            {
                Console.WriteLine("Record doesn't exist!");
                Thread.Sleep(1000);
                DeleteRecord();
                return;
            }

            bool success = _habitTrackerRepository.DeleteRecord(idInput);

            if (success) Console.WriteLine($"Row with ID {idInput} was deleted.");
            else Console.WriteLine("Sorry, Delete failed.");
        }

        private static void GetAllRecords()
        {
            Console.Clear();

            Console.WriteLine("\n\nPlease find below a list of all your records.");
            List<Habit> records = _habitTrackerRepository.GetListOfAllRecords();

            if (records.Count == 0)
            {
                Console.WriteLine("Sorry, no rows found.");
            }
            else
            {
                foreach (var record in records)
                {
                    Console.WriteLine($"ID: {record.ID} - Habit: {record.HabitName} - Date: {record.Date.ToString("dd-MM-yyyy")} - Measurement Unit: {record.MeasurementUnit} - Quantity: {record.Quantity}");
                    // Console.WriteLine($"{record.ID}) You drank {record.Quantity} glasses of water on {record.Date.ToString("dd-MM-yyyy")}");
                }
            }
        }

        private static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            do
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nPlease choose an action:");
                Console.WriteLine("\n0) Close Application");
                Console.WriteLine("1) View All Records");
                Console.WriteLine("2) Insert Record");
                Console.WriteLine("3) Delete Record");
                Console.WriteLine("4) Update Record");
                Console.WriteLine("5) Create Habit");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Console.WriteLine("\nBye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        CreateRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    case "5":
                        CreateHabit();
                        break;
                    case "6":
                        UpdateHabit();
                        break;
                    case "7":
                        DeleteHabit();
                        break;
                    default:
                        Console.WriteLine("Invalid command!");
                        break;

                }
            } while (closeApp == false);
        }

        private static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            int validatedInput;
            while (!int.TryParse(numberInput, out validatedInput))
            {
                Console.WriteLine("Please enter a valid number!");
                numberInput = Console.ReadLine();
            }

            return validatedInput;
        }

        private static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date in the following format: dd-mm-yy. Press 0 to return to main menu.");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-mm-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid Date!");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }
    }
}