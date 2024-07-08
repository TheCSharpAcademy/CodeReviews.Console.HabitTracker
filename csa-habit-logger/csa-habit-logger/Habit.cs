namespace csa_habit_logger
{
    public class Habit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }

        public Habit(int ID, string name, string unit)
        {
            this.ID = ID;
            Name = name;
            Unit = unit;
        }

        public override string ToString()
        {
            return $"{ID} | {Name} | {Unit}";
        }

        public NewHabit ToNewHabit()
        {
            return new NewHabit(Name, Unit);
        }
    }
}
