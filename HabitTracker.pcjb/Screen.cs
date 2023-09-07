namespace HabitTracker;

class Screen
{
    public static AppState MainMenu()
    {
        AppState nextAppState = AppState.MainMenu;
        do
        {
            Console.Clear();
            Console.WriteLine("1 - Log habit");
            Console.WriteLine("2 - View habit log");
            Console.WriteLine("0 - Exit");

            switch (Console.ReadKey().KeyChar.ToString().ToUpper())
            {
                case "1":
                    nextAppState = AppState.LogInsert;
                    break;
                case "2":
                    nextAppState = AppState.LogView;
                    break;
                case "0":
                    nextAppState = AppState.Exit;
                    break;
            }
        } while (nextAppState == AppState.MainMenu);
        return nextAppState;
    }

    public static HabitLogRecord LogInsert(Habit habit)
    {
        Console.Clear();
        Console.WriteLine($"New log entry for habit '{habit.Name}'");
        Console.WriteLine($"Enter '0' as date or quantity to return to main menu without adding a new log entry.");
        var today = DateOnly.FromDateTime(DateTime.Now);
        var date = today;
        int quantity = 0;
        bool exit = false;
        bool isValidInput = false;
        while (!isValidInput && !exit)
        {
            Console.Write("Date: ");
            var rawDate = Console.ReadLine();
            if ("0".Equals(rawDate))
            {
                exit = true;
            }
            else if (!DateOnly.TryParse(rawDate, out date))
            {
                Console.WriteLine($"Please enter a valid date, e.g. {today}.");
            }
            else if (date > today)
            {
                Console.WriteLine($"You cannot log habits for future dates.");
            }
            else
            {
                isValidInput = true;
            }
        }

        isValidInput = false;
        while (!isValidInput && !exit)
        {
            Console.Write($"Quantity [{habit.UOM}]: ");
            var rawQuantity = Console.ReadLine();
            if ("0".Equals(rawQuantity))
            {
                exit = true;
            }
            else if (!int.TryParse(rawQuantity, out quantity))
            {
                Console.WriteLine("Please enter an integer value.");
            }
            else if (quantity < 1)
            {
                Console.WriteLine("Please enter a value greater than zero.");
            }
            else
            {
                isValidInput = true;
            }
        }

        return new HabitLogRecord(date, quantity);
    }

    public static void LogInsertOK()
    {
        Console.WriteLine("Habit log entry saved.");
        Console.WriteLine("Press enter to return to main menu.");
        Console.ReadLine();
    }

    public static void LogInsertError()
    {
        Console.WriteLine("Technical Error: Habit log entry could not be saved. The error was logged.");
        Console.WriteLine("Press enter to return to main menu.");
        Console.ReadLine();
    }

    public static void LogView(Habit habit, List<HabitLogRecord> habitlog)
    {
        Console.Clear();
        Console.WriteLine($"Log entries for habit '{habit.Name}'");
        if (habitlog != null && habitlog.Count > 0)
        {
            string columnFormat = "{0,5} {1,10} {2,10}";
            Console.WriteLine(String.Format(columnFormat, "ID", "Date", habit.UOM));
            foreach (var habitLogRecord in habitlog)
            {
                Console.WriteLine(String.Format(columnFormat, habitLogRecord.ID, habitLogRecord.Date, habitLogRecord.Quantity));
            }
        }
        else
        {
            Console.WriteLine("No log entries found.");
        }

        Console.WriteLine("Press enter to return to main menu.");
        Console.ReadLine();
    }

}