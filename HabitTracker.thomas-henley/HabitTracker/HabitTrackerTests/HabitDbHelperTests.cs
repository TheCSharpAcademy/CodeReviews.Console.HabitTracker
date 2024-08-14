using HabitTracker;

namespace HabitTrackerTests;

public class HabitDbHelperTests : IDisposable
{
    private const string TABLE_NAME = "test_table";
    private readonly HabitDbHelper dbHelper = new(TABLE_NAME);

    public HabitDbHelperTests()
    {
        if (0 != HabitDbHelper.SqlScalarQuery("SELECT COUNT() FROM (SELECT name FROM sqlite_master WHERE type='table' AND name='test_table')"))
        {
            dbHelper.TeardownDB();
        }

        dbHelper.InitializeDB();
        Assert.True(dbHelper.IsDbEmpty());
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        dbHelper.TeardownDB();
        Assert.Equal(0, HabitDbHelper.SqlScalarQuery("SELECT name FROM sqlite_master WHERE type='table' AND name='test_table'"));
    }

    [Fact]
    public void TestPopulateDBAndGetAllRecords()
    {
        dbHelper.PopulateDB();
        Assert.Equal(100, dbHelper.GetAllRecords().Count);
    }

    [Fact]
    public void TestIsDbEmpty()
    {
        Assert.True(dbHelper.IsDbEmpty());
        dbHelper.Insert("01-01-01", 5);
        Assert.False(dbHelper.IsDbEmpty());
    }

    [Fact]
    public void TestInsert()
    {
        Assert.True(dbHelper.IsDbEmpty());
        dbHelper.Insert("01-01-01", 5);
        Assert.Single(dbHelper.GetAllRecords());
    }

    [Fact]
    public void TestGetById()
    {
        Assert.True(dbHelper.IsDbEmpty());
        dbHelper.Insert("01-01-01", 5);

        Assert.False(dbHelper.TryGetById(2, out var blank));
        Assert.Null(blank);

        Assert.True(dbHelper.TryGetById(1, out var entry));
        Assert.NotNull(entry);
        Assert.Equal(1, entry.Id);
        Assert.Equal("01-01-01", entry.Date.ToString("dd-MM-yy"));
        Assert.Equal(5, entry.Quantity);
    }

    [Fact]
    public void TestUpdate()
    {
        Assert.True(dbHelper.IsDbEmpty());
        dbHelper.Insert("01-01-01", 5);

        Assert.True(dbHelper.TryGetById(1, out var entry));
        Assert.NotNull(entry);
        Assert.Equal(1, entry.Id);
        Assert.Equal("01-01-01", entry.Date.ToString("dd-MM-yy"));
        Assert.Equal(5, entry.Quantity);

        dbHelper.Update(1, "02-02-02", 10);

        Assert.True(dbHelper.TryGetById(1, out entry));
        Assert.NotNull(entry);
        Assert.Equal(1, entry.Id);
        Assert.Equal("02-02-02", entry.Date.ToString("dd-MM-yy"));
        Assert.Equal(10, entry.Quantity);
    }

    [Fact]
    public void TestDelete()
    {
        Assert.True(dbHelper.IsDbEmpty());
        dbHelper.Insert("01-01-01", 1);
        dbHelper.Insert("02-02-02", 2);
        dbHelper.Insert("03-03-03", 3);

        dbHelper.Delete(2);
        Assert.False(dbHelper.TryGetById(2, out var _));
        Assert.Equal(2, dbHelper.GetAllRecords().Count);
    }

    [Fact]
    public void TestGetTotalDays()
    {
        Assert.True(dbHelper.IsDbEmpty());
        dbHelper.Insert("01-01-01", 1);
        dbHelper.Insert("02-02-02", 2);
        dbHelper.Insert("03-03-03", 3);
        dbHelper.Insert("03-03-03", 4);

        Assert.Equal(3, dbHelper.GetTotalDays());
    }

    [Fact]
    public void TestGetTotalPoints()
    {
        Assert.True(dbHelper.IsDbEmpty());
        dbHelper.Insert("01-01-01", 1);
        dbHelper.Insert("02-02-02", 2);
        dbHelper.Insert("03-03-03", 3);
        dbHelper.Insert("03-03-03", 4);

        Assert.Equal(10, dbHelper.GetTotalPoints());
        Assert.Equal(7, dbHelper.GetTotalPoints(03));
    }
}