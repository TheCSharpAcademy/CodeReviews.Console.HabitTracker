namespace HabitLogger;

public class StringValidator : IValidator<string>
{
    public (bool, string) Validate(string input)
    {
        return (!string.IsNullOrEmpty(input), input);
    }
}
