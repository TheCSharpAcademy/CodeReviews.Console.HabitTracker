using System;

namespace DotNet_SQLite
{
  class Helpers
  {
    public static int splitInteger(string word, string keyword, string errorMessage) {
      try {
        return Convert.ToInt32(word.Split(keyword)[1]);
      }
      catch(System.FormatException) {
        Console.WriteLine(errorMessage);
        return 0;
      }
    }

    public static string splitString(string word, string keyword, string errorMessage) {
      // TODO: Merge both splitString and splitInteger functions using typeof()
      try {
        return Convert.ToString(word.Split(keyword)[1]);
      }
      catch(System.FormatException) {
        Console.WriteLine(errorMessage);
        return "";
      }
    }

    public static TimeSpan splitTime(string word, string keyword, string errorMessage) {
      try {
        var time = word.Split(keyword)[1];
        if(time.Contains(":")) {
          var splitTime = time.Split(":");
          return new TimeSpan(Convert.ToInt32(splitTime[0]), Convert.ToInt32(splitTime[1]), 0);
        }

        else {
          return new TimeSpan(Convert.ToInt32(word.Split(keyword)[1]), 0, 0);
        }
      }
      catch(System.FormatException) {
        Console.WriteLine(errorMessage);
        return new TimeSpan(0, 0, 0);
      }
    }
  }
}
