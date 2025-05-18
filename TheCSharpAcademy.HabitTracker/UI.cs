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
      Console.WriteLine("5. Exit");

      string choice = Console.ReadLine();
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
          Environment.Exit(0);
          break;
        default:
          Console.WriteLine("Invalid choice, please try again.");
          MainMenu();
          break;
      }
      #endregion
    }

    void ViewHabitsMainmenu()
    {
      ViewHabits();
      MainMenu();
    }
    void ViewHabits()
    {
      DatabaseHandler databaseHandler = new DatabaseHandler();
      databaseHandler.Connect();
      List<Habit> habits = databaseHandler.GetAllHabits();
      foreach (var habit in habits)
      {
        Console.WriteLine($"ID: {habit.Id}, Name: {habit.Habitname}, Unit: {habit.MeasuringUnit}, Amount: {habit.Amount}");
        Console.WriteLine("");
      }
    }
    /// <summary>
    /// Prompts the user to enter habit details and updates the habit in the database.
    /// </summary>
    void UpdateHabit()
    {
      DatabaseHandler databaseHandler = new DatabaseHandler();
      databaseHandler.Connect();
      ViewHabits();
      Console.WriteLine("Enter the ID of the habit to update:");
      int id = Convert.ToInt32(Console.ReadLine());
      Console.WriteLine("Enter the new name of the habit:");
      string name = Console.ReadLine();
      Console.WriteLine("Enter the new measuring unit:");
      string unit = Console.ReadLine();
      Console.WriteLine("Enter the new amount:");
      double amount = Convert.ToDouble(Console.ReadLine());
      Habit oldHabit = databaseHandler.GetHabitById(id);
      Habit newHabit = new Habit(name, unit, amount);
      if (oldHabit.UpdateHabit(oldHabit, newHabit))
      { Console.WriteLine("Habit updated succesfully"); }
      ViewHabitsMainmenu();
    }

    /// <summary>
    /// Prompts the user to enter the ID of the habit to remove and removes it from the database.
    /// </summary>
    void RemoveHabit()
    {
      ViewHabits();
      Console.WriteLine("Enter the ID of the habit to remove:");
      int id = Convert.ToInt32(Console.ReadLine());
      Habit habit = new Habit("", "", 0);
      if (habit.RemoveHabit(habit))
      { Console.WriteLine("Habit removed succesfully"); }
      ViewHabitsMainmenu();

    }

    /// <summary>
    /// Prompts the user to enter habit details and adds the habit to the database.
    /// </summary>
    void AddHabit()
    {
      Console.WriteLine("Enter the name of the habit:");
      string name = Console.ReadLine();
      Console.WriteLine("Enter the measuring unit:");
      string unit = Console.ReadLine();
      Console.WriteLine("Enter the amount:");
      double amount = Convert.ToDouble(Console.ReadLine());
      Habit habit = new Habit(name, unit, amount);
      if (!habit.AddHabit(habit))
      {
        Console.WriteLine("Habit not created");
      }
      MainMenu();
    }
  }
}