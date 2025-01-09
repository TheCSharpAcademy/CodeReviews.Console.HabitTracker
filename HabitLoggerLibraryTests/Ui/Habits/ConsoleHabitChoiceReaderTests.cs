using FluentAssertions;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Habits;
using NSubstitute;

namespace HabitLoggerLibraryTests.Ui.Habits;

[TestFixture]
public class ConsoleHabitChoiceReaderTests : IntegrationTests
{
    [Test]
    public void WillReturnCorrectValue()
    {
        PopulateDatabase();
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("a", "ab", "12.23", "22,11", "1");

        var reader = new ConsoleHabitChoiceReader(consoleWrapper, CreateHabitsRepository());
        reader.GetChoice().Should().Be(1);
    }

    [Test]
    public void WillKeepAskingForValueUntilExistingIsFound()
    {
        PopulateDatabase();
        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("99", "89", "79", "69", "1");

        var reader = new ConsoleHabitChoiceReader(consoleWrapper, CreateHabitsRepository());
        reader.GetChoice().Should().Be(1);
    }
}