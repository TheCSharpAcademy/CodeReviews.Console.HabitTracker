using Microsoft.Data.Sqlite;

namespace Habit_Logger
{
    class Program
    {
        static DatabaseManagement database = new();
        static void Main(string[] args)
        {
            MainMenu();
        }
        static void MainMenu()
        {
            Console.Clear();
            bool endApp = false;

            while (!endApp)
            {
                Console.Clear();
                Console.WriteLine("\n-------------------------------\n");
                Console.WriteLine("\tHABIT LOGGER 1.0");
                Console.WriteLine("\n-------------------------------\n");
                Console.WriteLine($"CURRENT RECORD: {database.Name.Replace("_", " ").ToUpper()} RECORD!");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Records.");
                Console.WriteLine("Type 3 to Delete Records.");
                Console.WriteLine("Type 4 to Update Records.");
                Console.WriteLine("Type 5 to see your record REPORT. ");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Type 6 to CREATE YOUR HABIT LOGGER !");
                Console.WriteLine("-------------------------------\n");

                Console.Write("Your option: ");


                string? commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        Thread.Sleep(1500);
                        endApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        database.GetAllRecords(database.Name);
                        break;
                    case "2":
                        database.Insert(database.Name);
                        break;
                    case "3":
                        database.Delete(database.Name);
                        break;
                    case "4":
                        database.Update(database.Name);
                        break;
                    case "5":
                        database.Report(database.Name);
                        break;
                    case "6":


                        Console.Clear();
                        Console.Write("Name of habit you want to track: ");

                        string? newHabit = Console.ReadLine();

                        while (newHabit == null)
                        {
                            Console.Write("Invalid input please try again: ");
                            newHabit = Console.ReadLine();
                        }
                        newHabit = newHabit.Replace(" ", "_");

                        Console.WriteLine("Habit can't be tracked by time.");
                        Console.WriteLine("Only in quantity (Distance, Quantity, Calories, Laps, e.g.)");
                        Console.Write("Name of the the value you want to track: ");

                        string? newUserValue = Console.ReadLine();

                        while (newUserValue == null)
                        {
                            Console.Write("Invalid input please try again: ");
                            newUserValue = Console.ReadLine();
                        }

                        newUserValue = newUserValue.Replace(" ", "_");


                        database.Name = database.CreateNewRecord(newHabit, newUserValue);

                        MainMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
                        MainMenu();
                        break;
                }
            }
        }
    }
}