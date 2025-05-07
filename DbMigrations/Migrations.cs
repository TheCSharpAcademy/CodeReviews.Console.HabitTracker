namespace DotNETConsole.HabitTracker.DbMigrations;

public class Migrations
{
    public void Migrate()
    {
        var userMigration = new User();
        var habitMigration = new Habit();
        userMigration.Up();
        habitMigration.Up();
    }

    public void MigrateDown()
    {
        var userMigration = new User();
        var habitMigration = new Habit();
        userMigration.Down();
        habitMigration.Down();
    }
}