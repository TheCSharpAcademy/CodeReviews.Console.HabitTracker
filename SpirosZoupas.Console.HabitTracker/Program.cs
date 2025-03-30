using SpirosZoupas.Console.HabitTracker.Model;
using System.Globalization;

namespace habit_tracker
{
    class Program
    {
        public HabitTrackerApp app { get; set; } = new HabitTrackerApp();

        void Main(string[] args)
        {
            app.CreateTables();
            app.GetUserInput();
        }
    }
}