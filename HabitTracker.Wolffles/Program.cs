using Microsoft.Data.Sqlite;


namespace habit_tracker;
class Program
{
	static void Main(string[] args)
	{
		IHabit caloriesLog = new Habit(@"Data Source=Habit_Tracker.db", "Calories", "calories");
		HabitMenu menu = new HabitMenu(caloriesLog);

		bool runMenu = menu.MainMenu();

		while (runMenu)
		{ 
			runMenu = menu.MainMenu(); 
		}
		

		return;
	}
}
