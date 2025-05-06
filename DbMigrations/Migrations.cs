namespace DotNETConsole.HabitTracker.DbMigrations;

public class Migrations
{
    public void Migrate()
    {
        var userMigration = new User();
        userMigration.Up();
    }
}