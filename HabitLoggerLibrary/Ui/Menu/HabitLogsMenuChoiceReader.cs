namespace HabitLoggerLibrary.Ui.Menu;

public sealed class HabitLogsMenuChoiceReader(IConsoleWrapper consoleWrapper) : IHabitLogsMenuChoiceReader
{
    public HabitLogsMenuChoice GetChoice()
    {
        var leftPosition = Console.CursorLeft;
        var topPosition = Console.CursorTop;
        char choice;
        do
        {
            Console.SetCursorPosition(leftPosition, topPosition);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(leftPosition, topPosition);
            Console.Write("> ");
            choice = consoleWrapper.ReadKey().KeyChar;
        } while (!Enum.IsDefined(typeof(HabitLogsMenuChoice), (int)choice));

        return (HabitLogsMenuChoice)choice;
    }
}