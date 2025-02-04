namespace HabitTracker.BrozDa
{
    internal class HabitTracker
    {
        private DatabaseManager _databaseManager;
        private DisplayManager _displayManager;
        public HabitTracker()
        {
            _databaseManager = new DatabaseManager();
            _displayManager = new DisplayManager();
        }


        public void Start()
        {
            PrintMainMenu();
            ProcessUserInput();
        }
        public void PrintMainMenu()
        {
            Console.WriteLine("Welcome to habit tracker application");
            Console.WriteLine("Habit tracker tracks number of glasses of water drank during the day");
            Console.WriteLine();
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. View all records");
            Console.WriteLine("\t2. Insert record");
            Console.WriteLine("\t3. Update record");
            Console.WriteLine("\t4. Delete record");
            Console.WriteLine("\t5. Close the application");

            Console.Write("Your selection: ");

        }
        private int GetUserInput()
        {
            string? input = Console.ReadLine();
            int numericInput;

            while (!int.TryParse(input, out numericInput) || numericInput < 1 || numericInput > 5)
            {
                Console.Write("Please enter valid value: ");
                input = Console.ReadLine();
            }
           
            return numericInput;
        }
        private void ProcessUserInput() {
            int input = GetUserInput();

            switch (input)
            {
                case 1:
                    _displayManager.ShowRecords(_databaseManager.GetTableColumnNames("WaterIntake"), _databaseManager.GetTableRecords("WaterIntake"));
                    //_databaseManager.GetTableRecords("WaterIntake"); //ONLY SINGLE TABLE IMPLEMENTATION ATM
                    break;
                case 2: break;
                case 3: break;
                case 4: break;
                case 5: break;

                default:
                    break;
            }
        }
    }
    
}
