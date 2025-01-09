using System.Text;

namespace HabitLoggerLibraryTests;

public abstract class ConsoleTest
{
    private readonly TextWriter _currentConsoleWriter = Console.Out;

    [SetUp]
    public void SetUp()
    {
        var consoleOutput = new StringBuilder();
        Console.SetOut(new StringWriter(consoleOutput));
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(_currentConsoleWriter);
    }
}