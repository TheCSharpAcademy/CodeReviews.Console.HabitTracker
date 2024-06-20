using HabitRepo = HabitTrackerProgram.Database.HabitRepository;

namespace HabitTrackerProgram.Model
{
    public class Habit
    {
        public int id;
        public decimal quantity;
        public DateTime logTime;

        public Habit(int id, decimal quantity, DateTime logTime)
        {
            this.id = id;
            this.quantity = quantity;
            this.logTime = logTime;
        }

        public static void CreateHabit()
        {
            Console.WriteLine("\nEnter habit log details:");

            decimal quantity = Util.Input.TryReadInput("Quantity (Numerical values only)", Util.Input.ParseDecimal);

            DateTime logDateTime = Util.Input.TryReadInput("Date (E.g. 2024-01-01)", Util.Input.ParseDateTime);

            HabitRepo.CreateHabit(quantity, logDateTime);
        }

        public static void UpdateHabit()
        {
            Console.WriteLine("\nEnter habit log details:");

            int id = (int)Util.Input.TryReadInput("Habit ID", Util.Input.ParseDecimal);

            decimal quantity = Util.Input.TryReadInput("Quantity (Numerical values only)", Util.Input.ParseDecimal);

            DateTime logDateTime = Util.Input.TryReadInput("Date (E.g. 2024-01-01)", Util.Input.ParseDateTime);

            HabitRepo.UpdateHabit(id, quantity, logDateTime);
        }

        public static void ViewHabitLogs()
        {
            List<Habit> habits = HabitRepo.ReadHabitsQuery();
            Console.WriteLine($"\n\n\t\tID\t\tQuantity\t\tLogged At");

            foreach (Habit habit in habits)
            {
                Console.WriteLine($"\t\t{habit.id}\t\t{habit.quantity}\t\t{habit.logTime}");
            }
        }

        public static void DeleteHabit()
        {
            int id = (int)Util.Input.TryReadInput("Enter Habit ID to delete", Util.Input.ParseDecimal);

            HabitRepo.DeleteHabit(id);
        }
    }
}