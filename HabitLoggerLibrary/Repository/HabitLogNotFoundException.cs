namespace HabitLoggerLibrary.Repository;

public sealed class HabitLogNotFoundException(long id) : Exception($"Habit log with id: {id} was not found.");