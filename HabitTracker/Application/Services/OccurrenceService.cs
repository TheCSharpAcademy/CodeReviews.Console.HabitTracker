using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Models;
using HabitTracker.Domain.Repositories;
using HabitTracker.Infrastructure.Repositories;

namespace HabitTracker.Application.Services;

public class OccurrenceService(IOccurrenceRepository occurrenceRepo,
                               IHabitRepository habitRepo)
{
    public IReadOnlyList<OccurrenceDisplayDto> GetAllOccurrences()
    {
        var occurrences = occurrenceRepo.GetAllOccurrences();
        var habitsDict = habitRepo.GetAllHabits().ToDictionary(habit => habit.Id, habit => habit.Name);

        var occurrencesDtos = occurrences.Select(occ => new OccurrenceDisplayDto(
            occ.Id,
   habitsDict.GetValueOrDefault(occ.HabitId, "unknown"),
            occ.Date)).ToList();

        return occurrencesDtos;
    }

    public IReadOnlyList<OccurrenceDisplayDto>? GetOccurrencesForHabit(int id)
    {
        var habit = habitRepo.GetHabitById(id);
        if (habit == null)
        {
            return null;
        }
        
        var occurrences = occurrenceRepo.GetOccurrencesByHabitId(id);

        var occurrencesDtos = occurrences.Select(occ => new OccurrenceDisplayDto(
            occ.Id,
            habit.Name,
            occ.Date)).ToList();

        return occurrencesDtos;
    }

    public bool CreateOccurrence(OccurrenceCreationDto occurrence)
    {
        return occurrenceRepo.CreateOccurrence(occurrence);
    }

    public bool DeleteOccurrenceById(int id)
    {
        return occurrenceRepo.DeleteOccurrenceById(id);
    }
}