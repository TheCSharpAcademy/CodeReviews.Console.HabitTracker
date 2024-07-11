using HabitTracker.kwm0304.Models;
using HabitTracker.kwm0304.Services;

namespace HabitTracker.kwm0304.Helpers;

public class Utils
{
  public static List<Habit> habitList = new List<Habit>();
  private readonly HabitService _service;
  private readonly HabitReport _report;
  public Utils()
  {
    _service = new HabitService();
    _report = new HabitReport(_service);
  }
  public void HandleMainMenuChoice(string choice)
  {
    switch (choice)
    {
      case "Add a new habit":
        HandleAddHabit();
        break;
      case "View existing habbits":
        HandleViewHabits();
        break;
      case "Exit":
        int exitResponse = Printer.ExitProgramPrompt();
        if (exitResponse == 0)
        {
          Environment.Exit(exitResponse);
        }
        break;
    }
  }

  private void HandleViewHabits()
  {
    Habit? habit =  _service.GetHabitFromList() 
    ?? throw new InvalidOperationException("No habits found");
    string action = Printer.PrintListMenu();
    HandleHabitAction(action, habit);
  }

  private void HandleHabitAction(string action, Habit habit)
  {
    int habitId = habit.HabitId;
    string name = habit.HabitName;
    string unit = habit.UnitOfMeasurement;
    switch (action)
    {
      case "Add repetitions":
        int reps = Printer.AddRepetitionsPrompt();
        _service.AddRepetitionsToHabit(habit, reps);
        break;
      case "Edit habit":
        string attribute = Printer.FieldEditPrompt();
        object newFieldVal = Printer.EditValuePrompt(attribute);
        _service.EditHabbit(attribute, newFieldVal, habitId);
        break;
        case "Generate Report":
        List<int> reportNums = _report.GenerateReports(habitId);
        Printer.PrintReports(reportNums, name, unit);
        break;

      case "Delete habit":
        bool confirmDelete = Printer.ConsoleConfirmation();
        if (confirmDelete)
        {
          _service.DeleteHabit(habitId);
        }
        break;
    }
  }

  private void HandleAddHabit()
  {
    string name = Printer.HabitAttributePrompt("What is the name of the habit you want to add?");
    string unitOfMeasurement = Printer.HabitAttributePrompt($"What is the abbreviated unit of measurement for {name}?");
    _service.CreatHabit(name, unitOfMeasurement);
  }
}
