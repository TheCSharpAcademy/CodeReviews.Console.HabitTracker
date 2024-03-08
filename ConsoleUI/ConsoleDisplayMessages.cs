namespace ConsoleUI
{
    public static class ConsoleDisplayMessages
    {
        public static void DisplayAppWelcomeMessage()
        {
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine();
                Console.WriteLine("Welcome to HabitLogger App.");
                Console.WriteLine("The app version is <1.0.0>.");
                Console.WriteLine("This app was built by Chernobyl.");
                Console.WriteLine("Contact info: mfawzy1337@gmail.com");
                Console.WriteLine();
                Console.WriteLine("---------------------------------");
                Console.WriteLine($"Apllication Starts in {i} sec.");
                Thread.Sleep(1000);
                Console.Clear();
            }


        }
        public static void DisplayMainMenu()
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("-------------MAIN MENU-----------");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("[0] - Exit");
            Console.WriteLine("[1] - View All Habits.");
            Console.WriteLine("[2] - Insert Habit.");
            Console.WriteLine("[3] - Delete Habit.");
            Console.WriteLine("[4] - Update Habit.");
            Console.WriteLine("---------------------------------");
        }

        public static void TerminateAppMessage()
        {
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine();
                Console.WriteLine("Keep Doing looking Forward.");
                Console.WriteLine("Good Habits Such as Reading is Amazing Habit.");
                Console.WriteLine("If Youre Smoking Try To Reduce it till zero smoke.");
                Console.WriteLine("Going to miss you,");
                Console.WriteLine();
                Console.WriteLine("---------------------------------");
                Console.WriteLine($"Apllication Terminates in {i} sec.");
                Thread.Sleep(1000);
                Console.Clear();
            }
            Environment.Exit(0);

        }

    }
}
