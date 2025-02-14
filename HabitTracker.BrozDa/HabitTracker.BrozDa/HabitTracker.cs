namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Enums representing options in main menu used to clearer distinction in switch statements 
    /// </summary>
    enum MainMenuOptions
    {
        CheckTrackedHabits = 1,
        ManageHabits = 2,
        CreateHabit = 3,
        DeleteHabit = 4,
        ExitApplication = 5,
    }
    /// <summary>
    /// Enums representing options in habit menu used to clearer distinction in switch statements 
    /// </summary>
    enum HabitMenuOptions
    {
        ViewRecords = 1,
        InsertRecord = 2,
        UpdateRecord = 3,
        DeleteRecord = 4,
        CreateReport = 5,
        ExitToMainMenu = 6,
        ExitApplication = 7,
    }
    /// <summary>
    /// Represent class which utilizes DB reader, DB writer, Input manager, output manager to allow user to adjust database records 
    /// </summary>
    internal class HabitTracker
    {
        private DatabaseReader _databaseReader;
        private DatabaseWriter _databaseWriter;
        private OutputManager _outputManager;
        private InputManager _inputManager;
        /// <summary>
        /// Initializes new instance of <see cref="HabitTracker"/> class
        /// </summary>
        /// <param name="reader">Represents object of <see cref="DatabaseReader"/> class</param>
        /// <param name="writer">Represents object of <see cref="DatabaseWriter"/> class</param>
        /// <param name="inputManager">Represents object of <see cref="InputManager"/> class</param>
        /// <param name="outputManager">Represents object of <see cref="OutputManager"/> class</param>
        public HabitTracker(DatabaseReader reader, DatabaseWriter writer, InputManager inputManager, OutputManager outputManager)
        {
            _databaseReader = reader;
            _databaseWriter = writer;
            _outputManager = outputManager;
            _inputManager = inputManager;
        }
        /// <summary>
        /// Starts the loop used to CRUD operations on the database, loop is ongoing until user exists the application via coresponding option
        /// Database is created if it is not present already
        /// </summary>
        public void Start()
        {
            if (!_databaseReader.DoesDatabaseExist()) 
            {
                _databaseWriter.CreateNewDatabase();
            }
            //Used to autopopulate records for debugging and testing
            //AutoSeed();

            while (true)
            {
                _outputManager.PrintMainMenu();
                ProcessMainMenu();
            }
        }
        
        /// <summary>
        /// Prints out main menu, gets user input and perform actions based on the input
        /// </summary>
        private void ProcessMainMenu()
        {
            int input = _inputManager.GetInputInMenu(_outputManager.MainMenuLength);
            List<string> listOfTables = _databaseReader.GetListOfTablesFromDatabase();

            switch ((MainMenuOptions)input) 
            {
                case MainMenuOptions.CheckTrackedHabits:
                    HandleCheckTrackedHabits(listOfTables);
                    break;
                case MainMenuOptions.ManageHabits:
                    HandleManageHabits(listOfTables);
                    break;
                case MainMenuOptions.CreateHabit:
                    HandleCreateHabit(listOfTables);
                    break;
                case MainMenuOptions.DeleteHabit:
                    HandleDeleteHabit(listOfTables);
                    break;
                case MainMenuOptions.ExitApplication: 
                    Environment.Exit(0);
                    break;

            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
        }
        /// <summary>
        /// Prints out tracked habits - names of tables user created in the database
        /// </summary>
        /// <param name="listOfTables">List of <see cref="string"/> containing tabls/param>
        private void HandleCheckTrackedHabits(List<string> listOfTables)
        {
            _outputManager.PrintTablesInDatabase(listOfTables);
        }
        /// <summary>
        /// Prints out tracked habits, moves to selected habit menu
        /// </summary>
        /// <param name="listOfTables">List of <see cref="string"/> containing tabls/param>
        private void HandleManageHabits(List<string> listOfTables)
        {
            _outputManager.PrintTablesInDatabase(listOfTables);
            string table = _inputManager.GetTableNameById(listOfTables);
            if (table != string.Empty)
            {
                ProcessHabitMenu(table);
            }
        }
        /// <summary>
        /// Prints out tracked habits, and takes user input for new habit user wish to track
        /// </summary>
        /// <param name="listOfTables">List of <see cref="string"/> containing tabls/param>
        private void HandleCreateHabit(List<string> listOfTables)
        {
            _outputManager.PrintTablesInDatabase(listOfTables);
            string newTableName = _inputManager.GetNewTableName(listOfTables);
            string newTableUnit = _inputManager.GetNewTableUnit();
            Console.WriteLine("Creating new habit...");
            _databaseWriter.CreateNewTable(newTableName, newTableUnit);
            Console.WriteLine("Creating habit created...");
        }
        /// <summary>
        /// Prints out tracked habits, and takes user input for habit user wish to delete
        /// </summary>
        /// <param name="listOfTables">List of <see cref="string"/> containing tabls/param>
        private void HandleDeleteHabit(List<string> listOfTables)
        {
            _outputManager.PrintTablesInDatabase(listOfTables);
            string tableToBeDeleted = _inputManager.GetExistingTableName(listOfTables);
            if (tableToBeDeleted != "0")
            {
                _databaseWriter.DeleteTable(tableToBeDeleted);
            }
        }
        /// <summary>
        /// Prints out main menu, gets user input and perform actions based on the input
        /// </summary>
        /// <param name="table">Name of the table representing the habit</param>
        private void ProcessHabitMenu(string table) 
        {
            bool exitToMainMenu = false;
            string unit = _databaseReader.GetFromUnitTable(table);
            int input;

            while (!exitToMainMenu) 
            {
                _outputManager.PrintHabitMenu();
                input = _inputManager.GetInputInMenu(_outputManager.HabitMenuLength);
                switch ((HabitMenuOptions)input)
                {
                    case HabitMenuOptions.ViewRecords:
                        HandleViewRecords(table, unit);
                        break;
                    case HabitMenuOptions.InsertRecord:
                        HandleInsertRecord(table);
                        break;
                    case HabitMenuOptions.UpdateRecord:
                        HandleRecordAction(table, unit, "update");
                        break;
                    case HabitMenuOptions.DeleteRecord:
                        HandleRecordAction(table,unit, "delete");
                        break;
                    case HabitMenuOptions.CreateReport:
                        HandleCreateReport(table, unit);
                        break;
                    case HabitMenuOptions.ExitToMainMenu:
                        exitToMainMenu = true;
                        break;
                    case HabitMenuOptions.ExitApplication:
                        Environment.Exit(0);
                        break;
                }
                if (!exitToMainMenu)
                {
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadKey(true);
                }  
            }
        }
        /// <summary>
        /// Prints out record from the table
        /// </summary>
        /// <param name="table"><see cref="string>"/> representing name of the table</param>
        /// <param name="unit"><see cref="string"/> representing unit used for current table</param>
        private void HandleViewRecords(string table, string unit)
        {
            List<DatabaseRecord> records = _databaseReader.GetRecordsFromTable(table);
            _outputManager.PrintRecordsFromTable(table, records, unit);
        }
        /// <summary>
        /// Gets user input for new record and insert it to the database
        /// </summary>
        /// <param name="table"><see cref="string>"/> representing name of the table</param>
        private void HandleInsertRecord(string table)
        {
            DatabaseRecord newRecord = _inputManager.GetValuesForRecord();
            Console.WriteLine("Adding new record...");
            _databaseWriter.InsertRecord(table, newRecord);
            Console.WriteLine("Record added sucessfully");
        }
        /// <summary>
        /// Handles existing record modification action
        /// current supported actions are <see cref="string"/> "update" and <see cref="string"/> "delete"
        /// </summary>
        /// <param name="table"><see cref="string>"/> representing name of the table</param>
        /// <param name="unit"><see cref="string"/> representing unit used for current table</param>
        /// <param name="action"><see cref="string"/> representing action to be performed with existign record</param>
        private void HandleRecordAction(string table, string unit, string action)
        {
            List<DatabaseRecord> records = _databaseReader.GetRecordsFromTable(table);

            if (records == null || records.Count == 0)
            {
                Console.WriteLine("No records available to update.");
                return;
            }

            _outputManager.PrintRecordsFromTable(table, records, unit);
            int recordID = _inputManager.GetRecordIdFromUser(records, action);

            if (recordID == 0) 
            { 
                return;
            }

            if (action == "update")
            {
                DatabaseRecord updatedRecord = _inputManager.GetValuesForRecord(recordID);
                Console.WriteLine("Updating record ...");
                _databaseWriter.UpdateRecord(table, updatedRecord);     
                Console.WriteLine("Record updated");
            }
            if (action == "delete")
            {
                Console.WriteLine("Deleting record ...");
                _databaseWriter.DeleteRecord(table, recordID);
                Console.WriteLine("Record deleted ...");
            }
        }
        /// <summary>
        /// Creates report for specified table and prints it to the console
        /// </summary>
        /// <param name="table"><see cref="string>"/> representing name of the table</param>
        /// <param name="unit"><see cref="string"/> representing unit used for current table</param>
        private void HandleCreateReport(string table, string unit)
        {
            List<DatabaseRecord> records = _databaseReader.GetRecordsFromTable(table);

            ReportUnit report = CreateReport(table, unit, records);
            Console.WriteLine(report.GenerateReport());
        }
        /// <summary>
        /// Creates new report for specified table
        /// </summary>
        /// <param name="table"><see cref="string>"/> representing name of the table</param>
        /// <param name="unit"><see cref="string"/> representing unit used for current table</param>
        /// <param name="records"><see cref="List{String}"/> of records in current table</param>
        /// <returns><see cref="ReportUnit"/> containing values for the report </returns>
        public ReportUnit CreateReport(string table,string unit, List<DatabaseRecord> records)
        {
            int count = records.Count;
            int totalVolume = 0;
            int minVolume = records[0].Volume;
            DateTime minVolumeDate = DateTime.Now;
            int maxVolume = records[0].Volume;
            DateTime maxVolumeDate = DateTime.Now;
            int averageVolume = 1;
            foreach (DatabaseRecord record in records) 
            { 
                totalVolume += record.Volume;

                if(record.Volume > maxVolume)
                {
                    maxVolume = record.Volume;
                    maxVolumeDate = record.Date;
                }

                if (record.Volume < minVolume)
                {
                    minVolume = record.Volume;
                    minVolumeDate = record.Date;
                }
            }
            averageVolume = totalVolume / count;
            return new ReportUnit(table, unit, count, totalVolume, averageVolume, minVolume, minVolumeDate, maxVolume, maxVolumeDate);
        }
        /// <summary>
        /// Automatically fills out Database with tables with records
        /// </summary>
        private void AutoSeed()
        {
            //far from perfect because every iteration opens new connection
            //used only once so did not optimize it 
            Random random = new Random();
            DateTime start = new DateTime(2010, 1, 1);
            int range =(DateTime.Now - start).Days;

            string[] tables = { "Running", "Walking", "Drinking" };
            string[] units = { "km", "h", "glasses" };
            DatabaseRecord record;

            for (int i = 0; i < tables.Length; i++) {
                _databaseWriter.CreateNewTable(tables[i], units[i]);

                for (int j = 0; j < 100; j++) {
                    record = new DatabaseRecord();
                    record.Date = start.AddDays(random.Next(range));
                    record.Volume = random.Next(200);
                    _databaseWriter.InsertRecord(tables[i], record);
                    
                }
            }
        }
    }
    
}
