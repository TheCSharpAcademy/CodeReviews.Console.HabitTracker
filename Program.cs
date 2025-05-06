namespace DotNETConsole.HabitTracker;

using DB;
using DbMigrations;
public class Program
{
    public static void Main(string[] args)
    {
        var db = new DbConnection();
        db.ConnectionStatus();
        var migrations = new Migrations();
        migrations.Migrate();
    }
}