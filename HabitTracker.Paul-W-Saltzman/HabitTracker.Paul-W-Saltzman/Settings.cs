using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Paul_W_Saltzman
{
    internal class Settings
    {
        public int ID { get; set; }
        public int Version { get; set; }
        public bool TestMode { get; set; }
        public int Theme { get; set; }

        internal static Settings ToggleTestMode(Settings settings)
        {
            settings = Data.GetSettings();
                Data.ToggleTest(settings);
            settings = Data.GetSettings();
            return settings;
        }
        public static Settings ProgramSettings(Settings settings)
        {
            bool inMenu = true;
            while (inMenu)
            {

                Console.Clear();
                Console.WriteLine("_____________________");
                Console.WriteLine($@"Vwesion: {settings.Version}");
                Console.WriteLine($@"Test Mode: {settings.TestMode}");
                Console.WriteLine($@"Theme: {settings.Theme}");
                Console.WriteLine("_____________________");
                Console.WriteLine("Options: X to Exit : T to toggle test mode : S to switch theme :");
                string userInput = Console.ReadLine();
                userInput = userInput.Trim().ToLower();

                switch (userInput)
                {
                    case "x":
                        inMenu = false;
                        break;
                    case "t":
                        settings = Settings.ToggleTestMode(settings);
                        break;
                    case "s":
                        settings = Settings.ChangeTheme(settings);
                        break;
                    default: break;
                }

            }
            return settings;

        }

        internal static Settings ChangeTheme(Settings settings)
        {
            bool exitPage = false;
            while (!exitPage)
            {
                Console.Clear();
                Console.WriteLine("____________________________");
                Console.WriteLine("1: black and white");
                Console.WriteLine("2: white and black");
                Console.WriteLine("3: black and red");
                Console.WriteLine("4: white and red");
                Console.WriteLine("5: black and blue");
                Console.WriteLine("6: white and blue");
                Console.WriteLine("____________________________");
                Console.WriteLine("Options: X to Exit");
                string userInput = Console.ReadLine();
                userInput = userInput.Trim().ToLower();
                switch (userInput)
                {
                    case "x": 
                        exitPage = true;
                        break;
                    case "1":
                        Console.ResetColor();
                        Data.SaveTheme(1);
                        break;
                    case "2":
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Data.SaveTheme(2);
                        break;
                    case "3":
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Data.SaveTheme(3);
                        break;
                    case "4":
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Data.SaveTheme(4);
                        break;
                    case "5":
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Data.SaveTheme(5);
                        break;
                    case "6":
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Data.SaveTheme(6);
                        break;
                    default: break;

                }
            }
            return settings;

        }

        internal static void SetTheme(Settings settings)
        {
            switch (settings.Theme) 
            {

                case 1:
                    Console.ResetColor();
                    Data.SaveTheme(1);
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Data.SaveTheme(2);
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Data.SaveTheme(3);
                    break;
                case 4:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Data.SaveTheme(4);
                    break;
                case 5:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Data.SaveTheme(5);
                    break;
                case 6:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Data.SaveTheme(6);
                    break;
                default: break;

                }
        }



    }
 }

