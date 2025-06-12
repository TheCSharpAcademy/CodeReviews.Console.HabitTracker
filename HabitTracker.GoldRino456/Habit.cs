

namespace HabitTracker.GoldRino456
{
    public class Habit
    {
        int _tableId; //Determines the habit type this habit object belongs to.
        int _id;
        DateTime _date;
        string _habitType;
        float _quantity;
        string _unitOfMeasurement;

        public int TableID { get { return _tableId; } set { _tableId = value; } }
        public int ID { get { return _id; } set { _id = value; } }
        public DateTime Date { get { return _date; } set { _date = value; } }
        public string HabitType { get { return _habitType; } set { _habitType = value; } }
        public float Quantity { get { return _quantity; } set { _quantity = value; } }
        public string UnitOfMeasurement { get { return _unitOfMeasurement; } set { _unitOfMeasurement = value; } }


        //New Habit Object Constructor
        public Habit(DateTime date, string habitType, float quantity, string unitOfMeasurement)
        {
            Date = date;
            HabitType = habitType;
            Quantity = quantity;
            UnitOfMeasurement = unitOfMeasurement;
        }

        //Existing Habit Object Constructor
        public Habit(int tableId, int id)
        {
            //Fetch data from database?
        }

        public override string ToString()
        {
            return ($"{Date.ToShortDateString()}: {Quantity} {UnitOfMeasurement} of {HabitType}.");
        }
    }
}
