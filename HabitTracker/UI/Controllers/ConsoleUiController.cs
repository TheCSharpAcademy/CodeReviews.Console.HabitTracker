using HabitTracker.Application.DTOs;
using HabitTracker.Application.Services;
using HabitTracker.Domain.Models;
using HabitTracker.UI.Interfaces;

namespace HabitTracker.UI.Controllers;

public class ConsoleUiController(HabitService habitService,
                                 OccurrenceService occurrenceService,
                                 IView view)
{
    private readonly Dictionary<int, string> _features = new Dictionary<int, string>
    {
        {1, "Create new habit"},
        {2, "Check on habit"},
        {3, "Log new habit occurrence"},
        {4, "Modify habit"},
        {5, "Delete habit"},
        {6, "Exit application"}
    };
    
    public void Initialize()
    {
        view.DisplayHeader();
    }

    public void Execute()
    {
        var selection = 0;
        do
        {
            view.DisplayMainMenuOptions(_features.Values);
            selection = view.GetUserChoice();
            view.ClearMessages();
            if (!_features.ContainsKey(selection))
            {
                view.DisplayMessage("This selection is not valid!");
            }
            else
            {
                HandleMainMenuSelection(selection);
            }
        } while (true);
    }

    private void HandleMainMenuSelection(int selection)
    {
        switch (selection)
        {
            case 1:
                HandleHabitCreation();
                break;
            case 2:
                HandleHabitDisplay();
                break;
            case 3:
                HandleOccurrenceCreation();
                break;
            case 4:
                HandleHabitUpdate();
                break;
            case 5:
                HandleHabitDelete();
                break;
            case 6:
                HandleExit();
                break;
            default:
                throw new NotImplementedException("This does not exist yet!");
        }
    }

    private void HandleHabitCreation()
    {
        var newHabit = view.GetNewHabit();
        var success = habitService.CreateHabit(newHabit);
        if (success) view.DisplayMessage("Habit creation successful!");
        else view.DisplayMessage("Habit creation failed!");
    }

    private void HandleHabitDisplay()
    {
        var habits = habitService.GetAllHabits();

        if (habits.Count > 0)
        {
            view.DisplayHabits(habits);
            var selection = GetHabitSelection(habits);
            var habit = habitService.GetHabit(selection);
            var occurrences = occurrenceService.GetOccurrencesForHabit(selection);
            OccurrenceDisplayDto? lastOccurrence = null;
            if (occurrences!.Count > 0) lastOccurrence = occurrences[^1];
            if (habit == null) view.DisplayMessage("Habit fetch failed!");
            else view.DisplayHabit(habit, lastOccurrence);
        }
        else
        {
            view.DisplayMessage("There are no habits to display!");
        }
    }


    private void HandleOccurrenceCreation()
    {
        var habits = habitService.GetAllHabits();
        
        if (habits.Count > 0)
        {
            view.DisplayHabits(habits);
            var selection = GetHabitSelection(habits);
            var occurrence = view.GetUserOccurrence(selection);
            var success = occurrenceService.CreateOccurrence(occurrence);
            if (success) view.DisplayMessage("Occurrence creation successful!");
            else view.DisplayMessage("Occurrence creation failed!");
        }
        else
        {
            view.DisplayMessage("There are no habits to add occurrences to!");
        }
    }

    private void HandleHabitUpdate()
    {
        var habits = habitService.GetAllHabits();
        
        if (habits.Count > 0)
        {
            view.DisplayHabits(habits);
            var selection = GetHabitSelection(habits);
            var habitToUpdate = habits.Single(h => h.Id == selection);
            var userUpdates = view.GetUserHabitModification();
            var updatedHabit = new HabitUpdateDto(userUpdates.Name ?? habitToUpdate.Name,
                userUpdates.Unit ?? habitToUpdate.Unit);
            var success = habitService.UpdateHabit(selection, updatedHabit);
            if (success) view.DisplayMessage("Habit update successful!");
            else view.DisplayMessage("Habit update failed!");
        }
        else
        {
            view.DisplayMessage("There are no habits to update!");
        }
    }

    private void HandleHabitDelete()
    {
        var habits = habitService.GetAllHabits();

        if (habits.Count > 0)
        {
            view.DisplayHabits(habits);
            var selection = GetHabitSelection(habits);
            var success = habitService.DeleteHabit(selection);
            if (success) view.DisplayMessage("Habit deletion successful!");
            else view.DisplayMessage("Habit deletion failed!");
        }
        else
        {
            view.DisplayMessage("There are no habits to delete!");
        }
    }

    private void HandleExit()
    {
        view.DisplayMessage("Goodbye!");
        Environment.Exit(0);
    }

    private int GetHabitSelection(IReadOnlyList<HabitDisplayDto> habits)
    {
        var selection = 0;
        do
        {
            selection = view.GetUserChoice();
            var exists = Enumerable.Range(1, habits.Count).Contains(selection);
            if (!exists) view.DisplayMessage("This selection is not valid!");
            else return habits[selection - 1].Id;
        } while (true);
    }
}