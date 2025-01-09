namespace HabitLoggerLibrary.Ui.Input;

public sealed class ConsoleInputReader(IConsoleWrapper consoleWrapper) : IInputReader
{
    public string GetStringInput()
    {
        return GetInput(Console.CursorLeft, Console.CursorTop);
    }

    public long GetNumberInput()
    {
        var positionLeft = Console.CursorLeft;
        var positionRight = Console.CursorTop;
        int numberInput;
        string? input;
        do
        {
            input = GetInput(positionLeft, positionRight);
        } while (!int.TryParse(input, out numberInput));

        return numberInput;
    }

    public DateOnly GetDateInput()
    {
        var positionLeft = Console.CursorLeft;
        var positionRight = Console.CursorTop;
        DateOnly dateInput;
        string? input;
        do
        {
            input = GetInput(positionLeft, positionRight);
        } while (!DateOnly.TryParse(input, out dateInput));

        return dateInput;
    }

    private string GetInput(int positionLeft, int positionRight)
    {
        string? input;
        do
        {
            Console.SetCursorPosition(positionLeft, positionRight);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(positionLeft, positionRight);
            Console.Write("> ");
            input = consoleWrapper.ReadLine();
        } while (input is null || input.Trim() == string.Empty);

        return input.Trim();
    }
}