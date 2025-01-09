using Microsoft.CognitiveServices.Speech;

namespace HabitLoggerLibrary.Ui.Input;

public class SpeechInputReader(SpeechRecognizer speechRecognizer) : IInputReader
{
    public string GetStringInput()
    {
        return GetInput();
    }

    public long GetNumberInput()
    {
        int numberInput;
        string? input;
        do
        {
            input = GetInput();
        } while (!int.TryParse(input, out numberInput));

        return numberInput;
    }

    public DateOnly GetDateInput()
    {
        DateOnly dateInput;
        string? input;
        do
        {
            input = GetInput();
        } while (!DateOnly.TryParse(input, out dateInput));

        return dateInput;
    }

    private string GetInput()
    {
        do
        {
            Console.Write("> ");
            var result = speechRecognizer.RecognizeOnceAsync().Result;
            if (result.Reason == ResultReason.Canceled)
            {
                Console.WriteLine($"\nCANCELED: Reason={result.Reason}.");
                continue;
            }

            if (result.Reason != ResultReason.RecognizedSpeech)
            {
                Console.WriteLine("Speech could not be recognized. Please try again.");
                continue;
            }

            var text = result.Text.Substring(0, result.Text.Length - 1);
            if (text.Trim().Length == 0)
            {
                Console.WriteLine("Speech could not be recognized. Please try again.");
                continue;
            }

            return text;
        } while (true);
    }
}