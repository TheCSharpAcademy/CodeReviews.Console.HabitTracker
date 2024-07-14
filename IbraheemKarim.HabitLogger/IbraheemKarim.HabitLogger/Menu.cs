namespace IbraheemKarim.HabitLogger
{
    public static class Menu
    {
        public static void StartMenu()
        {
            while (true)
            {
                ActionType action = GetDesiredAction();
                ActionsProcessor.ProcessActionType(action);
            }
        }

        private static ActionType GetDesiredAction()
        {
            bool firstIteration = true;
            do
            {
                Console.Clear();

                if (!firstIteration)
                    Console.WriteLine("Invalid input!! \n");

                ShowMenu();

                if (int.TryParse(Console.ReadLine(), out int selection))
                {
                    if (selection <= 4 && selection >= 0)
                        return (ActionType)selection;
                }

                firstIteration = false;
            } while (true);
        }

        private static void ShowMenu()
        {
            Console.WriteLine(@$"
Enter a number to choose one of the options below:
1 - Add a new record
2 - Delete a record
3 - Show all previous records
4 - Update a record
0 - Exit the app");
            Console.WriteLine("---------------------------------------------");
        }
    }
}
