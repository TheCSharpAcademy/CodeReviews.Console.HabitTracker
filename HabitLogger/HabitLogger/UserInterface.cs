
namespace HabitLogger
    
{
    internal class UserInterface
    {
        readonly DatabaseHelper databaseHelper = new();
        public void Start()
        {
            bool AppOn = true;

            while(AppOn)
            {
                Console.WriteLine("MAIN MENU\n");
                Console.WriteLine("What would you like to do?\n");  
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("Type 1 to Insert New Habit");
                Console.WriteLine("Type 2 to Update Existing Habit");
                Console.WriteLine("Type 3 to View All Habits");
                Console.WriteLine("Type 4 to Delete All Records");

                string input = Console.ReadLine() ?? "Unknown input";
                try
                {
                    Int32.Parse(input);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Unrecognized Input");
                }
                
                switch (input)
                {
                    case "0":
                        AppOn = false;
                        break;
                    case "1":
                        InsertNewHabit();
                        break;
                    case "2":
                        SelectHabit();
                        break;
                    case "3":
                        databaseHelper.ShowValues();
                        break;
                    case "4":
                        Console.WriteLine("Are you sure you want to Delete ALL Records? y/n");
                        string ans = Console.ReadLine() ?? "Unknown input";
                        if (ans == "y" || ans == "yes")
                        {
                            databaseHelper.DeleteAllRecords();
                        }
                        break;
                }
            }         
        }
        public void SelectHabit()
        {
            bool isEmpty = false;
            isEmpty = databaseHelper.ShowValues();
            if (isEmpty)
            {
                Console.WriteLine("Database is EMPTY.\n");
                return;
            }

            Console.WriteLine("\n");
            Console.WriteLine("---------------------");
            Console.WriteLine("Please Select a Habit by typing Habit ID");
            Console.WriteLine("\n");
            int HabitID = Int32.Parse(Console.ReadLine() ?? "Unknown input");
            Console.WriteLine("\n");
            if (databaseHelper.CheckIfHabitExists(HabitID))
            {
                UpdateExistingHabit(HabitID);
            }
            else
            {
                return;
            }   
        }

        public void UpdateExistingHabit(int ID)
        {      
            Console.WriteLine("\n");
            Console.WriteLine("Type 0 to Return to Main Menu");
            Console.WriteLine("Type 1 to Delete Record");
            Console.WriteLine("Type 2 to Update Record");

            string inp = Console.ReadLine() ?? "Unknown input";
            try
            {
                Int32.Parse(inp);
            }
            catch (FormatException)
            {
                Console.WriteLine("Unrecognized Input");
            }
            
            switch(inp)
            {
                case "0":
                    return;
                case "1":
                    databaseHelper.DeleteRecord(ID);
                    break;
                case "2":
                    Console.WriteLine("How many units you want to add?");
                    string ans = "";
                    if (Helpers.IsNotNull(ans))
                    {
                        do
                        {
                            ans = Console.ReadLine() ?? "Unknow input";
                            if (Helpers.IsNotNull(ans))
                            {
                                if (!Helpers.IsNumeric(ans) || Convert.ToDouble(ans) < 0)
                                {
                                    Console.WriteLine("Invalid input. Unit must be positive numeric");
                                }
                            }
                        } while (!Helpers.IsNumeric(ans) || Convert.ToDouble(ans) < 0);
                    }
                    databaseHelper.UpdateRecord(ID, Convert.ToDouble(ans));
                    break;
                default:
                    break;
            }
        }
       
        public void InsertNewHabit()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Please type what habit would you like to track.");
            
            string InputHabit = string.Empty;
            if (Helpers.IsNotNull(InputHabit))
            {
                do
                {
                    InputHabit = Console.ReadLine()?? "Unknow input";
                    if (Helpers.IsNotNull(InputHabit))
                    {
                        if (InputHabit.Length < 8 || !Helpers.IsString(InputHabit))
                        {
                            Console.WriteLine("Invalid input. Habit length must be at least 8 alphabetic characters.");
                        }
                    }
                } while (InputHabit.Length < 8 || !Helpers.IsString(InputHabit));
            }

            Console.WriteLine("Please type Unit of measurement of the habit:");
            string UnitOfMeasurement = String.Empty;

            if (Helpers.IsNotNull(UnitOfMeasurement))
            {
                do
                {
                    UnitOfMeasurement = Console.ReadLine() ?? "Unknow input";
                    if (Helpers.IsNotNull(UnitOfMeasurement))
                    {
                        if (UnitOfMeasurement.Length < 3 || !Helpers.IsString(UnitOfMeasurement))
                        {
                            Console.WriteLine("Invalid input. Unit of measurement length must be at least 3 alphabetic characters.");
                        }
                    } 
                } while (UnitOfMeasurement.Length < 3 || !Helpers.IsString(UnitOfMeasurement));
            }
            
            Console.WriteLine("Please type how many units you want to log?");
            Console.WriteLine("If no units to log now, please type 0");

            string UnitsToLog = string.Empty;
            if (Helpers.IsNotNull(UnitsToLog))
            {
                do
                {
                    UnitsToLog = Console.ReadLine() ?? "Unknow input";
                    if (Helpers.IsNotNull(UnitsToLog))
                    {
                        if (!Helpers.IsNumeric(UnitsToLog) || Convert.ToDouble(UnitsToLog) < 0)
                        {
                            Console.WriteLine("Invalid input. Unit must be positive numeric");
                        }
                    }
                } while(!Helpers.IsNumeric(UnitsToLog) ||Convert.ToDouble(UnitsToLog) < 0 );
                
            }
            databaseHelper.InsertHabit(InputHabit, UnitOfMeasurement, Convert.ToDouble(UnitsToLog));
        }   
    }
}
