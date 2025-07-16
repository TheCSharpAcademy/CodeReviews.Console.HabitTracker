using System.Globalization;

namespace habit_logger;

public class DataSeeder
{
    public static void InsertArbitraryData()
    {
        Dictionary<string, string> genericHabits = new Dictionary<string, string>
        {
            {"Drinking Water", "glasses"},
            {"Coding", "hrs"},
            {"Running", "kms"},
            {"Working Out", "hrs"}
        };

        if (!Database.HasHabits())
        {
            // generate generic habits
            foreach (var ele in genericHabits)
            {
                Database.AddHabit(ele.Key, ele.Value);
            }

            DateTime startDate = DateTime.Now.AddDays(-100);
            Random random = new Random();
            List<Habit> habits = Database.GetAllHabits();

            foreach (var habit in habits)
            {
                // generate random data for those habits (we dont care about what the value actually is)
                for (int i = 0; i < 20; i++)
                {
                    DateTime randomDate = startDate.AddDays(random.Next(0, 101));
                    int quantity = random.Next(1, 1000);

                    Database.InsertRecord(habit.Id, randomDate.ToString("yyyy-MM-dd", new CultureInfo("en-US")), quantity);
                }
            }
        }
    }
}
