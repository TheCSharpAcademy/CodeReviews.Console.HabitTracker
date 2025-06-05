namespace Main.UI
{
    using Main.Data;
    using Main.Models;
    using Spectre.Console;
    using System;
    using System.Globalization;
    using static Main.Enums;

    internal abstract class CrudMenu(string item)
    {
        public void ShowMenu()
        {
            var running = true;
            while (running)
            {
                Console.Clear();
                var choice = AnsiConsole.Prompt(new SelectionPrompt<CrudChoice>().Title($"[red]{string.Concat(item[0].ToString().ToUpper(), item.AsSpan(1))} menu[/]").AddChoices(Enum.GetValues<CrudChoice>()));
                switch (choice)
                {
                    case CrudChoice.ViewAll:
                        this.ViewAll();
                        break;
                    case CrudChoice.Insert:
                        this.Insert();
                        break;
                    case CrudChoice.Exit:
                        running = false;
                        break;
                }
            }
        }

        protected abstract void ViewAll();

        protected abstract void Insert();

        protected static string GetString(string message, string input = "Input", string? defaultValue = null)
        {
            AnsiConsole.MarkupLine(message);
            AnsiConsole.MarkupLine("[italic]Or type in 'exit' at any time to return to the previous screen[/]");
            if (!string.IsNullOrEmpty(defaultValue))
            {
                return AnsiConsole.Ask<string>($"[red]{input}: [/]", defaultValue);
            }
            else
            {
                return AnsiConsole.Ask<string>($"[red]{input}: [/]");
            }
        }

        protected static int? GetNumber(string message, string input = "Input", string? defaultValue = null)
        {
            AnsiConsole.MarkupLine(message);
            AnsiConsole.MarkupLine("[italic]Or type in 'exit' at any time to return to the previous screen[/]");
            string response = string.Empty;
            int result = 0;
            bool validResult = false;
            while (!validResult)
            {
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    response = AnsiConsole.Ask<string>($"[red]{input}: [/]", defaultValue);
                }
                else
                {
                    response = AnsiConsole.Ask<string>($"[red]{input}: [/]");
                }

                if (response == "exit")
                {
                    return null;
                }

                if (!int.TryParse(response, out result) || result < 0)
                {
                    AnsiConsole.MarkupLine("[red]Please input a valid positive integer[/]");
                }
                else
                {
                    validResult = true;
                }
            }

            return result;
        }

        protected static string GetDate(string message, string input = "Input", string? defaultValue = null)
        {
            if (defaultValue == null)
            {
                defaultValue = DateTime.Now.ToString("yyyy-MM-dd");
            }

            AnsiConsole.MarkupLine(message);
            AnsiConsole.MarkupLine("Please insert the date in [red]yyyy-mm-dd[/] format");
            AnsiConsole.MarkupLine("[italic]Or type in 'exit' at any time to return to the previous screen[/]");
            string formattedDate = string.Empty;
            formattedDate = AnsiConsole.Ask<string>($"[red]{input}: [/]", defaultValue);
            while (!DateOnly.TryParseExact(formattedDate, "yyyy-mm-dd", new CultureInfo("lt-LT"), DateTimeStyles.None, out _))
            {
                if (formattedDate == "exit")
                {
                    return formattedDate;
                }

                AnsiConsole.MarkupLine("[red]Invalid date[/]");
                formattedDate = AnsiConsole.Ask<string>("Please insert the date in [red]yyyy-mm-dd[/] format. \n[italic]Type 0 to return to the main menu[/]\nDate: ");
            }

            return formattedDate;
        }

        protected static Category? PickCategory()
        {
            var items = CategoryDatabase.GetAll();
            if (items.Count == 0)
            {
                Console.WriteLine("No habit categories created");
                Console.ReadKey();
                return null;
            }

            items.Insert(0, new Category());
            Console.Clear();
            Console.WriteLine("List of habit categories");
            return AnsiConsole.Prompt(new SelectionPrompt<Category>().AddChoices(items).UseConverter(category =>
            {
                if (category.Id == 0)
                {
                    return "[blue]--BACK--[/]";
                }

                return $"[bold]{category.Name}[/] ({category.Unit})";
            }));
        }

        protected static bool InputTerminated(string input)
        {
            return input == "exit";
        }
    }
}
