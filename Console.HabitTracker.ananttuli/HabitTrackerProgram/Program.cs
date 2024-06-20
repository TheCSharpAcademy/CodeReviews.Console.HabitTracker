namespace HabitTrackerProgram
{
    class Program
    {
        public static void Main()
        {
            Init();

            bool keepRunningProgram = true;

            while (keepRunningProgram == true)
            {
                keepRunningProgram = RunApplication();
            }
        }

        static void Init()
        {
            Database.Connection.Init();
            ShowWelcomeScreen();
        }

        static bool RunApplication()
        {
            bool isProgramRunning = true;
            string? input = DisplayMenu();
            string exitMessage = "\n\tThanks for using habit tracker. Keep up the good work! Exiting...";

            switch (input)
            {
                case "0":
                    Console.WriteLine(exitMessage);
                    return false;
                case "1":
                    Model.HabitLog.CreateHabitLog();

                    break;
                case "2":
                    Model.HabitLog.ViewHabitLogs();

                    break;
                case "3":
                    Model.HabitLog.UpdateHabitLog();

                    break;
                case "4":
                    Model.HabitLog.DeleteHabitLog();

                    break;
                default:
                    Console.WriteLine("\nPlease enter a valid option.");
                    break;
            }

            Console.WriteLine("\n\n\tEnter 0 to exit or any other key to continue...");
            var continueInput = Console.ReadKey();

            Console.Clear();

            if (continueInput.KeyChar.Equals('0'))
            {
                Console.WriteLine(exitMessage);
                return false;
            }

            return isProgramRunning;
        }

        static void ShowWelcomeScreen()
        {
            Console.WriteLine($@"
            ::::::::::::::::::::::::::::::        
            :::::    HABIT TRACKER   :::::
            ::::::::::::::::::::::::::::::

            Welcome to Habit tracker!");
        }

        static string? DisplayMenu()
        {
            Console.Write($@"
                MAIN MENU

                0   -   Exit
                1   -   Create habit log
                2   -   View habit logs
                3   -   Update habit log
                4   -   Delete habit log

                Please select an option: ");

            return Console.ReadLine();
        }
    }
}