namespace HabitLogger;

public class EnumValidator<T> : IValidator<T> where T : struct, Enum
{
    public (bool, T) Validate(string input)
    {
        return (Enum.TryParse(input.Trim(), true, out T result) && Enum.IsDefined(typeof(T), result), result);
    }
}
