using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Models;

namespace HabitTracker.Domain.Repositories;

public interface IOccurrenceRepository
{
    public IReadOnlyList<Occurrence> GetAllOccurrences();
    public IReadOnlyCollection<Occurrence> GetOccurrencesByHabitId(int habitId);
    public bool CreateOccurrence(OccurrenceCreationDto occurrence);
    public bool DeleteOccurrenceById(int id);
}