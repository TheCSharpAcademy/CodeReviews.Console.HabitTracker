namespace HabitLoggerLibrary.Ui.Input;

public sealed class InputReaderFactory(
    IConsoleWrapper consoleWrapper,
    ISpeechInputReaderFactory speechInputReaderFactory) : IInputReaderFactory
{
    public IInputReader Create(InputChoice choice)
    {
        return choice switch
        {
            InputChoice.ConsoleInput => new ConsoleInputReader(consoleWrapper),
            InputChoice.SpeechInput => speechInputReaderFactory.Create(),
            _ => throw new ArgumentOutOfRangeException(nameof(choice), choice, null)
        };
    }
}