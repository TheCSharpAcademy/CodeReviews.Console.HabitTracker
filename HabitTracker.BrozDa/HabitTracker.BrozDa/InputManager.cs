using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Manages input for <see cref="HabitTrackerApp"/> application
    /// </summary>
    internal class InputManager
    {
        public string DateTimeFormat { get; init; }

        /// <summary>
        /// Initializes new object of InputManager class
        /// </summary>
        /// <param name="dateTimeFormat"><see cref="string"/> represeting DateTime format</param>
        public InputManager(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }
        /// <summary>
        /// Resets console - clear screen, set cursor to position (0,0) and set cursor visible
        /// </summary>
        private void ResetConsole()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
        }
        /// <summary>
        /// Retrieves a new <see cref="HabitRecord"/> based on user input
        /// </summary>
        /// <returns><see cref="HabitRecord"/> entity based on user input</returns>
        public HabitRecord GetNewRecord()
        {
            HabitRecord newRecord = new HabitRecord();

            newRecord.Date = GetDateForRecord();
            newRecord.Volume = GetVolumeForRecord();

            return newRecord;
        }
        /// <summary>
        /// Retrieved DateTime value from user input
        /// </summary>
        /// <returns><see cref="DateTime"/> value based on user input</returns>
        private DateTime GetDateForRecord()
        {
            DateTime date;
            while (true)
            {
                Console.Write($"Please enter date value in {DateTimeFormat.ToUpper()} format, or enter [today] for today's date: ");

                string? userInput = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    continue;
                }

                if (userInput == "today")
                {
                    return DateTime.Now;
                }

                if (DateTime.TryParseExact(userInput, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }

                Console.WriteLine("Invalid date format. Please try again.");
            }
        }
        /// <summary>
        /// Retrieves integer value representing volume of the record from user input
        /// </summary>
        /// <returns><see cref="int"/> value representing volume of the record from user input</returns>
        private int GetVolumeForRecord()
        {

            string prompt = "Please enter positive numeric value for volume: ";
            int numericInput = GetValidIntegerInput(prompt, 0, int.MaxValue);

            return numericInput;
        }
        /// <summary>
        /// Retrieves a new <see cref="Habit"/> based on user input
        /// </summary>
        /// <returns><see cref="Habi"/> entity based on user input</returns>
        public Habit GetNewHabit(List<Habit> existingHabits)
        {
            string name = GetNewHabitName(existingHabits);
            string unit = GetNewHabitUnit();

            return new Habit() { Name = name, Unit = unit };
        }
        /// <summary>
        /// Retrieves string value representing name of <see cref="Habit"/> from user input
        /// </summary>
        /// <returns><see cref="string"/> value representing name of <see cref="Habit"/> from user input</returns>
        private string GetNewHabitName(List<Habit> existingHabits)
        {
            HashSet<string> names = new HashSet<string>(existingHabits.Select(x => x.Name));
            ResetConsole();

            Console.WriteLine("Name of the habit can contain only letters or space between words, leading or trailing spaces are not permitted");
            string name;

            do
            {
                Console.Write("Please enter valid habit name: ");
                name = Console.ReadLine() ?? string.Empty;

                if (names.Contains(name))
                {
                    Console.WriteLine("This habit already exists");
                }
                else if (!IsInputAlphabetic(name))
                {
                    Console.WriteLine("This name is not valid");
                }
                else
                {
                    return name;
                }
            } while (true);
        }
        /// <summary>
        /// Retrieves string value representing unit of <see cref="Habit"/> from user input
        /// </summary>
        /// <returns><see cref="string"/> value representing unit of <see cref="Habit"/> from user input</returns>
        private string GetNewHabitUnit()
        {
            string unit;

            do
            {
                Console.Write("Please enter valid measurement unit: ");
                unit = Console.ReadLine() ?? string.Empty;

                if (IsInputAlphabetic(unit))
                {
                    return unit;
                }

            } while (true);
        }
        /// <summary>
        /// Checks whether input contains only letters
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="bool"/> true if input contains only letters, false otherwise</returns>
        private bool IsInputAlphabetic(string name)
        {
            return name == string.Empty || string.IsNullOrWhiteSpace(name)
                ? false
                : Regex.IsMatch(name, @"^[a-zA-Z]+( [a-zA-Z]+)*$");
        }
        /// <summary>
        /// Gets a confirmation from user to confirm submitted operation performed on <see cref="Habit"/> entity
        /// </summary>
        /// <param name="habit"><see cref="Habit"/> entity being adjusted by the operation</param>
        /// <param name="operation">Operation being performed eg.: delete, update, add</param>
        /// <returns><see cref="bool"/> true if user confirms the operation, false otherwise</returns>
        public bool ConfirmHabitOperation(Habit habit, string operation)
        {
            Console.CursorVisible = false;

            Console.WriteLine();
            Console.WriteLine($"Habit [{habit.Name}] with unit [{habit.Unit}] will be {operation}d.");
            Console.WriteLine();
            Console.WriteLine("Please press [ENTER] to confirm, anything else to cancel");

            return Console.ReadKey(false).Key == ConsoleKey.Enter;

        }
        /// <summary>
        /// Gets a confirmation from user to confirm submitted operation performed on <see cref="HabitRecord"/> entity
        /// </summary>
        /// <param name="habit"><see cref="Habit"/> to which provided record falls to</param>
        /// <param name="record"><see cref="HabitRecord"/> entity being adjusted by the operation</param>
        /// <param name="operation">Operation being performed eg.: delete, update, add</param>
        /// <returns></returns>
        public bool ConfirmHabitRecordOperation(Habit habit, HabitRecord record, string operation)
        {
            Console.CursorVisible = false;

            Console.WriteLine();
            Console.WriteLine($"Habit Record [{record.Date.ToString(DateTimeFormat)}] with volume [{record.Volume}] for habit [{habit.Name}] will be {operation}d.");
            Console.WriteLine();
            Console.WriteLine("Please press [ENTER] to confirm, anything else to cancel");

            return Console.ReadKey(true).Key == ConsoleKey.Enter;

        }
        /// <summary>
        /// Gets a valid <see cref="int"/> value representing ID of record
        /// </summary>
        /// <param name="ids"><see cref="HashSet{int}"/> containing values </param>
        /// <param name="operation"></param>
        /// <returns>value representing ID of record, or 0 in case user decided to exit</returns>
        public int GetValidIdFromUser(HashSet<int> ids, string operation)
        {
            int numericInput;
            string? input;
            string prompt = $"Please enter a valid ID to {operation}, or enter [0] to return to the menu: ";

            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();

            } while (!int.TryParse(input, out numericInput) || (!ids.Contains(numericInput) && numericInput != 0));

            return numericInput;
        }
        /// <summary>
        /// Method facilitating getting user input in the menu
        /// </summary>
        /// <param name="maxValue"><see cref="int"/> value representing maximum valid input representing menu option</param>
        /// <returns></returns>
        public int GetInputInMenu(int maxValue)
        {
            int numericInput = GetValidIntegerInput("",1, maxValue);
            return numericInput;
        }
        /// <summary>
        /// Retrieves valid integer input from the user
        /// </summary>
        /// <param name="prompt">string value representing prompt for the input</param>
        /// <param name="minValue">Minimum valid value - inclusive</param>
        /// <param name="maxValue">Maximum valid value - inclusive</param>
        /// <returns><see cref="int"/> value representing user input</returns>
        private int GetValidIntegerInput(string prompt, int minValue, int maxValue)
        {
            int numericInput;
            string? input;

            Console.Write(prompt);

            input = Console.ReadLine();

            while (!int.TryParse(input, out numericInput) || numericInput < minValue || numericInput > maxValue)
            {
                Console.Write("Please enter valid value: ");
                input = Console.ReadLine();
            }


            return numericInput;
        }
        /// <summary>
        /// Prints out "press any key to continue" text and awaits user input before moving forward
        /// </summary>
        public void PrintPressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

    }
}
