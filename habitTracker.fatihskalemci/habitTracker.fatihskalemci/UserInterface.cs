using habitTracker.fatihskalemci.Models;
using Spectre.Console;
using System.Globalization;
using static habitTracker.fatihskalemci.Enums;

namespace habitTracker.fatihskalemci;

internal class UserInterface
{
    private readonly DataBaseConnection dataBase = new();

    internal void MainMenu()
    {
        int randomEntryNumber = 100;

        if (!File.Exists("habit-Tracker.db"))
        {    
            for (int i = 0; i < randomEntryNumber; i++)
            {
                dataBase.CreateHabitEntriesTable();
                HabitEntry randomEntry = Helpers.GenerateRandomEntry();
                dataBase.AddEntry(randomEntry);
            }
        }
        
        bool exit = false;
        Habit habit = HabitSelection();

        while (!exit)
        {
            Console.Clear();
            HabitEntry habitEntry;

            var menuSelection = AnsiConsole.Prompt(new SelectionPrompt<MenuOptions>()
                .Title("Please select the action you want to perform")
                .AddChoices(Enum.GetValues<MenuOptions>()));

            switch (menuSelection)
            {
                case MenuOptions.Add:
                    habitEntry = GetHabitEntryFromUser(habit);
                    dataBase.AddEntry(habitEntry);
                    break;
                case MenuOptions.Update:
                    dataBase.UpdateEntry(habit);
                    break;
                case MenuOptions.Delete:
                    dataBase.DeleteEntry(habit);
                    break;
                case MenuOptions.ShowEntries:
                    dataBase.ShowEtries(habit);
                    break;
                case MenuOptions.ShowReport:
                    dataBase.MakeReport(habit);
                    break;
                case MenuOptions.ChangeHabit:
                    habit = HabitSelection();
                    break;
                case MenuOptions.Exit:
                    exit = true;
                    break;
            }
        }
    }

    internal Habit HabitSelection()
    {
        List<Habit> habitNames = dataBase.GetHabits();

        habitNames.Add(
            new Habit
            {
                HabitName = "New Habit",
                Unit = "none"
            });

        Habit selectedHabit = AnsiConsole.Prompt(
                new SelectionPrompt<Habit>()
                .Title("Please select the habit you want to perform on")
                .UseConverter(h => $"{h.HabitName}")
                .AddChoices(habitNames));

        if (selectedHabit.HabitName == "New Habit")
        {
            selectedHabit.HabitName = GetStringInput("Please enter the NAME of new habit");
            selectedHabit.Unit = GetStringInput("Please enter the UNIT of new habit");
        }
        return selectedHabit;
    }

    static internal HabitEntry GetHabitEntryFromUser(Habit habit)
    {
        DateTime dateInput = GetDateInput();

        int integerInput = GetIntegerInput("Please enter the quantity");

        return new HabitEntry
        {
            Quantity = integerInput,
            Date = dateInput,
            Unit = habit.Unit,
            HabitName = habit.HabitName
        };
    }

    static internal int GetIntegerInput(string message)
    {
        Console.WriteLine(message);
        string? userInput = Console.ReadLine();
        int integerInput;

        while (!Int32.TryParse(userInput, out integerInput))
        {
            Console.WriteLine("Please enter a valid integer");
            userInput = Console.ReadLine();
        }
        return integerInput;
    }

    static internal DateTime GetDateInput()
    {
        Console.WriteLine("Please enter the date of the entry as yyyy-MM-dd(Press enter for today)");
        string? userInput = Console.ReadLine();
        DateTime dateInput;

        if (userInput == "")
        {
            dateInput = DateTime.Now;
        }
        else
        {
            while (!DateTime.TryParseExact(userInput, "yyyy-MM-dd", new CultureInfo("tr-TR"), DateTimeStyles.None, out dateInput))
            {
                Console.WriteLine("Please check your date format (yyyy-MM-dd)");
                userInput = Console.ReadLine();
            }
        }
        return dateInput;
    }

    static internal string GetStringInput(string message)
    {
        Console.WriteLine(message);
        string? userInput = Console.ReadLine();

        while (userInput == null)
        {
            Console.WriteLine("Please enter a valid value");
            userInput = Console.ReadLine();
        }
        return userInput;
    }
}
