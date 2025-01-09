using FluentAssertions;
using HabitLoggerApp.Application.Handlers.Habits;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Habits;
using HabitLoggerLibrary.Ui.Input;
using NSubstitute;

namespace HabitLoggerAppTests.Application.Handlers.Habits;

public class EditHabitHandlerTests : IntegrationTests
{
    [Test]
    public void WillEditSelectedHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        var keyAwaiter = Substitute.For<IKeyAwaiter>();
        keyAwaiter.When(x => x.Wait()).Do(_ => { });

        var habitChoiceReader = Substitute.For<IHabitChoiceReader>();
        habitChoiceReader.GetChoice().Returns(1);

        var inputReader = Substitute.For<IInputReader>();
        inputReader.GetStringInput().Returns("new habit name", "new unit of measure");
        var inputReaderSelector = Substitute.For<IInputReaderSelector>();
        inputReaderSelector.GetInputReader().Returns(inputReader);

        var habit = repository.GetHabitById(1);
        habit.HabitName.Should().Be("Running");
        habit.UnitOfMeasure.Should().Be("kilometers");

        var handler = new EditHabitHandler(repository, keyAwaiter, habitChoiceReader, inputReaderSelector);
        handler.Handle();

        habit = repository.GetHabitById(1);
        habit.HabitName.Should().Be("new habit name");
        habit.UnitOfMeasure.Should().Be("new unit of measure");
    }
}