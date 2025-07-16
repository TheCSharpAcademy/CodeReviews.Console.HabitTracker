using HabitTracker.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;

namespace DatabaseTests;

public class DatabaseTests
{
    private const string ConnectionString = "Data Source = testhabitdb";
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void SuccessfullyInitializes()
    {
        Initializer init = new(ConnectionString);
        Assert.DoesNotThrow(() => init.Initialize());
    }

    [Test]
    public void SuccessfullyConnects()
    {
        SqliteConnection conn = new(ConnectionString);
        Assert.DoesNotThrow(() => conn.Open());
    }
}