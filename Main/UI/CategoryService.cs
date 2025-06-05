using Main.Data;
using Main.Models;
using Spectre.Console;

namespace Main.UI
{
    internal class CategoryService() : CrudMenu("Category")
    {

        override protected void ViewAll()
        {
            Console.Clear();
            var running = true;
            while (running)
            {
                var choice = PickCategory();
                if (choice == null)
                {
                    return;
                }
                if (choice.Id == 0)
                {
                    running = false;
                    break;
                }
                EditMode(choice);
            }
        }
        override protected void Insert()
        {
            Category category = new();
            var name = GetString("Please enter the name for the habit", "Habit name");
            if (InputTerminated(name))
            {
                return;
            }
            category.Name = name;
            Console.Clear();
            var unit = GetString($"How is {name} measured?", "Measurement");
            if (InputTerminated(unit))
            {
                return;
            }
            category.Unit = unit;
            CategoryDatabase.Insert(category);
            AnsiConsole.MarkupLine("[green]Category added successfully![/]");
            Console.ReadKey();
        }
        static void Update(Category category)
        {
            var name = GetString("Please enter a new name for the habit", "Habit name", category.Name);
            if (InputTerminated(name))
            {
                return;
            }
            category.Name = name;
            Console.Clear();
            var unit = GetString($"How is {name} measured?", "Measurement", category.Unit);
            if (InputTerminated(unit))
            {
                return;
            }
            category.Unit = unit;
            var affected = CategoryDatabase.Update(category);
            if (affected == 0)
            {
                AnsiConsole.MarkupLine("[red]Update not successful[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Habit category successfully updated[/]");
            }
            Console.ReadKey();
        }
        static void Delete(Category category)
        {
            var affected = CategoryDatabase.Delete(category.Id);
            if (affected == 0)
            {
                AnsiConsole.MarkupLine("[red]Delete not successful[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Habit category successfully deleted[/]");
            }
            Console.ReadKey();
        }

        static void EditMode(Category category)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"Selected habit category: [red]{category.Name}[/]");
            AnsiConsole.WriteLine("What would you like to do?");
            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new List<string>() { "[blue]--BACK--[/]", "Edit", "Delete" }));
            switch (choice)
            {
                case "--BACK--":
                    return;
                case "Edit":
                    Update(category);
                    break;
                case "Delete":
                    Delete(category);
                    break;
            }
        }

    }
}
