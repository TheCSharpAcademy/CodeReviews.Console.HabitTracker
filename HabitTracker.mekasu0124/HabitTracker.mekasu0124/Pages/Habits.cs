using HabitTracker.Services;
using HabitTracker.Models;

namespace HabitTracker;

public class NewHabit
{
    public static void NewHabitEntry()
    {
        Console.WriteLine("--------------------");
        Console.WriteLine("Creating A New Habit");
        Console.WriteLine("--------------------");
        Console.Write("New Habit Name: ");

        string newHabitName = Console.ReadLine();
        newHabitName = Helpers.ValidateEntry(newHabitName, "New Habit Name");
        
        Console.WriteLine("\n\nSelect A Tracking Type");

        List<string> trackingTypes = new() { "C - Count", "T - Time" };

        foreach (string line in trackingTypes)
        {
            Console.WriteLine(line);
        } ;
        
        Console.WriteLine("New Habit Track Type: ");

        string newTrackType = Console.ReadLine().ToLower();
        newTrackType = Helpers.ValidateTrackType(newTrackType, "New Habit Track Type");
        
        Console.Write("Description Of New Habit: ");

        string newHabitDescription = Console.ReadLine();
        newHabitDescription = Helpers.ValidateEntry(newHabitDescription, "Description Of New Habit");

        Habit newHabit = new()
        {
            Name = newHabitName,
            Date = DateTime.Now,
            TrackType = newTrackType,
            Description = newHabitDescription
        };
        
        Database.SaveEntry(newHabit);
    }
}