using ConsoleHabitTracker.kraven88.DataAccess;
using ConsoleHabitTracker.kraven88.Models;

namespace ConsoleHabitTracker.kraven88;

internal class Menu
{
    private SqliteDB db;
    private string nl = Environment.NewLine;
    private List<Habit> habits = new();

    public Menu(SqliteDB db)
    {
        this.db = db;
    }

    internal void MainMenu()
    {
        var isRunning = true;
        while (isRunning)
        {
            DisplayMainMenuItems();
            isRunning = SelectHabit(habits);
        }
    }
    private int AskForInteger(string message)
    {
        Console.Write(message);
        var input = Console.ReadLine()!.Trim();
        if (int.TryParse(input, out var value) && value >= 0)
            return value;
        else
        {
            Console.WriteLine("Invalid input. Please enter a whole, positive number." + nl);
            return AskForInteger(message);
        }
    }

    private bool CreateNewHabit()
    {
        var isCorrect = false;

        while (isCorrect == false)
        {
            HeaderText();

            Console.Write("Please enter new habit NAME: ");
            var name = Console.ReadLine()!.Trim();

            Console.Write("Please enter new habit UNIT OF MEASUREMENT: ");
            var unit = Console.ReadLine()!.Trim();

            var goal = AskForInteger("What is your daily goal (numbers only): ");

            Console.WriteLine(nl + $"Habit name: {name}, unit of measurement: {unit}, daily goal: {goal}");

            Console.Write("Is this correct (Yes/No): ");
            var confirmation = Console.ReadLine()!.Trim().ToLower();
            if (confirmation == "yes")
            {
                db.SaveNewHabit(name, unit, goal);
                var habit = new Habit()
                {
                    Name = name,
                    UnitOfMeasurement = unit,
                    CurrectGoal = goal,
                };
                habits.Add(habit);

                Console.WriteLine(nl + $"{name} habit was created");
                Console.ReadKey();
                isCorrect = true;
            }
            else if (confirmation == "no")
            {
                isCorrect = CreateNewHabit();
            }
            else isCorrect = true;
        };

        return isCorrect;
    }

    private bool DeleteAllProgress(Habit habit)
    {
        var validCoice = !ViewEntireProgress(habit);
        Console.WriteLine("Are you sure you want to delete all records? (Yes/No)");

        while (validCoice == false)
        {
            var answer = Console.ReadLine()!.Trim().ToLower();
            if (answer == "yes")
            {
                db.DeleteAllProgress(habit);
                validCoice = true;
                Console.WriteLine(nl + "All records has been deleted.");
            }
            else if (answer == "no")
                validCoice = true;
        }

        return true;
    }

    private bool DeleteCurrentProgress(Habit habit)
    {
        var validChoice = !ViewCurrentProgress(habit);
        var today = DateOnly.FromDateTime(DateTime.Now).ToString("dd.MM.yyyy");
        Console.WriteLine("Are you sure you want to delete that record? (Yes/No)");

        while (validChoice == false)
        {
            var answer = Console.ReadLine()!.Trim().ToLower();
            if (answer == "yes")
            {
                db.DeleteCurrentProgress(habit, today);
                validChoice = true;
                Console.WriteLine(nl + "Record deleted.");
                Console.ReadLine();
            }
            else if (answer == "no")
                validChoice = true;
        }

        return true;
    }

    private void DisplayHabitMenuItems(Habit habit)
    {
        HeaderText(habit.Name);
        Console.WriteLine("  1 - Update daily progress");
        Console.WriteLine("  2 - View todays progress");
        Console.WriteLine("  3 - View entire progress");
        Console.WriteLine("  4 - Set habit daily goal");
        Console.WriteLine("  5 - Delete current progress");
        Console.WriteLine("  6 - Delete all progress");

        Console.WriteLine($"{nl}  0 - Return");
    }

