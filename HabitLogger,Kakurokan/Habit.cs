namespace HabitLogger_Kakurokan
{
    internal class Habit
    {
        public string Name { get; private set; }
        public DateTime Date { get; private set; }
        public int Quantity { get; private set; }

        public int Id;
        public string Unit { get; private set; }

        public Habit(string name, DateTime date, int quantity, string unit)
        {
            Name = name;
            Date = date;
            Quantity = quantity;
            Unit = unit;
        }


        public Habit()
        {
        }

        public string ToStringWithoutId() => $@" Habit: {Name} 
 At: {Date:dd-MM-yyyy}
 Quantity: {Quantity}{Unit}";

        public override string ToString() => $@" Id : {Id}
 Habit: {Name} 
 At: {Date:dd-MM-yyyy}
 Quantity: {Quantity}{Unit}";

    }
}
