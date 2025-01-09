using FluentAssertions;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Menu;
using NSubstitute;

namespace HabitLoggerLibraryTests.Ui.Menu;

[TestFixture]
public class MainMenuChoiceReaderTests : ConsoleTest
{
    [Test]
    [TestCaseSource(nameof(_validCases))]
    public void WillReturnCorrectChoice((ConsoleKeyInfo input, MainMenuChoice expectedResult) valueTuple)
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadKey().Returns(valueTuple.input);
        var reader = new MainMenuChoiceReader(consoleWrapper);

        reader.GetChoice().Should().Be(valueTuple.expectedResult);
    }

    [Test]
    public void WillKeepOnWaitingForValidChoice()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadKey().Returns(new ConsoleKeyInfo('a', ConsoleKey.None, false, false, false),
            new ConsoleKeyInfo('b', ConsoleKey.None, false, false, false),
            new ConsoleKeyInfo('q', ConsoleKey.None, false, false, false));
        var reader = new MainMenuChoiceReader(consoleWrapper);

        reader.GetChoice().Should().Be(MainMenuChoice.Quit);
    }

    private static (ConsoleKeyInfo, MainMenuChoice)[] _validCases =
    [
        (new ConsoleKeyInfo('v', ConsoleKey.None, false, false, false), MainMenuChoice.ViewAllHabits),
        (new ConsoleKeyInfo('i', ConsoleKey.None, false, false, false), MainMenuChoice.InsertHabit),
        (new ConsoleKeyInfo('d', ConsoleKey.None, false, false, false), MainMenuChoice.DeleteHAbit),
        (new ConsoleKeyInfo('e', ConsoleKey.None, false, false, false), MainMenuChoice.EditHabit),
        (new ConsoleKeyInfo('q', ConsoleKey.None, false, false, false), MainMenuChoice.Quit)
    ];
}