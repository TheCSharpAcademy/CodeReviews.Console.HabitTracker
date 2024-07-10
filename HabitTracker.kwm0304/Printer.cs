

using System.Text.RegularExpressions;
using HabitTracker.kwm0304.Models;
using Spectre.Console;

namespace HabitTracker.kwm0304;

public class Printer
{
  public static string? PrintMainMenu()
  {
    Console.Clear();
    PrintHeader();
    return PrintMenuOptions();
  }

  private static string PrintMenuOptions()
  {
    string choice = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("Welcome to Habit Tracker, what would you like to do?")
      .AddChoices("Add a new habit", "View existing habbits", "Exit")
    );
    return choice;
  }

  private static void DisplayCurrentHabitsMenu()
  {
    DisplayHabits();
    throw new NotImplementedException();
  }

  private static bool IsValidOption(string response)
  {
    throw new NotImplementedException();
  }

  private static void PrintHeader()
  {
    AnsiConsole.WriteLine(@"
 __    __       ___      .______    __  .___________.   .___________..______          ___       ______  __  ___  _______ .______      
|  |  |  |     /   \     |   _  \  |  | |           |   |           ||   _  \        /   \     /      ||  |/  / |   ____||   _  \     
|  |__|  |    /  ^  \    |  |_)  | |  | `---|  |----`   `---|  |----`|  |_)  |      /  ^  \   |  ,----'|  '  /  |  |__   |  |_)  |    
|   __   |   /  /_\  \   |   _  <  |  |     |  |            |  |     |      /      /  /_\  \  |  |     |    <   |   __|  |      /     
|  |  |  |  /  _____  \  |  |_)  | |  |     |  |            |  |     |  |\  \----./  _____  \ |  `----.|  .  \  |  |____ |  |\  \----.
|__|  |__| /__/     \__\ |______/  |__|     |__|            |__|     | _| `._____/__/     \__\ \______||__|\__\ |_______|| _| `._____|
                                                                                                                                      
");
  }

  public static string PrintListMenu()
  {
    string action = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("What would you like to do?")
      .AddChoices("Add repetitions", "Edit habit", "Delete habit", "Generate Report", "Back to habit list", "Back to main menu")
    );
    return action;
  }

  public static string HabitAttributePrompt(string prompt)
  {
    Console.WriteLine($"{prompt}");
    string response = Console.ReadLine() ?? string.Empty;
    if (string.IsNullOrEmpty(response))
    {
      Console.WriteLine("Response cannot be empty");
      HabitAttributePrompt(prompt);
    }
    return response;
  }

  private static void DisplayHabits()
  {
    var table = new Table();
    table.Title("Habits");
    table.Border(TableBorder.Heavy);
    table.AddColumns("Id", "Name", "Repetitions", "Unit of Measurement", "Started On");
    foreach (var habit in Utils.habitList)
    {
      table.AddRow($"{habit.HabitId}, {habit.HabitName}, {habit.Repetitions}, {habit.UnitOfMeasurement}, {habit.StartedOn}");
    }
    AnsiConsole.Write(table);
  }

  public static int AddRepetitionsPrompt()
  {
    return AnsiConsole.Prompt(
      new TextPrompt<int>("How many repetitions are you adding?")
      .ValidationErrorMessage("[red]Must be a number[/]")
      .Validate(num =>
      {
        if (num < 0)
        {
          return ValidationResult.Error("[red]Number must be non-negative[/]");
        }
        return ValidationResult.Success();
      })
    );
  }
  public static string FieldEditPrompt()
  {
    string field = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("Which attribute would you like to edit?")
      .AddChoices("HabitName", "UnitOfMeasurement", "Repetitions")
    );
    return field;
  }
  public static object EditValuePrompt(string field)
  {
    if (field != "Repetitions")
    {
      string choice = AnsiConsole.Prompt(
        new TextPrompt<string>("What would you like to change it to?")
      );
      return (object)choice;
    }
    else
    {
      int repetitions = AnsiConsole.Prompt(
        new TextPrompt<int>("What would you like to change it to?")
        .Validate(num =>
        {
          if (num < 0)
          {
            return ValidationResult.Error("[red]Number must be non-negative[/]");
          }
          return ValidationResult.Success();
        })
      );
      return (object)repetitions;
    }
  }
  public static bool ConsoleConfirmation()
  {
    return AnsiConsole.Confirm("Are you sure?");
  }
  public static int ExitProgramPrompt()
  {
    int response = AnsiConsole.Prompt(
      new TextPrompt<int>("To exit the program, type 0 and press Enter")
    );
    return response;
  }
  public static void PrintReports(List<int> projectionsList, string name, string unit)
  {
    if (projectionsList[0] != 0)
    {
    AnsiConsole.MarkupLine($"{name} Report");
    AnsiConsole.WriteLine($"You are averaging {projectionsList[0]} {unit} per week");
    AnsiConsole.WriteLine($"You are averaging {projectionsList[1]} {unit} per month");
    AnsiConsole.WriteLine($"You are averaging {projectionsList[2]} {unit} per year");
    }
    else
    {
      AnsiConsole.WriteLine("Cannot generate report before a full day.");
    }
  }
}
