namespace Main.Models
{
    using System;

    internal class Habit
    {
        public int Id { get; set; }

        public string Date { get; set; } = DateTime.Now.ToString("yyyy-mm-dd");

        public int Quantity { get; set; }

        public Category? Category { get; set; }
    }
}
