using MoveTracker.Data;
using MoveTracker.Models;

namespace MoveTracker
{
    internal class MoveTrackerActions
    {
        public static void DecisionHandler(MoveRepository repository, int userChoiceNum)
        {
            switch (userChoiceNum)
            {
                case 0:
                    App.QuitApp();
                    break;
                case 1:
                    TrackNewMove(repository);
                    break;
                case 2:
                    DisplayListOfMoves(repository);
                    break;
                case 3:
                    UpdateListOfMoves(repository);
                    break;
                case 4:
                    DeleteMoveById(repository);
                    break;
            }
        }

        internal static void TrackNewMove(MoveRepository repository, string err = "")
        {
            Console.Clear();

            if (err != "") Console.WriteLine(err.ToUpper(), Console.BackgroundColor = ConsoleColor.Red, Console.ForegroundColor = ConsoleColor.Black);
            Console.ResetColor();
            Console.WriteLine("");

            Console.WriteLine("How much have you moved today? ");
            bool userTrack = int.TryParse(Console.ReadLine(), out int movesNum);

            if (userTrack)
            {
                repository.AddMove(movesNum);
                Console.WriteLine("Number of Moves Tracked", Console.BackgroundColor = ConsoleColor.Green, Console.ForegroundColor = ConsoleColor.Black);
                Console.ResetColor();

                Console.WriteLine("");
                Console.Write("Press any key to return to the main menu ");
                Console.ReadLine();
                App.DisplayMenu(repository, 51);
            }
            else
            {
                TrackNewMove(repository, "moves not tracked as you need to enter a valid number");
            }
        }

        internal static void DisplayListOfMoves(MoveRepository repository)
        {
            Console.Clear();
            Console.WriteLine("All Tracked Moves:".ToUpper(), Console.BackgroundColor = ConsoleColor.Green, Console.ForegroundColor = ConsoleColor.Black);
            Console.ResetColor(); 
            Console.WriteLine("");

            foreach (Move move in repository.GetAllMove())
            {
                Console.WriteLine($"ID: {move.Id}");
                Console.WriteLine($"Moves: {move.NumOfMoves}");
                Console.WriteLine($"Date Recorded: {move.TimeRecorded}");
                Console.WriteLine("");
            }

            Console.WriteLine("");
            Console.Write("Press any key to return to the main menu ");
            Console.ReadLine();
            App.DisplayMenu(repository, 51);
        }

        internal static void UpdateListOfMoves(MoveRepository repository, string err = "")
        {
            Console.Clear();

            if (err != "") Console.WriteLine(err.ToUpper(), Console.BackgroundColor = ConsoleColor.Red, Console.ForegroundColor = ConsoleColor.Black);
            Console.WriteLine("");
            Console.ResetColor();

            Console.WriteLine("What move do you want to update? ", Console.BackgroundColor = ConsoleColor.Green, Console.ForegroundColor = ConsoleColor.Black);
            Console.ResetColor();
            bool moveUpdate = int.TryParse(Console.ReadLine(), out int moveToUpdate);

            if (moveUpdate)
            {
                Console.WriteLine("What is the new value? ", Console.BackgroundColor = ConsoleColor.Green, Console.ForegroundColor = ConsoleColor.Black);
                Console.ResetColor();
                bool moveId = int.TryParse(Console.ReadLine(), out int newMoveValue);

                if (moveId)
                {
                    repository.UpdateMove(moveToUpdate, newMoveValue);
                    Console.WriteLine("Move Updated", Console.BackgroundColor = ConsoleColor.Green, Console.ForegroundColor = ConsoleColor.Black);
                    Console.ResetColor();

                    Console.WriteLine("");
                    Console.Write("Press any key to return to the main menu ");
                    Console.ReadLine();
                    App.DisplayMenu(repository, 51);
                }
                else
                {
                    UpdateListOfMoves(repository, "move not updated as you need to enter a valid id");
                }
            }
            else
            {
                UpdateListOfMoves(repository, "move not updated as you need to enter a valid id");
            }
        }

        internal static void DeleteMoveById(MoveRepository repository, string err = "")
        {
            Console.Clear();

            if (err != "") Console.WriteLine(err.ToUpper(), Console.BackgroundColor = ConsoleColor.Red, Console.ForegroundColor = ConsoleColor.Black);
            Console.WriteLine("");
            Console.ResetColor();

            Console.WriteLine("What move do you want to delete? ", Console.BackgroundColor = ConsoleColor.Green, Console.ForegroundColor = ConsoleColor.Black);
            Console.ResetColor();
            bool userDelete = int.TryParse(Console.ReadLine(), out int movesDeleteNum);

            if (userDelete)
            {
                repository.DeleteMove(movesDeleteNum);
                Console.WriteLine("Chosen Move Deleted", Console.BackgroundColor = ConsoleColor.Green, Console.ForegroundColor = ConsoleColor.Black);
                Console.ResetColor();

                Console.WriteLine("");
                Console.Write("Press any key to return to the main menu ");
                Console.ReadLine();
                App.DisplayMenu(repository, 51);
            }
            else
            {
                DeleteMoveById(repository, "move not deleted as you need to enter a valid id");
            }

        }
    }
}
