namespace HabitLogger.batus3010
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Action DisplayMenu = () => Console.WriteLine(@"___________________________________
Main Menu
What would you like to do?

Type 0 to Close Application.
Type 1 to View all Records
Type 2 to Insert Record.
Type 3 to Delete Record
Type 4 to Update Record.
-----------------------------------");
            DBContext db = new DBContext();


            while (true)
            {
                DisplayMenu();
                string input = ColoredConsole.ColoredConsole.Prompt("Enter your choice:");
                if (int.TryParse(input, out int choice) == false)
                {
                    ColoredConsole.ColoredConsole.WriteLine("Invalid Input", ConsoleColor.Red);
                    continue;
                }
                Console.WriteLine();
                switch (choice)
                {
                    case 0:
                        Console.WriteLine("Closing...");
                        return;
                    case 1:
                        if (db.IsEmpty())
                        {
                            Console.WriteLine("Database is empty, create something!");
                            break;
                        }
                        else
                            db.ViewAllRecords();
                        break;
                    case 2:
                        db.InsertIntoDatebase(GetHabitInput());
                        Console.WriteLine("sucessfully inserted.");
                        break;
                    case 3:
                        string id_delete_str = ColoredConsole.ColoredConsole.Prompt("Type the id of the habit you want to delete: ");

                        int.TryParse(id_delete_str, out int id_delete);

                        if (db.IsIdInDatabase(id_delete))
                        {
                            db.DeleteFromDatabase(id_delete);
                            ColoredConsole.ColoredConsole.WriteLine($"successfully deleted habit {id_delete}", ConsoleColor.Green);
                        }
                        else
                        {
                            Console.WriteLine($"id {id_delete} does not exist");
                        }
                        break;
                    case 4:
                        Console.Write("Type the id of the habit you want to update: ");
                        int.TryParse(Console.ReadLine(), out int id_update);

                        if (db.IsIdInDatabase(id_update))
                        {
                            db.UpdateDatabase(GetHabitInput(), id_update);
                            ColoredConsole.ColoredConsole.WriteLine($"successfully updated habit {id_update}", ConsoleColor.Green);
                        }
                        else
                        {
                            Console.WriteLine($"id {id_update} does not exist");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }

            }


        }

        static Habit GetHabitInput()
        {
            Console.Write("What would you like to call this habit? ");
            string? habitName = Console.ReadLine();
            Console.Write("How many times would you like to do this habit? ");
            int.TryParse(Console.ReadLine(), out int quantity);

            return new Habit(habitName, quantity);

        }

    }
}
