namespace HabitTrackerLibrary;

public class Habit
{
    private readonly DateTime _date;

    public readonly string Name;

    private readonly Amount? _amount;
    //quantity is the optional amount to measure and displayed as "39-unit"


    public Habit(string name, string date, string? quantity = null)
    {
        Name = name;
        _date = DateTime.Parse(date);
        _amount = Amount.ParseAmount(quantity
        );
    }

    public string? GetAmount()
    {
        if (_amount == null) return null;
        return _amount.ToString();
    }

    public override string ToString()
    {
        return $"Name: {Name} -- Date: {GetDate()} -- Quantity: {GetAmount()}";
    }

    public string GetDate()
    {
        return _date.ToString("yyyy-MM-dd");
    }

    public class Amount
    {
        public string Unit = "";
        public int Value;

        public Amount()
        {
        }

        public Amount(int value, string unit)
        {
            Value = value;
            Unit = unit;
        }

        public override string ToString()
        {
            return $"{Value}-{Unit}";
        }

        public static Amount? ParseAmount(string? quantity)
        {
            if (string.IsNullOrWhiteSpace(quantity) || quantity.Length < 3 || !quantity.Contains("-")) return null;

            var amount = new Amount();
            var amountParts = quantity.Replace(" ", "").Split('-');

            amount.Unit = amountParts[1];
            var parsed = int.TryParse(amountParts[0], out amount.Value);
            if (!parsed)
                throw new FormatException(
                    $"Invalid Format {quantity}. Make Sure it is formatted like this \"Num-Value\"");
            return amount;
        }
    }
}