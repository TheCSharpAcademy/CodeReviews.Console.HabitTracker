using FunRun.HabitTracker.Data.Model;
using FunRun.HabitTracker.Services.Interfaces;
using Spectre.Console;
using System.Data;
using System.Data.Common;


namespace FunRun.HabitTracker;

public class HabitTrackerApp
{
    private ICrudService _crud;

    public HabitTrackerApp(ICrudService crud)
    {
        _crud = crud;
    }

    public async Task RunApp()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("HabitTracker").Centered().Color(Color.Blue));

            AnsiConsole.MarkupLine("[blue] Inpired by the [link=https://thecsharpacademy.com/project/12/habit-logger]C#Acadamy [/][/]");
            AnsiConsole.MarkupLine("");


            var habits = _crud.ReturnAllHabits();
            habits.Add(new HabitModel(0, "[dim]> Create a new Habit <[/]", "[dim]Select me and press [[Enter]][/]", 0));

            var table = new Table().Centered();
            table.Border = TableBorder.Rounded;

            table.AddColumn(" Id").Centered();
            table.AddColumn("Name");
            table.AddColumn("Description");
            table.AddColumn("Counter");

            int selectedIndex = 0;
            bool exit = false;
            HabitModel selectedHabit = null;

            await AnsiConsole.Live(table)
                .Overflow(VerticalOverflow.Ellipsis)
                .StartAsync(async ctx =>
                {
                    while (!exit)
                    {
                        table.Rows.Clear();
                        table.Title("[[ [green]Habit Overview [/]]]");
                        table.Caption("[[[blue] [[Up/Down]] Navigation, [[Enter]] Select, [[ESC]] Escape[/]]]");

                        for (int i = 0; i < habits.Count; i++)
                        {
                            var habit = habits[i];
                            if (i == selectedIndex)
                            {
                                table.AddRow(
                                    $"[blue]>{habit.Id}[/]",
                                    $"[blue]{habit.HabitName}[/]",
                                    $"[blue]{habit.HabitDescription}[/]",
                                    $"[blue]{habit.HabitCounter}[/]"
                                );
                            }
                            else
                            {
                                table.AddRow(
                                    habit.Id.ToString(),
                                    habit.HabitName,
                                    habit.HabitDescription,
                                    habit.HabitCounter.ToString()
                                );
                            }
                        }


                        ctx.Refresh();

                        var key = Console.ReadKey(true).Key;

                        switch (key)
                        {
                            case ConsoleKey.Escape:
                                exit = true;
                                break;

                            case ConsoleKey.UpArrow:
                                selectedIndex--;
                                if (selectedIndex < 0)
                                    selectedIndex = habits.Count - 1;
                                break;

                            case ConsoleKey.DownArrow:
                                selectedIndex++;
                                if (selectedIndex >= habits.Count)
                                    selectedIndex = 0;
                                break;

                            case ConsoleKey.Enter:

                                selectedHabit = habits[selectedIndex];
                                exit = true;
                                break;
                        }
                    }
                });


            if (selectedHabit != null)
            {
                if (selectedHabit.Id == 0 && selectedHabit.HabitName == "[dim]> Create a new Habit <[/]")
                {
                    var newHabitInput = InputCreateHabit();
                    _crud.CreateOneHabit(newHabitInput);
                }
                else
                {
                    var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .PageSize(10)
                            .AddChoices(new[] {
                                "Update", "Delete", "Back"
                            }));

                    if (choice == "Update")
                    {
                        var newHabitInput = InputCreateHabit();
                        newHabitInput.Id = selectedHabit.Id;
                        _crud.UpdateOneHabit(newHabitInput);
                    }
                    if (choice == "Delete")
                    {
                        var confirmation = AnsiConsole.Prompt(
                            new TextPrompt<bool>($"[yellow]Are you sure you want to [red]Delete[/] the Habit: [underline] {selectedHabit.HabitName}[/][/]")
                                .AddChoice(true)
                                .AddChoice(false)
                                .DefaultValue(false)
                                .WithConverter(choice => choice ? "y" : "n"));

                        if (confirmation)
                        {
                            _crud.DeleteOneHabit(selectedHabit);
                        }
                    }
                }
            }
            else 
            {
                    var confirmation = AnsiConsole.Prompt(
            new TextPrompt<bool>($"[yellow]Are you sure you want to Close the App[/]")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "y" : "n"));

                    if (confirmation)
                    {
                        break;
                    }
            }
        }

    }



    private HabitModel InputCreateHabit()
    {
        AnsiConsole.MarkupLine("[yellow]Each Habit consists of a name, a description, and an integer counter.[/]");

        var habitName = AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]Enter Habit Name (max 100 chars):[/]")
                .Validate(name =>
                {
                    if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
                        return ValidationResult.Error("[red]Please enter a valid name up to 100 characters![/]");
                    return ValidationResult.Success();
                }));

        var habitDescription = AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]Enter Habit Description (max 250 chars):[/]")
                .Validate(desc =>
                {
                    if (string.IsNullOrWhiteSpace(desc) || desc.Length > 250)
                        return ValidationResult.Error("[red]Please enter a valid description up to 250 characters![/]");
                    return ValidationResult.Success();
                }));

        var habitCounter = AnsiConsole.Prompt(
            new TextPrompt<int>("[yellow]Enter the initial counter (32-bit integer):[/]")
                .ValidationErrorMessage("[red]That's not a valid 32-bit integer![/]"));

        var habit = new HabitModel(0, habitName, habitDescription, habitCounter);

        return habit;
    }

}
