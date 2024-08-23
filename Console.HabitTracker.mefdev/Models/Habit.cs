using System;
using System.Text.Json.Serialization;

namespace HabitLogger.Models
{
	public class Habit
	{
		public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public DateTime Date { get; set; }
        public Habit(int id, string name, string quantity, DateTime date)
		{
			Id = id;
			Name = name;
			Quantity = quantity;
			Date = date;
		}
	}
}

