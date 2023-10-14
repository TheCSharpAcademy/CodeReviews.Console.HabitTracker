namespace HabitTracker
{
	class Program
	{
		public static void Main(string[] args)
		{
			Database.SetupDB();

			Menu.GetUserInput();
		}
	}
}