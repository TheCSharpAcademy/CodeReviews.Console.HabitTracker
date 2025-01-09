namespace HabitLoggerLibrary.Ui.Input;

public sealed class ConsoleInputChoiceReader(IConsoleWrapper consoleWrapper)
    : IInputChoiceReader
{
    public InputChoice GetChoice()
    {
        var positionLeft = Console.CursorLeft;
        var positionTop = Console.CursorTop;
        char choice;
        do
        {
            Console.SetCursorPosition(positionLeft, positionTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(positionLeft, positionTop);
            Console.Write("> ");
            choice = consoleWrapper.ReadKey().KeyChar;
        } while (!Enum.IsDefined(typeof(InputChoice), (int)choice));

        return (InputChoice)choice;
    }
}