namespace Nelson.Habit_Tracker.Utils
{
    public interface IInputValidator
    {
        string GetQualityInput();
        int ConvertToInt(string input);
        string? GetNameInput();
        string? GetMeasurementInput();
    }
}