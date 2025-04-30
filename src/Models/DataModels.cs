namespace Golvi1124.HabitLogger.src.Models;

    // Record is a new C# 9 feature. It is a reference type that provides built-in functionality for encapsulating data.
    // It is immutable(cannot be changed after it is created) by default and provides value-based equality.
    public record RecordWithHabit(int Id, DateTime Date, string HabitName, int Quantity, string MeasurementUnit);

    public record Habit(int Id, string Name, string UnitOfMeasurement);

/* Nesting the RecordWithHabit type inside a class would have restricted its visibility, even with the public modifier. 
 By moving it out of the class and ensuring it is directly within the namespace, you made it accessible across your project.
*/
