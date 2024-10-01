using HabitTrackerLibrary;

namespace HabitTracker;

public static class Program
{
    public static void Main()
    {
        Console.Clear();
        Console.WriteLine("===============================================");
        Console.WriteLine("=========== Welcome to HabitTracker ===========");
        Console.WriteLine("===============================================");
        HabitDatabase.CreateDatabase();
        var isRunning = true;
        while (isRunning)
        {
            Menu.ShowOptions();
            var option = Menu.GetOptions();
            switch (option)
            {
                case Options.ViewHabits:
                    ShowHabitsHistory();
                    break;
                case Options.LogHabit:
                    LogHabit();
                    break;
                case Options.DeleteHabit:
                    DeleteHabit();
                    break;
                case Options.UpdateHabit:
                    UpdateHabitLog();
                    break;
                case Options.Help:
                    Menu.ShowHelp();
                    break;
                case Options.Exit:
                    isRunning = false;
                    break;
            }
        }
    }

    private static void ShowHabitsHistory()
    {
        Console.Clear();
        Console.WriteLine("Enter habit's name or press enter to skip");
        var name = "";
        var date = "";
        do
        {
            name = Console.ReadLine();
        } while (!HabitDatabase.HabitTypes.ContainsKey(name) && !string.IsNullOrEmpty(name));


        Console.WriteLine("Enter habit's date or press enter to skip");
        do
        {
            var readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine))
            {
                date = null;
                break;
            }

