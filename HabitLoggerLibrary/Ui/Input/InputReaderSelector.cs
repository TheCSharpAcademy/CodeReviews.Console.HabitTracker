namespace HabitLoggerLibrary.Ui.Input;

public class InputReaderSelector(
    IInputReaderFactory inputReaderFactory,
    IInputChoiceReader inputChoiceReader)
    : IInputReaderSelector
{
    public IInputReader GetInputReader()
    {
        Console.Clear();
        Console.WriteLine("How do you want to provide input?");
        Console.WriteLine($@"{Convert.ToChar(InputChoice.ConsoleInput)}: Console
{Convert.ToChar(InputChoice.SpeechInput)}: Speech");

        var inputChoice = inputChoiceReader.GetChoice();

        return inputReaderFactory.Create(inputChoice);
    }
}