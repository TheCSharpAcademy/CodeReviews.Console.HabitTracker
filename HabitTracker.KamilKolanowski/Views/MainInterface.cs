using HabitTracker.KamilKolanowski.Data;

namespace HabitTracker.KamilKolanowski.Views;

public class MainInterface
{
    public void Start()
    {
            DatabaseManager db = new();
            HabitsPresentator hp = new();
            var connection = db.OpenConnection();
            
            db.CreateDatabaseIfNotExists();
            db.CreateHabitLoggerTable(connection);

            string[] menuOptions = new [] { "Add Habit", "Delete Habit", "Update Habit", "List Habit(s)", "Get Report", "Quit" };
            int menuSelect = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                hp.PresentTitle();
                
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (i == menuSelect)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(menuOptions[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(menuOptions[i]); 
                    }
                }

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    if (menuSelect == menuOptions.Length - 1)
                    {
                        menuSelect = 0;
                    }
                    else
                    {
                        menuSelect++;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    if (menuSelect == 0)
                    {
                        menuSelect = menuOptions.Length - 1;
                    }
                    else
                    {
                        menuSelect--;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    switch (menuSelect)
                    {
                        case 0:
                            Console.Clear();
                            hp.PresentTitle();
                            db.AddHabit(connection);
                            break;

                        case 1:
                            hp.PresentHabits(db, connection);
                            db.DeleteHabit(connection);
                            break;

                        case 2:
                            hp.PresentHabits(db, connection);
                            db.UpdateHabit(connection); 
                            break;

                        case 3:
                            hp.PresentHabits(db, connection);
                            Console.WriteLine("Press any key to go back...");
                            Console.ReadKey();
                            break;

                        case 4:
                            hp.PresentReport(db, connection);
                            Console.WriteLine("Press any key to go back...");
                            Console.ReadKey();
                            break;
                        
                        case 5:
                            db.CloseConnection(connection);
                            return;
                    }
                }
            }
    }
}