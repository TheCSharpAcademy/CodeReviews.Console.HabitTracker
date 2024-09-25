namespace HabitTracker.carlosmorales125;

public class HabitService(DbContext dbContext)
{
    public void ShowAllHabits()
    {
        var habits = dbContext.GetHabits();
        
        if (habits.Count == 0)
        {
            Console.WriteLine("No habits logged yet.");
            return;
        }

        Console.WriteLine("Habits: ---------------------------------");
        foreach (var habit in habits)
        {
            Console.WriteLine($"Id: {habit.Id}, Name: {habit.Name}, Quantity: {habit.Quantity}, Date: {habit.Date:MM/dd/yyyy}");
            Console.WriteLine("-----------------------------------------");
        }
    }

    public void AddHabit(Habit habit)
    {
        dbContext.AddHabit(habit);
    }

    public void DeleteHabit(int id)
    {
        dbContext.DeleteHabit(id);
    }
    
    public void UpdateHabit(Habit habit)
    {
        dbContext.UpdateHabit(habit);
    }
}