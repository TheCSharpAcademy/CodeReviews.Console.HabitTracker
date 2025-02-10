using System.Text.RegularExpressions;

namespace HabitTracker.BrozDa
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HabitTracker tracker = new HabitTracker();
            tracker.Start();
            Console.ReadLine();

        }
    }
}
