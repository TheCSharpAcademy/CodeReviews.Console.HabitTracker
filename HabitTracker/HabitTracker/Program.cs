using System;

namespace HabitTracker
{
    public class Program
    {
        static void Main(string[] args)
        {
            App.CreateDatabase();
            Menu menu = new Menu();
            menu.ProjectMenu();
        }
    }
}