    private void DisplayMainMenuItems()
    {
        HeaderText();
        try
        {
            habits = db.LoadExistingHabits();
            foreach (var habit in habits)
            {
                Console.WriteLine($"{habit.Id}. {habit.Name} - Unit of measurement: {habit.UnitOfMeasurement}");
            }
            Console.WriteLine(nl + "============================");
            Console.WriteLine("Please select the number of habit you want to access, " + nl +
                              "Type \"new\" to create new habit for the logger," + nl +
                              "Type \"0\" to quit the Application");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Couldn't load the Habit list: {ex.Message}");
            Console.WriteLine(nl + "============================");
            Console.WriteLine("Type \"new\" to create new habit for the logger," + nl +
                              "Type \"0\" to quit the Application");
        }
    }

    internal bool HabitMenu(Habit habit)
    {
        var isRunning = true;
        while (isRunning)
        {
            DisplayHabitMenuItems(habit);
            var select = Console.ReadKey(true).KeyChar;
            isRunning = select switch
            {
                '1' => UpdateDailyProgress(habit),
                '2' => ViewCurrentProgress(habit),
                '3' => ViewEntireProgress(habit),
                '4' => SetDailyGoal(habit),
                '5' => DeleteCurrentProgress(habit),
                '6' => DeleteAllProgress(habit),
                '0' => false,
                _ => true,
            };
        }
        return true;
    }

    private void HeaderText(string habitName = "Habit")
    {
        Console.Clear();
        Console.WriteLine("============================");
        Console.WriteLine($"   {habitName} Logger by kraven   ");
        Console.WriteLine("============================" + nl);
    }

    private bool SelectHabit(List<Habit> habits)
    {
        Console.Write(nl + "Select: ");
        var select = Console.ReadLine()!.Trim().ToLower();

        if (select == "new")
            return CreateNewHabit();

        else if (select == "0")
            return false;

        else if (int.TryParse(select, out int number) && number > 0 && number <= habits.Count)
        {
            var habit = habits.Where(x => x.Id == number).First();
            return HabitMenu(habit);
        }
        else
            return true;

    }

    private bool SetDailyGoal(Habit habit)
    {
        HeaderText(habit.Name);

        Console.WriteLine($"{habit.Name} current daily goal is {habit.CurrectGoal} {habit.UnitOfMeasurement}");

        var newGoal = AskForInteger("Enter new daily goal (0 to leave unchanged): ");

        if (newGoal == 0) return true;
        else
        {
            db.SaveNewGoal(habit, newGoal);
            habit.CurrectGoal = newGoal;
        }

        return true;

    }

    private bool UpdateDailyProgress(Habit habit)
    {
        HeaderText(habit.Name);
        var today = DateOnly.FromDateTime(DateTime.Now).ToString("dd.MM.yyyy");

        var newProgress = AskForInteger($"Please enter the number of ({habit.UnitOfMeasurement}) since your last update: ");
        if (newProgress == 0) return true;
        else
        {
            db.SaveProgress(habit, today, newProgress);
            Console.WriteLine("Progress succesfuly updated.");
            Console.ReadKey();
            return true;
        }
    }

    private bool ViewCurrentProgress(Habit habit)
    {
        HeaderText(habit.Name);
        var today = DateOnly.FromDateTime(DateTime.Now).ToString("dd.MM.yyyy");

        try
        {
            habit.ProgressList = db.LoadCurrentProgress(habit);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Oops! Something went wrong: {ex.Message}");
            Console.ReadKey();
            return true;
        }

        var current = habit.ProgressList.LastOrDefault();
        Console.WriteLine($"Todays progress for {habit.Name}: [ {habit.ProgressList.First().Quantity} / {habit.ProgressList.First().DailyGoal} ] {habit.UnitOfMeasurement}.");
        Console.ReadLine();

        return true;
    }

    private bool ViewEntireProgress(Habit habit)
    {
        HeaderText(habit.Name);
        try
        {
            habit.ProgressList = db.LoadAllProgress(habit);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Oops! Something went wrong: {ex.Message}");
            Console.ReadKey();
            return true;
        }
        var progress = habit.ProgressList.OrderByDescending(x => x.Date);

        foreach (var day in progress)
            Console.WriteLine($" Day: {day.Date}, [ {day.Quantity} / {day.DailyGoal} ] {habit.UnitOfMeasurement}");

        Console.ReadKey();

        return true;
    }
}
