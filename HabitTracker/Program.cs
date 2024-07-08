using ConsoleTables;

namespace HabitTracker
{
    class Program
    {
        private static LocalDatabaseService _dbService = null!;
        private static Habit _habit = null!;
        static void Main(string[] args)
        {
            _dbService = new LocalDatabaseService("Data Source=habitTracker.sqlite");

            Console.WriteLine("Habit Tracker");
            Console.WriteLine("Create custom habit or use default(Habit of doing eye exercises. Measurement: number of eye exercises per day.)?");
            Console.WriteLine("Please, type \"n\" for new habit or skip by \"Enter\":");
            try
            {
                string input = Helpers.InputStringWithRegexValidation($"^(n|)$", "Wrong input. Please, type \"n\" for new habit or skip by \"Enter\":");
                if (input == "n")
                {
                    Console.WriteLine("Habit name (example - trips to the gym):");
                    string inputHabitName = Helpers.InputStringWithValidation();

                    Console.WriteLine("Habit measurement (example - trips per day):");
                    string inputHabitMeasurement = Helpers.InputStringWithValidation();

                    _habit = new Habit() { Name = inputHabitName, MeasurementMethod = inputHabitMeasurement };
                    _habit = _dbService.CreateHabit(_habit);
                    MainMenu();
                }
                else
                {
                    _habit = _dbService.GetLastHabit();
                    MainMenu();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Main([]); // restart after crash
            }
            finally
            {
                _dbService.CloseConnection();
            }
        }

        public static void MainMenu()
        {
            bool endApp = false;
            while (!endApp)
            {
                Console.WriteLine($"/// Habit - {_habit.Name} \\\\\\");
                Console.WriteLine("------------------------");
                Console.WriteLine("Main menu:");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("\tType 0 to Close Application");
                Console.WriteLine("\tType 1 to View all Records");
                Console.WriteLine("\tType 2 to Insert Record");
                Console.WriteLine("\tType 3 to Update Record");
                Console.WriteLine("\tType 4 to Delete Record");
                Console.WriteLine("------------------------");

                int input = Helpers.InputNumberWithValidation();

                switch (input)
                {
                    case 0:
                        endApp = true;
                        break;
                    case 1:
                        Console.Clear();
                        Console.WriteLine("<<All Records>>");
                        ViewAllRecords();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("<<Insert Record>>");
                        InsertRecord();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("<<Update Record>>");
                        UpdateRecord();
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("<<Delete Record>>");
                        DeleteRecord();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Please enter the number corresponding to the menu item:");
                        break;
                }
            }
        }

        public static void InsertRecord()
        {
            string datePattern = "MM-dd-yyyy";
            Console.WriteLine("Record date in format (month-day-year).");
            Console.WriteLine("Constraints: no older than 1 year, no more than the current day. :");

            DateTime date = Helpers.InputDataWithValidation(
                datePattern,
                DateTime.Now.AddYears(-1),
                DateTime.Now,
                "Wrong input. Input record date in format (month-day-year):");
            if (_dbService.IsExistDateRecord(date))
            {
                Console.WriteLine("There is already an entry with the same date. Сhoose another date.");
                return;
            }

            Console.WriteLine("Number Of Approaches:");
            int numberOfApproaches = Helpers.InputNumberWithValidation(0, int.MaxValue);

            HabitRecord habitRecord = new HabitRecord()
            {
                Date = date,
                NumberOfApproachesPerDay = numberOfApproaches,
            };

            _dbService.CreateHabitRecord(habitRecord, _habit.Id);
            Console.Clear();
        }

        public static void ViewAllRecords()
        {
            string datePattern = "MM-dd-yyyy"; // ignored date format order for my culture
            var habitRecords = _dbService.GetAllHabitRecords(_habit.Id).OrderBy(x => x.Date);

            if (habitRecords.Count() == 0) {
                Console.WriteLine("No records yet...");
                Console.WriteLine();
                return;
            }

            var table = new ConsoleTable("Id", "Date", $"Number Of Approaches <-> {_habit.MeasurementMethod}");
            table.Configure(o => { o.EnableCount = false; });
            foreach (var habitRecord in habitRecords)
            {
                table.AddRow(habitRecord.Id, habitRecord.Date.ToString(datePattern), habitRecord.NumberOfApproachesPerDay);
            }

            table.Write(Format.Default);
            Console.WriteLine();
        }

        public static void UpdateRecord()
        {
            Console.WriteLine("Enter record Id for update:");
            int id = Helpers.InputNumberWithValidation(1, int.MaxValue);

            HabitRecord? habitRecord = _dbService.GetHabitRecordById(id, _habit.Id);
            if (habitRecord == null)
            {
                Console.WriteLine("Find an existing habit record Id.");
                Console.WriteLine();
                return;
            }

            Console.WriteLine($"Current Number Of Approaches: {habitRecord.NumberOfApproachesPerDay}");
            Console.WriteLine($"Enter new Number Of Approaches:");
            int numberOfApproaches = Helpers.InputNumberWithValidation(0, int.MaxValue);

            habitRecord.NumberOfApproachesPerDay = numberOfApproaches;

            _dbService.UpdateHabitRecord(habitRecord);
            Console.Clear();
        }

        public static void DeleteRecord()
        {
            Console.WriteLine("Enter record Id for delete:");
            int id = Helpers.InputNumberWithValidation(1, int.MaxValue);
            HabitRecord? habitRecord = _dbService.DeleteHabitRecord(id, _habit.Id);
            if (habitRecord == null)
            {
                Console.WriteLine("Find an existing habit record Id. Try delete again.");
                Console.WriteLine();
                return;
            }
            Console.Clear();
        }
    }
}
