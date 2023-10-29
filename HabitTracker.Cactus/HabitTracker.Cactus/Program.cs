using System.Globalization;

public class HabitTracker
{
    // Store water habit records in the memory to avoid frequent database queries.
    private static List<WaterHabit>? waterHabitsCache;
    static int Main(string[] args)
    {
        WaterHabitHelpers.CreateWaterHabitTableIfNotExist();
        waterHabitsCache = WaterHabitHelpers.SeleteAll(); // init waterHabit cache
        if (waterHabitsCache == null) waterHabitsCache = new List<WaterHabit>();
        bool endApp = false;

        while (!endApp)
        {
            Console.Clear();
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Main Menu");
            Console.WriteLine("Type 0, exit app.");
            Console.WriteLine("Type 1, insert a water drinking habit record.");
            Console.WriteLine("Type 2, show all water drinking habit records.");
            Console.WriteLine("Type 3, update specific water drinking habit record.");
            Console.WriteLine("Type 4, delete specific water drinking habit record.");
            Console.WriteLine("---------------------------------------------------------");
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
                case "4":
                    DeleteSpecificWaterHabitRecord();
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

    public static void DeleteSpecificWaterHabitRecord()
    {
        Console.Clear();
        Console.WriteLine("DELETE MENU");
        ShowAllWaterHabitRecords();
        if (waterHabitsCache == null || waterHabitsCache.Count == 0) return;
        HashSet<int> ids = waterHabitsCache.Select(x => x.Id).ToHashSet<int>();
        int id = InputUtils.GetInValidInputId(ids);
        WaterHabitHelpers.Delete(id);
        waterHabitsCache = WaterHabitHelpers.SeleteAll(); // Update the cache after deleting a record
    }

    public static void UpdateSpecificWaterHabitRecord()
    {
        Console.Clear();
        Console.WriteLine("UPDATE MENU");
        ShowAllWaterHabitRecords();
        if (waterHabitsCache == null || waterHabitsCache.Count == 0) return;
        HashSet<int> ids = waterHabitsCache.Select(x => x.Id).ToHashSet<int>();
        int id = InputUtils.GetInValidInputId(ids);
        DateTime date = InputUtils.GetValidInputDate();
        int quantity = InputUtils.GetValidInputQuantity();
        WaterHabit waterHabit = waterHabitsCache.Where(habit => habit.Id == id).ToList()[0];
        waterHabit.Date = date;
        waterHabit.Quantity = quantity;
        WaterHabitHelpers.Update(waterHabit);
    }

    public static void InsertWaterHabitRecord()
    {
        Console.Clear();
        Console.WriteLine("INSERT MENU");
        DateTime date = InputUtils.GetValidInputDate();
        int quantity = InputUtils.GetValidInputQuantity();
        WaterHabit habit = new();
        habit.Date = date;
        habit.Quantity = quantity;
        habit.Id = WaterHabitHelpers.Insert(habit);
        if (habit.Id != -1)
        {
            if (waterHabitsCache == null) waterHabitsCache = new List<WaterHabit>();
            waterHabitsCache.Add(habit);
        }
    }

    public static void ShowAllWaterHabitRecords()
    {
        Console.Clear();
        if (waterHabitsCache == null || waterHabitsCache.Count == 0)
        {
            Console.WriteLine("There is no water habit record.");
            return;
        }
        Console.WriteLine("======================Habit Records======================");
        waterHabitsCache.ForEach(waterHab => Console.WriteLine($"{waterHab.Id}: {waterHab.Date.ToString("dd-MM-yyyy")} {waterHab.Quantity}"));
        Console.WriteLine("=========================================================");
    }
}