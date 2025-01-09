namespace HabitLoggerLibrary.Ui;

public sealed class ConsoleWrapper : IConsoleWrapper
{
    public ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey(true);
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}