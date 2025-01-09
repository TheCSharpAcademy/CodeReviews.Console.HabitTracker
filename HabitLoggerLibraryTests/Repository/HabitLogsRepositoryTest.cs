using FluentAssertions;
using HabitLoggerLibrary;
using LibraryRepository = HabitLoggerLibrary.Repository;

namespace HabitLoggerLibraryTests.Repository;

[TestFixture]
public class HabitLogsRepositoryTest : IntegrationTests
{
    [Test]
    public void CollectionIsEmptyWhenThereAreNoResults()
    {
        var repository = CreateHabitLogsRepository();
        repository.GetHabitLogs().Should().BeEmpty();
    }

    [Test]
    public void CollectionContainsAllResultsInDb()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();
        repository.GetHabitLogs().Should().HaveCount(9);
    }

    [Test]
    public void WillReturnHabitLogById()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        var habit = repository.GetHabitLogById(4);

        habit.Id.Should().Be(4);
        habit.HabitId.Should().Be(2);
        habit.Quantity.Should().Be(6);
        habit.HabitDate.Should().Be(new DateOnly(2020, 4, 1));
        habit.HabitName.Should().Be("Swimming");
        habit.HabitUnitOfMeasure.Should().Be("meters");
    }

    [Test]
    public void WillThrowExceptionIfHabitDoesNotExist()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        Action action = () => repository.GetHabitLogById(0);
        action.Should().Throw<LibraryRepository.HabitLogNotFoundException>();
    }

    [Test]
    public void WillReturnTrueIfHabitExists()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        repository.HasHabitLogById(1).Should().BeTrue();
    }

    [Test]
    public void WillReturnFalseIfHabitDoesNotExist()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        repository.HasHabitLogById(0).Should().BeFalse();
    }

    [Test]
    public void WIllDeleteExistingHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        repository.HasHabitLogById(1).Should().BeTrue();
        repository.DeleteHabitLogById(1);
        repository.HasHabitLogById(1).Should().BeFalse();
    }

    [Test]
    public void WillThrowExceptionIfTryingToDeleteNotExistingHabitLog()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        var action = () => repository.DeleteHabitLogById(0);
        action.Should().Throw<LibraryRepository.HabitLogNotFoundException>();
    }

    [Test]
    public void ExistingHabitWillBeUpdated()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        var updatedHabit = repository.GetHabitLogById(1) with
        {
            Quantity = 55,
            HabitDate = new DateOnly(2024, 2, 12)
        };
        repository.UpdateHabitLog(updatedHabit);
        repository.GetHabitLogById(1).Should().BeEquivalentTo(updatedHabit);
    }

    [Test]
    public void WillThrowExceptionIfUpdatingNonExistingHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        var updatedHabit = repository.GetHabitLogById(1) with { Id = 100 };

        var action = () => repository.UpdateHabitLog(updatedHabit);
        action.Should().Throw<LibraryRepository.HabitLogNotFoundException>();
    }

    [Test]
    public void WillAddNewHabit()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        repository.HasHabitLogById(10).Should().BeFalse();
        var habit = repository.AddHabitLog(new HabitLogDraft(1, 21, new DateOnly(2024, 2, 22)));

        habit.Id.Should().Be(10);
        habit.HabitId.Should().Be(1);
        habit.Quantity.Should().Be(21);
        repository.HasHabitLogById(10).Should().BeTrue();
    }

    [Test]
    public void WillGetCorrectHabitsCount()
    {
        var repository = CreateHabitLogsRepository();

        repository.GetHabitLogs().Count().Should().Be(0);
        PopulateDatabase();
        repository.GetHabitLogs().Count().Should().Be(9);
    }

    [Test]
    public void WillReturnTrueIfHabitLogByIdAndDateExists()
    {
        PopulateDatabase();
        var repository = CreateHabitLogsRepository();

        repository.HasHabitLogByHabitIdAndDate(1, new DateOnly(2024, 2, 22)).Should().BeFalse();
        repository.HasHabitLogByHabitIdAndDate(1, new DateOnly(2020, 1, 1)).Should().BeTrue();
    }

    [Test]
    public void WillGetStatisticsForGivenPeriod()
    {
        PopulateDatabase();

        var repository = CreateHabitLogsRepository();

        var statistics = repository.GetStatistics("%Y");
        statistics.Count.Should().Be(3);

        statistics[0].Should()
            .BeEquivalentTo(new Statistic("Running", "2020", 12, "kilometers"));
        statistics[1].Should()
            .BeEquivalentTo(new Statistic("Swimming", "2020", 21, "meters"));
        statistics[2].Should()
            .BeEquivalentTo(new Statistic("Drinking water", "2020", 30, "glasses"));

        statistics = repository.GetStatistics("%Y-%m");
        statistics.Count.Should().Be(9);

        statistics[0].Should()
            .BeEquivalentTo(new Statistic("Running", "2020-01", 3, "kilometers"));
        statistics[1].Should()
            .BeEquivalentTo(new Statistic("Running", "2020-02", 4, "kilometers"));
        statistics[2].Should()
            .BeEquivalentTo(new Statistic("Running", "2020-03", 5, "kilometers"));
        statistics[3].Should()
            .BeEquivalentTo(new Statistic("Swimming", "2020-04", 6, "meters"));
        statistics[4].Should()
            .BeEquivalentTo(new Statistic("Swimming", "2020-05", 7, "meters"));
        statistics[5].Should()
            .BeEquivalentTo(new Statistic("Swimming", "2020-06", 8, "meters"));
        statistics[6].Should()
            .BeEquivalentTo(new Statistic("Drinking water", "2020-07", 9, "glasses"));
        statistics[7].Should()
            .BeEquivalentTo(new Statistic("Drinking water", "2020-08", 10, "glasses"));
        statistics[8].Should()
            .BeEquivalentTo(new Statistic("Drinking water", "2020-09", 11, "glasses"));
    }
}