            if (DateTime.TryParse(readLine, out var dateTime))
            {
                date = dateTime.ToString("yyyy-MM-dd");
                break;
            }
        } while (true);

        HabitDatabase.DisplayHabitsHistory(name, date);
    }

    private static void UpdateHabitLog()
    {
        var name = "";
        var date = "";
        var quantity = "";
        Console.Clear();
        Console.WriteLine("--Update Habit Log--");
        Console.WriteLine("Enter habit's name");
        do
        {
            name = Console.ReadLine();
        } while (!HabitDatabase.HabitTypes.ContainsKey(name));

        if (HabitDatabase.HabitTypes[name] == null)
        {
            Console.WriteLine("Sorry the habit doesnt have amount to alter");
            Console.WriteLine("Press enter to return back");
            Console.ReadLine();
        }

        Console.WriteLine("Enter habit's date");
        do
        {
            var readLine = Console.ReadLine();

            if (DateTime.TryParse(readLine, out var dateTime))
            {
                date = dateTime.ToString("yyyy-MM-dd");
                break;
            }
        } while (true);

        Console.WriteLine("Enter habit's amount ##-" + HabitDatabase.HabitTypes[name].Unit);
        var amount = 0;
        while (amount == 0)
        {
            var readLine = Console.ReadLine();
            int.TryParse(readLine, out amount);
        }

        quantity = new Habit.Amount(amount, HabitDatabase.HabitTypes[name].Unit).ToString();
        var command =
            @"UPDATE Habits SET Quantity = @Quantity WHERE HabitType = @HabitType AND LoggingDate = @LoggingDate";
        HabitDatabase.UpdateHabitLog(command, name, date, quantity);
    }

    private static void LogHabit()
    {
        Console.Clear();
        Console.WriteLine("--Log Habit--");
        Console.WriteLine("{1} log an existing habit type");
        Console.WriteLine("{2} Log of a habit of new type (will be added to your choices)");
        var choice = 0;
        do
        {
            Console.Write("Enter num: ");
            var readline = Console.ReadLine();
            int.TryParse(readline, out choice);
        } while (choice < 1 || choice > 2);

        switch (choice)
        {
            case 1:
                LogExistingHabit();
                break;
            case 2:
                LogNewHabit();
                break;
        }
    }

    private static void LogNewHabit()
    {
        Console.Clear();
        Console.WriteLine("Let's create new habits");
        Console.WriteLine("Enter Habit name: ");
        var habitName = "";
        var date = "";
        var amount = "";
        do
        {
            var readline = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(readline)) habitName = readline;
        } while (string.IsNullOrWhiteSpace(habitName));

        Console.WriteLine("Press Enter to get current date or enter custom date in format yyyy-MM-dd");
        do
        {
            var readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine))
            {
                date = DateTime.Now.ToString("yyyy-MM-dd");
                break;
            }

            if (DateTime.TryParse(readLine, out var dateTime))
            {
                date = dateTime.ToString("yyyy-MM-dd");
                break;
            }
        } while (true);

        Console.WriteLine("Finally create the habit's measurement.");
        Console.WriteLine("press enter to avoid or enter it in this format ##-unit");

        {
            var readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine))
                amount = null;
            else
                amount = readLine;
        }

        try
        {
            var newHabit = new Habit(habitName, date, amount);
            HabitDatabase.AddHabit(newHabit);
            HabitDatabase.UpdateHabitTypes();
            Console.WriteLine($"Here is your habit details: {newHabit}\nPress enter to continue...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + "\nPress enter to continue...");
            Console.ReadLine();
        }
    }

    private static void DeleteHabit()
    {
        Console.Clear();
        Console.WriteLine("--Delete Habit--");
        Console.WriteLine("Enter Habit name: ");
        var habitName = "";
        do
        {
            var readline = Console.ReadLine();
            if (!string.IsNullOrEmpty(readline)) habitName = readline;

            if (!HabitDatabase.HabitTypes.ContainsKey(habitName)) habitName = "";
        } while (string.IsNullOrWhiteSpace(habitName));

        Console.WriteLine("Enter habit date(yyyy-MM-dd) or press enter to select all");
        var date = "";
        do
        {
            var readline = Console.ReadLine();
            if (DateTime.TryParse(readline, out var dateTime)) date = dateTime.ToString("yyyy-MM-dd");
        } while (!string.IsNullOrWhiteSpace(date) && !DateTime.TryParse(date, out _));

        HabitDatabase.DeleteHabit(habitName, date);
    }

    private static void LogExistingHabit()
    {
        Console.WriteLine("--Choose Habit Type--");
        var habitTypeNames = HabitDatabase.HabitTypes.Keys.ToList();

        for (var i = 0; i < habitTypeNames.Count; i++) Console.WriteLine($"{i + 1}->  {habitTypeNames[i]}");

        //Gets habit type
        Console.WriteLine("Enter Choice's num:");
        int option;
        do
        {
            var readLine = Console.ReadLine();
            int.TryParse(readLine, out option);
        } while (option < 1 || option > habitTypeNames.Count);

        var type = habitTypeNames[option - 1];

        //Gets Habit date log
        Console.WriteLine("press Enter to get to use current data or enter date with format DD-MM-YYYY");
        var date = "";
        do
        {
            var readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine))
            {
                date = DateTime.Now.ToString("yyyy-MM-dd");
                break;
            }

            if (DateTime.TryParse(readLine, out var dateTime))
            {
                date = dateTime.ToString("yyyy-MM-dd");
                break;
            }
        } while (true);

        //Get Amount
        string? quantity = null;
        if (HabitDatabase.HabitTypes[type] != null)
        {
            Console.WriteLine($"Enter Amount ##-{HabitDatabase.HabitTypes[type].Unit}");
            do
            {
                var readline = Console.ReadLine();
                if (int.TryParse(readline, out var amount))
                {
                    quantity = new Habit.Amount(amount, HabitDatabase.HabitTypes[type].Unit).ToString();
                    Console.WriteLine(quantity);
                    Console.ReadLine();
                    break;
                }
            } while (true);
        }

        Habit habit;
        try
        {
            habit = new Habit(type, date, quantity);
            HabitDatabase.AddHabit(habit);
            Console.WriteLine($"Habit added successfully - {habit}");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error logging Habit : " + ex.Message + "\nPress enter to continue...");
            Console.ReadLine();
        }
    }
}