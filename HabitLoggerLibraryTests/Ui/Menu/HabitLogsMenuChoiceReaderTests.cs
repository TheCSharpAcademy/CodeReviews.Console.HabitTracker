using FluentAssertions;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Menu;
using NSubstitute;

namespace HabitLoggerLibraryTests.Ui.Menu;

[TestFixture]
public class HabitLogsMenuChoiceReaderTests : ConsoleTest
{
    [Test]
    [TestCaseSource(nameof(_valueTuples))]
    public void WillReturnCorrectChoice((ConsoleKeyInfo pressedKey, HabitLogsMenuChoice expectedChoice) testTuple)
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadKey().Returns(testTuple.pressedKey);

        var reader = new HabitLogsMenuChoiceReader(consoleWrapper);

        reader.GetChoice().Should().Be(testTuple.expectedChoice);
    }

    [Test]
    public void WillKeepAskingUntilValidChoiceIsProvided()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadKey().Returns(new ConsoleKeyInfo('a', ConsoleKey.None, false, false, false),
            new ConsoleKeyInfo('c', ConsoleKey.None, false, false, false),
            new ConsoleKeyInfo('b', ConsoleKey.None, false, false, false));

        var reader = new HabitLogsMenuChoiceReader(consoleWrapper);

        reader.GetChoice().Should().Be(HabitLogsMenuChoice.GoBack);
    }

    private static (ConsoleKeyInfo, HabitLogsMenuChoice)[] _valueTuples =
    [
        (new ConsoleKeyInfo('v', ConsoleKey.None, false, false, false), HabitLogsMenuChoice.ViewAllLogs),
        (new ConsoleKeyInfo('b', ConsoleKey.None, false, false, false), HabitLogsMenuChoice.GoBack),
        (new ConsoleKeyInfo('d', ConsoleKey.None, false, false, false), HabitLogsMenuChoice.DeleteLogEntry),
        (new ConsoleKeyInfo('i', ConsoleKey.None, false, false, false), HabitLogsMenuChoice.InsertLogEntry),
        (new ConsoleKeyInfo('e', ConsoleKey.None, false, false, false), HabitLogsMenuChoice.EditLogEntry)
    ];
}