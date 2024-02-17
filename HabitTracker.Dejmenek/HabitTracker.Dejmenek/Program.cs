using ConsoleTables;
using HabitTracker.Dejmenek.DataAccess.Repositories;
using HabitTracker.Dejmenek.Enums;
using HabitTracker.Dejmenek.Helpers;
using HabitTracker.Dejmenek.Models;

namespace HabitTracker.Dejmenek
{
    internal class Program
    {
        public static void Main() {
            string connection_string = @"Data Source=habit-Tracker.db";
            bool exit = false;

            HabitRepository habitRepository = new HabitRepository(connection_string);

            do
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("Type 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");

                MenuOptions userOption = UserInput.GetMenuOption();

                switch (userOption) {
                    case MenuOptions.Close:
                        exit = true;
                        break;

                    case MenuOptions.ViewAll:
                        List<Habit> habits = habitRepository.GetAllHabits();

                        var table = new ConsoleTable("Id", "Name", "Description", "Quantity", "QuantityUnit", "Date");

                        foreach (var habit in habits) {
                            table.AddRow(habit.Id, habit.Name, habit.Description, habit.Quantity, habit.QuantityUnit, habit.Date);
                        }

                        table.Write();
                        break;

                    case MenuOptions.Insert:
                        Console.WriteLine("Enter habit's name:");
                        string habitName = UserInput.GetString();

                        Console.WriteLine("Enter habit's description:");
                        string habitDescription = UserInput.GetString();

                        Console.WriteLine("Enter habit's date:");
                        string habitDate = UserInput.GetHabitDate();

                        Console.WriteLine("Enter habit's quantity:");
                        int habitQuantity = UserInput.GetNumber();

                        Console.WriteLine("Enter habit's quantity unit:");
                        string habitQuantityUnit = UserInput.GetString();

                        habitRepository.AddHabit(habitName, habitDescription, habitDate, habitQuantity, habitQuantityUnit);
                        break;

                    case MenuOptions.Delete:
                        Console.WriteLine("Enter habit's id:");
                        int habitId = UserInput.GetNumber();

                        habitRepository.DeleteHabit(habitId);
                        break;

                    case MenuOptions.Update:
                        Console.WriteLine("Enter habit's id:");
                        habitId = UserInput.GetNumber();

                        Console.WriteLine("Enter habit's date:");
                        habitDate = UserInput.GetHabitDate();

                        Console.WriteLine("Enter habit's quantity:");
                        habitQuantity = UserInput.GetNumber();

                        habitRepository.UpdateHabit(habitId, habitDate, habitQuantity);
                        break;
                }
            } while (!exit);
        }
    }
}
