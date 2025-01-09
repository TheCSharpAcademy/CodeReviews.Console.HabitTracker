namespace HabitLoggerLibrary.Ui;

public sealed class ConsoleKeyAwaiter : IKeyAwaiter
{
    public void Wait()
    {
        Console.ReadKey();
    }
}