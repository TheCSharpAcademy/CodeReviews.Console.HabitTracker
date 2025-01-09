namespace HabitLoggerLibrary.Ui.Input;

public interface IInputReader
{
    public string GetStringInput();

    public long GetNumberInput();

    public DateOnly GetDateInput();
}