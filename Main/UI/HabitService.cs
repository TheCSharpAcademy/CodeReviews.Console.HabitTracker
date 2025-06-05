using Main.Data;
using Main.Models;
using Spectre.Console;

namespace Main.UI
{
    internal class HabitService() : CrudMenu("Habit")
    {
        override protected void ViewAll()
        {
            Console.Clear();
            var running = true;
            var secondRunning = false;
            while (running)
            {
                var category = PickCategory();
                if (category == null || category.Id == 0)
                {
                    return;
                }
                secondRunning = true;
                while (secondRunning)
                {
                    var items = HabitDatabase.GetHabitsForCategory(category.Id);
                    if (items.Count == 0)
                    {
                        Console.WriteLine($"No habits created for {category.Name}");
                        Console.ReadKey();
                        secondRunning = false;
                        break;
                    }
                    items.Insert(0, new Habit());
                    Console.Clear();
                    AnsiConsole.MarkupLine($"[blue]{category.Name}[/]");
                    var choice = AnsiConsole.Prompt(new SelectionPrompt<Habit>().AddChoices(items).UseConverter(habit =>
                    {
                        if (habit.Id == 0)
                        {
                            return "[blue]--BACK--[/]";
                        }
                        return $"[bold]{habit.Date}[/] ({habit.Quantity})";
                    }));
                    if (items.IndexOf(choice) == 0)
                    {
                        secondRunning = false;
                        break;
                    }
                    EditMode(choice);
                }
            }
        }
        override protected void Insert()
        {
            Habit habit = new();
            var category = PickCategory();
            if (category == null)
            {
                return;
            }
            if (category.Id == 0)
            {
                return;
            }
            habit.Category = category;
            var date = GetDate("Enter the date of the habit", "Day of habit");
            if (InputTerminated(date))
            {
                return;
            }
            habit.Date = date;
            Console.Clear();
            var quantity = GetNumber($"How much {habit.Category.Unit}?", habit.Category.Unit);
            if (quantity == null)
            {
                return;
            }
            habit.Quantity = (int)quantity;
            HabitDatabase.Insert(habit);
            AnsiConsole.MarkupLine("[green]Habit added successfully![/]");
            Console.ReadKey();
        }
        static void Update(Habit habit)
        {
            if (habit.Category == null)
            {
                AnsiConsole.MarkupLine($"[red]Category not assigned to habit[/]");
                Console.ReadKey();
                return;
            }
            var date = GetDate("Please enter a new date for the habit", "Day of habit", habit.Date);
            if (InputTerminated(date))
            {
                return;
            }

            habit.Date = date;
            Console.Clear();
            var quantity = GetNumber($"How much {habit.Category.Unit}?", habit.Category.Unit, habit.Quantity.ToString());
            if (quantity == null)
            {
                return;
            }
            habit.Quantity = (int)quantity;
            var affected = HabitDatabase.Update(habit);
            if (affected == 0)
            {
                AnsiConsole.MarkupLine("[red]Update not successful[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Habit successfully updated[/]");
            }
            Console.ReadKey();
        }
        static void Delete(Habit habit)
        {
            var affected = HabitDatabase.Delete(habit.Id);
            if (affected == 0)
            {
                AnsiConsole.MarkupLine("[red]Delete not successful[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Habit successfully deleted[/]");
            }
            Console.ReadKey();
        }

        static void EditMode(Habit habit)
        {
            if (habit.Category == null)
            {
                AnsiConsole.MarkupLine($"[red]Category not assigned to habit[/]");
                Console.ReadKey();
                return;
            }
            Console.Clear();
            AnsiConsole.MarkupLine($"Selected {habit.Category.Name} habit from [red]{habit.Date}[/]");
            AnsiConsole.WriteLine("What would you like to do?");
            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new List<string>() { "[blue]--BACK--[/]", "Edit", "Delete" }));
            switch (choice)
            {
                case "--BACK--":
                    return;
                case "Edit":
                    Update(habit);
                    break;
                case "Delete":
                    Delete(habit);
                    break;
            }
        }
    }
}
