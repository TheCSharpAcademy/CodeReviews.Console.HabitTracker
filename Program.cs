namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseManager.Initialize();
            while (true)
            {
                ShowMainMenu();
            }
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Habits Manager!");
            Console.WriteLine("0. To exit.");
            Console.WriteLine("1. List Ocurrences");
            Console.WriteLine("2. Insert Ocurrence");
            Console.WriteLine("3. Delete Ocurrence");
            Console.WriteLine("4. Update Ocurrence");
            Console.Write("Please choose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    ListAllHabits();
                    PressAnyKeyToReturn();
                    break;
                case "2":
                    InsertHabitsAsk();
                    PressAnyKeyToReturn();
                    break;
                case "3":
                    DestroyHabit();
                    PressAnyKeyToReturn();
                    break;
                default:
                    Console.WriteLine("Opção inválida, tente novamente.");
                    PressAnyKeyToReturn();
                    break;
            }
        }

        static void PressAnyKeyToReturn()
        {
            Console.WriteLine("\nPress any key to back to the main menu...");
            Console.ReadKey();
        }

        static void InsertHabitsAsk()
        {
            Console.Clear();
            Console.WriteLine("Let's start with your new Habit!\n");

            Console.Write("What's the name of your new Habit?: ");
            string habitName = Console.ReadLine();

            Console.Write("How many times did you do this habit?: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid quantity. Please enter a number.");
                return;
            }

            Console.Write("Enter the date (YYYY-MM-DD): ");
            string date = Console.ReadLine();

            bool success = DatabaseManager.InsertHabits(habitName, quantity, date);

            if (success)
                Console.WriteLine("Habit inserted successfully!");
            else
                Console.WriteLine("Failed to insert habit.");
        }

        static void ListAllHabits()
        {
            Console.Clear();
            List<string> habits = DatabaseManager.ListHabits();

            if (habits.Count == 0)
            {
                Console.WriteLine("No habits found.");
            }
            else
            {
                foreach (string habit in habits)
                {
                    Console.WriteLine(habit);
                }
            }
        }
        }
    }
}