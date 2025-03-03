namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Manages input for <see cref="HabitTrackerApp"/> application
    /// </summary>
    internal class OutputManager
    {
        public string DateFormat { get; init; }

        public const int MainMenuLength = 5;
        public const int HabitMenuLength = 7;

        private const int IdColumnWidth = 5;
        private const int NameColumnWidth = 15;
        private const int UnitColumnWidth = 15;
        private const int BorderPadding = 4;
        private const int HorizontalLineLength = IdColumnWidth + NameColumnWidth + UnitColumnWidth + BorderPadding;

        private const string IdFormatSpecifier = "{0,-5:N}";
        private const string FormatSpecifier = "{0,-15:N}";

        

        /// <summary>
        /// Initializes new object of <see cref="OutputManager"/> class
        /// </summary>
        /// <param name="dateTimeFormat"><see cref="string"/> represeting DateTime format</param>
        public OutputManager(string dateFormat)
        {
            DateFormat = dateFormat;
        }
        
        /// <summary>
        /// Resets console - clear screen and sets cursor to position (0,0)
        /// </summary>
        public void ResetConsole()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }
        /// <summary>
        /// Prints a main menu of <see cref="HabitTrackerApp"/> to the user
        /// </summary>
        public void PrintMainMenu()
        {
            ResetConsole();
            Console.WriteLine("Welcome to habit tracker application");
            Console.WriteLine("Habit tracker can be used to track custom habits");
            Console.WriteLine();
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. Check tracked habits");
            Console.WriteLine("\t2. Manage existing habit");
            Console.WriteLine("\t3. Create new habit");
            Console.WriteLine("\t4. Delete habit");
            Console.WriteLine("\t5. Exit the application");
            Console.Write("Your selection: ");
        }
        /// <summary>
        /// Prints a habit menu of <see cref="HabitTrackerApp"/> to the user
        /// </summary>
        public void PrintHabitMenu()
        {
            ResetConsole();
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. View records");
            Console.WriteLine("\t2. Insert record");
            Console.WriteLine("\t3. Update record");
            Console.WriteLine("\t4. Delete record");
            Console.WriteLine("\t5. Get Report");
            Console.WriteLine("\t6. Exit to main menu");
            Console.WriteLine("\t7. Close the application");

            Console.Write("Your selection: ");
        }
        /// <summary>
        /// Prints out table header containing title aligned to the middle
        /// </summary>
        /// <param name="title"><see cref="string"/> value representing title - Habit name</param>
        private void PrintTableHeader(string title)
        {
            ResetConsole();
            PrintHorizontalLine();
           
            int totalSpaces = (HorizontalLineLength - 2 - title.Length);
            int leftpadding = totalSpaces / 2;
            int rightPadding = totalSpaces - leftpadding;
            Console.WriteLine("|" + new string(' ', leftpadding) + title + new string(' ', rightPadding) + "|");

            PrintHorizontalLine();
        }
        /// <summary>
        /// Prints out <see cref="Habit"/> entities to the output in form of table
        /// </summary>
        /// <param name="habits"><see cref="List{Habit}"/> collection of <see cref="Habit"/> entities</param>
        public void PrintHabits(List<Habit> habits)
        {
            PrintTableHeader("Habits");

            PrintHabitColumns();
            PrintHorizontalLine();
            if (habits == null || habits.Count == 0)
            {
                Console.WriteLine("You have no tracked habits yet");
                PrintHorizontalLine();
                return;
            }

            foreach (Habit habit in habits)
            {
                PrintHabit(habit);
            }
            PrintHorizontalLine();

        }
        /// <summary>
        /// Prints out <see cref="HabitRecord"/> entities to the output in form of table
        /// </summary>
        /// <param name="habits"><see cref="List{T}"/> collection of <see cref="HabitRecord"/> entities</param>
        public void PrintHabitRecords(List<HabitRecord> records, string unit)
        {
            PrintTableHeader("Habit Records");

            PrintHabitRecordColumns();
            PrintHorizontalLine();

            if (records == null || records.Count == 0)
            {
                Console.WriteLine("You have no records of this habit yet");
                PrintHorizontalLine();
                return;
            }

            foreach (HabitRecord record in records) 
            { 
                PrintHabitRecord(record, unit);
            }

            PrintHorizontalLine();
        }
        /// <summary>
        /// Prints out contents of single <see cref="HabitRecord"/> entity to the output as line in the table
        /// </summary>
        /// <param name="record"><see cref="HabitRecord"/> entity to be printed</param>
        /// <param name="unit"><see cref="string"/> representing unit for the record</param>
        private void PrintHabitRecord(HabitRecord record, string unit)
        {
            Console.Write('|');
            Console.Write(string.Format("{0,-5}", record.Id));
            Console.Write('|');
            Console.Write(string.Format(FormatSpecifier, record.Date.ToString(DateFormat)));
            Console.Write('|');
            Console.Write(string.Format(FormatSpecifier, record.Volume + " " + unit));
            Console.WriteLine('|');
        }
        /// <summary>
        /// Prints out contents of single <see cref="Habit"/> entity to the output as line in the table
        /// </summary>
        /// <param name="record"><see cref="Habit"/> entity to be printed</param>
        private void PrintHabit(Habit habit)
        {
            Console.Write('|');
            Console.Write(string.Format("{0,-5}", habit.Id));
            Console.Write('|');
            Console.Write(string.Format(FormatSpecifier, habit.Name));
            Console.Write('|');
            Console.Write(string.Format(FormatSpecifier, habit.Unit));
            Console.WriteLine('|');
        }
        /// <summary>
        /// Print columns provided as argument
        /// </summary>
        /// <param name="columnNames">string array containing names of columns</param>
        private void PrintColumns(string[] columnNames)
        {
            Console.Write('|');
            Console.Write(string.Format(IdFormatSpecifier, columnNames[0]));
            Console.Write('|');

            for (int i = 1; i < columnNames.Length; i++) 
            {
                Console.Write(string.Format(FormatSpecifier, columnNames[i]));
                Console.Write('|');
            }
            Console.WriteLine();
        }
        /// <summary>
        /// Prints out columns for <see cref="Habit"/> entity
        /// </summary>
        public void PrintHabitColumns()
        {
            PrintColumns(["ID", "Name", "Unit"]);
        }
        /// <summary>
        /// Prints out columns for <see cref="Habit"/> entity
        /// </summary>
        public void PrintHabitRecordColumns()
        {
            PrintColumns(["ID", "Date", "Volume"]);
        }
        /// <summary>
        /// Prints out horizontal line to the output
        /// </summary>
        public void PrintHorizontalLine()
        {
            Console.WriteLine(new string('-', HorizontalLineLength));
        }

    }
}
