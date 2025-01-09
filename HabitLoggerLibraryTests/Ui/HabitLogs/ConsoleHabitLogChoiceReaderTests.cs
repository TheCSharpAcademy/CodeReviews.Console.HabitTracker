using FluentAssertions;
using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.HabitLogs;
using NSubstitute;

namespace HabitLoggerLibraryTests.Ui.HabitLogs;

[TestFixture]
public class ConsoleHabitLogChoiceReaderTests : IntegrationTests
{
    [Test]
    public void WillReturnSelectedHabitLog()
    {
        var repository = Substitute.For<IHabitLogsRepository>();
        repository.HasHabitLogById(1).Returns(true);

        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("1");

        var reader = new ConsoleHabitLogChoiceReader(repository, consoleWrapper);
        reader.GetChoice().Should().Be(1);
    }

    [Test]
    public void WillWaitUntilExistingLogIsGiven()
    {
        var repository = Substitute.For<IHabitLogsRepository>();
        repository.HasHabitLogById(1).Returns(true);

        var consoleWrapper = Substitute.For<IConsoleWrapper>();
        consoleWrapper.ReadLine().Returns("100", "a", "1.1", "1");

        var reader = new ConsoleHabitLogChoiceReader(repository, consoleWrapper);
        reader.GetChoice().Should().Be(1);
    }
}