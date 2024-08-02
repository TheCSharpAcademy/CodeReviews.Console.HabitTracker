using System.Diagnostics.CodeAnalysis;

namespace HabitLogger
{
    internal class Helpers
    {
        public static bool IsString(string str)
        {
            foreach(char c in str)
            {
                if (!char.IsLetter(c))   
                {
                    if (c ==' ')
                    {
                        continue;
                    }
                    return false;
                }
            }
            return true;
        }

        public static bool IsNumeric(string str)
        {
            foreach(char c in str)
            {
                if (char.IsLetter(c))
                {
                    if (c == '.')
                    {
                        continue;
                    }
                    return false;
                }
            }
            return true;
        }

        public static bool IsNotNull([NotNullWhen(true)] object? obj) => obj != null;
    }
}
