using System;

namespace HabitTracker
{
    public class HabitTrackerApp
    {
        private readonly HabitRepository habitRepository;

        public HabitTrackerApp(string connectionString)
        {
            habitRepository = new HabitRepository(connectionString);
        }
    }
}
