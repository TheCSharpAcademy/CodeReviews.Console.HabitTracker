using System.Globalization;
using HabitLogger;
using HabitLogger.Models;
using HabitLogger.Repositories;
using HabitLogger.Views;
using Microsoft.Data.Sqlite;

if (!File.Exists("habits.db"))
{
    var db = new HabitLoggerDatabase();
    db.CreateDatabase();
}


var endApp = false;
while (!endApp)
{
    var v = new MenuView();
    v.Title = "Habit Logger Main Menu";

    MenuItem[] menu =
    [
        new MenuItem
        {
            Text = "New Habit",
            View = NewHabitView
        },
        new MenuItem
        {
            Text = "List Habits",
            View = ListHabitsView
        }
    ];

    try
    {
        var userChoice = v.ShowMenu(menu.Select(m => m.Text).ToArray());
        switch (userChoice)
        {
            case -1:
                endApp = true;
                break;
            default:
                menu[userChoice].View();
                break;
        }
    }
    catch (SqliteException e) when (e.SqliteErrorCode == 8)
    {
        Console.WriteLine("ERROR: An error occured with accessing the database.");
        break;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Unknown error occured: {e.Message}");
        Console.WriteLine("Exiting application");
        break;
    }
}

return;

static void NewHabitView()
{
    bool endHabitView;
    do
    {
        Console.Clear();
        Console.WriteLine("Create a New Habit");
        Console.WriteLine("Enter the name of the habit:");
        var habitName = Console.ReadLine();
        Console.WriteLine("Enter a unit for measurement of the habit:");
        var habitUnit = Console.ReadLine();

        if (string.IsNullOrEmpty(habitName?.Trim()) || string.IsNullOrEmpty(habitUnit?.Trim()))
        {
            Console.WriteLine("Wrong input for either name or unit. Both fields require at least 1 character.");
            Console.WriteLine("Press any key to try again.");
            Console.ReadKey();
            endHabitView = false;
        }
        else
        {
            var habitRepository = new HabitRepository();
            habitRepository.AddHabit(new HabitModel { HabitName = habitName, Unit = habitUnit });

            Console.WriteLine($"Your habit {habitName} has been created! Press a key to return to the main menu.");
            Console.ReadKey();

            endHabitView = true;
        }
    } while (!endHabitView);
}

static void ListHabitsView()
{
    var habitRepository = new HabitRepository();
    var habits = habitRepository.GetAllHabits();
    bool endListHabitsView;

    do
    {
        List<MenuItem> menu = [];
        for (var i = 0; i < habits.Count; i++)
        {
            var index = i;
            menu.Add(new MenuItem
            {
                Text = habits[i].HabitName,
                View = () => HabitMenuView(habits[index])
            });
        }

        var view = new MenuView();
        view.Title = "A List of All Your Habits";

        var userChoice = view.ShowMenu(menu.Select(m => m.Text).ToArray());
        switch (userChoice)
        {
            case -1:
                endListHabitsView = true;
                break;
            default:
                menu[userChoice].View();
                endListHabitsView = false;
                break;
        }
    } while (!endListHabitsView);
}

static void HabitMenuView(HabitModel habit)
{
    var habitRepository = new HabitRepository();
    bool endHabitMenuView;

    do
    {
        var menu = new List<MenuItem>
        {
            new() { Text = "Show Log", View = () => HabitLogView(habit) },
            new() { Text = "Update Log", View = () => UpdateLogView(habit) },
            new()
            {
                Text = "Update Habit", View = () =>
                {
                    HabitUpdateView(habit);
                    habit = habitRepository.GetHabitById(habit.Id); // Refresh the habit details
                }
            }
        };

        var view = new MenuView();
        view.Title = $"{habit.HabitName} Menu";

        var userChoice = view.ShowMenu(menu.Select(m => m.Text).ToArray());
        switch (userChoice)
        {
            case -1:
                endHabitMenuView = true;
                break;
            default:
                menu[userChoice].View();
                endHabitMenuView = false;
                break;
        }
    } while (!endHabitMenuView);
}


static void HabitLogView(HabitModel habit)
{
    var habitLogRepository = new HabitLogRepository();
    var logs = habitLogRepository.GetAllLogsForHabitId(habit.Id);

    Console.Clear();
    Console.WriteLine($"{habit.HabitName} Logs");

    if (logs.Count == 0) Console.WriteLine("No logs found.");

    foreach (var log in logs) Console.WriteLine($"{log.Date:d} - {log.Quantity} {habit.Unit}");

    Console.WriteLine("Press any key to return to the habit menu.");
    Console.ReadKey();
}

static void UpdateLogView(HabitModel habit)
{
    bool endUpdateLogView;
    do
    {
        Console.Clear();
        Console.WriteLine($"Update {habit.HabitName} log");
        Console.WriteLine($"What date ({DateTime.Today.ToString("d", CultureInfo.CurrentCulture)})?");
        Console.WriteLine("(Press enter to use current date)");
        var date = Console.ReadLine();
        Console.WriteLine($"What quantity (in {habit.Unit})?");
        var quantity = Console.ReadLine();

        var parsedDate = DateTime.Today;
        var isDateValid = true;

        if (!string.IsNullOrEmpty(date?.Trim())) isDateValid = DateTime.TryParse(date, out parsedDate);

        var isQuantityValid = int.TryParse(quantity, out var parsedQuantity);

        if (isDateValid && isQuantityValid)
        {
            endUpdateLogView = true;
            var habitLog = new HabitLogModel
            {
                HabitId = habit.Id,
                Date = parsedDate,
                Quantity = parsedQuantity
            };
            var habitLogRepository = new HabitLogRepository();
            habitLogRepository.UpdateHabitLog(habitLog);
        }
        else
        {
            Console.WriteLine("Wrong input. Enter a valid date and a numeric quantity.");
            Console.WriteLine("Press any key to try again.");
            Console.ReadKey();
            endUpdateLogView = false;
        }
    } while (!endUpdateLogView);
}

static void HabitUpdateView(HabitModel habit)
{
    Console.Clear();
    Console.WriteLine($"Updating Habit: {habit.HabitName}");
    Console.WriteLine("Keep the values empty to keep the current value.");
    Console.WriteLine($"Update the name ({habit.HabitName})?");
    var habitName = Console.ReadLine();
    Console.WriteLine($"Update the unit ({habit.Unit})?");
    var habitUnit = Console.ReadLine();

    var updatedHabit = new HabitModel
    {
        HabitName = string.IsNullOrEmpty(habitName) ? habit.HabitName : habitName,
        Unit = string.IsNullOrEmpty(habitUnit) ? habit.Unit : habitUnit
    };

    var habitRepository = new HabitRepository();
    habitRepository.UpdateHabit(updatedHabit, habit.Id);
}