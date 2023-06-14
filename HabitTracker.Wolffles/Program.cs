using Microsoft.Data.Sqlite;
using System.Globalization;

namespace habit_tracker;
class Program
{
	static void Main(string[] args)
	{
		//Start
		IHabit caloriesLog = new Habit(@"Data Source=Habit_Tracker.db", "Calories", "calories");
		HabitMenu menu = new HabitMenu(caloriesLog);

		while (menu.MainMenu()) ;

		return;
	}
}
