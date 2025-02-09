namespace HKHemanthsharma.HabitLogger
{
    using System.Data.SqlClient;
    using System.Data;
    using System.Data.Common;
    using System.ComponentModel.Design;



    public class Program
    {

        public static DatabaseManager dbmanager = new DatabaseManager();
        public static HabitRepository hr = new HabitRepository(dbmanager, "habitDetails");

        public static void Main(string[] args)
        {


            dbmanager.DBExists("habits");

            bool close = false;
            while (!close)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU:  ");
                Console.WriteLine("\nType 0 to close the application: ");
                Console.WriteLine("Type 1 to View All habits: ");
                Console.WriteLine("Type 2 to Insert a habit");
                Console.WriteLine("Type 3 to Delete a habbit");
                Console.WriteLine("Type 4 to Update a habit");
                switch (Console.ReadLine())
                {
                    case "0":
                        close = true; break;
                    case "1":
                        hr.FetchAllRecords(); break;
                    case "2":
                        hr.InsertRecord(); break;
                    case "3":
                        hr.DeleteRecord(); break;
                    case "4":
                        hr.UpdateRecord(); break;
                    default:
                        Console.WriteLine("Please enter a valid input! try again");
                        Console.WriteLine("Enter any key to try again");
                        Console.ReadLine();

                        break;
                }

            }


        }


    }
}