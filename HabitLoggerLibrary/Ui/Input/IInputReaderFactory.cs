namespace HabitLoggerLibrary.Ui.Input;

public interface IInputReaderFactory
{
    public IInputReader Create(InputChoice choice);
}