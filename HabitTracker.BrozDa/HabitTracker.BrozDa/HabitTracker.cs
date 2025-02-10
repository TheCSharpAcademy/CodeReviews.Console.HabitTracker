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


            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                _inputOutputManager.PrintMainMenu();
                ProcessMainMenu();
            }
                

            /*bool exists = _databaseManager.CheckIfTableExists("WaterIntake");
            Console.WriteLine(exists);
            Console.ReadLine();*/
            //_databaseManager.CreateNewTable("WaterIntake");

            /*while (true) 
            {
                
                _InputOutputManager.PrintHabitMenu();
                ProcessUserInput();
            }*/
            
        }
        
        
        private void ProcessMainMenu()
        {
            int input = _inputOutputManager.GetInputInMenu(_inputOutputManager.MainMenuLength);
            List<string> listOfTables = _databaseManager.GetListOfTablesFromDatabase();

            switch (input) 
            {
                case 1:
                    _inputOutputManager.PrintTables(listOfTables);
                    break;
                case 2:
                    _inputOutputManager.PrintTables(listOfTables);
                    string table = _inputOutputManager.GetTableNameFromUser(listOfTables);
                    
                    ProcessHabitMenu(table);
                    break;
                case 3: 
                    _inputOutputManager.PrintTables(listOfTables);
                    string newTableName = _inputOutputManager.GetNewTableName();
                    string newTableUnit = _inputOutputManager.GetNewTableUnit();
                    _databaseManager.CreateNewTable(newTableName, newTableUnit);
                    break;
                case 4: 
                    Environment.Exit(0);
                    break;

            }
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey(true);
        }

        private void ProcessHabitMenu(string table) 
        {
            
            bool exitToMainMenu = false;

           

            while (!exitToMainMenu) 
            {
                _inputOutputManager.PrintHabitMenu();
                int input = _inputOutputManager.GetInputInMenu(_inputOutputManager.HabitMenuLength);
                int recordID;
                switch (input)
                {
                    case 1: //print records
                        _databaseManager.PrintRecordsFromATable(table);
                        break;
                    case 2: //new record
                        DatabaseRecord newRecord = _inputOutputManager.GetNewRecord(table);
                        _databaseManager.InsertRecord(table,newRecord);
                        break;
                    case 3: //update record
                        _databaseManager.PrintRecordsFromATable(table);
                        recordID = GetValidRecordID(table, "update");
                        DatabaseRecord oldRecord = _databaseManager.GetRecordUsingID(table, recordID);
                        DatabaseRecord updatedRecord = _inputOutputManager.GetNewValuesForRecord(oldRecord);
                        _databaseManager.UpdateRecord(table, updatedRecord);
                        break;
                    case 4: //delete record
                        _databaseManager.PrintRecordsFromATable(table);
                        recordID = GetValidRecordID(table, "delete");
                        _databaseManager.DeleteRecord(table, recordID);
                        break;
                    case 5:
                        exitToMainMenu = true;
                        break;
                    case 6:
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
       public int GetValidRecordID(string table, string operation)
        {
            int recordID = _inputOutputManager.GetRecordIdFromUser(operation);
            while (!_databaseManager.IsIdPresentInDatabase(table,recordID))
            {
                Console.WriteLine("Entered ID is not present in the database");
                recordID = _inputOutputManager.GetRecordIdFromUser("update");
            }
            return recordID;

        }
    }
    
}
