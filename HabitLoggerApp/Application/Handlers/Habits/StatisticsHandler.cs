using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;

namespace HabitLoggerApp.Application.Handlers.Habits;

public class StatisticsHandler(IHabitLogsRepository repository, IKeyAwaiter keyAwaiter)
{
    public void Handle()
    {
        Console.Clear();
        var yearlyStatistics = repository.GetStatistics("%Y");
        var monthlyStatistics = repository.GetStatistics("%Y-%m");
        var weeklyStatistics = repository.GetStatistics("%Y-%W");

        Console.WriteLine("Yearly statistics:");
        foreach (var record in yearlyStatistics)
            Console.WriteLine($"{record.habitName}: {record.period}; {record.total} {record.unitOfMeasure}");

        Console.WriteLine("--------------------");
        Console.WriteLine("Monthly statistics:");
        foreach (var record in monthlyStatistics)
            Console.WriteLine($"{record.habitName}: {record.period}; {record.total} {record.unitOfMeasure}");

        Console.WriteLine("--------------------");
        Console.WriteLine("Weekly statistics:");
        foreach (var record in weeklyStatistics)
            Console.WriteLine($"{record.habitName}: {record.period}; {record.total} {record.unitOfMeasure}");

        Console.WriteLine("Press any key to continue...");
        keyAwaiter.Wait();
    }
}