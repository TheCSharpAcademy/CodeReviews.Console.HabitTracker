namespace HabitTracker;

class Screen
{
    public static AppState MainMenu()
    {
        AppState nextAppState = AppState.MainMenu;
        do
        {
            Console.Clear();
            Console.WriteLine("1 - Add new habit");
            Console.WriteLine("2 - Select habit");
            Console.WriteLine("3 - Log habit");
            Console.WriteLine("4 - View habit log");
            Console.WriteLine("5 - Report: Frequency and total per month");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("Enter one of the numbers above to select a menu option.");

            switch (Console.ReadKey().KeyChar.ToString().ToUpper())
            {
                case "1":
                    nextAppState = AppState.HabitInsert;
                    break;
                case "2":
                    nextAppState = AppState.HabitSelect;
                    break;
                case "3":
                    nextAppState = AppState.LogInsert;
                    break;
                case "4":
                    nextAppState = AppState.LogViewList;
                    break;
                case "5":
                    nextAppState = AppState.ReportFrequencyAndTotalPerMonth;
                    break;
                case "0":
                    nextAppState = AppState.Exit;
                    break;
            }
        } while (nextAppState == AppState.MainMenu);
        return nextAppState;
    }

    public static Habit? HabitInsert()
    {
        Console.Clear();
        Console.WriteLine($"New habit");
        Console.WriteLine($"Enter '0' as name or uom to return to main menu without adding a new log entry.");
        string name = "";
        int nameMaxLength = 20;
        string uom = "";
        int uomMaxLength = 10;
        bool exit = false;
        bool isValidInput = false;
        while (!isValidInput && !exit)
        {
            Console.Write("Name: ");
            name = Console.ReadLine() ?? "";
            if ("0".Equals(name))
            {
                exit = true;
            }
            else if (String.IsNullOrEmpty(name))
            {
                Console.WriteLine("Please enter a name for the new habit.");
            }
            else if (name.Length > nameMaxLength)
            {
                Console.WriteLine($"Habit names must not be longer than {nameMaxLength} chars.");
            }
            else
            {
                isValidInput = true;
            }
        }

        isValidInput = false;
        while (!isValidInput && !exit)
        {
            Console.Write($"Unit of measure: ");
            uom = Console.ReadLine() ?? "";
            if ("0".Equals(uom))
            {
                exit = true;
            }
            else if (String.IsNullOrEmpty(uom))
            {
                Console.WriteLine("Please enter an uom for the new habit.");
            }
            else if (uom.Length > uomMaxLength)
            {
                Console.WriteLine($"The uom must not be longer than {uomMaxLength} chars.");
            }
            else
            {
                isValidInput = true;
            }
        }

        if (exit)
        {
            return null;
        }
        return new Habit(name, uom);
    }

    public static Habit? HabitSelect(List<Habit> habits)
    {
        Console.Clear();
        Console.WriteLine($"Habits");
        if (habits != null && habits.Count > 0)
        {
            string columnFormat = "{0,5} {1,20} {2,10}";
            Console.WriteLine(String.Format(columnFormat, "ID", "Name", "UOM"));
            foreach (var habit in habits)
            {
                Console.WriteLine(String.Format(columnFormat, habit.ID, habit.Name, habit.Uom));
            }
        }
        else
        {
            Console.WriteLine("No habits found.");
        }

        string? rawInput;
        Habit? selectedHabit = null;
        do
        {
            if (habits != null && habits.Count > 0)
            {
                Console.WriteLine("Enter ID and press enter to select a habit.");
            }
            else
            {
                Console.WriteLine("Press enter to return to main menu.");
            }

            rawInput = Console.ReadLine();
            if (!String.IsNullOrEmpty(rawInput) && habits != null)
            {
                if (int.TryParse(rawInput, out int id))
                {
                    selectedHabit = habits.Find(h => h.ID == id);
                    if (selectedHabit == null)
                    {
                        Console.WriteLine("Please enter an ID from the list above.");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter an integer value.");
                }
            }
        } while (!String.IsNullOrEmpty(rawInput) && selectedHabit == null);
        return selectedHabit;
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
            Console.Write($"Quantity [{habit.Uom}]: ");
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
        return new HabitLogRecord(habit.ID, date, quantity);
    }

    public static HabitLogRecord? LogViewList(Habit habit, List<HabitLogRecord> habitlog)
    {
        Console.Clear();
        Console.WriteLine($"Log entries for habit '{habit.Name}'");
        if (habitlog != null && habitlog.Count > 0)
        {
            string columnFormat = "{0,5} {1,10} {2,10}";
            Console.WriteLine(String.Format(columnFormat, "ID", "Date", habit.Uom));
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
        Console.WriteLine($"Quantity: {selectedLogRecord.Quantity} {habit.Uom}");
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
        Console.WriteLine($"Quantity: {selectedLogRecord.Quantity} {habit.Uom}");

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
            Console.Write($"New quantity [{habit.Uom}] (leave empty to keep old quantity): ");
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

    public static void ReportFrequencyAndTotalPerMonth(Habit habit, List<ReportFreqTotalMonthRecord> reportData)
    {
        Console.Clear();
        Console.WriteLine("Report 'Frequency and total per month'");
        Console.WriteLine($"Habit '{habit.Name}' measured in '{habit.Uom}'");
        if (reportData != null && reportData.Count > 0)
        {
            string columnFormat = "{0,5} {1,5} {2,10} {3,10}";
            Console.WriteLine(String.Format(columnFormat, "Year", "Month", "Frequency", "Total"));
            foreach (var reportLine in reportData)
            {
                Console.WriteLine(String.Format(columnFormat, reportLine.Year, reportLine.Month, reportLine.Frequency, reportLine.Total));
            }
        }
        else
        {
            Console.WriteLine("No report data found.");
        }
        Console.WriteLine("Press enter to proceed.");
        Console.ReadLine();
    }

    public static void Message(string msg)
    {
        Console.Clear();
        Console.WriteLine(msg);
        Console.WriteLine("Press enter to proceed.");
        Console.ReadLine();
    }
}