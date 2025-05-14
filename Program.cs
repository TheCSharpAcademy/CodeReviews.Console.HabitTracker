namespace DotNETConsole.HabitTracker;

using DB;
using DbMigrations;
using Controllers;

public class Program
{
    public static void Main(string[] args)
    {
        var db = new DbConnection();
        db.ConnectionStatus();
        var migrations = new Migrations();
        migrations.Migrate();

        var mainController = new MenuController();
        Console.Clear();
        mainController.MainMenu();
    }
}
