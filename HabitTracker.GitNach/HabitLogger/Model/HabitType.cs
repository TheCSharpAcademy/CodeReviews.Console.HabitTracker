namespace HabitLogger.Model
{
    public class HabitType
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public string Metric { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name;
        }
    }
}
