namespace HabitTracker
{
    public class Menu
    {
        public static List<string> habits = new List<string>();
        
        public static string HabitMenu()
        {
            string? result;
            do
            {
            Console.Clear();
            Console.WriteLine("Would you like to add a new habit or use an existing one?");
            Console.WriteLine("1. Create new habit");
            Console.WriteLine("2. Use an existing one");
            Console.WriteLine("3. Delete an existing one");
            result = Console.ReadLine();
            return result;
            } while (!int.TryParse(result, out _) || string.IsNullOrEmpty(result));
        }

        public static void SwitchMenu()
        {
            string result = HabitMenu();
            switch (result)
            {
                case "1":
                    Habits.CreateHabit(habits);
                    break;
                case "2":
                    GetUserInput();
                    break;
                case "3":
                    Habits.DeleteHabit(habits);
                    break;
                default:
                    Console.WriteLine("Not an option. Press enter to try again.");
                    Console.ReadLine();
                    SwitchMenu();
                    break;

            }
        }

        public static void GetUserInput()
        {
            string habit = Habits.GetInputHabit(habits);
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine("--------------------");
                Console.WriteLine("0. Close Application");
                Console.WriteLine("1. View Records");
                Console.WriteLine("2. Insert Record");
                Console.WriteLine("3. Delete Record");
                Console.WriteLine("4. Update Record");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        Console.ReadLine();
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        CRUD.GetAllRecords();
                        break;
                    case "2":
                        CRUD.Insert(habit);
                        break;
                    case "3":
                        CRUD.Delete(habit);
                        break;
                    case "4":
                        CRUD.Update(habit);
                        break;
                    default:
                        Console.WriteLine("Invalid Command. Please type a number from 0 to 4.");
                        break;
                }
            }
        }


    }
}