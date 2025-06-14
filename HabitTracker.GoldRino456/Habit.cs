namespace HabitTracker.GoldRino456
{
    public class Habit
    {
        int _id;
        DateTime _date;
        string _habitType;
        int _quantity;
        string _unitOfMeasurement;

        public int ID { get { return _id; } set { _id = value; } }
        public DateTime Date { get { return _date; } set { _date = value; } }
        public string HabitType { get { return _habitType; } set { _habitType = value; } }
        public int Quantity { get { return _quantity; } set { _quantity = value; } }
        public string UnitOfMeasurement { get { return _unitOfMeasurement; } set { _unitOfMeasurement = value; } }


        //New Habit Object Constructor
        public Habit(DateTime date, string habitType, int quantity, string unitOfMeasurement)
        {
            Date = date;
            HabitType = habitType;
            Quantity = quantity;
            UnitOfMeasurement = unitOfMeasurement;
        }

        public Habit(int id, DateTime date, string habitType, int quantity, string unitOfMeasurement)
        {
            ID = id;
            Date = date;
            HabitType = habitType;
            Quantity = quantity;
            UnitOfMeasurement = unitOfMeasurement;
        }

        public override string ToString()
        {
            return ($"{ID}. {Date.ToShortDateString()}: {Quantity} {UnitOfMeasurement} of {HabitType}.");
        }
    }
}