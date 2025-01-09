namespace HabitLoggerLibrary;

public record HabitLog(
    long Id,
    long HabitId,
    int Quantity,
    DateOnly HabitDate,
    string HabitName,
    string HabitUnitOfMeasure);