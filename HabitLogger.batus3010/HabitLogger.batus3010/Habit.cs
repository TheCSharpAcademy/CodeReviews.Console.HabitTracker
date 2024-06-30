

namespace HabitLogger.batus3010
{
    internal class Habit
    {
        private static int _count;
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        public Habit(string name, int quantity)
        {
            Id = ++_count;
            Name = name;
            Quantity = quantity;
        }

        public override string ToString() => $"Id: {Id}, Name: {Name}, Quantity: {Quantity}";
    }
}
