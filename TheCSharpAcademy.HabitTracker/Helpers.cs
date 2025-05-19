using System;
using System.Linq;

namespace TheCSharpAcademy.HabitTracker
{
  static class Helpers
  {
    /// <summary>
    /// Takes a date string and returns a string array [day, month, year].
    /// </summary>
    /// <param name="date"></param>
    /// <returns>string[] { day, month, year }</returns>
    public static string[] SliceDateToParts(string date)
    {
      DateTime parsedDate;
      if (string.IsNullOrWhiteSpace(date))
      { Console.WriteLine($"An error has occured parsing the date. {date}"); }

      try
      {
      parsedDate = DateTime.Parse(date);
      }catch (FormatException ex)
      {
        Console.WriteLine($"An error has occured parsing the date. {date}");
        Console.WriteLine(ex.Message);
        return new string[] { "00", "00", "0000" };
      }
      return new string[]
      {
        parsedDate.Day.ToString("D2"),
        parsedDate.Month.ToString("D2"),
        parsedDate.Year.ToString()
      };
    }
  }
}
