using HabitTracker.Application.DTOs;

namespace HabitTracker.UI.Interfaces;

public interface IView
{
    public void DisplayHeader();
    public void DisplayMainMenuOptions(IEnumerable<string> options);
    public HabitCreationDto GetNewHabit();
    public void DisplayHabits(IEnumerable<HabitDisplayDto> habits);
    public void DisplayHabit(HabitDisplayDto habit, OccurrenceDisplayDto? lastOccurrence);
    public void DisplayOccurrences(IEnumerable<OccurrenceDisplayDto> occurrences);
    public OccurrenceCreationDto GetUserOccurrence(int habitId);
    public HabitUpdateDto GetUserHabitModification();
    public int GetUserChoice();
    public void DisplayMessage(string message);
    public void ClearMessages();

}