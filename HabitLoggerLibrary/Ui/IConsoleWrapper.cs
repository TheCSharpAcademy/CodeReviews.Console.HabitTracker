namespace HabitLoggerLibrary.Ui;

public interface IConsoleWrapper
{
    public ConsoleKeyInfo ReadKey();

    public string? ReadLine();
}