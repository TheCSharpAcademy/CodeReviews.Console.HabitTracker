using System.Globalization;

public class HabitTracker
{
    // Store water habit records in the memory to avoid frequent database queries.
    private static List<WaterHabit> waterHabits;
    static int Main(string[] args)
    {
        WaterHabitHelpers.CreateWaterHabitTableIfNotExist();
        bool endApp = false;

        while (!endApp)
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Main Menu");
            Console.WriteLine("Type 0, exit app.");
            Console.WriteLine("Type 1, insert a water drinking habit record.");
            Console.WriteLine("Type 2, show all water drinking habit records.");
            Console.WriteLine("Type 3, update specific water drinking habit record.");
            Console.WriteLine("-----------------------------------------");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    InsertWaterHabitRecord();
                    break;
                case "2":
                    ShowAllWaterHabitRecords();
                    break;
                case "3":
                    UpdateSpecificWaterHabitRecord();
                    break;
                default:
                    break;
            }
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;
            Console.WriteLine("\n");
        }
        return 0;
    }

    public static void UpdateSpecificWaterHabitRecord()
    {
        Console.Clear();
        ShowAllWaterHabitRecords();
        int cnt = waterHabits.Count;
        Console.WriteLine($"Please type a id(1-{cnt}) you want to update:");
        string? idStr = Console.ReadLine();
        int id = -1;
        while (!int.TryParse(idStr, out id))
        {
            Console.WriteLine($"Sorry, your id is invalid. Please type a valid id(1-{cnt}):");
            idStr = Console.ReadLine();
        }
        DateTime date;
        int quantity;
        Console.WriteLine("Please type your date(dd-MM-yyyy):");
        string? dateStr = Console.ReadLine();
        while (!DateTime.TryParseExact(dateStr, "dd-MM-yyyy", new CultureInfo("en-US"),
                               DateTimeStyles.None, out date))
        {
            Console.WriteLine("Sorry, your date is invalid. Please type a valid date(dd-MM-yyyy):");
            dateStr = Console.ReadLine();
        }
        Console.WriteLine("Please type your quantity:");
        string? quantityStr = Console.ReadLine();
        while (!int.TryParse(quantityStr, out quantity))
        {
            Console.WriteLine("Sorry, your quantity is invalid. Please type a valid quantity:");
            quantityStr = Console.ReadLine();
        }
        WaterHabit waterHabit = waterHabits[id - 1];
        waterHabit.Date = date;
        waterHabit.Quantity = quantity;
        WaterHabitHelpers.Update(waterHabit);
    }

    public static void InsertWaterHabitRecord()
    {
        Console.Clear();
        DateTime date;
        int quantity;
        Console.WriteLine("Please type your date(dd-MM-yyyy):");
        string? dateStr = Console.ReadLine();
        while (!DateTime.TryParseExact(dateStr, "dd-MM-yyyy", new CultureInfo("en-US"),
                               DateTimeStyles.None, out date))
        {
            Console.WriteLine("Sorry, your date is invalid. Please type a valid date(dd-MM-yyyy):");
            dateStr = Console.ReadLine();
        }
        Console.WriteLine("Please type your quantity:");
        string? quantityStr = Console.ReadLine();
        while (!int.TryParse(quantityStr, out quantity))
        {
            Console.WriteLine("Sorry, your quantity is invalid. Please type a valid quantity:");
            quantityStr = Console.ReadLine();
        }
        WaterHabit habit = new();
        habit.Date = date;
        habit.Quantity = quantity;
        habit.Id = WaterHabitHelpers.Insert(habit);
        if (habit.Id != -1) waterHabits.Add(habit);
    }

    public static void ShowAllWaterHabitRecords()
    {
        Console.Clear();
        if (waterHabits == null)
        {
            waterHabits = WaterHabitHelpers.SeleteAll();
        }
        if (waterHabits == null)
        {
            Console.WriteLine("There is no water habit record.");
            return;
        }
        Console.WriteLine("========================================");
        waterHabits.ForEach(waterHab => Console.WriteLine($"{waterHab.Id}: {waterHab.Date.ToString("dd-MM-yyyy")} {waterHab.Quantity}"));
        Console.WriteLine("========================================");
    }
}