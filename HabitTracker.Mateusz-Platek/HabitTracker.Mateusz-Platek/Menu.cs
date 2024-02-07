namespace HabitTracker.Mateusz_Platek;

public static class Menu
{
    public static void Start()
    {
        bool end = false;
        while (!end)
        {
            Console.WriteLine("Choose option:");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Manage logs");
            Console.WriteLine("2 - Manage habits");
            Console.WriteLine("3 - Manage units");

            string? option = Console.ReadLine();
            switch (option)
            {
                case "0":
                    end = true;
                    break;
                case "1":
                    Logs();
                    break;
                case "2":
                    Habits();
                    break;
                case "3":
                    Units();
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
        }
    }

    private static void Logs()
    {
        bool end = false;
        while (!end)
        {
            Console.WriteLine("Choose option:");
            Console.WriteLine("0 - Move back");
            Console.WriteLine("1 - View logs");
            Console.WriteLine("2 - View logs - filter by date");
            Console.WriteLine("3 - Add log");
            Console.WriteLine("4 - Update log");
            Console.WriteLine("5 - Delete log");

            string? option = Console.ReadLine();
            switch (option)
            {
                case "0":
                    end = true;
                    break;
                case "1":
                    DatabaseManager.GetHabitLogs();
                    break;
                case "2":
                    DatabaseManager.GetHabitLogsByDate();
                    break;
                case "3":
                    DatabaseManager.AddHabitLog();
                    break;
                case "4":
                    DatabaseManager.UpdateHabitLog();
                    break;
                case "5":
                    DatabaseManager.DeleteHabitLog();
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
        }
    }
    
    private static void Habits()
    {
        bool end = false;
        while (!end)
        {
            Console.WriteLine("Choose option:");
            Console.WriteLine("0 - Move back");
            Console.WriteLine("1 - View habits");
            Console.WriteLine("2 - Add habit");
            Console.WriteLine("3 - Update habit");
            Console.WriteLine("4 - Delete habit");

            string? option = Console.ReadLine();
            switch (option)
            {
                case "0":
                    end = true;
                    break;
                case "1":
                    DatabaseManager.GetHabits();
                    break;
                case "2":
                    DatabaseManager.AddHabit();
                    break;
                case "3":
                    DatabaseManager.UpdateHabit();
                    break;
                case "4":
                    DatabaseManager.DeleteHabit();
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
        }
    }
    
    private static void Units()
    {
        bool end = false;
        while (!end)
        {
            Console.WriteLine("Choose option:");
            Console.WriteLine("0 - Move back");
            Console.WriteLine("1 - View units");
            Console.WriteLine("2 - Add unit");
            Console.WriteLine("3 - Update unit");
            Console.WriteLine("4 - Delete unit");

            string? option = Console.ReadLine();
            switch (option)
            {
                case "0":
                    end = true;
                    break;
                case "1":
                    DatabaseManager.GetUnits();
                    break;
                case "2":
                    DatabaseManager.AddUnit();
                    break;
                case "3":
                    DatabaseManager.UpdateUnit();
                    break;
                case "4":
                    DatabaseManager.DeleteUnit();
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
        }
    }
}