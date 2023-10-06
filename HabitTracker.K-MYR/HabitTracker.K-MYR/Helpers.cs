using HabitTracker.K_MYR.Models;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace HabitTracker.K_MYR
{
    static internal class Helpers
    {
        static internal void GetUserInput()
        {
            bool closeApp = false;

            while (!closeApp)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU".PadLeft(19));
                Console.WriteLine("------------------------------");
                Console.WriteLine("0 - View Records");
                Console.WriteLine("1 - Insert Record");
                Console.WriteLine("2 - Delete Record");
                Console.WriteLine("3 - Update Records");
                Console.WriteLine("4 - Create Report");
                Console.WriteLine("5 - Exit Application");
                Console.WriteLine("------------------------------");

                string Input = Console.ReadLine();

                switch (Input)
                {
                    case "0":
                        ShowAllRecords();
                        Console.WriteLine("Press enter to go back to the main menu");
                        Console.ReadLine();
                        break;
                    case "1":
                        InsertRecord();
                        break;
                    case "2":
                        DeleteRecord();
                        break;
                    case "3":
                        UpdateRecord();
                        break;
                    case "4":
                        ShowReport();
                        break;
                    case "5":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please enter a valid choice (0-4)!");
                        break;
                }
            }
        }

        private static void ShowReport()
        {
            Console.Clear();
            var habits = PrintAllHabits();
            int habitIndex = GetNumberInput("Please choose a habit or enter -1 to return to the main menu\n");

            while (habitIndex >= habits.Count)
            {
                habitIndex = GetNumberInput($"Invalid Number! Please enter a valid input: ", -1);
            }
            if (habitIndex == -1)
                return;

            var habit = habits[habitIndex];
            var results = SQLiteOperations.SelectTotalByHabit(habits[habitIndex]);

            Console.Clear();
            Console.WriteLine($"Report for {habit.Name}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Total number of records      : {results[1]}");
            Console.WriteLine($"Total number of {habit.Measurement,-12} : {results[0]}");
            Console.WriteLine($"Average {habit.Measurement,-20} : {results[2]}");
            Console.WriteLine($"Highest amount of {habit.Measurement,-10} : {results[3]}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("\nPress enter to go back to the main menu");
            Console.ReadLine();
        }

        private static void ShowAllRecords()
        {
            Console.Clear();
            List<HabitRecord> tableData = SQLiteOperations.SelectAll();
            tableData.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            Console.Clear();
            Console.WriteLine("----------------------------------");
            if (tableData.Count != 0)
            {
                foreach (var row in tableData)
                {
                    Console.WriteLine($"{row.Id,3} | {row.Date:dd-MM-yyy} | {row.Habit,15}  | Quantity: {row.Quantity} ");
                }
            }
            else
            {
                Console.WriteLine("No records were found");
            }
            Console.WriteLine("----------------------------------\n");
        }

        private static void InsertRecord()
        {
            Habit habit = GetHabit();
            string dateInput = GetDateInput();
            int numberInput = GetNumberInput("Please insert quantity (no decimals allowed)\n");

            SQLiteOperations.Insert(habit, dateInput, numberInput);
        }

        private static void DeleteRecord()
        {
            Console.Clear();
            ShowAllRecords();
            int numberInput;

            do
            {
                numberInput = GetNumberInput("Please enter the Id of the record you want to delete. Type 0 to return to the main menu\n");

                int rowCount = SQLiteOperations.Delete(numberInput);

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with Id {numberInput} doesn't exist");
                }
                else
                {
                    Console.Clear();
                    ShowAllRecords();
                    Console.WriteLine($"Record with Id {numberInput} has been deleted!");
                }
            } while (numberInput != 0);

        }

        private static void UpdateRecord()
        {
            Console.Clear();
            ShowAllRecords();
            int numberInput;

            do
            {
                numberInput = GetNumberInput("Please enter the Id of the record you want to update. Type 0 to return to the main menu\n");

                if (SQLiteOperations.RecordExists(numberInput) == 1)
                {
                    string date = GetDateInput();
                    int quantity = GetNumberInput("Please insert quantity (no decimals allowed)\n");
                    SQLiteOperations.Update(numberInput, date, quantity);
                    Console.Clear();
                    ShowAllRecords();
                    Console.WriteLine($"Record with the Id {numberInput} has been updated\n");

                }
                else
                {
                    Console.WriteLine("\nRecord with Id {id} doesn't exist");
                }
            } while (numberInput != 0);
        }

        private static Habit GetHabit()
        {
            Console.Clear();
            var habits = PrintAllHabits();
            int habitIndex = GetNumberInput("Please choose a habit or enter '-1' to add a new one\n", -1);

            while (habitIndex < -1 || habitIndex >= habits.Count)
            {
                habitIndex = GetNumberInput($"Invalid Number! Please enter a valid input: ", -1);
            }

            if (habitIndex == -1)
            {
                string habitName = GetStringInput("Please enter the name of the habit: ");
                string measurement = GetStringInput("Please enter the type of measurement of the habit (km, glasses, etc.): ");
                return new Habit
                {
                    Name = habitName,
                    Measurement = measurement
                };
            }

            return habits[habitIndex];

        }

        private static List<Habit> PrintAllHabits()
        {
            var habits = SQLiteOperations.GetAllHabits();
            Console.Clear();
            Console.WriteLine("Current Habits".PadLeft(22));
            Console.WriteLine("------------------------------");
            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"{i,3} | {habits[i].Name} ");
            }
            Console.WriteLine("------------------------------\n");

            return habits;
        }

        private static string GetDateInput()
        {
            Console.WriteLine("Please enter the date: (Format: dd-mm-yy)");
            string Input = Console.ReadLine();

            while (!DateTime.TryParseExact(Input, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date. (Format dd-mm-yy)");
                Input = Console.ReadLine();
            }
            return Input;
        }

        private static int GetNumberInput(string message, int lowerLimit = 0)
        {
            Console.Write(message);
            string numberInput = Console.ReadLine();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < lowerLimit)
            {
                Console.WriteLine("Invalid input");
                Console.Write(message);
                numberInput = Console.ReadLine();
            }

            return Convert.ToInt32(numberInput);
        }

        private static string GetStringInput(string message)
        {
            Console.WriteLine(message);
            string stringInput = Console.ReadLine();

            while (string.IsNullOrEmpty(stringInput))
            {
                Console.WriteLine("\nInvalid input.");
                stringInput = Console.ReadLine();
            }

            return stringInput;
        }
    }
}


