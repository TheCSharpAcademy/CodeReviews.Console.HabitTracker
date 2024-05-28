public class HabitRepository
{
    private readonly DatabaseManager _dbManager;

    public HabitRepository(DatabaseManager dbManager)
    {
        _dbManager = dbManager;
    }

    // TODO placeholder Repository method/pattern. Also think about: DeleteHabit, UpdateHabit, GetHabitById, GetAllHabits
    public void AddHabit(Habit habit)
    {
        throw new NotImplementedException();
    }
}