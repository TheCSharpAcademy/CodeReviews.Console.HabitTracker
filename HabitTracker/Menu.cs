namespace HabitTracker
{
    public class Menu
    {
        public static void ShowMenu()
        {
            int option = 1;
            ConsoleKeyInfo key;
            bool isSelected = false;
            string color = "\u001b[32m";

            while (!isSelected)
            {
                Console.WriteLine("\nPlease, choose an option from below: ");
                Console.WriteLine($"{(option == 1 ? color : "    ")}Current Habits\u001b[0m");
                Console.WriteLine($"{(option == 2 ? color : "    ")}Create a Habit\u001b[0m");
                Console.WriteLine($"{(option == 3 ? color : "    ")}Show all Habit Records\u001b[0m");
                Console.WriteLine($"{(option == 4 ? color : "    ")}Create a Record in a Habit\u001b[0m");
                Console.WriteLine($"{(option == 5 ? color : "    ")}Edit a Habit\u001b[0m");
                Console.WriteLine($"{(option == 6 ? color : "    ")}Remove a Habit\u001b[0m");
                Console.WriteLine($"{(option == 7 ? color : "    ")}Export Report\u001b[0m");
                Console.WriteLine($"{(option == 8 ? color : "    ")}Credits\u001b[0m");

                key = Console.ReadKey();
                Console.Clear();
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (option == 8) break;
                        option++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (option == 1) break;
                        option--;
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        SelectMenuItem(option);
                        break;
                }
            }

        }

        public static void SelectMenuItem(int option)
        {
            switch (option)
            {
                case 1:
                    Habit.ShowAll("Habits");
                    break;
                case 2:
                    Habit.CreateHabit();
                    break;
                case 3:
                    Habit.ShowAll("Records");
                    break;
                case 4:
                    Habit.AddHabitRecord();
                    break;
                case 5:
                    Habit.UpdateHabit();
                    break;
                case 6:
                    Habit.RemoveHabit();
                    break;
                case 7:
                    Report.GetReport();
                    break;
                case 8:
                    Credits.GetCredits();
                    break;
            }
            ShowMenu();
        }
    }


}
