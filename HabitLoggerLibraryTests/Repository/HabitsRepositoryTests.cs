using FluentAssertions;
using HabitLoggerLibrary;
using LibraryRepository = HabitLoggerLibrary.Repository;

namespace HabitLoggerLibraryTests.Repository;

[TestFixture]
public class HabitsRepositoryTests : IntegrationTests
{
    [Test]
    public void CollectionIsEmptyWhenThereAreNoResults()
    {
        var repository = CreateHabitsRepository();
        repository.GetHabits().Should().BeEmpty();
    }

    [Test]
    public void CollectionContainsAllResultsInDb()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();
        repository.GetHabits().Should().HaveCount(3);
    }

    [Test]
    public void WillReturnHabitById()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        var habit = repository.GetHabitById(2);

        habit.Id.Should().Be(2);
        habit.HabitName.Should().Be("Swimming");
        habit.UnitOfMeasure.Should().Be("meters");
    }

    [Test]
    public void WillThrowExceptionIfHabitDoesNotExist()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        Action action = () => repository.GetHabitById(0);
        action.Should().Throw<LibraryRepository.HabitNotFoundException>();
    }

    [Test]
    public void WillReturnTrueIfHabitExists()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        repository.HasHabitById(1).Should().BeTrue();
    }

    [Test]
    public void WillReturnFalseIfHabitDoesNotExist()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        repository.HasHabitById(0).Should().BeFalse();
    }

    [Test]
    public void WIllDeleteExistingHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        repository.HasHabitById(1).Should().BeTrue();
        repository.DeleteHabitById(1);
        repository.HasHabitById(1).Should().BeFalse();
    }

    [Test]
    public void WillThrowExceptionIfTryingToDeleteNotExistingHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        var action = () => repository.DeleteHabitById(0);
        action.Should().Throw<LibraryRepository.HabitNotFoundException>();
    }

    [Test]
    public void ExistingHabitWillBeUpdated()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        var updatedHabit = repository.GetHabitById(1) with
        {
            HabitName = "updated_habit_name",
            UnitOfMeasure = "miles"
        };
        repository.UpdateHabit(updatedHabit);
        repository.GetHabitById(1).Should().BeEquivalentTo(updatedHabit);
    }

    [Test]
    public void WillThrowExceptionIfUpdatingNonExistingHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        var updatedHabit = repository.GetHabitById(1) with { Id = 10 };

        var action = () => repository.UpdateHabit(updatedHabit);
        action.Should().Throw<LibraryRepository.HabitNotFoundException>();
    }

    [Test]
    public void WillAddNewHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitsRepository();

        repository.HasHabitById(4).Should().BeFalse();
        var habit = repository.AddHabit(new HabitDraft("added_habit_name", "kilograms"));

        habit.Id.Should().Be(4);
        habit.HabitName.Should().Be("added_habit_name");
        habit.UnitOfMeasure.Should().Be("kilograms");
        repository.HasHabitById(4).Should().BeTrue();
    }

    [Test]
    public void WillGetCorrectHabitsCount()
    {
        var repository = CreateHabitsRepository();

        repository.GetHabits().Count().Should().Be(0);
        PopulateDatabase();
        repository.GetHabits().Count().Should().Be(3);
    }
}