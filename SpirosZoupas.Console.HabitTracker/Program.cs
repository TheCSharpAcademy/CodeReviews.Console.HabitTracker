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
            _habitTrackerRepository.CreateTable();
            GetUserInput();
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
                        InsertRecord();
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
                    Console.WriteLine($"{record.ID} Habit: {record.HabitName} Measurement Unit: {record.MeasurementUnit} Quantity: {record.Quantity}");
                    // Console.WriteLine($"{record.ID}) You drank {record.Quantity} glasses of water on {record.Date.ToString("dd-MM-yyyy")}");
                }
            }
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

            int quantity = GetNumberInput("\n\nPleaser enter number of glasses");

            bool success = _habitTrackerRepository.Update(idInput, date, quantity);

            if (success) Console.WriteLine($"Row with ID {idInput} was updated.");
            else Console.WriteLine("Sorry, update failed.");
        }

        private static void InsertRecord()
        {
            Console.WriteLine("\n\nPlease enter habit name");
            string habit = Console.ReadLine();

            int habitId = _habitTrackerRepository.GetHabitIdByName(habit);
            bool habitFound = habitId != -1;

            if (!habitFound)
            {
                Console.WriteLine("Habit doesn't exist!");
                Thread.Sleep(1000);
                InsertRecord();
                return;
            }

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease enter number of glasses.");

            bool success = _habitTrackerRepository.Insert(habitId, date, quantity);

            if (success) Console.WriteLine("Insert Successful");
            else Console.WriteLine("Sorry, insert failed.");
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

            bool success = _habitTrackerRepository.Delete(idInput);

            if (success) Console.WriteLine($"Row with ID {idInput} was deleted.");
            else Console.WriteLine("Sorry, Delete failed.");
        }
    }
}