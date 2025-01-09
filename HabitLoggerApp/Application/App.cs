using HabitLoggerApp.Fixtures;
using HabitLoggerLibrary.DbManager;

namespace HabitLoggerApp.Application;

public sealed class App(IDatabaseManager databaseManager, FixturesGenerator fixturesGenerator, Loop loop)
{
    public void Run()
    {
        databaseManager.GetConnection().Open();
        databaseManager.SetUp();
        fixturesGenerator.Populate();
        loop.Run();
    }
}