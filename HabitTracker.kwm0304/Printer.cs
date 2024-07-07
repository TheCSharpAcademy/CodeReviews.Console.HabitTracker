

using System.Text.RegularExpressions;
using HabitTracker.kwm0304.Models;

namespace HabitTracker.kwm0304;

public class Printer
{
  public static void PrintMainMenu()
  {
    PrintHeader();
    PrintMenuOptions();
    string response = Console.ReadLine() ?? string.Empty;
    if (string.IsNullOrEmpty(response) || !Regex.IsMatch(response.ToUpper(), "[A|V|E]"))
    {
      Console.WriteLine("Please choose a valid option");
      PrintMenuOptions();
    }
    HandleMenuChoice(response);
  }

    private static void HandleMenuChoice(string response)
    {
        switch (response)
        {
          case "A":
          AddNewHabit();
          break;
          case "V":
          DisplayCurrentHabitsMenu();
          break;
          case "E":
          Console.WriteLine("Goodbye");
          break;
        }
    }

    private static void DisplayCurrentHabitsMenu()
    {
      DisplayHabits();
        throw new NotImplementedException();
    }

    private static bool IsValidOption(string response)
    {
        throw new NotImplementedException();
    }

    private static void PrintHeader()
  {
    Console.WriteLine(@"
 __    __       ___      .______    __  .___________.   .___________..______          ___       ______  __  ___  _______ .______      
|  |  |  |     /   \     |   _  \  |  | |           |   |           ||   _  \        /   \     /      ||  |/  / |   ____||   _  \     
|  |__|  |    /  ^  \    |  |_)  | |  | `---|  |----`   `---|  |----`|  |_)  |      /  ^  \   |  ,----'|  '  /  |  |__   |  |_)  |    
|   __   |   /  /_\  \   |   _  <  |  |     |  |            |  |     |      /      /  /_\  \  |  |     |    <   |   __|  |      /     
|  |  |  |  /  _____  \  |  |_)  | |  |     |  |            |  |     |  |\  \----./  _____  \ |  `----.|  .  \  |  |____ |  |\  \----.
|__|  |__| /__/     \__\ |______/  |__|     |__|            |__|     | _| `._____/__/     \__\ \______||__|\__\ |_______|| _| `._____|
                                                                                                                                      
");
  }
  private static void PrintMenuOptions()
  {
    Console.WriteLine("[A]dd a new habit");
    Console.WriteLine("[V]iew current habits");
    Console.WriteLine("[E]xit the program");
  }

    private static void AddNewHabit()
    {
        throw new NotImplementedException();
    }


    private static void DisplayHabits()
    {
      
    }

}
