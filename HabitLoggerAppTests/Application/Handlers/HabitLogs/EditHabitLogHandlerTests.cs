using FluentAssertions;
using HabitLoggerApp.Application.Handlers.HabitLogs;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.HabitLogs;
using HabitLoggerLibrary.Ui.Input;
using NSubstitute;

namespace HabitLoggerAppTests.Application.Handlers.HabitLogs;

[TestFixture]
public class EditHabitLogHandlerTests : IntegrationTests
{
    [Test]
    public void WillCorrectlyUpdatedSelectedHabitLog()
    {
        PopulateDatabase();

        var repository = CreateHabitLogsRepository();

        var choiceReader = Substitute.For<IHabitLogChoiceReader>();
        choiceReader.GetChoice().Returns(1);

        var readerSelector = Substitute.For<IInputReaderSelector>();
        var inputReader = Substitute.For<IInputReader>();
        inputReader.GetNumberInput().Returns(4);
        inputReader.GetDateInput().Returns(new DateOnly(2024, 2, 3));
        readerSelector.GetInputReader().Returns(inputReader);

        var keyAwaiter = Substitute.For<IKeyAwaiter>();

        var habit = repository.GetHabitLogById(1);
        habit.HabitDate.Should().Be(new DateOnly(2020, 1, 1));
        habit.Quantity.Should().Be(3);

        var handler = new EditHabitLogHandler(repository, choiceReader, readerSelector, keyAwaiter);
        handler.Handle();

        habit = repository.GetHabitLogById(1);
        habit.HabitDate.Should().Be(new DateOnly(2024, 2, 3));
        habit.Quantity.Should().Be(4);
    }
}