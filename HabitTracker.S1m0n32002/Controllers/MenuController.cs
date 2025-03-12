using HabitTracker.S1m0n32002.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Diagnostics;
using System.Globalization;
namespace HabitTracker.S1m0n32002.Controllers;

 class MenuController
{
    enum MainMenuChoices
    {
        Begin, 
        ShowReport,
        Exit
    }

    readonly Dictionary<string, MainMenuChoices> mainMenuStrs = new()
    {
        { "Begin"     , MainMenuChoices.Begin},
        { "Show report"     , MainMenuChoices.ShowReport },
        { "[yellow]Exit[/]" , MainMenuChoices.Exit }
    };

    static readonly DbController dbController = new();

    public bool ShowMainMenu()
    {
        Console.Clear();

        PrintRule("Main menu"); // Somehow if i make the whole class static the first time it's called the rule gets deleted

        var prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(mainMenuStrs.Keys)
        );

        if (mainMenuStrs.TryGetValue(prompt, out MainMenuChoices result))
        {
            switch (result)
            {
                case MainMenuChoices.Begin:
                    EditHabits();
                    break;
                case MainMenuChoices.ShowReport:
                    ShowReport();
                    break;
                case MainMenuChoices.Exit:
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Show the report of all habits
    /// </summary>
    void ShowReport()
    {
        Console.Clear();

        var habits = dbController.LoadAllHabits();

        BarChart chart = new()
        {
            Label = "Habits",
        };

        int c = 0;

        foreach (Habit habit in habits)
        {
            chart.AddItem($"{habit.Name} [yellow]({habit.Periodicity})[/]", habit.TimesPerPeriod, Color.FromInt32(++c));
        }

        Panel panel = new(chart)
        {
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Yellow),
            Header = new PanelHeader("Habits")
            {
                Justification = Justify.Left
            }
        };

        AnsiConsole.Write(panel);
        AnsiConsole.Markup("Press [blue]ENTER[/] to exit");
        Console.ReadLine();
    }

    /// <summary>
    /// Show all habits
    /// </summary>
    void EditHabits()
    {
        while (true)
        {
            var habit = ChooseHabit();

            if (habit == null)
                return;

            if (habit.Id < 0)
            {
                habit = EditHabit(habit);

                try
                {
                    dbController.SaveHabit(habit);
                }
                catch (SqliteException ex)
                {
                    Debug.WriteLine(ex);
                    AnsiConsole.MarkupLine($"[red]Habit name \"{habit.Name}\" already exists[/]");
                    AnsiConsole.MarkupLine($"Press [blue]ENTER[/] to continue");
                    Console.ReadLine();
                }
                continue;
            }

            switch (ChooseAction(false))
            {
                case Actions.EditOccurrences:
                    EditOccurrences(habit);
                    break;
                case Actions.Edit:
                    dbController.SaveHabit(EditHabit(habit));
                    break;
                case Actions.Delete:
                    DeleteHabit(habit);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Show all habit's occurrences
    /// </summary>
    void EditOccurrences(Habit habit)
    {
        while (true)
        {
            var occurrence = ChooseOccurrence(habit);

            if (occurrence == null)
                return;

            if (occurrence.Id < 0)
            {
                occurrence = EditOccurrence(occurrence);

                try
                {
                    dbController.SaveOccurrence(occurrence);
                }
                catch (SqliteException ex)
                {
                    Debug.WriteLine(ex);
                    AnsiConsole.MarkupLine($"[red]Habit name \"{habit.Name}\" already exists[/]");
                    AnsiConsole.MarkupLine($"Press [blue]ENTER[/] to continue");
                    Console.ReadLine();
                }
                continue;
            }

            switch (ChooseAction())
            {
                case Actions.Edit:
                    dbController.SaveOccurrence(EditOccurrence(occurrence));
                    break;
                case Actions.Delete:
                    dbController.DeleteOccurrences(occurrence);
                    break;
                default:
                    break;
            }
        }
    }

    enum Actions
    {
        EditOccurrences,
        Edit,
        Delete,
        Exit
    }

     readonly Dictionary<string, Actions> actionsMenuStrs = new()
    {
        { "Edit occurrences"  , Actions.EditOccurrences},
        { "Edit"              , Actions.Edit },
        { "Delete"            , Actions.Delete },
        { "[yellow]Exit[/]"   , Actions.Exit   }
    };

    /// <summary>
    /// Promt user to choose an action
    /// </summary>
     Actions? ChooseAction(bool generic = true)
    {
        Console.Clear();

        PrintRule("Choose Action");

        var prompt = new SelectionPrompt<string>();

        if (!generic)
            prompt.AddChoices(actionsMenuStrs.Keys.Where(x => actionsMenuStrs[x] == Actions.EditOccurrences));

        prompt.AddChoices(actionsMenuStrs.Keys.Where(x => actionsMenuStrs[x] != Actions.EditOccurrences));


        var chosenAction = AnsiConsole.Prompt(prompt);

        if (actionsMenuStrs.TryGetValue(chosenAction, out Actions action))
            return action;

        return null;
    }

    /// <summary>
    /// Prompt user to choose a habit
    /// </summary>
    /// <returns> The chosen habit or null if user chose to exit</returns>
     Habit? ChooseHabit()
    {
        Console.Clear();

        PrintRule("Choose Habit"); // idk why but this doesn't remain on screen the first time it's called

        Dictionary<string, Habit?> menuStrs = [];

        int c = 0;
        foreach (Habit habit in dbController.LoadAllHabits())
        {
            menuStrs.Add($"{c++}.\t{habit.Name}", habit);
        }

        menuStrs.Add("[green]Add[/]", new Habit());
        menuStrs.Add("[yellow]Exit[/]", null);

        var habitPrompt = new SelectionPrompt<string>()
            .AddChoices(menuStrs.Keys);

        var chosenHabitName = AnsiConsole.Prompt(habitPrompt);

        if (menuStrs.TryGetValue(chosenHabitName,out Habit? chosenHabit))
                return chosenHabit;

        return null;
    }

    /// <summary>
    /// Prompt user to choose an occurrence
    /// </summary>
    /// <param name="habit">The habit to choose occurrences from</param>
    /// <returns> The chosen occurrence or null if user chose to exit</returns>
     Habit.Occurrence? ChooseOccurrence(Habit habit)
    {
        Console.Clear();

        PrintRule($"Occurrences of \"{habit.Name}\"");

        Dictionary<string, Habit.Occurrence?> menuStrs = [];
        
        int c = 0;
        foreach (Habit.Occurrence occurrence in dbController.LoadAllOccurrences(habit))
        {
            menuStrs.Add($"{++c}.\t{occurrence.Date}", occurrence);
        }

        menuStrs.Add("[green]Add[/]", new Habit.Occurrence() { HabitId = habit.Id});
        menuStrs.Add("[yellow]Exit[/]", null);

        var occurrancePrompt = new SelectionPrompt<string>()
            .AddChoices(menuStrs.Keys);

        var chosenOccurrenceStr = AnsiConsole.Prompt(occurrancePrompt);

        if (menuStrs.TryGetValue(chosenOccurrenceStr, out Habit.Occurrence? chosenOccurrence))
            return chosenOccurrence;

        return null;
    }
     
    /// <summary>
    /// Allows user to edit a habit
    /// </summary>
     Habit EditHabit(Habit habit)
    {
        Console.Clear();

        PrintRule("Edit Habit");

        var namePrompt = new TextPrompt<string>("Enter habit name:")
        {
            ShowDefaultValue = true,
            AllowEmpty = false,
        };
        namePrompt.DefaultValue(!string.IsNullOrWhiteSpace(habit.Name) ? habit.Name : "Habit");    

        habit.Name = AnsiConsole.Prompt(namePrompt);

        var periodicityPrompt = new SelectionPrompt<Habit.Periodicities>()
        {
            Title = $"Select periodicity [green]({habit.Periodicity})[/]"
        };

        if (habit.Id < 0)
        {
            periodicityPrompt.AddChoices(Enum.GetValues<Habit.Periodicities>()
                                                    .Where(p => p != Habit.Periodicities.None));
        }
        else
        {
            periodicityPrompt.AddChoices(Enum.GetValues<Habit.Periodicities>()
                                                    .OrderBy(p => p == Habit.Periodicities.None ? -1 : 1));
        }

        var periodicity = AnsiConsole.Prompt(periodicityPrompt);

        if(periodicity != Habit.Periodicities.None)
            habit.Periodicity = periodicity;

        return habit;
    }

    /// <summary>
    /// Allows user to edit an occurrence
    /// </summary>
     Habit.Occurrence EditOccurrence(Habit.Occurrence occurrence)
    {
        Console.Clear();

        PrintRule("Edit Occurrence");

        var prompt = new TextPrompt<DateTime>($"Enter date [yellow](Format: {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} {CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern})[/]")
        {
            ShowDefaultValue = true,
            AllowEmpty = false,
            Converter = (x) => x.ToString(),
            Culture = CultureInfo.CurrentCulture
        };

        if (occurrence.Date == new DateTime())
            prompt.DefaultValue(DateTime.Now);
        else
            prompt.DefaultValue(occurrence.Date);

        occurrence.Date = AnsiConsole.Prompt(prompt);

        return occurrence;
    }

    enum MyConfirmation
    {
        No,
        Yes
    }

    /// <summary>
    /// Allows user to delete a habit
    /// </summary>
     Habit DeleteHabit(Habit habit)
    {
        Console.Clear();

        PrintRule("Edit Habit");

        var prompt = new SelectionPrompt<MyConfirmation>()
        {
            Title = $"Are you sure you want to delete \"{habit.Name}\"?"
        }
        .AddChoices(Enum.GetValues<MyConfirmation>());

        var answer = AnsiConsole.Prompt(prompt);

        if (answer == MyConfirmation.Yes)
        {
            dbController.DeleteAllOccurrences(habit);
            dbController.DeleteHabit(habit);
        }

        return habit;
    }

     void PrintRule(string Title)
    {
        Rule rule = new()
        {
            Title = $"[white]{Title}[/]",
            Justification = Justify.Left,
            Style = Style.Parse("yellow")
        };
        AnsiConsole.Write(rule);
    }
}