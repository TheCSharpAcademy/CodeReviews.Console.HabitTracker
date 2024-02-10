namespace HabitLogger.data_and_access;

/// <summary>
/// Record representing a record with habit.
/// </summary>
internal sealed record RecordWithHabit(int? Id, DateTime Date, int Quantity, string? HabitName, string? Unit);

/// <summary>
/// Represents a record for managing habits in the Habit Logger application.
/// </summary>
internal sealed record Habit(int Id, string Name, string Unit);