using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Models;
using HabitTracker.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;

namespace DatabaseTests;

public class OccurrenceRepoTests
{
    private const string ConnectionString = "Data Source = testhabitdb";
    private OccurrenceRepository _occRepo = new(ConnectionString);
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
    public void ReturnsNoOccurrences()
    {
        var occs = _occRepo.GetAllOccurrences();
        Assert.That(occs.Count, Is.EqualTo(0));
    }

    [Test]
    public void AddOneOccurrence()
    {
        _habitRepo.CreateHabit(new HabitCreationDto( "test", "tests"));
        OccurrenceCreationDto occ = new( "2025-07-14 10:30:00", 1);
        Assert.That(_occRepo.CreateOccurrence(occ), Is.True);
    }

    [Test]
    public void FailsToAddWithoutValidHabitId()
    {
        _habitRepo.CreateHabit(new HabitCreationDto( "test", "tests"));
        OccurrenceCreationDto occ = new( "2025-07-14 10:30:00", 2);
        Assert.Throws<InvalidOperationException>(() => _occRepo.CreateOccurrence(occ));
    }
    
    [Test]
    public void GetsOnlySpecifiedHabitOccurrences()
    {
        _habitRepo.CreateHabit(new HabitCreationDto("test", "tests"));
        _habitRepo.CreateHabit(new HabitCreationDto("test2", "tests"));
        OccurrenceCreationDto occ1 = new( "2025-07-14 10:30:00", 1);
        _occRepo.CreateOccurrence(occ1);
        OccurrenceCreationDto occ2 = new( "2025-07-14 10:30:01", 1);
        _occRepo.CreateOccurrence(occ2);
        OccurrenceCreationDto occ3 = new( "2025-07-14 10:30:00", 2);
        _occRepo.CreateOccurrence(occ3);
        var occs = _occRepo.GetOccurrencesByHabitId(1);
        Assert.That(occs.Count, Is.EqualTo(2));
    }
}