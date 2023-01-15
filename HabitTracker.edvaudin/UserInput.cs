namespace HabitTracker.edvaudin;

internal static class UserInput
{
    public static string GetEntryDate()
    {
        Console.Write("\nWhat date are you adding for? (yyyy-mm-dd): ");
        string input = Console.ReadLine();

        while (!Validator.IsValidDateInput(input))
        {
            Console.Write("This is not a valid date, please use the format 'yyyy-mm-dd'");
            input = Console.ReadLine();
        }
        return input;
    }

    public static int GetEntryQuantity(string measurement)
    {
        Console.Write($"\nHow many {measurement} did you do? ");
        string input = Console.ReadLine();

        while (!Validator.IsPositiveIntInput(input))
        {
            Console.Write("This is not a valid quantity, please enter a number: ");
            input = Console.ReadLine();
        }
        return Int32.Parse(input);
    }

    public static int GetHabitId()
    {
        string input = string.Empty;
        string listOfHabits = "\n";
        List<Habit> validHabits = DataAccessor.GetHabits();

        foreach (Habit habit in validHabits)
        {
            listOfHabits += $"{habit.GetString()}\n";
        }

        Console.WriteLine(listOfHabits);
        List<int> validHabitIds = validHabits.Select(h => h.Id).ToList();
        Console.Write("\nEnter the number corresponding to the habit this entry is for: ");

        while (true)
        {
            int result;
            if (Validator.IsIntInputWithResult(out result))
            {
                if (validHabitIds.Contains(result))
                {
                    return result;
                }
            }
            Console.Write("This is not a valid id, please enter a number: ");
        }
    }

    public static string GetHabitMeasurement()
    {
        Console.Write("\nWhat is the measurement of your habit? ");
        string input = Console.ReadLine();

        while (Validator.IsNonEmptyNonFullString(input))
        {
            Console.Write("Habit measurement should not be empty or more than 255 characters. Try again: ");
            input = Console.ReadLine();
        }
        return input;
    }

    public static string GetHabitName()
    {
        Console.Write("\nWhat is the name of your habit? ");
        string input = Console.ReadLine();

        while (Validator.IsNonEmptyNonFullString(input))
        {
            Console.Write("Habit name should not be empty or more than 255 characters. Try a new name: ");
            input = Console.ReadLine();
        }
        return input;
    }

    public static int GetIdForRemoval()
    {
        Console.Write("\nWhich entry do you want to remove? ");
        DataAccessor dal = new();
        List<int> validIds = DataAccessor.GetEntries().Select(o => o.Id).ToList();

        while (true)
        {
            if (Int32.TryParse(Console.ReadLine(), out int result))
            {
                if (validIds.Contains(result))
                {
                    return result;
                }
            }
            Console.Write("This is not a valid id, please enter a number: ");
        }
    }

    public static int GetIdForUpdate()
    {
        Console.Write("\nWhich entry do you want to update? ");
        DataAccessor dal = new();
        List<int> validIds = DataAccessor.GetEntries().Select(o => o.Id).ToList();

        while (true)
        {
            if (Int32.TryParse(Console.ReadLine(), out int result))
            {
                if (validIds.Contains(result))
                {
                    return result;
                }
            }
            Console.Write("This is not a valid id, please enter a number: ");
        }
    }

    public static string GetUserOption()
    {
        string input = Console.ReadLine();
        while (!Validator.IsValidOption(input))
        {
            Console.Write("This is not a valid input. Please enter one of the above options: ");
            input = Console.ReadLine();
        }
        return input;
    }
}