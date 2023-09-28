using System;
using DatabaseConnection;
namespace HabitTracker.Nik00NN
{
    internal class Program
    {
        static DataAccess da = new DataAccess();
        static void Main(string[] args)
        {
            da.CreateDatabase();
            GetUserInput();
        }
        static void GetUserInput()
        {
            bool isRunning = true;
            string choice;
            while (isRunning)
            {
                Console.WriteLine("--------Main Menu--------");
                Console.WriteLine("What would u want to do today ?");
                Console.WriteLine("Select from the options below:");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");

                choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        isRunning = false;
                        break;
                    case "1":
                        Console.Clear();
                        da.ViewAll();
                        break;
                    case "2":
                        Console.Clear();
                        da.Insert();
                        break;
                    case "3":
                        Console.Clear();
                        da.Delete();
                        break;
                    case "4":
                        Console.Clear();
                        da.Update();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("invalid input");
                        break;
                }
            }
        }
    }
}
