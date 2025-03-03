namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Repository interface for managing <see cref="HabitRecord"/> entities
    /// Extends <see cref="IRepository{T}"/> with <see cref="HabitRecord"/> specific querries
    /// </summary>
    internal interface IHabitRecordRepository : IRepository<HabitRecord>
    {
        /// <summary>
        /// Retrieves all <see cref="HabitRecord"/> entities which have coresponding habit record ID
        /// </summary>
        /// <param name="habitId"><see cref="int"/> value representing <see cref="Habit"/> ID</param>
        /// <returns>Collection of <see cref="HabitRecord"/>entities</returns>
        IEnumerable<HabitRecord> GetAllByHabitID(int habitID);
        /// <summary>
        /// Removes all <see cref="HabitRecord"/> entities which have coresponding habit record ID from repository
        /// </summary>
        /// <param name="habitId"><see cref="int"/> value representing <see cref="Habit"/> ID</param>
        void DeleteAllByHabitId(int habitID);   
    }
}
