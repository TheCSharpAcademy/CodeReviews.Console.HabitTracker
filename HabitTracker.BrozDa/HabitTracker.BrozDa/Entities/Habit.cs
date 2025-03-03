namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Class representing objects for <see cref="HabitRepository"/>
    /// </summary>
    internal class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; } = "defaultValue";
        public string Unit {  get; set; } = "defaultValue";

    }
}
