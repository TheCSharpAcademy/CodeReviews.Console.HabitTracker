using FluentAssertions;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Input;
using NSubstitute;

namespace HabitLoggerLibraryTests.Ui.Input;

public class ConsoleInputChoiceReaderTests : ConsoleTest
{
    private static (ConsoleKeyInfo input, InputChoice expectedChoice)[] _validChoices =
    [
        (new ConsoleKeyInfo('c', ConsoleKey.None, false, false, false), InputChoice.ConsoleInput),
        (new ConsoleKeyInfo('s', ConsoleKey.None, false, false, false), InputChoice.SpeechInput)
    ];

    [TestCaseSource(nameof(_validChoices))]
    public void WillReturnCorrectChoice((ConsoleKeyInfo input, InputChoice expectedChoice) testTuple)
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadKey().Returns(testTuple.input);

        var reader = new ConsoleInputChoiceReader(consoleWrapper);
        reader.GetChoice().Should().Be(testTuple.expectedChoice);
    }

    [Test]
    public void WillWaitForWalidInput()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadKey().Returns(new ConsoleKeyInfo(' ', ConsoleKey.Backspace, false, false, false),
            new ConsoleKeyInfo('a', ConsoleKey.Backspace, false, false, false),
            new ConsoleKeyInfo('1', ConsoleKey.Backspace, false, false, false),
            new ConsoleKeyInfo('c', ConsoleKey.Backspace, false, false, false));

        var reader = new ConsoleInputChoiceReader(consoleWrapper);
        reader.GetChoice().Should().Be(InputChoice.ConsoleInput);
    }
}