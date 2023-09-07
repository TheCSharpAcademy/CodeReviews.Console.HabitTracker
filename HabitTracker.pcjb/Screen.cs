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
            Console.WriteLine("Enter one of the numbers above to select a menu option.");

            switch (Console.ReadKey().KeyChar.ToString().ToUpper())
            {
                case "1":
                    nextAppState = AppState.LogInsert;
                    break;
                case "2":
                    nextAppState = AppState.LogViewList;
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

    public static HabitLogRecord? LogViewList(Habit habit, List<HabitLogRecord> habitlog)
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

        string? rawInput;
        HabitLogRecord? selectedLogRecord = null;
        do
        {
            if (habitlog != null && habitlog.Count > 0)
            {
                Console.WriteLine("Enter ID and press enter to edit/delete a log entry or press enter alone to return to main menu.");
            }
            else
            {
                Console.WriteLine("Press enter to return to main menu.");
            }

            rawInput = Console.ReadLine();
            if (!String.IsNullOrEmpty(rawInput) && habitlog != null)
            {
                if (int.TryParse(rawInput, out int id))
                {
                    selectedLogRecord = habitlog.Find(r => r.ID == id);
                    if (selectedLogRecord == null)
                    {
                        Console.WriteLine("Please enter an ID from the list above.");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter an integer value.");
                }
            }
        } while (!String.IsNullOrEmpty(rawInput) && selectedLogRecord == null);
        return selectedLogRecord;
    }

    public static AppState LogViewOne(Habit habit, HabitLogRecord selectedLogRecord)
    {
        Console.Clear();
        Console.WriteLine($"Log entry for habit '{habit.Name}':");
        Console.WriteLine($"ID      : {selectedLogRecord.ID}");
        Console.WriteLine($"Date    : {selectedLogRecord.Date}");
        Console.WriteLine($"Quantity: {selectedLogRecord.Quantity} {habit.UOM}");
        Console.WriteLine("Enter 'e' to edit or 'd' to delete this log entry and press enter or press enter alone to cancel.");
        return Console.ReadLine() switch
        {
            "e" => AppState.LogEdit,
            "d" => AppState.LogDelete,
            _ => AppState.LogViewList,
        };
    }

    public static HabitLogRecord LogEdit(Habit habit, HabitLogRecord selectedLogRecord)
    {
        var editedLogRecord = (HabitLogRecord)selectedLogRecord.Clone();
        Console.Clear();
        Console.WriteLine($"Edit log entry for habit '{habit.Name}':");
        Console.WriteLine($"ID      : {selectedLogRecord.ID}");
        Console.WriteLine($"Date    : {selectedLogRecord.Date}");
        Console.WriteLine($"Quantity: {selectedLogRecord.Quantity} {habit.UOM}");

        var today = DateOnly.FromDateTime(DateTime.Now);
        bool isValidInput = false;
        while (!isValidInput)
        {
            Console.Write("New Date (leave empty to keep old date): ");
            var rawDate = Console.ReadLine();
            if (String.IsNullOrEmpty(rawDate))
            {
                isValidInput = true;
            }
            else if (!DateOnly.TryParse(rawDate, out DateOnly date))
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
                editedLogRecord.Date = date;
            }
        }

        isValidInput = false;
        while (!isValidInput)
        {
            Console.Write($"New quantity [{habit.UOM}] (leave empty to keep old quantity): ");
            var rawQuantity = Console.ReadLine();
            if (String.IsNullOrEmpty(rawQuantity))
            {
                isValidInput = true;
            }
            else if (!int.TryParse(rawQuantity, out int quantity))
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
                editedLogRecord.Quantity = quantity;
            }
        }
        return editedLogRecord;
    }

    public static void Message(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Console.WriteLine("Press enter to proceed.");
        Console.ReadLine();
    }
}