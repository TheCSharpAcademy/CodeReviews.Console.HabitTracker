using FluentAssertions;
using HabitLoggerApp.Application.Handlers.HabitLogs;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.HabitLogs;
using NSubstitute;

namespace HabitLoggerAppTests.Application.Handlers.HabitLogs;

[TestFixture]
public class DeleteHabitLogHandlerTests : IntegrationTests
{
    [Test]
    public void WillDeleteSelectedHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();
        var choiceReader = Substitute.For<IHabitLogChoiceReader>();
        choiceReader.GetChoice().Returns(1);
        var keyAwaiter = Substitute.For<IKeyAwaiter>();
        keyAwaiter.When(x => x.Wait()).Do(_ => { });

        repository.HasHabitLogById(1).Should().Be(true);
        var handler = new DeleteHabitLogHandler(repository, choiceReader, keyAwaiter);
        handler.Handle();
        repository.HasHabitLogById(1).Should().Be(false);
    }
}