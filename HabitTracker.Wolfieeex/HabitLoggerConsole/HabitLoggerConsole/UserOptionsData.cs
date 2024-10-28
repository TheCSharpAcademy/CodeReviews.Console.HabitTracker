using Newtonsoft.Json;

namespace HabitLoggerConsole;

internal class UserOptionsData
{
    internal static string path = @"User options\Report selection options.json";

    internal static void RetreiveUserDefaultOptions()
    {
        bool[] previousOptions;

        if (File.Exists(path))
        {
            using (JsonTextReader jsonTextReader = new JsonTextReader(File.OpenText(path)))
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                previousOptions = jsonSerializer.Deserialize<bool[]>(jsonTextReader);
            }
        }
        else
        {

        }
    }
}
