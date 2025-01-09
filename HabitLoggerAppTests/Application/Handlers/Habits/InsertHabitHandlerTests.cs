using FluentAssertions;
using HabitLoggerApp.Application.Handlers.Habits;
using HabitLoggerLibrary;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Input;
using NSubstitute;

namespace HabitLoggerAppTests.Application.Handlers.Habits;

public class InsertHabitHandlerTests : IntegrationTests
{
    [Test]
    public void WillAddHabit()
    {
        var inputReader = Substitute.For<IInputReader>();
        inputReader.GetStringInput().Returns("Some habit name", "kilometers");
        var inputReaderSelector = Substitute.For<IInputReaderSelector>();
        inputReaderSelector.GetInputReader().Returns(inputReader);

        var keyAwaiter = Substitute.For<IKeyAwaiter>();
        keyAwaiter.When(x => x.Wait()).Do(_ => { });
        var repository = CreateHabitsRepository();

        repository.HasHabitById(1).Should().BeFalse();
        var handler = new InsertHabitHandler(inputReaderSelector, repository, keyAwaiter);
        handler.Handle();
        repository.HasHabitById(1).Should().BeTrue();
        repository.GetHabitById(1).Should().BeEquivalentTo(new Habit(1, "Some habit name", "kilometers"));
    }
}