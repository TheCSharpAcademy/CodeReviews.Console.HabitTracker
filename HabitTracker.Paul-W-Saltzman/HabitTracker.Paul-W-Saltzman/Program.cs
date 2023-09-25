using HabitTracker.Paul_W_Saltzman;
using Microsoft.Data.Sqlite;

internal class Program
{
    
    private static void Main(string[] args)
    {
        Data.Init();
        Menu.GetMenu();

    }
}