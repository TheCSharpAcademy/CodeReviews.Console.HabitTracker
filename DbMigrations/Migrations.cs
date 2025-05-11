namespace DotNETConsole.HabitTracker.DbMigrations;

public class Migrations
{
    public void Migrate()
    {
        var userMigration = new User();
        var habitMigration = new Habit();
        var habitLogMigration = new HabitLog();
        userMigration.Up();
        habitMigration.Up();
        habitLogMigration.Up();
    }

    public void MigrateDown()
    {
        var userMigration = new User();
        var habitMigration = new Habit();
        var habitLogMigration = new HabitLog();
        userMigration.Down();
        habitMigration.Down();
        habitLogMigration.Down();
    }
}