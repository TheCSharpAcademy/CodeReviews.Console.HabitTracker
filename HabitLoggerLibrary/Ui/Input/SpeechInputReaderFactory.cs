using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Options;

namespace HabitLoggerLibrary.Ui.Input;

public class SpeechInputReaderFactory(IOptions<SpeechRecognizerFactoryOptions> options)
    : ISpeechInputReaderFactory
{
    private SpeechRecognizer? _recognizer;

    public SpeechInputReader Create()
    {
        _recognizer ??=
            new SpeechRecognizer(SpeechConfig.FromSubscription(options.Value.SubscriptionKey, options.Value.Region));

        return new SpeechInputReader(_recognizer);
    }
}