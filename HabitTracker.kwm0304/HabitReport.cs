using HabitTracker.kwm0304.Models;
using HabitTracker.kwm0304.Repositories;
using HabitTracker.kwm0304.Services;

namespace HabitTracker.kwm0304;

public class HabitReport
{
  private readonly HabitService _service;
  public HabitReport(HabitService service)
  {
    _service = service;
  }
  public List<int> GenerateReports(int habitId)
  {
    List<int> projectedOutput = new List<int>();
    Habit? habit = _service.GetHabit(habitId) ?? null;
    int daysTracked = habit!.DaysTracked;
    int currentReps = habit.Repetitions;
    double averageDailyReps = daysTracked > 0 ? (double)currentReps / daysTracked : 0;
    int weeklyPace = (int)(averageDailyReps * 7);
    int monthlyPace = (int)(averageDailyReps * 30);
    int yearlyPace = (int)(averageDailyReps * 365);
    projectedOutput.Add(weeklyPace);
    projectedOutput.Add(monthlyPace);
    projectedOutput.Add(yearlyPace);
    return projectedOutput;
  }
}
