using FluentAssertions;
using HabitLoggerApp.Application.Handlers.Habits;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Habits;
using NSubstitute;

namespace HabitLoggerAppTests.Application.Handlers.Habits;

[TestFixture]
public class DeleteHabitHandlerTests : IntegrationTests
{
    [SetUp]
    public void SetUp()
    {
        _keyAwaiter.When(x => x.Wait())
            .Do(_ => { });
    }

    private readonly IKeyAwaiter _keyAwaiter = Substitute.For<IKeyAwaiter>();

    [Test]
    public void WillDeleteSelectedHabit()
    {
        PopulateDatabase();
        var choiceReader = Substitute.For<IHabitChoiceReader>();
        choiceReader.GetChoice().Returns(2);

        var repository = CreateHabitsRepository();
        repository.HasHabitById(2).Should().BeTrue();

        var handler = new DeleteHabitHandler(choiceReader, CreateHabitsRepository(), _keyAwaiter);
        handler.Handle();

        repository.HasHabitById(2).Should().BeFalse();
    }

    [Test]
    public void WillNotAllowToChooseHabitForDeletionIfNoHabitsExist()
    {
        var choiceReader = Substitute.For<IHabitChoiceReader>();
        choiceReader.DidNotReceive().GetChoice();

        var handler = new DeleteHabitHandler(choiceReader, CreateHabitsRepository(), _keyAwaiter);
        handler.Handle();
    }
}