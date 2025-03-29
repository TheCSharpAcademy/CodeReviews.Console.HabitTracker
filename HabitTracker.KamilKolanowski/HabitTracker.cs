using HabitTracker.KamilKolanowski.Data;
using HabitTracker.KamilKolanowski.Views;

namespace HabitTracker.KamilKolanowski
{
    class HabitTracker
    {
        public static void Main(string[] args)
        {
           MainInterface mainInterface = new MainInterface();
           mainInterface.Start();
        }
    }
}
