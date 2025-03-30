using SpirosZoupas.Console.HabitTracker.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace habit_tracker
{
    public class HabitTrackerApp
    {
        private readonly HabitTrackerRepository _habitTrackerRepository;

        public HabitTrackerApp(HabitTrackerRepository habitTrackerRepository)
        {
            _habitTrackerRepository = habitTrackerRepository;
        }

        public void CreateTables() =>
            _habitTrackerRepository.CreateTables();

        public void CreateHabit()
        {
            Console.WriteLine("\n\nPlease enter a name for the habit");
            string habit = Console.ReadLine();
            Console.WriteLine("Please enter unit of measurement");
            string uom = Console.ReadLine();

            var parameters = new Dictionary<string, object>
            {
                { "Name", habit },
                { "MeasurementUnit", uom }
            };

            bool success = _habitTrackerRepository.Insert("habit", parameters);

            Console.WriteLine(success ? "Habit Created Successfully" : "Sorry, insert failed.");
        }

        public void CreateRecord()
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

            var parameters = new Dictionary<string, object>
            {
                { "Date", date },
                { "Quantity", quantity },
                { "HabitId", habitId }
            };

            bool success = _habitTrackerRepository.Insert("habit_tracker", parameters);

            if (success) Console.WriteLine("Insert Successful");
            else Console.WriteLine("Sorry, insert failed.");
        }

        public void UpdateHabit()
        {
            Console.Clear();
            GetAllHabits();

            int habitId = GetNumberInput("\n\nPlease enter ID of the habit you want to update. Press 0 to return to main menu.");

            bool habitFound = _habitTrackerRepository.DoesRowExist(habitId, "habit");

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

        public void UpdateRecord()
        {
            Console.Clear();
            GetAllRecords();

            int idInput = GetNumberInput("\n\nPlease enter the ID of the record you want to update. Press 0 to return to main menu.");

            if (!_habitTrackerRepository.DoesRowExist(idInput, "habit_tracker"))
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

        public void DeleteHabit()
        {
            Console.Clear();
            int habitId = GetNumberInput("\n\nPlease enter ID of the habit you want to delete or 0 to go back to main menu");

            bool habitFound = _habitTrackerRepository.DoesRowExist(habitId, "habit");

            if (!habitFound)
            {
                Console.WriteLine("Habit doesn't exist!");
                Thread.Sleep(1000);
                UpdateHabit();
                return;
            }

            bool success = _habitTrackerRepository.Delete("habit", habitId);

            if (success) Console.WriteLine($"Habit with ID '{habitId}' was deleted.");
            else Console.WriteLine("Sorry, Delete failed.");
        }

        public void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();

            int idInput = GetNumberInput("\n\nPlease enter the ID of the record you want to delete or 0 to go back to main menu");

            if (!_habitTrackerRepository.DoesRowExist(idInput, "habit_tracker"))
            {
                Console.WriteLine("Record doesn't exist!");
                Thread.Sleep(1000);
                DeleteRecord();
                return;
            }

            bool success = _habitTrackerRepository.Delete("habit_tracker", idInput);

            if (success) Console.WriteLine($"Row with ID {idInput} was deleted.");
            else Console.WriteLine("Sorry, Delete failed.");
        }

        public void GetAllRecords()
        {
            Console.Clear();

            Console.WriteLine("\n\nPlease find below a list of all your records.");
            List<HabitRow> records = _habitTrackerRepository.GetListOfAllRecords();

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

        public void GetAllHabits()
        {
            Console.Clear();

            Console.WriteLine("\n\nPlease find below a list of all your habits.");
            List<Habit> habits = _habitTrackerRepository.GetListOfAllHabits();

            if (habits.Count == 0)
            {
                Console.WriteLine("Sorry, no habits found.");
            }
            else
            {
                foreach (var habit in habits)
                {
                    Console.WriteLine($"ID: {habit.ID} - Name: {habit.Name} - Measurement Unit: {habit.MeasurementUnit}");
                }
            }
        }

        public void GetUserInput()
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
                Console.WriteLine("5) View all Habits");
                Console.WriteLine("2) Insert Habit");
                Console.WriteLine("7) Delete Habit");
                Console.WriteLine("8) Update Habit");
                Console.WriteLine("9) View Reports");


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
                        GetAllHabits();
                        break;
                    case "6":
                        CreateHabit();
                        break;
                    case "7":
                        UpdateHabit();
                        break;
                    case "8":
                        DeleteHabit();
                        break;
                    case "9":
                        GetReportMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid command!");
                        break;

                }
            } while (closeApp == false);
        }

        private void GetReportMenu()
        {
            Console.WriteLine("\n\nREPORT MENU");
            Console.WriteLine("\nPlease choose an action:");
            Console.WriteLine("\n0) Close Application");
            Console.WriteLine("1) View number of times a habit was practiced");
            Console.WriteLine("2) View total amount of measurement unit of a hobby");

            string input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    Console.WriteLine("\nBye!\n");
                    Environment.Exit(0);
                    break;
                case "1":
                    GetNumberOfTimesHabitWasPracticed();
                    break;
                case "2":
                    GetTotalMeasurementUnits();
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;

            }
        }

        private void GetNumberOfTimesHabitWasPracticed()
        {
            Console.Clear();
            GetAllHabits();

            int habitId = GetNumberInput("\n\nPlease enter ID of habit");

            bool habitFound = _habitTrackerRepository.DoesRowExist(habitId, "habit");

            if (!habitFound)
            {
                Console.WriteLine("Habit doesn't exist!");
                Thread.Sleep(1000);
                GetNumberOfTimesHabitWasPracticed();
                return;
            }

            int occurences = _habitTrackerRepository.GetHabitTrackerCountByHabitId(habitId);

            Console.WriteLine($"You practiced this habit {occurences} times last year!");
        }

        private void GetTotalMeasurementUnits()
        {
            throw new NotImplementedException();
        }

        private int GetNumberInput(string message)
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

        private string GetDateInput()
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
