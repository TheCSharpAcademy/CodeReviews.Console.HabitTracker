// See https://aka.ms/new-console-template for more information
using HabitTrackingDatabase;


namespace HabitTracker
{
    class Program
    {
        public static void Main(string[] args)
        {
            Program program = new Program();
            program.Menu();
        }

        void Menu()
        {
            string? choice ;
            try
            {
                HabitDatabase db = new HabitDatabase();
                db.InitializeDatabse();
                do
                {

                    Console.WriteLine("MAIN MENU");
                    Console.WriteLine("\nWhat would you like to do\n");
                    Console.WriteLine("Type 0 to Close Application");
                    Console.WriteLine("Type 1 to View All Records");
                    Console.WriteLine("Type 2 to Insert Record");
                    Console.WriteLine("Type 3 to Delete Record");
                    Console.WriteLine("Type 4 to Update Record");
                    Console.WriteLine("Type 5 to Generate Report");
                    Console.WriteLine("----------------------------\n");
                    Console.Write("Enter choice: ");
                    choice = Console.ReadLine().Trim();
                    switch (choice)
                    {
                        case "0":
                            Console.WriteLine("Exiting........");
                            break;
                        case "1":
                            db.ViewHabits();
                            break;
                        case "2":
                            db.InsertHabits();
                            break;
                        case "3":
                            db.DeleteHabits();
                            break;
                        case "4":
                            db.UpdateHabits();
                            break;
                        case "5":
                            db.ReportHabit();
                            break;
                        default:
                            Console.WriteLine("Invalid choice\n\n");
                            break;
                    }
                } while (choice != "0");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps error occured: " + ex);
            }
        }
    }
}