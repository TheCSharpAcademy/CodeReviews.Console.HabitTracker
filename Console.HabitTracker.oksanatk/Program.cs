namespace HabitLogger;

internal class Program
{
    static bool userSpeechInput = false;
    static void Main(string[] args)
    {
        if (!File.Exists("Habits.db"))
        {
            File.Create("Habits.db").Close();
            HabitInteractions.AutoPopulateSampleData();
        }

        if (args.Contains("--voice-input")) { userSpeechInput = true; }
        Interface.ShowMainMenu(userSpeechInput);
    }
}