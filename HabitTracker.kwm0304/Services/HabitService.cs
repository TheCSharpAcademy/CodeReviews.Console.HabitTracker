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
    Console.WriteLine("Entering CreateHabit method.");
    if (IsEntryValid(name, unitOfMeasurement))
    {
      Habit habit = new(name, unitOfMeasurement);
      Console.WriteLine($"Creating habit: {habit.HabitName}, {habit.UnitOfMeasurement}");
      _repository.CreateHabit(habit);
      Utils.habitList.Add(habit);
      Console.WriteLine("Habit successfully created");
    }
    else
    {
      Console.WriteLine("Invalid habit entry");
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
  public Habit? GetHabit(int habitId)
  {
    try
    {
      return _repository.GetHabit(habitId);
    }
    catch (Exception e)
    {
      Console.WriteLine($"Error: {e.Message}");
      return null;
    }
  }
  public Habit? GetHabitFromList()
  {
    var habitList = _repository.GetHabits();
    if (habitList != null && habitList.Any())
    {
      Habit habit = AnsiConsole.Prompt(
        new SelectionPrompt<Habit>()
        .Title("Select the habit you would like to view/edit: ")
        .AddChoices(habitList)
      );
      return habit;
    }
    else
    {
      AnsiConsole.WriteLine("No habits found");
      return null;
    }
  }

  public void AddRepetitionsToHabit(Habit habit, int addedReps)
  {
    if (habit != null)
    {
      int id = habit.HabitId;
      _repository.UpdateHabitRepetitions(addedReps, id);
    }
  }
  public void EditHabbit(string field, object newValue, int habitId)
  {
    //string field, object newFieldValue, int id
    try
    {
      _repository.UpdateHabitFields(field, newValue, habitId);
    }
    catch (Exception e)
    {
      Console.WriteLine($"Error: {e.Message}");
    }
  }

  public void DeleteHabit(int id)
  {
    try
    {
      _repository.DeleteHabit(id);
    }
    catch (Exception e)
    {
      Console.WriteLine($"Error: {e.Message}");
    }
  }

  public static bool IsEntryValid(string name, string unitOfMeasurement)
  {
    if (!string.IsNullOrEmpty(name)
    && !string.IsNullOrEmpty(unitOfMeasurement)
    && name.Length > 3 && name.Length <= 20
    && unitOfMeasurement.Length <= 5)
    {
      return true;
    }
    return false;
  }
}