using HabitTracker.Models;
namespace HabitTracker.Utilities;
class HabitTrackerHistory
{
    List<HabitTrackerEntry> habitTrackerEntries = new List<HabitTrackerEntry>();

    internal void AddToHabitTrackerHistory(int id, DateTime date, int quantity)
    {
        habitTrackerEntries.Add(new HabitTrackerEntry(id, date, quantity));
    }
    internal bool CheckForExistingEntries(int id)
    {
        if (!habitTrackerEntries.Any(entry => entry.Id == id))
        {
            return false;
        }
        return true;
    }
    internal void RemoveMatchingIdFromHabitTRackerHistory(int id)
    {
        habitTrackerEntries.RemoveAll(entry => entry.Id == id);
    }
    internal void UpdateHabitTrackerHistory(int id, DateTime date, int quantity)
    {
        var entryToUpdate = habitTrackerEntries.Find(entry => entry.Id == id);
        if (entryToUpdate != null)
        {
            entryToUpdate.Date = date;
            entryToUpdate.Quantity = quantity;
        }
    }
    internal void PrintHabitTrackerHistory()
    {
        Console.WriteLine("------------------------------------\n");
        foreach (var entry in habitTrackerEntries)
        {
            Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MM-yy")} - Quantity: {entry.Quantity}");

        }
        Console.WriteLine("------------------------------------\n");
    }
}