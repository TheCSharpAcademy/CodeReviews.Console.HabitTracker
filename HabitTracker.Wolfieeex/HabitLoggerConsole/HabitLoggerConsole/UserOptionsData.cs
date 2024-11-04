using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace HabitLoggerConsole;

internal class UserOptionsData
{
    internal static string path = @"User options\Report selection options.json";
    static private bool[] previousOptions;

    internal static bool[] RetreiveUserDefaultOptions()
    {
        var match = Regex.Match(UserOptionsData.path, @"(.*)(?=\\)");
        System.IO.Directory.CreateDirectory(match.Value);

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
            previousOptions = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                previousOptions[i] = false;
            }

            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter writeStream = File.CreateText(UserOptionsData.path))
            {
                serializer.Serialize(writeStream, previousOptions);
            }
        }

        return previousOptions;
    }
}
