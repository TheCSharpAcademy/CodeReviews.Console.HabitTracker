namespace HabitLogger;

public class IntValidator : IValidator<int>
{
    public (bool, int) Validate(string input)
    {
        return (int.TryParse(input, out int result), result);
    }
}
