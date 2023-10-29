using System.Globalization;

public class HabitTracker
{
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
                default:
                    break;
            }
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;
            Console.WriteLine("\n");
        }
        return 0;
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
        WaterHabitHelpers.Insert(habit);
    }

    public static void ShowAllWaterHabitRecords()
    {
        Console.Clear();
        List<WaterHabit> waterHabits = WaterHabitHelpers.SeleteAll();
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