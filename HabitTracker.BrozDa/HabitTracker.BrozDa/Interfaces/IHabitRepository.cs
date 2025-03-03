namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Repository interface for managing Habit entities
    /// Extends <see cref="IRepository{T}"/> with <see cref="Habit"/> specific querries
    /// </summary>
    internal interface IHabitRepository : IRepository<Habit>
    {
        /// <summary>
        /// Retrieves <see cref="Habit"/> entity based on ID provided
        /// </summary>
        /// <param name="habitId"><see cref="int"/> value representing <see cref="Habit"/> ID</param>
        /// <returns><see cref="Habit"/> entity with coresponding ID</returns>
        Habit GetHabitById(int habitId);
    }
}
