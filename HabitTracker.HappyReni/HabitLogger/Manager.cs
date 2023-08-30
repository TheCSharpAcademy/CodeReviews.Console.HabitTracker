namespace HabitLogger
{
    public class Manager
    {
        private SELECTOR Selector { get; set; }
        private SQLite SQL { get; set; }
        public Manager()
        {
            SQL = new();
            MainMenu();
        }

        private void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Habit Logger");
            Console.WriteLine("".PadRight(24, '='));
            Console.WriteLine("1. Register your habit.");
            Console.WriteLine("2. Insert a log");
            Console.WriteLine("3. Delete a log");
            Console.WriteLine("4. Update a log");
            Console.WriteLine("5. Drop a habit");
            Console.WriteLine("6. View habits");
            Console.WriteLine("0. Exit\n");
            Selector = (SELECTOR)GetInput("Select ").val;
            Action(Selector);
        }
        private void Action(SELECTOR selector)
        {
            switch (selector)
            {
                case SELECTOR.REGISTER:
                    Register();
                    break;
                case SELECTOR.INSERT:
                    Insert();
                    break;
                case SELECTOR.DELETE:
                    Delete();
                    break;
                case SELECTOR.UPDATE:
                    Update();
                    break;
                case SELECTOR.VIEW:
                    ViewTheHabits();
                    break;
                case SELECTOR.DROP:
                    Drop();
                    break;
                case SELECTOR.EXIT:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
        }
        private void Register()
        {
            Console.Clear();
            var name = GetInput("Input the name of the habit.").str;

            SQL.CreateTable($"\"{name}\"");
            GoToMainMenu("Register Completed.");
        }
        private void Insert()
        {
            ViewTables();

            try
            {
                var table = IsTable(GetInput("Input the name of the table to delete a log.").str);
                if (table == null) throw new Exception();
                var log = GetInput("Write the log.").str;
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SQL.Insert($"\"{table}\"", $"\"{time}\"", $"\"{log}\"");
            }
            catch
            {
                Console.WriteLine("Invalid Input. Try again.");
            }

            GoToMainMenu("Type any keys to continue.");
        }
        private void Delete()
        {
            ViewTables();

            try
            {
                var table = IsTable(GetInput("Input the name of the table to delete a log.").str);
                if (table == null) throw new Exception();
                var input = GetInput("Select the index of the log to delete");
                SQL.Delete($"\"{table}\"", input.val);
            }
            catch
            {
                Console.WriteLine("Invalid Input. Try again.");
            }

            GoToMainMenu("Type any keys to continue.");
        }

        private void Drop()
        {
            ViewTables();

            var table = GetInput("Input the name of the table to drop.").str;
            SQL.DropTable($"\"{table}\"");
            GoToMainMenu();
        }

        private void Update()
        {
            ViewTables();

            try
            {
                var table = IsTable(GetInput("Input the name of the table to update a log.").str);
                if (table == null) throw new Exception();
                var idx = GetInput("Select the index of the log to update");
                var input = GetInput("Input the new log");
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                SQL.Update($"\"{table}\"", $"\"{time}\"", $"\"{input.str}\"", idx.val);
            }
            catch
            {
                Console.WriteLine("Invalid Input. Try again.");
            }

            GoToMainMenu("Type any keys to continue.");
        }
        private void ViewTheHabits()
        {
            ViewTables();
            GoToMainMenu("Type any keys to continue.");
        }

        private void GoToMainMenu(string message = "")
        {
            WaitForInput(message);
            MainMenu();
        }

        private void ViewTables()
        {
            Console.Clear();
            SQL.ViewTables();
            Console.WriteLine("".PadRight(24, '='));
        }

        private (bool res, string str, int val) GetInput(string message)
        {
            // This function returns string input too in case you need it
            int number;
            Console.WriteLine(message);
            Console.Write(">> ");
            string str = Console.ReadLine();
            var res = int.TryParse(str, out number);

            number = res ? number : (int)SELECTOR.INVALID_SELECT;
            str = str == null ? "" : str;

            return (res, str, number);
        }
        private void WaitForInput(string message = "")
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        private string? IsTable(string table) => SQL.IsTable(table) ? table : null;

    }
}
