using Spectre.Console;


namespace FunRun.HabitTracker;

public class HabitTrackerApp
{
    public async Task RunApp()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("HabitTracker").Centered().Color(Color.Blue));

            AnsiConsole.MarkupLine("[blue] Inpired by the [link=https://thecsharpacademy.com/project/12/habit-logger]C#Acadamy [/][/]");
            AnsiConsole.MarkupLine("");


            Console.ReadKey(true);
        }
    }
}
