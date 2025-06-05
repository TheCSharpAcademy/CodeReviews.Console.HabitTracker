using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Main.Enums;

namespace Main.UI
{
    internal static class MenuService
    {
        public static MenuChoice ShowMainMenu()
        {
            Console.Clear();
            return AnsiConsole.Prompt(new SelectionPrompt<MenuChoice>().Title("[red]Main menu[/]").AddChoices(Enum.GetValues<MenuChoice>()));
        }
    }
}
