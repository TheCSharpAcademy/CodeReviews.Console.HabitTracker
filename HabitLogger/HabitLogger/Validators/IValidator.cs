namespace HabitLogger;

public interface IValidator<T>
{
    (bool, T) Validate(string? input);
}
