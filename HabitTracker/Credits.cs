namespace HabitTracker
{
    internal class Credits
    {
        static public void GetCredits()
        {
            var credits = new List<string>();
            credits.Add("Created by Nikita Kostin.\n");
            credits.ForEach(i => Console.Write(i));
            Console.Write("Press any key to go back to the main menu...");
            Console.ReadKey();
        }
    }
}
