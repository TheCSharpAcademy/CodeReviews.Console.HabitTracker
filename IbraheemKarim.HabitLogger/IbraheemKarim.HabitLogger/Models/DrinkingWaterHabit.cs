namespace IbraheemKarim.HabitLogger.Models
{
    public class DrinkingWaterHabit
    {
        public int Id {  get; set; }

        public int Quantity { get; set;}

        public DateTime AddedOn { get; set;}

        public override string ToString()
        {
            return $"{Id}| On {AddedOn.ToString("dd-MM-yyyy")}" +
                $", the number of water glasses you drank was {Quantity}";
        }
    }
}
