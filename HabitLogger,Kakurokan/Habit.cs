namespace HabitLogger_Kakurokan
{
    internal class Habit
    {
        public string Name { get; private set; }
        public DateTime Date { get; private set; }
        public int Quantity { get; private set; }
        public int Id { get; private set; }
        public string Unit { get; private set; }

        public Habit(string name, DateTime date, int quantity, int id, string unit)
        {
            Name = name;
            Date = date;
            Quantity = quantity;
            Id = id;
            this.Unit = unit;
        }

        public Habit()
        {
        }

        public override string ToString()
        {
            return $@" Id : {Id}
 Habit: {Name} 
 At: {Date:dd-MM-yyyy}
 Quantity: {Quantity}{Unit}";
        }
    }
}
