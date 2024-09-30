namespace HabitTrackerLibrary;

public class Habit
{

    public readonly string Name;
    readonly DateTime _date;
    private Amount? _amount;

    public class Amount
    {
        public int Value;
        public string Unit = "";

        public Amount(){}
        public Amount(int value, string unit)
        {
            Value = value;
            Unit = unit;
        }
        public override string ToString()
        {
            return $"{Value}-{Unit}";
        }

        public static  Amount? ParseAmount(string? quantity)
        {
            
            if (String.IsNullOrWhiteSpace(quantity) || quantity.Length < 3 || !quantity.Contains("-"))
            {
                return null;
            }
            
            var amount = new Amount();       
            string[] amountParts = quantity.Replace(" ","").Split('-'); 
           
            amount.Unit = amountParts[1]; 
            bool parsed = int.TryParse(amountParts[0], out amount.Value); 
            if (!parsed) 
            { 
                throw new FormatException($"Invalid Format {quantity}. Make Sure it is formatted like this \"Num-Value\"");
            }
            return amount;
            
            

            
        }
    }

    public string? GetAmount()
    {
           if (_amount == null)
           {
               return null;
           }
           return _amount.ToString();
    }
    //quantity is the optional amount to measure and displayed as "39-unit"
    

    public Habit(string name, string date, string? quantity = null)
    {
        Name = name;
        _date = DateTime.Parse(date);
        _amount = Amount.ParseAmount(quantity
        );
       
    }

    public override string ToString()
    {
        return $"Name: {Name} -- Date: {GetDate()} -- Quantity: {GetAmount()}";
    }
    public string GetDate()
    {
        return _date.ToString("yyyy-MM-dd");
    }
}
