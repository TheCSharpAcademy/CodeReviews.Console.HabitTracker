using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Mo3ses
{
    public static class Menu
    {
        public static void ShowMenu()
        {
            var menuData = new List<List<object>>
            {
                new List<object> {"1 - TRACK HABIT"},
                new List<object>{ "2 - SHOW HABITS"},
                new List<object>{ "3 - CREATE HABIT"},
                new List<object>{ "4 - UPDATE HABIT"},
                new List<object>{ "5 - DELETE HABIT"},
                new List<object>{ "6 - HABITS REPORT"},
                new List<object>{ "0 - Exit"},
            };

            ConsoleTableBuilder.From(menuData)
               .WithFormat(ConsoleTableBuilderFormat.MarkDown)
               .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 0, TextAligntment.Left },
                    { 1, TextAligntment.Left },
                    { 3, TextAligntment.Left },
                    { 100, TextAligntment.Left }
               })
               .WithMinLength(new Dictionary<int, int> {
                    { 1, 30 }
               })
               .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
               .WithTitle("MENU", ConsoleColor.Green, ConsoleColor.DarkGray, TextAligntment.Left)
               .WithFormatter(1, (text) =>
               {
                   return text.ToString().ToUpper().Replace(" ", "-") + " «";
               })
               .ExportAndWriteLine(TableAligntment.Left);
        }
    }

}
