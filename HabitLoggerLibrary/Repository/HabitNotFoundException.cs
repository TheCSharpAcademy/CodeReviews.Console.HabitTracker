namespace HabitLoggerLibrary.Repository;

public sealed class HabitNotFoundException(long id) : Exception($"Habit with id: {id} was not found.");