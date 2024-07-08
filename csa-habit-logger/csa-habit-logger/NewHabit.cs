namespace csa_habit_logger
{
    public class NewHabit
    {
        public string Name { get; set; }
        public string Unit { get; set; }

        public NewHabit(string name, string unit)
        {
            Name = name;
            Unit = unit;
        }

        public override string ToString()
        {
            return $"{Name} {Unit}";
        }
    }
}
