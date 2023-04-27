using ConsoleTables;

namespace HabitTracker.barakisbrown;

public class Habit
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }

    public override string ToString()
    {
        return $"Id = {Id}, Date = {Date.ToShortDateString()}, Amount = {Amount}";
    }

    public static void DisplayAllRecords(List<Habit> ?habits)
    {
        var table = new ConsoleTable("ID", "AMOUNT", "DATE");
        table.Configure(o => o.EnableCount = false);

        foreach(Habit hab in habits)
        {
            table.AddRow(hab.Id, hab.Amount, hab.Date.ToShortDateString());
        }

        table.Write();
        Console.WriteLine();
    }
}
