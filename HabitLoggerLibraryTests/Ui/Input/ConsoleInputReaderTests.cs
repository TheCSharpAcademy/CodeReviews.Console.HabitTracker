using FluentAssertions;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Input;
using NSubstitute;

namespace HabitLoggerLibraryTests.Ui.Input;

public class ConsoleInputReaderTests : ConsoleTest
{
    [Test]
    public void WillReturnProvidedStringInput()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("Provided text");

        var reader = new ConsoleInputReader(consoleWrapper);
        reader.GetStringInput().Should().Be("Provided text");
    }

    [Test]
    public void WillKeepAskingForStringInputUntilProvidedInputAfterTrimIsNotEmpty()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("", " ", "    ", "Provided text");

        var reader = new ConsoleInputReader(consoleWrapper);
        reader.GetStringInput().Should().Be("Provided text");
    }

    [Test]
    public void WillReturnProvidedNumberInput()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("22");

        var reader = new ConsoleInputReader(consoleWrapper);
        reader.GetNumberInput().Should().Be(22);
    }

    [Test]
    public void WillKeepAskingForNumberInputUntilNumberProvided()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("", "23s", "abcd", "22");

        var reader = new ConsoleInputReader(consoleWrapper);
        reader.GetNumberInput().Should().Be(22);
    }

    [Test]
    public void WillReturnProvidedDateOnlyInput()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("2024.01.02");

        var reader = new ConsoleInputReader(consoleWrapper);
        reader.GetDateInput().Should().Be(new DateOnly(2024, 1, 2));
    }

    [Test]
    public void WillKeepAskingForDateOnlyInputUntilDateProvided()
    {
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("", "23s", "abcd", "2024.01.02");

        var reader = new ConsoleInputReader(consoleWrapper);
        reader.GetDateInput().Should().Be(new DateOnly(2024, 1, 2));
    }
}