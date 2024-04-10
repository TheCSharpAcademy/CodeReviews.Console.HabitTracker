using DatabaseLibrary;

namespace HabitLogger.BBualdo;

internal class Engine
{
  public bool IsOn { get; set; }
  public DbContext db { get; set; }

  public Engine()
  {
    IsOn = true;
    db = new DbContext();
  }

  public void MainMenu()
  {
    Console.Clear();
    Console.WriteLine("------------------");
    Console.WriteLine("---HABIT LOGGER---");
    Console.WriteLine("------------------");
    Console.WriteLine("\nMAIN MENU");
    Console.WriteLine("\nWhat what would you like to do?");
    Console.WriteLine(@"
      0 - Close App
      1 - Create Habit
      2 - Get All Records
      3 - Insert Record
      4 - Update Record
      5 - Delete Record
      6 - Delete Habit
      7 - Get Summary

Type number and press Enter:");

    SelectOption();
  }

  public void SelectOption()
  {
    int menuOption;
    string? userInput = Console.ReadLine();

    while (!int.TryParse(userInput, out menuOption))
    {
      Console.WriteLine("\nPlease enter valid number.\n");
      userInput = Console.ReadLine();
    }

    switch (menuOption)
    {
      case 0:
        Console.WriteLine("\nSee you soon! Keep working on your habits!\n");
        IsOn = false; break;
      case 1:
        db.CreateHabit();
        break;
      case 2:
        db.GetAllRecords();
        break;
      case 3:
        db.InsertRecord();
        break;
      case 4:
        db.UpdateRecord();
        break;
      case 5:
        db.DeleteRecord();
        break;
      case 6:
        db.DeleteHabit();
        break;
      case 7:
        db.GetSummary();
        break;
      default:
        Console.WriteLine("Invalid input!");
        break;
    }
  }
}