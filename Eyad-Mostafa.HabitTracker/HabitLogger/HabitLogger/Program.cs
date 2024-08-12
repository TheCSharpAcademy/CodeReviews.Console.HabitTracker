namespace HabitLogger;

class Program
{
    public static void Main(string[] args)
    {
        DataBaseManager.CreateDataBase();
        Menu.ShowMenu();
    }
}