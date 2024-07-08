using HabitTracker.kwm0304.Models;
using HabitTracker.kwm0304.Repositories;
using Spectre.Console;
namespace HabitTracker.kwm0304.Services;

public class HabitService
{
  private readonly HabitRepository _repository;
  public HabitService()
  {
    _repository = new HabitRepository();
  }
  public void CreatHabit(string name, string unitOfMeasurement)
  {
    if (IsEntryValid(name, unitOfMeasurement))
    {
      Habit habit = new Habit(name, unitOfMeasurement);
      _repository.CreateHabit(habit);
      Console.WriteLine("Habit successfully created");
    }
  }
  public void GetHabitById(int habitId)
  {
    var habit = _repository.GetHabit(habitId);
    if (habit != null)
    {
      var table = new Table();
      table.Title("[yellow3_1]Habit[/]");
      table.Border(TableBorder.Heavy);
      table.AddColumns("Id", "Name", "Repetitions", "Unit of Measurement", "Started On");
      table.AddRow($"{habit.HabitId}, {habit.HabitName}, {habit.Repetitions}, {habit.UnitOfMeasurement}, {habit.StartedOn}");
      AnsiConsole.Write(table);
    }
    else
    {
      AnsiConsole.WriteLine("No habit found with this id");
    }

  }
  public void GetAllHabits()
  {
    var habitList = _repository.GetHabits();
    if (habitList != null && habitList.Any())
    {
      int index = 0;
      var table = new Table();
      table.Title("[yellow3_1]All Habits[/]");
      table.Border(TableBorder.Heavy);
      table.AddColumns("Id", "Name", "Repetitions", "Unit of Measurement", "Started On");
      foreach (Habit habit in habitList)
      {
        table.AddRow(index.ToString(), habit.HabitName, habit.Repetitions.ToString(), habit.UnitOfMeasurement, habit.StartedOn.ToString("yyyy-MM-dd"));
      }
      AnsiConsole.Write(table);
    }
    else
    {
      AnsiConsole.WriteLine("No habits found");
    }
  }
  public static bool IsEntryValid(string name, string unitOfMeasurement)
  {
    if (!string.IsNullOrEmpty(name)
    && !string.IsNullOrEmpty(unitOfMeasurement)
    && name.Length < 3 && name.Length <= 20
    && unitOfMeasurement.Length <= 5)
    {
      return true;
    }
    return false;
  }
}