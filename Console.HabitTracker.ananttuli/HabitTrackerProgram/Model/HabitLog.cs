using HabitLogRepo = HabitTrackerProgram.Database.HabitLogRepository;

namespace HabitTrackerProgram.Model
{
    public class HabitLog
    {
        public int id;
        public decimal quantity;
        public DateTime logTime;

        public HabitLog(int id, decimal quantity, DateTime logTime)
        {
            this.id = id;
            this.quantity = quantity;
            this.logTime = logTime;
        }

        public static void CreateHabitLog()
        {
            Console.WriteLine("\nEnter habit log details:");

            decimal quantity = Util.Input.TryReadInput("Quantity (Numerical values only)", Util.Input.ParseDecimal);

            DateTime logDateTime = Util.Input.TryReadInput("Date (E.g. 2024-01-01)", Util.Input.ParseDateTime);

            HabitLogRepo.CreateHabit(quantity, logDateTime);
        }

        public static void UpdateHabitLog()
        {
            Console.WriteLine("\nEnter habit log details:");

            int id = (int)Util.Input.TryReadInput("Habit ID", Util.Input.ParseDecimal);

            decimal quantity = Util.Input.TryReadInput("Quantity (Numerical values only)", Util.Input.ParseDecimal);

            DateTime logDateTime = Util.Input.TryReadInput("Date (E.g. 2024-01-01)", Util.Input.ParseDateTime);

            HabitLogRepo.UpdateHabit(id, quantity, logDateTime);
        }

        public static void ViewHabitLogs()
        {
            List<HabitLog> habits = HabitLogRepo.ReadHabitsQuery();
            Console.WriteLine($"\n\n\t\tID\t\tQuantity\t\tLogged At");

            foreach (HabitLog habit in habits)
            {
                Console.WriteLine($"\t\t{habit.id}\t\t{habit.quantity}\t\t{habit.logTime}");
            }
        }

        public static void DeleteHabitLog()
        {
            int id = (int)Util.Input.TryReadInput("Enter Habit ID to delete", Util.Input.ParseDecimal);

            HabitLogRepo.DeleteHabit(id);
        }
    }
}