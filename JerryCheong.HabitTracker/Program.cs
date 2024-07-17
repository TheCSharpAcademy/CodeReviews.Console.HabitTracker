using System;
using Microsoft.Data.Sqlite;
using System.IO;
using ConnectionLibrary;
using Terminal.Gui;
using TerminalGUILibrary;

namespace JerryCheong.HabitTracker
{
    class Program
    {
        private Features? _selectedFeatures = null;

        static void Main(string[] args)
        {
            Features features;

            Program program = new Program();
            program.RunMainMenu();

            if (MainMenu.isSeeded)
            {
                ConnectionService connection = new ConnectionService("SeededDb.db");
                connection.Init();
            }
            else if (!MainMenu.isSeeded)
            {
                ConnectionService connection = new ConnectionService();
                connection.Init();
            }

            if (MainMenu.isStart)
            {
                while ((features = program.RunMainLoop()) != null)
                {
                    features.Init();
                    features.Setup();
                    features.Run();

                    // Before the application exits, reset Terminal.Gui for clean shutdown
                    Application.Shutdown();
                }
            }
        }

        void RunMainMenu()
        {
            Application.Init();
            Application.Run<MainMenu>();

            Application.Shutdown();
        }
        Features RunMainLoop()
        {
            HabitTrackerTopLevel mainloop = new HabitTrackerTopLevel();
            Application.Init();
            _selectedFeatures = mainloop.Run();

            Application.Shutdown();

            return _selectedFeatures;
        }
    }
}