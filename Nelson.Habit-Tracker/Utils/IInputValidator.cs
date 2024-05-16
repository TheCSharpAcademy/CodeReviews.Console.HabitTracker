namespace Nelson.Habit_Tracker.Utils
{
    public interface IInputValidator
    {
        int GetQualityInput();
        int ConvertToInt(string input);
        string? GetNameInput();
        string? GetMeasurementInput();
    }
}