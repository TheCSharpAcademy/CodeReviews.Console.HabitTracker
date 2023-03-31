namespace ConsoleUI.Helpers
{
    internal static class MenuHelper
    {
        public static void MainMenu()
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("--------------------");
            Console.WriteLine("Track water drinking");
            Console.WriteLine("--------------------");
            Console.WriteLine("1: Load Habit records.");
            Console.WriteLine("2: Insert Record into Habit");
            Console.WriteLine("3: Delete Habit Record");
            Console.WriteLine("4: Update Habit Record");
            Console.WriteLine("0: Exit Program");
            Console.WriteLine("--------------------");

        }

    }
}
