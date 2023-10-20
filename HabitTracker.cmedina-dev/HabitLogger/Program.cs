namespace HabitLogger
{
    using HabitHandler;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to the Habit Logger!");

            string input = "";
            while (input != "0")
            {
                MainMenu();

                input = Console.ReadLine();

                switch (input)
                {
                    case "n":
                        HabitHandler.CreateHabit();
                        break;
                    case "u":
                        HabitHandler.UpdateHabit();
                        break;
                    case "d":
                        HabitHandler.DeleteHabitDB();
                        break;
                    case "v":
                        HabitHandler.ViewHabit();
                        break;
                }
            }
        }

        private static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("--------------------");
            Console.WriteLine("n: New Habit");
            Console.WriteLine("u: Update Habit");
            Console.WriteLine("d: Delete Habit");
            Console.WriteLine("v: View Habit");
            Console.WriteLine("0: Exit Application");
            Console.WriteLine("--------------------");
            Console.Write("\n\nEnter an option: ");
        }
    }
}
