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

            if (habits.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey] Oohh... seems like you dont have any Habits right now...[/]");
                AnsiConsole.MarkupLine("[yellow] You need to have atleast one Habit. Please follow the intructions[/]");
                var input = InputCreateHabit();
                _crud.CreateOneHabit(input);

            }

            habits = _crud.ReturnAllHabits();
            habits.Add(new HabitModel(0, "Create a new habit", "Press Enter", 999));

            var table = new Table();
            table.Border = TableBorder.Rounded;

            table.AddColumn("Id");
            table.AddColumn("Name");
            table.AddColumn("Description");
            table.AddColumn("Counter");

            int selectedIndex = 0;
            bool exit = false;
            HabitModel selectedHabit = null;

            //TODO: TESTING
            await AnsiConsole.Live(table)
                .StartAsync(async ctx =>
                {
                    while (!exit)
                    {

                        table.Rows.Clear();


                        for (int i = 0; i < habits.Count; i++)
                        {
                            var habit = habits[i];
                            if (i == selectedIndex)
                            {

                                table.AddRow(
                                    $"[blue]{habit.Id}[/]",
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


                        //AnsiConsole.MarkupLine("↑/↓ Navigation, [green]Enter[/] Select, [red]ESC[/] Escape.");

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
                AnsiConsole.MarkupLine($"Selected: [blue]{selectedHabit.HabitName}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("Nothing Selected!");
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
