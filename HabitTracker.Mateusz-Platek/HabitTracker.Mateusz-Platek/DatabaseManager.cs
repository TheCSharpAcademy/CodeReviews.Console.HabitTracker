namespace HabitTracker.Mateusz_Platek;

public static class DatabaseManager
{
    private static DateTime? ConvertToDateTime(string date)
    {
        try
        {
            string[] dateParts = date.Split("-");
            if (dateParts.Length != 3 || dateParts[2].Length != 4 || dateParts[1].Length != 2 || dateParts[0].Length != 2)
            {
                return null;
            }

            return new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[1]), int.Parse(dateParts[0]));
        }
        catch (Exception exception)
        {
            return null;
        }
    }
    
    public static void GetUnits()
    {
        List<Unit> units = Database.GetUnits();
        foreach (Unit unit in units)
        {
            Console.WriteLine(unit);
        }
    }
    
    public static void GetHabits()
    {
        List<Habit> habits = Database.GetHabits();
        foreach (Habit habit in habits)
        {
            Console.WriteLine(habit);
        }
    }

    public static void GetHabitLogs()
    {
        List<HabitLog> habitLogs = Database.GetHabitLogs();
        foreach (HabitLog habitLog in habitLogs)
        {
            Console.WriteLine(habitLog);
        }
    }
    
    public static void GetHabitLogsByDate()
    {
        Console.WriteLine("Insert start date in format dd-MM-yyyy:");
        string? startDate = Console.ReadLine();
        if (startDate == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        DateTime? start = ConvertToDateTime(startDate);
        if (start == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        Console.WriteLine("Insert end date in format dd-MM-yyyy:");
        string? endDate = Console.ReadLine();
        if (endDate == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        DateTime? end = ConvertToDateTime(endDate);
        if (end == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        List<HabitLog> habitLogs = Database.GetHabitLogs();
        habitLogs.RemoveAll(log => log.date < start || log.date > end);
        foreach (HabitLog habitLog in habitLogs)
        {
            Console.WriteLine(habitLog);
        }
    }
    
    public static void AddUnit()
    {
        Console.WriteLine("Insert unit name:");
        string? unit = Console.ReadLine();
        if (unit == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        Database.AddUnit(unit);
    }

    public static void AddHabit()
    {
        Console.WriteLine("Insert habit name:");
        string? habit = Console.ReadLine();
        if (habit == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        GetUnits();
        Console.WriteLine("Select unit by id:");
        if (!int.TryParse(Console.ReadLine(), out int unitId))
        {
            Console.WriteLine("Wrong input");
            return;
        }

        try
        {
            Database.AddHabit(habit, unitId);
        }
        catch (Exception)
        {
            Console.WriteLine("Wrong input");
        }
    }

    public static void AddHabitLog()
    {
        Console.WriteLine("Insert date in format dd-MM-yyyy:");
        string? date = Console.ReadLine();
        if (date == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        DateTime? dateTime = ConvertToDateTime(date);
        if (dateTime == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        DateTime nonNullDate = dateTime.Value;
        
        Console.WriteLine("Insert quantity:");
        if (!int.TryParse(Console.ReadLine(), out int quantity))
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        GetHabits();
        Console.WriteLine("Select habit by id:");
        if (!int.TryParse(Console.ReadLine(), out int habitId))
        {
            Console.WriteLine("Wrong input");
            return;
        }

        try
        {
            Database.AddHabitLog(nonNullDate, quantity, habitId);
        }
        catch (Exception)
        {
            Console.WriteLine("Wrong input");
        }
    }

    public static void DeleteUnit()
    {
        GetUnits();
        Console.WriteLine("Insert unit by id:");
        if (!int.TryParse(Console.ReadLine(), out int unitId))
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        Database.DeleteUnit(unitId);
    }

    public static void DeleteHabit()
    {
        GetHabits();
        Console.WriteLine("Insert habit by id:");
        if (!int.TryParse(Console.ReadLine(), out int habitId))
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        Database.DeleteHabit(habitId);
    }

    public static void DeleteHabitLog()
    {
        GetHabitLogs();
        Console.WriteLine("Insert habit log by id:");
        if (!int.TryParse(Console.ReadLine(), out int habitLogId))
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        Database.DeleteHabitLog(habitLogId);
    }

    public static void UpdateUnit()
    {
        GetUnits();
        Console.WriteLine("Select unit by id");
        if (!int.TryParse(Console.ReadLine(), out int unitId))
        {
            Console.WriteLine("Wrong input");
            return;
        }

        Console.WriteLine("Insert unit name:");
        string? unit = Console.ReadLine();
        if (unit == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        Database.UpdateUnit(unitId, unit);
    }

    public static void UpdateHabit()
    {
        GetHabits();
        Console.WriteLine("Select habit by id:");
        if (!int.TryParse(Console.ReadLine(), out int habitId))
        {
            Console.WriteLine("Wrong input");
            return;
        }

        Console.WriteLine("Insert habit name:");
        string? habit = Console.ReadLine();
        if (habit == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }

        GetUnits();
        Console.WriteLine("Select unit by id:");
        if (!int.TryParse(Console.ReadLine(), out int unitId))
        {
            Console.WriteLine("Wrong input");
            return;
        }

        try
        {
            Database.UpdateHabit(habitId, habit, unitId);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Wrong input");
        }
    }

    public static void UpdateHabitLog()
    {
        GetHabitLogs();
        Console.WriteLine("Select habit log by id:");
        if (!int.TryParse(Console.ReadLine(), out int habitLogId))
        {
            Console.WriteLine("Wrong input");
            return;
        }

        Console.WriteLine("Insert date in format dd-MM-yyyy:");
        string? date = Console.ReadLine();
        if (date == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        DateTime? dateTime = ConvertToDateTime(date);
        if (dateTime == null)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        DateTime nonNullDate = dateTime.Value;
        
        Console.WriteLine("Insert quantity:");
        if (!int.TryParse(Console.ReadLine(), out int quantity))
        {
            Console.WriteLine("Wrong input");
            return;
        }
        
        GetHabits();
        Console.WriteLine("Select habit by id:");
        if (!int.TryParse(Console.ReadLine(), out int habitId))
        {
            Console.WriteLine("Wrong input");
            return;
        }

        try
        {
            Database.UpdateHabitLog(habitLogId, nonNullDate, quantity, habitId);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Wrong input");
        }
    }
}