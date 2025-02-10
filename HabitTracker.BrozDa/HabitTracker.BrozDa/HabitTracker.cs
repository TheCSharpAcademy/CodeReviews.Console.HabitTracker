namespace HabitTracker.BrozDa
{
    internal class HabitTracker
    {
        private readonly string _dateFormat = "dd-MM-yyyy";
        private DatabaseManager _databaseManager;
        private InputOutputManager _inputOutputManager;
       
        public HabitTracker()
        {
            _databaseManager = new DatabaseManager(_dateFormat);
            _inputOutputManager = new InputOutputManager(_dateFormat); 
        }
        public void Start()
        {
            if (!_databaseManager.DoesDatabaseExist()) 
            {
                _databaseManager.CreateNewDatabase();
            }
            //Used to autopopulate records for debugging and testing
            AutoSeed();

            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                _inputOutputManager.PrintMainMenu();
                ProcessMainMenu();
            }
        }
        
        
        private void ProcessMainMenu()
        {
            int input = _inputOutputManager.GetInputInMenu(_inputOutputManager.MainMenuLength);
            List<string> listOfTables = _databaseManager.GetListOfTablesFromDatabase();

            _inputOutputManager.PrintTables(listOfTables);
            switch (input) 
            {
                case 1: //Check tracked habits
                    //_inputOutputManager.PrintTables(listOfTables);
                    
                    break;
                case 2: //Manage existing habit
                    
                    string table = _inputOutputManager.GetTableNameFromUser(listOfTables);
                    if(table != string.Empty)
                    {
                        ProcessHabitMenu(table);
                    }
                    break;
                case 3: //Create new habit
                    string newTableName = _inputOutputManager.GetNewTableName(listOfTables);
                    string newTableUnit = _inputOutputManager.GetNewTableUnit();
                    _databaseManager.CreateNewTable(newTableName, newTableUnit);
                    break;
                case 4:
                    string tableToBeDeleted = _inputOutputManager.GetExistingTableName(listOfTables);
                    if(tableToBeDeleted != "0")
                        _databaseManager.DeleteTable(tableToBeDeleted);
                    break;
                case 5: //Exit the app
                    Environment.Exit(0);
                    break;

            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);

        }

        private void ProcessHabitMenu(string table) 
        {
            
            bool exitToMainMenu = false;
            List<DatabaseRecord> records;
            string unit;


            while (!exitToMainMenu) 
            {
                _inputOutputManager.PrintHabitMenu();
                int input = _inputOutputManager.GetInputInMenu(_inputOutputManager.HabitMenuLength);
                records = _databaseManager.GetRecordsFromTable(table);
                unit = _databaseManager.GetFromUnitTable(table);
                int recordID;
                switch (input)
                {
                    case 1: //print records
                        _inputOutputManager.PrintTable(table, records, unit);
                        //_databaseManager.PrintRecordsFromATable(table);
                        break;
                    case 2: //new record
                        DatabaseRecord newRecord = _inputOutputManager.GetValuesForNewRecord();
                        _databaseManager.InsertRecord(table,newRecord);
                        break;
                    case 3: //update record
                        _databaseManager.PrintRecordsFromATable(table);
                        recordID = _inputOutputManager.GetRecordIdFromUser(records, "update");
                        DatabaseRecord updatedRecord = _inputOutputManager.GetValuesForNewRecord(recordID);
                        _databaseManager.UpdateRecord(table, updatedRecord);
                        Console.WriteLine("Updating record ...");
                        Console.WriteLine("Record updated");
                        break;
                    case 4: //delete record
                        _databaseManager.PrintRecordsFromATable(table);
                        recordID = _inputOutputManager.GetRecordIdFromUser(records, "delete");
                        _databaseManager.DeleteRecord(table, recordID);
                        Console.WriteLine("Deleting record ...");
                        Console.WriteLine("Record deleted ...");
                        break;
                    case 5:
                        ReportUnit report = CreateReport(table,unit, records);
                        Console.WriteLine(report.ToString());
                        break;
                    case 6:
                        exitToMainMenu = true;
                        break;
                    case 7:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
                if (!exitToMainMenu)
                {
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadKey(true);
                }  
            }
            
        }

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
                _databaseManager.CreateNewTable(tables[i], units[i]);

                for (int j = 0; j < 100; j++) {
                    record = new DatabaseRecord();
                    record.Date = start.AddDays(random.Next(range));
                    record.Volume = random.Next(200);
                    _databaseManager.InsertRecord(tables[i], record);
                    
                }
            }
       }
        

    }
    
}
