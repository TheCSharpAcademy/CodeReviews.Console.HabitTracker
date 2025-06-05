namespace Main.UI
{
    using Spectre.Console;
    using System;
    using static Main.Enums;

    internal static class MenuService
    {
        public static MenuChoice ShowMainMenu()
        {
            Console.Clear();
            return AnsiConsole.Prompt(new SelectionPrompt<MenuChoice>().Title("[red]Main menu[/]").AddChoices(Enum.GetValues<MenuChoice>()));
        }
    }
}
