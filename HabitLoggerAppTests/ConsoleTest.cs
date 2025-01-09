using System.Text;

namespace HabitLoggerAppTests;

public abstract class ConsoleTest
{
    private readonly TextWriter _currentConsoleWriter = Console.Out;

    [SetUp]
    public void SetUpConsoleOutput()
    {
        var consoleOutput = new StringBuilder();
        Console.SetOut(new StringWriter(consoleOutput));
    }

    [TearDown]
    public void RestoreConsoleOutput()
    {
        Console.SetOut(_currentConsoleWriter);
    }
}