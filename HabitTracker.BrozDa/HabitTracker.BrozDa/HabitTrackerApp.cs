namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Represent Habit Tracker Application
    /// </summary>
    internal class HabitTrackerApp
    {
        private string _repositoryPath;
        private HabitRecordRepository _habitRecordRepository;
        private HabitRepository _habitRepository;
        private InputManager _inputManager;
        private OutputManager _outputManager;

        /// <summary>
        /// Initializes new instance of <see cref="HabitTrackerApp"/>
        /// </summary>
        /// <param name="repositoryPath"><see cref="string"/> containing path to the databse file</param>
        /// <param name="habitRepository">Instance of <see cref="HabitRepository"/> class</param>
        /// <param name="habitRecordRepository">Instance of <see cref="HabitRecordRepository"/> class</param>
        /// <param name="inputManager">Instance of <see cref="InputManager"/> class</param>
        /// <param name="outputManager">Instance of <see cref="OutputManager"/> class</param>
        public HabitTrackerApp(string repositoryPath, HabitRepository habitRepository, HabitRecordRepository habitRecordRepository, InputManager inputManager, OutputManager outputManager)
        {
            _repositoryPath = repositoryPath;
            _habitRepository = habitRepository;
            _habitRecordRepository = habitRecordRepository;
            _inputManager = inputManager;
            _outputManager = outputManager;
        }
        /// <summary>
        /// Main Application loop, database is created if it does not exist yet
        /// </summary>
        public void Run()
        {
            if(!DoesRepositoryExist())
            {
                _habitRepository.CreateTable();
                _habitRecordRepository.CreateTable();
                AutoFill();
            }

            bool isAppRunning = true;
            while (isAppRunning) 
            {
                isAppRunning = ProcessMainMenu();

            }
        }
        /// <summary>
        /// Checks whether the database already exists
        /// </summary>
        /// <returns>true if it exists, false otherwise</returns>
        private bool DoesRepositoryExist() => File.Exists(_repositoryPath);
        /// <summary>
        /// Process Main menu of the application and perform actions based on user input
        /// </summary>
        private bool ProcessMainMenu()
        {
            _outputManager.PrintMainMenu();
            int input = _inputManager.GetInputInMenu(OutputManager.MainMenuLength);

            switch ((MainMenuOptions)input)
            {
                case MainMenuOptions.ViewHabits:
                    HandleViewHabits();
                    break;
                case MainMenuOptions.ManageHabits:
                    HandleManageHabits();
                    break;
                case MainMenuOptions.CreateHabit:
                    HandleCreateHabit();
                    break;
                case MainMenuOptions.DeleteHabit:
                    HandleDeleteHabit();
                    break;
                case MainMenuOptions.ExitApplication:
                    return false;
                    
            }
            return true;    
        }
        /// <summary>
        /// Process Habit menu of the application and perform actions based on user input
        /// </summary>
        public void ProcessHabitMenu(Habit habit)
        {
            bool exitToMainMenu = false;

            while (!exitToMainMenu) 
            {
                _outputManager.PrintHabitMenu();
                int menuChoice = _inputManager.GetInputInMenu(OutputManager.HabitMenuLength);
                switch ((HabitMenuOptions)menuChoice)
                {
                    case HabitMenuOptions.ViewRecords:
                        HandleViewRecords(habit);
                        break;
                    case HabitMenuOptions.InsertRecord:
                        HandleInsertRecord(habit);
                        break;
                    case HabitMenuOptions.UpdateRecord:
                        HandleUpdateRecord(habit);
                        break;
                    case HabitMenuOptions.DeleteRecord:
                        HandleDeleteRecord(habit);
                        break;
                    case HabitMenuOptions.CreateReport:
                        HandleCreateReport(habit);
                        break;
                    case HabitMenuOptions.ExitToMainMenu:
                        exitToMainMenu = true;
                        break;
                    case HabitMenuOptions.ExitApplication:
                        Environment.Exit(0);
                        break;
                }
            }
        }
        /// <summary>
        /// Retrieves all records for provided <see cref="Habit"/> and prints them to the output
        /// </summary>
        /// <param name="habit"><see cref="Habit"/> for which user want to retrieve records</param>
        private void HandleViewRecords(Habit habit)
        {
            List<HabitRecord> records = _habitRecordRepository.GetAllByHabitID(habit.Id).ToList();

            _outputManager.PrintHabitRecords(records, habit.Unit);
            _inputManager.PrintPressAnyKeyToContinue();
        }
        /// <summary>
        /// Gets and inserts record to the database for provided <see cref="Habit"/>
        /// </summary>
        /// <param name="habit"><see cref="Habit"/> for which user want to insert new record</param>
        private void HandleInsertRecord(Habit habit)
        {
            HabitRecord newRecord = _inputManager.GetNewRecord();
            newRecord.habitId = habit.Id;

            if (_inputManager.ConfirmHabitRecordOperation(habit, newRecord, "create"))
            {
                Console.WriteLine("Adding new record...");
                _habitRecordRepository.Insert(newRecord);
                Console.WriteLine("Record added successfully");
            }
            else
            {
                Console.WriteLine("Operation aborted");
            }

            _inputManager.PrintPressAnyKeyToContinue();
        }
        /// <summary>
        /// Gets new values for <see cref="HabitRecord"/> and inserts it to the database for provided <see cref="Habit"/>
        /// </summary>
        /// <param name="habit"><see cref="Habit"/> for which user want to update the record</param>
        private void HandleUpdateRecord(Habit habit)
        {
            List<HabitRecord> records = _habitRecordRepository.GetAllByHabitID(habit.Id).ToList();
            _outputManager.PrintHabitRecords(records, habit.Unit);

            HashSet<int> ids = new HashSet<int>(records.Select(record => record.Id));
            int id = _inputManager.GetValidIdFromUser(new HashSet<int>(ids), "update");

            if (id == 0) { return; }

            HabitRecord habitRecord = _inputManager.GetNewRecord();
            habitRecord.Id = id;

            if (_inputManager.ConfirmHabitRecordOperation(habit, habitRecord, "update"))
            {
                Console.WriteLine("Updating record...");
                _habitRecordRepository.Update(habitRecord);
                Console.WriteLine("Record update successfully");
            }
            else
            {
                Console.WriteLine("Operation aborted");
            }
            _inputManager.PrintPressAnyKeyToContinue();
        }
        /// <summary>
        /// Deletes <see cref="HabitRecord"/> from the database
        /// </summary>
        /// <param name="habit"><see cref="Habit"/> for which user want to delete the record</param>
        private void HandleDeleteRecord(Habit habit)
        {
            List<HabitRecord> records = _habitRecordRepository.GetAllByHabitID(habit.Id).ToList();
            _outputManager.PrintHabitRecords(records, habit.Unit);

            HashSet<int> ids = new HashSet<int>(records.Select(record => record.Id));
            int id = _inputManager.GetValidIdFromUser(new HashSet<int>(ids), "delete");

            if (id == 0) { return; }

            HabitRecord toBeDeleted = records.FirstOrDefault(x => x.Id == id)!;

            if (_inputManager.ConfirmHabitRecordOperation(habit, toBeDeleted, "delete"))
            {
                Console.WriteLine("Deleting record...");
                _habitRecordRepository.Delete(toBeDeleted);
                Console.WriteLine("Record deleted successfully");
            }
            else
            {
                Console.WriteLine("Operation aborted");
            }
            _inputManager.PrintPressAnyKeyToContinue();
        }
        /// <summary>
        /// Gets all records for provided <see cref="Habit"/> and generates report and prints to the output
        /// </summary>
        /// <param name="habit"><see cref="Habit"/> for which user want to get the report</param>
        private void HandleCreateReport(Habit habit)
        {
            List<HabitRecord> records = _habitRecordRepository.GetAllByHabitID(habit.Id).ToList();
            ReportUnit report = new ReportUnit(habit, records);

            _outputManager.ResetConsole();
            Console.WriteLine(report.GenerateReport());
            _inputManager.PrintPressAnyKeyToContinue();
        }
        /// <summary>
        /// Retrieves all tracked Habits in the database and prints them to the output
        /// </summary>
        private void HandleViewHabits() 
        {
            List<Habit> habits = _habitRepository.GetAll().ToList();

            _outputManager.PrintHabits(habits);
            _inputManager.PrintPressAnyKeyToContinue();
        }
        /// <summary>
        /// Prints out currently tracked habits to the output, asks user to get Id of habit user wants to manage and then starts processing it
        /// </summary>
        private void HandleManageHabits()
        {
            List<Habit> habits = _habitRepository.GetAll().ToList();

            _outputManager.PrintHabits(habits);

            HashSet<int> ids = _habitRepository.GetIds().ToHashSet();

            int idToManage = _inputManager.GetValidIdFromUser(ids, "manage");
            if (idToManage == 0) { return; }

            Habit habitToManage = _habitRepository.GetHabitById(idToManage);

            ProcessHabitMenu(habitToManage);

        }
        /// <summary>
        /// Creates new habit for tracking based on user input
        /// </summary>
        private void HandleCreateHabit()
        {
            List<Habit> habits = _habitRepository.GetAll().ToList();

            Habit newHabit = _inputManager.GetNewHabit(habits);

            if (_inputManager.ConfirmHabitOperation(newHabit,"create"))
            {
                Console.WriteLine("Adding new habit...");
                _habitRepository.Insert(newHabit);
                Console.WriteLine("Habit added successfully");
            }
        }
        /// <summary>
        /// Prints out currently tracked habits to the output, asks user to get Id of habit user wants to delete and deletes chosen habit unless user decide to exit
        /// </summary>
        private void HandleDeleteHabit()
        {
            List<Habit> habits = _habitRepository.GetAll().ToList();
            _outputManager.PrintHabits(habits);

            HashSet<int> ids = new HashSet<int>(habits.Select(habit => habit.Id));
            int id = _inputManager.GetValidIdFromUser(new HashSet<int>(ids), "delete");

            if (id == 0) { return; }

            Habit toBeDeleted = habits.Where(x => x.Id == id).FirstOrDefault()!; 

            if (_inputManager.ConfirmHabitOperation(toBeDeleted, "delete"))
            {
                Console.WriteLine("Deleting habit and all its records");
                _habitRepository.Delete(toBeDeleted);
                _habitRecordRepository.DeleteAllByHabitId(id);
                Console.WriteLine("Habits and all it's record successfully deleted");
            }
        }
        /// <summary>
        /// Generates 3 new habits, then 100 random records for each habit and inserts them to the database
        /// </summary>
        private void AutoFill()
        {
            List<Habit> habits = GenerateHabitsForAutoFill();

            List<HabitRecord> records = new List<HabitRecord>();

            foreach(Habit habit in habits)
            {
                records.AddRange(GenerateRecordsForAutoFill(habit));
            }

            _habitRepository.InsertBulk(habits);
            _habitRecordRepository.InsertBulk(records);
        }
        /// <summary>
        /// Generate new <see cref="List{T}"/> containing new habits
        /// </summary>
        /// <returns><see cref="List{T}"/> containing new habits</returns>
        private List<Habit> GenerateHabitsForAutoFill()
        {
            return new List<Habit>(){
                new Habit(){Id = 1,Name="Running", Unit="Km"},
                new Habit(){Id = 2,Name="Learning", Unit="h"},
                new Habit(){Id = 3,Name="Drinking", Unit="litre" }
            };
        }
        /// <summary>
        /// Generate new <see cref="List{T}"/> containing new record for provided Habit
        /// </summary>
        /// <param name="habit"><see cref="Habit"/> for which records should be generated</param>
        /// <returns><see cref="List{T}"/> containing new records</returns>
        private List<HabitRecord> GenerateRecordsForAutoFill(Habit habit)
        {
            List<HabitRecord> records = new List<HabitRecord>();

            Random random = new Random();
            DateTime start = new DateTime(2024, 1, 1);
            
            for (int i = 0; i < 300; i++)
            {
                records.Add(new HabitRecord { Date = start, Volume = random.Next(100), habitId=habit.Id});
                start = start.AddHours(random.Next(24));
            }
            return records;
        }

    }
}
