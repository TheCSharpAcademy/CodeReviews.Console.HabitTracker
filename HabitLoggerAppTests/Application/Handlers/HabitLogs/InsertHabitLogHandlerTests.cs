using FluentAssertions;
using HabitLoggerApp.Application.Handlers.HabitLogs;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Habits;
using HabitLoggerLibrary.Ui.Input;
using NSubstitute;

namespace HabitLoggerAppTests.Application.Handlers.HabitLogs;

[TestFixture]
public class InsertHabitLogHandlerTests : IntegrationTests
{
    [Test]
    public void WillAddNewHabitLog()
    {
        var inputReaderSelector = Substitute.For<IInputReaderSelector>();
        var inputReader = Substitute.For<IInputReader>();
        inputReader.GetDateInput().Returns(new DateOnly(2024, 1, 2));
        inputReader.GetNumberInput().Returns(5);
        inputReaderSelector.GetInputReader().Returns(inputReader);

        var habitChoiceReader = Substitute.For<IHabitChoiceReader>();
        habitChoiceReader.GetChoice().Returns(1);

        var keyAwaiter = Substitute.For<IKeyAwaiter>();

        var habitRepository = CreateHabitsRepository();
        var habitLogsRepository = CreateHabitLogsRepository();

        var handler = new InsertHabitLogHandler(inputReaderSelector, habitChoiceReader, habitRepository,
            habitLogsRepository, keyAwaiter);

        PopulateDatabase();
        habitLogsRepository.HasHabitLogById(10).Should().BeFalse();
        handler.Handle();
        habitLogsRepository.HasHabitLogById(10).Should().BeTrue();
        var habitLog = habitLogsRepository.GetHabitLogById(10);
        habitLog.HabitDate.Should().Be(new DateOnly(2024, 1, 2));
        habitLog.Quantity.Should().Be(5);
    }
}