using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCSharpAcademy.HabitTracker.Models;

namespace TheCSharpAcademy.HabitTracker
{
  class UI
  {
    #region properties

    #endregion
    #region constructors
    public UI()
    {
      MainMenu();
    }
    #endregion
    #region methods
    /// <summary>
    /// Displays the main menu and prompts the user for an action.
    /// </summary>
    void MainMenu()
    {
      Console.WriteLine("Welcome to the Habit Tracker!");
      Console.WriteLine("Please select an option:");
      Console.WriteLine("1. Add a habit");
      Console.WriteLine("2. Remove a habit");
      Console.WriteLine("3. Update a habit");
      Console.WriteLine("4. View habits");
      Console.WriteLine("5. Log a habit");
      Console.WriteLine("6. View this months occurences of a habit");
      Console.WriteLine("7. Exit");

      string? choice = Console.ReadLine();
      switch (choice)
      {
        case "1":
          AddHabit();
          break;
        case "2":
          RemoveHabit();
          break;
        case "3":
          UpdateHabit();
          break;
        case "4":
          ViewHabitsMainmenu();
          break;
        case "5":
          LogHabitOccurence();
          break;
        case "6":
          ViewOccurences();
          break;
        case "7":
          Environment.Exit(0);
          break;
        default:
          Console.WriteLine("Invalid choice, please try again.");
          MainMenu();
          break;
      }
      #endregion
    }

    /// <summary>
    /// Prompts the user to log a habit occurrence.
    /// </summary>
    void LogHabitOccurence()
    {
      ViewHabits();

      int id;
      while (true)
      {
        Console.WriteLine("Enter the ID of the habit to log an occurrence:");
        string? idInput = Console.ReadLine();
        if (int.TryParse(idInput, out id))
          break;
        Console.WriteLine("Invalid ID. Please enter a valid positive integer.");
      }

      string? date;
      while (true)
      {
        Console.WriteLine("Enter the date of the occurrence (DD-MM-YYYY):");
        date = Console.ReadLine();
        if (DateTime.TryParseExact(date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
          break;
        Console.WriteLine("Invalid date format. Please use DD-MM-YYYY.");
      }

      double value;
      while (true)
      {
        Console.WriteLine("Enter the value of the occurrence:");
        string? valueInput = Console.ReadLine();
        if (double.TryParse(valueInput, out value))
          break;
        Console.WriteLine("Invalid value. Please enter a valid number.");
      }

      Occurence occurence = new(date, id, value);

      if (occurence.LogOccurence(occurence))
      {
        Console.WriteLine("Occurrence logged successfully.");
      }
      else
      {
        Console.WriteLine("Failed to log occurrence.");
      }

      MainMenu();


    }

    /// <summary>
    /// Displays the habits and then returns to the main menu.
    /// </summary>
    void ViewHabitsMainmenu()
    {
      ViewHabits();
      MainMenu();
    }
    /// <summary>
    /// Calculates the current month's occurrences for a given habit.
    /// </summary>
    /// <param name="habit"></param>
    /// <returns></returns>
   static int CalculateCurrentMonthOccurences(Habit habit)
    {
      int value = 0;

      DatabaseHandler databaseHandler = new();
      databaseHandler.Connect();

      List<Occurence> occurences = databaseHandler.GetOccurencesForCurrentMonthByHabit(habit);

      foreach(Occurence occurence in occurences)
      {
        value += (int)occurence.value;
      }

      return value;
    }
    /// <summary>
    /// Retrieves all occurrences for the current month and displays them to the user.
    /// </summary>
    void ViewOccurences()
    {
      Console.WriteLine("Enter the ID of the habit to view occurrences:");
      ViewHabits();
      int id = Convert.ToInt32(Console.ReadLine());
      DatabaseHandler databaseHandler = new();
      databaseHandler.Connect();
      List<Occurence> occurences = databaseHandler.GetOccurencesForCurrentMonthByHabit(databaseHandler.GetHabitById(id));
      foreach (var occurence in occurences)
      {
        Console.WriteLine($"ID: {occurence.id}, Date: {occurence.Date}, Habit: {databaseHandler.GetHabitById(id).Habitname}, Value: {occurence.value}");
        Console.WriteLine("");
      }
      databaseHandler.Close();
      MainMenu();
    }
    /// <summary>
    /// Retrieves all habits from the database and displays them to the user.
    /// </summary>
    static void ViewHabits()
    {
      DatabaseHandler databaseHandler = new();
      databaseHandler.Connect();
      List<Habit> habits = databaseHandler.GetAllHabits();
      foreach (var habit in habits)
      {
        Console.WriteLine($"ID: {habit.Id}, Name: {habit.Habitname}, Unit: {habit.MeasuringUnit}, Current month occurences: {CalculateCurrentMonthOccurences(habit)}");
        Console.WriteLine("");
      }
    }
    /// <summary>
    /// Prompts the user to enter habit details and updates the habit in the database.
    /// </summary>
    void UpdateHabit()
    {
      DatabaseHandler databaseHandler = new();
      databaseHandler.Connect();
      ViewHabits();
      Console.WriteLine("Enter the ID of the habit to update:");
      int id = Convert.ToInt32(Console.ReadLine());
      Console.WriteLine("Enter the new name of the habit:");
      string? name = Console.ReadLine();
      Console.WriteLine("Enter the new measuring unit:");
      string? unit = Console.ReadLine();
      Console.WriteLine("Enter the new amount:");
      double amount = Convert.ToDouble(Console.ReadLine());
      Habit oldHabit = databaseHandler.GetHabitById(id);
      Habit newHabit = new(name, unit, amount);
      if (oldHabit.UpdateHabit(oldHabit, newHabit))
      { Console.WriteLine("Habit updated succesfully"); }
      ViewHabitsMainmenu();
    }

    /// <summary>
    /// Prompts the user to enter the ID of the habit to remove and removes it from the database.
    /// </summary>
    void RemoveHabit()
    {
      DatabaseHandler databaseHandler = new();
      databaseHandler.Connect();
      ViewHabits();
      Console.WriteLine("Enter the ID of the habit to remove:");
      int id = Convert.ToInt32(Console.ReadLine());
      Habit habit = new("", "", 0);
      if (habit.RemoveHabit(databaseHandler.GetHabitById(id)))
      { Console.WriteLine("Habit removed succesfully"); }
      ViewHabitsMainmenu();

    }

    /// <summary>
    /// Prompts the user to enter habit details and adds the habit to the database.
    /// </summary>
    void AddHabit()
    {
      Console.WriteLine("Enter the name of the habit:");
      string? name = Console.ReadLine();
      Console.WriteLine("Enter the measuring unit:");
      string? unit = Console.ReadLine();
      Console.WriteLine("Enter the amount:");
      double amount = Convert.ToDouble(Console.ReadLine());
      Habit habit = new(name, unit, amount);
      if (!habit.AddHabit(habit))
      {
        Console.WriteLine("Habit not created");
      }
      MainMenu();
    }
  }
}