namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseManager.Initialize();
            while (true)
            {
                ConstructMainMenu();
            }
        }

        static void ConstructMainMenu()
        {
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
                    // Call method to insert person
                    break;
                case "2":
                    // Call method to list people
                    break;
                case "3":
                    // Call method to delete person
                    break;
                default:
                    Console.WriteLine("Opção inválida, tente novamente.");
                    ConstructMainMenu();
                    break;
            }
        }

        void InsertHabitsAsk()
        {
            Console.WriteLine("Let's start with your new Habit!\n");
            
        }
    }
}