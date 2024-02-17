using HabitTracker.Dejmenek.Models;

namespace HabitTracker.Dejmenek.Helpers
{
    public class RandomHabits
    {
        private static readonly Random _random = new Random();
        private static readonly List<(string, string, int, string)> _habitTemplates = new List<(string, string, int, string)>() {
            ( "Drink water", "Drink 5 glasses of water", 5, "glasses" ),
            ( "Do pushups", "Do 10 pushups", 10, "pushups" ),
            ( "Gratitude", "Write down 4 things you're grateful", 4, "things"),
            ( "New Things", "Try 2 new things ", 2, "things" ),
            ( "Stretching", "Perform 15 repetitions of each major muscle group stretch", 15, "repetitions" ),
        };

        public static List<Habit> GenerateRandomHabits() {
            List<Habit> habits = new List<Habit>();
            for (int i = 0; i < 100; i++) {
                (string name, string description, long quantity, string quantityUnit) = _habitTemplates.ElementAt(_random.Next(_habitTemplates.Count));
                DateTime date = DateTime.Now.AddDays(_random.Next(-10, -2));

                habits.Add(new Habit
                {
                    Name = name,
                    Description = description,
                    Quantity = quantity,
                    QuantityUnit = quantityUnit,
                    Date = date.ToString("yyyy-MM-dd"),
                });
            }

            return habits;
        }
    }
}
