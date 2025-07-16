using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Models;
using HabitTracker.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;

namespace DatabaseTests;

public class HabitRepoTests
{
    private const string ConnectionString = "Data Source = testhabitdb";
    private HabitRepository _habitRepo = new(ConnectionString);

    [SetUp]
    public void SetUp()
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();
            var query = "DELETE FROM occurrences;";
            SqliteCommand command = new(query, conn);
            command.ExecuteNonQuery();
            command.CommandText = "DELETE FROM habits;";
            command.ExecuteNonQuery(); 
            command.CommandText = "DELETE FROM SQLITE_SEQUENCE WHERE name = 'occurrences';";
            command.ExecuteNonQuery();
            command.CommandText = "DELETE FROM SQLITE_SEQUENCE WHERE name = 'habits';";
            command.ExecuteNonQuery(); 
        }
    }
    
    [Test]
    public void ReturnsNoHabits()
    {
        var habits = _habitRepo.GetAllHabits();
        Assert.That(habits.Count, Is.EqualTo(0));
    }

    [Test]
    public void AddOneHabit()
    {
        var habit = new HabitCreationDto("thing", "tests");
        Assert.That(_habitRepo.CreateHabit(habit), Is.True);
    }

    [Test]
    public void ReturnsOneHabit()
    {
        var habit = new HabitCreationDto("thing", "tests");
        _habitRepo.CreateHabit(habit);
        var habits = _habitRepo.GetAllHabits();
        Assert.That(habits.Count, Is.EqualTo(1));
        Assert.That(habits[0].Name, Is.EqualTo("thing"));
        Assert.That(habits[0].Unit, Is.EqualTo("tests"));
    }

    [Test]
    public void ReturnsOneHabitById()
    {
        var newHabit = new HabitCreationDto("thing", "tests");
        Assert.That(_habitRepo.CreateHabit(newHabit), Is.True);
        var habit = _habitRepo.GetHabitById(1);
        Assert.That(habit!.Name, Is.EqualTo("thing"));
        Assert.That(habit!.Unit, Is.EqualTo("tests"));
    }

    [Test]
    public void GetNullIfHabitDoesntExist()
    {
        var habit = _habitRepo.GetHabitById(1);
        Assert.That(habit, Is.Null);
    }

    [Test]
    public void SuccessfullyUpdateHabit()
    {
        var newHabit = new HabitCreationDto("thing", "tests");
        Assert.That(_habitRepo.CreateHabit(newHabit), Is.True);
        var updatedHabit = new Habit(1,"thingy", "testamundo");
        Assert.That(_habitRepo.UpdateHabit(updatedHabit), Is.True);
        Assert.That(updatedHabit!.Name, Is.EqualTo("thingy"));
        Assert.That(updatedHabit!.Unit, Is.EqualTo("testamundo"));
    }

    [Test]
    public void UnsuccessfullyUpdateInexistantHabit()
    {
        var updatedHabit = new Habit(1, "thingy", "testamundo");
        Assert.That(_habitRepo.UpdateHabit(updatedHabit), Is.False);
    }

    [Test]
    public void SuccessfullyDeleteHabit()
    {
        var newHabit = new HabitCreationDto("thing", "tests");
        Assert.That(_habitRepo.CreateHabit(newHabit), Is.True);
        Assert.That(_habitRepo.DeleteHabitById(1), Is.True);
    }
}